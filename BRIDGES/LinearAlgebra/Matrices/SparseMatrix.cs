using System;


namespace BRIDGES.LinearAlgebra.Matrices
{
    /// <summary>
    /// Class defining a sparse matrix.
    /// </summary>
    public abstract class SparseMatrix : Matrix
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
        /// <exception cref="NotImplementedException"> The addition of these two matrix types as <see cref="SparseMatrix"/> is not implemented. </exception>
        public static SparseMatrix Add(SparseMatrix left, SparseMatrix right)
        {
            if (left is Sparse.CompressedColumn ccsLeft) { return Sparse.CompressedColumn.Add(ccsLeft, right); }
            else if (left is Sparse.CompressedRow crsLeft) { return Sparse.CompressedRow.Add(crsLeft, right); }
            else { throw new NotImplementedException($"The addition of a {left.GetType()} and a {right.GetType()} as SparseMatrix is not implemented."); }
        }

        /// <summary>
        /// Computes the subtraction of two <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix"/> to subtract. </param>
        /// <param name="right"> Right <see cref="SparseMatrix"/> to subtract with. </param>
        /// <returns> The new <see cref="SparseMatrix"/> resulting from the subtraction. </returns>
        /// <exception cref="NotImplementedException"> The subtraction of these two matrix types as <see cref="SparseMatrix"/> is not implemented. </exception>
        public static SparseMatrix Subtract(SparseMatrix left, SparseMatrix right)
        {
            if (left is Sparse.CompressedColumn ccsLeft) { return Sparse.CompressedColumn.Subtract(ccsLeft, right); }
            else if (left is Sparse.CompressedRow crsLeft) { return Sparse.CompressedRow.Subtract(crsLeft, right); }
            else { throw new NotImplementedException($"The subtraction of a {left.GetType()} and a {right.GetType()} as SparseMatrix is not implemented."); }
        }


        /******************** Algebraic Multiplicative SemiGroup ********************/

        /// <summary>
        /// Computes the multiplication of two <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="SparseMatrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="SparseMatrix"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> The multiplication of these two matrix types as <see cref="SparseMatrix"/> is not implemented. </exception>
        public static SparseMatrix Multiply(SparseMatrix left, SparseMatrix right)
        {
            if (left is Sparse.CompressedColumn ccsLeft) { return Sparse.CompressedColumn.Multiply(ccsLeft, right); }
            else if (left is Sparse.CompressedRow crsLeft) { return Sparse.CompressedRow.Multiply(crsLeft, right); }
            else { throw new NotImplementedException($"The multiplication of a {left.GetType()} and a {right.GetType()} as SparseMatrix is not implemented."); }
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
            else { throw new NotImplementedException($"The scalar multiplication on the left of {operand.GetType()} as a SparseMatrix is not implemented."); }
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
            else { throw new NotImplementedException($"The scalar multiplication on the right of {operand.GetType()} as a SparseMatrix is not implemented."); }
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
            else { throw new NotImplementedException($"The scalar division of {operand.GetType()} as a SparseMatrix is not implemented."); }
        }

        #endregion
    }
}
