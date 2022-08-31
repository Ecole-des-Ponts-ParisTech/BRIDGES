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
        /// <remarks> Array of length (ColumnCount + 1), starting at 0 and ending at <see cref="SparseMatrix.NonZeroCount"/>. </remarks>
        private int[] _columnPointers;

        /// <summary>
        /// Row indices associated with the non-zero values.
        /// </summary>
        private List<int> _rowIndices;

        /// <summary>
        /// Non-zero values of the matrix.
        /// </summary>
        private List<double> _values;

        #endregion

        #region Properties

        /// <inheritdoc/>
        public override int RowCount { get { return _rowCount; } }

        /// <inheritdoc/>
        public override int ColumnCount { get { return _columnCount; } }


        /// <inheritdoc/>
        public override int NonZeroCount 
        {
            get { return _values.Count; }
        }

        /// <inheritdoc/>
        public override double this[int row, int column]
        {
            get
            {
                for (int i_NZ = _columnPointers[column]; i_NZ < _columnPointers[column + 1]; i_NZ++)
                {
                    if (_rowIndices[i_NZ] == row) { return _values[i_NZ]; }
                    else if(row < _rowIndices[i_NZ]) { break; }
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
            _rowCount = rowCount;
            _columnCount = columnCount;


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
            _columnPointers = new int[columnCount + 1];
            _rowIndices = new List<int>(dok.Count);
            _values = new List<double>(dok.Count);

            _columnPointers[0] = 0;
            for (int i_C = 0; i_C < columnCount; i_C++)
            {
                List<(int, double)> column = columns[i_C];
                int count = column.Count;

                _columnPointers[i_C + 1] = _columnPointers[i_C] + columns[i_C].Count;

                for (int i_NZ = 0; i_NZ < count; i_NZ++)
                {
                    _rowIndices.Add(column[i_NZ].Item1);
                    _values.Add(column[i_NZ].Item2);
                }
            }
        }


        /// <summary>
        /// Initialises a new instance of the <see cref="CompressedColumn"/> class by defining its number of row and column.
        /// </summary>
        /// <param name="rowCount"> Number of rows of the <see cref="CompressedColumn"/>. </param>
        /// <param name="columnCount"> Number of columns of the <see cref="CompressedColumn"/>. </param>
        /// <param name="values"> Non-zero values of the <see cref="CompressedColumn"/>. </param>
        /// <param name="rowIndices"> Row indices of the <see cref="CompressedColumn"/>.</param>
        /// <param name="columnPointers"> Column pointers of the <see cref="CompressedColumn"/>. </param>
        public CompressedColumn(int rowCount, int columnCount, int[] columnPointers, List<int> rowIndices, List<double> values)
        {
            _rowCount = rowCount;
            _columnCount = columnCount;


            // Verifications
            if (columnPointers.Length != columnCount + 1)
            {
                throw new ArgumentException("The number of column pointers is not consistent with the number of columns.", nameof(columnPointers));
            }
            if (values.Count != rowIndices.Count)
            {
                throw new ArgumentException($"The number of elements in {nameof(values)} and {nameof(rowIndices)} do not match.");
            }
            if (columnPointers[columnPointers.Length - 1] != values.Count)
            {
                throw new ArgumentException($"The last value of {nameof(columnPointers)} is not equal to the number of non-zero values.");
            }

            _columnPointers = columnPointers;
            _rowIndices = rowIndices;
            _values = values;
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
            int[] columnPointer = new int[columnCount + 1];
            List<int> rowIndices = new List<int>();
            List<double> values = new List<double>();

            return new CompressedColumn(rowCount, columnCount, columnPointer, rowIndices, values);
        }

        /// <summary>
        /// Returns the neutral <see cref="CompressedColumn"/> for the multiplication. 
        /// </summary>
        /// <param name="size"> Number of rows and columns of the <see cref="CompressedColumn"/>. </param>
        /// <returns> The <see cref="CompressedColumn"/> of the given size, with ones on the diagonal and zeros elsewhere. </returns>
        public static new CompressedColumn Identity(int size)
        {
            int[] columnPointers = new int[size + 1];
            List<int> rowIndices = new List<int>(size);
            List<double> values = new List<double>(size);

            for (int i = 0; i < size; i++)
            {
                columnPointers[i] = i;
                rowIndices.Add(i);
                values.Add(1.0);
            }
            columnPointers[size] = size;

            return new CompressedColumn(size, size, columnPointers, rowIndices, values);
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
            // Verifications
            if (left.ColumnCount != right.ColumnCount || left.RowCount != right.RowCount)
            {
                throw new ArgumentException("The matrices do not have the same size.");
            }

            List<double> values = new List<double>(left.NonZeroCount + right.NonZeroCount);
            List<int> rowIndices = new List<int>(left.NonZeroCount + right.NonZeroCount);

            int[] columnPointers = new int[left.ColumnCount + 1];
            columnPointers[0] = 0;

            int i_RightNZ = 0;
            // Iterate on the columns of left
            for (int i_C = 0; i_C < left.ColumnCount; i_C++)
            {
                int i_RighColumnPointer = right._columnPointers[i_C + 1];

                // Iterate on the non-zero values of the current left column
                for (int i_LeftNZ = left._columnPointers[i_C]; i_LeftNZ < left._columnPointers[i_C + 1]; i_LeftNZ++)
                {
                    int i_LeftRow = left._rowIndices[i_LeftNZ];

                    // Add the non-zero values of the current right column which are before the current left non-zero value.
                    while (right._rowIndices[i_RightNZ] < i_LeftRow && i_RightNZ < i_RighColumnPointer)
                    {
                        values.Add(right._values[i_RightNZ]);
                        rowIndices.Add(right._rowIndices[i_RightNZ]);

                        i_RightNZ++;
                    }

                    // If the the non-zero values of the current left and right column are a the same row.
                    if (right._rowIndices[i_RightNZ] == i_LeftRow)
                    {
                        values.Add(left._values[i_LeftNZ] + right._values[i_RightNZ]);
                        rowIndices.Add(i_LeftRow);

                        i_RightNZ++;
                    }
                    else
                    {
                        values.Add(left._values[i_LeftNZ]);
                        rowIndices.Add(i_LeftRow);
                    }
                }

                // Add the remaining non-zero values of the current right column which are after these of the current left column.
                for (; i_RightNZ < i_RighColumnPointer; i_RightNZ++)
                {
                    values.Add(right._values[i_RightNZ]);
                    rowIndices.Add(right._rowIndices[i_RightNZ]);
                }

                columnPointers[i_C + 1] = values.Count;
            }

            return new CompressedColumn(left.RowCount, left.ColumnCount, columnPointers, rowIndices, values);
        }

        /// <summary>
        /// Computes the subtraction of two <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedColumn"/> to subtract. </param>
        /// <param name="right"> Right <see cref="CompressedColumn"/> to subtract with. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the subtraction. </returns>
        public static CompressedColumn Subtract(CompressedColumn left, CompressedColumn right)
        {
            // Verifications
            if (left.ColumnCount != right.ColumnCount || left.RowCount != right.RowCount)
            {
                throw new ArgumentException("The matrices do not have the same size.");
            }

            List<double> values = new List<double>(left.NonZeroCount + right.NonZeroCount);
            List<int> rowIndices = new List<int>(left.NonZeroCount + right.NonZeroCount);

            int[] columnPointers = new int[left.ColumnCount + 1];
            columnPointers[0] = 0;

            int i_RightNZ = 0;
            // Iterate on the columns of left
            for (int i_C = 0; i_C < left.ColumnCount; i_C++)
            {
                int i_RighColumnPointer = right._columnPointers[i_C + 1];

                // Iterate on the non-zero values of the current left column
                for (int i_LeftNZ = left._columnPointers[i_C]; i_LeftNZ < left._columnPointers[i_C + 1]; i_LeftNZ++)
                {
                    int i_LeftRow = left._rowIndices[i_LeftNZ];

                    // Add the non-zero values of the current right column which are before the current left non-zero value.
                    while (right._rowIndices[i_RightNZ] < i_LeftRow && i_RightNZ < i_RighColumnPointer)
                    {
                        values.Add(-right._values[i_RightNZ]);
                        rowIndices.Add(right._rowIndices[i_RightNZ]);

                        i_RightNZ++;
                    }

                    // If the the non-zero values of the current left and right column are a the same row.
                    if (right._rowIndices[i_RightNZ] == i_LeftRow)
                    {
                        values.Add(left._values[i_LeftNZ] - right._values[i_RightNZ]);
                        rowIndices.Add(i_LeftRow);

                        i_RightNZ++;
                    }
                    else
                    {
                        values.Add(left._values[i_LeftNZ]);
                        rowIndices.Add(i_LeftRow);
                    }
                }

                // Add the remaining non-zero values of the current right column which are after these of the current left column.
                for ( ; i_RightNZ < i_RighColumnPointer; i_RightNZ++)
                {
                    values.Add(-right._values[i_RightNZ]);
                    rowIndices.Add(right._rowIndices[i_RightNZ]);
                }

                columnPointers[i_C + 1] = values.Count;
            }

            return new CompressedColumn(left.RowCount, left.ColumnCount, columnPointers, rowIndices, values);
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

            // Row indices and values per columns.

            int[] columnPointers = new int[right.ColumnCount + 1];
            List<int> rowIndices = new List<int>();
            List<double> values = new List<double>();

            columnPointers[0] = 0;

            int i_RightNZ = right._columnPointers[0];
            // Iterate on the columns of right
            for (int i_C = 0; i_C < right.ColumnCount; i_C++)
            {
                int i_RightNZ_ColumnBound = right._columnPointers[i_C + 1];

                // For the initialisation of rowIndices[i_C] and values[i_C]
                while (rowIndices.Count == columnPointers[i_C] & i_RightNZ < i_RightNZ_ColumnBound)
                {
                    int i_RR = right._rowIndices[i_RightNZ];
                    double rightVal = right._values[i_RightNZ];

                    for (int i_LeftNZ = left._columnPointers[i_RR]; i_LeftNZ < left._columnPointers[i_RR + 1]; i_LeftNZ++)
                    {
                        int i_R = left._rowIndices[i_LeftNZ];
                        double leftVal = left._values[i_LeftNZ];

                        rowIndices.Add(i_R);
                        values.Add(leftVal * rightVal);
                    }

                    i_RightNZ++;
                }

                for (; i_RightNZ < i_RightNZ_ColumnBound; i_RightNZ++)
                {
                    int i_RR = right._rowIndices[i_RightNZ];
                    double rightVal = right._values[i_RightNZ];

                    int i_Pointer = columnPointers[i_C];
                    int i_LeftNZ_ColumnBound = left._columnPointers[i_RR + 1];

                    int i_LeftNZ = left._columnPointers[i_RR];
                    for (; i_LeftNZ < i_LeftNZ_ColumnBound; i_LeftNZ++)
                    {
                        int i_R = left._rowIndices[i_LeftNZ];
                        double leftVal = left._values[i_LeftNZ];

                        if(i_R < rowIndices[i_Pointer])
                        {
                            rowIndices.Insert(i_Pointer, i_R);
                            values.Insert(i_Pointer, leftVal * rightVal);

                            i_Pointer++;
                        }
                        else if(i_R == rowIndices[i_Pointer])
                        {
                            values[i_Pointer] += leftVal * rightVal;

                            i_Pointer++;
                            if (i_Pointer == values.Count) { i_LeftNZ++; break; }
                        }
                        else
                        {
                            i_Pointer++;
                            if (i_Pointer == values.Count) { break; }

                            i_LeftNZ--;
                        }
                    }

                    for (; i_LeftNZ < i_LeftNZ_ColumnBound; i_LeftNZ++)
                    {
                        int i_R = left._rowIndices[i_LeftNZ];
                        double leftVal = left._values[i_LeftNZ];

                        rowIndices.Add(i_R);
                        values.Add(leftVal * rightVal);
                    }
                }

                columnPointers[i_C + 1] = values.Count;
            }
            return new CompressedColumn(left.RowCount, right.ColumnCount, columnPointers, rowIndices, values);
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
            List<double> values = new List<double>(operand.NonZeroCount);
            for (int i_NZ = 0; i_NZ < operand.NonZeroCount; i_NZ++)
            {
                values.Add(factor * operand._values[i_NZ]);
            }

            List<int> rowIndices = new List<int>(operand._rowIndices);

            int[] columnPointers = new int[operand._columnPointers.Length];
            for (int i = 0; i < columnPointers.Length; i++)
            {
                columnPointers[i] = operand._columnPointers[i];
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
            List<double> values = new List<double>(operand.NonZeroCount);
            for (int i_NZ = 0; i_NZ < operand.NonZeroCount; i_NZ++)
            {
                values.Add(operand._values[i_NZ] * factor);
            }

            List<int> rowIndices = new List<int>(operand._rowIndices);

            int[] columnPointers = new int[operand._columnPointers.Length];
            for (int i = 0; i < columnPointers.Length; i++)
            {
                columnPointers[i] = operand._columnPointers[i];
            }

            return new CompressedColumn(operand.RowCount, operand.ColumnCount, columnPointers, rowIndices, values);
        }


        /// <summary>
        /// Computes the scalar division of a <see cref="CompressedColumn"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="operand"> <see cref="CompressedColumn"/> to divide. </param>
        /// <param name="divisor"> <see cref="double"/>-precision real number to divide with. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the scalar division. </returns>
        public static CompressedColumn Divide(CompressedColumn operand, double divisor)
        {
            List<double> values = new List<double>(operand.NonZeroCount);
            for (int i_NZ = 0; i_NZ < operand.NonZeroCount; i_NZ++)
            {
                values.Add(operand._values[i_NZ] / divisor);
            }

            List<int> rowIndices = new List<int>(operand._rowIndices);

            int[] columnPointers = new int[operand._columnPointers.Length];
            for (int i = 0; i < columnPointers.Length; i++)
            {
                columnPointers[i] = operand._columnPointers[i];
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

            int i_NZ = matrix._columnPointers[0];
            for (int i = 0; i < matrix.ColumnCount; i++)
            {
                for (; i_NZ < matrix._columnPointers[i + 1]; i_NZ++)
                {
                    int i_R = matrix._rowIndices[i_NZ];
                    components[i_R] += matrix._values[i_NZ] * vector[i];
                }
            }

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
            SparseVector result = new SparseVector(matrix.RowCount);

            for (int i = 0; i < matrix.ColumnCount; i++)
            {
                bool isZero = !vector.TryGetComponent(i, out double val);
                if (isZero) { continue; }

                for (int i_NZ = matrix._columnPointers[i]; i_NZ < matrix._columnPointers[i + 1]; i_NZ++)
                {
                    int i_R = matrix._rowIndices[i_NZ];

                    if (result.TryGetComponent(i_R, out double existing)) { result[i_R] = existing + (matrix._values[i_NZ] * val); }
                    else { result[i_R] = matrix._values[i_NZ] * val; ; }
                }
            }

            return result;

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

            int i_NZ = matrix._columnPointers[0];
            for (int i_R = 0; i_R < components.Length; i_R++)
            {
                double component = 0.0;

                for (; i_NZ < matrix._columnPointers[i_R + 1]; i_NZ++)
                {
                    int i = matrix._rowIndices[i_NZ];
                    component += matrix._values[i_NZ] * vector[i];
                }

                components[i_R] = component;
            }

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
            int size = matrix.ColumnCount;
            List<int> rowIndices = new List<int>();
            List<double> values = new List<double>();

            int i_NZ = matrix._columnPointers[0];
            for (int i_R = 0; i_R < size; i_R++)
            {
                double component = 0.0;

                for (; i_NZ < matrix._columnPointers[i_R + 1]; i_NZ++)
                {
                    if (vector.TryGetComponent(matrix._rowIndices[i_NZ], out double val)) { component += matrix._values[i_NZ] * val; }
                }

                if (component != 0.0) { rowIndices.Add(i_R); values.Add(component); }
            }

            return new SparseVector(size, rowIndices, values);
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
            return _values[index];
        }

        /// <summary>
        /// Returns the non-zero values of the current <see cref="CompressedColumn"/> sparse matrix.
        /// </summary>
        /// <returns> The non-zero values of the current <see cref="CompressedColumn"/> sparse matrix. </returns>
        public double[] Values()
        {
            return _values.ToArray();
        }


        /// <summary>
        /// Returns the row index of the current <see cref="CompressedColumn"/> sparse matrix at a given index.
        /// </summary>
        /// <param name="index"> Index of the row index to get.</param>
        /// <returns> The row index of the current <see cref="CompressedColumn"/> sparse matrix at the given index. </returns>
        public int GetRowIndex(int index)
        {
            return _rowIndices[index];
        }

        /// <summary>
        /// Returns the row indices of the current <see cref="CompressedColumn"/> sparse matrix.
        /// </summary>
        /// <returns> The row indices of the current <see cref="CompressedColumn"/> sparse matrix. </returns>
        public int[] RowIndices()
        {
            return _rowIndices.ToArray();
        }


        /// <summary>
        /// Returns the column pointers of the current <see cref="CompressedColumn"/> sparse matrix.
        /// </summary>
        /// <param name="index"> Index of the column pointers to get.</param>
        /// <returns> The column pointers of the current <see cref="CompressedColumn"/> sparse matrix. </returns>
        public int GetColumnPointers(int index)
        {
            return _columnPointers[index];
        }

        /// <summary>
        /// Returns the column pointers of the current <see cref="CompressedColumn"/> sparse matrix.
        /// </summary>
        /// <returns> The column pointers of the current <see cref="CompressedColumn"/> sparse matrix. </returns>
        public int[] ColumnPointers()
        {
            return _columnPointers.Clone() as int[];
        }


        /// <inheritdoc/>
        public override void Transpose()
        {
            // Get the number of elements per row of the current matrix
            int[] rowHelper = new int[RowCount];
            for (int i_NZ = 0; i_NZ < NonZeroCount; i_NZ++)
            {
                rowHelper[_rowIndices[i_NZ]]++;
            }


            // Creates the new column pointer
            int[] columnPointers = new int[RowCount + 1];

            columnPointers[0] = 0;
            for (int i_C = 0; i_C < RowCount; i_C++)
            {
                columnPointers[i_C + 1] = columnPointers[i_C] + rowHelper[i_C];
                rowHelper[i_C] = 0;
            }


            // Creates the new row index and value lists
            int[] rowIndices = new int[NonZeroCount];
            double[] values = new double[NonZeroCount];

            int i_ColumnNZ = _columnPointers[0];
            for (int i_R = 0; i_R < ColumnCount; i_R++)
            {
                for (; i_ColumnNZ < _columnPointers[i_R + 1]; i_ColumnNZ++)
                {

                    int i_C = _rowIndices[i_ColumnNZ];
                    int i_Pointer = columnPointers[i_C] + rowHelper[i_C];

                    rowIndices[i_Pointer] = i_R;
                    values[i_Pointer] = _values[i_ColumnNZ];
                    rowHelper[i_C]++;
                }
            }


            _columnPointers = columnPointers;
            _rowIndices = new List<int>(rowIndices);
            _values = new List<double>(values);

            (_rowCount, _columnCount) = (_columnCount, _rowCount);
        }

        
        /// <summary>
        /// Converts a the current <see cref="CompressedColumn"/> matrix into an equivalent <see cref="CompressedRow"/> matrix.
        /// </summary>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the conversion. </returns>
        public CompressedRow ToCompressedRow()
        {
            // Get the number of elements per row
            int[] rowHelper = new int[RowCount];
            for (int i_NZ = 0; i_NZ < NonZeroCount; i_NZ++)
            {
                rowHelper[_rowIndices[i_NZ]]++;
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
            int[] columnIndices = new int[NonZeroCount];
            double[] values = new double[NonZeroCount];

            int i_ColumnNZ = _columnPointers[0];
            for (int i_C = 0; i_C < ColumnCount; i_C++)
            {
                for (; i_ColumnNZ < _columnPointers[i_C + 1]; i_ColumnNZ++)
                {
                    int i_R = _rowIndices[i_ColumnNZ];
                    int i_Pointer = rowPointers[i_R] + rowHelper[i_R];

                    columnIndices[i_Pointer] = i_C;
                    values[i_Pointer] = _values[i_ColumnNZ];
                    rowHelper[i_R]++;
                }
            }


            List<int> list_ColumnsIndices = new List<int>(columnIndices);
            List<double> list_Values = new List<double>(values);

            return new CompressedRow(RowCount, ColumnCount, rowPointers, list_ColumnsIndices, list_Values);
        }

        #endregion


        /// <summary>
        /// Computes the kernel (or null-space) of the current <see cref="SparseMatrix"/> using the QR decomposition.
        /// </summary>
        /// <remarks> The method is adapted for rectangular matrix. </remarks>
        /// <returns> The vectors forming a basis of the null-space. </returns>
        public DenseVector[] Kernel()
        {
            // Verification
            if (NonZeroCount == 0) { throw new Exception("The kernel of a zero matrix cannot be computed."); }

            CompressedColumn squareCcs = CompleteToSquare();

            DenseVector[] kernel = ComputeKernel(ref squareCcs);

            if (RowCount != ColumnCount) { kernel = FilterKernelVectors(kernel); }

            return kernel;
        }

        #region Other Methods

        /// <summary>
        /// Completes the current <see cref="CompressedColumn"/> to be as square matrix by duplicating the non-zero column with the least component.
        /// </summary>
        /// <returns> The new suqare <see cref="CompressedColumn"/>. </returns>
        private CompressedColumn CompleteToSquare()
        {
            // If the matrix is rectangular with more columns
            if (_rowCount < _columnCount)
            {
                // Complete the current matrix to be a square matrix.
                // In order to keep the null space unchanged, the non-zero row with the least component is duplicated.
                CompressedRow crs = ToCompressedRow();

                int i_SparseRow = 0;
                int sparseRowCount = NonZeroCount;

                int[] rowPointers = new int[_columnCount + 1];
                rowPointers[0] = 0;
                for (int i_R = 0; i_R < _rowCount; i_R++)
                {
                    rowPointers[i_R + 1] = crs.GetRowPointer(i_R + 1);

                    int count = rowPointers[i_R + 1] - rowPointers[i_R];
                    if (count != 0 & count < sparseRowCount) { sparseRowCount = count; i_SparseRow = i_R; }
                }
                for (int i_R = _rowCount; i_R < _columnCount; i_R++)
                {
                    rowPointers[i_R + 1] = rowPointers[i_R] + sparseRowCount;
                }

                List<int> columnIndices = new List<int>(NonZeroCount + (sparseRowCount * (_columnCount - _rowCount)));
                List<double> values = new List<double>(NonZeroCount + (sparseRowCount * (_columnCount - _rowCount)));
                for (int i_R = 0; i_R < _rowCount; i_R++)
                {
                    for (int i_NZ = rowPointers[i_R]; i_NZ < rowPointers[i_R + 1]; i_NZ++)
                    {
                        columnIndices.Add(crs.GetColumnIndex(i_NZ));
                        values.Add(crs.GetValues(i_NZ));
                    }
                }
                for (int i_R = 0; i_R < _columnCount - _rowCount; i_R++)
                {
                    for (int i_NZ = rowPointers[i_SparseRow]; i_NZ < rowPointers[i_SparseRow + 1]; i_NZ++)
                    {
                        columnIndices.Add(crs.GetColumnIndex(i_NZ));
                        values.Add(crs.GetValues(i_NZ));
                    }
                }

                CompressedRow result = new CompressedRow(_columnCount, _columnCount, rowPointers, columnIndices, values);
                return result.ToCompressedColumn();
            }
            // If the matrix is rectangular with more rows
            else if (_columnCount < _rowCount)
            {
                // Complete the current matrix to be a square matrix.
                // In order to keep the null space unchanged, the non-zero column with the least component is duplicated.
                int i_SparseColumn = 0;
                int sparseColumnCount = NonZeroCount;

                int[] columnPointers = new int[_rowCount + 1];
                columnPointers[0] = 0;
                for (int i_C = 0; i_C < _columnCount; i_C++)
                {
                    columnPointers[i_C + 1] = _columnPointers[i_C + 1];

                    int count = columnPointers[i_C + 1] - columnPointers[i_C];
                    if (count != 0 & count < sparseColumnCount) { sparseColumnCount = count; i_SparseColumn = i_C; }
                }
                for (int i_C = _columnCount; i_C < _rowCount; i_C++)
                {
                    columnPointers[i_C + 1] = columnPointers[i_C] + sparseColumnCount;
                }

                List<int> rowIndices = new List<int>(NonZeroCount + (sparseColumnCount * (_rowCount - _columnCount)));
                List<double> values = new List<double>(NonZeroCount + (sparseColumnCount * (_rowCount - _columnCount)));
                for (int i_C = 0; i_C < _columnCount; i_C++)
                {
                    for (int i_NZ = columnPointers[i_C]; i_NZ < columnPointers[i_C + 1]; i_NZ++)
                    {
                        rowIndices.Add(_rowIndices[i_NZ]);
                        values.Add(_values[i_NZ]);
                    }
                }
                for (int i_C = 0; i_C < _rowCount - _columnCount; i_C++)
                {
                    for (int i_NZ = columnPointers[i_SparseColumn]; i_NZ < columnPointers[i_SparseColumn + 1]; i_NZ++)
                    {
                        rowIndices.Add(_rowIndices[i_NZ]);
                        values.Add(_values[i_NZ]);
                    }
                }

                return new CompressedColumn(_rowCount, _rowCount, columnPointers, rowIndices, values);
            }
            // If the matrix is a square matrix
            else
            {
                int[] columnPointers = new int[_columnPointers.Length];
                for (int i = 0; i < _columnPointers.Length; i++)
                {
                    columnPointers[i] = _columnPointers[i];
                }
                List<int> rowIndices = new List<int>(_rowIndices.Count);
                for (int i = 0; i < _rowIndices.Count; i++)
                {
                    rowIndices.Add(_rowIndices[i]);
                }
                List<double> values = new List<double>(_values.Count);
                for (int i = 0; i < _values.Count; i++)
                {
                    values.Add(_values[i]);
                }

                return new CompressedColumn(_rowCount, _columnCount, columnPointers, rowIndices, values);
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
            if (_rowCount < _columnCount)
            {
                // In this case, the initial rectangular matrix was completed with linearly-dependent row.
                // Fortunately, the kernel of the initial matrix and the completed matrix are the same.
                return kernel;
            }
            // If the matrix is rectangular with more rows
            else if (_columnCount < _rowCount)
            {
                // In this case, the initial rectangular matrix was completed with linearly-dependent columns.
                // Fortunately, we know that the vectors of the actual null-space should be orthogonal to

                // This array contains the vectors to which our null space should be orthogonal
                DenseVector[] orthogonalVectors = new DenseVector[_rowCount - _columnCount]; 
                for (int i = 0; i < (_rowCount - _columnCount); i++)
                {
                    orthogonalVectors[i] = new DenseVector(_rowCount);
                    orthogonalVectors[i][_columnCount + i] = 1;
                }

                Storage.DictionaryOfKeys dok = new Storage.DictionaryOfKeys();
                for (int i_R = 0; i_R < (_rowCount - _columnCount); i_R++)
                {
                    for (int i_C = 0; i_C < kernel.Length; i_C++)
                    {
                        double val = DenseVector.TransposeMultiply(kernel[i_C], orthogonalVectors[i_R]);
                        if (val != 0.0) { dok.Add(val, i_R, i_C); }
                    }
                }

                CompressedColumn ccs = new CompressedColumn((_rowCount - _columnCount), kernel.Length, dok);

                DenseVector[] intermediateresult = ccs.Kernel();

                // Create the solution
                DenseVector[] finalResults = new DenseVector[intermediateresult.Length];

                int counter = 0;
                foreach (DenseVector combination in intermediateresult)
                {
                    DenseVector finalResult = new DenseVector(_rowCount);
                    for (int i = 0; i < kernel.Length; i++)
                    {
                        DenseVector vec = DenseVector.Multiply(combination[i], kernel[i]);
                        finalResult = DenseVector.Add(finalResult,vec);
                    }
                    //This part can be improved
                    var array = finalResult.ToArray();

                    Array.Resize(ref array, _columnCount);

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
            List<double> values = new List<double>(NonZeroCount);
            for (int i_NZ = 0; i_NZ < NonZeroCount; i_NZ++)
            {
                values.Add(-_values[i_NZ]);
            }

            _values = values;
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