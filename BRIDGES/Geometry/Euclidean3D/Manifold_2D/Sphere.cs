using System;


namespace BRIDGES.Geometry.Euclidean3D
{
    /// <summary>
    /// Class defining a sphere in three-dimensional euclidean space.
    /// </summary>
    public class Sphere
    {
        #region Properties

        /// <summary>
        /// Gets the origin of the current <see cref="Sphere"/>.
        /// </summary>
        public Point Centre { get; set; }

        /// <summary>
        /// Gets or sets the radius of the circle.
        /// </summary>
        public double Radius { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Sphere"/> class by defining its centre and radius.
        /// </summary>
        /// <param name="centre"> Centre of the <see cref="Circle"/>. </param>
        /// <param name="radius"> Radius of the <see cref="Circle"/>. </param>
        public Sphere(Point centre, double radius)
        {
            Centre = centre;
            Radius = radius;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Evaluates whether the current <see cref="Sphere"/> is equal to another <see cref="Sphere"/>.
        /// </summary>
        /// <remarks> 
        /// Two <see cref="Sphere"/> are equal if their centre and radius are equal.
        /// </remarks>
        /// <param name="other"> <see cref="Sphere"/> to compare with. </param>
        /// <returns> <see langword="true"/> if the two <see cref="Sphere"/> are equal, <see langword="false"/> otherwise. </returns>
        public bool Equals(Circle other)
        {
            return Centre.Equals(other.Centre) && (Radius - other.Radius < Settings.AbsolutePrecision);
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
