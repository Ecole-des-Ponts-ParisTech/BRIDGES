using System;
using System.Collections.Generic;


namespace BRIDGES.LinearAlgebra.Matrices.Sparse
{
    /// <summary>
    /// Class defining a sparse matrix with a compressed column storage.
    /// </summary>
    public class CompressedColumn : SparseMatrix
    {
        #region Fields

        /// <summary>
        /// Non-zero values of the matrix.
        /// </summary>
        private List<double> _values;

        /// <summary>
        /// Row indices associated with the non-zero values.
        /// </summary>
        private List<int> _rowIndices;

        /// <summary>
        /// Pointers giving the number of non-zero values before the row at a given index.
        /// </summary>
        /// <remarks> Array of length (ColumnCount + 1), starting at 0 and ending at <see cref="SparseMatrix.NonZeroCount"/>. </remarks>
        private int[] _columnPointers;

        #endregion

        #region Properties

        public override double this[int row, int column] => throw new NotImplementedException();

        #endregion

        #region Contructors

        /// <summary>
        /// Initialises a new instance of the <see cref="CompressedColumn"/> class by defining its number of row and column.
        /// </summary>
        /// <param name="rowCount"> Number of rows of the <see cref="CompressedColumn"/>. </param>
        /// <param name="columnCount"> Number of columns of the <see cref="CompressedColumn"/>. </param>
        /// <param name="values"> Non-zero values of the <see cref="CompressedColumn"/>. </param>
        /// <param name="rowIndices"> Row indices of the <see cref="CompressedColumn"/>.</param>
        /// <param name="columnPointers"> Column pointers of the <see cref="CompressedColumn"/>. </param>
        protected CompressedColumn(int rowCount, int columnCount, List<double> values, List<int> rowIndices, int[] columnPointers)
            : base(rowCount, columnCount)
        {
            // Verifications
            if (columnPointers.Length != columnCount + 1) 
            { 
                throw new ArgumentException("The number of column pointers is not consistent with the number of columns.", nameof(columnPointers)); 
            }
            if (values.Count != rowIndices.Count) 
            { 
                throw new ArgumentException($"The number of elements in {nameof(values)} and {nameof(rowIndices)} do not match."); 
            }
            if (columnPointers[columnPointers.Length - 1] != values.Count ) 
            { 
                throw new ArgumentException($"The last value of {nameof(columnPointers)} is not equal to the number of non-zero values."); 
            }

            _values = values;
            _rowIndices = rowIndices;
            _columnPointers = columnPointers;

            NonZeroCount = values.Count;
        }

        public CompressedColumn(int rowCount, int columnCount, Storage.DictionaryOfKeys dok)
            : base(rowCount, columnCount)
        {
            
        }

        public CompressedColumn(int rowCount, int columnCount, Storage.CoordinateList cl)
            : base(rowCount, columnCount)
        {

        }

        #endregion

        #region Static Methods

        /******************** Algebraic Near Ring ********************/

        /// <summary>
        /// Computes the addition of two <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> <see cref="CompressedColumn"/> for the addition. </param>
        /// <param name="right"> <see cref="CompressedColumn"/> for the addition. </param>
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

            return new CompressedColumn(left.RowCount, left.ColumnCount, values, rowIndices, columnPointers);
        }

        /// <summary>
        /// Computes the subtraction of two <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> <see cref="CompressedColumn"/> to subtract. </param>
        /// <param name="right"> <see cref="CompressedColumn"/> to subtract with. </param>
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

