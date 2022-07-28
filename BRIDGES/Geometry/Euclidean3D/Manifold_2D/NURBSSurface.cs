using System;
using System.Collections.Generic;

using Arith_Spe = BRIDGES.Arithmetic.Polynomials.Specials;


namespace BRIDGES.Geometry.Euclidean3D
{

    /// <summary>
    /// Class defining a NURBS surface in three-dimensional euclidean space.
    /// </summary>
    public class NURBSSurface : Kernel.BSplineSurface<Projective3D.Point>
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of <see cref="BSplineSurface"/> class.
        /// </summary>
        /// <param name="degreeU"> Degree of the interpolating polynomials in the <see cref="Arith_Spe.BSpline"/> basis in u-direction. </param>
        /// <param name="degreeV"> Degree of the interpolating polynomials in the <see cref="Arith_Spe.BSpline"/> basis in v-direction. </param>
        /// <param name="controlPoints"> Control points of the <see cref="BSplineSurface"/>. </param>
        /// <exception cref="ArgumentException"> The degree of the surface in u-direction and v-direction should be positive. </exception>
        public NURBSSurface(int degreeU, int degreeV, Point[,] controlPoints)
            :base()
        {
            if (degreeU < 0) { throw new ArgumentException("The degree of the surface in u-direction should be positive.", nameof(degreeU)); }
            if (degreeV < 0) { throw new ArgumentException("The degree of the surface in v-direction should be positive.", nameof(degreeV)); }

            // Initialise properties
            DegreeU = degreeU;
            DegreeV = degreeV;

            // Initialise fields
            SetControlPoints(controlPoints);

            SetUniformKnotVectors((0.0, 1.0), (0.0, 1.0), (degreeU, degreeV), (degreeU + controlPoints.GetLength(0), degreeV + controlPoints.GetLength(1)));

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Evaluates the current surface at the given parameter.
        /// </summary>
        /// <param name="parameter"> Value of the parameter. </param>
        /// <returns> The point on the surface at the given parameter. </returns>
        /// <exception cref="NotImplementedException"></exception>
        public new Point PointAt((double, double) parameter)
        {
            return (Point)base.PointAt(parameter);
        }

        #endregion


        #region Other Methods

        /// <summary>
        /// Converts the control points for the current <see cref="NURBSSurface"/>.
        /// </summary>
        /// <param name="controlPoints"> <see cref="Point"/> to convert. </param>
        /// <returns> The <see cref="Projective3D.Point"/> resulting from the conversion. </returns>
        protected void SetControlPoints(Point[,] controlPoints)
        {
            int uCount = controlPoints.GetLength(0);
            int vCount = controlPoints.GetLength(1);

            _controlPoints = new Projective3D.Point[uCount, vCount];
            for (int i_U = 0; i_U < uCount; i_U++)
            {
                for (int i_V = 0; i_V < vCount; i_V++)
                {
                    Point point = controlPoints[i_U, i_V];
                    _controlPoints[i_U, i_V] = new Projective3D.Point(point.X, point.Y, point.Z, 1.0);
                }
            }
        }

        #endregion
    }
}
