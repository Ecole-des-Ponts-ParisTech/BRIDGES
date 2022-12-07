using System;


namespace BRIDGES.Geometry.Euclidean3D
{
    /// <summary>
    /// Structure defining a half-line in three-dimensional euclidean space.<br/>
    /// It is defined by a start point and an end point (finite length). For an infinite line, refer to <see cref="Line"/>. 
    /// </summary>
    public struct Ray
    {
        #region Properties

        /// <summary>
        /// Gets the start <see cref="Point"/> of the current <see cref="Ray"/>.
        /// </summary>
        public Point StartPoint { get; set;  }

        /// <summary>
        /// Gets the axis of the current <see cref="Ray"/>.
        /// </summary>
        public Vector Axis { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Ray"/> structure by defining its origin and axis.
        /// </summary>
        /// <param name="start"> Start <see cref="Point"/> of the <see cref="Ray"/>. </param>
        /// <param name="axis"> Axis of the <see cref="Ray"/>. </param>
        public Ray(Point start, Vector axis)
        {
            StartPoint = start;
            Axis = axis;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Ray"/> structure from another <see cref="Ray"/>.
        /// </summary>
        /// <param name="ray"> <see cref="Ray"/> to copy. </param>
        public Ray(Ray ray)
        {
            StartPoint = ray.StartPoint;
            Axis = ray.Axis;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Flips the current <see cref="Ray"/> by flipping the <see cref="Axis"/>.
        /// </summary>
        public void Flip()
        {
            Axis = -Axis;
        }

        /// <summary>
        /// Evaluates the current <see cref="Ray"/> at the given length parameter.
        /// </summary>
        /// <param name="lengthParameter"> Value of the length parameter. </param>
        /// <returns> The <see cref="Point"/> on the <see cref="Line"/> at the given length parameter. </returns>
        /// <exception cref="ArgumentOutOfRangeException"> The input length parameter cannot be negative. </exception>
        public Point PointAt(double lengthParameter)
        {
            if (lengthParameter < 0) { throw new ArgumentOutOfRangeException("The input length parameter cannot be negative."); }

            Vector axis = Axis;
            axis.Unitise();

            return StartPoint + (lengthParameter * axis);
        }


        /// <summary>
        /// Evaluates whether the current <see cref="Ray"/> is equal to another <see cref="Ray"/>.
        /// </summary>
        /// <remarks> 
        /// Two <see cref="Ray"/> are equal if their start <see cref="Point"/> are equal and their <see cref="Vector"/> axis parallel. 
        /// </remarks>
        /// <param name="other"> <see cref="Ray"/> to compare with. </param>
        /// <returns> <see langword="true"/> if the two <see cref="Ray"/> are equal, <see langword="false"/> otherwise. </returns>
        public bool Equals(Ray other)
        {
            return Vector.AreParallel(Axis, other.Axis) && StartPoint.Equals(other.StartPoint);
        }

        #endregion


        #region Override : Object

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is Ray ray && Equals(ray);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"Ray starting at {StartPoint}, aligned with the direction {Axis}.";
        }

        #endregion
    }
}
