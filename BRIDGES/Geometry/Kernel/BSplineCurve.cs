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
        /// Gets the curve degree.
        /// </summary>
        public int Degree { get; protected set; }

        /// <summary>
        /// Gets the number of knots.
        /// </summary>
        public int KnotCount 
        { 
            get { return _knotVector.Count; } 
        }

        /// <summary>
        /// Gets the number of control points.
        /// </summary>
        public int PointCount 
        { 
            get { return _controlPoints.Count; } 
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of <see cref="BSplineCurve{TPoint}"/>.
        /// </summary>
        protected BSplineCurve()
        {
            // Instanciate Fields
            _controlPoints = new List<TPoint>();
            _knotVector = new List<double>();
        }

        /// <summary>
        /// Initialises a new instance of <see cref="BSplineCurve{TPoint}"/> class.
        /// </summary>
        /// <param name="degree"> Degree of the interpolating <see cref="Arith_Spe.BSpline"/> polynomial basis. </param>
        /// <param name="controlPoints"> Control points of the <see cref="BSplineCurve{TPoint}"/>. </param>
        /// <exception cref="ArgumentException"> The degree of the curve should be positive. </exception>
        public BSplineCurve(int degree, IEnumerable<TPoint> controlPoints)
        {
            // Initialise fields
            _controlPoints = new List<TPoint>();
            foreach (TPoint controlPoint in controlPoints)
            {
                _controlPoints.Add(controlPoint);
            }

            // Compute a "uniform" knot vector between 0.0 and 1.0
            double domainStart = 0.0, domainEnd = 1.0;

            int i_LastKnot = _controlPoints.Count + degree - 1;

            _knotVector = new List<double>(degree + _controlPoints.Count);
            for (int i = 0; i < (degree + 1); i++) // Constant knots at the start
            {
                _knotVector[i] = domainStart; 
            }
            for (int i = (degree + 1); i < (i_LastKnot - degree); i++) // Varying knots in the middle
            {
                var ratio = (double)(i - degree) / ((double)(i_LastKnot - 2 * degree));
                _knotVector[i] = domainStart + (domainEnd - domainStart) * ratio;
            }
            for (int i = (i_LastKnot - degree); i < (i_LastKnot + 1); i++) // Constant knots at the end
            { 
                _knotVector[i] = domainEnd; 
            }

            // Initialise properties
            if (degree < 0) { throw new ArgumentException("The degree of the curve should be positive.", nameof(degree)); }
            Degree = degree;

        }

        /// <summary>
        /// Initialises a new instance of <see cref="BSplineCurve{TControlPoint}"/> class by defining its fields.
        /// </summary>
        /// <param name="degree"> Degree of the interpolating <see cref="Arith_Spe.BSpline"/> polynomial basis. </param>
        /// <param name="knotVector"> Knot vector of the interpolating <see cref="Arith_Spe.BSpline"/> polynomial basis. </param>
        /// <param name="controlPoints"> Control points of the <see cref="BSplineCurve{TControlPoint}"/>. </param>
        /// <exception cref="ArgumentException"> The knots should be provided in ascending order. </exception>
        /// <exception cref="ArgumentException"> The number of knots provided is not valid. </exception>
        /// <exception cref="ArgumentException"> The degree of the curve should be positive. </exception>
        public BSplineCurve(int degree, IEnumerable<double> knotVector, IEnumerable<TPoint> controlPoints)
        {
            // Initialise fields
            _controlPoints = new List<TPoint>();
            foreach (TPoint controlPoint in controlPoints)
            {
                _controlPoints.Add(controlPoint);
            }

            _knotVector = new List<double>(_controlPoints.Count + degree);
            foreach(double knot in knotVector)
            {
                if (knot < _knotVector[_knotVector.Count - 1]) 
                {
                    throw new ArgumentException("The knots should be provided in ascending order.", nameof(knotVector));
                }
                _knotVector.Add(knot);
            }

            if(_knotVector.Count - 1 != (_controlPoints.Count + degree)) 
            {
                throw new ArgumentException($"The number of knots provided is not valid. {_controlPoints.Count + degree} knots are expected.", nameof(knotVector));
            }

            // Initialise properties
            if (degree < 0) { throw new ArgumentException("The degree of the curve should be positive.", nameof(degree)); }
            Degree = degree;
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
            List<double> knotVector = new List<double>(KnotCount);
            double startKnot = _knotVector[0], endKnot = _knotVector[KnotCount - 1];
            for (int i_K = 0; i_K < KnotCount; i_K++)
            {
                knotVector[i_K] = endKnot + (startKnot - _knotVector[KnotCount - 1 - i_K]);
            }

            List<TPoint> controlPoints = new List<TPoint>(PointCount);
            for (int i_P = 0; i_P < PointCount; i_P++)
            {
                controlPoints[i_P] = _controlPoints[PointCount - 1 - i_P];
            }

            _knotVector = knotVector;
            _controlPoints = controlPoints;
        }


        /// <inheritdoc/>
        public TPoint PointAt(double parameter, CurveParameterFormat format)
        {
            if (format == CurveParameterFormat.Normalised)
            {
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
    }
}
