using System;
using System.Collections.Generic;


namespace BRIDGES.LinearAlgebra.Matrices.Sparse
{
    /// <summary>
    /// Class defining a sparse matrix with a compressed row storage.
    /// </summary>
    public class CompressedRow : SparseMatrix
    {
        #region Fields

        /// <summary>
        /// Non-zero values of the matrix
        /// </summary>
        private List<double> _values;

        /// <summary>
        /// Row indices associated with the non-zero values.
        /// </summary>
        private List<int> _columnIndices;

        /// <summary>
        /// Pointers giving the number of non-zero values before the row at a given index.
        /// </summary>
        /// <remarks> Array of length (ColumnCount + 1), starting at 0 and ending at <see cref="SparseMatrix.NonZeroCount"/>. </remarks>
        private int[] _rowPointers;

        #endregion

        #region Properties

        public override double this[int row, int column] => throw new NotImplementedException();

        #endregion

        #region Contructors

        /// <summary>
        /// Initialises a new instance of the <see cref="CompressedRow"/> class by defining its number of row and column.
        /// </summary>
        /// <param name="rowCount"> Number of rows of the <see cref="CompressedRow"/>. </param>
        /// <param name="columnCount"> Number of columns of the <see cref="CompressedRow"/>. </param>
        /// <param name="values"> Non-zero values of the <see cref="CompressedRow"/>. </param>
        /// <param name="columnIndices"> Column indices of the <see cref="CompressedRow"/>.</param>
        /// <param name="rowPointers"> Row pointers of the <see cref="CompressedRow"/>. </param>
        protected CompressedRow(int rowCount, int columnCount, List<double> values, List<int> columnIndices, int[] rowPointers)
            : base(rowCount, columnCount)
        {
            // Verifications
            if (rowPointers.Length != rowCount + 1)
            {
                throw new ArgumentException("The number of row pointers is not consistent with the number of columns.", nameof(rowPointers));
            }
            if (values.Count != columnIndices.Count)
            {
                throw new ArgumentException($"The number of elements in {nameof(values)} and {nameof(columnIndices)} do not match.");
            }
            if (rowPointers[rowPointers.Length - 1] != values.Count)
            {
                throw new ArgumentException($"The last value of {nameof(rowPointers)} is not equal to the number of non-zero values.");
            }

            _values = values;
            _columnIndices = columnIndices;
            _rowPointers = rowPointers;

            NonZeroCount = values.Count;
        }

        public CompressedRow(int rowCount, int columnCount, Storage.DictionaryOfKeys dok)
            : base(rowCount, columnCount)
        {

        }

        public CompressedRow(int rowCount, int columnCount, Storage.CoordinateList cl)
            : base(rowCount, columnCount)
        {

        }

        #endregion

        #region Static Methods

        /******************** Algebraic Near Ring ********************/

        /// <summary>
        /// Computes the addition of two <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> <see cref="CompressedRow"/> for the addition. </param>
        /// <param name="right"> <see cref="CompressedRow"/> for the addition. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the addition. </returns>
        public static CompressedRow Add(CompressedRow left, CompressedRow right)
        {
            // Verifications
            if (left.ColumnCount != right.ColumnCount || left.RowCount != right.RowCount)
            {
                throw new ArgumentException("The matrices do not have the same size.");
            }

            List<double> values = new List<double>(left.NonZeroCount + right.NonZeroCount);
            List<int> columnIndices = new List<int>(left.NonZeroCount + right.NonZeroCount);

            int[] rowPointers = new int[left.ColumnCount + 1];
            rowPointers[0] = 0;

            int i_RightNZ = 0;

            // Iterate on the rows of left
            for (int i_R = 0; i_R < left.RowCount; i_R++)
            {
                int i_RighRowPointer = right._rowPointers[i_R + 1];

                // Iterate on the non-zero values of the current left row
                for (int i_LeftNZ = left._rowPointers[i_R]; i_LeftNZ < left._rowPointers[i_R + 1]; i_LeftNZ++)
                {
                    int i_LeftColumn = left._columnIndices[i_LeftNZ];

                    // Add the non-zero values of the current right row which are before the current left non-zero value.
                    while (right._columnIndices[i_RightNZ] < i_LeftColumn && i_RightNZ < i_RighRowPointer)
                    {
                        values.Add(right._values[i_RightNZ]);
                        columnIndices.Add(right._columnIndices[i_RightNZ]);

                        i_RightNZ++;
                    }

                    // If the the non-zero values of the current left and right row are a the same column.
                    if (right._columnIndices[i_RightNZ] == i_LeftColumn)
                    {
                        values.Add(left._values[i_LeftNZ] + right._values[i_RightNZ]);
                        columnIndices.Add(i_LeftColumn);

                        i_RightNZ++;
                    }
                    else
                    {
                        values.Add(left._values[i_LeftNZ]);
                        columnIndices.Add(i_LeftColumn);
                    }
                }

                // Add the remaining non-zero values of the current right row which are after these of the current left row.
                for (; i_RightNZ < i_RighRowPointer; i_RightNZ++)
                {
                    values.Add(right._values[i_RightNZ]);
                    columnIndices.Add(right._columnIndices[i_RightNZ]);
                }

                rowPointers[i_R + 1] = values.Count;
            }

            return new CompressedRow(left.RowCount, left.ColumnCount, values, columnIndices, rowPointers);
        }

