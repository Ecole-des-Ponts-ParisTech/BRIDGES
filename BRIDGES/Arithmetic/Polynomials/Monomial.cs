using System;

using Alg = BRIDGES.Algebra.Sets;

namespace BRIDGES.Arithmetic.Polynomials
{
    /// <summary>
    /// Class defining a multivariate monomial.
    /// </summary>
    public class Monomial
    {
        #region Fields

        /// <summary>
        /// Degrees of the variables.
        /// </summary>
        private int[] _exponents;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the exponent of the variable at a given index in the current <see cref="Monomial"/>.
        /// </summary>
        /// <param name="index"> Index of the varaible whose exponent to get. </param>
        /// <returns> The exponent of the variable at the given index. </returns>
        public int this[int index]
        {
            get { return _exponents[index]; }
        }

        /// <summary>
        /// Gets the total degree of the current <see cref="Monomial"/>.
        /// </summary>
        public int TotalDegree
        {
            get
            {
                int totaldegree = 0;
                for (int i = 0; i < _exponents.Length; i++)
                {
                    totaldegree += _exponents[i];
                }
                return totaldegree;
            }
        }

        /// <summary>
        /// Gets the number of variable of the current <see cref="Monomial"/>
        /// </summary>
        public int VariableCount 
        {
            get { return _exponents.Length; }
                
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of <see cref="Polynomial"/> class by defining the variable's degree.
        /// </summary>
        /// <param name="exponents"> Variables' exponent. </param>
        public Monomial(params int[] exponents)
        {
            _exponents = exponents;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Computes the multiplication of two <see cref="Monomial"/>.
        /// </summary>
        /// <param name="monomialA"> <see cref="Monomial"/> for the multiplication. </param>
        /// <param name="monomialB"> <see cref="Monomial"/> for the multiplication. </param>
        /// <returns> The new <see cref="Monomial"/> resulting from the multiplication. </returns>
        public static Monomial Multiply(Monomial monomialA, Monomial monomialB)
        {
            Monomial less, more;
            if(monomialA.VariableCount < monomialB.VariableCount) { less = monomialA; more = monomialB; }
            else { less = monomialB; more = monomialA; }

            int lessCount = less.VariableCount;
            int moreCount = more.VariableCount;

            int[] exponents = new int[moreCount];

            for (int i_E = 0; i_E < lessCount; i_E++)
            {
                exponents[i_E] = less._exponents[i_E] + more._exponents[i_E];
            }

            for (int i_D = lessCount; i_D < moreCount; i_D++)
            {
                exponents[i_D] = more._exponents[i_D];
            }

            return new Monomial(exponents);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Computes the current <see cref="Monomial"/> at a given value.
        /// </summary>
        /// <param name="val"> Value to evaluate at. </param>
        /// <returns> The computed value of the current <see cref="Monomial"/>. </returns>
        public virtual double EvaluateAt(double[] val)
        {
            if (val is null)
            {
                throw new ArgumentNullException(nameof(val));
            }

            double result = 1.0;

            for (int i_V = 0; i_V < VariableCount; i_V++)
            {
                int exponent = _exponents[i_V];
                double variable = val[i_V];

                for (int i_E = 0; i_E < exponent; i_E++)
                {
                    result = result * variable;
                }
            }

            return result;
        }

        #endregion
    }

}



