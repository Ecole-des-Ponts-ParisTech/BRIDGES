using System;

using Geo_Ker = BRIDGES.Geometry.Kernel;


namespace BRIDGES.Geometry.Euclidean3D
{
    /// <summary>
    /// Structure defining a segment curve in three-dimensional euclidean space.<br/>
    /// It is defined by a start point and an end point (finite length). 
    /// </summary>
    /// <remarks> For an infinite line, refer to <see cref="Line"/>. </remarks>
    public struct Segment : Geo_Ker.ICurve<Point>
    {
        #region Properties

        /// <inheritdoc/>
        public bool IsClosed 
        { 
            get { return false; }
        }


        /// <inheritdoc/>
        public Point StartPoint { get; set; }

        /// <inheritdoc/>
        public Point EndPoint { get; set; }


        /// <inheritdoc/>
        public double DomainStart 
        { 
            get { return 0.0; } 
        }

        /// <inheritdoc/>
        public double DomainEnd
        {
            get { return 1.0; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Segment"/> structure by defining its start and end <see cref="Point"/>.
        /// </summary>
        /// <param name="start"> Start <see cref="Point"/> of the <see cref="Segment"/>. </param>
        /// <param name="end"> End <see cref="Point"/> of the <see cref="Segment"/>. </param>
        public Segment(Point start, Point end)
        {
            StartPoint = start;
            EndPoint = end;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Segment"/> structure from another <see cref="Segment"/>.
        /// </summary>
        /// <param name="segment"> <see cref="Segment"/> to copy. </param>
        public Segment(Segment segment)
        {
            StartPoint = segment.StartPoint;
            EndPoint = segment.EndPoint;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Flips the current <see cref="Segment"/> by inverting the <see cref="StartPoint"/> and the <see cref="EndPoint"/>.
        /// </summary>
        public void Flip()
        {
            (EndPoint, StartPoint) = (StartPoint, EndPoint);
        }


        /// <summary>
        /// Computes the length of the current <see cref="Segment"/>.
        /// </summary>
        /// <returns> The value corresponding to the <see cref="Segment"/>'s length. </returns>
        public double Length()
        {
            return StartPoint.DistanceTo(EndPoint);
        }

        /// <summary>
        /// Evaluates the current <see cref="Segment"/> at the given parameter.
        /// </summary>
        /// <param name="parameter"> Value of the parameter. </param>
        /// <param name="format"> Format of the parameter. </param>
        /// <returns> The <see cref="Point"/> on the <see cref="Segment"/> at the given parameter.</returns>
        /// <exception cref="ArgumentOutOfRangeException"> The input curve parameter cannot be negative. </exception>
        public Point PointAt(double parameter, Geo_Ker.CurveParameterFormat format)
        {
            if (format == Geo_Ker.CurveParameterFormat.ArcLength) { return PointAt_ArcLengthParameter(parameter); }
            else if (format == Geo_Ker.CurveParameterFormat.Normalised) { return PointAt_NormalizedParameter(parameter); }

            else { throw new NotImplementedException(); }
        }


        /// <summary>
        /// Evaluates whether the current <see cref="Segment"/> is equal to another <see cref="Segment"/>.
        /// </summary>
        /// <remarks> 
        /// Two <see cref="Segment"/> are equal if their start and end <see cref="Point"/> are equal. 
        /// </remarks>
        /// <param name="other"> <see cref="Segment"/> to compare with. </param>
        /// <returns> <see langword="true"/> if the two <see cref="Segment"/> are equal, <see langword="false"/> otherwise. </returns>
        public bool Equals(Segment other)
        {
            return StartPoint.Equals(other.StartPoint) && EndPoint.Equals(other.EndPoint);
        }


        /********** Private Helpers **********/

        /// <summary>
        /// Evaluates the current <see cref="Segment"/> at the given length parameter.
        /// </summary>
        /// <param name="parameter"> Positive length parameter. </param>
        /// <returns> The <see cref="Point"/> on the <see cref="Segment"/> at the given length parameter. </returns>
        /// <exception cref="ArgumentOutOfRangeException"> The arc length parameter can not be negative. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> The arc length parameter is larger than the curve length. </exception>
        private Point PointAt_ArcLengthParameter(double parameter)
        {
            // If the parameter is larger than the curve length.
            if (parameter < 0.0) { throw new ArgumentOutOfRangeException("The arc length parameter can not be negative."); }
            if (Length() < parameter) { throw new ArgumentOutOfRangeException("The arc length parameter is larger than the curve length."); }

            // If the parameter is lower than the curve length (and positive).
            Vector axis = EndPoint - StartPoint;
            axis.Unitise();

            return StartPoint + (parameter * axis);
        }

        /// <summary>
        /// Evaluates the current <see cref="Segment"/> at the given normalised parameter.
        /// </summary>
        /// <param name="parameter"> Positive normalised parameter. </param>
        /// <returns> The <see cref="Point"/> on the <see cref="Segment"/> at the given normalised parameter. </returns>
        /// <exception cref="ArgumentOutOfRangeException"> The normalized parameter is outside the segment's domain. </exception>
        private Point PointAt_NormalizedParameter(double parameter)
        {
            // If the parameter exceeds the curve domain.
            if (parameter < DomainStart || DomainEnd < parameter) { throw new ArgumentOutOfRangeException("The normalized parameter is outside the segment's domain."); }

            Vector vector = EndPoint - StartPoint;
            
            return StartPoint + (parameter * vector);
        }

        #endregion


        #region Override : Object

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is Segment segment && Equals(segment);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"Segment from {StartPoint} to {EndPoint}.";
        }

        #endregion
    }
}
