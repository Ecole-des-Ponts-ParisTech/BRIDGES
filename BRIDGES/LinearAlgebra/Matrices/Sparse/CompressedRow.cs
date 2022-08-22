﻿using System;
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
        private int[] _rowPointers;

        /// <summary>
        /// Row indices associated with the non-zero values.
        /// </summary>
        private List<int> _columnIndices;

        /// <summary>
        /// Non-zero values of the matrix
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
                for (int i_NZ = _rowPointers[row]; i_NZ < _rowPointers[row + 1]; i_NZ++)
                {
                    if (_columnIndices[i_NZ] == column) { return _values[i_NZ]; }
                    else if (column < _columnIndices[i_NZ]) { break; }
                }
                return 0.0;
            }
        }

        #endregion

        #region Contructors

        /// <summary>
        /// Initialises a new instance of the <see cref="CompressedRow"/> class by defining its size,
        /// and by giving its values in a <see cref="Storage.DictionaryOfKeys"/>.
        /// </summary>
        /// <param name="rowCount"> Number of rows of the <see cref="CompressedRow"/>. </param>
        /// <param name="columnCount"> Number of columns of the <see cref="CompressedRow"/>. </param>
        /// <param name="dok"> Values of the <see cref="CompressedRow"/>. </param>
        public CompressedRow(int rowCount, int columnCount, Storage.DictionaryOfKeys dok)
        {
            _rowCount = rowCount;
            _columnCount = columnCount;


            List<(int, double)>[] rows = new List<(int, double)>[columnCount];
            for (int i_R = 0; i_R < rowCount; i_R++) { rows[i_R] = new List<(int, double)>(); }

            // Distribute among the columns
            IEnumerator<KeyValuePair<(int, int), double>> kvpEnumerator = dok.GetNonZeros();
            try
            {
                while (kvpEnumerator.MoveNext())
                {
                    KeyValuePair<(int, int), double> kvp = kvpEnumerator.Current;

                    rows[kvp.Key.Item1].Add((kvp.Key.Item2, kvp.Value));
                }
            }
            finally { kvpEnumerator.Dispose(); }

            // Sorts each columns with regard to the row index
            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                rows[i_R].Sort((x, y) => x.Item1.CompareTo(y.Item1));
            }

            // Creates the new column pointer, the row index list and value lists
            _rowPointers = new int[rowCount + 1];
            _columnIndices = new List<int>(NonZeroCount);
            _values = new List<double>(NonZeroCount);

            _rowPointers[0] = 0;
            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                List<(int, double)> row = rows[i_R];
                int count = row.Count;

                _rowPointers[i_R + 1] = _rowPointers[i_R] + count;

                for (int i_NZ = 0; i_NZ < count; i_NZ++)
                {
                    _columnIndices.Add(row[i_NZ].Item1);
                    _values.Add(row[i_NZ].Item2);
                }
            }
        }


        /// <summary>
        /// Initialises a new instance of the <see cref="CompressedRow"/> class by defining its number of row and column.
        /// </summary>
        /// <param name="rowCount"> Number of rows of the <see cref="CompressedRow"/>. </param>
        /// <param name="columnCount"> Number of columns of the <see cref="CompressedRow"/>. </param>
        /// <param name="values"> Non-zero values of the <see cref="CompressedRow"/>. </param>
        /// <param name="columnIndices"> Column indices of the <see cref="CompressedRow"/>.</param>
        /// <param name="rowPointers"> Row pointers of the <see cref="CompressedRow"/>. </param>
        internal CompressedRow(int rowCount, int columnCount, int[] rowPointers, List<int> columnIndices, List<double> values)
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

            _rowPointers = rowPointers;
            _columnIndices = columnIndices;
            _values = values;

        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Returns the neutral <see cref="CompressedRow"/> for the addition. 
        /// </summary>
        /// <param name="rowCount"> Number of rows of the <see cref="CompressedRow"/>. </param>
        /// <param name="columnCount"> Number of columns of the <see cref="CompressedRow"/>. </param>
        /// <returns> The <see cref="CompressedRow"/> of the given size, with zeros on every coordinates. </returns>
        public static new CompressedRow Zero(int rowCount, int columnCount)
        {
            int[] rowPointers = new int[rowCount + 1];
            List<int> columnIndices = new List<int>();
            List<double> values = new List<double>();

            return new CompressedRow(rowCount, columnCount, rowPointers, columnIndices, values);
        }

        /// <summary>
        /// Returns the neutral <see cref="CompressedRow"/> for the multiplication. 
        /// </summary>
        /// <param name="size"> Number of rows and columns of the <see cref="CompressedRow"/>. </param>
        /// <returns> The <see cref="CompressedRow"/> of the given size, with ones on the diagonal and zeros elsewhere. </returns>
        public static new CompressedRow Identity(int size)
        {
            int[] rowPointers = new int[size + 1];
            List<int> columnIndices = new List<int>();
            List<double> values = new List<double>();

            for (int i = 0; i < size; i++)
            {
                rowPointers[i] = i;
                columnIndices[i] = i;
                values.Add(1.0);
            }
            rowPointers[size] = size;

            return new CompressedRow(size, size, rowPointers, columnIndices, values);
        }

        #endregion

        #region Static Methods

        /******************** Algebraic Near Ring ********************/

        /// <summary>
        /// Computes the addition of two <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedRow"/> for the addition. </param>
        /// <param name="right"> Right <see cref="CompressedRow"/> for the addition. </param>
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

            return new CompressedRow(left.RowCount, left.ColumnCount, rowPointers, columnIndices, values);
        }

        /// <summary>
        /// Computes the subtraction of two <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedRow"/> to subtract. </param>
        /// <param name="right"> Right <see cref="CompressedRow"/> to subtract with. </param>
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

            return new CompressedRow(left.RowCount, left.ColumnCount, rowPointers, columnIndices, values);
        }


        /// <summary>
        /// Computes the multiplication of two <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedRow"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="CompressedRow"/> for the multiplication. </param>
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

            return new CompressedRow(left.RowCount, left.ColumnCount, rowPointers, columnIndices, values);
        }


        /******************** Sparse Matrix Embedding ********************/

        /// <summary>
        /// Computes the addition of a <see cref="CompressedRow"/> with a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedRow"/> for the addition. </param>
        /// <param name="right"> Right <see cref="SparseMatrix"/> for the addition. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the addition. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The addition of a <see cref="CompressedRow"/> with the right matrix type as a <see cref="SparseMatrix"/> is not implemented.
        /// </exception>
        public static CompressedRow Add(CompressedRow left, SparseMatrix right)
        {
            if (right is CompressedRow crsRight) { return CompressedRow.Add(left, crsRight); }
            else if(right is CompressedColumn ccsRight) { return CompressedRow.Add(left, ccsRight); }
            else { throw new NotImplementedException($"The addition of a {left.GetType()} and a {right.GetType()} as a SparseMatrix is not implemented."); }
        }

        /// <summary>
        /// Computes the addition of a <see cref="SparseMatrix"/> with a <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix"/> for the addition. </param>
        /// <param name="right"> Right <see cref="CompressedRow"/> for the addition. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the addition. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The addition of the left matrix type as a <see cref="SparseMatrix"/> with a <see cref="CompressedRow"/> is not implemented.
        /// </exception>
        public static CompressedRow Add(SparseMatrix left, CompressedRow right)
        {
            if (left is CompressedRow crsLeft) { return CompressedRow.Add(crsLeft, right); }
            else if(left is CompressedRow ccsLeft) { return CompressedRow.Add(ccsLeft, right); }
            else { throw new NotImplementedException($"The addition of a {left.GetType()} as a SparseMatrix and a {right.GetType()} is not implemented."); }
        }


        /// <summary>
        /// Computes the subtraction of a <see cref="CompressedRow"/> with a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedRow"/> to subtract. </param>
        /// <param name="right"> Right <see cref="SparseMatrix"/> to subtract with. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the subtraction. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The subtracation of a <see cref="CompressedRow"/> with the right matrix type as a <see cref="SparseMatrix"/> is not implemented.
        /// </exception>
        public static CompressedRow Subtract(CompressedRow left, SparseMatrix right)
        {
            if (right is CompressedRow crsRight) { return CompressedRow.Subtract(left, crsRight); }
            else if (right is CompressedColumn ccsRight) { return CompressedRow.Subtract(left, ccsRight); }
            else { throw new NotImplementedException($"The subtraction of a {left.GetType()} and a {right.GetType()} as a SparseMatrix is not implemented."); }
        }

        /// <summary>
        /// Computes the subtraction of a <see cref="SparseMatrix"/> with a <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix"/> to subtract. </param>
        /// <param name="right"> Right <see cref="CompressedRow"/> to subtract with. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the subtraction. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The subtraction of the left matrix type as a <see cref="SparseMatrix"/> with a <see cref="CompressedRow"/> is not implemented.
        /// </exception>
        public static CompressedRow Subtract(SparseMatrix left, CompressedRow right)
        {
            if (left is CompressedRow crsLeft) { return CompressedRow.Subtract(crsLeft, right); }
            else if (left is CompressedRow ccsLeft) { return CompressedRow.Subtract(ccsLeft, right); }
            else { throw new NotImplementedException($"The subtraction of a {left.GetType()} as a SparseMatrix and a {right.GetType()} is not implemented."); }
        }


        /// <summary>
        /// Computes the multiplication of a <see cref="CompressedRow"/> with a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedRow"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="SparseMatrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The multiplication of a <see cref="CompressedRow"/> with the right matrix type as a <see cref="SparseMatrix"/> is not implemented.
        /// </exception>
        public static CompressedRow Multiply(CompressedRow left, SparseMatrix right)
        {
            if (right is CompressedRow crsRight) { return CompressedRow.Multiply(left, crsRight); }
            else if (right is CompressedColumn ccsRight) { return CompressedRow.Multiply(left, ccsRight); }
            else { throw new NotImplementedException($"The multiplication of a {left.GetType()} and a {right.GetType()} as a SparseMatrix is not implemented."); }
        }

        /// <summary>
        /// Computes the multiplication of a <see cref="SparseMatrix"/> with a <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="CompressedRow"/> for the multiplication. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The multiplication of the left matrix type as a <see cref="SparseMatrix"/> with a <see cref="CompressedRow"/> is not implemented.
        /// </exception>
        public static CompressedRow Multiply(SparseMatrix left, CompressedRow right)
        {
            if (left is CompressedRow crsLeft) { return CompressedRow.Multiply(crsLeft, right); }
            else if (left is CompressedRow ccsLeft) { return CompressedRow.Multiply(ccsLeft, right); }
            else { throw new NotImplementedException($"The multiplication of a {left.GetType()} as a SparseMatrix and a {right.GetType()} is not implemented."); }
        }


        /******************** CompressedRow Embedding ********************/

        /// <summary>
        /// Computes the addition of a <see cref="CompressedRow"/> with a <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedRow"/> for the addition. </param>
        /// <param name="right"> Right <see cref="CompressedColumn"/> for the addition. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the addition. </returns>
        public static CompressedRow Add(CompressedRow left, CompressedColumn right)
        {
            CompressedRow ccs = right.ToCompressedRow();

            return CompressedRow.Add(left, ccs);
        }

        /// <summary>
        /// Computes the addition of a <see cref="CompressedColumn"/> with a <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedColumn"/> for the addition. </param>
        /// <param name="right"> Right <see cref="CompressedRow"/> for the addition. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the addition. </returns>
        public static CompressedRow Add(CompressedColumn left, CompressedRow right)
        {
            CompressedRow ccs = left.ToCompressedRow();

            return CompressedRow.Add(ccs, right);
        }


        /// <summary>
        /// Computes the subtraction of a <see cref="CompressedRow"/> with a <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedRow"/> to subtract. </param>
        /// <param name="right"> Right <see cref="CompressedColumn"/> to subtract with. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the subtraction. </returns>
        public static CompressedRow Subtract(CompressedRow left, CompressedColumn right)
        {
            CompressedRow ccs = right.ToCompressedRow();

            return CompressedRow.Subtract(left, ccs);
        }

        /// <summary>
        /// Computes the subtraction of a <see cref="CompressedColumn"/> with a <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedColumn"/> to subtract. </param>
        /// <param name="right"> Right <see cref="CompressedRow"/> to subtract with. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the subtraction. </returns>
        public static CompressedRow Subtract(CompressedColumn left, CompressedRow right)
        {
            CompressedRow ccs = left.ToCompressedRow();

            return CompressedRow.Subtract(ccs, right);
        }


        /// <summary>
        /// Computes the multiplication of a <see cref="CompressedRow"/> with a <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedRow"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="CompressedColumn"/> for the multiplication. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the multiplication. </returns>
        public static CompressedRow Multiply(CompressedRow left, CompressedColumn right)
        {
            CompressedRow ccs = right.ToCompressedRow();

            return CompressedRow.Multiply(left, ccs);
        }

        /// <summary>
        /// Computes the multiplication of a <see cref="CompressedColumn"/> with a <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedColumn"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="CompressedRow"/> for the multiplication. </param>
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

            return new CompressedRow(operand.RowCount, operand.ColumnCount, rowPointers, columnIndices, values);
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

            return new CompressedRow(operand.RowCount, operand.ColumnCount, rowPointers, columnIndices, values);
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

            return new CompressedRow(operand.RowCount, operand.ColumnCount, rowPointers, columnIndices, values);
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


        /// <inheritdoc/>
        public override void Transpose()
        {
            // Get the number of elements per column of the current matrix
            int[] columnHelper = new int[ColumnCount];
            for (int i_NZ = 0; i_NZ < NonZeroCount; i_NZ++)
            {
                columnHelper[_columnIndices[i_NZ]]++;
            }


            // Creates the new row pointer
            int[] rowPointers = new int[ColumnCount + 1];

            rowPointers[0] = 0;
            for (int i_R = 0; i_R < rowPointers.Length; i_R++)
            {
                rowPointers[i_R + 1] = rowPointers[i_R] + columnHelper[i_R];
                columnHelper[i_R] = 0;
            }


            // Creates the new column index and value lists
            int[] columnIndices = new int[NonZeroCount];
            double[] values = new double[NonZeroCount];

            int i_RowNZ = _rowPointers[0];
            for (int i_R = 0; i_R < RowCount; i_R++)
            {
                for (; i_RowNZ < _rowPointers[i_R + 1]; i_RowNZ++)
                {

                    int i_C = _columnIndices[i_RowNZ];
                    int i_Pointer = rowPointers[i_C] + columnHelper[i_C];

                    values[i_Pointer] = _values[i_RowNZ];
                    columnIndices[i_Pointer] = i_R;
                    columnHelper[i_C]++;
                }
            }
        }


        /// <summary>
        /// Converts a the current <see cref="CompressedRow"/> matrix into an equivalent <see cref="CompressedColumn"/> matrix.
        /// </summary>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the conversion. </returns>
        public CompressedColumn ToCompressedColumn()
        {
            // Get the number of elements per column
            int[] columnHelper = new int[ColumnCount];
            for (int i_NZ = 0; i_NZ < NonZeroCount; i_NZ++)
            {
                columnHelper[_columnIndices[i_NZ]]++;
            }


            // Creates the column pointer
            int[] columnPointers = new int[ColumnCount + 1];

            columnPointers[0] = 0;
            for (int i_C = 0; i_C < columnPointers.Length; i_C++)
            {
                columnPointers[i_C + 1] = columnPointers[i_C] + columnHelper[i_C];
                columnHelper[i_C] = 0;
            }


            // Creates the row index and value lists
            double[] values = new double[NonZeroCount];
            int[] rowIndices = new int[NonZeroCount];

            int i_RowNZ = _rowPointers[0];
            for (int i_R = 0; i_R < RowCount; i_R++)
            {
                for (; i_RowNZ < _rowPointers[i_R + 1]; i_RowNZ++)
                {
                    int i_C = _columnIndices[i_RowNZ];
                    int i_Pointer = columnPointers[i_C] + columnHelper[i_C];

                    values[i_Pointer] = _values[i_RowNZ];
                    rowIndices[i_Pointer] = i_R;
                    columnHelper[i_C]++;
                }
            }


            List<int> list_RowIndices = new List<int>(rowIndices);
            List<double> list_Values = new List<double>(values);

            return new CompressedColumn(RowCount, ColumnCount, columnPointers , list_RowIndices, list_Values);
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
    }
}
