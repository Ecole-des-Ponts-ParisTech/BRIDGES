using System;
using System.Collections.Generic;
using System.Text;

namespace BRIDGES.Arithmetic.Polynomials.Specials
{
    /// <summary>
    /// Class defining a Bernstein polynomial.
    /// </summary>
    public class Bernstein : Polynomial
    {
        #region Properties

        /// <summary>
        /// Gets the index of the current <see cref="Bernstein"/>.
        /// </summary>
        public int Index { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of <see cref="Bernstein"/> class by defining its index and degree.
        /// </summary>
        /// <param name="index"> Index of the <see cref="Bernstein"/> polynomial. </param>
        /// <param name="degree"> Degree of the <see cref="Bernstein"/> polynomial. </param>
        public Bernstein(int index, int degree)
        {
            // Instanciate fields (Necessary)
            _coefficients = ComputeCoefficients(index, degree);

            // Initialise properties
            Index = index;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Computes the coefficients of a <see cref="Bernstein"/> polynomial of a given index and degree.
        /// </summary>
        /// <param name="index"> Index of the <see cref="Bernstein"/> polynomial. </param>
        /// <param name="degree"> Degree of the <see cref="Bernstein"/> polynomial. </param>
        /// <returns> The coefficients of the <see cref="Bernstein"/> polynomial. </returns>
        private static double[] ComputeCoefficients(int index, int degree)
        {
            Polynomial[] temp = new Polynomial[degree + 1];

            /********** Initialise the zeroth-degree Bernstein polynomials **********/

            for (int j = 0; j < degree + 1; j++)
            {
                temp[j] = Polynomial.Zero;
            }
            temp[degree - index] = Polynomial.One;

            /********** Compute the triangular table **********/
            Polynomial x = new Polynomial(0.0, 1.0);
            Polynomial x1 = new Polynomial(1.0, -1.0);

            for (int k = 1; k < degree + 1; k++)
            {
                for (int j = degree; j > k - 1; j--)
                {
                    temp[j] = (x1 * temp[j]) + (x * temp[j - 1]);
                }

            }

            return temp[degree]._coefficients;
        }


        /******************** On B-Spline Polynomial Basis ********************/

        /// <summary>
        /// Evaluates a <see cref="Bernstein"/> polynomial basis of a given degree, at a given value. 
        /// </summary>
        /// <remarks> 
        /// The code is adapted from algorithm 1.3 described in <see href="https://doi.org/10.1007/978-3-642-59223-2">the NURBS Book</see>, by L. Piegl and  W. Tiller.
        /// </remarks>
        /// <param name="val"> Value to evaluate at. </param>
        /// <param name="degree"> Degree of the <see cref="Bernstein"/> polynomial basis. </param>
        /// <returns> The values of the <see cref="Bernstein"/> polynomials at the given value. </returns>
        public static double[] EvaluateBasisAt(double val, int degree)
        {
            double[] result = new double[degree + 1];

            result[0] = 1.0;

            double val1 = 1.0 - val; ;
            for (int j = 1; j < degree + 1; j++)
            {
                double saved = 0.0;
                for (int k = 0; k < j; k++)
                {
                    double temp = result[k];
                    result[k] = saved + (val1 * temp);
                    saved = val * temp;
                }

                result[j] = saved;
            }

            return result;
        }


        /******************** On Bernstein Polynomial ********************/

        /// <summary>
        /// Evaluates a <see cref="Bernstein"/> of a given index and degree, at a given value. 
        /// </summary>
        /// <remarks>
        /// The code is adapted from algorithm 1.2 described in <see href="https://doi.org/10.1007/978-3-642-59223-2">the NURBS Book</see>, by L. Piegl and  W. Tiller.
        /// </remarks>
        /// <param name="val"> Value to evaluate at. </param>
        /// <param name="index"> Index of the <see cref="Bernstein"/> to evaluate. </param>
        /// <param name="degree"> Degree of the <see cref="Bernstein"/> to evaluate. </param>
        /// <returns> The value of the <see cref="Bernstein"/> polynomial at the given value. </returns>
        public static double EvaluateAt(double val, int index, int degree)
        {
            double[] temp = new double[degree + 1];

            /********** Initialise the zeroth-degree Bernstein polynomials **********/

            for (int j = 0; j < degree + 1; j++)
            {
                temp[j] = 0.0;
            }
            temp[degree - index] = 1.0;

            /********** Compute the triangular table **********/

            double val1 = 1.0 - val;

            for (int k = 1; k < degree + 1; k++)
            {
                for (int j = degree; j > k - 1; j--)
                {
                    temp[j] = (val1 * temp[j]) + (val * temp[j - 1]);
                }

            }

            return temp[degree];
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a <see cref="BSpline"/> polynomial from the current <see cref="Bernstein"/> polynomial.
        /// </summary>
        /// <returns> The new <see cref="BSpline"/> polynomial. </returns>
        public BSpline ToBSpline()
        {
            double[] knotVector = new double[2 * (Degree + 1)];
            for (int i = Degree + 1; i < 2 * (Degree + 1); i++)
            {
                knotVector[i] = 1.0;
            }

            return new BSpline(Degree, Index, Degree, knotVector);
        }

        #endregion
    }
}