            return new CompressedColumn(left.RowCount, left.ColumnCount, values, rowIndices, columnPointers);
        }


        /// <summary>
        /// Computes the multiplication of two <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> <see cref="CompressedColumn"/> for the multiplication. </param>
        /// <param name="right"> <see cref="CompressedColumn"/> for the multiplication. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the multiplication. </returns>
        public static CompressedColumn Multiply(CompressedColumn left, CompressedColumn right)
        {
            // Verifications
            if (left.ColumnCount != right.RowCount)
            {
                throw new ArgumentException("The matrices size does not allow their multiplication.");
            }

            List<double> values = new List<double>(left.NonZeroCount * right.NonZeroCount);
            List<int> rowIndices = new List<int>(left.NonZeroCount * right.NonZeroCount);

            int[] columnPointers = new int[left.ColumnCount + 1];
            columnPointers[0] = 0;

            // Iterate on the columns of right
            for (int i_C = 0; i_C < right.ColumnCount; i_C++)
            {
                // Initialisation on the first non-zero values of the current right column
                {
                    int i_RightNZ = right._columnPointers[i_C];

                    int i_RR = right._rowIndices[i_RightNZ];
                    for (int i_LeftNZ = left._columnPointers[i_RR]; i_LeftNZ < left._columnPointers[i_RR + 1]; i_LeftNZ++) 
                    {
                        values.Add(left._values[i_LeftNZ] * right._values[i_RightNZ]);
                        rowIndices.Add(left._rowIndices[i_LeftNZ]);
                    }
                }

                // Iteration from the second non-zero values of the current right column
                for (int i_RightNZ = right._columnPointers[i_C] + 1; i_RightNZ < right._columnPointers[i_C + 1]; i_RightNZ++)
                {
                    int i_RR = right._rowIndices[i_RightNZ];

                    int i_Pointer = columnPointers[i_C]; // To place the potential new value in the list of values and row indices

                    // Insert or add the values whose row is below or equal to the last column row
                    int i_LeftNZ = left._columnPointers[i_RR];
                    for (; i_LeftNZ < left._columnPointers[i_RR + 1]; i_LeftNZ++)
                    {
                        int i_R = left._rowIndices[i_LeftNZ];
                        if (i_R < rowIndices[i_Pointer])
                        {
                            values.Insert(i_Pointer, left._values[i_LeftNZ] * right._values[i_RightNZ]);
                            rowIndices.Insert(i_Pointer, left._rowIndices[i_LeftNZ]);

                            i_Pointer++;
                        }
                        else if(i_R == rowIndices[i_Pointer])
                        {
                            values[i_Pointer] += left._values[i_LeftNZ] * right._values[i_RightNZ] ;

                            i_Pointer++;
                            if (i_Pointer == values.Count) { break; }
                        }
                        else 
                        {
                            i_Pointer++;
                            if (i_Pointer == values.Count) { break; }

                            i_LeftNZ--;
                        }
                    }

                    // Add the values whose row is strictly after the last column row
                    for (; i_LeftNZ < left._columnPointers[i_RR + 1]; i_LeftNZ++)
                    {
                        values.Add(left._values[i_LeftNZ] * right._values[i_RightNZ]);
                        rowIndices.Add(left._rowIndices[i_LeftNZ]);
                    }
                }

                columnPointers[i_C + 1] = values.Count;
            }

            return new CompressedColumn(left.RowCount, left.ColumnCount, values, rowIndices, columnPointers);
        }


        /******************** Sparse Matrix Embedding ********************/

        /// <summary>
        /// Computes the addition of a <see cref="CompressedColumn"/> with a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> <see cref="CompressedColumn"/> for the addition. </param>
        /// <param name="right"> <see cref="SparseMatrix"/> for the addition. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the addition. </returns>
        public static CompressedColumn Add(CompressedColumn left, SparseMatrix right)
        {
            if (right is CompressedColumn ccsRight) { return CompressedColumn.Add(left, ccsRight); }
            else if (right is CompressedRow crsRight) { return CompressedColumn.Add(left, crsRight); }
            else { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Computes the addition of a <see cref="SparseMatrix"/> with a <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> <see cref="SparseMatrix"/> for the addition. </param>
        /// <param name="right"> <see cref="CompressedColumn"/> for the addition. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the addition. </returns>
        public static CompressedColumn Add(SparseMatrix left, CompressedColumn right)
        {
            if (left is CompressedColumn ccsLeft) { return CompressedColumn.Add(ccsLeft, right); }
            else if (left is CompressedRow crsLeft) { return CompressedColumn.Add(crsLeft, right); }
            else { throw new NotImplementedException(); }
        }


        /// <summary>
        /// Computes the subtraction of a <see cref="CompressedColumn"/> with a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> <see cref="CompressedColumn"/> to subtract. </param>
        /// <param name="right"> <see cref="SparseMatrix"/> to subtract with. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the subtraction. </returns>
        public static CompressedColumn Subtract(CompressedColumn left, SparseMatrix right)
        {
            if (right is CompressedColumn ccsRight) { return CompressedColumn.Subtract(left, ccsRight); }
            else if (right is CompressedRow crsRight) { return CompressedColumn.Subtract(left, crsRight); }
            else { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Computes the subtraction of a <see cref="SparseMatrix"/> with a <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> <see cref="SparseMatrix"/> to subtract. </param>
        /// <param name="right"> <see cref="CompressedColumn"/> to subtract with. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the subtraction. </returns>
        public static CompressedColumn Subtract(SparseMatrix left, CompressedColumn right)
        {
            if (left is CompressedColumn ccsLeft) { return CompressedColumn.Subtract(ccsLeft, right); }
            else if (left is CompressedRow crsLeft) { return CompressedColumn.Subtract(crsLeft, right); }
            else { throw new NotImplementedException(); }
        }


        /// <summary>
        /// Computes the multiplication of a <see cref="CompressedColumn"/> with a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> <see cref="CompressedColumn"/> for the multiplication. </param>
        /// <param name="right"> <see cref="SparseMatrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the multiplication. </returns>
        public static CompressedColumn Multiply(CompressedColumn left, SparseMatrix right)
        {
            if (right is CompressedColumn ccsRight) { return CompressedColumn.Multiply(left, ccsRight); }
            else if (right is CompressedRow crsRight) { return CompressedColumn.Multiply(left, crsRight); }
            else { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Computes the multiplication of a <see cref="SparseMatrix"/> with a <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> <see cref="SparseMatrix"/> for the multiplication. </param>
        /// <param name="right"> <see cref="CompressedColumn"/> for the multiplication. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the multiplication. </returns>
        public static CompressedColumn Multiply(SparseMatrix left, CompressedColumn right)
        {
            if (left is CompressedColumn ccsLeft) { return CompressedColumn.Multiply(ccsLeft, right); }
            else if (left is CompressedRow crsLeft) { return CompressedColumn.Multiply(crsLeft, right); }
            else { throw new NotImplementedException(); }
        }


        /******************** CompressedRow Embedding ********************/

        /// <summary>
        /// Computes the addition of a <see cref="CompressedColumn"/> with a <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> <see cref="CompressedColumn"/> for the addition. </param>
        /// <param name="right"> <see cref="CompressedRow"/> for the addition. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the addition. </returns>
        public static CompressedColumn Add(CompressedColumn left, CompressedRow right)
        {
            CompressedColumn ccs = right.ToCompressedColumn();

            return CompressedColumn.Add(left, ccs);
        }

        /// <summary>
        /// Computes the addition of a <see cref="CompressedRow"/> with a <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> <see cref="CompressedRow"/> for the addition. </param>
        /// <param name="right"> <see cref="CompressedColumn"/> for the addition. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the addition. </returns>
        public static CompressedColumn Add(CompressedRow left, CompressedColumn right)
        {
            CompressedColumn ccs = left.ToCompressedColumn();

            return CompressedColumn.Add(ccs, right);
        }


        /// <summary>
        /// Computes the subtraction of a <see cref="CompressedColumn"/> with a <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> <see cref="CompressedColumn"/> to subtract. </param>
        /// <param name="right"> <see cref="CompressedRow"/> to subtract with. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the subtraction. </returns>
        public static CompressedColumn Subtract(CompressedColumn left, CompressedRow right)
        {
            CompressedColumn ccs = right.ToCompressedColumn();

            return CompressedColumn.Subtract(left, ccs);
        }

        /// <summary>
        /// Computes the subtraction of a <see cref="CompressedRow"/> with a <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> <see cref="CompressedRow"/> to subtract. </param>
        /// <param name="right"> <see cref="CompressedColumn"/> to subtract with. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the subtraction. </returns>
        public static CompressedColumn Subtract(CompressedRow left, CompressedColumn right)
        {
            CompressedColumn ccs = left.ToCompressedColumn();

            return CompressedColumn.Subtract(ccs, right);
        }


        /// <summary>
        /// Computes the multiplication of a <see cref="CompressedColumn"/> with a <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> <see cref="CompressedColumn"/> for the multiplication. </param>
        /// <param name="right"> <see cref="CompressedRow"/> for the multiplication. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the multiplication. </returns>
        public static CompressedColumn Multiply(CompressedColumn left, CompressedRow right)
        {
            CompressedColumn ccs = right.ToCompressedColumn();

            return CompressedColumn.Multiply(left, ccs);
        }

        /// <summary>
        /// Computes the multiplication of a <see cref="CompressedRow"/> with a <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> <see cref="CompressedRow"/> for the multiplication. </param>
        /// <param name="right"> <see cref="CompressedColumn"/> for the multiplication. </param>
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

            return new CompressedColumn(operand.RowCount, operand.ColumnCount, values, rowIndices, columnPointers);
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

            return new CompressedColumn(operand.RowCount, operand.ColumnCount, values, rowIndices, columnPointers);
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

            return new CompressedColumn(operand.RowCount, operand.ColumnCount, values, rowIndices, columnPointers);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the non-zero values of the current <see cref="CompressedColumn"/> sparse matrix.
        /// </summary>
        /// <returns> The non-zero values of the current <see cref="CompressedColumn"/> sparse matrix. </returns>
        public double[] GetValues()
        {
            return _values.ToArray();
        }

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
        /// Returns the row indices of the current <see cref="CompressedColumn"/> sparse matrix.
        /// </summary>
        /// <returns> The row indices of the current <see cref="CompressedColumn"/> sparse matrix. </returns>
        public int[] GetRowIndices()
        {
            return _rowIndices.ToArray();
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
        /// Returns the column pointers of the current <see cref="CompressedColumn"/> sparse matrix.
        /// </summary>
        /// <returns> The column pointers of the current <see cref="CompressedColumn"/> sparse matrix. </returns>
        public int[] GetColumnPointers()
        {
            return _columnPointers.Clone() as int[];
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


        public override void Transpose()
        {
            ww;
        }


        public CompressedRow ToCompressedRow()
        {

        }

        #endregion

        #region Other Methods

        /******************** Algebraic Additive Group ********************/

        /// <summary>
        /// Computes the addition of the current <see cref="CompressedColumn"/> with another <see cref="Matrix"/>.
        /// </summary>
        /// <param name="right"> <see cref="Matrix"/> to add with on the right. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the addition. </returns>
        protected override Matrix Add(Matrix right)
        {
            if (right is DenseMatrix denseRight) { return DenseMatrix.Add(this, denseRight); }
            else if (right is SparseMatrix sparseRight) { return CompressedColumn.Add(this, sparseRight); }
            else { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Computes the subtraction of the current <see cref="CompressedColumn"/> with another <see cref="Matrix"/>.
        /// </summary>
        /// <param name="right"> <see cref="Matrix"/> to subtract with on the right. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the subtraction. </returns>
        protected override Matrix Subtract(Matrix right)
        {
            if (right is DenseMatrix denseRight) { return DenseMatrix.Subtract(this, denseRight); }
            else if (right is SparseMatrix sparseRight) { return CompressedColumn.Subtract(this, sparseRight); }
            else { throw new NotImplementedException(); }
        }

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

        protected override Matrix Zero()
        {
            return new CompressedColumn(RowCount, ColumnCount);
        }

        /******************** Algebraic Multiplicative SemiGroup ********************/

        /// <summary>
        /// Computes the multiplication of the current <see cref="CompressedColumn"/> with another <see cref="Matrix"/>.
        /// </summary>
        /// <param name="right"> <see cref="Matrix"/> to multiply with on the right. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the addition. </returns>
        protected override Matrix Multiply(Matrix right)
        {
            if (right is DenseMatrix denseRight) { return DenseMatrix.Multiply(this, denseRight); }
            else if (right is SparseMatrix sparseRight) { return CompressedColumn.Multiply(this, sparseRight); }
            else { throw new NotImplementedException(); }
        }

        /******************** Group Action ********************/

        /// <summary>
        /// Computes the scalar multiplication the current <see cref="CompressedColumn"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="factor"> <see cref="double"/>-precision real number. </param>
        /// <returns> The new <see cref="CompressedColumn"/> as a <see cref="Matrix"/> resulting from the scalar multiplication. </returns>
        protected override Matrix Multiply(double factor)
        {
            return CompressedColumn.Multiply(this, factor);
        }

        /// <summary>
        /// Computes the scalar division of the current <see cref="CompressedColumn"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="divisor"> <see cref="double"/>-precision real number to divide with. </param>
        /// <returns> The new <see cref="CompressedColumn"/> as a <see cref="Matrix"/> resulting from the scalar division. </returns>
        protected override Matrix Divide(double divisor)
        {
            return CompressedColumn.Divide(this, divisor);
        }

        #endregion
    }
}
