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
            if (degree < 0) 
            {
                throw new ArgumentException("The degree of the curve should be positive.", nameof(degree)); 
            }

            // Initialise properties
            Degree = degree;

            // Initialise fields
            IEnumerable<Projective3D.Point> projectivePoints =  ConvertControlPoints(controlPoints);
            base.SetControlPoints(projectivePoints);

            SetUniformKnotVector(0.0, 1.0, degree, degree + _controlPoints.Count);
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
            if (degree < 0)
            {
                throw new ArgumentException("The degree of the curve should be positive.", nameof(degree));
            }

            // Initialise properties
            Degree = degree;

            // Initialise fields
            IEnumerable<Projective3D.Point> projectivePoints = ConvertControlPoints(controlPoints, weights);
            base.SetControlPoints(projectivePoints);

            base.SetKnotVector(knotVector);
        }

        #endregion

        #region Public Methods

        /// <inheritdoc cref="Kernel.ICurve{TPoint}.PointAt(double, Kernel.CurveParameterFormat)"/>
        public new Point PointAt(double parameter, Kernel.CurveParameterFormat format)
        {
            return (Point)base.PointAt(parameter, format);
        }

        #endregion

        #region Other Methods

        /// <summary>
        /// Converts the control points for the current <see cref="NURBSCurve"/>.
        /// </summary>
        /// <param name="controlPoints"> <see cref="Point"/> to convert. </param>
        /// <returns> The <see cref="Projective3D.Point"/> resulting from the conversion. </returns>
        protected IEnumerable<Projective3D.Point> ConvertControlPoints(IEnumerable<Point> controlPoints)
        {
            foreach (Point controlPoint in controlPoints)
            {
                yield return new Projective3D.Point(controlPoint.X, controlPoint.Y, controlPoint.Z, 1.0);
            }
        }

        /// <summary>
        /// Converts the control points for the current <see cref="NURBSCurve"/>.
        /// </summary>
        /// <param name="controlPoints"> <see cref="Point"/> to convert. </param>
        /// <param name="weights"> Weigths to include in the conversion. </param>
        /// <returns> The <see cref="Projective3D.Point"/> resulting from the conversion. </returns>
        protected IEnumerable<Projective3D.Point> ConvertControlPoints(IEnumerable<Point> controlPoints, IEnumerable<double> weights)
        {
            // Initialise fields
            IEnumerator<Point> pointsEnumerator = controlPoints.GetEnumerator();
            IEnumerator<double> weightsEnumerator = weights.GetEnumerator();
            try
            {
                while (pointsEnumerator.MoveNext())
                {
                    if (!weightsEnumerator.MoveNext())
                    {
                        throw new ArgumentException("The numbers of weights and control points should be the same.");
                    }

                    Point controlPoint = pointsEnumerator.Current;
                    double weight = weightsEnumerator.Current;

                    yield return new Projective3D.Point(controlPoint.X, controlPoint.Y, controlPoint.Z, weight);
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
        }

        #endregion
    }
}

