using System;


namespace BRIDGES.Geometry.Euclidean3D
{
    /// <summary>
    /// Structure defining a line in three-dimensional euclidean space.<br/>
    /// It is defined by an start point and an axis (infinite length).
    /// </summary>
    /// <remarks> For a finite line, refer to <see cref="Segment"/>. </remarks>
    public struct Line : IEquatable<Line>
    {
        #region Properties

        /// <summary>
        /// Gets the origin of the current <see cref="Line"/>.
        /// </summary>
        public Point Origin { get; }

        /// <summary>
        /// Gets the axis of the current <see cref="Line"/>.
        /// </summary>
        public Vector Axis { get; internal set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Line"/> structure by defining its origin and axis.
        /// </summary>
        /// <param name="origin"> Origin <see cref="Point"/> of the <see cref="Line"/>. </param>
        /// <param name="axis"> Axis of the <see cref="Line"/>. </param>
        public Line(Point origin, Vector axis)
        {
            Origin = origin;
            Axis = axis;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Line"/> structure from another <see cref="Line"/>.
        /// </summary>
        /// <param name="line"> <see cref="Line"/> to copy. </param>
        public Line(Line line)
        {
            Origin = line.Origin;
            Axis = line.Axis;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Flips the current <see cref="Line"/> by flipping the <see cref="Axis"/>.
        /// </summary>
        public void Flip()
        {
            Axis = -Axis;
        }

        /// <summary>
        /// Evaluates the current <see cref="Line"/> at the given length parameter.
        /// </summary>
        /// <param name="lengthParameter"> Value of the length parameter. </param>
        /// <returns> The <see cref="Point"/> on the <see cref="Line"/> at the given length parameter. </returns>
        public Point PointAt(double lengthParameter)
        {
            Vector axis = Axis;
            axis.Unitise();

            return Origin + (lengthParameter * axis);
        }


        /// <summary>
        /// Evaluates whether the current <see cref="Line"/> is equal to another <see cref="Line"/>.
        /// </summary>
        /// <remarks> 
        /// Two <see cref="Line"/> are equal if their <see cref="Vector"/> axis are parallel, 
        /// and if the <see cref="Vector"/> connecting the two origin <see cref="Point"/> is zero or parallel to their parallel axis. 
        /// </remarks>
        /// <param name="other"> <see cref="Line"/> to compare with. </param>
        /// <returns> <see langword="true"/> if the two <see cref="Line"/> are equal, <see langword="false"/> otherwise. </returns>
        public bool Equals(Line other)
        {
            return Vector.AreParallel(Axis, other.Axis)
                && (Origin.Equals(other.Origin) || Vector.AreParallel(Origin - other.Origin, Axis));
        }

        #endregion


        #region Override : Object

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is Line line && Equals(line);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"Line starting at {Origin}, aligned with the direction {Axis}.";
        }

        #endregion
    }
}
