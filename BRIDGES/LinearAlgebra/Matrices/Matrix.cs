using System;
using System.Collections.Generic;

using Alg_Fund = BRIDGES.Algebra.Fundamentals;
using Alg_Set = BRIDGES.Algebra.Sets;

using BRIDGES.LinearAlgebra.Vectors;


namespace BRIDGES.LinearAlgebra.Matrices
{
    /// <summary>
    /// Class defining a matrix.
    /// </summary>
    public abstract class Matrix
        : Alg_Set.Additive.IAbelianGroup<Matrix>, Alg_Set.Multiplicative.ISemiGroup<Matrix>, Alg_Set.IGroupAction<double, Matrix>
    {
        #region Properties

        /// <summary>
        /// Gets the number of rows in the current matrix.
        /// </summary>
        public abstract int RowCount { get; }

        /// <summary>
        /// Gets the number of columns in the current matrix.
        /// </summary>
        public abstract int ColumnCount { get; }


        /// <summary>
        /// Gets the value of the current matrix at the given row and column.
        /// </summary>
        /// <param name="row"> Row of the value to get. </param>
        /// <param name="column"> Column of the value to get. </param>
        /// <returns> The value at the given row and column index. </returns>
        public abstract double this[int row, int column] { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Matrix"/> class.
        /// </summary>
        protected Matrix()
        {
            /* Do nothing */
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Returns the neutral <see cref="Matrix"/> for the addition. 
        /// </summary>
        /// <param name="rowCount"> Number of rows of the <see cref="Matrix"/>. </param>
        /// <param name="columnCount"> Number of columns of the <see cref="Matrix"/>. </param>
        /// <returns> The <see cref="Sparse.CompressedColumn"/>, as a <see cref="Matrix"/>, of the given size and with zeros on every coordinates. </returns>
        public static Matrix Zero(int rowCount, int columnCount)
        {
            return Sparse.CompressedColumn.Zero(rowCount, columnCount);
        }

        /// <summary>
        /// Returns the neutral <see cref="Matrix"/> for the multiplication. 
        /// </summary>
        /// <param name="size"> Number of rows and columns of the <see cref="Matrix"/>. </param>
        /// <returns> The <see cref="Sparse.CompressedColumn"/>, as a <see cref="Matrix"/>, of the given size, with ones on the diagonal and zeros elsewhere. </returns>
        public static Matrix Identity(int size)
        {
            return Sparse.CompressedColumn.Identity(size);
        }

        #endregion

        #region Static Methods

        /******************** Algebraic Additive Group ********************/

        /// <summary>
        /// Computes the addition of two <see cref="Matrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Matrix"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Matrix"/> for the addition. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the addition. </returns>
        /// <exception cref="NotImplementedException"> The addition of these two matrix types as <see cref="Matrix"/> is not implemented. </exception>
        public static Matrix Add(Matrix left, Matrix right)
        {
            if (left is DenseMatrix denseLeft) { return DenseMatrix.Add(denseLeft, right); }
            else if (right is DenseMatrix denseRight) { return DenseMatrix.Add(left, denseRight); }
            else if (left is SparseMatrix sparseLeft && right is SparseMatrix sparseRight) { return SparseMatrix.Add(sparseLeft, sparseRight); }
            else { throw new NotImplementedException($"The addition of a {left.GetType()} and a {right.GetType()} as a {nameof(Matrix)} is not implemented."); }
        }

        /// <summary>
        /// Computes the subtraction of two <see cref="Matrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Matrix"/> to subtract. </param>
        /// <param name="right"> Right <see cref="Matrix"/> to subtract with. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the subtraction. </returns>
        /// <exception cref="NotImplementedException"> The subtraction of these two matrix types as <see cref="Matrix"/> is not implemented. </exception>
        public static Matrix Subtract(Matrix left, Matrix right)
        {
            if (left is DenseMatrix denseLeft) { return DenseMatrix.Subtract(denseLeft, right); }
            else if (right is DenseMatrix denseRight) { return DenseMatrix.Subtract(left, denseRight); }
            else if (left is SparseMatrix sparseLeft && right is SparseMatrix sparseRight) { return SparseMatrix.Subtract(sparseLeft, sparseRight); }
            else { throw new NotImplementedException($"The subtraction of a {left.GetType()} and a {right.GetType()} as {nameof(Matrix)} is not implemented."); }
        }


        /******************** Algebraic Multiplicative SemiGroup ********************/

        /// <summary>
        /// Computes the multiplication of two <see cref="Matrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Matrix"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="Matrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> The multiplication of these two matrix types is not implemented. </exception>
        public static Matrix Multiply(Matrix left, Matrix right)
        {
            if (left is DenseMatrix denseLeft) { return DenseMatrix.Multiply(denseLeft, right); }
            else if (right is DenseMatrix denseRight) { return DenseMatrix.Multiply(left, denseRight); }
            else if (left is SparseMatrix sparseLeft && right is SparseMatrix sparseRight) { return SparseMatrix.Multiply(sparseLeft, sparseRight); }
            else { throw new NotImplementedException($"The multiplication of a {left.GetType()} and a {right.GetType()} as {nameof(Matrix)} is not implemented."); }
        }

        /// <summary>
        /// Computes the left multiplication of a <see cref="Matrix"/> with its transposition : <c>At*A</c>.
        /// </summary>
        /// <param name="matrix">transposed <see cref="Matrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> The multiplication of these two matrix types is not implemented. </exception>
        public static Matrix TransposeMultiplySelf(Matrix matrix)
        {
            if (matrix is DenseMatrix dense) { return DenseMatrix.TransposeMultiplySelf(dense); }
            else if (matrix is SparseMatrix sparse) { return SparseMatrix.TransposeMultiplySelf(sparse); }
            else { throw new NotImplementedException($"The left multiplication of a {matrix.GetType()} with it transposition as {nameof(Matrix)} is not implemented."); }
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Computes the scalar multiplication of a <see cref="Matrix"/> with a <see cref="double"/>-precision real number on the left.
        /// </summary>
        /// <param name="factor"> <see cref="double"/>-precision real number. </param>
        /// <param name="operand"> <see cref="Matrix"/> to multiply on the left. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the scalar multiplication. </returns>
        /// <exception cref="NotImplementedException"> The scalar multiplication on the left of the operand as a <see cref="Matrix"/> is not implemented. </exception>
        public static Matrix Multiply(double factor, Matrix operand)
        {
            if (operand is DenseMatrix denseOperand) { return DenseMatrix.Multiply(factor, denseOperand); }
            else if (operand is SparseMatrix sparseOperand) { return SparseMatrix.Multiply(factor, sparseOperand); }
            else { throw new NotImplementedException($"The scalar multiplication on the left of {operand.GetType()} as a {nameof(Matrix)} is not implemented."); }
        }

        /// <summary>
        /// Computes the scalar multiplication of a <see cref="Matrix"/> with a <see cref="double"/>-precision real number on the right.
        /// </summary>
        /// <param name="operand"> <see cref="Matrix"/> to multiply on the right. </param>
        /// <param name="factor"> <see cref="double"/>-precision real number. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the scalar multiplication. </returns>
        /// <exception cref="NotImplementedException"> The scalar multiplication on the right of the operand as a <see cref="Matrix"/> is not implemented. </exception>
        public static Matrix Multiply(Matrix operand, double factor)
        {
            if (operand is DenseMatrix denseOperand) { return DenseMatrix.Multiply(denseOperand, factor); }
            else if (operand is SparseMatrix sparseOperand) { return SparseMatrix.Multiply(sparseOperand, factor); }
            else { throw new NotImplementedException($"The scalar multiplication on the left of {operand.GetType()} as a {nameof(Matrix)} is not implemented."); }
        }


        /// <summary>
        /// Computes the scalar division of a <see cref="Matrix"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="operand"> <see cref="Matrix"/> to divide. </param>
        /// <param name="divisor"> <see cref="double"/>-precision real number to divide with. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the scalar division. </returns>
        /// <exception cref="NotImplementedException"> The scalar division of the operand as a <see cref="Matrix"/> is not implemented. </exception>
        public static Matrix Divide(Matrix operand, double divisor)
        {
            if (operand is DenseMatrix denseOperand) { return DenseMatrix.Divide(denseOperand, divisor); }
            else if (operand is SparseMatrix sparseOperand) { return SparseMatrix.Divide(sparseOperand, divisor); }
            else { throw new NotImplementedException($"The scalar division of {operand.GetType()} as a {nameof(Matrix)} is not implemented."); }
        }


        /******************** Other Operations ********************/

        /// <summary>
        /// Computes the right multiplication of a <see cref="Matrix"/> with a <see cref="Vector"/> : <c>A*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="Matrix"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The right multiplication of the matrix as a <see cref="Matrix"/> with a <see cref="Vector"/> is not implemented.
        /// </exception>
        public static Vector Multiply(Matrix matrix, Vector vector)
        {
            if (matrix is DenseMatrix denseMatrix) { return DenseMatrix.Multiply(denseMatrix, vector); }
            else if (matrix is SparseMatrix sparseMatrix) { return SparseMatrix.Multiply(sparseMatrix, vector); }
            else { throw new NotImplementedException($"The right multiplication of a {matrix.GetType()} as a {nameof(Matrix)} and a {vector.GetType()} is not implemented."); }
        }

        /// <summary>
        /// Computes the right multiplication of a <see cref="Matrix"/> with a <see cref="DenseVector"/> : <c>A*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="Matrix"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="DenseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The right multiplication of the matrix as a <see cref="Matrix"/> with a <see cref="Vector"/> is not implemented.
        /// </exception>
        public static DenseVector Multiply(Matrix matrix, DenseVector vector)
        {
            if (matrix is DenseMatrix denseMatrix) { return DenseMatrix.Multiply(denseMatrix, vector); }
            else if (matrix is SparseMatrix sparseMatrix) { return SparseMatrix.Multiply(sparseMatrix, vector); }
            else { throw new NotImplementedException($"The right multiplication of a {matrix.GetType()} as a {nameof(Matrix)} and a {vector.GetType()} is not implemented."); }
        }

        /// <summary>
        /// Computes the right multiplication of a <see cref="Matrix"/> with a <see cref="SparseVector"/> : <c>A*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="Matrix"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="SparseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The right multiplication of the matrix as a <see cref="Matrix"/> with a <see cref="SparseVector"/> is not implemented.
        /// </exception>
        public static Vector Multiply(Matrix matrix, SparseVector vector)
        {
            if (matrix is DenseMatrix denseMatrix) { return DenseMatrix.Multiply(denseMatrix, vector); }
            else if (matrix is SparseMatrix sparseMatrix) { return SparseMatrix.Multiply(sparseMatrix, vector); }
            else { throw new NotImplementedException($"The right multiplication of a {matrix.GetType()} as a {nameof(Matrix)} and a {vector.GetType()} is not implemented."); }
        }


        /// <summary>
        /// Computes the right multiplication of a transposed <see cref="Matrix"/> with a <see cref="Vector"/> : <c>At*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="Matrix"/> to transpose then multiply on the right. </param>
        /// <param name="vector"> <see cref="Vector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The right multiplication of the transposed matrix as a <see cref="Matrix"/> with a <see cref="Vector"/> is not implemented.
        /// </exception>
        public static Vector TransposeMultiply(Matrix matrix, Vector vector)
        {
            if (matrix is DenseMatrix denseMatrix) { return DenseMatrix.TransposeMultiply(denseMatrix, vector); }
            else if (matrix is SparseMatrix sparseMatrix) { return SparseMatrix.TransposeMultiply(sparseMatrix, vector); }
            else { throw new NotImplementedException($"The right multiplication of a transposed {matrix.GetType()} as a {nameof(Matrix)} and a {vector.GetType()} is not implemented."); }

        }

        /// <summary>
        /// Computes the right multiplication of a transposed <see cref="Matrix"/> with a <see cref="DenseVector"/> : <c>At*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="Matrix"/> to transpose then multiply on the right. </param>
        /// <param name="vector"> <see cref="DenseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The right multiplication of the transposed matrix as a <see cref="Matrix"/> with a <see cref="DenseVector"/> is not implemented.
        /// </exception>
        public static DenseVector TransposeMultiply(Matrix matrix, DenseVector vector)
        {
            if (matrix is DenseMatrix denseMatrix) { return DenseMatrix.TransposeMultiply(denseMatrix, vector); }
            else if (matrix is SparseMatrix sparseMatrix) { return SparseMatrix.TransposeMultiply(sparseMatrix, vector); }
            else { throw new NotImplementedException($"The right multiplication of a transposed {matrix.GetType()} as a {nameof(Matrix)} and a {vector.GetType()} is not implemented."); }

        }

        /// <summary>
        /// Computes the right multiplication of a transposed <see cref="Matrix"/> with a <see cref="SparseVector"/> : <c>At*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="Matrix"/> to transpose then multiply on the right. </param>
        /// <param name="vector"> <see cref="SparseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The right multiplication of the transposed matrix as a <see cref="Matrix"/> with a <see cref="SparseVector"/> is not implemented.
        /// </exception>
        public static Vector TransposeMultiply(Matrix matrix, SparseVector vector)
        {
            if (matrix is DenseMatrix denseMatrix) { return DenseMatrix.TransposeMultiply(denseMatrix, vector); }
            else if (matrix is SparseMatrix sparseMatrix) { return SparseMatrix.TransposeMultiply(sparseMatrix, vector); }
            else { throw new NotImplementedException($"The right multiplication of a transposed {matrix.GetType()} as a {nameof(Matrix)} and a {vector.GetType()} is not implemented."); }

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Transposes the current matrix.
        /// </summary>
        /// <returns> <see langword="true"/> if the matrix was transposed successfully, <see langword="false"/> otherwise. </returns>
        public abstract void Transpose();

        #endregion

        #region Other Methods

        /******************** Algebraic Additive Group ********************/

        /// <inheritdoc cref="Alg_Set.Additive.IGroup{T}.Opposite()"/>
        protected abstract void Opposite();

        #endregion


        #region Explicit : Additive.IAbelianGroup<Matrix>

        /******************** Properties ********************/

        /// <inheritdoc/>
        bool Alg_Fund.IAddable<Matrix>.IsAssociative => true;

        /// <inheritdoc/>
        bool Alg_Fund.IAddable<Matrix>.IsCommutative => true;


        /******************** Methods ********************/

        /// <inheritdoc/>
        Matrix Alg_Fund.IAddable<Matrix>.Add(Matrix right) { return Matrix.Add(this, right); }

        /// <inheritdoc/>
        Matrix Alg_Fund.ISubtractable<Matrix>.Subtract(Matrix right) { return Matrix.Subtract(this, right); }

        /// <inheritdoc/>
        bool Alg_Set.Additive.IGroup<Matrix>.Opposite()
        {
            this.Opposite();
            return true;
        }

        /// <inheritdoc/>
        Matrix Alg_Fund.IZeroable<Matrix>.Zero() { return Matrix.Zero(RowCount, ColumnCount); }

        #endregion

        #region Explicit : Multiplicative.SemiGroup<Matrix>

        /******************** Properties ********************/

        /// <inheritdoc/>
        bool Alg_Fund.IMultiplicable<Matrix>.IsAssociative => true;

        /// <inheritdoc/>
        bool Alg_Fund.IMultiplicable<Matrix>.IsCommutative => false;


        /******************** Methods ********************/

        /// <inheritdoc/>
        Matrix Alg_Fund.IMultiplicable<Matrix>.Multiply(Matrix right) { return Matrix.Multiply(this, right); }

        #endregion

        #region Explicit : IGroupAction<Double,Matrix>

        /******************** Methods ********************/

        /// <inheritdoc/>
        Matrix Alg_Set.IGroupAction<double, Matrix>.Multiply(double factor) { return Matrix.Multiply(this, factor); }

        /// <inheritdoc/>
        Matrix Alg_Set.IGroupAction<double, Matrix>.Divide(double divisor) { return Matrix.Divide(this, divisor); }

        #endregion
    }
}
