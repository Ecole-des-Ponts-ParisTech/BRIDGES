using System;

using Alg_Set = BRIDGES.Algebra.Sets;

using Arith_Spe = BRIDGES.Arithmetic.Polynomials.Specials;

namespace BRIDGES.Geometry.Kernel
{
    /// <summary>
    /// Class defining a generic NURBS Curve
    /// </summary>
    /// <typeparam name="TPoint"></typeparam>
    public class NURBSCurve<TPoint>
        : ICurve<TPoint>
        where TPoint : Alg_Set.Additive.IMagma<TPoint>, Alg_Set.IGroupAction<double, TPoint>
    {
        #region Fields

        /// <summary>
        /// Degree of the interpolation.
        /// </summary>
        protected int _degree;

        /// <summary>
        /// Knot vector.
        /// </summary>
        protected double[] _knotVector;

        /// <summary>
        /// Control points.
        /// </summary>
        protected TPoint[] _controlPoints;

        /// <summary>
        /// Weights associated to the control points
        /// </summary>
        private double[] _weights;

        #endregion

        #region Properties

        /// <inheritdoc/>
        public bool IsClosed
        {
            get { return StartPoint.Equals(EndPoint); }
        }


        /// <inheritdoc/>
        public TPoint StartPoint
        {
            get { return _controlPoints[0]; }
        }

        /// <inheritdoc/>
        public TPoint EndPoint
        {
            get { return _controlPoints[_controlPoints.Length - 1]; }
        }


        /// <inheritdoc/>
        public double DomainStart
        { 
            get { return _knotVector[0]; } 
        }

        /// <inheritdoc/>
        public double DomainEnd
        {
            get { return _knotVector[KnotCount - 1]; }
        }


        /// <summary>
        /// Gets the curve degree.
        /// </summary>
        public int Degree
        {
            get { return _degree; }
        }

        /// <summary>
        /// Gets the number of knots.
        /// </summary>
        public int KnotCount { get { return _knotVector.Length; } }

        /// <summary>
        /// Gets the number of control points.
        /// </summary>
        public int PointCount { get { return _controlPoints.Length; } }


        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of <see cref="NURBSCurve{TControlPoint}"/> class between 0.0 and 1.0.
        /// </summary>
        /// <param name="degree"> Degree of the interpolating <see cref="Arith_Spe.BSplinePolynomial"/>. </param>
        /// <param name="controlPoints"> Control points of the <see cref="NURBSCurve{TControlPoint}"/>. </param>
        public NURBSCurve(int degree, TPoint[] controlPoints)
        {
            // Initialise Fields
            _degree = degree;
            _controlPoints = controlPoints;

            // Compute Weigths
            _weights = new double[PointCount];
            for (int i_W = 0; i_W < PointCount; i_W++) { _weights[i_W] = 1.0; }

            // Compute "uniform" knot vector between 0.0 and 1.0
            double domainStart = 0.0, domainEnd = 1.0;

            int i_MaxKnot = _knotVector.Length - 1;

            _knotVector = new double[degree + PointCount];
            for (int i = 0; i < (degree + 1); i++) { _knotVector[i] = domainStart; }
            for (int i = (degree + 1); i < (i_MaxKnot - degree); i++) 
            {
                var ratio = (double)(i - degree) / ((double)(i_MaxKnot - 2 * degree));
                _knotVector[i] = domainStart + (domainEnd - domainStart) * ratio;
            }
            for (int i = (i_MaxKnot - degree); i < (i_MaxKnot + 1); i++) { _knotVector[i] = domainEnd; }
        }

        /// <summary>
        /// Initialises a new instance of <see cref="NURBSCurve{TControlPoint}"/> class by defining its fields.
        /// </summary>
        /// <param name="degree"> Degree of the interpolating <see cref="Arith_Spe.BSplinePolynomial"/>. </param>
        /// <param name="knotVector"> Knot vector of the interpolating <see cref="Arith_Spe.BSplinePolynomial"/>. </param>
        /// <param name="controlPoints"> Control points of the <see cref="NURBSCurve{TControlPoint}"/>. </param>
        /// <param name="weights"> Control points of the <see cref="NURBSCurve{TControlPoint}"/>. </param>
        public NURBSCurve(int degree, double[] knotVector, TPoint[] controlPoints, double[] weights)
        {
            if (controlPoints.Length != weights.Length) { throw new ArgumentException("The number of weights should match the number of control Points.","weigths"); }
            
            // Initialise Fields
            _degree = degree;
            _controlPoints = controlPoints;
            _weights = weights;

            // If the whole knot vector is given (should check the validity)
            if (knotVector.Length - 1 == (controlPoints.Length + degree)) { _knotVector = knotVector; }
            // If the nonconstant partof the knot vector is given (with the domain start and end)
            else if (knotVector.Length - 1 != (controlPoints.Length - degree))
            {
                int i_MaxKnot = PointCount + degree;

                _knotVector = new double[i_MaxKnot + 1];
                for (int i = 0; i < degree; i++) { _knotVector[i] = knotVector[0]; }
                for (int i = 0; i < i_MaxKnot - degree + 1; i++)
                {
                    _knotVector[degree + i] = knotVector[i];
                }
                for (int i = i_MaxKnot - degree + 1; i < _knotVector.Length; i++) { _knotVector[i] = knotVector[knotVector.Length - 1]; }
            }
            else { throw new ArgumentException("The number of knots is not valid.", "knotVector"); }
        }

        #endregion

        #region Methods

        /// <inheritdoc/>
        public double Length()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ICurve{TVector}.Flip"/> 
        public void Flip()
        {
            double[] knotVector = new double[KnotCount];
            double startKnot = _knotVector[0], endKnot = _knotVector[KnotCount - 1];
            for (int i_K = 0; i_K < KnotCount; i_K++)
            {
                knotVector[i_K] = endKnot + (startKnot - _knotVector[KnotCount - 1 - i_K]);
            }

            TPoint[] controlPoints = new TPoint[PointCount];
            for (int i_P = 0; i_P < PointCount; i_P++)
            {
                controlPoints[i_P] = _controlPoints[PointCount - 1 - i_P];
            }

            double[] weights = new double[PointCount];
            for (int i_W = 0; i_W < PointCount; i_W++)
            {
                weights[i_W] = weights[PointCount - 1 - i_W];
            }

            _knotVector = knotVector;
            _controlPoints = controlPoints;
            _weights = weights;
        }

        /// <inheritdoc/>
        public TPoint PointAt(double parameter, CurveParameterFormat format)
        {
            if (format == CurveParameterFormat.Normalised)
            {
                int i_KnotSpan = Arith_Spe.BSplinePolynomial.FindKnotSpanIndex(parameter, Degree, _knotVector);

                double[] bSplines = Arith_Spe.BSplinePolynomial.EvaluateBasisAt(parameter, i_KnotSpan, Degree, _knotVector);

                // Initialisation
                double denominator = bSplines[0] * _weights[i_KnotSpan - Degree];
                TPoint result = _controlPoints[i_KnotSpan - Degree].Multiply(denominator);

                // Iteration
                for (int i = 1; i < Degree + 1; i++)
                {
                    double ajustedWeigth = bSplines[i] * _weights[i_KnotSpan - Degree + i];

                    TPoint temp = _controlPoints[i_KnotSpan - Degree + i].Multiply(ajustedWeigth);
                    result = result.Add(temp);
                    denominator += ajustedWeigth;
                }

                return result.Divide(denominator);
            }
            else if(format == CurveParameterFormat.ArcLength)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
