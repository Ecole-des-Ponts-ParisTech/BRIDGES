using System;

using MNet_LinAlg = MathNet.Numerics.LinearAlgebra;


namespace BRIDGES.LinearAlgebra.Matrices
{
    /// <summary>
    /// Class defining a dense matrix.
    /// </summary>
    public class DenseMatrix : Matrix
    {
        #region Fields

        /// <summary>
        /// Dense matrix from Math.Net library.
        /// </summary>
        private MNet_LinAlg.Double.DenseMatrix _storedMatrix;

        #endregion

        #region Properties

        /// <inheritdoc/>
        public override int RowCount
        {
            get { return _storedMatrix.RowCount; }
        }

        /// <inheritdoc/>
        public override int ColumnCount
        {
            get { return _storedMatrix.ColumnCount; }
        }

        /// <inheritdoc/>
        public override double this[int row, int column]
        {
             get { return _storedMatrix[row, column]; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="DenseMatrix"/> class.
        /// </summary>
        private DenseMatrix() 
            : base()
        {
            /* Do nothing */
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DenseMatrix"/> class by defining its number of row and column.
        /// </summary>
        /// <param name="rowCount"> Number of rows of the <see cref="DenseMatrix"/>. </param>
        /// <param name="columnCount"> Number of columns of the <see cref="DenseMatrix"/>. </param>
        public DenseMatrix(int rowCount, int columnCount)
            : base()
        {
            // Instanciate fields
            MNet_LinAlg.Double.DenseMatrix.Create(rowCount, columnCount, 0.0);
        }

        #endregion

        #region Static Methods

        /******************** Algebraic Near Ring ********************/

        /// <summary>
        /// Computes the addition of two <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="left"> <see cref="DenseMatrix"/> for the addition. </param>
        /// <param name="right"> <see cref="DenseMatrix"/> for the addition. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the addition. </returns>
        public static DenseMatrix Add(DenseMatrix left, DenseMatrix right)
        {
            DenseMatrix matrix = new DenseMatrix();

            matrix._storedMatrix = left._storedMatrix.Add(right._storedMatrix) as MNet_LinAlg.Double.DenseMatrix;

            return matrix;
        }

        /// <summary>
        /// Computes the subtraction of two <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="left"> <see cref="DenseMatrix"/> to subtract. </param>
        /// <param name="right"> <see cref="DenseMatrix"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the subtraction. </returns>
        public static DenseMatrix Subtract(DenseMatrix left, DenseMatrix right)
        {
            DenseMatrix matrix = new DenseMatrix();

            matrix._storedMatrix = left._storedMatrix.Subtract(right._storedMatrix) as MNet_LinAlg.Double.DenseMatrix;

            return matrix;
        }


        /// <summary>
        /// Computes the multiplication of two <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="left"> <see cref="DenseMatrix"/> for the multiplication. </param>
        /// <param name="right"> <see cref="DenseMatrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the multiplication. </returns>
        public static DenseMatrix Multiply(DenseMatrix left, DenseMatrix right)
        {
            DenseMatrix matrix = new DenseMatrix();

            matrix._storedMatrix = left._storedMatrix.Multiply(right._storedMatrix) as MNet_LinAlg.Double.DenseMatrix;

            return matrix;
        }


        /******************** Sparse Matrix Embedding ********************/

        /// <summary>
        /// Computes the addition of a <see cref="DenseMatrix"/> with a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> <see cref="DenseMatrix"/> for the addition. </param>
        /// <param name="right"> <see cref="SparseMatrix"/> for the addition. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the addition. </returns>
        public static DenseMatrix Add(DenseMatrix left, SparseMatrix right)
        {
            if (right is Sparse.CompressedColumn ccsRight) { return DenseMatrix.Add(left, ccsRight); }
            else if (right is Sparse.CompressedRow crsRight) { return DenseMatrix.Add(left, crsRight); }
            else { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Computes the addition of a <see cref="SparseMatrix"/> with a <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="left"> <see cref="SparseMatrix"/> for the addition. </param>
        /// <param name="right"> <see cref="DenseMatrix"/> for the addition. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the addition. </returns>
        public static DenseMatrix Add(SparseMatrix left, DenseMatrix right)
        {
            if (left is Sparse.CompressedColumn ccsLeft) { return DenseMatrix.Add(ccsLeft, right); }
            else if (left is Sparse.CompressedRow crsLeft) { return DenseMatrix.Add(crsLeft, right); }
            else { throw new NotImplementedException(); }
        }


        /// <summary>
        /// Computes the subtraction of a <see cref="DenseMatrix"/> with a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> <see cref="DenseMatrix"/> to subtract. </param>
        /// <param name="right"> <see cref="SparseMatrix"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the subtraction. </returns>
        public static DenseMatrix Subtract(DenseMatrix left, SparseMatrix right)
        {
            if (right is Sparse.CompressedColumn ccsRight) { return DenseMatrix.Subtract(left, ccsRight); }
            else if (right is Sparse.CompressedRow crsRight) { return DenseMatrix.Subtract(left, crsRight); }
            else { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Computes the subtraction of a <see cref="SparseMatrix"/> with a <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="left"> <see cref="SparseMatrix"/> to subtract. </param>
        /// <param name="right"> <see cref="DenseMatrix"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the subtraction. </returns>
        public static DenseMatrix Subtract(SparseMatrix left, DenseMatrix right)
        {
            if (left is Sparse.CompressedColumn ccsLeft) { return DenseMatrix.Subtract(ccsLeft, right); }
            else if (left is Sparse.CompressedRow crsLeft) { return DenseMatrix.Subtract(crsLeft, right); }
            else { throw new NotImplementedException(); }
        }


        /// <summary>
        /// Computes the multiplication of a <see cref="DenseMatrix"/> with a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> <see cref="DenseMatrix"/> for the multiplication. </param>
        /// <param name="right"> <see cref="SparseMatrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the multiplication. </returns>
        public static DenseMatrix Multiply(DenseMatrix left, SparseMatrix right)
        {
            if (right is Sparse.CompressedColumn ccsRight) { return DenseMatrix.Multiply(left, ccsRight); }
            else if (right is Sparse.CompressedRow crsRight) { return DenseMatrix.Multiply(left, crsRight); }
            else { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Computes the multiplication of a <see cref="SparseMatrix"/> with a <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="left"> <see cref="SparseMatrix"/> for the multiplication. </param>
        /// <param name="right"> <see cref="DenseMatrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the multiplication. </returns>
        public static DenseMatrix Multiply(SparseMatrix left, DenseMatrix right)
        {
            if (left is Sparse.CompressedColumn ccsLeft) { return DenseMatrix.Multiply(ccsLeft, right); }
            else if (left is Sparse.CompressedRow crsLeft) { return DenseMatrix.Multiply(crsLeft, right); }
            else { throw new NotImplementedException(); }
        }


        /******************** CompressedColumn Embedding ********************/

        /// <summary>
        /// Computes the addition of a <see cref="DenseMatrix"/> with a <see cref="Sparse.CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> <see cref="DenseMatrix"/> for the addition. </param>
        /// <param name="right"> <see cref="Sparse.CompressedColumn"/> for the addition. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the addition. </returns>
        public static DenseMatrix Add(DenseMatrix left, Sparse.CompressedColumn right)
        {
            // Verifications
            if( left.ColumnCount != right.ColumnCount || left.RowCount != right.RowCount)
            { 
                throw new ArgumentException("The matrices do not have the same size."); 
            }

            DenseMatrix result = new DenseMatrix();

            result._storedMatrix = left._storedMatrix.Clone() as MNet_LinAlg.Double.DenseMatrix;

            int[] columnPointers = right.GetColumnPointers();
            int[] rowIndices = right.GetRowIndices();
            double[] values = right.GetValues();

            // Iterate on the columns of right
            for (int i_C = 0; i_C < right.ColumnCount; i_C++)
            {
                // Iterate on the non-zero values of the current right column
                for (int i = columnPointers[i_C]; i < columnPointers[i_C + 1]; i++)
                {
                    result._storedMatrix[rowIndices[i], i_C] += values[i];
                }
            }

            return result;
        }

        /// <summary>
        /// Computes the addition of a <see cref="Sparse.CompressedColumn"/> with a <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="left"> <see cref="Sparse.CompressedColumn"/> for the addition. </param>
        /// <param name="right"> <see cref="DenseMatrix"/> for the addition. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the addition. </returns>
        public static DenseMatrix Add(Sparse.CompressedColumn left, DenseMatrix right)
        {
            // Verifications
            if (left.ColumnCount != right.ColumnCount || left.RowCount != right.RowCount)
            {
                throw new ArgumentException("The matrices do not have the same size.");
            }

            DenseMatrix result = new DenseMatrix();

            result._storedMatrix = right._storedMatrix.Clone() as MNet_LinAlg.Double.DenseMatrix;

            int[] columnPointers = left.GetColumnPointers();
            int[] rowIndices = left.GetRowIndices();
            double[] values = left.GetValues();

            // Iterate on the columns of left
            for (int i_C = 0; i_C < left.ColumnCount; i_C++)
            {
                // Iterate on the non-zero values of the current left column
                for (int i = columnPointers[i_C]; i < columnPointers[i_C + 1]; i++)
                {
                    result._storedMatrix[rowIndices[i], i_C] += values[i];
                }
            }

            return result;
        }


        /// <summary>
        /// Computes the subtraction of a <see cref="DenseMatrix"/> with a <see cref="Sparse.CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> <see cref="DenseMatrix"/> to subtract. </param>
        /// <param name="right"> <see cref="Sparse.CompressedColumn"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the subtraction. </returns>
        public static DenseMatrix Subtract(DenseMatrix left, Sparse.CompressedColumn right)
        {
            // Verifications
            if (left.ColumnCount != right.ColumnCount || left.RowCount != right.RowCount)
            {
                throw new ArgumentException("The matrices do not have the same size.");
            }

            DenseMatrix result = new DenseMatrix();

            result._storedMatrix = left._storedMatrix.Clone() as MNet_LinAlg.Double.DenseMatrix;

            int[] columnPointers = right.GetColumnPointers();
            int[] rowIndices = right.GetRowIndices();
            double[] values = right.GetValues();

            // Iterate on the columns of right
            for (int i_C = 0; i_C < right.ColumnCount; i_C++)
            {
                // Iterate on the non-zero values of the current right column
                for (int i = columnPointers[i_C]; i < columnPointers[i_C + 1]; i++)
                {
                    result._storedMatrix[rowIndices[i], i_C] -= values[i];
                }
            }

            return result;
        }

        /// <summary>
        /// Computes the subtraction of a <see cref="Sparse.CompressedColumn"/> with a <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="left"> <see cref="Sparse.CompressedColumn"/> to subtract. </param>
        /// <param name="right"> <see cref="DenseMatrix"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the subtraction. </returns>
        public static DenseMatrix Subtract(Sparse.CompressedColumn left, DenseMatrix right)
        {
            // Verifications
            if (left.ColumnCount != right.ColumnCount || left.RowCount != right.RowCount)
            {
                throw new ArgumentException("The matrices do not have the same size.");
            }

            DenseMatrix result = new DenseMatrix();

            result._storedMatrix = right._storedMatrix.Multiply(-1.0) as MNet_LinAlg.Double.DenseMatrix;

            int[] columnPointers = left.GetColumnPointers();
            int[] rowIndices = left.GetRowIndices();
            double[] values = left.GetValues();

            // Iterate on the columns of left
            for (int i_C = 0; i_C < left.ColumnCount; i_C++)
            {
                // Iterate on the non-zero values of the current left column
                for (int i = columnPointers[i_C]; i < columnPointers[i_C + 1]; i++)
                {
                    result._storedMatrix[rowIndices[i], i_C] += values[i];
                }
            }

            return result;
        }


        /// <summary>
        /// Computes the multiplication of a <see cref="DenseMatrix"/> with a <see cref="Sparse.CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> <see cref="DenseMatrix"/> for the multiplication. </param>
        /// <param name="right"> <see cref="Sparse.CompressedColumn"/> for the multiplication. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the multiplication. </returns>
        public static DenseMatrix Multiply(DenseMatrix left, Sparse.CompressedColumn right)
        {
            // Verifications
            if (left.ColumnCount != right.RowCount)
            {
                throw new ArgumentException("The matrices size does not allow their multiplication.");
            }
 
            DenseMatrix result = new DenseMatrix(left.RowCount, right.ColumnCount);

            int[] columnPointers = right.GetColumnPointers();
            int[] rowIndices = right.GetRowIndices();
            double[] values = right.GetValues();
           
            // Iterate on the columns of right 
            for (int i_C = 0; i_C < right.ColumnCount; i_C++)
            {
                // Iterate on the rows of left
                for (int i_R = 0; i_R < left.RowCount; i_R++)
                {
                    double sum = 0.0;

                    // Iterate on the non-zero values of the current right column
                    for (int i_NZ = columnPointers[i_C]; i_NZ < columnPointers[i_C + 1]; i_NZ++)
                    {
                        sum += left[i_R, rowIndices[i_NZ]] * values[i_NZ];
                    }

                    result._storedMatrix[i_R, i_C] = sum;
                }
            }

            return result;
        }

        /// <summary>
        /// Computes the multiplication of a <see cref="Sparse.CompressedColumn"/> with a <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="left"> <see cref="Sparse.CompressedColumn"/> for the multiplication. </param>
        /// <param name="right"> <see cref="DenseMatrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the multiplication. </returns>
        public static DenseMatrix Multiply(Sparse.CompressedColumn left, DenseMatrix right)
        {
            // Verifications
            if (left.ColumnCount != right.RowCount)
            {
                throw new ArgumentException("The matrices size does not allow their multiplication.");
            }

            DenseMatrix result = new DenseMatrix(left.RowCount, right.ColumnCount);

            int[] columnPointers = left.GetColumnPointers();
            int[] rowIndices = left.GetRowIndices();
            double[] values = left.GetValues();

            // Iterate on the columns of left
            for (int i_LC = 0; i_LC < left.ColumnCount; i_LC++)
            {
                // Iterate on the non-zero values of the current left column
                for (int i_NZ = columnPointers[i_LC]; i_NZ < columnPointers[i_LC + 1]; i_NZ++)
                {
                    // Iterate on the columns of right
                    for (int i_C = 0; i_C < right.ColumnCount; i_C++)
                    {
                        result._storedMatrix[rowIndices[i_NZ], i_C] += values[i_NZ] * right[i_LC, i_C];
                    }
                }
            }

            return result;
        }


        /******************** CompressedRow Embedding ********************/

        /// <summary>
        /// Computes the addition of a <see cref="DenseMatrix"/> with a <see cref="Sparse.CompressedRow"/>.
        /// </summary>
        /// <param name="left"> <see cref="DenseMatrix"/> for the addition. </param>
        /// <param name="right"> <see cref="Sparse.CompressedRow"/> for the addition. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the addition. </returns>
        public static DenseMatrix Add(DenseMatrix left, Sparse.CompressedRow right)
        {
            // Verifications
            if (left.ColumnCount != right.ColumnCount || left.RowCount != right.RowCount)
            {
                throw new ArgumentException("The matrices do not have the same size.");
            }

            DenseMatrix result = new DenseMatrix();

            result._storedMatrix = left._storedMatrix.Clone() as MNet_LinAlg.Double.DenseMatrix;

            int[] columnIndices = right.GetColumnIndices();
            int[] rowPointers = right.GetRowPointers();
            double[] values = right.GetValues();

            // Iterate on the rows of right
            for (int i_R = 0; i_R < right.RowCount; i_R++)
            {
                // Iterate on the non-zero values of the current right row
                for (int i_NZ = rowPointers[i_R]; i_NZ < rowPointers[i_R + 1]; i_NZ++)
                {
                    result._storedMatrix[i_R, columnIndices[i_NZ]] += values[i_NZ];
                }
            }

            return result;
        }

        /// <summary>
        /// Computes the addition of a <see cref="Sparse.CompressedRow"/> with a <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="left"> <see cref="Sparse.CompressedRow"/> for the addition. </param>
        /// <param name="right"> <see cref="DenseMatrix"/> for the addition. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the addition. </returns>
        public static DenseMatrix Add(Sparse.CompressedRow left, DenseMatrix right)
        {
            // Verifications
            if (left.ColumnCount != right.ColumnCount || left.RowCount != right.RowCount)
            {
                throw new ArgumentException("The matrices do not have the same size.");
            }

            DenseMatrix result = new DenseMatrix();

            result._storedMatrix = right._storedMatrix.Clone() as MNet_LinAlg.Double.DenseMatrix;

            int[] columnIndices = left.GetColumnIndices();
            int[] rowPointers = left.GetRowPointers();
            double[] values = left.GetValues();

            // Iterate on the rows of left
            for (int i_R = 0; i_R < left.RowCount; i_R++)
            {
                // Iterate on the non-zero values of the current left row
                for (int i_NZ = rowPointers[i_R]; i_NZ < rowPointers[i_R + 1]; i_NZ++)
                {
                    result._storedMatrix[i_R, columnIndices[i_NZ]] += values[i_NZ];
                }
            }

            return result;
        }


        /// <summary>
        /// Computes the subtraction of a <see cref="DenseMatrix"/> with a <see cref="Sparse.CompressedRow"/>.
        /// </summary>
        /// <param name="left"> <see cref="DenseMatrix"/> to subtract. </param>
        /// <param name="right"> <see cref="Sparse.CompressedRow"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the subtraction. </returns>
        public static DenseMatrix Subtract(DenseMatrix left, Sparse.CompressedRow right)
        {
            // Verifications
            if (left.ColumnCount != right.ColumnCount || left.RowCount != right.RowCount)
            {
                throw new ArgumentException("The matrices do not have the same size.");
            }

            DenseMatrix result = new DenseMatrix();

            result._storedMatrix = left._storedMatrix.Clone() as MNet_LinAlg.Double.DenseMatrix;

            int[] columnIndices = right.GetColumnIndices();
            int[] rowPointers = right.GetRowPointers();
            double[] values = right.GetValues();

            // Iterate on the rows of right
            for (int i_R = 0; i_R < right.RowCount; i_R++)
            {
                // Iterate on the non-zero values of the current right row
                for (int i_NZ = rowPointers[i_R]; i_NZ < rowPointers[i_R + 1]; i_NZ++)
                {
                    result._storedMatrix[i_R, columnIndices[i_NZ]] -= values[i_NZ];
                }
            }

            return result;
        }

        /// <summary>
        /// Computes the subtraction of a <see cref="Sparse.CompressedRow"/> with a <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="left"> <see cref="Sparse.CompressedRow"/> to subtract. </param>
        /// <param name="right"> <see cref="DenseMatrix"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the subtraction. </returns>
        public static DenseMatrix Subtract(Sparse.CompressedRow left, DenseMatrix right)
        {
            // Verifications
            if (left.ColumnCount != right.ColumnCount || left.RowCount != right.RowCount)
            {
                throw new ArgumentException("The matrices do not have the same size.");
            }

            DenseMatrix result = new DenseMatrix();

            result._storedMatrix = right._storedMatrix.Multiply(-1.0) as MNet_LinAlg.Double.DenseMatrix;

            int[] columnIndices = left.GetColumnIndices();
            int[] rowPointers = left.GetRowPointers();
            double[] values = left.GetValues();

            // Iterate on the rows of left
            for (int i_R = 0; i_R < left.RowCount; i_R++)
            {
                // Iterate on the non-zero values of the current left row
                for (int i_NZ = rowPointers[i_R]; i_NZ < rowPointers[i_R + 1]; i_NZ++)
                {
                    result._storedMatrix[i_R, columnIndices[i_NZ]] += values[i_NZ];
                }
            }

            return result;
        }


        /// <summary>
        /// Computes the multiplication of a <see cref="DenseMatrix"/> with a <see cref="Sparse.CompressedRow"/>.
        /// </summary>
        /// <param name="left"> <see cref="DenseMatrix"/> for the multiplication. </param>
        /// <param name="right"> <see cref="Sparse.CompressedRow"/> for the multiplication. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the multiplication. </returns>
        public static DenseMatrix Multiply(DenseMatrix left, Sparse.CompressedRow right)
        {
            // Verifications
            if (left.ColumnCount != right.RowCount)
            {
                throw new ArgumentException("The matrices size does not allow their multiplication.");
            }

            DenseMatrix result = new DenseMatrix(left.RowCount, right.ColumnCount);

            int[] columnIndices = right.GetColumnIndices();
            int[] rowPointers = right.GetRowPointers();
            double[] values = right.GetValues();

            // Iterate on the rows of right
            for (int i_RR = 0; i_RR < right.RowCount; i_RR++)
            {
                // Iterate on the non-zero values of current right row
                for (int i_NZ = rowPointers[i_RR]; i_NZ < rowPointers[i_RR + 1]; i_NZ++)
                {
                    int i_C = columnIndices[i_NZ];
                    double val = values[i_NZ];

                    // Iterate on the rows of left
                    for (int i_R = 0; i_R < left.RowCount; i_R++)
                    {
                        result._storedMatrix[i_R, i_C] = left[i_R, i_RR] * val;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Computes the multiplication of a <see cref="Sparse.CompressedRow"/> with a <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="left"> <see cref="Sparse.CompressedRow"/> for the multiplication. </param>
        /// <param name="right"> <see cref="DenseMatrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the multiplication. </returns>
        public static DenseMatrix Multiply(Sparse.CompressedRow left, DenseMatrix right)
        {
            // Verifications
            if (left.ColumnCount != right.RowCount)
            {
                throw new ArgumentException("The matrices size does not allow their multiplication.");
            }

            DenseMatrix result = new DenseMatrix(left.RowCount, right.ColumnCount);

            int[] columnIndices = left.GetColumnIndices();
            int[] rowPointers = left.GetRowPointers();
            double[] values = left.GetValues();

            // Iterate on the rows of left
            for (int i_R = 0; i_R < left.RowCount; i_R++)
            {
                // ITerate on the columns of righ
                for (int i_C = 0; i_C < right.ColumnCount; i_C++)
                {
                    double sum = 0.0;

                    // Iterate on the non-zero values of the current left row
                    for (int i_NZ = rowPointers[i_R]; i_NZ < rowPointers[i_R + 1]; i_NZ++)
                    {
                        sum += values[i_NZ] * right[columnIndices[i_NZ], i_C];
                    }

                    result._storedMatrix[i_R, i_C] = sum;
                }
            }

            return result;
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Computes the scalar multiplication of a <see cref="DenseMatrix"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="factor"> <see cref="double"/>-precision real number. </param>
        /// <param name="operand"> <see cref="DenseMatrix"/> to multiply. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the scalar multiplication. </returns>
        public static DenseMatrix Multiply(double factor, DenseMatrix operand)
        {
            DenseMatrix matrix = new DenseMatrix();

            matrix._storedMatrix = operand._storedMatrix.Multiply(factor) as MNet_LinAlg.Double.DenseMatrix;

            return matrix;
        }

        /// <summary>
        /// Computes the scalar multiplication of a <see cref="DenseMatrix"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="factor"> <see cref="double"/>-precision real number. </param>
        /// <param name="operand"> <see cref="DenseMatrix"/> to multiply. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the scalar multiplication. </returns>
        public static DenseMatrix Multiply(DenseMatrix operand, double factor)
        {
            DenseMatrix result = new DenseMatrix();

            result._storedMatrix = operand._storedMatrix * factor;

            return result;
        }


        /// <summary>
        /// Computes the scalar division of a <see cref="DenseMatrix"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="operand"> <see cref="DenseMatrix"/> to divide. </param>
        /// <param name="divisor"> <see cref="double"/>-precision real number to divide with. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the scalar division. </returns>
        public static DenseMatrix Divide(DenseMatrix operand, double divisor)
        {
            DenseMatrix matrix = new DenseMatrix();

            matrix._storedMatrix = operand._storedMatrix.Divide(divisor) as MNet_LinAlg.Double.DenseMatrix;

            return matrix;
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override void Transpose()
        {
            _storedMatrix = _storedMatrix.Transpose() as MNet_LinAlg.Double.DenseMatrix;
        }

        #endregion

        #region Other Methods

        /******************** Algebraic Additive Group ********************/

        /// <summary>
        /// Computes the addition of the current <see cref="DenseMatrix"/> with another <see cref="Matrix"/>.
        /// </summary>
        /// <param name="right"> <see cref="Matrix"/> to add with on the right. </param>
        /// <returns> The new <see cref="DenseMatrix"/> as a <see cref="Matrix"/> resulting from the addition. </returns>
        protected override Matrix Add(Matrix right)
        {
            if (right is DenseMatrix denseRight) { return DenseMatrix.Add(this, denseRight); }
            else if (right is SparseMatrix sparseRight) { return DenseMatrix.Add(this, sparseRight); }
            else { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Computes the subtraction of the current <see cref="DenseMatrix"/> with another <see cref="Matrix"/>.
        /// </summary>
        /// <param name="right"> <see cref="Matrix"/> to subtract with on the right. </param>
        /// <returns> The new <see cref="DenseMatrix"/> as a <see cref="Matrix"/> resulting from the subtraction. </returns>
        protected override Matrix Subtract(Matrix right)
        {
            if (right is DenseMatrix denseRight) { return DenseMatrix.Subtract(this, denseRight); }
            else if (right is SparseMatrix sparseRight) { return DenseMatrix.Subtract(this, sparseRight); }
            else { throw new NotImplementedException(); }
        }

        /// <inheritdoc/>
        protected override void Opposite()
        {
            _storedMatrix = _storedMatrix.Multiply(-1.0) as MNet_LinAlg.Double.DenseMatrix;
        }

        /// <summary>
        /// Gets the neutral <see cref="DenseMatrix"/> of the addition.
        /// </summary>
        /// <returns> The neutral <see cref="DenseMatrix"/> of the addition. </returns>
        protected override Matrix Zero()
        {
            return new DenseMatrix(RowCount, ColumnCount);
        }


        /******************** Algebraic Multiplicative SemiGroup ********************/

        /// <summary>
        /// Computes the multiplication of the current <see cref="DenseMatrix"/> with another <see cref="Matrix"/>.
        /// </summary>
        /// <param name="right"> <see cref="Matrix"/> to multiply with on the right. </param>
        /// <returns> The new <see cref="DenseMatrix"/> as a <see cref="Matrix"/> resulting from the addition. </returns>
        protected override Matrix Multiply(Matrix right)
        {
            if (right is DenseMatrix denseRight) { return DenseMatrix.Multiply(this, denseRight); }
            else if (right is SparseMatrix sparseRight) { return DenseMatrix.Multiply(this, sparseRight); }
            else { throw new NotImplementedException(); }
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Computes the scalar multiplication the current <see cref="DenseMatrix"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="factor"> <see cref="double"/>-precision real number. </param>
        /// <returns> The new <see cref="DenseMatrix"/> as a <see cref="Matrix"/> resulting from the scalar multiplication. </returns>
        protected override Matrix Multiply(double factor)
        {
            return DenseMatrix.Multiply(this, factor);
        }

        /// <summary>
        /// Computes the scalar division of the current <see cref="DenseMatrix"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="divisor"> <see cref="double"/>-precision real number to divide with. </param>
        /// <returns> The new <see cref="DenseMatrix"/> as a <see cref="Matrix"/> resulting from the scalar division. </returns>
        protected override Matrix Divide(double divisor)
        {
            return DenseMatrix.Divide(this, divisor);
        }

        #endregion

    }
}
