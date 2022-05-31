using System;


namespace BRIDGES.Geometry.Euclidean3D
{
    /// <summary>
    /// Class defining a plane in three-dimensional euclidean space. 
    /// The X and Y axes are the in-plane <see cref="Vector"/> and Z the normal of the <see cref="Plane"/>.
    /// </summary>
    public class Plane
    {
        #region Properties

        /// <summary>
        /// Gets the origin of the current <see cref="Plane"/>.
        /// </summary>
        public Point Origin { get; }


        /// <summary>
        /// Gets the X axis, the first in-plane axis of the current <see cref="Plane"/>.
        /// </summary>
        public Vector XAxis { get; }

        /// <summary>
        /// Gets the Y axis, the second in-plane of the current <see cref="Plane"/>.
        /// </summary>
        public Vector YAxis { get; }

        /// <summary>
        /// Gets the Z axis, the normal axis of the current <see cref="Plane"/>.
        /// </summary>
        public Vector Normal { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Plane"/> class by defining its origin,
        /// two linearly independent in-plane <see cref="Vector"/> and a normal <see cref="Vector"/> orthogonal to the previous ones.
        /// </summary>
        /// <param name="origin"> Origin <see cref="Point"/> of the <see cref="Plane"/>. </param>
        /// <param name="xAxis"> First <see cref="Vector"/> in-plane axis  of the <see cref="Plane"/>. </param>
        /// <param name="yAxis"> Second <see cref="Vector"/> in-plane axis of the <see cref="Plane"/>. </param>
        /// <param name="normal"> Normal <see cref="Vector"/> of the <see cref="Plane"/>. </param>
        /// <exception cref="ArgumentException"> The given axes cannot define a <see cref="Plane"/>. </exception>
        public Plane(Point origin, Vector xAxis, Vector yAxis, Vector normal)
        {
            // Verification : Linearly independent in-plane axes.
            if (Vector.AreParallel(yAxis, xAxis))
            {
                throw new ArgumentException("The given in-plane axes are not linearly independent.");
            }

            // Verification : Normal zAxis.
            if (!Vector.AreOrthogonal(normal, xAxis) || !Vector.AreOrthogonal(normal, yAxis))
            {
                throw new ArgumentException("The given normal axis is not orthogonal to the in-plane axes.");
            }

            // Initialisation of the Property
            Origin = origin;

            XAxis = xAxis;
            YAxis = yAxis;
            Normal = normal;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Plane"/> class by defining its origin and normal.
        /// </summary>
        /// <param name="origin"> Origin <see cref="Point"/> of the <see cref="Plane"/>. </param>
        /// <param name="normal"> Normal <see cref="Vector"/> of the <see cref="Plane"/>. </param>
        public Plane(Point origin, Vector normal)
        {
            // Initialisation of the Property
            Origin = origin;

            Normal = normal;

            if (!Vector.AreParallel(normal,Vector.WorldX))
            {
                Vector worldX = Vector.WorldX;
                XAxis = worldX - (normal * worldX) * normal; XAxis.Unitise();
                YAxis = Vector.CrossProduct(normal, XAxis); YAxis.Unitise();
            }
            else
            {
                Vector worldY = Vector.WorldY;
                XAxis = worldY - (normal * worldY) * normal; XAxis.Unitise();
                YAxis = Vector.CrossProduct(normal, XAxis); YAxis.Unitise();
            }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Plane"/> class from another <see cref="Plane"/>.
        /// </summary>
        /// <param name="plane"> <see cref="Plane"/> to copy. </param>
        public Plane(Plane plane)
        {
            // Initialisation of the Property
            Origin = plane.Origin;
            XAxis = plane.XAxis;
            YAxis = plane.YAxis;
            Normal = plane.Normal;
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Gets the <see cref="Plane"/> centered in (0.0, 0.0, 0.0) with WorldZ as its normal axis.
        /// </summary>
        public Plane WorldXY
        {
            get { return new Plane(Point.Zero, Vector.WorldX, Vector.WorldY, Vector.WorldZ); }
        }

        /// <summary>
        /// Gets the <see cref="Plane"/> centered in (0.0, 0.0, 0.0) with WorldX as its normal axis.
        /// </summary>
        public Plane WorldYZ
        {
            get { return new Plane(Point.Zero, Vector.WorldY, Vector.WorldZ, Vector.WorldX); }
        }

        /// <summary>
        /// Gets the <see cref="Plane"/> centered in (0.0, 0.0, 0.0) with WorldY as its normal axis.
        /// </summary>
        public Plane WorldZX
        {
            get { return new Plane(Point.Zero, Vector.WorldZ, Vector.WorldX, Vector.WorldY); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Computes the closest <see cref="Point"/> of the given <see cref="Point"/> on the current <see cref="Plane"/>.
        /// </summary>
        /// <param name="point"> <see cref="Point"/> to project on the current <see cref="Plane"/>. </param>
        /// <returns> The new <see cref="Point"/> on the <see cref="Plane"/>, cloesest to the initial one. </returns>
        public Point ClosestPoint(Point point)
        {
            double dZ = Vector.DotProduct(Origin - point, Normal);
            return point + (dZ * Normal);
        }


        /// <summary>
        /// Evaluates whether the current <see cref="Plane"/> is equal to another <see cref="Plane"/>.
        /// </summary>
        /// <remarks> 
        /// Two <see cref="Plane"/> are equal if their normal <see cref="Vector"/> are parallel, 
        /// and if the <see cref="Vector"/> connecting the two origin <see cref="Point"/> is zero or orthogonal to their parallel normal. 
        /// </remarks>
        /// <param name="other"> <see cref="Plane"/> to compare with. </param>
        /// <returns> <see langword="true"/> if the two <see cref="Plane"/> are equal, <see langword="false"/> otherwise. </returns>
        public bool Equals(Plane other)
        {
            return Vector.AreParallel(Normal, other.Normal) &&
                (Origin.Equals(other.Origin) || Vector.AreOrthogonal(Origin - other.Origin, Normal));
        }

        #endregion


        #region Override : Object

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is Plane plane && Equals(plane);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"A plane at {Origin}, of normal {Normal}.";
        }

        #endregion
    }
}
