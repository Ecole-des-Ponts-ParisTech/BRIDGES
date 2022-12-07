using System;
using System.Collections.Generic;

using Geo_Ker = BRIDGES.Geometry.Kernel;


namespace BRIDGES.Geometry.Euclidean3D
{
    /// <summary>
    /// Class defining a polyline curve in three-dimensional euclidean space. 
    /// </summary>
    public class Polyline : IEquatable<Polyline>,
        Geo_Ker.ICurve<Point>, Geo_Ker.IGeometricallyEquatable<Polyline>
    {
        #region Fields

        /// <summary>
        /// Vertices of the current <see cref="Polyline"/>.
        /// </summary>
        /// <remarks> 
        /// If the polyline is closed, the last vertex is not a duplicate of the first one.
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
            get { return IsClosed ? _vertices.Count : _vertices.Count - 1; }
        }


        /// <summary>
        /// Gets the vertex of the <see cref="Polyline"/> at the given index.
        /// </summary>
        /// <param name="index"> Index of the vertex to retrieve. </param>
        /// <returns> The <see cref="Point"/> representing the position of the vertex. </returns>
        public Point this[int index] 
        {
            get { return _vertices[index]; }
            set { _vertices[index] = value; }
        }


        /// <summary>
        /// Gets the number of vertices of the current <see cref="Polyline"/>.
        /// </summary>
        public int VertexCount 
        {
            get { return _vertices.Count; }
        }