        /// <summary>
        /// Computes the subtraction of two <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> <see cref="CompressedRow"/> to subtract. </param>
        /// <param name="right"> <see cref="CompressedRow"/> to subtract with. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the subtraction. </returns>
        public static CompressedRow Subtract(CompressedRow left, CompressedRow right)
        {
            // Verifications
            if (left.ColumnCount != right.ColumnCount || left.RowCount != right.RowCount)
            {
                throw new ArgumentException("The matrices do not have the same size.");
            }

            List<double> values = new List<double>(left.NonZeroCount + right.NonZeroCount);
            List<int> columnIndices = new List<int>(left.NonZeroCount + right.NonZeroCount);

            int[] rowPointers = new int[left.ColumnCount + 1];
            rowPointers[0] = 0;

            int i_RightNZ = 0;

            // Iterate on the rows of left
            for (int i_R = 0; i_R < left.RowCount; i_R++)
            {
                int i_RighRowPointer = right._rowPointers[i_R + 1];

                // Iterate on the non-zero values of the current left row
                for (int i_LeftNZ = left._rowPointers[i_R]; i_LeftNZ < left._rowPointers[i_R + 1]; i_LeftNZ++)
                {
                    int i_LeftColumn = left._columnIndices[i_LeftNZ];

                    // Add the non-zero values of the current right row which are before the current left non-zero value.
                    while (right._columnIndices[i_RightNZ] < i_LeftColumn && i_RightNZ < i_RighRowPointer)
                    {
                        values.Add(-right._values[i_RightNZ]);
                        columnIndices.Add(right._columnIndices[i_RightNZ]);

                        i_RightNZ++;
                    }

                    // If the the non-zero values of the current left and right row are a the same column.
                    if (right._columnIndices[i_RightNZ] == i_LeftColumn)
                    {
                        values.Add(left._values[i_LeftNZ] - right._values[i_RightNZ]);
                        columnIndices.Add(i_LeftColumn);

                        i_RightNZ++;
                    }
                    else
                    {
                        values.Add(left._values[i_LeftNZ]);
                        columnIndices.Add(i_LeftColumn);
                    }
                }

                // Add the remaining non-zero values of the current right row which are after these of the current left row.
                for (; i_RightNZ < i_RighRowPointer; i_RightNZ++)
                {
                    values.Add(-right._values[i_RightNZ]);
                    columnIndices.Add(right._columnIndices[i_RightNZ]);
                }

                rowPointers[i_R + 1] = values.Count;
            }

            return new CompressedRow(left.RowCount, left.ColumnCount, values, columnIndices, rowPointers);
        }


