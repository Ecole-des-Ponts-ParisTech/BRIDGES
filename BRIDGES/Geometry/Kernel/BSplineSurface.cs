using System;
using System.Collections.Generic;

using Alg_Set = BRIDGES.Algebra.Sets;

using Arith_Spe = BRIDGES.Arithmetic.Polynomials.Specials;


namespace BRIDGES.Geometry.Kernel
{
    /// <summary>
    /// Class defining a generic B-Spline surface.
    /// </summary>
    /// <typeparam name="TPoint"> Type of point in the geometric space. </typeparam>
    public class BSplineSurface<TPoint>
        where TPoint : Alg_Set.Additive.IMagma<TPoint>, Alg_Set.IGroupAction<double, TPoint>
    {
        #region Fields

        /// <summary>
        /// Knot vector in the u-direction.
        /// </summary>
        protected List<double> _knotVectorU;

        /// <summary>
        /// Knot vector in the v-direction.
        /// </summary>
        protected List<double> _knotVectorV;


        /// <summary>
        /// Control points.
        /// </summary>
        protected TPoint[,] _controlPoints;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the degree of the interpolating polynomials in the <see cref="Arith_Spe.BSpline"/> basis in u-direction.
        /// </summary>
        public int DegreeU { get; protected set; }

        /// <summary>
        /// Gets the degree of the interpolating polynomials in the <see cref="Arith_Spe.BSpline"/> basis in v-direction.
        /// </summary>
        public int DegreeV { get; protected set; }


        /// <summary>
        /// Number of knots of the interpolating <see cref="Arith_Spe.BSpline"/> polynomial basis in u-direction.
        /// </summary>
        public int KnotCountU { get { return _knotVectorU.Count; } }

        /// <summary>
        /// Number of knots of the interpolating <see cref="Arith_Spe.BSpline"/> polynomial basis in v-direction.
        /// </summary>
        public int KnotCountV { get { return _knotVectorV.Count; } }


        /// <summary>
        /// Gets the number of control points of the surface in u-direction..
        /// </summary>
        public int PointCountU { get { return _controlPoints.GetLength(0); } }

        /// <summary>
        /// Gets the number of control points of the surface in v-direction..
        /// </summary>
        public int PointCountV { get { return _controlPoints.GetLength(1); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of <see cref="BSplineSurface{TPoint}"/> class.
        /// </summary>
        protected BSplineSurface()
        {
            /* Do nothing */
        }

        /// <summary>
        /// Initialises a new instance of <see cref="BSplineSurface{TPoint}"/> class.
        /// </summary>
        /// <param name="degreeU"> Degree of the interpolating polynomials in the <see cref="Arith_Spe.BSpline"/> basis in u-direction. </param>
        /// <param name="degreeV"> Degree of the interpolating polynomials in the <see cref="Arith_Spe.BSpline"/> basis in v-direction. </param>
        /// <param name="controlPoints"> Control points of the <see cref="BSplineSurface{TPoint}"/>. </param>
        /// <exception cref="ArgumentException"> The degree of the surface in u-direction and v-direction should be positive. </exception>
        public BSplineSurface(int degreeU, int degreeV, TPoint[,] controlPoints)
        {
            if (degreeU  < 0) { throw new ArgumentException("The degree of the surface in u-direction should be positive.", nameof(degreeU)); }
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
        public TPoint PointAt( (double, double) parameter)
        {
            int i_KnotSpanU = Arith_Spe.BSpline.FindKnotSpanIndex(parameter.Item1, DegreeU, _knotVectorU);
            int i_KnotSpanV = Arith_Spe.BSpline.FindKnotSpanIndex(parameter.Item2, DegreeV, _knotVectorV);

            double[] bSplinesU = Arith_Spe.BSpline.EvaluateBasisAt(parameter.Item1, i_KnotSpanU, DegreeU, _knotVectorU);
            double[] bSplinesV = Arith_Spe.BSpline.EvaluateBasisAt(parameter.Item2, i_KnotSpanV, DegreeV, _knotVectorV);

            // Initialisation
            TPoint result = default;

            for (int i_U = 0; i_U < bSplinesU.Length; i_U++)
            {
                for (int i_V = 0; i_V < bSplinesV.Length; i_V++)
                {
                    TPoint temp = _controlPoints[i_KnotSpanU - DegreeU + i_U, i_KnotSpanV - DegreeV + i_V].Multiply(bSplinesU[i_U] * bSplinesU[i_V]);
                    result = result.Add(temp);
                }
            }

            return result;
        }

        #endregion

        #region Other Methods

        /// <summary>
        /// Sets the control points of the current <see cref="BSplineSurface{TPoint}"/>.
        /// </summary>
        /// <param name="controlPoints"> Control points of the <see cref="BSplineSurface{TPoint}"/>. </param>
        /// <exception cref="ArgumentException"> The number of control points provided is not valid. </exception>
        protected void SetControlPoints(TPoint[,] controlPoints)
        {
            int uCount = controlPoints.GetLength(0);
            int vCount = controlPoints.GetLength(1);

            _controlPoints = new TPoint[uCount, vCount];
            for (int i_U = 0; i_U < uCount; i_U++)
            {
                for (int i_V = 0; i_V < vCount; i_V++)
                {
                    _controlPoints[i_U, i_V] = controlPoints[i_U, i_V];
                }
            }

            if (uCount < 2 || vCount < 2)
            {
                throw new ArgumentException("The number of control points provided is not valid.");
            }
        }

        /// <summary>
        /// Creates and sets the knot vector for the current <see cref="BSplineSurface{TPoint}"/> with a uniform middle part.
        /// </summary>
        /// <param name="domainU"> Start and End value of the surface domain in u-direction. </param>
        /// <param name="domainV"> Start and End value of the surface domain in v-direction. </param>
        /// <param name="degrees"> Degrees of the interpolating polynomials in the <see cref="Arith_Spe.BSpline"/> basis in u and v directions. </param>
        /// <param name="knotCounts"> Number of knots of the interpolating <see cref="Arith_Spe.BSpline"/> polynomial basis in u and v directions. </param>
        protected void SetUniformKnotVectors((double, double) domainU, (double, double) domainV, (int,int) degrees, (int, int) knotCounts)
        {
            // Knot vector in u-direction
            int i_LastKnotU = knotCounts.Item1 - 1;

            _knotVectorU = new List<double>(knotCounts.Item1);
            for (int i = 0; i < (degrees.Item1 + 1); i++) // Constant knots at the start
            {
                _knotVectorU[i] = domainU.Item1;
            }
            for (int i = (degrees.Item1 + 1); i < (i_LastKnotU - degrees.Item1); i++) // Varying knots in the middle
            {
                var ratio = (double)(i - degrees.Item1) / ((double)(i_LastKnotU - 2 * degrees.Item1));
                _knotVectorU[i] = domainU.Item1 + (domainU.Item2 - domainU.Item1) * ratio;
            }
            for (int i = (i_LastKnotU - degrees.Item1); i < (i_LastKnotU + 1); i++) // Constant knots at the end
            {
                _knotVectorU[i] = domainU.Item2;
            }

            // Knot vector in v-direction
            int i_LastKnotV = knotCounts.Item2 - 1;

            _knotVectorV = new List<double>(knotCounts.Item2);
            for (int i = 0; i < (degrees.Item2 + 1); i++) // Constant knots at the start
            {
                _knotVectorV[i] = domainV.Item1;
            }
            for (int i = (degrees.Item2 + 1); i < (i_LastKnotV - degrees.Item2); i++) // Varying knots in the middle
            {
                var ratio = (double)(i - degrees.Item2) / ((double)(i_LastKnotV - 2 * degrees.Item2));
                _knotVectorV[i] = domainV.Item1 + (domainV.Item2 - domainV.Item1) * ratio;
            }
            for (int i = (i_LastKnotV - degrees.Item2); i < (i_LastKnotV + 1); i++) // Constant knots at the end
            {
                _knotVectorV[i] = domainV.Item2;
            }
        }

        #endregion
    }
}
