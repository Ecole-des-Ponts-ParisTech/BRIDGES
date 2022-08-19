using System;

using Alg_Fund = BRIDGES.Algebra.Fundamentals;
using Alg_Meas = BRIDGES.Algebra.Measure;
using Alg_Set = BRIDGES.Algebra.Sets;


namespace BRIDGES.LinearAlgebra.Vectors
{
    /// <summary>
    /// Class defining a vector.
    /// </summary>
    public abstract class Vector
        : Alg_Set.Additive.IAbelianGroup<Vector>, Alg_Set.IGroupAction<double, Vector>,
          Alg_Meas.INorm<Vector>,
          IEquatable<Vector>
    {
        #region Properties

        /// <summary>
        /// Number of component of the current vector.
        /// </summary>
        public abstract int Size { get; }

        /// <summary>
        /// Gets or sets the component at a given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public abstract double this[int index] { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Vector"/> class.
        /// </summary>
        protected Vector()
        {
            /* Do nothing */
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Returns the neutral <see cref="Vector"/> for the addition. 
        /// </summary>
        /// <param name="size"> Number of component of the current vector. </param>
        /// <returns> The <see cref="Vector"/> of the given size, with 0.0 on each coordinate. </returns>
        public static Vector Zero(int size)
        {
            return SparseVector.Zero(size);
        }


        /// <summary>
        /// Returns the unit <see cref="Vector"/> of a given <paramref name="size"/>, with one at the given row <paramref name="index"/> and zeros elsewhere.
        /// </summary>
        /// <param name="size"> Size of the new <see cref="Vector"/>. </param>
        /// <param name="index"> Index of the standard vector, i.e of the component equal to one. </param>
        /// <returns> The new <see cref="Vector"/> representing the standard vector. </returns>
        public static SparseVector StandardVector(int size, int index)
        {
            return SparseVector.StandardVector(size, index);
        }

        #endregion

        #region Static Methods

        /******************** Algebraic Additive Group ********************/

        /// <summary>
        /// Computes the addition of two <see cref="Vector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vector"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Vector"/> for the addition. </param>
        /// <returns> The new <see cref="Vector"/> resulting from the addition. </returns>
        /// <exception cref="NotImplementedException"> The addition of these two vector types is not implemented. </exception>
        public static Vector Add(Vector left, Vector right)
        {
            if (left is DenseVector denseLeft) { return DenseVector.Add(denseLeft, right); }
            else if (right is DenseVector denseRight) { return DenseVector.Add(left, denseRight); }
            else if (left is SparseVector sparseLeft && right is SparseVector sparseRight) { return SparseVector.Add(sparseLeft, sparseRight); }
            else { throw new NotImplementedException($"The addition of a {left.GetType()} and a {right.GetType()} as Vector is not implemented."); }
        }

        /// <summary>
        /// Computes the subtraction of two <see cref="Vector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vector"/> to subtract. </param>
        /// <param name="right"> Right <see cref="Vector"/> to subtract with. </param>
        /// <returns> The new <see cref="Vector"/> resulting from the subtraction. </returns>
        /// <exception cref="NotImplementedException"> The subtraction of these two types as vector is not implemented. </exception>
        public static Vector Subtract(Vector left, Vector right)
        {
            if (left is DenseVector denseLeft) { return DenseVector.Subtract(denseLeft, right); }
            else if (right is DenseVector denseRight) { return DenseVector.Subtract(left, denseRight); }
            else if (left is SparseVector sparseLeft && right is SparseVector sparseRight) { return SparseVector.Subtract(sparseLeft, sparseRight); }
            else { throw new NotImplementedException($"The subtraction of a {left.GetType()} and a {right.GetType()} as Vector is not implemented."); }
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Computes the scalar multiplication of a <see cref="Vector"/> with a <see cref="double"/>-precision real number on the left.
        /// </summary>
        /// <param name="factor"> <see cref="double"/>-precision real number. </param>
        /// <param name="operand"> <see cref="Vector"/> to multiply on the left. </param>
        /// <returns> The new <see cref="Vector"/> resulting from the scalar multiplication. </returns>
        /// <exception cref="NotImplementedException"> The scalar multiplication on the left of the operand as a <see cref="Vector"/> is not implemented. </exception>
        public static Vector Multiply(double factor, Vector operand)
        {
            if (operand is DenseVector denseOperand) { return DenseVector.Multiply(factor, denseOperand); }
            else if (operand is SparseVector sparseOperand) { return DenseVector.Multiply(factor, sparseOperand); }
            else { throw new NotImplementedException($"The scalar multiplication on the left of {operand.GetType()} as a Vector is not implemented."); }
        }

        /// <summary>
        /// Computes the scalar multiplication of a <see cref="Vector"/> with a <see cref="double"/>-precision real number on the right.
        /// </summary>
        /// <param name="operand"> <see cref="Vector"/> to multiply on the right. </param>
        /// <param name="factor"> <see cref="double"/>-precision real number. </param>
        /// <returns> The new <see cref="Vector"/> resulting from the scalar multiplication. </returns>
        /// <exception cref="NotImplementedException"> The scalar multiplication on the right of the operand as a <see cref="Vector"/> is not implemented. </exception>
        public static Vector Multiply(Vector operand, double factor)
        {
            if (operand is DenseVector denseOperand) { return DenseVector.Multiply(denseOperand, factor); }
            else if (operand is SparseVector sparseOperand) { return DenseVector.Multiply(sparseOperand, factor); }
            else { throw new NotImplementedException($"The scalar multiplication on the right of {operand.GetType()} as a Vector is not implemented."); }
        }

        /// <summary>
        /// Computes the scalar division of a <see cref="Vector"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="operand"> <see cref="Vector"/> to divide. </param>
        /// <param name="divisor"> <see cref="double"/>-precision real number to divide with. </param>
        /// <returns> The new <see cref="Vector"/> resulting from the scalar division. </returns>
        /// <exception cref="NotImplementedException"> The scalar division of the operand as a <see cref="Vector"/> is not implemented. </exception>
        public static Vector Divide(Vector operand, double divisor)
        {
            if (operand is DenseVector denseOperand) { return DenseVector.Divide(denseOperand, divisor); }
            else if (operand is SparseVector sparseOperand) { return DenseVector.Divide(sparseOperand, divisor); }
            else { throw new NotImplementedException($"The scalar division of {operand.GetType()} as a Vector is not implemented."); }
        }


        /******************** Other Operations ********************/

        /// <summary>
        /// Computes the multiplication between the transposed left <see cref="Vector"/> and the right <see cref="Vector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vector"/> to transpose, then multiply. </param>
        /// <param name="right"> Right <see cref="Vector"/> to multiply. </param>
        /// <returns> The <see cref="double"/>-precision scalar resulting from the multiplication. </returns>
        /// <exception cref="ArgumentException"> This transpose-multiply operation of these two types as vector is not implemented. </exception>
        public static double TransposeMultiply(Vector left, Vector right)
        {
            if(left is DenseVector denseLeft)
            {
                if (right is DenseVector denseRight) { return DenseVector.TransposeMultiply(denseLeft, denseRight); }
                else if (right is SparseVector sparseRight) { return SparseVector.TransposeMultiply(denseLeft, sparseRight); }
                else { throw new NotImplementedException($"This operation between a {left.GetType()} and a {right.GetType()} as Vector is not implemented."); }
            }
            else if (left is DenseVector sparseLeft)
            {
                if (right is DenseVector denseRight) { return SparseVector.TransposeMultiply(sparseLeft, denseRight); }
                else if (right is SparseVector sparseRight) { return SparseVector.TransposeMultiply(sparseLeft, sparseRight); }
                else { throw new NotImplementedException($"This operation between a {left.GetType()} and a {right.GetType()} as Vector is not implemented."); }
            }
            else { throw new NotImplementedException($"This operation between a {left.GetType()} and a {right.GetType()} as Vector is not implemented."); }
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public abstract void Unitise();

        /// <summary>
        /// Computes the length of the current vector.
        /// </summary>
        /// <returns> The value of the vector length. </returns>
        public abstract double Length();

        /// <summary>
        /// Computes the squared length of the current vector.
        /// </summary>
        /// <returns> The value of the vector squared length. </returns>
        public abstract double SquaredLength();


        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public abstract bool Equals(Vector other);

        #endregion

        #region Other Methods

        /******************** Algebraic Additive Group ********************/

        /// <inheritdoc cref="Alg_Set.Additive.IGroup{T}.Opposite()"/>
        protected abstract void Opposite();

        #endregion


        #region Explicit : Additive.IAbelianGroup<Vector>

        /******************** Properties ********************/

        /// <inheritdoc/>
        bool Alg_Fund.IAddable<Vector>.IsAssociative => true;

        /// <inheritdoc/>
        bool Alg_Fund.IAddable<Vector>.IsCommutative => true;


        /******************** Methods ********************/

        /// <inheritdoc/>
        Vector Alg_Fund.IAddable<Vector>.Add(Vector right) { return Vector.Add(this, right); }

        /// <inheritdoc/>
        Vector Alg_Fund.ISubtractable<Vector>.Subtract(Vector right) { return Vector.Subtract(this, right); }

        /// <inheritdoc/>
        bool Alg_Set.Additive.IGroup<Vector>.Opposite()
        {
            this.Opposite();
            return true;
        }

        /// <inheritdoc/>
        Vector Alg_Fund.IZeroable<Vector>.Zero() { return Vector.Zero(Size); }

        #endregion

        #region Explicit : IGroupAction<Double,Vector>

        /******************** Methods ********************/

        /// <inheritdoc/>
        Vector Alg_Set.IGroupAction<double, Vector>.Multiply(double factor) { return Vector.Multiply(this, factor); }

        /// <inheritdoc/>
        Vector Alg_Set.IGroupAction<double, Vector>.Divide(double divisor) { return Vector.Divide(this, divisor); }

        #endregion

        #region Explicit : INorm<Vector>

        /// <inheritdoc/>
        double Alg_Meas.INorm<Vector>.Norm() { return this.Length(); }

        /// <inheritdoc/>
        double Alg_Meas.IMetric<Vector>.DistanceTo(Vector other)
        {
            Vector diff = Vector.Subtract(this, other);
            return diff.Length();
        }
        #endregion
    }
}