        /// <summary>
        /// Computes the multiplication of two <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> <see cref="CompressedRow"/> for the multiplication. </param>
        /// <param name="right"> <see cref="CompressedRow"/> for the multiplication. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the multiplication. </returns>
        public static CompressedRow Multiply(CompressedRow left, CompressedRow right)
        {
            // Verifications
            if (left.ColumnCount != right.RowCount)
            {
                throw new ArgumentException("The matrices size does not allow their multiplication.");
            }

            List<double> values = new List<double>(left.NonZeroCount * right.NonZeroCount);
            List<int> columnIndices = new List<int>(left.NonZeroCount * right.NonZeroCount);

            int[] rowPointers = new int[left.ColumnCount + 1];
            rowPointers[0] = 0;

            // Iterate on the row of left
            for (int i_R = 0; i_R < left.RowCount; i_R++)
            {
                // Initialisation on the first non-zero values of the current left row
                {
                    int i_LeftNZ = left._rowPointers[i_R];

                    int i_LC = left._columnIndices[i_LeftNZ];
                    for (int i_RightNZ = right._rowPointers[i_LC]; i_RightNZ < right._rowPointers[i_LC + 1]; i_RightNZ++)
                    {
                        values.Add(left._values[i_LeftNZ] * right._values[i_RightNZ]);
                        columnIndices.Add(right._columnIndices[i_RightNZ]);
                    }
                }

                for (int i_LeftNZ = left._rowPointers[i_R] + 1; i_LeftNZ < left._rowPointers[i_R + 1]; i_LeftNZ++)
                {
                    int i_LC = left._columnIndices[i_LeftNZ];

                    int i_Pointer = rowPointers[i_R]; // To place the potential new value in the list of values and column indices

                    // Insert or add the values whose column is below or equal to the last row column
                    int i_RightNZ = right._rowPointers[i_LC];
                    for (; i_RightNZ < right._rowPointers[i_LC + 1]; i_RightNZ++)
                    {
                        int i_C = right._columnIndices[i_RightNZ];
                        if (i_C < columnIndices[i_Pointer])
                        {
                            values.Insert(i_Pointer, left._values[i_LeftNZ] * right._values[i_RightNZ]);
                            columnIndices.Insert(i_Pointer, right._columnIndices[i_RightNZ]);

                            i_Pointer++;
                        }
                        else if (i_C == columnIndices[i_Pointer])
                        {
                            values[i_Pointer] += left._values[i_LeftNZ] * right._values[i_RightNZ];

                            i_Pointer++;
                            if (i_Pointer == values.Count) { break; }
                        }
                        else
                        {
                            i_Pointer++;
                            if (i_Pointer == values.Count) { break; }

                            i_RightNZ--;
                        }
                    }

                    // Add the values whose column is strictly after the last row column
                    for (; i_RightNZ < right._rowPointers[i_LC + 1]; i_RightNZ++)
                    {
                        values.Add(left._values[i_LeftNZ] * right._values[i_RightNZ]);
                        columnIndices.Add(right._columnIndices[i_RightNZ]);
                    }
                }

                rowPointers[i_R + 1] = values.Count;
            }

            return new CompressedRow(left.RowCount, left.ColumnCount, values, columnIndices, rowPointers);
        }


        /******************** Sparse Matrix Embedding ********************/

