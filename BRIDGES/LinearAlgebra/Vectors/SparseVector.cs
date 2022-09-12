using System;
using System.Collections.Generic;

using Alg_Fund = BRIDGES.Algebra.Fundamentals;
using Alg_Meas = BRIDGES.Algebra.Measure;
using Alg_Set = BRIDGES.Algebra.Sets;


namespace BRIDGES.LinearAlgebra.Vectors
{
    /// <summary>
    /// Class defining a sparse vector.
    /// </summary>
    public sealed class SparseVector : Vector,
          Alg_Set.Additive.IAbelianGroup<SparseVector>, Alg_Set.IGroupAction<double, SparseVector>,
          Alg_Meas.INorm<SparseVector>,
          IEquatable<SparseVector>
    {
        #region Fields

        /// <summary>
        /// Size of the current <see cref="SparseVector"/>.
        /// </summary>
        private int _size;

        /// <summary>
        /// Non-zero components of the current <see cref="SparseVector"/>.
        /// </summary>
        /// <remarks>
        /// Key : Row index of a non-zero component.<br/>
        /// Value : Non-zero component at the given row index.
        /// </remarks>
        private Dictionary<int, double> _components;

        #endregion

        #region Properties

        /// <inheritdoc/>
        public override int Size 
        { 
            get { return _size; }
        }

        /// <inheritdoc/>
        public override double this[int index]
        {
            get 
            {
                _components.TryGetValue(index, out double val);
                return val;
            }
            set 
            {
                if(_components.ContainsKey(index)) { _components[index] = value; }
                else { _components.Add(index, value); }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="SparseVector"/> class of given size, containing only zeros.
        /// </summary>
        /// <param name="size"> Number of component of the current <see cref="SparseVector"/>. </param>
        public SparseVector(int size)
        {
            _size = size;

            _components = new Dictionary<int, double>();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SparseVector"/> class of given size, with given values.
        /// </summary>
        /// <param name="size"> Number of component of the current <see cref="SparseVector"/>. </param>
        /// <param name="rowIndices"> Row indices of the non-zero values of the current <see cref="SparseVector"/>. </param>
        /// <param name="values"> Non-zero values of the current <see cref="SparseVector"/>. </param>
        /// <exception cref="ArgumentException"> The numbers of row indices and values should be the same. </exception>
        public SparseVector(int size, IEnumerable<int> rowIndices, IEnumerable<double> values)
        {
            _size = size;

            _components = new Dictionary<int, double>();

            IEnumerator<int> rowEnumerator = rowIndices.GetEnumerator();
            IEnumerator<double> valueEnumerator = values.GetEnumerator();
            try
            {
                while(rowEnumerator.MoveNext())
                {
                    if(!valueEnumerator.MoveNext())
                    {
                        throw new ArgumentException("The numbers of row indices and values should be the same.");
                    }

                    _components.Add(rowEnumerator.Current, valueEnumerator.Current);
                }

                if (valueEnumerator.MoveNext())
                {
                    throw new ArgumentException("The numbers of row indices and values should be the same.");
                }
            }
            finally
            {
                rowEnumerator.Dispose();
                valueEnumerator.Dispose();
            }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SparseVector"/> class from another <see cref="SparseVector"/>.
        /// </summary>
        /// <param name="other"> <see cref="SparseVector"/> to copy. </param>
        public SparseVector(SparseVector other)
        {
            _size = other._size;

            _components = new Dictionary<int, double>(other._components);
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Returns the neutral <see cref="SparseVector"/> for the addition.
        /// </summary>
        /// <param name="size"> Number of component of the current <see cref="SparseVector"/>. </param>
        /// <returns> The <see cref="SparseVector"/> of the given size and with zeros on every coordinates. </returns>
        public static new SparseVector Zero(int size)
        {
            return new SparseVector(size);
        }


        /// <summary>
        /// Returns the unit <see cref="SparseVector"/> of a given <paramref name="size"/>, with one at the given row <paramref name="index"/> and zeros elsewhere.
        /// </summary>
        /// <param name="size"> Size of the new <see cref="SparseVector"/>. </param>
        /// <param name="index"> Index of the standard vector, i.e of the component equal to one. </param>
        /// <returns> The new <see cref="SparseVector"/> representing the standard vector. </returns>
        public static new SparseVector StandardVector(int size, int index)
        {
            SparseVector vector = new SparseVector(size);
            vector[index] = 1.0;

            return vector;
        }

        #endregion

        #region Static Methods

        /******************** Algebraic Additive Group ********************/

        /// <summary>
        /// Computes the addition of two <see cref="SparseVector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseVector"/> for the addition. </param>
        /// <param name="right"> Right <see cref="SparseVector"/> for the addition. </param>
        /// <returns> The new <see cref="SparseVector"/> resulting from the addition. </returns>
        /// <exception cref="ArgumentException"> The size of the vectors should be the same. </exception>
        public static SparseVector Add(SparseVector left, SparseVector right)
        {
            if(left.Size != right.Size) { throw new ArgumentException("The size of the vectors should be the same."); }


            SparseVector result = new SparseVector(left);

            foreach (KeyValuePair<int,double> kvp in right._components)
            {
                if (result._components.TryGetValue(kvp.Key, out double val))
                {
                    if(kvp.Value != -val) { result[kvp.Key] = val + kvp.Value; }
                }
                else { result._components.Add(kvp.Key, kvp.Value); }
            }

            return result;
        }

        /// <summary>
        /// Computes the subtraction of two <see cref="SparseVector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseVector"/> to subtract. </param>
        /// <param name="right"> Right <see cref="SparseVector"/> to subtract with. </param>
        /// <returns> The new <see cref="SparseVector"/> resulting from the subtraction. </returns>
        /// <exception cref="ArgumentException"> The size of the vectors should be the same. </exception>
        public static SparseVector Subtract(SparseVector left, SparseVector right)
        {
            if (left.Size != right.Size) { throw new ArgumentException("The size of the vectors should be the same."); }


            SparseVector result = new SparseVector(left);

            foreach (KeyValuePair<int, double> kvp in right._components)
            {
                if (result._components.TryGetValue(kvp.Key, out double val))
                {
                    if (kvp.Value != val) { result[kvp.Key] = val - kvp.Value; }
                }
                else { result._components.Add(kvp.Key, -kvp.Value); }
            }

            return result;
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Computes the scalar multiplication of a <see cref="SparseVector"/> with a <see cref="double"/>-precision real number on the left.
        /// </summary>
        /// <param name="factor"> <see cref="double"/>-precision real number. </param>
        /// <param name="operand"> <see cref="SparseVector"/> to multiply on the left. </param>
        /// <returns> The new <see cref="SparseVector"/> resulting from the scalar multiplication. </returns>
        public static SparseVector Multiply(double factor, SparseVector operand)
        {
            if (factor == 0.0) { return SparseVector.Zero(operand.Size); }

            SparseVector result = new SparseVector(operand);

            foreach(KeyValuePair<int,double> kvp in result._components)
            {
                result._components[kvp.Key] = factor * result._components[kvp.Key];
            }

            return result;
        }

        /// <summary>
        /// Computes the scalar multiplication of a <see cref="SparseVector"/> with a <see cref="double"/>-precision real number on the right.
        /// </summary>
        /// <param name="operand"> <see cref="SparseVector"/> to multiply on the right. </param>
        /// <param name="factor"> <see cref="double"/>-precision real number. </param>
        /// <returns> The new <see cref="SparseVector"/> resulting from the scalar multiplication. </returns>
        public static SparseVector Multiply(SparseVector operand, double factor)
        {
            if (factor == 0.0) { return SparseVector.Zero(operand.Size); }

            SparseVector result = new SparseVector(operand);

            foreach (KeyValuePair<int, double> kvp in result._components)
            {
                result._components[kvp.Key] = result._components[kvp.Key] * factor;
            }

            return result;
        }


        /// <summary>
        /// Computes the scalar division of a <see cref="SparseVector"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="operand"> <see cref="SparseVector"/> to divide. </param>
        /// <param name="divisor"> <see cref="double"/>-precision real number to divide with. </param>
        /// <returns> The new <see cref="SparseVector"/> resulting from the scalar division. </returns>
        public static SparseVector Divide(SparseVector operand, double divisor)
        {
            SparseVector result = new SparseVector(operand);

            foreach (KeyValuePair<int, double> kvp in result._components)
            {
                result._components[kvp.Key] /= divisor;
            }

            return result;
        }


        /******************** Other Operations ********************/

        /// <summary>
        /// Computes the multiplication between the transposed left <see cref="SparseVector"/> and the right <see cref="SparseVector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseVector"/> to transpose, then multiply. </param>
        /// <param name="right"> Right <see cref="SparseVector"/> to multiply. </param>
        /// <returns> The <see cref="double"/>-precision scalar resulting from the multiplication. </returns>
        /// <exception cref="ArgumentException"> The size of the vectors should be the same. </exception>
        public static double TransposeMultiply(SparseVector left, SparseVector right)
        {
            if (left.Size != right.Size) { throw new ArgumentException("The size of the vectors should be the same."); }


            double result = 0.0;

            foreach (KeyValuePair<int, double> kvp in left._components)
            {
                if(right._components.TryGetValue(kvp.Key, out double val))
                { 
                    result += kvp.Value * val; 
                }
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

            foreach (KeyValuePair<int, double> kvp in right._components)
            {
                result += left[kvp.Key] * kvp.Value;
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

            foreach (KeyValuePair<int, double> kvp in left._components)
            {
                result += kvp.Value * right[kvp.Key];
            }

            return result;
        }


        /******************** On SparseVector Sets ********************/

        /// <summary>
        /// Ortho-normalise the set of <see cref="SparseVector"/> using a Gram-Schimdt process. 
        /// </summary>
        /// <remarks> If the vectors are not linearly independent the number of vectors will change. </remarks>
        /// <param name="vectors"> Set of <see cref="SparseVector"/> to operate on. </param>
        /// <returns> The ortho-normal set of <see cref="SparseVector"/>. </returns>
        public static SparseVector[] GramSchmidt(IEnumerable<SparseVector> vectors)
        {
            List<SparseVector> results = new List<SparseVector>();

            foreach (SparseVector sparseVector in vectors)
            {
                SparseVector vector = new SparseVector(sparseVector);

                for (int i_R = 0; i_R < results.Count; i_R++)
                {
                    SparseVector result = new SparseVector(results[i_R]);

                    double numerator = SparseVector.TransposeMultiply(vector, result);
                    double denominator = result.SquaredLength();

                    result = SparseVector.Multiply(numerator / denominator, result);

                    vector = SparseVector.Subtract(vector, result);
                }

                double length = vector.Length();
                if (length > Settings.AbsolutePrecision)
                {
                    vector = SparseVector.Divide(vector, length);
                    results.Add(vector);
                }
            }

            return results.ToArray();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the components at the given index.
        /// </summary>
        /// <param name="index"> The index of the component to get. </param>
        /// <param name="val"> Value containing the component at the given index if it was found, zero otherwise. </param>
        /// <returns> <see langword="true"/> if the component was found, <see langword="false"/> otherwise. </returns>
        public bool TryGetComponent(int index, out double val)
        {
            return _components.TryGetValue(index, out val);
        }


        /// <summary>
        /// Returns an enumerator which reads through the non-zero components of the current <see cref="SparseVector"/>. <br/>
        /// The <see cref="KeyValuePair{TKey, TValue}"/> represents is composed of the row index and thr component value.
        /// </summary>
        /// <returns> The enumerator of the <see cref="SparseVector"/>. </returns>
        public IEnumerator<KeyValuePair<int, double>> GetNonZeros()
        {
            return _components.GetEnumerator();
        }


        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public bool Equals(SparseVector other)
        {
            if (Size != other.Size) { return false; }

            if(_components.Count != other._components.Count) { return false; }

            foreach(KeyValuePair<int, double> kvp in _components)
            {
                if(!other._components.TryGetValue(kvp.Key, out double val) || val != kvp.Value) { return false; }
            }

            return true;
        }

        #endregion


        #region Override : Vector

        /******************** Public Methods ********************/

        /// <inheritdoc/>
        public override void Unitise()
        {
            double length = this.Length();

            foreach (KeyValuePair<int, double> kvp in _components)
            {
                _components[kvp.Key] = kvp.Value / length;
            }
        }

        /// <inheritdoc/>
        public override double Length()
        {
            double result = 0.0;

            foreach (KeyValuePair<int, double> kvp in _components)
            {
                result += kvp.Value * kvp.Value;
            }

            return Math.Sqrt(result);
        }

        /// <inheritdoc/>
        public override double SquaredLength()
        {
            double result = 0.0;

            foreach (KeyValuePair<int, double> kvp in _components)
            {
                result += kvp.Value * kvp.Value;
            }

            return result;
        }


        /// <inheritdoc/>
        public override bool Equals(Vector other)
        {
            return other is SparseVector sparseVector ? this.Equals(sparseVector) : false;
        }


        /// <inheritdoc/>
        public override double[] ToArray()
        {
            double[] components = new double[_size];

            foreach(KeyValuePair<int,double> kvp in _components) { components[kvp.Key] = kvp.Value; }

            return components;
        }


        /******************** Other Methods ********************/

        /// <inheritdoc/>
        protected override void Opposite()
        {
            foreach (KeyValuePair<int, double> kvp in _components)
            {
                _components[kvp.Key] = -kvp.Value;
            }
        }

        #endregion


        #region Explicit : Additive.IAbelianGroup<SparseVector>

        /******************** Properties ********************/

        /// <inheritdoc/>
        bool Alg_Fund.IAddable<SparseVector>.IsAssociative => true;

        /// <inheritdoc/>
        bool Alg_Fund.IAddable<SparseVector>.IsCommutative => true;


        /******************** Methods ********************/

        /// <inheritdoc/>
        SparseVector Alg_Fund.IAddable<SparseVector>.Add(SparseVector right) 
        { 
            return SparseVector.Add(this, right); 
        }

        /// <inheritdoc/>
        SparseVector Alg_Fund.ISubtractable<SparseVector>.Subtract(SparseVector right) 
        { 
            return SparseVector.Subtract(this, right); 
        }

        /// <inheritdoc/>
        bool Alg_Set.Additive.IGroup<SparseVector>.Opposite()
        {
            this.Opposite();
            return true;
        }

        /// <inheritdoc/>
        SparseVector Alg_Fund.IZeroable<SparseVector>.Zero() { return SparseVector.Zero(Size); }

        #endregion

        #region Explicit : IGroupAction<Double,SparseVector>

        /******************** Methods ********************/

        /// <inheritdoc/>
        SparseVector Alg_Set.IGroupAction<double, SparseVector>.Multiply(double factor) { return SparseVector.Multiply(this, factor); }

        /// <inheritdoc/>
        SparseVector Alg_Set.IGroupAction<double, SparseVector>.Divide(double divisor) { return SparseVector.Divide(this, divisor); }

        #endregion

        #region Explicit : INorm<SparseVector>

        /// <inheritdoc/>
        double Alg_Meas.INorm<SparseVector>.Norm() { return this.Length(); }

        /// <inheritdoc/>
        double Alg_Meas.IMetric<SparseVector>.DistanceTo(SparseVector other)
        {
            Vector diff = Vector.Subtract(this, other);
            return diff.Length();
        }
        #endregion
    }
}
