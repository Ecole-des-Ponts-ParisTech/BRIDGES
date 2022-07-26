using System;
using System.Collections.Generic;
using System.Text;

namespace BRIDGES.Geometry.Euclidean3D
{

    /// <summary>
    /// Class defining a generic NURBS Curve in three-dimensional euclidean space.
    /// </summary>
    public class NURBSCurve : Kernel.NURBSCurve<Point>
    {
        /// <inheritdoc/>
        public NURBSCurve(int degree, Point[] controlPoints) 
            : base(degree, controlPoints)
        {
        }

        /// <inheritdoc/>
        public NURBSCurve(int degree, double[] knotVector, Point[] controlPoints, double[] weights)
            : base(degree, knotVector, controlPoints, weights)
        {

        }
    }
}
