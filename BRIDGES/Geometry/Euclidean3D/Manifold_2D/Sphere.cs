using System;

using Geo_Ker = BRIDGES.Geometry.Kernel;


namespace BRIDGES.Geometry.Euclidean3D
{
    /// <summary>
    /// Class defining a sphere in three-dimensional euclidean space.
    /// </summary>
    public class Sphere : IEquatable<Sphere>, Geo_Ker.IGeometricallyEquatable<Sphere>
    {
        #region Fields

        /// <summary>
        /// Orthogonal frame defining the centre and the orientation of the current <see cref="Sphere"/>.
        /// </summary>
        private Frame _frame;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the radius of the current <see cref="Sphere"/>.
        /// </summary>
        public double Radius { get; set; }

        /// <summary>
        /// Gets or sets the orthogonal frame defining the centre and the orientation of the current <see cref="Sphere"/>.
        /// </summary>
        public Frame Frame 
        { 
            get { return Frame; } 
            set
            {
                // Verifications
                if (!Frame.IsOrthogonal(value)) { throw new ArgumentException(nameof(value), "The frame of a sphere must be orthogonal"); }

                _frame = value;
            }
        }


        /// <summary>
        /// Gets or sets the centre of the current <see cref="Sphere"/>.
        /// </summary>
        public Point Centre
        {
            get { return _frame.Origin; }
            set { _frame.Origin = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Sphere"/> class by defining its centre and radius.
        /// </summary>
        /// <param name="centre"> Centre of the <see cref="Sphere"/>. </param>
        /// <param name="radius"> Radius of the <see cref="Sphere"/>. </param>
        public Sphere(Point centre, double radius)
        {
            Radius = radius;
            _frame = new Frame(centre, Vector.WorldX, Vector.WorldY, Vector.WorldZ);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Sphere"/> class by defining its centre and radius.
        /// </summary>
        /// <param name="frame"> Orthogonal frame defining the centre and orientation of the <see cref="Sphere"/>. </param>
        /// <param name="radius"> Radius of the <see cref="Sphere"/>. </param>
        public Sphere(Frame frame, double radius)
        {
            // Verifications
            if (!Frame.IsOrthogonal(frame)) { throw new ArgumentException(nameof(frame), "The frame of a sphere must be orthogonal"); }

            // Initialisation
            Radius = radius;
            _frame = frame;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Evaluates whether the current <see cref="Sphere"/> is memberwise equal to another <see cref="Sphere"/>.
        /// </summary>
        /// <remarks> 
        /// Two <see cref="Sphere"/> are equal if their centre and radius are equal.
        /// </remarks>
        /// <param name="other"> <see cref="Sphere"/> to compare with. </param>
        /// <returns> <see langword="true"/> if the two <see cref="Sphere"/> are equal, <see langword="false"/> otherwise. </returns>
        public bool Equals(Sphere other)
        {
            return (Radius - other.Radius < Settings.AbsolutePrecision) && Frame.Origin.Equals(other.Frame.Origin)
                && Vector.AreParallel(Frame.XAxis, other.Frame.XAxis) && Vector.AreParallel(Frame.YAxis, other.Frame.YAxis)
                && Vector.AreParallel(Frame.ZAxis, other.Frame.ZAxis);
        }

        /// <inheritdoc/>
        public bool GeometricallyEquals(Sphere other)
        {
            return (Radius - other.Radius < Settings.AbsolutePrecision) && Frame.Origin.Equals(other.Frame.Origin);
        }

        #endregion


        #region Override : Object

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is Sphere sphere && Equals(sphere);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"Sphere at {Centre}, of radius {Radius}.";
        }

        #endregion
    }
}
