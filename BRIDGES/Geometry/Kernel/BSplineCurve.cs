using System;
using System.Collections.Generic;

using Alg_Set = BRIDGES.Algebra.Sets;

using Arith_Spe = BRIDGES.Arithmetic.Polynomials.Specials;


namespace BRIDGES.Geometry.Kernel
{
    /// <summary>
    /// Class defining a generic B-Spline curve.
    /// </summary>
    /// <typeparam name="TPoint"> Type of point in the geometric space. </typeparam>
    public class BSplineCurve<TPoint>
        : ICurve<TPoint>
        where TPoint : Alg_Set.Additive.IMagma<TPoint>, Alg_Set.IGroupAction<double, TPoint>
    {
        #region Fields

        /// <summary>
        /// Knot vector.
        /// </summary>
        protected List<double> _knotVector;

        /// <summary>
        /// Control points.
        /// </summary>
        protected List<TPoint> _controlPoints;

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
            get { return _controlPoints[_controlPoints.Count - 1]; }
        }


        /// <inheritdoc/>
        public double DomainStart
        { 
            get { return _knotVector[0]; } 
        }

        /// <inheritdoc/>
        public double DomainEnd
        {
            get { return _knotVector[_knotVector.Count - 1]; }
        }


        /// <summary>
        /// Gets the degree of the interpolating polynomials in the <see cref="Arith_Spe.BSpline"/> basis.
        /// </summary>
        public int Degree { get; protected set; }

        /// <summary>
        /// Number of knots of the interpolating <see cref="Arith_Spe.BSpline"/> polynomial basis.
        /// </summary>
        public int KnotCount 
        { 
            get { return _knotVector.Count; } 
        }

