﻿using System;
using System.Collections.Generic;

using Geo_Ker = BRIDGES.Geometry.Kernel;


namespace BRIDGES.Geometry.Euclidean3D
{
    /// <summary>
    /// Class defining a polyline curve in three-dimensional euclidean space.<br/>
    /// It is defined by a succession of vertices. 
    /// </summary>
    public class Polyline : Geo_Ker.ICurve<Point>
    {
        #region Fields

        /// <summary>
        /// Vertices of the current <see cref="Polyline"/>.
        /// </summary>
        /// <remarks> 
        /// If the polyline is closed, the first vertex is not duplicated at the end of the list of the vertices.<br/> 
        /// The main part of the polyline designates the consecutive segments generated by the non looping list of vertices.<br/> 
        /// Then, for a closed polyline, the closing segment is the line computed from the last to the first vertex of the list. 
        /// </remarks>
        private List<Point> _vertices;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a boolean evaluating whether the current <see cref="Polyline"/> is closed or not;
        /// </summary>
        public bool IsClosed { get; set; }


        /// <summary>
        /// Gets the start <see cref="Point"/> of the current <see cref="Polyline"/>.
        /// </summary>
        public Point StartPoint
        {
            get { return _vertices[0]; }
        }

        /// <summary>
        /// Gets the end <see cref="Point"/> of the current <see cref="Polyline"/>.
        /// </summary>
        public Point EndPoint
        {
            get { return IsClosed ? _vertices[0] : _vertices[_vertices.Count - 1]; }
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
        /// Initialises a new instance of the <see cref="Polyline"/> class.
        /// </summary>
        /// <param name="isClosed"> Determines whether the new <see cref="Polyline"/> is closed or not. </param>
        public Polyline(bool isClosed = false)
        {
            IsClosed = isClosed;
            _vertices = new List<Point>();
        }

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
        /// Flips the current <see cref="Polyline"/> by reversing the list of vertices.
        /// </summary>
        public void Flip()
        {
            if (IsClosed) { _vertices.Reverse(1, _vertices.Count - 1); }
            else { _vertices.Reverse(); }
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
                // Calculates the length of the closing segment (if necessary).
                if (IsClosed) { length += (_vertices[0] - _vertices[_vertices.Count - 1]).Norm(); }

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
            if (parameter < 0) { throw new ArgumentOutOfRangeException("The input curve parameter cannot be negative."); }

            else if (format == Geo_Ker.CurveParameterFormat.Length) { return PointAt_LengthParameter(parameter); }
            else if (format == Geo_Ker.CurveParameterFormat.Normalised) { return PointAt_NormalizedParameter(parameter); }

            else { throw new NotImplementedException(); }
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

        #endregion

        #region Private Methods

        /********** For PointAt(Double,CurveParameterFormat) **********/

        /// <summary>
        /// Evaluates the current <see cref="Polyline"/> at the given length parameter.
        /// </summary>
        /// <param name="parameter"> Positive length parameter. </param>
        /// <returns> The <see cref="Point"/> on the <see cref="Polyline"/> at the given length parameter. </returns>
        /// <exception cref="ArgumentOutOfRangeException"> The input length curve parameter is larger than the polyline length. </exception>
        private Point PointAt_LengthParameter(double parameter)
        {
            double cumulativeLength = 0.0;

            // If the parameter is in the main part.
            for (int i_Vertex = 0; i_Vertex < _vertices.Count - 1; i_Vertex++)
            {
                Vector axis = _vertices[i_Vertex + 1] - _vertices[i_Vertex];
                cumulativeLength += axis.Length();

                if (parameter < cumulativeLength)
                {
                    double delta = parameter % 1.0;

                    axis.Unitise();

                    return _vertices[i_Vertex] + (delta * axis);
                }
            }
            // If the parameter is in the closing segment.
            if (IsClosed)
            {
                Vector axis = _vertices[0] - _vertices[_vertices.Count - 1];
                cumulativeLength += axis.Length();

                if (parameter < cumulativeLength)
                {
                    double delta = parameter % 1.0;

                    axis.Unitise();

                    return _vertices[_vertices.Count - 1] + (delta * axis);
                }
            }

            // If the parameter is equal to the length of the polyline.
            if (parameter == cumulativeLength) 
            { 
                return IsClosed ? _vertices[0] : _vertices[_vertices.Count - 1]; 
            }

            // If the parameter is larger than the length of the polyline.
            else
            { 
                throw new ArgumentOutOfRangeException("The input length curve parameter is larger than the polyline length."); 
            }

        }

        /// <summary>
        /// Evaluates the current <see cref="Polyline"/> at the given normalised parameter.
        /// </summary>
        /// <param name="parameter"> Positive normalised parameter. </param>
        /// <returns> The <see cref="Point"/> on the <see cref="Polyline"/> at the given normalised parameter. </returns>
        /// <exception cref="ArgumentOutOfRangeException"> The input normalised curve parameter is larger than the upper bound of the polyline domain. </exception>
        private Point PointAt_NormalizedParameter(double parameter)
        {
            double domainEnd = IsClosed ? _vertices.Count : _vertices.Count - 1;

            // If the parameter exceeds the curve domain.
            if (parameter > domainEnd)
            { 
                throw new ArgumentOutOfRangeException("The input normalised curve parameter is larger than the upper bound of the polyline domain."); 
            }

            // If the parameter is equal to the upper bound of the curve domain.
            if (parameter == domainEnd) { return IsClosed ? _vertices[0] : _vertices[_vertices.Count - 1]; }

            // If the curve is closed and the parameter is in the closing segment of the polyline.
            if (IsClosed && (parameter > domainEnd - 1))
            {
                double delta = parameter % 1.0;

                Point vertex = _vertices[_vertices.Count - 1];
                Vector axis = _vertices[0] - _vertices[_vertices.Count - 1];
                axis.Unitise();

                return vertex + (delta * axis);
            }

            // If the parameter is in the main part of the polyline.
            {
                double delta = parameter % 1.0;
                int index = (int)(parameter - delta);

                Point vertex = _vertices[index];
                Vector axis = _vertices[index + 1] - _vertices[index];
                axis.Unitise();

                return vertex + (delta * axis);
            }
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
