using System;
using System.Collections.Generic;

using Arith_Spe = BRIDGES.Arithmetic.Polynomials.Specials;


namespace BRIDGES.Geometry.Euclidean3D
{

    /// <summary>
    /// Class defining a B-Spline curve in three-dimensional euclidean space.
    /// </summary>
    public class BSplineCurve : Kernel.BSplineCurve<Point>
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of <see cref="BSplineCurve"/> class.
        /// </summary>
        /// <param name="degree"> Degree of the interpolating <see cref="Arith_Spe.BSpline"/> polynomial basis. </param>
        /// <param name="controlPoints"> Control points of the <see cref="BSplineCurve"/>. </param>
        /// <exception cref="ArgumentException"> The degree of the curve should be positive. </exception>
        public BSplineCurve(int degree, IEnumerable<Point> controlPoints) 
            : base(degree, controlPoints)
        {
            /* Do nothing */
        }

        /// <summary>
        /// Initialises a new instance of <see cref="BSplineCurve"/> class by defining its fields.
        /// </summary>
        /// <param name="degree"> Degree of the interpolating <see cref="Arith_Spe.BSpline"/> polynomial basis. </param>
        /// <param name="knotVector"> Knot vector of the interpolating <see cref="Arith_Spe.BSpline"/> polynomial basis. </param>
        /// <param name="controlPoints"> Control points of the <see cref="BSplineCurve"/>. </param>
        /// <exception cref="ArgumentException"> The knots should be provided in ascending order. </exception>
        /// <exception cref="ArgumentException"> The number of knots provided is not valid. </exception>
        /// <exception cref="ArgumentException"> The degree of the curve should be positive. </exception>
        public BSplineCurve(int degree, IEnumerable<double> knotVector, IEnumerable<Point> controlPoints)
            : base(degree, knotVector, controlPoints)
        {
            /* Do nothing */
        }

        #endregion
    }
}
