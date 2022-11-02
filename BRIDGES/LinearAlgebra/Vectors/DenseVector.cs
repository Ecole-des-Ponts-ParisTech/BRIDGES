using System;
using System.Collections.Generic;

using Alg_Fund = BRIDGES.Algebra.Fundamentals;
using Alg_Meas = BRIDGES.Algebra.Measure;
using Alg_Set = BRIDGES.Algebra.Sets;


namespace BRIDGES.LinearAlgebra.Vectors
{
    /// <summary>
    /// Class defining a dense vector.
    /// </summary>
    public sealed class DenseVector : Vector,
          Alg_Set.Additive.IAbelianGroup<DenseVector>, Alg_Set.IGroupAction<double, DenseVector>,
          Alg_Meas.INorm<DenseVector>,
          IEquatable<DenseVector>
    {
        #region Fields

        /// <summary>
        /// Components of the current <see cref="DenseVector"/>.
        /// </summary>
        private double[] _components;

        #endregion

        #region Properties

        /// <inheritdoc/>
        public override int Size
        {
            get { return _components.Length; }
        }

        /// <inheritdoc/>
        public override double this[int index]
        {  
            get { return _components[index]; }
            set { _components[index] = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="DenseVector"/> class of given size, containing only zeros.
        /// </summary>
        /// <param name="size"> Number of component of the current <see cref="DenseVector"/>. </param>
        public DenseVector(int size)
        {
            _components = new double[size];
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DenseVector"/> class from its components.
        /// </summary>
        /// <param name="components"> Components of the new <see cref="DenseVector"/>. </param>
        public DenseVector(IEnumerable<double> components)
        {
            List<double> temp = new List<double>();
            foreach(double component in components)
            {
                temp.Add(component);
            }

            _components = temp.ToArray();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DenseVector"/> class from another <see cref="DenseVector"/>.
        /// </summary>
        /// <param name="vector"> <see cref="DenseVector"/> to copy. </param>
        public DenseVector(DenseVector vector)
        {
            int size = vector.Size;

            _components = new double[size];
            for (int i = 0; i < size; i++)
            {
                _components[i] = vector._components[i];
            }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DenseVector"/> class from a <see cref="SparseVector"/>.
        /// </summary>
        /// <param name="vector"> <see cref="SparseVector"/> to copy. </param>
        public DenseVector(SparseVector vector)
        {
            int size = vector.Size;

            _components = new double[size];
            foreach(var component in vector.GetNonZeros())
            {
                _components[component.RowIndex] = component.Value;
            }
        }


        /// <summary>
        /// Initialises a new instance of the <see cref="DenseVector"/> class from its components.
        /// </summary>
        /// <param name="components"> Components of the new <see cref="DenseVector"/>. </param>
        internal DenseVector(double[] components)
        {
            _components = components;
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Returns the neutral <see cref="DenseVector"/> for the addition.
        /// </summary>
        /// <param name="size"> Number of component of the current <see cref="DenseVector"/>. </param>
        /// <returns> The <see cref="DenseVector"/> of the given size and with zeros on every coordinates. </returns>
        public static new DenseVector Zero(int size)
        {
            return new DenseVector(size);
        }


        /// <summary>
        /// Returns the unit <see cref="DenseVector"/> of a given <paramref name="size"/>, with one at the given row <paramref name="index"/> and zeros elsewhere.
        /// </summary>
        /// <param name="size"> Size of the new <see cref="DenseVector"/>. </param>
        /// <param name="index"> Index of the standard vector, i.e of the component equal to one. </param>
        /// <returns> The new <see cref="DenseVector"/> representing the standard vector. </returns>
        public static new DenseVector StandardVector(int size, int index)
        {
            DenseVector vector = new DenseVector(size);
            vector[index] = 1.0;

            return vector;
        }

        #endregion

        #region Static Methods

        /******************** Algebraic Additive Group ********************/

        /// <summary>
        /// Computes the addition of two <see cref="DenseVector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector"/> for the addition. </param>
        /// <param name="right"> Right <see cref="DenseVector"/> for the addition. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the addition. </returns>
        /// <exception cref="ArgumentException"> The size of the vectors should be the same. </exception>
        public static DenseVector Add(DenseVector left, DenseVector right)
        {
            if (left.Size != right.Size) { throw new ArgumentException("The size of the vectors should be the same."); }

            int size = left.Size;

            double[] components = new double[size];
            for (int i = 0; i < size; i++)
            {
                components[i] = left._components[i] + right._components[i];
            }

            return new DenseVector(components);
        }

        /// <summary>
        /// Computes the subtraction of two <see cref="DenseVector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector"/> to subtract. </param>
        /// <param name="right"> Right <see cref="DenseVector"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the subtraction. </returns>
        /// <exception cref="ArgumentException"> The size of the vectors should be the same. </exception>
        public static DenseVector Subtract(DenseVector left, DenseVector right)
        {
            if (left.Size != right.Size) { throw new ArgumentException("The size of the vectors should be the same."); }

            int size = left.Size;

            double[] components = new double[size];
            for (int i = 0; i < size; i++)
            {
                components[i] = left._components[i] - right._components[i];
            }

            return new DenseVector(components);
        }


        /******************** Vector Embedding ********************/

        /// <summary>
        /// Computes the addition of a <see cref="DenseVector"/> with a <see cref="Vector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Vector"/> for the addition. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the addition. </returns>
        /// <exception cref="NotImplementedException"> The addition of a <see cref="DenseVector"/> with the right vector type is not implemented. </exception>
        public static DenseVector Add(DenseVector left, Vector right)
        {
            if (right is DenseVector denseRight) { return DenseVector.Add(left, denseRight); }
            else if (right is SparseVector sparseRight) { return DenseVector.Add(left, sparseRight); }
            else { throw new NotImplementedException($"The addition of a {left.GetType()} and a {right.GetType()} is not implemented."); }
        }

        /// <summary>
        /// Computes the addition of a <see cref="Vector"/> with a <see cref="DenseVector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vector"/> for the addition. </param>
        /// <param name="right"> Right <see cref="DenseVector"/> for the addition. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the addition. </returns>
        /// <exception cref="NotImplementedException"> The addition of the left vector type with a <see cref="DenseVector"/> is not implemented. </exception>
        public static DenseVector Add(Vector left, DenseVector right)
        {
            if (left is DenseVector denseLeft) { return DenseVector.Add(denseLeft, right); }
            else if (left is SparseVector sparseleft) { return DenseVector.Add(sparseleft, right); }
            else { throw new NotImplementedException($"The addition of a {left.GetType()} and a {right.GetType()} is not implemented."); }
        }


        /// <summary>
        /// Computes the subtraction of a <see cref="DenseVector"/> with a <see cref="Vector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector"/> to subtract. </param>
        /// <param name="right"> Right <see cref="Vector"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the subtraction. </returns>
        /// <exception cref="NotImplementedException"> The subtraction of a <see cref="DenseVector"/> with the right vector type is not implemented. </exception>
        public static DenseVector Subtract(DenseVector left, Vector right)
        {
            if (right is DenseVector denseRight) { return DenseVector.Subtract(left, denseRight); }
            else if (right is SparseVector sparseRight) { return DenseVector.Subtract(left, sparseRight); }
            else { throw new NotImplementedException($"The subtraction of a {left.GetType()} and a {right.GetType()} is not implemented."); }
        }

        /// <summary>
        /// Computes the subtraction of a <see cref="Vector"/> with a <see cref="DenseVector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vector"/> to subtract. </param>
        /// <param name="right"> Right <see cref="DenseVector"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the subtraction. </returns>
        /// <exception cref="NotImplementedException"> The subtraction of the left vector type with a <see cref="DenseVector"/> is not implemented. </exception>
        public static DenseVector Subtract(Vector left, DenseVector right)
        {
            if (left is DenseVector denseLeft) { return DenseVector.Subtract(denseLeft, right); }
            else if (left is SparseVector sparseleft) { return DenseVector.Subtract(sparseleft, right); }
            else { throw new NotImplementedException($"The addition of a {left.GetType()} and a {right.GetType()} is not implemented."); }
        }


        /******************** Sparse Vector Embedding ********************/

        /// <summary>
        /// Computes the addition of a <see cref="DenseVector"/> with a <see cref="SparseVector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector"/> for the addition. </param>
        /// <param name="right"> Right <see cref="SparseVector"/> for the addition. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the addition. </returns>
        /// <exception cref="ArgumentException"> The size of the vectors should be the same. </exception>
        public static DenseVector Add(DenseVector left, SparseVector right)
        {
            if (left.Size != right.Size) { throw new ArgumentException("The size of the vectors should be the same."); }

            DenseVector result = new DenseVector(left);

            foreach (var component in right.GetNonZeros())
            {
                result._components[component.RowIndex] = result._components[component.RowIndex] + component.Value;
            }

            return result;
        }

        /// <summary>
        /// Computes the addition of a <see cref="SparseVector"/> with a <see cref="DenseVector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseVector"/> for the addition. </param>
        /// <param name="right"> Right <see cref="DenseVector"/> for the addition. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the addition. </returns>
        /// <exception cref="ArgumentException"> The size of the vectors should be the same. </exception>
        public static DenseVector Add(SparseVector left, DenseVector right)
        {
            if (left.Size != right.Size) { throw new ArgumentException("The size of the vectors should be the same."); }

            DenseVector result = new DenseVector(right);

            foreach (var component in left.GetNonZeros())
            {
                result._components[component.RowIndex] = component.Value + result._components[component.RowIndex];
            }

            return result;
        }


        /// <summary>
        /// Computes the subtraction of a <see cref="DenseVector"/> with a <see cref="SparseVector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector"/> to subtract. </param>
        /// <param name="right"> Right <see cref="SparseVector"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the subtraction. </returns>
        /// <exception cref="ArgumentException"> The size of the vectors should be the same. </exception>
        public static DenseVector Subtract(DenseVector left, SparseVector right)
        {
            if (left.Size != right.Size) { throw new ArgumentException("The size of the vectors should be the same."); }

            DenseVector result = new DenseVector(left);

            foreach (var component in right.GetNonZeros())
            {
                result._components[component.RowIndex] = result._components[component.RowIndex] - component.Value;
            }

            return result;
        }

        /// <summary>
        /// Computes the subtraction of a <see cref="SparseVector"/> with a <see cref="DenseVector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseVector"/> to subtract. </param>
        /// <param name="right"> Right <see cref="DenseVector"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the subtraction. </returns>
        /// <exception cref="ArgumentException"> The size of the vectors should be the same. </exception>
        public static DenseVector Subtract(SparseVector left, DenseVector right)
        {
            if (left.Size != right.Size) { throw new ArgumentException("The size of the vectors should be the same."); }

            DenseVector result = DenseVector.Multiply(-1.0, right);

            foreach (var component in left.GetNonZeros())
            {
                result._components[component.RowIndex] = component.Value + result._components[component.RowIndex];
            }

            return result;
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Computes the scalar multiplication of a <see cref="DenseVector"/> with a <see cref="double"/>-precision real number on the left.
        /// </summary>
        /// <param name="factor"> <see cref="double"/>-precision real number. </param>
        /// <param name="operand"> <see cref="DenseVector"/> to multiply on the left. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the scalar multiplication. </returns>
        public static DenseVector Multiply(double factor, DenseVector operand)
        {
            int size = operand.Size;

            double[] components = new double[size];
            for (int i = 0; i < size; i++)
            {
                components[i] = factor * operand._components[i];
            }

            return new DenseVector(components);
        }

        /// <summary>
        /// Computes the scalar multiplication of a <see cref="DenseVector"/> with a <see cref="double"/>-precision real number on the right.
        /// </summary>
        /// <param name="operand"> <see cref="DenseVector"/> to multiply on the right. </param>
        /// <param name="factor"> <see cref="double"/>-precision real number. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the scalar multiplication. </returns>
        public static DenseVector Multiply(DenseVector operand, double factor)
        {
            int size = operand.Size;

            double[] components = new double[size];
            for (int i = 0; i < size; i++)
            {
                components[i] = operand._components[i] * factor;
            }

            return new DenseVector(components);
        }


        /// <summary>
        /// Computes the scalar division of a <see cref="DenseVector"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="operand"> <see cref="DenseVector"/> to divide. </param>
        /// <param name="divisor"> <see cref="double"/>-precision real number to divide with. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the scalar division. </returns>
        public static DenseVector Divide(DenseVector operand, double divisor)
        {
            int size = operand.Size;

            double[] components = new double[size];
            for (int i = 0; i < size; i++)
            {
                components[i] = operand._components[i] / divisor;
            }

            return new DenseVector(components);
        }


        /******************** Other Operations ********************/

        /// <summary>
        /// Computes the multiplication between the transposed left <see cref="DenseVector"/> and the right <see cref="DenseVector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector"/> to transpose, then multiply. </param>
        /// <param name="right"> Right <see cref="DenseVector"/> to multiply. </param>
        /// <returns> The <see cref="double"/>-precision scalar resulting from the multiplication. </returns>
        /// <exception cref="ArgumentException"> The size of the vectors should be the same. </exception>
        public static double TransposeMultiply(DenseVector left, DenseVector right)
        {
            if (left.Size != right.Size) { throw new ArgumentException("The size of the vectors should be the same."); }

            int size = left.Size;
            double result = 0.0;

            for (int i = 0; i < size; i++)
            {
                result += left._components[i] * right._components[i];
            }

            return result;
        }

        /// <summary>
        /// Computes the multiplication between the transposed left <see cref="DenseVector"/> and the right <see cref="SparseVector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector"/> to transpose, then multiply. </param>
        /// <param name="right"> Right <see cref="SparseVector"/> to multiply. </param>
        /// <returns> The <see cref="double"/>-precision scalar resulting from the multiplication. </returns>
        /// <exception cref="ArgumentException"> The size of the vectors should be the same. </exception>
        public static double TransposeMultiply(DenseVector left, SparseVector right)
        {
            if (left.Size != right.Size) { throw new ArgumentException("The size of the vectors should be the same."); }
            

            double result = 0.0;

            foreach(var component in right.GetNonZeros())
            {
                result += left[component.RowIndex] * component.Value;
            }

            return result;
        }

        /// <summary>
        /// Computes the multiplication between the transposed left <see cref="SparseVector"/> and the right <see cref="DenseVector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseVector"/> to transpose, then multiply. </param>
        /// <param name="right"> Right <see cref="DenseVector"/> to multiply. </param>
        /// <returns> The <see cref="double"/>-precision scalar resulting from the multiplication. </returns>
        /// <exception cref="ArgumentException"> The size of the vectors should be the same. </exception>
        public static double TransposeMultiply(SparseVector left, DenseVector right)
        {
            if (left.Size != right.Size) { throw new ArgumentException("The size of the vectors should be the same."); }


            double result = 0.0;

            foreach(var component in left.GetNonZeros())
            {
                result += component.Value * right[component.RowIndex];
            }

            return result;
        }


        /******************** On DenseVector Sets ********************/

        /// <summary>
        /// Ortho-normalise the set of <see cref="DenseVector"/> using a Gram-Schimdt process. 
        /// </summary>
        /// <remarks> If the vectors are not linearly independent the number of vectors will change. </remarks>
        /// <param name="vectors"> Set of <see cref="DenseVector"/> to operate on. </param>
        /// <returns> The ortho-normal set of <see cref="DenseVector"/>. </returns>
        public static DenseVector[] GramSchmidt(IEnumerable<DenseVector> vectors)
        {
            List<DenseVector> results = new List<DenseVector>();

            foreach(DenseVector denseVector in vectors)
            {
                DenseVector vector = new DenseVector(denseVector);

                for (int i_R = 0; i_R < results.Count; i_R++)
                {
                    DenseVector result = new DenseVector(results[i_R]);

                    double numerator = DenseVector.TransposeMultiply(vector, result);
                    double denominator = result.SquaredLength();

                    result = DenseVector.Multiply(numerator / denominator, result);

                    vector = DenseVector.Subtract(vector, result);
                }

                double length = vector.Length();
                if (length > Settings.AbsolutePrecision)
                {
                    vector = DenseVector.Divide(vector, length);
                    results.Add(vector);
                }
            }

            return results.ToArray();
        }

        #endregion

        #region Public Methods

        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public bool Equals(DenseVector other)
        {
            if (Size != other.Size) { return false; }

            int size = _components.Length;
            for (int i = 0; i < size; i++)
            {
                if (_components[i] != other._components[i]) { return false; }
            }

            return true;
        }

        #endregion


        #region Override : Vector

        /******************** Public Methods ********************/

        /// <inheritdoc/>
        public override void Unitise()
        {
            int size = _components.Length;
            double length = this.Length();

            for (int i = 0; i < size; i++)
            {
                _components[i] /= length;
            }
        }

        /// <inheritdoc/>
        public override double Length()
        {
            int size = _components.Length;
            double result = 0.0;

            for (int i = 0; i < size; i++)
            {
                result += _components[i] * _components[i];
            }

            return Math.Sqrt(result);
        }

        /// <inheritdoc/>
        public override double SquaredLength()
        {
            int size = _components.Length;
            double result = 0.0;

            for (int i = 0; i < size; i++)
            {
                result += _components[i] * _components[i];
            }

            return result;
        }


        /// <inheritdoc/>
        public override bool Equals(Vector other)
        {
            return other is DenseVector denseVector ? this.Equals(denseVector) : false;
        }


        /// <inheritdoc/>
        public override double[] ToArray()
        {
            return _components.Clone() as double[];
        }


        /******************** Other Methods ********************/

        /// <inheritdoc/>
        protected override void Opposite()
        {
            int size = _components.Length;

            for (int i = 0; i < size; i++)
            {
                _components[i] = -_components[i];
            }
        }

        #endregion


        #region Explicit : Additive.IAbelianGroup<DenseVector>

        /******************** Properties ********************/

        /// <inheritdoc/>
        bool Alg_Fund.IAddable<DenseVector>.IsAssociative => true;

        /// <inheritdoc/>
        bool Alg_Fund.IAddable<DenseVector>.IsCommutative => true;


        /******************** Methods ********************/

        /// <inheritdoc/>
        DenseVector Alg_Fund.IAddable<DenseVector>.Add(DenseVector right)
        {
            return DenseVector.Add(this, right);
        }

        /// <inheritdoc/>
        DenseVector Alg_Fund.ISubtractable<DenseVector>.Subtract(DenseVector right)
        {
            return DenseVector.Subtract(this, right);
        }

        /// <inheritdoc/>
        bool Alg_Set.Additive.IGroup<DenseVector>.Opposite()
        {
            this.Opposite();
            return true;
        }

        /// <inheritdoc/>
        DenseVector Alg_Fund.IZeroable<DenseVector>.Zero() { return DenseVector.Zero(Size); }

        #endregion

        #region Explicit : IGroupAction<Double,DenseVector>

        /******************** Methods ********************/

        /// <inheritdoc/>
        DenseVector Alg_Set.IGroupAction<double, DenseVector>.Multiply(double factor) { return DenseVector.Multiply(this, factor); }

        /// <inheritdoc/>
        DenseVector Alg_Set.IGroupAction<double, DenseVector>.Divide(double divisor) { return DenseVector.Divide(this, divisor); }

        #endregion

        #region Explicit : INorm<DenseVector>

        /// <inheritdoc/>
        double Alg_Meas.INorm<DenseVector>.Norm() { return this.Length(); }

        /// <inheritdoc/>
        double Alg_Meas.IMetric<DenseVector>.DistanceTo(DenseVector other)
        {
            DenseVector diff = DenseVector.Subtract(this, other);
            return diff.Length();
        }
        #endregion
    }
}
