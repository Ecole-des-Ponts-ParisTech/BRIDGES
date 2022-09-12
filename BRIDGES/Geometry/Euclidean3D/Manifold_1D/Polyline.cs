using System;
using System.Collections.Generic;

using Geo_Ker = BRIDGES.Geometry.Kernel;


namespace BRIDGES.Geometry.Euclidean3D
{
    /// <summary>
    /// Class defining a polyline curve in three-dimensional euclidean space.<br/>
    /// It is defined by an ordered list of vertices. 
    /// </summary>
    public class Polyline : Geo_Ker.ICurve<Point>
    {
        #region Fields

        /// <summary>
        /// Vertices of the current <see cref="Polyline"/>.
        /// </summary>
        /// <remarks> 
        /// If the polyline is closed, the last vertex must be equal to the first one.<br/> 
        /// However, if the last vertex matches the first one, the polyline is not necessarily closed.
        /// </remarks>
        private List<Point> _vertices;

        #endregion

        #region Properties

        /// <inheritdoc/>
        public bool IsClosed { get; set; }


        /// <inheritdoc/>
        public Point StartPoint
        {
            get { return _vertices[0]; }
        }

        /// <inheritdoc/>
        public Point EndPoint
        {
            get { return IsClosed ? _vertices[0] : _vertices[_vertices.Count - 1]; }
        }


        /// <inheritdoc/>
        public double DomainStart 
        { 
            get { return 0.0; }
        }

        /// <inheritdoc/>
        public double DomainEnd
        {
            get { return _vertices.Count - 1; }
        }


        /// <summary>
        /// Gets the vertex of the <see cref="Polyline"/> at the given index.
        /// </summary>
        /// <param name="index"> Index of the vertex to retrieve. </param>
        /// <returns> The <see cref="Point"/> representing the position of the vertex. </returns>
        public Point this[int index] 
        {
            get { return _vertices[index]; }
        }