        /// <summary>
        /// Computes the addition of a <see cref="CompressedRow"/> with a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> <see cref="CompressedRow"/> for the addition. </param>
        /// <param name="right"> <see cref="SparseMatrix"/> for the addition. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the addition. </returns>
        public static CompressedRow Add(CompressedRow left, SparseMatrix right)
        {
            if (right is CompressedRow crsRight) { return CompressedRow.Add(left, crsRight); }
            else if(right is CompressedColumn ccsRight) { return CompressedRow.Add(left, ccsRight); }
            else { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Computes the addition of a <see cref="SparseMatrix"/> with a <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> <see cref="SparseMatrix"/> for the addition. </param>
        /// <param name="right"> <see cref="CompressedRow"/> for the addition. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the addition. </returns>
        public static CompressedRow Add(SparseMatrix left, CompressedRow right)
        {
            if (left is CompressedRow crsLeft) { return CompressedRow.Add(crsLeft, right); }
            else if(left is CompressedRow ccsLeft) { return CompressedRow.Add(ccsLeft, right); }
            else { throw new NotImplementedException(); }
        }


        /// <summary>
        /// Computes the subtraction of a <see cref="CompressedRow"/> with a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> <see cref="CompressedRow"/> to subtract. </param>
        /// <param name="right"> <see cref="SparseMatrix"/> to subtract with. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the subtraction. </returns>
        public static CompressedRow Subtract(CompressedRow left, SparseMatrix right)
        {
            if (right is CompressedRow crsRight) { return CompressedRow.Subtract(left, crsRight); }
            else if (right is CompressedColumn ccsRight) { return CompressedRow.Subtract(left, ccsRight); }
            else { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Computes the subtraction of a <see cref="SparseMatrix"/> with a <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> <see cref="SparseMatrix"/> to subtract. </param>
        /// <param name="right"> <see cref="CompressedRow"/> to subtract with. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the subtraction. </returns>
        public static CompressedRow Subtract(SparseMatrix left, CompressedRow right)
        {
            if (left is CompressedRow crsLeft) { return CompressedRow.Subtract(crsLeft, right); }
            else if (left is CompressedRow ccsLeft) { return CompressedRow.Subtract(ccsLeft, right); }
            else { throw new NotImplementedException(); }
        }


        /// <summary>
        /// Computes the multiplication of a <see cref="CompressedRow"/> with a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> <see cref="CompressedRow"/> for the multiplication. </param>
        /// <param name="right"> <see cref="SparseMatrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the multiplication. </returns>
        public static CompressedRow Multiply(CompressedRow left, SparseMatrix right)
        {
            if (right is CompressedRow crsRight) { return CompressedRow.Multiply(left, crsRight); }
            else if (right is CompressedColumn ccsRight) { return CompressedRow.Multiply(left, ccsRight); }
            else { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Computes the multiplication of a <see cref="SparseMatrix"/> with a <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> <see cref="SparseMatrix"/> for the multiplication. </param>
        /// <param name="right"> <see cref="CompressedRow"/> for the multiplication. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the multiplication. </returns>
        public static CompressedRow Multiply(SparseMatrix left, CompressedRow right)
        {
            if (left is CompressedRow crsLeft) { return CompressedRow.Multiply(crsLeft, right); }
            else if (left is CompressedRow ccsLeft) { return CompressedRow.Multiply(ccsLeft, right); }
            else { throw new NotImplementedException(); }
        }


        /******************** CompressedRow Embedding ********************/

        /// <summary>
        /// Computes the addition of a <see cref="CompressedRow"/> with a <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> <see cref="CompressedRow"/> for the addition. </param>
        /// <param name="right"> <see cref="CompressedColumn"/> for the addition. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the addition. </returns>
        public static CompressedRow Add(CompressedRow left, CompressedColumn right)
        {
            CompressedRow ccs = right.ToCompressedRow();

            return CompressedRow.Add(left, ccs);
        }

        /// <summary>
        /// Computes the addition of a <see cref="CompressedColumn"/> with a <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> <see cref="CompressedColumn"/> for the addition. </param>
        /// <param name="right"> <see cref="CompressedRow"/> for the addition. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the addition. </returns>
        public static CompressedRow Add(CompressedColumn left, CompressedRow right)
        {
            CompressedRow ccs = left.ToCompressedRow();

            return CompressedRow.Add(ccs, right);
        }


        /// <summary>
        /// Computes the subtraction of a <see cref="CompressedRow"/> with a <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> <see cref="CompressedRow"/> to subtract. </param>
        /// <param name="right"> <see cref="CompressedColumn"/> to subtract with. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the subtraction. </returns>
        public static CompressedRow Subtract(CompressedRow left, CompressedColumn right)
        {
            CompressedRow ccs = right.ToCompressedRow();

            return CompressedRow.Subtract(left, ccs);
        }

        /// <summary>
        /// Computes the subtraction of a <see cref="CompressedColumn"/> with a <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> <see cref="CompressedColumn"/> to subtract. </param>
        /// <param name="right"> <see cref="CompressedRow"/> to subtract with. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the subtraction. </returns>
        public static CompressedRow Subtract(CompressedColumn left, CompressedRow right)
        {
            CompressedRow ccs = left.ToCompressedRow();

            return CompressedRow.Subtract(ccs, right);
        }


        /// <summary>
        /// Computes the multiplication of a <see cref="CompressedRow"/> with a <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> <see cref="CompressedRow"/> for the multiplication. </param>
        /// <param name="right"> <see cref="CompressedColumn"/> for the multiplication. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the multiplication. </returns>
        public static CompressedRow Multiply(CompressedRow left, CompressedColumn right)
        {
            CompressedRow ccs = right.ToCompressedRow();

            return CompressedRow.Multiply(left, ccs);
        }

        /// <summary>
        /// Computes the multiplication of a <see cref="CompressedColumn"/> with a <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> <see cref="CompressedColumn"/> for the multiplication. </param>
        /// <param name="right"> <see cref="CompressedRow"/> for the multiplication. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the multiplication. </returns>
        public static CompressedRow Multiply(CompressedColumn left, CompressedRow right)
        {
            CompressedRow ccs = left.ToCompressedRow();

            return CompressedRow.Multiply(ccs, right);
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Computes the scalar multiplication of a <see cref="CompressedRow"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="factor"> <see cref="double"/>-precision real number. </param>
        /// <param name="operand"> <see cref="CompressedRow"/> to multiply. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the scalar multiplication. </returns>
        public static CompressedRow Multiply(double factor, CompressedRow operand)
        {
            List<double> values = new List<double>(operand.NonZeroCount);
            for (int i_NZ = 0; i_NZ < operand.NonZeroCount; i_NZ++)
            {
                values.Add(factor * operand._values[i_NZ]);
            }

            List<int> columnIndices = new List<int>(operand._columnIndices);

            int[] rowPointers = new int[operand._rowPointers.Length];
            for (int i = 0; i < rowPointers.Length; i++)
            {
                rowPointers[i] = operand._rowPointers[i];
            }

            return new CompressedRow(operand.RowCount, operand.ColumnCount, values, columnIndices, rowPointers);
        }

        /// <summary>
        /// Computes the scalar multiplication of a <see cref="CompressedRow"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="factor"> <see cref="double"/>-precision real number. </param>
        /// <param name="operand"> <see cref="CompressedRow"/> to multiply. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the scalar multiplication. </returns>
        public static CompressedRow Multiply(CompressedRow operand, double factor)
        {
            List<double> values = new List<double>(operand.NonZeroCount);
            for (int i_NZ = 0; i_NZ < operand.NonZeroCount; i_NZ++)
            {
                values.Add(operand._values[i_NZ] * factor);
            }

            List<int> columnIndices = new List<int>(operand._columnIndices);

            int[] rowPointers = new int[operand._rowPointers.Length];
            for (int i = 0; i < rowPointers.Length; i++)
            {
                rowPointers[i] = operand._rowPointers[i];
            }

            return new CompressedRow(operand.RowCount, operand.ColumnCount, values, columnIndices, rowPointers);
        }


        /// <summary>
        /// Computes the scalar division of a <see cref="CompressedRow"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="operand"> <see cref="CompressedRow"/> to divide. </param>
        /// <param name="divisor"> <see cref="double"/>-precision real number to divide with. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the scalar division. </returns>
        public static CompressedRow Divide(CompressedRow operand, double divisor)
        {
            List<double> values = new List<double>(operand.NonZeroCount);
            for (int i_NZ = 0; i_NZ < operand.NonZeroCount; i_NZ++)
            {
                values.Add(operand._values[i_NZ] / divisor);
            }

            List<int> columnIndices = new List<int>(operand._columnIndices);

            int[] rowPointers = new int[operand._rowPointers.Length];
            for (int i = 0; i < rowPointers.Length; i++)
            {
                rowPointers[i] = operand._rowPointers[i];
            }

            return new CompressedRow(operand.RowCount, operand.ColumnCount, values, columnIndices, rowPointers);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the non-zero values of the current <see cref="CompressedRow"/> sparse matrix.
        /// </summary>
        /// <returns> The non-zero values of the current <see cref="CompressedRow"/> sparse matrix. </returns>
        public double[] GetValues()
        {
            return _values.ToArray();
        }

        /// <summary>
        /// Returns the non-zero value of the current <see cref="CompressedRow"/> sparse matrix at a given index.
        /// </summary>
        /// <param name="index"> Index of the non-zero value to get.</param>
        /// <returns> The non-zero value of the current <see cref="CompressedRow"/> sparse matrix at the given index. </returns>
        public double GetValues(int index)
        {
            return _values[index];
        }


        /// <summary>
        /// Returns the column indices of the current <see cref="CompressedRow"/> sparse matrix.
        /// </summary>
        /// <returns> The column indices of the current <see cref="CompressedRow"/> sparse matrix. </returns>
        public int[] GetColumnIndices()
        {
            return _columnIndices.ToArray();
        }

        /// <summary>
        /// Returns the column index of the current <see cref="CompressedRow"/> sparse matrix at a given index.
        /// </summary>
        /// <param name="index"> Index of the column index to get.</param>
        /// <returns> The column index of the current <see cref="CompressedRow"/> sparse matrix at the given index. </returns>
        public int GetColumnIndex(int index)
        {
            return _columnIndices[index];
        }


        /// <summary>
        /// Returns the row pointers of the current <see cref="CompressedRow"/> sparse matrix.
        /// </summary>
        /// <returns> The row pointers of the current <see cref="CompressedRow"/> sparse matrix. </returns>
        public int[] GetRowPointers()
        {
            return _rowPointers.Clone() as int[]; 
        }

        /// <summary>
        /// Returns the row pointers of the current <see cref="CompressedRow"/> sparse matrix.
        /// </summary>
        /// <param name="index"> Index of the row pointers to get.</param>
        /// <returns> The row pointers of the current <see cref="CompressedRow"/> sparse matrix. </returns>
        public int GetRowPointer(int index)
        {
            return _rowPointers[index];
        }


        public override void Transpose()
        {
            ww;
        }


        public CompressedColumn ToCompressedColumn()
        {
            int[] columnHelper = new int[ColumnCount];
            // Get the number of elements per column
            for (int i_LefttNZ = 0; i_LefttNZ < NonZeroCount; i_LefttNZ++)
            {
                columnHelper[_columnIndices[i_LefttNZ]]++;
            }

            // Creates the column pointer
            int[] columnPointers = new int[ColumnCount + 1];
            columnPointers[0] = 0;

            for (int i_C = 0; i_C < columnPointers.Length; i_C++)
            {
                columnPointers[i_C + 1] = columnPointers[i_C] + columnHelper[i_C];
                columnHelper[i_C] = 0;
            }

            // Creates the value and 
            double[] values = new double[NonZeroCount];
            int[] rowIndices = new int[NonZeroCount];

            int i_LeftNZ = _rowPointers[0];
            for (int i_R = 0; i_R < ColumnCount; i_R++)
            {
                for (; i_LeftNZ < _rowPointers[i_R + 1]; i_LeftNZ++)
                {
                    int i_LC = _columnIndices[i_LeftNZ];
                    int i_Pointer = columnPointers[i_LC] + columnHelper[i_LC];

                    values[i_Pointer] = _values[i_LeftNZ];
                    values[i_Pointer] = i_R;
                    columnHelper[i_LC]++;
                }
            }

            return new CompressedColumn(RowCount, ColumnCount, values, rowIndices, columnPointers);
        }

        #endregion

        #region Other Methods

        /******************** Algebraic Additive Group ********************/

        /// <summary>
        /// Computes the addition of the current <see cref="CompressedRow"/> with another <see cref="Matrix"/>.
        /// </summary>
        /// <param name="right"> <see cref="Matrix"/> to add with on the right. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the addition. </returns>
        protected override Matrix Add(Matrix right)
        {
            if (right is DenseMatrix denseRight) { return DenseMatrix.Add(this, denseRight); }
            else if (right is SparseMatrix sparseRight) { return CompressedRow.Add(this, sparseRight); }
            else { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Computes the subtraction of the current <see cref="CompressedRow"/> with another <see cref="Matrix"/>.
        /// </summary>
        /// <param name="right"> <see cref="Matrix"/> to subtract with on the right. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the subtraction. </returns>
        protected override Matrix Subtract(Matrix right)
        {
            if (right is DenseMatrix denseRight) { return DenseMatrix.Subtract(this, denseRight); }
            else if (right is SparseMatrix sparseRight) { return CompressedRow.Subtract(this, sparseRight); }
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
            return new CompressedRow(RowCount, ColumnCount);
        }

        /******************** Algebraic Multiplicative SemiGroup ********************/

        /// <summary>
        /// Computes the multiplication of the current <see cref="CompressedRow"/> with another <see cref="Matrix"/>.
        /// </summary>
        /// <param name="right"> <see cref="Matrix"/> to multiply with on the right. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the addition. </returns>
        protected override Matrix Multiply(Matrix right)
        {
            if (right is DenseMatrix denseRight) { return DenseMatrix.Multiply(this, denseRight); }
            else if (right is SparseMatrix sparseRight) { return CompressedRow.Multiply(this, sparseRight); }
            else { throw new NotImplementedException(); }
        }

        /******************** Group Action ********************/

        /// <summary>
        /// Computes the scalar multiplication the current <see cref="CompressedRow"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="factor"> <see cref="double"/>-precision real number. </param>
        /// <returns> The new <see cref="CompressedRow"/> as a <see cref="Matrix"/> resulting from the scalar multiplication. </returns>
        protected override Matrix Multiply(double factor)
        {
            return CompressedRow.Multiply(this, factor);
        }

        /// <summary>
        /// Computes the scalar division of the current <see cref="CompressedRow"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="divisor"> <see cref="double"/>-precision real number to divide with. </param>
        /// <returns> The new <see cref="CompressedRow"/> as a <see cref="Matrix"/> resulting from the scalar division. </returns>
        protected override Matrix Divide(double divisor)
        {
            return CompressedRow.Divide(this, divisor);
        }

        #endregion
    }
}
