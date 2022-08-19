using System;
using System.Collections.Generic;

using Alg_Fund = BRIDGES.Algebra.Fundamentals;
using Alg_Set = BRIDGES.Algebra.Sets;


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
        /// <returns> The value at the given row and index. </returns>
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

        #region Static Methods

        /******************** Algebraic Additive Group ********************/

        /// <summary>
        /// Computes the addition of two <see cref="Matrix"/>.
        /// </summary>
        /// <param name="left"> <see cref="Matrix"/> for the addition. </param>
        /// <param name="right"> <see cref="Matrix"/> for the addition. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the addition. </returns>
        public static Matrix Add(Matrix left, Matrix right)
        {
            return left.Add(right);
        }

        /// <summary>
        /// Computes the subtraction of two <see cref="Matrix"/>.
        /// </summary>
        /// <param name="left"> <see cref="Matrix"/> to subtract. </param>
        /// <param name="right"> <see cref="Matrix"/> to subtract with. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the subtraction. </returns>
        public static Matrix Subtract(Matrix left, Matrix right)
        {
            return left.Subtract(right);
        }


        /******************** Algebraic Multiplicative SemiGroup ********************/

        /// <summary>
        /// Computes the multiplication of two <see cref="Matrix"/>.
        /// </summary>
        /// <param name="left"> <see cref="Matrix"/> for the multiplication. </param>
        /// <param name="right"> <see cref="Matrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the multiplication. </returns>
        public static Matrix Multiply(Matrix left, Matrix right)
        {
            return left.Multiply(right);
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Computes the scalar multiplication of a <see cref="Matrix"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="factor"> <see cref="double"/>-precision real number. </param>
        /// <param name="operand"> <see cref="Matrix"/> to multiply. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the scalar multiplication. </returns>
        public static Matrix Multiply(double factor, Matrix operand)
        {
            return operand.Multiply(factor);
        }

        /// <summary>
        /// Computes the scalar division of a <see cref="Matrix"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="operand"> <see cref="Matrix"/> to divide. </param>
        /// <param name="divisor"> <see cref="double"/>-precision real number to divide with. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the scalar division. </returns>
        public static Matrix Divide(Matrix operand, double divisor)
        {
            return operand.Divide(divisor);
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

        /// <summary>
        /// Computes the addition of the current <see cref="Matrix"/> with another <see cref="Matrix"/>.
        /// </summary>
        /// <param name="right"> <see cref="Matrix"/> to add with on the right. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the addition. </returns>
        protected abstract Matrix Add(Matrix right);
        
        /// <summary>
        /// Computes the subtraction of the current <see cref="Matrix"/> with another <see cref="Matrix"/>.
        /// </summary>
        /// <param name="right"> <see cref="Matrix"/> to subtract with on the right. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the subtraction. </returns>
        protected abstract Matrix Subtract(Matrix right);

        /// <summary>
        /// Opposes the current matrix.
        /// </summary>
        protected abstract void Opposite();

        /// <summary>
        /// Gets the neutral <see cref="Matrix"/> of the addition.
        /// </summary>
        /// <returns> The neutral <see cref="Matrix"/> of the addition. </returns>
        protected abstract Matrix Zero();


        /******************** Algebraic Multiplicative SemiGroup ********************/

        /// <summary>
        /// Computes the multiplication of the current <see cref="Matrix"/> with another <see cref="Matrix"/>.
        /// </summary>
        /// <param name="right"> <see cref="Matrix"/> to multiply with on the right. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the addition. </returns>
        protected abstract Matrix Multiply(Matrix right);


        /******************** Group Action ********************/

        /// <summary>
        /// Computes the scalar multiplication the current <see cref="Matrix"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="factor"> <see cref="double"/>-precision real number. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the scalar multiplication. </returns>
        protected abstract Matrix Multiply(double factor);

        /// <summary>
        /// Computes the scalar division of the current <see cref="Matrix"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="divisor"> <see cref="double"/>-precision real number to divide with. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the scalar division. </returns>
        protected abstract Matrix Divide(double divisor);

        #endregion


        #region Explicit : Additive.IAbelianGroup<Matrix>

        /******************** Properties ********************/

        /// <inheritdoc/>
        bool Alg_Fund.IAddable<Matrix>.IsAssociative => true;

        /// <inheritdoc/>
        bool Alg_Fund.IAddable<Matrix>.IsCommutative => true;


        /******************** Methods ********************/

        /// <inheritdoc/>
        Matrix Alg_Fund.IAddable<Matrix>.Add(Matrix right) { return this.Add(right); }

        /// <inheritdoc/>
        Matrix Alg_Fund.ISubtractable<Matrix>.Subtract(Matrix right) { return this.Subtract(right); }

        /// <inheritdoc/>
        bool Alg_Set.Additive.IGroup<Matrix>.Opposite()
        {
            this.Opposite();
            return true;
        }

        /// <inheritdoc/>
        Matrix Alg_Fund.IZeroable<Matrix>.Zero() { return this.Zero(); }

        #endregion

        #region Explicit : Multiplicative.SemiGroup<Matrix>

        /******************** Properties ********************/

        /// <inheritdoc/>
        bool Alg_Fund.IMultiplicable<Matrix>.IsAssociative => true;

        /// <inheritdoc/>
        bool Alg_Fund.IMultiplicable<Matrix>.IsCommutative => false;


        /******************** Methods ********************/

        /// <inheritdoc/>
        Matrix Alg_Fund.IMultiplicable<Matrix>.Multiply(Matrix right) { return this.Multiply(right); }

        #endregion

        #region Explicit : IGroupAction<Double,Matrix>

        /******************** Methods ********************/

        /// <inheritdoc/>
        Matrix Alg_Set.IGroupAction<double, Matrix>.Multiply(double factor) { return this.Multiply(factor); }

        /// <inheritdoc/>
        Matrix Alg_Set.IGroupAction<double, Matrix>.Divide(double divisor) { return this.Divide(divisor); }

        #endregion
    }
}