        /// <summary>
        /// Gets the number of vertices of the current <see cref="Polyline"/>.
        /// </summary>
        public int VertexCount 
        {
            get { return _vertices.Count; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Polyline"/> class by defining its vertices.
        /// </summary>
        /// <param name="vertices"> Position of the vertices. </param>
        /// <param name="isClosed"> Determines whether the new <see cref="Polyline"/> is closed or not. </param>
        public Polyline(Point[] vertices, bool isClosed = false)
        {
            IsClosed = isClosed;

            _vertices = new List<Point> (vertices);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Polyline"/> class from another <see cref="Polyline"/>.
        /// </summary>
        /// <param name="polyline"> <see cref="Polyline"/> to copy. </param>
        public Polyline(Polyline polyline)
        {
            IsClosed = polyline.IsClosed;

            _vertices = new List<Point>(polyline._vertices);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a vertex at the end of the polyline.
        /// </summary>
        /// <param name="vertex"> Position of the new vertex. </param>
        public void AddVertex(Point vertex)
        {
            _vertices.Add(vertex);
        }

        /// <summary>
        /// Adds a list of vertices at the end of the polyline.
        /// </summary>
        /// <param name="vertices"> Position of the new vertices. </param>
        public void AddVertices(List<Point> vertices)
        {
            _vertices.AddRange(vertices);
        }

        /// <summary>
        /// Inserts a vertex in the polyline at the given index.
        /// </summary>
        /// <param name="index"> Zero-based index at which the vertex should be inserted. </param>
        /// <param name="vertex"> Position of the new vertex. </param>
        public void InsertVertex(int index, Point vertex)
        {
            _vertices.Insert(index, vertex);
        }


        /// <summary>
        /// Flips the current <see cref="Polyline"/> by reversing the list of vertices.
        /// </summary>
        public void Flip()
        {
            _vertices.Reverse();
        }

        /// <summary>
        /// Computes the length of the current <see cref="Polyline"/>.
        /// </summary>
        /// <returns> The value corresponding to the <see cref="Polyline"/>'s length. </returns>
        /// <exception cref="InvalidOperationException"> The length can't be computed. The current <see cref="Polyline"/> doesn't have any vertices. </exception>
        public double Length()
        {
            // If the polyline is empty.
            if (_vertices.Count == 0) { throw new InvalidOperationException("The length can't be computed. The current polyline doesn't have any vertices."); }
            // If the polyline is reduced to a point.
            else if (_vertices.Count == 1) { return 0.0; }
            // If the polyline has several vertices.
            else
            {
                double length = 0.0;
                // Calculates the length of the main part.
                for (int i = 0; i < _vertices.Count - 1; i++)
                {
                    length += (_vertices[i + 1] - _vertices[i]).Norm();
                }

                return length;
            }
        }

        /// <summary>
        /// Evaluates the current <see cref="Polyline"/> at the given parameter.
        /// </summary>
        /// <param name="parameter"> Value of the parameter. </param>
        /// <param name="format"> Format of the parameter. </param>
        /// <returns> The <see cref="Point"/> on the <see cref="Polyline"/> at the given parameter. </returns>
        /// <exception cref="ArgumentOutOfRangeException"> The input curve parameter cannot be negative. </exception>
        public Point PointAt(double parameter, Geo_Ker.CurveParameterFormat format)
        {
            if (format == Geo_Ker.CurveParameterFormat.ArcLength) { return PointAt_LengthParameter(parameter); }
            else if (format == Geo_Ker.CurveParameterFormat.Normalised) { return PointAt_NormalizedParameter(parameter); }

            else { throw new NotImplementedException(); }
        }


        /// <summary>
        /// Retrieves the closest point on a polyline to a given point, and the distance from the point to the polyline.
        /// </summary>
        /// <param name="point"> Point to find the closest point from. </param>
        /// <param name="t"> Distance from the given point and the point on the polyline. </param>
        /// <returns> The closest point on the polyline. </returns>
        public Point ClosestPoint(Point point, out double t)
        {
            // Initialization
            t = _vertices[0].DistanceTo(point);
            Point closePoint = _vertices[0];

            // Find the closest "edge"
            for (int i = 0; i < _vertices.Count - 1; i++)
            {
                Vector edgeDir = _vertices[i + 1] - _vertices[i];
                edgeDir.Unitise();
                double alpha = Point.DotProduct(point - _vertices[i], edgeDir);

                if (alpha <= 0)
                {
                    double d = _vertices[i].DistanceTo(point);
                    if (d < t)
                    {
                        t = d;
                        closePoint = _vertices[i];
                    }
                }
                else if (alpha >= _vertices[i + 1].DistanceTo(_vertices[i]))
                {
                    double d = _vertices[i + 1].DistanceTo(point);
                    if (d < t)
                    {
                        t = d;
                        closePoint = _vertices[i + 1];
                    }
                }
                else
                {
                    Point scaledEdge = _vertices[i] + (alpha * edgeDir);
                    double d = point.DistanceTo(scaledEdge);
                    if (d < t)
                    {
                        t = d;
                        closePoint = _vertices[i] + edgeDir;
                    }
                }
            }

            if (_vertices[_vertices.Count - 1].DistanceTo(point) < t)
            {
                t = _vertices[_vertices.Count - 1].DistanceTo(point);
                closePoint = _vertices[_vertices.Count - 1];
            }

            return closePoint;

        }


        /// <summary>
        /// Evaluates whether the current <see cref="Polyline"/> is equal to another <see cref="Polyline"/>.
        /// </summary>
        /// <remarks> 
        /// Two <see cref="Polyline"/> are equal if they have the same topology and their corresponding vertices are equal.
        /// </remarks>
        /// <param name="other"> <see cref="Polyline"/> to compare with. </param>
        /// <returns> <see langword="true"/> if the two <see cref="Polyline"/> are equal, <see langword="false"/> otherwise. </returns>
        public bool Equals(Polyline other)
        {
            if (IsClosed != other.IsClosed) { return false; }
            if (VertexCount != other.VertexCount) { return false; }

            for (int i = 0; i < VertexCount; i++)
            {
                if (!_vertices[i].Equals(other._vertices[i])) { return false; }
            }

            return true;
        }


        /********** Private Helpers **********/

        /// <summary>
        /// Evaluates the current <see cref="Polyline"/> at the given length parameter.
        /// </summary>
        /// <param name="parameter"> Positive length parameter. </param>
        /// <returns> The <see cref="Point"/> on the <see cref="Polyline"/> at the given length parameter. </returns>
        /// <exception cref="ArgumentOutOfRangeException"> The input length curve parameter is larger than the polyline length. </exception>
        private Point PointAt_LengthParameter(double parameter)
        {
            if (parameter < 0) { throw new ArgumentOutOfRangeException("The arc length parameter can not be negative."); }

            double cumulativeLength = 0.0;

            // If the parameter is in the main part.
            for (int i_Vertex = 0; i_Vertex < _vertices.Count - 1; i_Vertex++)
            {
                Vector axis = _vertices[i_Vertex + 1] - _vertices[i_Vertex];
                cumulativeLength += axis.Length();

                if (parameter <= cumulativeLength)
                {
                    double delta = parameter - (cumulativeLength - axis.Length());

                    axis.Unitise();

                    return _vertices[i_Vertex] + (delta * axis);
                }
            }

            // If the parameter is equal to the length of the polyline.
            if (parameter == cumulativeLength)
            {
                return IsClosed ? _vertices[0] : _vertices[_vertices.Count - 1];
            }

            // If the parameter is larger than the length of the polyline.
            else { throw new ArgumentOutOfRangeException("The arc length parameter is larger than the curve length."); }

        }

        /// <summary>
        /// Evaluates the current <see cref="Polyline"/> at the given normalised parameter.
        /// </summary>
        /// <param name="parameter"> Positive normalised parameter. </param>
        /// <returns> The <see cref="Point"/> on the <see cref="Polyline"/> at the given normalised parameter. </returns>
        /// <exception cref="ArgumentOutOfRangeException"> The normalized parameter is outside the segment's domain. </exception>
        private Point PointAt_NormalizedParameter(double parameter)
        {
            // If the parameter exceeds the curve domain.
            if (parameter < DomainStart || DomainEnd < parameter) { throw new ArgumentOutOfRangeException("The normalized parameter is outside the segment's domain."); }

            // If the parameter is equal to the upper bound of the curve domain.
            if (parameter == DomainEnd) { return _vertices[_vertices.Count - 1]; }

            // If the parameter is in the rest of the polyline.
            {
                int index = Convert.ToInt16(Math.Truncate(parameter));
                double delta = parameter - index;

                Point vertex = _vertices[index];
                Vector axis = _vertices[index + 1] - _vertices[index];

                return vertex + (delta * axis);
            }
        }

        #endregion

        #region Private Methods

        /********** For PointAt(Double,CurveParameterFormat) **********/

        #endregion


        #region Override : Object

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is Polyline polyline && Equals(polyline);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            if (IsClosed) { return $"Polyline with {VertexCount} vertices."; }
            else { return $"Closed polyline with {VertexCount} vertices."; }
            
        }

        #endregion
    }
}
