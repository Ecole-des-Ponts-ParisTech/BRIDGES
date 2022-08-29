using System;

using Alg_Fund = BRIDGES.Algebra.Fundamentals;
using Alg_Set = BRIDGES.Algebra.Sets;

using BRIDGES.LinearAlgebra.Vectors;


namespace BRIDGES.LinearAlgebra.Matrices
{
    /// <summary>
    /// Class defining a sparse matrix.
    /// </summary>
    public abstract class SparseMatrix : Matrix,
          Alg_Set.Additive.IAbelianGroup<SparseMatrix>, Alg_Set.Multiplicative.ISemiGroup<SparseMatrix>, Alg_Set.IGroupAction<double, SparseMatrix>
    {
        #region Properties

        /// <summary>
        /// Gets the number of non-zero values in the current sparse matrix.
        /// </summary>
        public abstract int NonZeroCount { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="SparseMatrix"/> class.
        /// </summary>
        protected SparseMatrix()
        {
            /* Do nothing */
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Returns the neutral <see cref="SparseMatrix"/> for the addition. 
        /// </summary>
        /// <param name="rowCount"> Number of rows of the <see cref="SparseMatrix"/>. </param>
        /// <param name="columnCount"> Number of columns of the <see cref="SparseMatrix"/>. </param>
        /// <returns> The <see cref="Sparse.CompressedColumn"/>, as a <see cref="SparseMatrix"/>, of the given size and with zeros on every coordinates. </returns>
        public static new SparseMatrix Zero(int rowCount, int columnCount)
        {
            return Sparse.CompressedColumn.Zero(rowCount, columnCount);
        }

        /// <summary>
        /// Returns the neutral <see cref="SparseMatrix"/> for the multiplication. 
        /// </summary>
        /// <param name="size"> Number of rows and columns of the <see cref="SparseMatrix"/>. </param>
        /// <returns> The <see cref="Sparse.CompressedColumn"/>, as a <see cref="SparseMatrix"/>, of the given size, with ones on the diagonal and zeros elsewhere. </returns>
        public static new SparseMatrix Identity(int size)
        {
            return Sparse.CompressedColumn.Identity(size);
        }

        #endregion

        #region Static Methods

        /******************** Algebraic Additive Group ********************/

        /// <summary>
        /// Computes the addition of two <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix"/> for the addition. </param>
        /// <param name="right"> Right <see cref="SparseMatrix"/> for the addition. </param>
        /// <returns> The new <see cref="SparseMatrix"/> resulting from the addition. </returns>
        /// <exception cref="NotImplementedException"> The addition of these two matrix as <see cref="SparseMatrix"/> is not implemented. </exception>
        public static SparseMatrix Add(SparseMatrix left, SparseMatrix right)
        {
            if (left is Sparse.CompressedColumn ccsLeft) { return Sparse.CompressedColumn.Add(ccsLeft, right); }
            else if (left is Sparse.CompressedRow crsLeft) { return Sparse.CompressedRow.Add(crsLeft, right); }
            else { throw new NotImplementedException($"The addition of a {left.GetType()} and a {right.GetType()} as {nameof(SparseMatrix)} is not implemented."); }
        }

        /// <summary>
        /// Computes the subtraction of two <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix"/> to subtract. </param>
        /// <param name="right"> Right <see cref="SparseMatrix"/> to subtract with. </param>
        /// <returns> The new <see cref="SparseMatrix"/> resulting from the subtraction. </returns>
        /// <exception cref="NotImplementedException"> The subtraction of these two matrix as <see cref="SparseMatrix"/> is not implemented. </exception>
        public static SparseMatrix Subtract(SparseMatrix left, SparseMatrix right)
        {
            if (left is Sparse.CompressedColumn ccsLeft) { return Sparse.CompressedColumn.Subtract(ccsLeft, right); }
            else if (left is Sparse.CompressedRow crsLeft) { return Sparse.CompressedRow.Subtract(crsLeft, right); }
            else { throw new NotImplementedException($"The subtraction of a {left.GetType()} and a {right.GetType()} as {nameof(SparseMatrix)} is not implemented."); }
        }


        /******************** Algebraic Multiplicative SemiGroup ********************/

        /// <summary>
        /// Computes the multiplication of two <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="SparseMatrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="SparseMatrix"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> The multiplication of these two matrix as <see cref="SparseMatrix"/> is not implemented. </exception>
        public static SparseMatrix Multiply(SparseMatrix left, SparseMatrix right)
        {
            if (left is Sparse.CompressedColumn ccsLeft) { return Sparse.CompressedColumn.Multiply(ccsLeft, right); }
            else if (left is Sparse.CompressedRow crsLeft) { return Sparse.CompressedRow.Multiply(crsLeft, right); }
            else { throw new NotImplementedException($"The multiplication of a {left.GetType()} and a {right.GetType()} as {nameof(SparseMatrix)} is not implemented."); }
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Computes the scalar multiplication of a <see cref="SparseMatrix"/> with a <see cref="double"/>-precision real number on the left.
        /// </summary>
        /// <param name="factor"> <see cref="double"/>-precision real number. </param>
        /// <param name="operand"> <see cref="SparseMatrix"/> to multiply on the left. </param>
        /// <returns> The new <see cref="SparseMatrix"/> resulting from the scalar multiplication. </returns>
        /// <exception cref="NotImplementedException"> The scalar multiplication on the left of the operand as a <see cref="SparseMatrix"/> is not implemented. </exception>
        public static SparseMatrix Multiply(double factor, SparseMatrix operand)
        {
            if (operand is Sparse.CompressedColumn ccsOperand) { return Sparse.CompressedColumn.Multiply(factor, ccsOperand); }
            else if (operand is Sparse.CompressedRow crsOperand) { return Sparse.CompressedRow.Multiply(factor, crsOperand); }
            else { throw new NotImplementedException($"The scalar multiplication on the left of {operand.GetType()} as a {nameof(SparseMatrix)} is not implemented."); }
        }

        /// <summary>
        /// Computes the scalar multiplication of a <see cref="SparseMatrix"/> with a <see cref="double"/>-precision real number on the right.
        /// </summary>
        /// <param name="operand"> <see cref="SparseMatrix"/> to multiply on the right. </param>
        /// <param name="factor"> <see cref="double"/>-precision real number. </param>
        /// <returns> The new <see cref="SparseMatrix"/> resulting from the scalar multiplication. </returns>
        /// <exception cref="NotImplementedException"> The scalar multiplication on the right of the operand as a <see cref="SparseMatrix"/> is not implemented. </exception>
        public static SparseMatrix Multiply(SparseMatrix operand, double factor)
        {
            if (operand is Sparse.CompressedColumn ccsOperand) { return Sparse.CompressedColumn.Multiply(ccsOperand, factor); }
            else if (operand is Sparse.CompressedRow crsOperand) { return Sparse.CompressedRow.Multiply(crsOperand, factor); }
            else { throw new NotImplementedException($"The scalar multiplication on the right of {operand.GetType()} as a {nameof(SparseMatrix)} is not implemented."); }
        }


        /// <summary>
        /// Computes the scalar division of a <see cref="SparseMatrix"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="operand"> <see cref="SparseMatrix"/> to divide. </param>
        /// <param name="divisor"> <see cref="double"/>-precision real number to divide with. </param>
        /// <returns> The new <see cref="SparseMatrix"/> resulting from the scalar division. </returns>
        /// <exception cref="NotImplementedException"> The scalar division of the operand as a <see cref="SparseMatrix"/> is not implemented. </exception>
        public static SparseMatrix Divide(SparseMatrix operand, double divisor)
        {
            if (operand is Sparse.CompressedColumn ccsOperand) { return Sparse.CompressedColumn.Divide(ccsOperand, divisor); }
            else if (operand is Sparse.CompressedRow crsOperand) { return Sparse.CompressedRow.Divide(crsOperand, divisor); }
            else { throw new NotImplementedException($"The scalar division of {operand.GetType()} as a {nameof(SparseMatrix)} is not implemented."); }
        }


        /******************** Other Operations ********************/

        /// <summary>
        /// Computes the right multiplication of a <see cref="SparseMatrix"/> with a <see cref="Vector"/> : <c>A*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="SparseMatrix"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The right multiplication of the matrix as a <see cref="SparseMatrix"/> with a <see cref="Vector"/> is not implemented.
        /// </exception>
        public static Vector Multiply(SparseMatrix matrix, Vector vector)
        {
            if (matrix is Sparse.CompressedColumn ccsMatrix) { return Sparse.CompressedColumn.Multiply(ccsMatrix, vector); }
            else if (matrix is Sparse.CompressedRow crsMatrix) { return Sparse.CompressedRow.Multiply(crsMatrix, vector); }
            else { throw new NotImplementedException($"The right multiplication of a {matrix.GetType()} as a {nameof(SparseMatrix)} and a {vector.GetType()} is not implemented."); }
        }

        /// <summary>
        /// Computes the right multiplication of a <see cref="SparseMatrix"/> with a <see cref="DenseVector"/> : <c>A*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="SparseMatrix"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="DenseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The right multiplication of the matrix as a <see cref="SparseMatrix"/> with a <see cref="DenseVector"/> is not implemented.
        /// </exception>
        public static DenseVector Multiply(SparseMatrix matrix, DenseVector vector)
        {
            if (matrix is Sparse.CompressedColumn ccsMatrix) { return Sparse.CompressedColumn.Multiply(ccsMatrix, vector); }
            else if (matrix is Sparse.CompressedRow crsMatrix) { return Sparse.CompressedRow.Multiply(crsMatrix, vector); }
            else { throw new NotImplementedException($"The right multiplication of a {matrix.GetType()} as a {nameof(SparseMatrix)} and a {vector.GetType()} is not implemented."); }
        }

        /// <summary>
        /// Computes the right multiplication of a <see cref="SparseMatrix"/> with a <see cref="SparseVector"/> : <c>A*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="SparseMatrix"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="SparseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="SparseVector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The right multiplication of the matrix as a <see cref="SparseMatrix"/> with a <see cref="SparseVector"/> is not implemented.
        /// </exception>
        public static SparseVector Multiply(SparseMatrix matrix, SparseVector vector)
        {
            if (matrix is Sparse.CompressedColumn ccsMatrix) { return Sparse.CompressedColumn.Multiply(ccsMatrix, vector); }
            else if (matrix is Sparse.CompressedRow crsMatrix) { return Sparse.CompressedRow.Multiply(crsMatrix, vector); }
            else { throw new NotImplementedException($"The right multiplication of a {matrix.GetType()} as a {nameof(SparseMatrix)} and a {vector.GetType()} is not implemented."); }
        }


        /// <summary>
        /// Computes the right multiplication of a transposed <see cref="SparseMatrix"/> with a <see cref="Vector"/> : <c>At*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="SparseMatrix"/> to transpose then multiply on the right. </param>
        /// <param name="vector"> <see cref="Vector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The right multiplication of the transposed matrix as a <see cref="SparseMatrix"/> with a <see cref="Vector"/> is not implemented.
        /// </exception>
        public static Vector TransposeMultiply(SparseMatrix matrix, Vector vector)
        {
            if (matrix is Sparse.CompressedColumn ccsMatrix) { return Sparse.CompressedColumn.TransposeMultiply(ccsMatrix, vector); }
            else if (matrix is Sparse.CompressedRow crsMatrix) { return Sparse.CompressedRow.TransposeMultiply(crsMatrix, vector); }
            else { throw new NotImplementedException($"The right multiplication of a transposed {matrix.GetType()} as a {nameof(SparseMatrix)} and a {vector.GetType()} is not implemented."); }

        }

        /// <summary>
        /// Computes the right multiplication of a transposed <see cref="SparseMatrix"/> with a <see cref="DenseVector"/> : <c>At*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="SparseMatrix"/> to transpose then multiply on the right. </param>
        /// <param name="vector"> <see cref="DenseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The right multiplication of the transposed matrix as a <see cref="SparseMatrix"/> with a <see cref="DenseVector"/> is not implemented.
        /// </exception>
        public static DenseVector TransposeMultiply(SparseMatrix matrix, DenseVector vector)
        {
            if (matrix is Sparse.CompressedColumn ccsMatrix) { return Sparse.CompressedColumn.TransposeMultiply(ccsMatrix, vector); }
            else if (matrix is Sparse.CompressedRow crsMatrix) { return Sparse.CompressedRow.TransposeMultiply(crsMatrix, vector); }
            else { throw new NotImplementedException($"The right multiplication of a transposed {matrix.GetType()} as a {nameof(SparseMatrix)} and a {vector.GetType()} is not implemented."); }

        }

        /// <summary>
        /// Computes the right multiplication of a transposed <see cref="SparseMatrix"/> with a <see cref="SparseVector"/> : <c>At*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="SparseMatrix"/> to transpose then multiply on the right. </param>
        /// <param name="vector"> <see cref="SparseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="SparseVector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The right multiplication of the transposed matrix as a <see cref="SparseMatrix"/> with a <see cref="SparseVector"/> is not implemented.
        /// </exception>
        public static SparseVector TransposeMultiply(SparseMatrix matrix, SparseVector vector)
        {
            if (matrix is Sparse.CompressedColumn ccsMatrix) { return Sparse.CompressedColumn.TransposeMultiply(ccsMatrix, vector); }
            else if (matrix is Sparse.CompressedRow crsMatrix) { return Sparse.CompressedRow.TransposeMultiply(crsMatrix, vector); }
            else { throw new NotImplementedException($"The right multiplication of a transposed {matrix.GetType()} as a {nameof(SparseMatrix)} and a {vector.GetType()} is not implemented."); }

        }

        #endregion


        #region Explicit : Additive.IAbelianGroup<SparseMatrix>

        /******************** Properties ********************/

        /// <inheritdoc/>
        bool Alg_Fund.IAddable<SparseMatrix>.IsAssociative => true;

        /// <inheritdoc/>
        bool Alg_Fund.IAddable<SparseMatrix>.IsCommutative => true;


        /******************** Methods ********************/

        /// <inheritdoc/>
        SparseMatrix Alg_Fund.IAddable<SparseMatrix>.Add(SparseMatrix right) { return SparseMatrix.Add(this, right); }

        /// <inheritdoc/>
        SparseMatrix Alg_Fund.ISubtractable<SparseMatrix>.Subtract(SparseMatrix right) { return SparseMatrix.Subtract(this, right); }

        /// <inheritdoc/>
        bool Alg_Set.Additive.IGroup<SparseMatrix>.Opposite()
        {
            this.Opposite();
            return true;
        }

        /// <inheritdoc/>
        SparseMatrix Alg_Fund.IZeroable<SparseMatrix>.Zero() { return SparseMatrix.Zero(RowCount, ColumnCount); }

        #endregion

        #region Explicit : Multiplicative.SemiGroup<SparseMatrix>

        /******************** Properties ********************/

        /// <inheritdoc/>
        bool Alg_Fund.IMultiplicable<SparseMatrix>.IsAssociative => true;

        /// <inheritdoc/>
        bool Alg_Fund.IMultiplicable<SparseMatrix>.IsCommutative => false;


        /******************** Methods ********************/

        /// <inheritdoc/>
        SparseMatrix Alg_Fund.IMultiplicable<SparseMatrix>.Multiply(SparseMatrix right) { return SparseMatrix.Multiply(this, right); }

        #endregion

        #region Explicit : IGroupAction<Double,SparseMatrix>

        /******************** Methods ********************/

        /// <inheritdoc/>
        SparseMatrix Alg_Set.IGroupAction<double, SparseMatrix>.Multiply(double factor) { return SparseMatrix.Multiply(this, factor); }

        /// <inheritdoc/>
        SparseMatrix Alg_Set.IGroupAction<double, SparseMatrix>.Divide(double divisor) { return SparseMatrix.Divide(this, divisor); }

        #endregion
    }
}
