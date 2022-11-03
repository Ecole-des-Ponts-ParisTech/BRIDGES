using System;
using System.Collections.Generic;

using Alg_Fund = BRIDGES.Algebra.Fundamentals;
using Alg_Set = BRIDGES.Algebra.Sets;

using BRIDGES.LinearAlgebra.Vectors;


namespace BRIDGES.LinearAlgebra.Matrices.Sparse
{
    /// <summary>
    /// Class defining a sparse matrix with a compressed column storage.
    /// </summary>
    public sealed class CompressedColumn : SparseMatrix,
          Alg_Set.Additive.IAbelianGroup<CompressedColumn>, Alg_Set.Multiplicative.ISemiGroup<CompressedColumn>, Alg_Set.IGroupAction<double, CompressedColumn>
    {
        #region Fields

        /// <summary>
        /// Sparse matrix from CSparse library, in the form of a compressed column storage.
        /// </summary>
        private CSparse.Storage.CompressedColumnStorage<double> _storedMatrix;

        /* Old fields 


        /// <summary>
        /// Number of row of the current <see cref="CompressedColumn"/>
        /// </summary>
        private int _rowCount;

        /// <summary>
        /// Number of column of the current <see cref="CompressedColumn"/>
        /// </summary>
        private int _columnCount;


        /// <summary>
        /// Pointers giving the number of non-zero values before the row at a given index.
        /// </summary>
        /// <remarks> Array of length (ColumnCount + 1), starting at 0 and ending at <see cref="SparseMatrix.NonZerosCount"/>. </remarks>
        private int[] _columnPointers;

        /// <summary>
        /// Row indices associated with the non-zero values.
        /// </summary>
        private List<int> _rowIndices;

        /// <summary>
        /// Non-zero values of the matrix.
        /// </summary>
        private List<double> _values;

        */

        #endregion

        #region Properties

        /// <inheritdoc/>
        public override int RowCount { get { return _storedMatrix.RowCount; } }

        /// <inheritdoc/>
        public override int ColumnCount { get { return _storedMatrix.ColumnCount; } }


        /// <inheritdoc/>
        public override int NonZerosCount 
        {
            get { return _storedMatrix.NonZerosCount; }
        }

        /// <inheritdoc/>
        public override double this[int row, int column]
        {
            get
            {
                for (int i_NZ = _storedMatrix.ColumnPointers[column]; i_NZ < _storedMatrix.ColumnPointers[column + 1]; i_NZ++)
                {
                    if (_storedMatrix.RowIndices[i_NZ] == row) { return _storedMatrix.Values[i_NZ]; }
                    else if(row < _storedMatrix.RowIndices[i_NZ]) { break; }
                }
                return 0.0;
            }
        }

        #endregion

        #region Contructors

        /// <summary>
        /// Initialises a new instance of the <see cref="CompressedColumn"/> class by defining its size,
        /// and by giving its values in a <see cref="Storage.DictionaryOfKeys"/>.
        /// </summary>
        /// <param name="rowCount"> Number of rows of the <see cref="CompressedColumn"/>. </param>
        /// <param name="columnCount"> Number of columns of the <see cref="CompressedColumn"/>. </param>
        /// <param name="dok"> Values of the <see cref="CompressedColumn"/>. </param>
        public CompressedColumn(int rowCount, int columnCount, Storage.DictionaryOfKeys dok)
        {
            List<(int, double)>[] columns = new List<(int, double)>[columnCount];
            for (int i_C = 0; i_C < columnCount; i_C++) { columns[i_C] = new List<(int, double)>(); }
            
            // Distribute among the columns
            IEnumerator<KeyValuePair<(int, int), double>> kvpEnumerator = dok.GetNonZeros();
            try 
            {
                while (kvpEnumerator.MoveNext())
                {
                    KeyValuePair<(int, int), double> kvp = kvpEnumerator.Current;

                    columns[kvp.Key.Item2].Add((kvp.Key.Item1, kvp.Value));
                }
            }
            finally { kvpEnumerator.Dispose(); }

            // Sorts each columns with regard to the row index
            for (int i_C = 0; i_C < columnCount; i_C++)
            {
                columns[i_C].Sort((x, y) => x.Item1.CompareTo(y.Item1));
            }

            // Creates the new row index and value lists
            int[] columnPointers = new int[columnCount + 1];
            int[] rowIndices = new int[dok.Count];
            double[] values = new double[dok.Count];

            int counter = 0;

            columnPointers[0] = 0;
            for (int i_C = 0; i_C < columnCount; i_C++)
            {
                List<(int, double)> column = columns[i_C];
                int count = column.Count;

                columnPointers[i_C + 1] = columnPointers[i_C] + columns[i_C].Count;

                for (int i_NZ = 0; i_NZ < count; i_NZ++)
                {
                    rowIndices[counter] = column[i_NZ].Item1;
                    values[counter] = column[i_NZ].Item2;
                    counter++;
                }
            }

            _storedMatrix = new CSparse.Double.SparseMatrix(rowCount, columnCount, values, rowIndices, columnPointers);
        }


        /// <summary>
        /// Initialises a new instance of the <see cref="CompressedColumn"/> class by defining its number of row and column.
        /// </summary>
        /// <param name="rowCount"> Number of rows of the <see cref="CompressedColumn"/>. </param>
        /// <param name="columnCount"> Number of columns of the <see cref="CompressedColumn"/>. </param>
        /// <param name="values"> Non-zero values of the <see cref="CompressedColumn"/>. </param>
        /// <param name="rowIndices"> Row indices of the <see cref="CompressedColumn"/>.</param>
        /// <param name="columnPointers"> Column pointers of the <see cref="CompressedColumn"/>. </param>
        internal CompressedColumn(int rowCount, int columnCount, int[] columnPointers, int[] rowIndices, double[] values)
        {
            // Verifications
            if (columnPointers.Length != columnCount + 1)
            {
                throw new ArgumentException("The number of column pointers is not consistent with the number of columns.", nameof(columnPointers));
            }
            if (values.Length != rowIndices.Length)
            {
                throw new ArgumentException($"The number of elements in {nameof(values)} and {nameof(rowIndices)} do not match.");
            }
            if (columnPointers[columnPointers.Length - 1] != values.Length)
            {
                throw new ArgumentException($"The last value of {nameof(columnPointers)} is not equal to the number of non-zero values.");
            }


            _storedMatrix = new CSparse.Double.SparseMatrix(rowCount, columnCount, values, rowIndices, columnPointers);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="CompressedColumn"/> class from a CSparse compressed column storage.
        /// </summary>
        /// <param name="ccs"> CSparse compressed column storage. </param>
        private CompressedColumn(CSparse.Storage.CompressedColumnStorage<double> ccs)
        {
            _storedMatrix = ccs;
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Returns the neutral <see cref="CompressedColumn"/> for the addition. 
        /// </summary>
        /// <param name="rowCount"> Number of rows of the <see cref="CompressedColumn"/>. </param>
        /// <param name="columnCount"> Number of columns of the <see cref="CompressedColumn"/>. </param>
        /// <returns> The <see cref="CompressedColumn"/> of the given size, with zeros on every coordinates. </returns>
        public static new CompressedColumn Zero(int rowCount, int columnCount)
        {
            CSparse.Storage.CompressedColumnStorage<double> ccs = new CSparse.Double.SparseMatrix(rowCount, columnCount, 0);

            return new CompressedColumn(ccs);
        }

        /// <summary>
        /// Returns the neutral <see cref="CompressedColumn"/> for the multiplication. 
        /// </summary>
        /// <param name="size"> Number of rows and columns of the <see cref="CompressedColumn"/>. </param>
        /// <returns> The <see cref="CompressedColumn"/> of the given size, with ones on the diagonal and zeros elsewhere. </returns>
        public static new CompressedColumn Identity(int size)
        {
            return new CompressedColumn(CSparse.Double.SparseMatrix.CreateIdentity(size));
        }

        #endregion

        #region Static Methods

        /******************** Algebraic Near Ring ********************/

        /// <summary>
        /// Computes the addition of two <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedColumn"/> for the addition. </param>
        /// <param name="right"> Right <see cref="CompressedColumn"/> for the addition. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the addition. </returns>
        public static CompressedColumn Add(CompressedColumn left, CompressedColumn right)
        {
            CSparse.Storage.CompressedColumnStorage<double> ccs = left._storedMatrix.Add(right._storedMatrix);

            return new CompressedColumn(ccs);
        }

        /// <summary>
        /// Computes the subtraction of two <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedColumn"/> to subtract. </param>
        /// <param name="right"> Right <see cref="CompressedColumn"/> to subtract with. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the subtraction. </returns>
        public static CompressedColumn Subtract(CompressedColumn left, CompressedColumn right)
        {
            CSparse.Storage.CompressedColumnStorage<double> ccs = new CSparse.Double.SparseMatrix(left.RowCount, left.ColumnCount, left.NonZerosCount + right.RowCount);

            left._storedMatrix.Add(1.0, -1.0, right._storedMatrix, ccs);
            return new CompressedColumn(ccs);
        }


        /// <summary>
        /// Computes the multiplication of two <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedColumn"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="CompressedColumn"/> for the multiplication. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the multiplication. </returns>
        /// <exception cref="ArgumentException"> The matrices size does not allow their multiplication. </exception>
        public static CompressedColumn Multiply(CompressedColumn left, CompressedColumn right)
        {
            // Verifications
            if (left.ColumnCount != right.RowCount)
            {
                throw new ArgumentException("The matrices size does not allow their multiplication.");
            }

            CSparse.Storage.CompressedColumnStorage<double> ccs = left._storedMatrix.Multiply(right._storedMatrix);
            return new CompressedColumn(ccs);
        }

        /// <summary>
        /// Computes the left multiplication of a <see cref="CompressedColumn"/> with its transposition : <c>At*A</c>.
        /// </summary>
        /// <param name="matrix">transposed <see cref="CompressedColumn"/> for the multiplication. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the multiplication. </returns>
        public static CompressedColumn TransposeMultiplySelf(CompressedColumn matrix)
        {
            CSparse.Storage.CompressedColumnStorage<double> transposed = matrix._storedMatrix.Transpose();

            CSparse.Storage.CompressedColumnStorage<double> ccs = transposed.Multiply(matrix._storedMatrix);

            return new CompressedColumn(ccs);
        }


        /******************** Sparse Matrix Embedding ********************/

        /// <summary>
        /// Computes the addition of a <see cref="CompressedColumn"/> with a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedColumn"/> for the addition. </param>
        /// <param name="right"> Right <see cref="SparseMatrix"/> for the addition. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the addition. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The addition of a <see cref="CompressedColumn"/> with the right matrix as a <see cref="SparseMatrix"/> is not implemented.
        /// </exception>
        public static CompressedColumn Add(CompressedColumn left, SparseMatrix right)
        {
            if (right is CompressedColumn ccsRight) { return CompressedColumn.Add(left, ccsRight); }
            else if (right is CompressedRow crsRight) { return CompressedColumn.Add(left, crsRight); }
            else { throw new NotImplementedException($"The addition of a {left.GetType()} and a {right.GetType()} as a {nameof(SparseMatrix)} is not implemented."); }
        }

        /// <summary>
        /// Computes the addition of a <see cref="SparseMatrix"/> with a <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix"/> for the addition. </param>
        /// <param name="right"> Right <see cref="CompressedColumn"/> for the addition. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the addition. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The addition of the left matrix as a <see cref="SparseMatrix"/> with a <see cref="CompressedColumn"/> is not implemented.
        /// </exception>
        public static CompressedColumn Add(SparseMatrix left, CompressedColumn right)
        {
            if (left is CompressedColumn ccsLeft) { return CompressedColumn.Add(ccsLeft, right); }
            else if (left is CompressedRow crsLeft) { return CompressedColumn.Add(crsLeft, right); }
            else { throw new NotImplementedException($"The addition of a {left.GetType()} as a {nameof(SparseMatrix)} and a {right.GetType()} is not implemented."); }
        }


        /// <summary>
        /// Computes the subtraction of a <see cref="CompressedColumn"/> with a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedColumn"/> to subtract. </param>
        /// <param name="right"> Right <see cref="SparseMatrix"/> to subtract with. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the subtraction. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The subtraction of a <see cref="CompressedColumn"/> with the right matrix as a <see cref="SparseMatrix"/> is not implemented.
        /// </exception>
        public static CompressedColumn Subtract(CompressedColumn left, SparseMatrix right)
        {
            if (right is CompressedColumn ccsRight) { return CompressedColumn.Subtract(left, ccsRight); }
            else if (right is CompressedRow crsRight) { return CompressedColumn.Subtract(left, crsRight); }
            else { throw new NotImplementedException($"The addition of a {left.GetType()} and a {right.GetType()} as a {nameof(SparseMatrix)} is not implemented."); }
        }

        /// <summary>
        /// Computes the subtraction of a <see cref="SparseMatrix"/> with a <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix"/> to subtract. </param>
        /// <param name="right"> Right <see cref="CompressedColumn"/> to subtract with. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the subtraction. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The subtraction of the left matrix as a <see cref="SparseMatrix"/> with a <see cref="CompressedColumn"/> is not implemented.
        /// </exception>
        public static CompressedColumn Subtract(SparseMatrix left, CompressedColumn right)
        {
            if (left is CompressedColumn ccsLeft) { return CompressedColumn.Subtract(ccsLeft, right); }
            else if (left is CompressedRow crsLeft) { return CompressedColumn.Subtract(crsLeft, right); }
            else { throw new NotImplementedException($"The subtraction of a {left.GetType()} as a {nameof(SparseMatrix)} and a {right.GetType()} is not implemented."); }
        }


        /// <summary>
        /// Computes the multiplication of a <see cref="CompressedColumn"/> with a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedColumn"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="SparseMatrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The multiplication of a <see cref="CompressedColumn"/> with the right matrix as a <see cref="SparseMatrix"/> is not implemented.
        /// </exception>
        public static CompressedColumn Multiply(CompressedColumn left, SparseMatrix right)
        {
            if (right is CompressedColumn ccsRight) { return CompressedColumn.Multiply(left, ccsRight); }
            else if (right is CompressedRow crsRight) { return CompressedColumn.Multiply(left, crsRight); }
            else { throw new NotImplementedException($"The multiplication of a {left.GetType()} and a {right.GetType()} as a {nameof(SparseMatrix)} is not implemented."); }
        }

        /// <summary>
        /// Computes the multiplication of a <see cref="SparseMatrix"/> with a <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="CompressedColumn"/> for the multiplication. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The multiplication of the left matrix as a <see cref="SparseMatrix"/> with a <see cref="CompressedColumn"/> is not implemented.
        /// </exception>
        public static CompressedColumn Multiply(SparseMatrix left, CompressedColumn right)
        {
            if (left is CompressedColumn ccsLeft) { return CompressedColumn.Multiply(ccsLeft, right); }
            else if (left is CompressedRow crsLeft) { return CompressedColumn.Multiply(crsLeft, right); }
            else { throw new NotImplementedException($"The multiplication of a {left.GetType()} as a {nameof(SparseMatrix)} and a {right.GetType()} is not implemented."); }
        }


        /******************** CompressedRow Embedding ********************/

        /// <summary>
        /// Computes the addition of a <see cref="CompressedColumn"/> with a <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedColumn"/> for the addition. </param>
        /// <param name="right"> Right <see cref="CompressedRow"/> for the addition. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the addition. </returns>
        public static CompressedColumn Add(CompressedColumn left, CompressedRow right)
        {
            CompressedColumn ccs = right.ToCompressedColumn();

            return CompressedColumn.Add(left, ccs);
        }

        /// <summary>
        /// Computes the addition of a <see cref="CompressedRow"/> with a <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedRow"/> for the addition. </param>
        /// <param name="right"> Right <see cref="CompressedColumn"/> for the addition. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the addition. </returns>
        public static CompressedColumn Add(CompressedRow left, CompressedColumn right)
        {
            CompressedColumn ccs = left.ToCompressedColumn();

            return CompressedColumn.Add(ccs, right);
        }


        /// <summary>
        /// Computes the subtraction of a <see cref="CompressedColumn"/> with a <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedColumn"/> to subtract. </param>
        /// <param name="right"> Right <see cref="CompressedRow"/> to subtract with. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the subtraction. </returns>
        public static CompressedColumn Subtract(CompressedColumn left, CompressedRow right)
        {
            CompressedColumn ccs = right.ToCompressedColumn();

            return CompressedColumn.Subtract(left, ccs);
        }

        /// <summary>
        /// Computes the subtraction of a <see cref="CompressedRow"/> with a <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedRow"/> to subtract. </param>
        /// <param name="right"> Right <see cref="CompressedColumn"/> to subtract with. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the subtraction. </returns>
        public static CompressedColumn Subtract(CompressedRow left, CompressedColumn right)
        {
            CompressedColumn ccs = left.ToCompressedColumn();

            return CompressedColumn.Subtract(ccs, right);
        }


        /// <summary>
        /// Computes the multiplication of a <see cref="CompressedColumn"/> with a <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedColumn"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="CompressedRow"/> for the multiplication. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the multiplication. </returns>
        public static CompressedColumn Multiply(CompressedColumn left, CompressedRow right)
        {
            CompressedColumn ccs = right.ToCompressedColumn();

            return CompressedColumn.Multiply(left, ccs);
        }

        /// <summary>
        /// Computes the multiplication of a <see cref="CompressedRow"/> with a <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedRow"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="CompressedColumn"/> for the multiplication. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the multiplication. </returns>
        public static CompressedColumn Multiply(CompressedRow left, CompressedColumn right)
        {
            CompressedColumn ccs = left.ToCompressedColumn();

            return CompressedColumn.Multiply(ccs, right);
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Computes the scalar multiplication of a <see cref="CompressedColumn"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="factor"> <see cref="double"/>-precision real number. </param>
        /// <param name="operand"> <see cref="CompressedColumn"/> to multiply. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the scalar multiplication. </returns>
        public static CompressedColumn Multiply(double factor, CompressedColumn operand)
        {
            if (factor == 0.0) { return CompressedColumn.Zero(operand.RowCount, operand.ColumnCount); }

            int[] columnPointers = operand._storedMatrix.ColumnPointers.Clone() as int[];
            int[] rowIndices = operand._storedMatrix.RowIndices.Clone() as int[];

            double[] values = new double[operand.NonZerosCount];
            for (int i_NZ = 0; i_NZ < values.Length; i_NZ++)
            {
                values[i_NZ] = factor * operand._storedMatrix.Values[i_NZ];
            }

            return new CompressedColumn(operand.RowCount, operand.ColumnCount, columnPointers, rowIndices, values);
        }

        /// <summary>
        /// Computes the scalar multiplication of a <see cref="CompressedColumn"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="factor"> <see cref="double"/>-precision real number. </param>
        /// <param name="operand"> <see cref="CompressedColumn"/> to multiply. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the scalar multiplication. </returns>
        public static CompressedColumn Multiply(CompressedColumn operand, double factor)
        {
            return CompressedColumn.Multiply(factor, operand);
        }


        /// <summary>
        /// Computes the scalar division of a <see cref="CompressedColumn"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="operand"> <see cref="CompressedColumn"/> to divide. </param>
        /// <param name="divisor"> <see cref="double"/>-precision real number to divide with. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the scalar division. </returns>
        /// <exception cref="DivideByZeroException"> The divisor can not be zero. </exception>
        public static CompressedColumn Divide(CompressedColumn operand, double divisor)
        {
            if (divisor == 0.0)
            {
                throw new DivideByZeroException("The divisor can not be zero.");
            }

            int[] columnPointers = operand._storedMatrix.ColumnPointers.Clone() as int[];
            int[] rowIndices = operand._storedMatrix.RowIndices.Clone() as int[];

            double[] values = new double[operand.NonZerosCount];
            for (int i_NZ = 0; i_NZ < values.Length; i_NZ++)
            {
                values[i_NZ] = operand._storedMatrix.Values[i_NZ] / divisor;
            }

            return new CompressedColumn(operand.RowCount, operand.ColumnCount, columnPointers, rowIndices, values);
        }


        /******************** Other Operations ********************/

        /// <summary>
        /// Computes the right multiplication of a <see cref="CompressedColumn"/> with a <see cref="Vector"/> : <c>A*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="CompressedColumn"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The right multiplication of a <see cref="CompressedColumn"/> with a the vector as a <see cref="Vector"/> is not implemented.
        /// </exception>
        public static Vector Multiply(CompressedColumn matrix, Vector vector)
        {
            if (vector is DenseVector denseVector) { return CompressedColumn.Multiply(matrix, denseVector); }
            else if (vector is SparseVector sparseVector) { return CompressedColumn.Multiply(matrix, sparseVector); }
            else { throw new NotImplementedException($"The right multiplication of a {matrix.GetType()} and a {vector.GetType()} as a {nameof(Vector)} is not implemented."); }
        }

        /// <summary>
        /// Computes the right multiplication of a <see cref="CompressedColumn"/> with a <see cref="DenseVector"/> : <c>A*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="CompressedColumn"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="DenseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the multiplication. </returns>
        public static DenseVector Multiply(CompressedColumn matrix, DenseVector vector)
        {
            double[] components = new double[matrix.RowCount];

            matrix._storedMatrix.Multiply(vector._components, components);

            return new DenseVector(components);
        }

        /// <summary>
        /// Computes the right multiplication of a <see cref="CompressedColumn"/> with a <see cref="SparseVector"/> : <c>A*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="CompressedColumn"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="SparseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the multiplication. </returns>
        public static SparseVector Multiply(CompressedColumn matrix, SparseVector vector)
        {
            double[] components = new double[matrix.RowCount];

            matrix._storedMatrix.Multiply(vector.ToArray(), components);

            List<int> rowIndices = new List<int>();
            List<double> values = new List<double>();

            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] != 0.0) { rowIndices.Add(i); values.Add(components[i]); }
            }

            return new SparseVector(components.Length, rowIndices, values);
        }


        /// <summary>
        /// Computes the right multiplication of a transposed <see cref="CompressedColumn"/> with a <see cref="Vector"/> : <c>At*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="CompressedColumn"/> to transpose then multiply on the right. </param>
        /// <param name="vector"> <see cref="Vector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The right multiplication of a transposed <see cref="CompressedColumn"/> with a the vector as a <see cref="Vector"/> is not implemented.
        /// </exception>
        public static Vector TransposeMultiply(CompressedColumn matrix, Vector vector)
        {
            if (vector is DenseVector denseVector) { return CompressedColumn.TransposeMultiply(matrix, denseVector); }
            else if (vector is SparseVector sparseVector) { return CompressedColumn.TransposeMultiply(matrix, sparseVector); }
            else { throw new NotImplementedException($"The multiplication of a transposed {matrix.GetType()} and a {vector.GetType()} as a {nameof(Vector)} is not implemented."); }
        }

        /// <summary>
        /// Computes the right multiplication of a transposed <see cref="CompressedColumn"/> with a <see cref="DenseVector"/> : <c>At*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="CompressedColumn"/> to transpose then multiply on the right. </param>
        /// <param name="vector"> <see cref="DenseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the multiplication. </returns>
        public static DenseVector TransposeMultiply(CompressedColumn matrix, DenseVector vector)
        {
            double[] components = new double[matrix.ColumnCount];

            matrix._storedMatrix.TransposeMultiply(vector._components, components);

            return new DenseVector(components);
        }

        /// <summary>
        /// Computes the right multiplication of a transposed <see cref="CompressedColumn"/> with a <see cref="SparseVector"/> : <c>At*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="CompressedColumn"/> to transpose then multiply on the right. </param>
        /// <param name="vector"> <see cref="SparseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the multiplication. </returns>
        public static SparseVector TransposeMultiply(CompressedColumn matrix, SparseVector vector)
        {
            double[] components = new double[matrix.ColumnCount];

            matrix._storedMatrix.TransposeMultiply(vector.ToArray(), components);

            List<int> rowIndices = new List<int>();
            List<double> values = new List<double>();

            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] != 0.0) { rowIndices.Add(i); values.Add(components[i]); }
            }

            return new SparseVector(components.Length, rowIndices, values);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the non-zero value of the current <see cref="CompressedColumn"/> sparse matrix at a given index.
        /// </summary>
        /// <param name="index"> Index of the non-zero value to get.</param>
        /// <returns> The non-zero value of the current <see cref="CompressedColumn"/> sparse matrix at the given index. </returns>
        public double GetValue(int index)
        {
            return _storedMatrix.Values[index];
        }

        /// <summary>
        /// Returns the non-zero values of the current <see cref="CompressedColumn"/> sparse matrix.
        /// </summary>
        /// <returns> The non-zero values of the current <see cref="CompressedColumn"/> sparse matrix. </returns>
        public double[] Values()
        {
            return _storedMatrix.Values.Clone() as double[];
        }


        /// <summary>
        /// Returns the row index of the current <see cref="CompressedColumn"/> sparse matrix at a given index.
        /// </summary>
        /// <param name="index"> Index of the row index to get.</param>
        /// <returns> The row index of the current <see cref="CompressedColumn"/> sparse matrix at the given index. </returns>
        public int GetRowIndex(int index)
        {
            return _storedMatrix.RowIndices[index];
        }

        /// <summary>
        /// Returns the row indices of the current <see cref="CompressedColumn"/> sparse matrix.
        /// </summary>
        /// <returns> The row indices of the current <see cref="CompressedColumn"/> sparse matrix. </returns>
        public int[] RowIndices()
        {
            return _storedMatrix.RowIndices.Clone() as int[];
        }


        /// <summary>
        /// Returns the column pointers of the current <see cref="CompressedColumn"/> sparse matrix.
        /// </summary>
        /// <param name="index"> Index of the column pointers to get.</param>
        /// <returns> The column pointers of the current <see cref="CompressedColumn"/> sparse matrix. </returns>
        public int GetColumnPointers(int index)
        {
            return _storedMatrix.ColumnPointers[index];
        }

        /// <summary>
        /// Returns the column pointers of the current <see cref="CompressedColumn"/> sparse matrix.
        /// </summary>
        /// <returns> The column pointers of the current <see cref="CompressedColumn"/> sparse matrix. </returns>
        public int[] ColumnPointers()
        {
            return _storedMatrix.ColumnPointers.Clone() as int[];
        }


        /// <inheritdoc/>
        public override IEnumerable<(int RowIndex, int ColumnIndex, double Value)> GetNonZeros()
        {
            for (int i_C = 0; i_C < ColumnCount; i_C++)
            {
                for (int i_NZ = _storedMatrix.ColumnPointers[i_C]; i_NZ < _storedMatrix.ColumnPointers[i_C + 1]; i_NZ++)
                {
                    yield return (RowIndex: _storedMatrix.RowIndices[i_NZ], ColumnIndex:  i_C, Value: _storedMatrix.Values[i_NZ]);
                }
            }
        }


        /// <inheritdoc/>
        public override void Transpose()
        {
            _storedMatrix = _storedMatrix.Transpose();
        }


        /// <summary>
        /// Computes the kernel (or null-space) of the current <see cref="SparseMatrix"/> using the QR decomposition.
        /// </summary>
        /// <remarks> The method is adapted for rectangular matrix. </remarks>
        /// <returns> The vectors forming a basis of the null-space. </returns>
        public DenseVector[] Kernel()
        {
            // Verification
            if (NonZerosCount == 0) { throw new Exception("The kernel of a zero matrix cannot be computed."); }

            CompressedColumn squareCcs = CompleteToSquare();

            DenseVector[] kernel = ComputeKernel(ref squareCcs);

            if (RowCount != ColumnCount) { kernel = FilterKernelVectors(kernel); }

            return kernel;
        }

        /// <summary>
        /// Solve the system <code>Ax=y</code> using Cholesky decomposition.
        /// </summary>
        /// <param name="vector"> The vector y in the system. </param>
        /// <returns> The vector x in the system. </returns>
        public DenseVector SolveCholesky(DenseVector vector)
        {
            var cholesky = CSparse.Double.Factorization.SparseCholesky.Create(_storedMatrix, CSparse.ColumnOrdering.MinimumDegreeAtPlusA);

            double[] x = new double[ColumnCount];
            cholesky.Solve(vector._components, x);
            return new DenseVector(x);
        }


        /// <summary>
        /// Converts a the current <see cref="CompressedColumn"/> matrix into an equivalent <see cref="CompressedRow"/> matrix.
        /// </summary>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the conversion. </returns>
        public CompressedRow ToCompressedRow()
        {
            // Get the number of elements per row
            int[] rowHelper = new int[RowCount];
            for (int i_NZ = 0; i_NZ < NonZerosCount; i_NZ++)
            {
                rowHelper[_storedMatrix.RowIndices[i_NZ]]++;
            }


            // Creates the row pointer
            int[] rowPointers = new int[RowCount + 1];

            rowPointers[0] = 0;
            for (int i_R = 0; i_R < RowCount; i_R++)
            {
                rowPointers[i_R + 1] = rowPointers[i_R] + rowHelper[i_R];
                rowHelper[i_R] = 0;
            }


            // Creates the column index and value lists
            int[] columnIndices = new int[NonZerosCount];
            double[] values = new double[NonZerosCount];

            int i_ColumnNZ = _storedMatrix.ColumnPointers[0];
            for (int i_C = 0; i_C < ColumnCount; i_C++)
            {
                for (; i_ColumnNZ < _storedMatrix.ColumnPointers[i_C + 1]; i_ColumnNZ++)
                {
                    int i_R = _storedMatrix.RowIndices[i_ColumnNZ];
                    int i_Pointer = rowPointers[i_R] + rowHelper[i_R];

                    columnIndices[i_Pointer] = i_C;
                    values[i_Pointer] = _storedMatrix.Values[i_ColumnNZ];
                    rowHelper[i_R]++;
                }
            }


            List<int> list_ColumnsIndices = new List<int>(columnIndices);
            List<double> list_Values = new List<double>(values);

            return new CompressedRow(RowCount, ColumnCount, rowPointers, list_ColumnsIndices, list_Values);
        }

        #endregion

        #region Other Methods

        /// <summary>
        /// Completes the current <see cref="CompressedColumn"/> to be as square matrix by duplicating the non-zero column with the least component.
        /// </summary>
        /// <returns> The new suqare <see cref="CompressedColumn"/>. </returns>
        private CompressedColumn CompleteToSquare()
        {
            // If the matrix is rectangular with more columns
            if (RowCount < ColumnCount)
            {
                // Complete the current matrix to be a square matrix.
                // In order to keep the null space unchanged, the non-zero row with the least component is duplicated.
                CompressedRow crs = ToCompressedRow();

                int i_SparseRow = 0;
                int sparseRowCount = NonZerosCount;

                int[] rowPointers = new int[ColumnCount + 1];
                rowPointers[0] = 0;
                for (int i_R = 0; i_R < RowCount; i_R++)
                {
                    rowPointers[i_R + 1] = crs.GetRowPointer(i_R + 1);

                    int count = rowPointers[i_R + 1] - rowPointers[i_R];
                    if (count != 0 & count < sparseRowCount) { sparseRowCount = count; i_SparseRow = i_R; }
                }
                for (int i_R = RowCount; i_R < ColumnCount; i_R++)
                {
                    rowPointers[i_R + 1] = rowPointers[i_R] + sparseRowCount;
                }

                List<int> columnIndices = new List<int>(NonZerosCount + (sparseRowCount * (ColumnCount - RowCount)));
                List<double> values = new List<double>(NonZerosCount + (sparseRowCount * (ColumnCount - RowCount)));
                for (int i_R = 0; i_R < RowCount; i_R++)
                {
                    for (int i_NZ = rowPointers[i_R]; i_NZ < rowPointers[i_R + 1]; i_NZ++)
                    {
                        columnIndices.Add(crs.GetColumnIndex(i_NZ));
                        values.Add(crs.GetValue(i_NZ));
                    }
                }
                for (int i_R = 0; i_R < ColumnCount - RowCount; i_R++)
                {
                    for (int i_NZ = rowPointers[i_SparseRow]; i_NZ < rowPointers[i_SparseRow + 1]; i_NZ++)
                    {
                        columnIndices.Add(crs.GetColumnIndex(i_NZ));
                        values.Add(crs.GetValue(i_NZ));
                    }
                }

                CompressedRow result = new CompressedRow(ColumnCount, ColumnCount, rowPointers, columnIndices, values);
                return result.ToCompressedColumn();
            }
            // If the matrix is rectangular with more rows
            else if (ColumnCount < RowCount)
            {
                // Complete the current matrix to be a square matrix.
                // In order to keep the null space unchanged, the non-zero column with the least component is duplicated.
                int i_SparseColumn = 0;
                int sparseColumnCount = NonZerosCount;

                int[] columnPointers = new int[RowCount + 1];
                columnPointers[0] = 0;
                for (int i_C = 0; i_C < ColumnCount; i_C++)
                {
                    columnPointers[i_C + 1] = _storedMatrix.ColumnPointers[i_C + 1];

                    int count = columnPointers[i_C + 1] - columnPointers[i_C];
                    if (count != 0 & count < sparseColumnCount) { sparseColumnCount = count; i_SparseColumn = i_C; }
                }
                for (int i_C = ColumnCount; i_C < RowCount; i_C++)
                {
                    columnPointers[i_C + 1] = columnPointers[i_C] + sparseColumnCount;
                }

                List<int> rowIndices = new List<int>(NonZerosCount + (sparseColumnCount * (RowCount - ColumnCount)));
                List<double> values = new List<double>(NonZerosCount + (sparseColumnCount * (RowCount - ColumnCount)));
                for (int i_C = 0; i_C < ColumnCount; i_C++)
                {
                    for (int i_NZ = columnPointers[i_C]; i_NZ < columnPointers[i_C + 1]; i_NZ++)
                    {
                        rowIndices.Add(_storedMatrix.RowIndices[i_NZ]);
                        values.Add(_storedMatrix.Values[i_NZ]);
                    }
                }
                for (int i_C = 0; i_C < RowCount - ColumnCount; i_C++)
                {
                    for (int i_NZ = columnPointers[i_SparseColumn]; i_NZ < columnPointers[i_SparseColumn + 1]; i_NZ++)
                    {
                        rowIndices.Add(_storedMatrix.RowIndices[i_NZ]);
                        values.Add(_storedMatrix.Values[i_NZ]);
                    }
                }

                return new CompressedColumn(RowCount, RowCount, columnPointers, rowIndices.ToArray(), values.ToArray());
            }
            // If the matrix is a square matrix
            else
            {
                int[] columnPointers = new int[_storedMatrix.ColumnPointers.Length];
                for (int i = 0; i < _storedMatrix.ColumnPointers.Length; i++)
                {
                    columnPointers[i] = _storedMatrix.ColumnPointers[i];
                }
                List<int> rowIndices = new List<int>(NonZerosCount);
                for (int i = 0; i < NonZerosCount; i++)
                {
                    rowIndices.Add(_storedMatrix.RowIndices[i]);
                }
                List<double> values = new List<double>(NonZerosCount);
                for (int i = 0; i < NonZerosCount; i++)
                {
                    values.Add(_storedMatrix.Values[i]);
                }

                return new CompressedColumn(RowCount, ColumnCount, columnPointers, rowIndices.ToArray(), values.ToArray());
            }
        }

        /// <summary>
        /// Compute the kernel of a square matrix.
        /// </summary>
        /// <param name="ccs"> The square matrix to operate on.  </param>
        /// <returns> The vectors forming a basis of the null-space. If the kernel is reduced to { 0 }, an empty array is returned. </returns>
        private DenseVector[] ComputeKernel(ref CompressedColumn ccs)
        {
            double tolerance = 1e-8;

            ccs.Transpose();

            #region QR Factorization

            Factorisation.SparseQR qr = new Factorisation.SparseQR(ccs);

            CSparse.Double.SparseMatrix R = qr.GetR();
            CSparse.Double.SparseMatrix Q = qr.ComputeQ();

            int[] pInv = qr.GetInverseRowPermutation();

            #endregion

            // Dimension of the null-space of the matrix.
            int dimension = 0;
            for (int i_C = 0; i_C < R.ColumnCount; i_C++)
            {
                if(Math.Abs(R.At(i_C,i_C)) < tolerance) { dimension += 1; }
            }

            // (Inverse) Row Permutation
            int[] rowPermutation = new int[pInv.Length];
            for (int i = 0; i < pInv.Length; i++)
            {
                rowPermutation[pInv[i]] = i;
            }

            /***** Add non-zero vectors whose diagonal comonent is zero *****/

            DenseVector[] result = new DenseVector[dimension];
            if (dimension == 0) { return result; }

            // Check every single null vector in the left-hand
            int counter = 0;
            for (int i_C = 0; i_C < R.ColumnCount; i_C++)
            {
                // If the diagonal component is not null.
                if (Math.Abs(R.At(i_C, i_C)) > tolerance) { continue; }

                double[] column = Q.Column(i_C);
                double[] temp = new double[column.Length];
                bool isZero = true;

                // We perform the row permutation
                for (int j = 0; j < column.Length; j++)
                {
                    temp[rowPermutation[j]] = column[j];
                }

                // We resize the array to fit the number of columns
                Array.Resize(ref temp, ccs.ColumnCount); 

                for (int j = 0; j < ccs.ColumnCount; j++)
                {
                    isZero &= (temp[j] == 0);
                }
                if (!isZero)
                {
                    // We add the new vector if it's not zero
                    // The null vector can be applied to the fictitious rows...
                    result[counter] = new DenseVector(temp);
                    counter += 1;
                }
            }

            /***** Add non-zero vectors which ar not on the diagonal *****/

            for (int i_C = R.ColumnCount; i_C < Q.ColumnCount; i_C++)
            {
                var column = Q.Column(i_C);
                var temp = new double[column.Length];
                bool isZero = true;

                //We perform the permutation
                for (int j = 0; j < column.Length; j++)
                {
                    temp[rowPermutation[j]] = column[j];
                }

                System.Array.Resize(ref temp, ccs.ColumnCount);

                for (int j = 0; j < ccs.ColumnCount; j++)
                {
                    isZero &= (temp[j] == 0);
                }

                if (!isZero)
                {
                    //We add the new vector if it's not zero
                    //The null vector can be applied to the fictitious rows...
                    result[counter] = new DenseVector(temp);
                    counter += 1;
                }
            }

            return result;
        }

        /// <summary>
        /// Filters the kernel vectors to fit the initial rectangular matrix (and not the completed square matrix).
        /// </summary>
        /// <param name="kernel"> Kernel vectors of the completed square matrix. </param>
        /// <returns> The kernel vectors of the current matrix. </returns>
        private DenseVector[] FilterKernelVectors(DenseVector[] kernel)
        {
            // If the matrix is rectangular with more columns
            if (RowCount < ColumnCount)
            {
                // In this case, the initial rectangular matrix was completed with linearly-dependent row.
                // Fortunately, the kernel of the initial matrix and the completed matrix are the same.
                return kernel;
            }
            // If the matrix is rectangular with more rows
            else if (ColumnCount < RowCount)
            {
                // In this case, the initial rectangular matrix was completed with linearly-dependent columns.
                // Fortunately, we know that the vectors of the actual null-space should be orthogonal to

                // This array contains the vectors to which our null space should be orthogonal
                DenseVector[] orthogonalVectors = new DenseVector[RowCount - ColumnCount]; 
                for (int i = 0; i < (RowCount - ColumnCount); i++)
                {
                    orthogonalVectors[i] = new DenseVector(RowCount);
                    orthogonalVectors[i][ColumnCount + i] = 1;
                }

                Storage.DictionaryOfKeys dok = new Storage.DictionaryOfKeys();
                for (int i_R = 0; i_R < (RowCount - ColumnCount); i_R++)
                {
                    for (int i_C = 0; i_C < kernel.Length; i_C++)
                    {
                        double val = DenseVector.TransposeMultiply(kernel[i_C], orthogonalVectors[i_R]);
                        if (val != 0.0) { dok.Add(val, i_R, i_C); }
                    }
                }

                CompressedColumn ccs = new CompressedColumn((RowCount - ColumnCount), kernel.Length, dok);

                DenseVector[] intermediateresult = ccs.Kernel();

                // Create the solution
                DenseVector[] finalResults = new DenseVector[intermediateresult.Length];

                int counter = 0;
                foreach (DenseVector combination in intermediateresult)
                {
                    DenseVector finalResult = new DenseVector(RowCount);
                    for (int i = 0; i < kernel.Length; i++)
                    {
                        DenseVector vec = DenseVector.Multiply(combination[i], kernel[i]);
                        finalResult = DenseVector.Add(finalResult,vec);
                    }
                    //This part can be improved
                    var array = finalResult.ToArray();

                    Array.Resize(ref array, ColumnCount);

                    finalResults[counter] = (new DenseVector(array));
                    counter += 1;
                }

                //En fait finalResults contient les vecteurs orthogonaux à notre solution? on peut tenter un gram-schmidt
                return finalResults;
            }
            else { throw new InvalidOperationException("The filtering of the kernel vectors is not necessary for square matrices."); }
        }

        #endregion


        #region Override : Matrix

        /// <inheritdoc/>
        protected override void Opposite()
        {
            for (int i_NZ = 0; i_NZ < NonZerosCount; i_NZ++)
            {
                _storedMatrix.Values[i_NZ] = -_storedMatrix.Values[i_NZ];
            }
        }

        #endregion


        #region Explicit : Additive.IAbelianGroup<CompressedColumn>

        /******************** Properties ********************/

        /// <inheritdoc/>
        bool Alg_Fund.IAddable<CompressedColumn>.IsAssociative => true;

        /// <inheritdoc/>
        bool Alg_Fund.IAddable<CompressedColumn>.IsCommutative => true;


        /******************** Methods ********************/

        /// <inheritdoc/>
        CompressedColumn Alg_Fund.IAddable<CompressedColumn>.Add(CompressedColumn right) { return CompressedColumn.Add(this, right); }

        /// <inheritdoc/>
        CompressedColumn Alg_Fund.ISubtractable<CompressedColumn>.Subtract(CompressedColumn right) { return CompressedColumn.Subtract(this, right); }

        /// <inheritdoc/>
        bool Alg_Set.Additive.IGroup<CompressedColumn>.Opposite()
        {
            this.Opposite();
            return true;
        }

        /// <inheritdoc/>
        CompressedColumn Alg_Fund.IZeroable<CompressedColumn>.Zero() { return CompressedColumn.Zero(RowCount, ColumnCount); }

        #endregion

        #region Explicit : Multiplicative.SemiGroup<CompressedColumn>

        /******************** Properties ********************/

        /// <inheritdoc/>
        bool Alg_Fund.IMultiplicable<CompressedColumn>.IsAssociative => true;

        /// <inheritdoc/>
        bool Alg_Fund.IMultiplicable<CompressedColumn>.IsCommutative => false;


        /******************** Methods ********************/

        /// <inheritdoc/>
        CompressedColumn Alg_Fund.IMultiplicable<CompressedColumn>.Multiply(CompressedColumn right) { return CompressedColumn.Multiply(this, right); }

        #endregion

        #region Explicit : IGroupAction<Double,CompressedColumn>

        /******************** Methods ********************/

        /// <inheritdoc/>
        CompressedColumn Alg_Set.IGroupAction<double, CompressedColumn>.Multiply(double factor) { return CompressedColumn.Multiply(this, factor); }

        /// <inheritdoc/>
        CompressedColumn Alg_Set.IGroupAction<double, CompressedColumn>.Divide(double divisor) { return CompressedColumn.Divide(this, divisor); }

        #endregion
    }
}