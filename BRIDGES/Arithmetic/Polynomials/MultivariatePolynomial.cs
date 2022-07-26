using System;
using System.Collections.Generic;
using System.Text;

namespace BRIDGES.Arithmetic.Polynomials
{
    /// <summary>
    /// Class defining a multivariate polynomial.
    /// </summary>
    /// <remarks> For a univarite polynomial, refer to <see cref="Polynomial"/>. </remarks>
    public class MultivariatePolynomial
    {
        #region Fields

        /// <summary>
        /// Coefficients of the current <see cref="MultivariatePolynomial"/>, associated with the <see cref="Monomial"/> at the same index.
        /// </summary>
        double[] _coefficients;

        /// <summary>
        /// Monomials of the current <see cref="MultivariatePolynomial"/>.
        /// </summary>
        /// <remarks> By default, the monomials are not ordered in a specific way. </remarks>
        Monomial[] _monomials;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of <see cref="MultivariatePolynomial"/> class by defining its coefficients and associated monomials.
        /// </summary>
        /// <param name="coefficients"> Coefficients of the multivariate polynomial. </param>
        /// <param name="monomials"> Monomials of the multivariate polynomial. </param>
        public MultivariatePolynomial(double[] coefficients, Monomial[] monomials)
        {
            _coefficients = coefficients;

            _monomials = monomials;
        }

        #endregion

        #region Casts

        /// <summary>
        /// Casts a <see cref="Monomial"/> into a <see cref="MultivariatePolynomial"/>.
        /// </summary>
        /// <param name="monomial"> <see cref="Monomial"/> to cast. </param>
        /// <returns> The new <see cref="MultivariatePolynomial"/> resulting from the cast. </returns>
        public static implicit operator MultivariatePolynomial(Monomial monomial)
        {
            return new MultivariatePolynomial(new double[1] { 1.0 }, new Monomial[1] { monomial });
        }

        #endregion
    }
}
