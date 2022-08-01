using System;
using System.Collections.Generic;

using Arith_Spe = BRIDGES.Arithmetic.Polynomials.Specials;


namespace BRIDGES.Geometry.Euclidean3D
{

    /// <summary>
    /// Class defining a B-Spline surface in three-dimensional euclidean space.
    /// </summary>
    public class BSplineSurface : Kernel.BSplineSurface<Point>
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of <see cref="BSplineSurface"/> class.
        /// </summary>
        /// <param name="degreeU"> Degree of the interpolating polynomials in the <see cref="Arith_Spe.BSpline"/> basis in u-direction. </param>
        /// <param name="degreeV"> Degree of the interpolating polynomials in the <see cref="Arith_Spe.BSpline"/> basis in v-direction. </param>
        /// <param name="controlPoints"> Control points of the <see cref="BSplineSurface"/>. </param>
        /// <exception cref="ArgumentException"> The degree of the surface in u-direction and v-direction should be positive. </exception>
        public BSplineSurface(int degreeU, int degreeV, Point[,] controlPoints)
            :base(degreeU, degreeV, controlPoints)
        {

        }

        #endregion
    }
}
