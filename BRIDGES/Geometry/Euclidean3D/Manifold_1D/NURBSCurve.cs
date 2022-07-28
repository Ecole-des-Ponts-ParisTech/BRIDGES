using System;
using System.Collections.Generic;

using Arith_Spe = BRIDGES.Arithmetic.Polynomials.Specials;


namespace BRIDGES.Geometry.Euclidean3D
{
    /// <summary>
    /// Class defining a NURBS curve in three-dimensional euclidean space.
    /// </summary>
    public class NURBSCurve : Kernel.BSplineCurve<Projective3D.Point>
    {
        #region Properties

        /// <inheritdoc cref="Kernel.ICurve{TPoint}.StartPoint"/>
        public new Point StartPoint
        {
            get { return (Point)_controlPoints[0]; }
        }

        /// <inheritdoc cref="Kernel.ICurve{TPoint}.EndPoint"/>
        public new Point EndPoint
        {
            get { return (Point)_controlPoints[_controlPoints.Count - 1]; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of <see cref="NURBSCurve"/> class.
        /// </summary>
        /// <param name="degree"> Degree of the interpolating <see cref="Arith_Spe.BSpline"/> polynomial basis. </param>
        /// <param name="controlPoints"> Control points of the <see cref="NURBSCurve"/>. </param>
        /// <exception cref="ArgumentException"> The degree of the curve should be positive. </exception>
        public NURBSCurve(int degree, IEnumerable<Point> controlPoints)
            : base()
        {
            // Initialise fields
            foreach(Point controlPoint in controlPoints)
            {
                _controlPoints.Add(new Projective3D.Point(controlPoint.X, controlPoint.Y, controlPoint.Z, 1.0));
            }

            // Compute a "uniform" knot vector between 0.0 and 1.0
            double domainStart = 0.0, domainEnd = 1.0;

            int i_LastKnot = _controlPoints.Count + degree - 1;

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
        /// Initialises a new instance of <see cref="NURBSCurve"/> class by defining its fields.
        /// </summary>
        /// <param name="degree"> Degree of the interpolating <see cref="Arith_Spe.BSpline"/> polynomial basis. </param>
        /// <param name="knotVector"> Knot vector of the interpolating <see cref="Arith_Spe.BSpline"/> polynomial basis. </param>
        /// <param name="controlPoints"> Control points of the <see cref="NURBSCurve"/>. </param>
        /// <param name="weights"> Weights of the control points. </param>
        /// <exception cref="ArgumentException"> The numbers of weights and control points should be the same. </exception>
        /// <exception cref="ArgumentException"> The knots should be provided in ascending order. </exception>
        /// <exception cref="ArgumentException"> The number of knots provided is not valid. </exception>
        /// <exception cref="ArgumentException"> The degree of the curve should be positive. </exception>
        public NURBSCurve(int degree, IEnumerable<double> knotVector, IEnumerable<Point> controlPoints, IEnumerable<double> weights)
            : base()
        {
            // Initialise fields
            IEnumerator<Point> pointsEnumerator = controlPoints.GetEnumerator();
            IEnumerator<double> weightsEnumerator = weights.GetEnumerator();
            try 
            {
                while(pointsEnumerator.MoveNext())
                {
                    if (!weightsEnumerator.MoveNext()) 
                    { 
                        throw new ArgumentException("The numbers of weights and control points should be the same."); 
                    }

                    Point controlPoint = pointsEnumerator.Current;
                    double weight = weightsEnumerator.Current;

                    _controlPoints.Add(new Projective3D.Point(controlPoint.X, controlPoint.Y, controlPoint.Z, weight));
                }

                if (weightsEnumerator.MoveNext())
                {
                    throw new ArgumentException("The numbers of weights and control points should be the same.");
                }
            }
            finally
            {
                pointsEnumerator.Dispose();
                weightsEnumerator.Dispose();
            }

            _knotVector = new List<double>(_controlPoints.Count + degree);
            foreach (double knot in knotVector)
            {
                if (knot < _knotVector[_knotVector.Count - 1])
                {
                    throw new ArgumentException("The knots should be provided in ascending order.", nameof(knotVector));
                }
                _knotVector.Add(knot);
            }

            if (_knotVector.Count - 1 != (_controlPoints.Count + degree))
            {
                throw new ArgumentException($"The number of knots provided is not valid. {_controlPoints.Count + degree} knots are expected.", nameof(knotVector));
            }

            // Initialise properties
            if (degree < 0) { throw new ArgumentException("The degree of the curve should be positive.", nameof(degree)); }
            Degree = degree;
        }

        #endregion

        #region Methods

        /// <inheritdoc cref="Kernel.ICurve{TPoint}.PointAt(double, Kernel.CurveParameterFormat)"/>
        public new Point PointAt(double parameter, Kernel.CurveParameterFormat format)
        {
            return (Point)base.PointAt(parameter, format);
        }

        #endregion
    }
}
