using System;
using System.Collections.Generic;

using MNet_LinAlg = MathNet.Numerics.LinearAlgebra;

using Alg_Fund = BRIDGES.Algebra.Fundamentals;
using Alg_Set = BRIDGES.Algebra.Sets;

using BRIDGES.LinearAlgebra.Vectors;


namespace BRIDGES.LinearAlgebra.Matrices
{
    /// <summary>
    /// Class defining a dense matrix.
    /// </summary>
    public sealed class DenseMatrix : Matrix,
          Alg_Set.Additive.IAbelianGroup<DenseMatrix>, Alg_Set.Multiplicative.ISemiGroup<DenseMatrix>, Alg_Set.IGroupAction<double, DenseMatrix>
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
        /// Initialises a new instance of the <see cref="DenseMatrix"/> class by defining its number of row and column.
        /// </summary>
        /// <param name="rowCount"> Number of rows of the <see cref="DenseMatrix"/>. </param>
        /// <param name="columnCount"> Number of columns of the <see cref="DenseMatrix"/>. </param>
        public DenseMatrix(int rowCount, int columnCount)
        {
            // Instanciate fields
            _storedMatrix = MNet_LinAlg.Double.DenseMatrix.Create(rowCount, columnCount, 0.0);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DenseMatrix"/> class by defining its size and values row by row.
        /// </summary>
        /// <param name="rowCount"> Number of rows of the <see cref="DenseMatrix"/>. </param>
        /// <param name="columnCount"> Number of columns of the <see cref="DenseMatrix"/>. </param>
        /// <param name="components"> Component values given row by row. </param>
        /// <exception cref="ArgumentException"> The number of components provided doesn't match the given number of rows and columns. </exception>
        public DenseMatrix(int rowCount, int columnCount, double[] components)
        {
            // Verifications
            if (components.Length != rowCount * columnCount)
            {
                throw new ArgumentException("The number of values provided doesn't match the given number of rows and columns.");
            }

            // Instanciate fields
            _storedMatrix =  MNet_LinAlg.Double.DenseMatrix.Create(rowCount, columnCount, (int i_R, int i_C) => components[(i_R * columnCount) + i_C]);
        }


        /// <summary>
        /// Initialises a new instance of the <see cref="DenseMatrix"/> class.
        /// </summary>
        private DenseMatrix()
        {
            /* Do nothing */
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Returns the neutral <see cref="DenseMatrix"/> for the addition. 
        /// </summary>
        /// <param name="rowCount"> Number of rows of the <see cref="DenseMatrix"/>. </param>
        /// <param name="columnCount"> Number of columns of the <see cref="DenseMatrix"/>. </param>
        /// <returns> The <see cref="DenseMatrix"/> of the given size and with zeros on every coordinates. </returns>
        public static new DenseMatrix Zero(int rowCount, int columnCount)
        {
            return new DenseMatrix(rowCount, columnCount);
        }

        /// <summary>
        /// Returns the neutral <see cref="Matrix"/> for the multiplication. 
        /// </summary>
        /// <param name="size"> Number of rows and columns of the <see cref="Matrix"/>. </param>
        /// <returns> The <see cref="DenseMatrix"/> of the given size, with ones on the diagonal and zeros elsewhere. </returns>
        public static new DenseMatrix Identity(int size)
        {
            DenseMatrix result = new DenseMatrix(size, size);

            for (int i = 0; i < size; i++)
            {
                result._storedMatrix[i, i] = 1.0;
            }

            return result;
        }

        #endregion

        #region Static Methods

        /******************** Algebraic Near Ring ********************/

        /// <summary>
        /// Computes the addition of two <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix"/> for the addition. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> for the addition. </param>
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
        /// <param name="left"> Left <see cref="DenseMatrix"/> to subtract. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> to subtract with. </param>
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
        /// <param name="left"> Left <see cref="DenseMatrix"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the multiplication. </returns>
        public static DenseMatrix Multiply(DenseMatrix left, DenseMatrix right)
        {
            DenseMatrix matrix = new DenseMatrix();

            matrix._storedMatrix = left._storedMatrix.Multiply(right._storedMatrix) as MNet_LinAlg.Double.DenseMatrix;

            return matrix;
        }


        /******************** Matrix Embedding ********************/

        /// <summary>
        /// Computes the addition of a <see cref="DenseMatrix"/> with a <see cref="Matrix"/> on the right.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Matrix"/> for the addition. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the addition. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The addition of a <see cref="DenseMatrix"/> with the right matrix as a <see cref="Matrix"/> is not implemented.
        /// </exception>
        public static DenseMatrix Add(DenseMatrix left, Matrix right)
        {
            if (right is DenseMatrix denseRight) { return DenseMatrix.Add(left, denseRight); }
            else if (right is SparseMatrix sparseRight) { return DenseMatrix.Add(left, sparseRight); }
            else { throw new NotImplementedException($"The addition of a {left.GetType()} and a {right.GetType()} as a {nameof(Matrix)} is not implemented."); }
        }

        /// <summary>
        /// Computes the addition of a <see cref="Matrix"/> with a <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Matrix"/> for the addition. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> for the addition. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the addition. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The addition of the left matrix as a <see cref="Matrix"/> with a <see cref="DenseMatrix"/> is not implemented.
        /// </exception>
        public static DenseMatrix Add(Matrix left, DenseMatrix right)
        {
            if (left is DenseMatrix denseLeft) { return DenseMatrix.Add(denseLeft, right); }
            else if (left is SparseMatrix sparseLeft) { return DenseMatrix.Add(sparseLeft, right); }
            else { throw new NotImplementedException($"The addition of a {left.GetType()} as a {nameof(Matrix)} and a {right.GetType()} is not implemented."); }
        }


        /// <summary>
        /// Computes the subtraction of a <see cref="DenseMatrix"/> with a <see cref="Matrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix"/> to subtract. </param>
        /// <param name="right"> Right <see cref="Matrix"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the subtraction. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The subtraction of a <see cref="DenseMatrix"/> with the right matrix as a <see cref="Matrix"/> is not implemented.
        /// </exception>
        public static DenseMatrix Subtract(DenseMatrix left, Matrix right)
        {
            if (right is DenseMatrix denseRight) { return DenseMatrix.Subtract(left, denseRight); }
            else if (right is SparseMatrix sparseRight) { return DenseMatrix.Subtract(left, sparseRight); }
            else { throw new NotImplementedException($"The subtraction of a {left.GetType()} and a {right.GetType()} as a {nameof(Matrix)} is not implemented."); }
        }

        /// <summary>
        /// Computes the subtraction of a <see cref="Matrix"/> with a <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Matrix"/> to subtract. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the subtraction. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The subtraction of the left matrix as a <see cref="Matrix"/> with a <see cref="DenseMatrix"/> is not implemented.
        /// </exception>
        public static DenseMatrix Subtract(Matrix left, DenseMatrix right)
        {
            if (left is DenseMatrix denseLeft) { return DenseMatrix.Subtract(denseLeft, right); }
            else if (left is SparseMatrix sparseLeft) { return DenseMatrix.Subtract(sparseLeft, right); }
            else { throw new NotImplementedException($"The subtraction of a {left.GetType()} as a {nameof(Matrix)} and a {right.GetType()} is not implemented."); }
        }


        /// <summary>
        /// Computes the multiplication of a <see cref="DenseMatrix"/> with a <see cref="Matrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="Matrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The multiplication of a <see cref="DenseMatrix"/> with the right matrix as a <see cref="Matrix"/> is not implemented.
        /// </exception>
        public static DenseMatrix Multiply(DenseMatrix left, Matrix right)
        {
            if (right is DenseMatrix denseRight) { return DenseMatrix.Multiply(left, denseRight); }
            else if (right is SparseMatrix sparseRight) { return DenseMatrix.Multiply(left, sparseRight); }
            else { throw new NotImplementedException($"The multiplication of a {left.GetType()} and a {right.GetType()} as a {nameof(Matrix)} is not implemented."); }
        }

        /// <summary>
        /// Computes the multiplication of a <see cref="Matrix"/> with a <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Matrix"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The multiplication of the left matrix as a <see cref="Matrix"/> with a <see cref="DenseMatrix"/> is not implemented.
        /// </exception>
        public static DenseMatrix Multiply(Matrix left, DenseMatrix right)
        {
            if (left is DenseMatrix denseLeft) { return DenseMatrix.Multiply(denseLeft, right); }
            else if (left is SparseMatrix sparseLeft) { return DenseMatrix.Multiply(sparseLeft, right); }
            else { throw new NotImplementedException($"The multiplication of a {left.GetType()} as a {nameof(Matrix)} and a {right.GetType()} is not implemented."); }
        }


        /******************** Sparse Matrix Embedding ********************/

        /// <summary>
        /// Computes the addition of a <see cref="DenseMatrix"/> with a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix"/> for the addition. </param>
        /// <param name="right"> Right <see cref="SparseMatrix"/> for the addition. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the addition. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The addition of a <see cref="DenseMatrix"/> with the right matrix as a <see cref="SparseMatrix"/> is not implemented. 
        /// </exception>
        public static DenseMatrix Add(DenseMatrix left, SparseMatrix right)
        {
            if (right is Sparse.CompressedColumn ccsRight) { return DenseMatrix.Add(left, ccsRight); }
            else if (right is Sparse.CompressedRow crsRight) { return DenseMatrix.Add(left, crsRight); }
            else { throw new NotImplementedException($"The addition of a {left.GetType()} and a {right.GetType()} as a {nameof(SparseMatrix)} is not implemented."); }
        }

        /// <summary>
        /// Computes the addition of a <see cref="SparseMatrix"/> with a <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix"/> for the addition. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> for the addition. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the addition. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The addition of the left matrix as a <see cref="SparseMatrix"/> with a <see cref="DenseMatrix"/> is not implemented.
        /// </exception>
        public static DenseMatrix Add(SparseMatrix left, DenseMatrix right)
        {
            if (left is Sparse.CompressedColumn ccsLeft) { return DenseMatrix.Add(ccsLeft, right); }
            else if (left is Sparse.CompressedRow crsLeft) { return DenseMatrix.Add(crsLeft, right); }
            else { throw new NotImplementedException($"The addition of a {left.GetType()} as a {nameof(SparseMatrix)} and a {right.GetType()} is not implemented."); }
        }


        /// <summary>
        /// Computes the subtraction of a <see cref="DenseMatrix"/> with a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix"/> to subtract. </param>
        /// <param name="right"> Right <see cref="SparseMatrix"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the subtraction. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The subtraction of a <see cref="DenseMatrix"/> with the right matrix as a <see cref="SparseMatrix"/> is not implemented. 
        /// </exception>
        public static DenseMatrix Subtract(DenseMatrix left, SparseMatrix right)
        {
            if (right is Sparse.CompressedColumn ccsRight) { return DenseMatrix.Subtract(left, ccsRight); }
            else if (right is Sparse.CompressedRow crsRight) { return DenseMatrix.Subtract(left, crsRight); }
            else { throw new NotImplementedException($"The subtraction of a {left.GetType()} and a {right.GetType()} as a {nameof(SparseMatrix)} is not implemented."); }
        }

        /// <summary>
        /// Computes the subtraction of a <see cref="SparseMatrix"/> with a <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix"/> to subtract. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the subtraction. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The subtraction of the left matrix as a <see cref="SparseMatrix"/> with a <see cref="DenseMatrix"/> is not implemented.
        /// </exception>
        public static DenseMatrix Subtract(SparseMatrix left, DenseMatrix right)
        {
            if (left is Sparse.CompressedColumn ccsLeft) { return DenseMatrix.Subtract(ccsLeft, right); }
            else if (left is Sparse.CompressedRow crsLeft) { return DenseMatrix.Subtract(crsLeft, right); }
            else { throw new NotImplementedException($"The subtraction of a {left.GetType()} as a {nameof(SparseMatrix)} and a {right.GetType()} is not implemented."); }
        }


        /// <summary>
        /// Computes the multiplication of a <see cref="DenseMatrix"/> with a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="SparseMatrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The subtraction of a <see cref="DenseMatrix"/> with the right matrix as a <see cref="SparseMatrix"/> is not implemented. 
        /// </exception>
        public static DenseMatrix Multiply(DenseMatrix left, SparseMatrix right)
        {
            if (right is Sparse.CompressedColumn ccsRight) { return DenseMatrix.Multiply(left, ccsRight); }
            else if (right is Sparse.CompressedRow crsRight) { return DenseMatrix.Multiply(left, crsRight); }
            else { throw new NotImplementedException($"The multiplication of a {left.GetType()} and a {right.GetType()} as a {nameof(SparseMatrix)} is not implemented."); }
        }

        /// <summary>
        /// Computes the multiplication of a <see cref="SparseMatrix"/> with a <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The multiplication of the left matrix as a <see cref="SparseMatrix"/> with a <see cref="DenseMatrix"/> is not implemented.
        /// </exception>
        public static DenseMatrix Multiply(SparseMatrix left, DenseMatrix right)
        {
            if (left is Sparse.CompressedColumn ccsLeft) { return DenseMatrix.Multiply(ccsLeft, right); }
            else if (left is Sparse.CompressedRow crsLeft) { return DenseMatrix.Multiply(crsLeft, right); }
            else { throw new NotImplementedException($"The multiplication of a {left.GetType()} as a {nameof(SparseMatrix)} and a {right.GetType()} is not implemented."); }
        }


        /******************** CompressedColumn Embedding ********************/

        /// <summary>
        /// Computes the addition of a <see cref="DenseMatrix"/> with a <see cref="Sparse.CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Sparse.CompressedColumn"/> for the addition. </param>
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
        /// <param name="left"> Left <see cref="Sparse.CompressedColumn"/> for the addition. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> for the addition. </param>
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
        /// <param name="left"> Left <see cref="DenseMatrix"/> to subtract. </param>
        /// <param name="right"> Right <see cref="Sparse.CompressedColumn"/> to subtract with. </param>
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
        /// <param name="left"> Left <see cref="Sparse.CompressedColumn"/> to subtract. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> to subtract with. </param>
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
        /// <param name="left"> Left <see cref="DenseMatrix"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="Sparse.CompressedColumn"/> for the multiplication. </param>
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
        /// <param name="left"> Left <see cref="Sparse.CompressedColumn"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> for the multiplication. </param>
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
        /// <param name="left"> Left <see cref="DenseMatrix"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Sparse.CompressedRow"/> for the addition. </param>
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
        /// <param name="left"> Left <see cref="Sparse.CompressedRow"/> for the addition. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> for the addition. </param>
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
        /// <param name="left"> Left <see cref="DenseMatrix"/> to subtract. </param>
        /// <param name="right"> Right <see cref="Sparse.CompressedRow"/> to subtract with. </param>
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
        /// <param name="left"> Left <see cref="Sparse.CompressedRow"/> to subtract. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> to subtract with. </param>
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
        /// <param name="left"> Left <see cref="DenseMatrix"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="Sparse.CompressedRow"/> for the multiplication. </param>
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
        /// <param name="left"> Left <see cref="Sparse.CompressedRow"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> for the multiplication. </param>
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


        /******************** Other Operations ********************/

        /// <summary>
        /// Computes the right multiplication of a <see cref="DenseMatrix"/> with a <see cref="Vector"/> : <c>A*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="DenseMatrix"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vector"/> to multiply with. </param>
        /// <returns> The new <see cref="DenseVector"/> as a <see cref="Vector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The right multiplication of a <see cref="DenseMatrix"/> with a the vector as a <see cref="Vector"/> is not implemented.
        /// </exception>
        public static Vector Multiply(DenseMatrix matrix, Vector vector)
        {
            if (vector is DenseVector denseVector) { return DenseMatrix.Multiply(matrix, denseVector); }
            else if (vector is SparseVector sparseVector) { return DenseMatrix.Multiply(matrix, sparseVector); }
            else { throw new NotImplementedException($"The right multiplication of a {matrix.GetType()} and a {vector.GetType()} as a {nameof(Vector)} is not implemented."); }
        }

        /// <summary>
        /// Computes the right multiplication of a <see cref="DenseMatrix"/> with a <see cref="DenseVector"/> : <c>A*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="DenseMatrix"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="DenseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the multiplication. </returns>
        public static DenseVector Multiply(DenseMatrix matrix, DenseVector vector)
        {
            double[] components = new double[matrix.RowCount];

            for (int i_R = 0; i_R < components.Length; i_R++)
            {
                double component = 0.0;
                for (int i = 0; i < matrix.ColumnCount; i++)
                {
                    component += matrix._storedMatrix[i_R, i] * vector[i];
                }
                components[i_R] = component;
            }

            return new DenseVector(components);
        }

        /// <summary>
        /// Computes the right multiplication of a <see cref="DenseMatrix"/> with a <see cref="SparseVector"/> : <c>A*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="DenseMatrix"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="SparseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the multiplication. </returns>
        public static DenseVector Multiply(DenseMatrix matrix, SparseVector vector)
        {
            double[] components = new double[matrix.RowCount];

            IEnumerator<KeyValuePair<int, double>> kvpEnumerator = vector.GetNonZeros();
            try
            {
                while (kvpEnumerator.MoveNext())
                {
                    KeyValuePair<int, double> kvp = kvpEnumerator.Current;

                    for (int i_R = 0; i_R < components.Length; i_R++)
                    {
                        components[i_R] += matrix._storedMatrix[i_R, kvp.Key] * kvp.Value;
                    }
                }
            }
            finally { kvpEnumerator.Dispose(); }

            return new DenseVector(components);
        }


        /// <summary>
        /// Computes the right multiplication of a transposed <see cref="DenseMatrix"/> with a <see cref="Vector"/> : <c>At*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="DenseMatrix"/> to transpose then multiply on the right. </param>
        /// <param name="vector"> <see cref="Vector"/> to multiply with. </param>
        /// <returns> The new <see cref="DenseVector"/> as a <see cref="Vector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The right multiplication of a transposed <see cref="DenseMatrix"/> with a the vector as a <see cref="Vector"/> is not implemented.
        /// </exception>
        public static Vector TransposeMultiply(DenseMatrix matrix, Vector vector)
        {
            if (vector is DenseVector denseVector) { return DenseMatrix.TransposeMultiply(matrix, denseVector); }
            else if (vector is SparseVector sparseVector) { return DenseMatrix.TransposeMultiply(matrix, sparseVector); }
            else { throw new NotImplementedException($"The right multiplication of a transposed {matrix.GetType()} and a {vector.GetType()} as a {nameof(Vector)} is not implemented."); }
        }

        /// <summary>
        /// Computes the right multiplication of a transposed <see cref="DenseMatrix"/> with a <see cref="DenseVector"/> : <c>At*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="DenseMatrix"/> to transpose then multiply on the right. </param>
        /// <param name="vector"> <see cref="DenseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the multiplication. </returns>
        public static DenseVector TransposeMultiply(DenseMatrix matrix, DenseVector vector)
        {
            double[] components = new double[matrix.ColumnCount];

            for (int i_R = 0; i_R < components.Length; i_R++)
            {
                double component = 0.0;
                for (int i = 0; i < matrix.RowCount; i++)
                {
                    component += matrix._storedMatrix[i, i_R] * vector[i];
                }
                components[i_R] = component;
            }

            return new DenseVector(components);
        }

        /// <summary>
        /// Computes the right multiplication of a transposed <see cref="DenseMatrix"/> with a <see cref="SparseVector"/> : <c>At*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="DenseMatrix"/> to transpose then multiply on the right. </param>
        /// <param name="vector"> <see cref="SparseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the multiplication. </returns>
        public static DenseVector TransposeMultiply(DenseMatrix matrix, SparseVector vector)
        {
            double[] components = new double[matrix.ColumnCount];

            IEnumerator<KeyValuePair<int, double>> kvpEnumerator = vector.GetNonZeros();
            try
            {
                while (kvpEnumerator.MoveNext())
                {
                    KeyValuePair<int, double> kvp = kvpEnumerator.Current;

                    for (int i_R = 0; i_R < components.Length; i_R++)
                    {
                        components[i_R] += matrix._storedMatrix[kvp.Key, i_R] * kvp.Value;
                    }
                }
            }
            finally { kvpEnumerator.Dispose(); }

            return new DenseVector(components);
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override void Transpose()
        {
            _storedMatrix = _storedMatrix.Transpose() as MNet_LinAlg.Double.DenseMatrix;
        }

        #endregion


        #region Override : Matrix

        /// <inheritdoc/>
        protected override void Opposite()
        {
            _storedMatrix = _storedMatrix.Multiply(-1.0) as MNet_LinAlg.Double.DenseMatrix;
        }

        #endregion


        #region Explicit : Additive.IAbelianGroup<DenseMatrix>

        /******************** Properties ********************/

        /// <inheritdoc/>
        bool Alg_Fund.IAddable<DenseMatrix>.IsAssociative => true;

        /// <inheritdoc/>
        bool Alg_Fund.IAddable<DenseMatrix>.IsCommutative => true;


        /******************** Methods ********************/

        /// <inheritdoc/>
        DenseMatrix Alg_Fund.IAddable<DenseMatrix>.Add(DenseMatrix right) { return DenseMatrix.Add(this, right); }

        /// <inheritdoc/>
        DenseMatrix Alg_Fund.ISubtractable<DenseMatrix>.Subtract(DenseMatrix right) { return DenseMatrix.Subtract(this, right); }

        /// <inheritdoc/>
        bool Alg_Set.Additive.IGroup<DenseMatrix>.Opposite()
        {
            this.Opposite();
            return true;
        }

        /// <inheritdoc/>
        DenseMatrix Alg_Fund.IZeroable<DenseMatrix>.Zero() { return DenseMatrix.Zero(RowCount, ColumnCount); }

        #endregion

        #region Explicit : Multiplicative.SemiGroup<DenseMatrix>

        /******************** Properties ********************/

        /// <inheritdoc/>
        bool Alg_Fund.IMultiplicable<DenseMatrix>.IsAssociative => true;

        /// <inheritdoc/>
        bool Alg_Fund.IMultiplicable<DenseMatrix>.IsCommutative => false;


        /******************** Methods ********************/

        /// <inheritdoc/>
        DenseMatrix Alg_Fund.IMultiplicable<DenseMatrix>.Multiply(DenseMatrix right) { return DenseMatrix.Multiply(this, right); }

        #endregion

        #region Explicit : IGroupAction<Double,DenseMatrix>

        /******************** Methods ********************/

        /// <inheritdoc/>
        DenseMatrix Alg_Set.IGroupAction<double, DenseMatrix>.Multiply(double factor) { return DenseMatrix.Multiply(this, factor); }

        /// <inheritdoc/>
        DenseMatrix Alg_Set.IGroupAction<double, DenseMatrix>.Divide(double divisor) { return DenseMatrix.Divide(this, divisor); }

        #endregion
    }
}