        /// <summary>
        /// Gets the number of segments of the current <see cref="Polyline"/>.
        /// </summary>
        public int SegmentCount
        {
            get { return IsClosed ? _vertices.Count : _vertices.Count - 1; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Polyline"/> class by defining its vertices.
        /// </summary>
        /// <param name="vertices"> Position of the vertices. </param>
        /// <param name="isClosed"> Determines whether the new <see cref="Polyline"/> is closed or not. </param>
        /// <exception cref="ArgumentException"> The polyline needs at least two vertices. </exception>
        public Polyline(IEnumerable<Point> vertices, bool isClosed = false)
        {
            IsClosed = isClosed;

            _vertices = new List<Point>(vertices);

            // Verification
            if (_vertices.Count < 2) { throw new ArgumentException("The polyline needs at least two vertices."); }
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
        /// Replaces a vertex in the polyline at the given index.
        /// </summary>
        /// <param name="index"> Zero-based index of the vertex to replace. </param>
        /// <param name="vertex"> New position of the vertex. </param>
        public void ReplaceVertex(int index, Point vertex)
        {
            _vertices[index] = vertex;
        }

        /// <summary>
        /// Replaces all the vertices in the polyline.
        /// </summary>
        /// <param name="vertices"> New position of the vertices. </param>
        public void ReplaceVertices(IEnumerable<Point> vertices)
        {
            _vertices.RemoveRange(0, VertexCount);
            _vertices.AddRange(vertices);

            // Verification
            if (_vertices.Count < 2) { throw new ArgumentException("The polyline needs at least two vertices."); }
        }


        /// <summary>
        /// Flips the current <see cref="Polyline"/> by reversing the list of vertices.
        /// </summary>
        public void Flip()
        {
            if(IsClosed)
            {
                Point vertex = _vertices[0];
                _vertices.RemoveAt(0);
                _vertices.Add(vertex);
            }

            _vertices.Reverse();
        }

        /// <summary>
        /// Computes the length of the current <see cref="Polyline"/>.
        /// </summary>
        /// <returns> The value corresponding to the <see cref="Polyline"/>'s length. </returns>
        public double Length()
        {
            double length = 0.0;
            // Calculates the length of the main part.
            for (int i = 0; i < _vertices.Count - 1; i++)
            {
                length += _vertices[i].DistanceTo(_vertices[i + 1]);
            }

            if(IsClosed) { length += _vertices[_vertices.Count - 1].DistanceTo(_vertices[0]); }

            return length;
        }

        /// <summary>
        /// Evaluates the current <see cref="Polyline"/> at the given parameter.
        /// </summary>
        /// <param name="t"> Parameter to evaluate the curve. </param>
        /// <param name="format"> Format of the parameter. 
        /// <list type="bullet">
        /// <item>
        /// <term>Normalised</term>
        /// <description> The point at <paramref name="t"/> = i is the vertex at index <em>i</em>. </description>
        /// </item>
        /// <item>
        /// <term>ArcLength</term>
        /// <description> The point at <paramref name="t"/> = 1.0 is at a distance 1.0 from the start point along the polyline. </description>
        /// </item>
        /// </list> </param>
        /// <returns> The <see cref="Point"/> on the <see cref="Polyline"/> at the given parameter. </returns>
        /// <exception cref="NotImplementedException"> The given format for the curve parameter is not implemented. </exception>
        public Point PointAt(double t, Geo_Ker.CurveParameterFormat format)
        {
            if (format == Geo_Ker.CurveParameterFormat.ArcLength) 
            { 
                return PointAt_LengthParameter(t); 
            }
            else if (format == Geo_Ker.CurveParameterFormat.Normalised) 
            { 
                return PointAt_NormalisedParameter(t);
            }
            else { throw new NotImplementedException("The given format for the curve parameter is not implemented."); }
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
            for (int i = 0; i < SegmentCount + 1; i++)
            {
                Point edgeStart = _vertices[i];
                Point edgeEnd = _vertices[(i + 1) % VertexCount];

                Vector edgeDir = edgeEnd - edgeStart; edgeDir.Unitise();

                double alpha = Point.DotProduct(point - edgeStart, edgeDir);

                if (alpha <= 0d)
                {
                    double d = edgeStart.DistanceTo(point);
                    if (d < t)
                    {
                        t = d;
                        closePoint = edgeStart;
                    }
                }
                else if (alpha >= edgeStart.DistanceTo(edgeEnd))
                {
                    double d = edgeEnd.DistanceTo(point);
                    if (d < t)
                    {
                        t = d;
                        closePoint = edgeEnd;
                    }
                }
                else
                {
                    Point scaledEdge = edgeStart + (alpha * edgeDir);
                    double d = point.DistanceTo(scaledEdge);
                    if (d < t)
                    {
                        t = d;
                        closePoint = edgeStart + edgeDir;
                    }
                }
            }

            if (EndPoint.DistanceTo(point) < t)
            {
                t = EndPoint.DistanceTo(point);
                closePoint = EndPoint;
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

        /// <inheritdoc/>
        public bool GeometricallyEquals(Polyline other)
        {
            return Equals(other);
        }

        #endregion

        #region Other Methods

        /// <summary>
        /// Evaluates the current <see cref="Polyline"/> at the given length parameter.
        /// </summary>
        /// <param name="t"> Length parameter to evaluate the curve. </param>
        /// <returns> The <see cref="Point"/> on the <see cref="Polyline"/> at the given length parameter. </returns>
        /// <exception cref="ArgumentException"> The arc length parameter can not be negative. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> The input length curve parameter is larger than the polyline length. </exception>
        private Point PointAt_LengthParameter(double t)
        {
            if (t < 0d) { throw new ArgumentException("The arc length parameter can not be negative."); }

            double cumulativeLength = 0.0;

            // If the parameter is in the main part.
            for (int i_Vertex = 0; i_Vertex < _vertices.Count - 1; i_Vertex++)
            {
                double segmentLength = _vertices[i_Vertex].DistanceTo(_vertices[i_Vertex + 1]);
                cumulativeLength += segmentLength;

                if (t <= cumulativeLength)
                {
                    Vector axis = _vertices[i_Vertex + 1] - _vertices[i_Vertex];
                    axis.Unitise();

                    double delta = t - (cumulativeLength - segmentLength);

                    return _vertices[i_Vertex] + (delta * axis);
                }
            }

            if (IsClosed)
            {
                double segmentLength = _vertices[_vertices.Count - 1].DistanceTo(_vertices[0]);
                cumulativeLength += segmentLength;

                if (t <= cumulativeLength)
                {
                    Vector axis = _vertices[0] - _vertices[_vertices.Count - 1];
                    axis.Unitise();

                    double delta = t - (cumulativeLength - segmentLength);

                    return _vertices[_vertices.Count - 1] + (delta * axis);
                }
            }

            // If the parameter is larger than the length of the polyline.
            throw new ArgumentOutOfRangeException("The arc length parameter is larger than the curve length.");

        }

        /// <summary>
        /// Evaluates the current <see cref="Polyline"/> at the given normalised parameter.
        /// </summary>
        /// <param name="t"> Normalised parameter. </param>
        /// <returns> The <see cref="Point"/> on the <see cref="Polyline"/> at the given normalised parameter. </returns>
        /// <exception cref="ArgumentOutOfRangeException"> The normalized parameter is outside the segment's domain. </exception>
        private Point PointAt_NormalisedParameter(double t)
        {
            // If the parameter exceeds the curve domain.
            if (t < DomainStart || DomainEnd < t) { throw new ArgumentOutOfRangeException("The normalized parameter is outside the segment's domain."); }

            // If the parameter is equal to the upper bound of the curve domain.
            if (t == DomainEnd) { return EndPoint; }

            // If the parameter is in the rest of the polyline.
            int index = Convert.ToInt16(Math.Truncate(t));
            double delta = t - index;

            Point vertex; Vector axis;
            if (IsClosed && index == _vertices.Count - 1)
            {
                vertex = _vertices[index];
                axis = _vertices[0] - _vertices[index];
            }
            else
            {
                vertex = _vertices[index];
                axis = _vertices[index + 1] - _vertices[index];
            }

            return vertex + (delta * axis);        
        }

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
