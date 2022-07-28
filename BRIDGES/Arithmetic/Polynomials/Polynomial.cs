using System;

using Alg_Fund = BRIDGES.Algebra.Fundamentals;
using Alg_Set = BRIDGES.Algebra.Sets;


namespace BRIDGES.Arithmetic.Polynomials
{
    /// <summary>
    /// Class defining a univarite polynomial.
    /// </summary>
    /// <remarks> For a multivariate polynomial, refer to <see cref="MultivariatePolynomial"/>. </remarks>
    public class Polynomial
        : Alg_Set.Additive.IAbelianGroup<Polynomial>, Alg_Set.Multiplicative.IMonoid<Polynomial>, Alg_Set.IGroupAction<double, Polynomial>
    {
        #region Fields

        /// <summary>
        /// Coefficients of the current <see cref="Polynomial"/>, starting from the constant value.
        /// </summary>
        internal double[] _coefficients;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the degree of the current <see cref="Polynomial"/>.
        /// </summary>
        public int Degree 
        {
            get { return _coefficients.Length - 1   ; }
        }

        /// <summary>
        /// Gets the value of the coefficient at a given index.
        /// </summary>
        /// <param name="index"> Index of the coefficient to get. </param>
        /// <returns> The value of the coefficient at the given index. </returns>
        public double this[int index]
        {
            get { return _coefficients[index]; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of <see cref="Polynomial"/> class by defining its coefficients.
        /// </summary>
        /// <param name="coefficients"> Coefficients of the polynomial, starting from the constant value. </param>
        public Polynomial(params double[] coefficients)
        {
            _coefficients = coefficients;

            if (coefficients[coefficients.Length - 1] == 0.0) { Clean(); }
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Gets a new <see cref="Polynomial"/>, constant equal to zero.
        /// </summary>
        public static Polynomial Zero
        {
            get { return new Polynomial(0.0); }
        }

        /// <summary>
        /// Gets a new <see cref="Polynomial"/>, constant equal to one.
        /// </summary>
        public static Polynomial One
        {
            get { return new Polynomial(1.0); }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Computes the <see cref="Polynomial"/> which is the derivative of the given <see cref="Polynomial"/> at the given order.
        /// </summary>
        /// <param name="polynomial"> <see cref="Polynomial"/> to derive. </param>
        /// <param name="order"> Order of the derivative to compute. </param>
        /// <returns> The new <see cref="Polynomial"/> resulting from the derivation. </returns>
        public static Polynomial Derive(Polynomial polynomial, int order = 1)
        {
            if (order > polynomial.Degree) { return new Polynomial(0.0); }

            int newDegree = polynomial.Degree - order;
            double[] coefficients = new double[newDegree + 1];

            for (int i_C = 0; i_C < newDegree + 1; i_C++)
            {
                coefficients[i_C] = polynomial[i_C + order];
                for (int i = 0; i < order; i++)
                {
                    coefficients[i_C] *= (i_C + 1 + order);
                }
            }

            return new Polynomial(coefficients);
        }


        /******************** Algebraic Additive Group ********************/

        /// <inheritdoc cref="operator +(Polynomial, Polynomial)"/>
        public static Polynomial Add(Polynomial left, Polynomial right) { return left + right; }

        /// <inheritdoc cref="operator -(Polynomial, Polynomial)"/>
        public static Polynomial Subtract(Polynomial left, Polynomial right) { return left - right; }


        /******************** Algebraic Multiplicative Monoid ********************/

        /// <inheritdoc cref="operator *(Polynomial, Polynomial)"/>
        public static Polynomial Multiply(Polynomial left, Polynomial right) { return left * right; }


        /******************** Group Action ********************/

        /// <inheritdoc cref="operator *(double, Polynomial)"/>
        public static Polynomial Multiply(double factor, Polynomial polynomial) { return factor * polynomial; }

        /// <inheritdoc cref="operator /(Polynomial, double)"/>
        public static Polynomial Divide(Polynomial polynomial, double divisor) { return polynomial / divisor; }


        #endregion

        #region Operators

        /******************** Algebraic Additive Group ********************/

        /// <summary>
        /// Computes the addition of two <see cref="Polynomial"/>.
        /// </summary>
        /// <param name="left"> <see cref="Polynomial"/> for the addition. </param>
        /// <param name="right"> <see cref="Polynomial"/> for the addition. </param>
        /// <returns> The new <see cref="Polynomial"/> resulting from the addition. </returns>
        public static Polynomial operator +(Polynomial left, Polynomial right)
        {
            Polynomial less, more;
            if (left.Degree < right.Degree) { less = left; more = right; }
            else { less = right; more = left; }

            int lessCoefCount = less._coefficients.Length;
            int moreCoefCount = more._coefficients.Length;

            double[] coefficients = new double[moreCoefCount];

            for (int i_C = 0; i_C < lessCoefCount; i_C++)
            {
                coefficients[i_C] = less[i_C] + more[i_C];
            }
            for (int i_C = lessCoefCount; i_C < moreCoefCount; i_C++)
            {
                coefficients[i_C] = more[i_C];
            }


            Polynomial result = new Polynomial(coefficients);
            if (coefficients[coefficients.Length - 1] == 0.0) { result.Clean(); }

            return result;
        }

        /// <summary>
        /// Computes the subtraction of two <see cref="Polynomial"/>.
        /// </summary>
        /// <param name="left"> <see cref="Polynomial"/> to subtract. </param>
        /// <param name="right"> <see cref="Polynomial"/> to subtract with. </param>
        /// <returns> The new <see cref="Polynomial"/> resulting from the subtraction. </returns>
        public static Polynomial operator -(Polynomial left, Polynomial right)
        {
            double[] coefficients;

            if (left.Degree < right.Degree)
            {
                int lessCoefCount = left._coefficients.Length;
                int moreCoefCount = right._coefficients.Length;

                coefficients = new double[moreCoefCount];

                for (int i_C = 0; i_C < lessCoefCount; i_C++)
                {
                    coefficients[i_C] = left[i_C] - right[i_C];
                }

                for (int i_C = lessCoefCount; i_C < moreCoefCount; i_C++)
                {
                    coefficients[i_C] = - right[i_C];
                }
            }
            else
            {
                int lessCoefCount = right._coefficients.Length;
                int moreCoefCount = left._coefficients.Length;

                coefficients = new double[moreCoefCount];

                for (int i_C = 0; i_C < lessCoefCount; i_C++)
                {
                    coefficients[i_C] = left[i_C] - right[i_C];
                }

                for (int i_C = lessCoefCount; i_C < moreCoefCount; i_C++)
                {
                    coefficients[i_C] = left[i_C];
                }
            }

            Polynomial result = new Polynomial(coefficients);
            if (coefficients[coefficients.Length - 1] == 0.0) { result.Clean(); }

            return result;
        }

        /// <summary>
        /// Computes the opposite of the given <see cref="Polynomial"/>.
        /// </summary>
        /// <param name="polynomial"> <see cref="Polynomial"/> to be opposed. </param>
        /// <returns> The new <see cref="Polynomial"/>, opposite of the initial one. </returns>
        public static Polynomial operator -(Polynomial polynomial)
        {
            int coefCount = polynomial.Degree + 1;

            double[] coefficients = new double[coefCount];
            for (int i_C = 0; i_C < coefCount; i_C++)
            {
                coefficients[i_C] = - polynomial[i_C];
            }

            return new Polynomial(coefficients);
        }


        /******************** Algebraic Multiplicative Monoid ********************/

        /// <summary>
        /// Computes the multiplication of two <see cref="Polynomial"/>.
        /// </summary>
        /// <param name="left"> <see cref="Polynomial"/> for the multiplication. </param>
        /// <param name="right"> <see cref="Polynomial"/> for the multiplication. </param>
        /// <returns> The new <see cref="Polynomial"/> resulting from the multiplication. </returns>
        public static Polynomial operator *(Polynomial left, Polynomial right)
        {
            int leftDegree = left.Degree;
            int rightDegree = right.Degree;

            double[] coefficients = new double[leftDegree + rightDegree + 1];
            for (int i = 0; i < leftDegree + 1; i++)
            {
                for (int j = 0; j < rightDegree + 1; j++)
                {
                    coefficients[i + j] += left[i] * right[j];
                }
            }

            return new Polynomial(coefficients);
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Computes the scalar multiplication of a <see cref="Polynomial"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="factor"> <see cref="double"/>-precision real number. </param>
        /// <param name="polynomial"> <see cref="Polynomial"/> to multiply. </param>
        /// <returns> The new <see cref="Polynomial"/> resulting from the scalar multiplication. </returns>
        public static Polynomial operator *(double factor, Polynomial polynomial)
        {
            // Special case
            if (factor == 0.0) { return new Polynomial(0.0); }


            int coefCount = polynomial.Degree + 1;

            double[] coefficients = new double[coefCount];
            for (int i_C = 0; i_C < coefCount; i_C++)
            {
                coefficients[i_C] = factor * polynomial[i_C];
            }

            return new Polynomial(coefficients);
        }

        /// <inheritdoc cref="Polynomial.operator *(double, Polynomial)"/>
        public static Polynomial operator *(Polynomial polynomial, double factor)
        {
            // Special case
            if (factor == 0.0) { return new Polynomial(0.0); }


            int coefCount = polynomial.Degree + 1;

            double[] coefficients = new double[coefCount];
            for (int i_C = 0; i_C < coefCount; i_C++)
            {
                coefficients[i_C] = polynomial[i_C] * factor;
            }

            return new Polynomial(coefficients);
        }

        /// <summary>
        /// Computes the scalar division of a <see cref="Polynomial"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="polynomial"> <see cref="Polynomial"/> to divide. </param>
        /// <param name="divisor"> <see cref="double"/>-precision real number to divide with. </param>
        /// <returns> The new <see cref="Polynomial"/> resulting from the scalar division. </returns>
        public static Polynomial operator /(Polynomial polynomial, double divisor)
        {
            int coefCount = polynomial.Degree + 1;

            double[] coefficients = new double[coefCount];
            for (int i_C = 0; i_C < coefCount; i_C++)
            {
                coefficients[i_C] = polynomial[i_C] / divisor;
            }

            return new Polynomial(coefficients);
        }


        #endregion

        #region Public Methods 

        /// <summary>
        /// Returns the coefficents of the current <see cref="Polynomial"/>, starting from the constant value.
        /// </summary>
        /// <returns> The coefficents of the current <see cref="Polynomial"/>. </returns>
        public double[] GetCoefficients()
        {
            return _coefficients.Clone() as double[];
        }


        /// <summary>
        /// Computes the current <see cref="Polynomial"/> at a given value using Horner's method.
        /// </summary>
        /// <param name="val"> Value to evaluate at. </param>
        /// <returns> The computed value of the current <see cref="Polynomial"/>. </returns>
        public virtual double EvaluateAt(double val)
        {
            /* Horners method could be used */

            double result = _coefficients[Degree];
            for (int i = Degree - 1; i > -1; i--)
            {
                result = (result * val) + _coefficients[i];
            }

            return result;
        }

        #endregion

        #region Other Methods

        /// <summary>
        /// Cleans the leading zero coefficients.
        /// </summary>
        private void Clean()
        {
            int actualDegree = 0;
            for (int i_D = _coefficients.Length - 1; i_D > -1; i_D--)
            {
                if (_coefficients[i_D] != 0.0) { actualDegree = i_D; break; }
            }

            int coefCount = actualDegree + 1;

            double[] coefficients = new double[coefCount];
            for (int i_C = 0; i_C < coefCount; i_C++)
            {
                coefficients[i_C] = _coefficients[i_C];
            }

            _coefficients = coefficients;

        }


        #endregion


        #region Explicit : Additive.IAbelianGroup<Polynomial>

        /******************** Properties ********************/

        /// <inheritdoc/>
        bool Alg_Fund.IAddable<Polynomial>.IsAssociative => true;

        /// <inheritdoc/>
        bool Alg_Fund.IAddable<Polynomial>.IsCommutative => true;


        /******************** Methods ********************/

        /// <inheritdoc/>
        Polynomial Alg_Fund.IAddable<Polynomial>.Add(Polynomial other) { return this + other; }

        /// <inheritdoc/>
        Polynomial Alg_Fund.ISubtractable<Polynomial>.Subtract(Polynomial other) { return this - other; }

        /// <inheritdoc/>
        bool Alg_Set.Additive.IGroup<Polynomial>.Opposite() 
        {
            int coefCount = Degree + 1;

            double[] coefficients = new double[coefCount];
            for (int i_C = 0; i_C < coefCount; i_C++)
            {
                coefficients[i_C] = - _coefficients[i_C];
            }

            _coefficients = coefficients;

            return true;
        }

        /// <inheritdoc/>
        Polynomial Alg_Fund.IZeroable<Polynomial>.Zero() { return Polynomial.Zero; }

        #endregion

        #region Explicit : Multiplicative.IAbelianGroup<Polynomial>

        /******************** Properties ********************/

        /// <inheritdoc/>
        bool Alg_Fund.IMultiplicable<Polynomial>.IsAssociative => true;

        /// <inheritdoc/>
        bool Alg_Fund.IMultiplicable<Polynomial>.IsCommutative => true;


        /******************** Methods ********************/

        /// <inheritdoc/>
        Polynomial Alg_Fund.IMultiplicable<Polynomial>.Multiply(Polynomial other) { return this * other; }

        /// <inheritdoc/>
        Polynomial Alg_Fund.IOneable<Polynomial>.One() { return Polynomial.One; }

        #endregion

        #region Explicit : IGroupAction<Double,Polynomial>

        /******************** Methods ********************/

        /// <inheritdoc/>
        Polynomial Alg_Set.IGroupAction<double, Polynomial>.Multiply(double factor) { return factor * this; }

        /// <inheritdoc/>
        Polynomial Alg_Set.IGroupAction<double, Polynomial>.Divide(double divisor) { return this / divisor; }

        #endregion
    }
}