        /// <summary>
        /// Gets the number of control points of the curve.
        /// </summary>
        public int PointCount 
        { 
            get { return _controlPoints.Count; } 
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of <see cref="BSplineCurve{TPoint}"/> class.
        /// </summary>
        protected BSplineCurve()
        {
            /* Do nothing */
        }

        /// <summary>
        /// Initialises a new instance of <see cref="BSplineCurve{TPoint}"/> class.
        /// </summary>
        /// <param name="degree"> Degree of the interpolating polynomials in the <see cref="Arith_Spe.BSpline"/> basis. </param>
        /// <param name="controlPoints"> Control points of the <see cref="BSplineCurve{TPoint}"/>. </param>
        /// <exception cref="ArgumentException"> The degree of the curve should be positive. </exception>
        public BSplineCurve(int degree, IEnumerable<TPoint> controlPoints)
        {
            if (degree < 0)
            {
                throw new ArgumentException("The degree of the curve should be positive.", nameof(degree));
            }

            // Initialise properties
            Degree = degree;

            // Initialise fields
            SetControlPoints(controlPoints);
            
            SetUniformKnotVector(0.0, 1.0, degree, degree + _controlPoints.Count + 1);
        }

        /// <summary>
        /// Initialises a new instance of <see cref="BSplineCurve{TControlPoint}"/> class by defining its fields.
        /// </summary>
        /// <param name="degree"> Degree of the interpolating polynomials in the <see cref="Arith_Spe.BSpline"/> basis. </param>
        /// <param name="knotVector"> Knot vector of the interpolating <see cref="Arith_Spe.BSpline"/> polynomial basis. </param>
        /// <param name="controlPoints"> Control points of the <see cref="BSplineCurve{TControlPoint}"/>. </param>
        /// <exception cref="ArgumentException"> The degree of the curve should be positive. </exception>
        public BSplineCurve(int degree, IEnumerable<double> knotVector, IEnumerable<TPoint> controlPoints)
        {
            if (degree < 0) 
            { 
                throw new ArgumentException("The degree of the curve should be positive.", nameof(degree));
            }

            // Initialise properties
            Degree = degree;

            // Initialise fields
            SetControlPoints(controlPoints);

            // Sets the knot
            SetKnotVector(knotVector);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the knot at the given index.
        /// </summary>
        /// <param name="index"> Index of the knot to look for. </param>
        /// <returns> The knot at the given index. </returns>
        public double GetKnot(int index)
        {
            return _knotVector[index];
        }

        /// <inheritdoc/>
        public double Length()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ICurve{TVector}.Flip"/> 
        public void Flip()
        {
            int knotCount = _knotVector.Count;
            List<double> knotVector = new List<double>(knotCount);

            double startKnot = _knotVector[0];
            double endKnot = _knotVector[knotCount - 1];

            for (int i_K = 0; i_K < KnotCount; i_K++)
            {
                knotVector.Add(endKnot + (startKnot - _knotVector[KnotCount - 1 - i_K]));
            }

            int pointCount = _controlPoints.Count;
            List<TPoint> controlPoints = new List<TPoint>(pointCount);
            for (int i_P = 0; i_P < pointCount; i_P++)
            {
                controlPoints.Add(_controlPoints[PointCount - 1 - i_P]);
            }

            _knotVector = knotVector;
            _controlPoints = controlPoints;
        }


        /// <inheritdoc/>
        public TPoint PointAt(double parameter, CurveParameterFormat format)
        {
            if (format == CurveParameterFormat.Normalised)
            {
                if (parameter < DomainStart || DomainEnd < parameter)
                {
                    throw new ArgumentOutOfRangeException(nameof(parameter), "The parameter in not within the curve's domain.");
                }

                int i_KnotSpan = Arith_Spe.BSpline.FindKnotSpanIndex(parameter, Degree, _knotVector);

                double[] bSplines = Arith_Spe.BSpline.EvaluateBasisAt(parameter, i_KnotSpan, Degree, _knotVector);

                // Initialisation
                TPoint result = _controlPoints[i_KnotSpan - Degree].Multiply(bSplines[0]);

                // Iteration
                for (int i = 1; i < Degree + 1; i++)
                {
                    TPoint temp = _controlPoints[i_KnotSpan - Degree + i].Multiply(bSplines[i]);
                    result = result.Add(temp);
                }

                return result;
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

        #region Other Methods

        /// <summary>
        /// Sets the control points of the current <see cref="BSplineCurve{TPoint}"/>.
        /// </summary>
        /// <param name="controlPoints"> Control points of the <see cref="BSplineCurve{TPoint}"/>. </param>
        /// <exception cref="ArgumentException"> The number of control points provided is not valid. </exception>
        protected void SetControlPoints(IEnumerable<TPoint> controlPoints)
        {
            // Initialise fields
            _controlPoints = new List<TPoint>();
            foreach (TPoint controlPoint in controlPoints)
            {
                _controlPoints.Add(controlPoint);
            }

            if (_controlPoints.Count == 0) 
            {
                throw new ArgumentException("The number of control points provided is not valid.");
            }
        }

        /// <summary>
        /// Sets the knot vector of the current <see cref="BSplineCurve{TPoint}"/> while ensuring its validity.
        /// </summary>
        /// <param name="knotVector"> Knot vector to set. </param>
        /// <exception cref="ArgumentException"> The number of knots provided is not valid. </exception>
        /// <exception cref="ArgumentException"> The knots should be provided in ascending order. </exception>
        protected void SetKnotVector(IEnumerable<double> knotVector)
        {
            IEnumerator<double> knotEnumerator = knotVector.GetEnumerator();

            try
            {
                _knotVector = new List<double>(_controlPoints.Count + Degree);

                double previousKnot;
                if (knotEnumerator.MoveNext())
                {
                    previousKnot = knotEnumerator.Current;
                    _knotVector.Add(previousKnot);
                }
                else { throw new ArgumentException($"The number of knots provided is not valid. {_controlPoints.Count + Degree} knots are expected.", nameof(knotVector)); }

                while (knotEnumerator.MoveNext())
                {
                    double knot = knotEnumerator.Current;

                    if (knot < previousKnot) { throw new ArgumentException("The knots should be provided in ascending order.", nameof(knotVector)); }

                    _knotVector.Add(knot);

                    previousKnot = knot;
                }

                if (_knotVector.Count - 1 != (_controlPoints.Count + Degree))
                {
                    throw new ArgumentException($"The number of knots provided is not valid. {_controlPoints.Count + Degree} knots are expected.", nameof(knotVector));
                }
            }
            finally { knotEnumerator.Dispose(); }
        }

        /// <summary>
        /// Creates and sets the knot vector for the current <see cref="BSplineCurve{TPoint}"/> with a uniform middle part.
        /// </summary>
        /// <param name="domainStart"> Start value of the curve domain. </param>
        /// <param name="domainEnd"> End value of the curve domain. </param>
        /// <param name="degree"> Degree of the interpolating polynomials in the <see cref="Arith_Spe.BSpline"/> basis. </param>
        /// <param name="knotCount"> Number of knots of the interpolating <see cref="Arith_Spe.BSpline"/> polynomial basis. </param>
        protected void SetUniformKnotVector(double domainStart, double domainEnd, int degree, int knotCount)
        {
            int i_LastKnot = knotCount - 1;

            _knotVector = new List<double>(knotCount);
            for (int i = 0; i < (degree + 1); i++) // Constant knots at the start
            {
                _knotVector.Add(domainStart);
            }
            for (int i = (degree + 1); i < (i_LastKnot - degree); i++) // Varying knots in the middle
            {
                var ratio = (double)(i - degree) / ((double)(i_LastKnot - 2 * degree));
                _knotVector.Add(domainStart + (domainEnd - domainStart) * ratio);
            }
            for (int i = i_LastKnot - degree; i < knotCount; i++) // Constant knots at the end
            {
                _knotVector.Add(domainEnd);
            }
        }

        #endregion
    }

}
