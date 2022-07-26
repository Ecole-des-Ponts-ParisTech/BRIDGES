using System;


namespace BRIDGES.Arithmetic.Polynomials.Specials
{
    /// <summary>
    /// Class defining a B-Spline polynomial.
    /// </summary>
    public class BSplinePolynomial : Polynomial
    {
        #region Fields

        /// <summary>
        /// Knot vector associated with the current <see cref="BSplinePolynomial"/>.
        /// </summary>
        protected double[] _knotVector;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the index of the current <see cref="BSplinePolynomial"/>.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Gets the knot vector associated with the current <see cref="BSplinePolynomial"/>.
        /// </summary>
        public double[] KnotVector
        {
            get { return _knotVector.Clone() as double[]; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of <see cref="BSplinePolynomial"/> class by defining its index, degree and knot vector.
        /// </summary>
        /// <param name="index"> Index of the B-Spline polynomial. </param>
        /// <param name="degree"> Degree of the B-Spline polynomial. </param>
        /// <param name="knotVector"> Knot vector of the B-Spline polynomial. </param>
        /// <exception cref="ArgumentException"> The knot vector should contain at least one element. </exception>
        /// <exception cref="RankException"> The knot vector should contain at least (degree - 1) elements.</exception>
        public BSplinePolynomial(int index, int degree, double[] knotVector)
        {
            int knotCount = knotVector.Length;

            if (knotCount == 0) { throw new ArgumentException("The knot vector should contain at least one element."); }
            if (knotCount < degree - 1) { throw new RankException($"The knot vector should contain at least {degree - 1} elements."); }

            // Instanciate fields (Necessary)
            _coefficients = ComputeCoefficients(index, degree, knotVector); 

            // Initialise fields (verifications on the knot vector should be done)
            _knotVector = knotVector;

            // Initialise properties
            Index = index;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Identifies the index of the knot span containing a given value, using a binary search.
        /// </summary>
        /// <remarks> 
        /// The code is adapted from algorithm 2.1 described in <see href="https://doi.org/10.1007/978-3-642-59223-2">the NURBS Book</see>, by L. Piegl and  W. Tiller.
        /// </remarks>
        /// <param name="val"> Value to locate in the knot vector. </param>
        /// <param name="degree"> Degree of the <see cref="BSplinePolynomial"/> basis. </param>
        /// <param name="knotVector"> Knot vector of the <see cref="BSplinePolynomial"/> basis. </param>
        /// <returns> The (zero-based) index of the knot span containing the value. </returns>
        public static int FindKnotSpanIndex(double val, int degree, double[] knotVector)
        {
            // Number of segments in the control polygon.
            int n = knotVector.Length - degree - 1;

            // Special case : The value equals the last possible knot.
            if (val == knotVector[n + 1]) { return n; }

            int i_StartKnot = degree;
            int i_EndKnot = n + 1;

            int i_MidKnot = (i_StartKnot + i_EndKnot) / 2;
            while (val < knotVector[i_MidKnot] || val >= knotVector[i_MidKnot + 1])
            {
                if (val < knotVector[i_MidKnot]) { i_EndKnot = i_MidKnot; }
                else { i_StartKnot = i_MidKnot; }

                i_MidKnot = (i_StartKnot + i_EndKnot / 2);
            }

            return i_MidKnot;
        }

        /// <summary>
        /// Computes the coefficients of the current <see cref="BSplinePolynomial"/>.
        /// </summary>
        /// <param name="index"> Index of the <see cref="BSplinePolynomial"/>. </param>
        /// <param name="degree"> Degree of the <see cref="BSplinePolynomial"/>. </param>
        /// <param name="knotVector"> Knot vector of the <see cref="BSplinePolynomial"/>. </param>
        /// <returns> The coefficients of the <see cref="BSplinePolynomial"/>. </returns>
        private static double[] ComputeCoefficients(int index, int degree, double[] knotVector)
        {
            // Number of knot span
            int m = knotVector.Length - 1;

            // Initialise the zeroth-degree B-Spline polynomials from N_{index,0} to N_{index+degree,0}
            Polynomial[][] N = new Polynomial[degree + 1][]; // Beware the indices are inverted in N.
            N[0] = new Polynomial[degree + 1];
            for (int j = 0; j < degree + 1; j++) { N[0][j] = new Polynomial(1.0); }

            // Compute the triangular table.
            for (int d = 1; d < degree + 1; d++)
            {

                N[d] = new Polynomial[degree + 1 - d];

                for (int j = 0; j < degree + 1 - d; j++)
                {
                    // Compute N_{index + j, d}
                    Polynomial first = new Polynomial(-knotVector[index + j], 1.0);
                    first = first / (knotVector[index + j + degree] - knotVector[index + j]);

                    Polynomial second = new Polynomial(knotVector[index + j + degree + 1], -1.0);
                    second = second / (knotVector[index + j + degree + 1] - knotVector[index + j + 1]);

                    N[d][j] = first * N[d - 1][j] + second * N[d - 1][j + 1];
                }
            }

            return N[degree][0]._coefficients;
        }


        /******************** On B-Spline Polynomial Basis ********************/

        /// <summary>
        /// Computes all the indices of the <see cref="BSplinePolynomial"/> of a given degree and associated with a given knot vector.
        /// </summary>
        /// <param name="degree"> Degree of the <see cref="BSplinePolynomial"/> in the set. </param>
        /// <param name="knotVector"> Knot vector of the <see cref="BSplinePolynomial"/> to compute. </param>
        /// <returns> The set of <see cref="BSplinePolynomial"/> of the given degree and associated with the knot vector. </returns>
        public static BSplinePolynomial[] ComputeBasis(int degree, double[] knotVector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Evaluates a <see cref="BSplinePolynomial"/> basis at a given value. 
        /// </summary>
        /// <remarks> 
        /// The code is adapted from algorithm 2.2 described in <see href="https://doi.org/10.1007/978-3-642-59223-2">the NURBS Book</see>, by L. Piegl and  W. Tiller.
        /// </remarks>
        /// <param name="val"> Value to evaluate at. </param>
        /// <param name="knotSpanIndex"> Index of knot span containing the value. </param>
        /// <param name="degree"> Degree of the <see cref="BSplinePolynomial"/> basis. </param>
        /// <param name="knotVector"> Knot vector of the <see cref="BSplinePolynomial"/> basis. </param>
        /// <returns> The non-zero values of the <see cref="BSplinePolynomial"/> basis at the given value. <br/>
        /// (i.e. the values of the <see cref="BSplinePolynomial"/> with indices from (<paramref name="knotSpanIndex"/> - <paramref name="degree"/>) and <paramref name="knotSpanIndex"/>. </returns>
        public static double[] EvaluateBasisAt(double val, int knotSpanIndex, int degree, double[] knotVector)
        {
            // Value of the polynomials
            double[] N = new double[degree + 1];

            double[] left = new double[degree + 1];
            double[] right = new double[degree + 1];

            // Initialise the zeroth-degree B-Spline polynomial
            N[0] = 1.0;

            // Compute inverse triangular table
            for (int j = 1; j < degree + 1; j++)
            {
                left[j] = val - knotVector[knotSpanIndex + 1 - j];
                right[j] = knotVector[knotSpanIndex + j];

                double saved = 0.0;

                for (int r = 0; r < j; r++)
                {
                    double temp = N[r] / (right[r + 1] - left[j - r]);
                    N[r] = saved + right[r + 1] * temp;
                    saved = left[j - r] * temp;
                }

                N[j] = saved;
            }

            return N;
        }

        /// <summary>
        /// Evaluates the <see cref="BSplinePolynomial"/> basis' derivatives of a given order, at a given value. 
        /// </summary>
        /// <remarks> 
        /// The code is adapted from algorithm 2.3 described in <see href="https://doi.org/10.1007/978-3-642-59223-2">the NURBS Book</see>, by L. Piegl and  W. Tiller.
        /// </remarks>
        /// <param name="val"> Value to evaluate at. </param>
        /// <param name="knotSpanIndex"> Index of knot span containing the value. </param>
        /// <param name="degree"> Initial degree of the <see cref="BSplinePolynomial"/> basis. </param>
        /// <param name="knotVector"> Initial knot vector of the <see cref="BSplinePolynomial"/> basis. </param>
        /// <param name="order"> Order of the derivatives. </param>
        /// <returns>  </returns>
        public static double[][] EvaluateBasisDerivativesAt(double val, int knotSpanIndex, int degree, double[] knotVector, int order)
        {
            throw new NotImplementedException();
        }

        /******************** On B-Spline Polynomial Basis ********************/

        /// <summary>
        /// Computes the <see cref="BSplinePolynomial"/> of a given index and degree, associated with a given knot vector.
        /// </summary>
        /// <param name="index"> Index of the <see cref="BSplinePolynomial"/> to compute. </param>
        /// <param name="degree"> Degree of the <see cref="BSplinePolynomial"/> to compute. </param>
        /// <param name="knotVector"> Knot Vector associated with the <see cref="BSplinePolynomial"/> to compute. </param>
        /// <returns> The <see cref="BSplinePolynomial"/> of the given index and degree, associated with the knot vector. </returns>
        public static BSplinePolynomial ComputeBSpline(int index, int degree, double[] knotVector)
        {
            return new BSplinePolynomial(index, degree, knotVector);
        }

        /// <summary>
        /// Evaluates the <see cref="BSplinePolynomial"/> at a given value. 
        /// </summary>
        /// <remarks>
        /// The code is adapted from algorithm 2.4 described in <see href="https://doi.org/10.1007/978-3-642-59223-2">the NURBS Book</see>, by L. Piegl and  W. Tiller.
        /// </remarks>
        /// <param name="u"> Value to evaluate at. </param>
        /// <param name="index"> Index of the <see cref="BSplinePolynomial"/> to evaluate. </param>
        /// <param name="degree"> Degree of the <see cref="BSplinePolynomial"/> to evaluate. </param>
        /// <param name="knotVector"> Knot Vector associated with the <see cref="BSplinePolynomial"/> to evaluate. </param>
        /// <returns> The value of the <see cref="BSplinePolynomial"/> the given value. </returns>
        public static double EvaluateAt(double u, int index,  int degree, double[] knotVector)
        {
            // Number of knot span
            int m = knotVector.Length - 1;

            // If the first B-spline polynomial is evaluated at the first knot,
            // or if the last B-spline polynomial is evaluated at the last knot.
            if ((index == 0 && u == knotVector[0])  || (index == (m - degree - 1) && u == knotVector[m])) { return 1.0; }

            // If value is out of the non-zero domain of the B-Spline polynomial.
            if (u < knotVector[index] || u <= knotVector[index + degree + 1]) { return 0.0; }


            // Initialise the zeroth-degree B-Spline polynomials
            double[] N = new double[degree + 1];
            for (int j = 0; j < degree + 1; j++)
            {
                if( u >= knotVector[index + j] && u < knotVector[index + j + 1]) { N[j] = 1.0; }
                else { N[j] = 0.0; }
            }

            // Compute the triangular table
            for (int k = 1; k < degree + 1; k++)
            {
                double saved;
                if (N[0] == 0.0) { saved = 0.0; }
                else { saved = ((u - knotVector[index]) * N[0]) / (knotVector[index + k] - knotVector[index]); }

                for (int j = 0; j < degree - k + 1; j++)
                {
                    double knotLeft = knotVector[index + j + 1];
                    double knotRight = knotVector[index + j + k + 1];

                    if (N[j + 1] == 0.0)
                    {
                        N[j] = saved; 
                        saved = 0.0;
                    }
                    else
                    {
                        double temp = N[j + 1] / (knotRight - knotLeft);
                        N[j] = saved + ((knotRight - u) * temp);
                        saved = (u - knotLeft) * temp;
                    }
                }
            }

            return N[0];
        }

        /// <summary>
        /// Evaluates the <see cref="BSplinePolynomial"/> derivative of a given order; at a given value. 
        /// </summary>
        /// <remarks>
        /// The code is adapted from algorithm 2.5 described in <see href="https://doi.org/10.1007/978-3-642-59223-2">the NURBS Book</see>, by L. Piegl and  W. Tiller.
        /// </remarks>
        /// <param name="u"> Value to evaluate at. </param>
        /// <param name="index"> Index of the <see cref="BSplinePolynomial"/> to evaluate. </param>
        /// <param name="degree"> Initial degree of the <see cref="BSplinePolynomial"/> to evaluate. </param>
        /// <param name="knotVector"> Initial knot Vector associated with the <see cref="BSplinePolynomial"/> to evaluate. </param>
        /// <param name="order"> Order of the derivative. </param>
        /// <returns> The value of the successive <see cref="BSplinePolynomial"/> derivatives at the given value. </returns>
        public static double[] EvaluateAt(double u, int index, int degree, double[] knotVector, int order)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Computes the current <see cref="BSplinePolynomial"/> at a given value.
        /// </summary>
        /// <param name="val"> Value to evaluate at. </param>
        /// <returns> The computed value of the current <see cref="BSplinePolynomial"/>. </returns>
        public override double EvaluateAt(double val)
        {
            // Number of knot span
            int m = _knotVector.Length - 1;

            // Get the index of the knot span containing the value.
            int i_KnotSpan = FindKnotSpanIndex(val, Degree, _knotVector);

            // If the first B-spline polynomial is evaluated at the first knot,
            // or if the last B-spline polynomial is evaluated at the last knot.
            if ((Index == 0 && val == _knotVector[0]) || (Index == (m - Degree - 1) && val == _knotVector[m])) { return 1.0; }

            // If value is out of the non-zero domain of the current B-Spline polynomial.
            if (val < _knotVector[Index] || val <= _knotVector[Index + Degree + 1]) { return 0.0; }

            // The value is within the non-zero domain of the current B-Spline polynomial.
            return base.EvaluateAt(val);
        }

        #endregion

        // cast from bernstein
    }
}
