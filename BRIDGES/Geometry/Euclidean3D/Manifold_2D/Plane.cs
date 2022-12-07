using System;

using Geo_Ker = BRIDGES.Geometry.Kernel;


namespace BRIDGES.Geometry.Euclidean3D
{
    /// <summary>
    /// Class defining a plane in three-dimensional euclidean space.
    /// </summary>
    public sealed class Plane : IEquatable<Plane>, Geo_Ker.IGeometricallyEquatable<Plane>
    {
        #region Fields

        /// <summary>
        /// First in-plane axis of the current <see cref="Plane"/>.
        /// </summary>
        private Vector _uAxis;

        /// <summary>
        /// Second in-plane axis of the current <see cref="Plane"/>.
        /// </summary>
        private Vector _vAxis;

        /// <summary>
        /// Normal axis of the current <see cref="Plane"/>.
        /// </summary>
        private Vector _normal;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the origin of the current <see cref="Plane"/>.
        /// </summary>
        public Point Origin { get; set; }


        /// <summary>
        /// Gets or sets the u-axis, the first in-plane axis of the current <see cref="Plane"/>.
        /// </summary>
        /// <remarks> Checks and eventual updates are performed to ensure that the in-plane axis are not parallel and that the normal vector is orthogonal to them. </remarks>
        /// <exception cref="ArgumentException"> The u-axis vector cannot have a length of zero. </exception>
        public Vector UAxis 
        { 
            get { return _uAxis; }
            set
            {
                // Verifications
                if (value.Length() == 0d) { throw new ArgumentException("The u-axis vector cannot have a length of zero."); }

                _uAxis = value;
                if (Vector.AreParallel(_uAxis, _vAxis)) { _vAxis = Vector.CrossProduct(_normal, _uAxis); _vAxis.Unitise(); }
                else if (!Vector.AreOrthogonal(_uAxis, _normal)) { _normal = Vector.CrossProduct(_uAxis, _vAxis); _normal.Unitise(); }
            }
        }

        /// <summary>
        /// Gets or sets the v-axis, the second in-plane axis of the current <see cref="Plane"/>.
        /// </summary>
        /// <remarks> Checks and eventual updates are performed to ensure that the in-plane axis are not parallel and that the normal vector is orthogonal to them. </remarks>
        /// <exception cref="ArgumentException"> The v-axis vector cannot have a length of zero. </exception>
        public Vector VAxis 
        {
            get { return _vAxis; }
            set
            {
                // Verifications
                if (value.Length() == 0d) { throw new ArgumentException("The v-axis vector cannot have a length of zero."); }

                _vAxis = value;
                if (Vector.AreParallel(_uAxis, _vAxis)) { _uAxis = Vector.CrossProduct(_vAxis, _normal); _uAxis.Unitise(); }
                else if (!Vector.AreOrthogonal(_vAxis, _normal)) { _normal = Vector.CrossProduct(_uAxis, _vAxis); _normal.Unitise(); }
            }
        }

        /// <summary>
        /// Gets or sets the normal axis of the current <see cref="Plane"/>.
        /// </summary>
        /// <remarks> Checks and eventual updates are performed to ensure that the in-plane axis are not parallel and that the normal vector is orthogonal to them. </remarks>
        /// <exception cref="ArgumentException"> The normal vector cannot have a length of zero. </exception>
        public Vector Normal 
        {
            get { return _normal; }
            set
            {
                // Verifications
                if (value.Length() == 0d) { throw new ArgumentException("The normal vector cannot have a length of zero."); }

                _normal = value;
                if (Vector.AreParallel(_uAxis, _normal)) { _uAxis = Vector.CrossProduct(_vAxis, _normal); _uAxis.Unitise(); }
                else if (Vector.AreParallel(_vAxis, _normal)) { _vAxis = Vector.CrossProduct(_normal, _uAxis); _vAxis.Unitise(); }
                
                if (!Vector.AreOrthogonal(_uAxis, _normal) || !Vector.AreOrthogonal(_vAxis, _normal))
                {
                    _uAxis = _uAxis - (_normal * _uAxis) * _normal; ; _uAxis.Unitise();
                    _vAxis = _vAxis - (_normal * _vAxis) * _normal; _vAxis.Unitise();
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Plane"/> class.
        /// </summary>
        public Plane()
        {
            // Initialisation
            Origin = Point.Zero;

            _uAxis = Vector.WorldX;
            _vAxis = Vector.WorldY; ;
            _normal = Vector.WorldZ; ;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Plane"/> class by defining its origin and normal vector.
        /// </summary>
        /// <param name="origin"> Origin <see cref="Point"/> of the <see cref="Plane"/>. </param>
        /// <param name="normal"> Normal <see cref="Vector"/> of the <see cref="Plane"/>. </param>
        /// <exception cref="ArgumentException"> The normal vector cannot have a length of zero. </exception>
        public Plane(Point origin, Vector normal)
        {
            // Verifications
            if (normal.Length() == 0d) { throw new ArgumentException("The normal vector cannot have a length of zero."); }

            // Initialisation
            Origin = origin;

            _normal = normal;

            if (!Vector.AreParallel(normal, Vector.WorldX))
            {
                Vector worldX = Vector.WorldX;
                _uAxis = worldX - (normal * worldX) * normal; UAxis.Unitise();
                _vAxis = Vector.CrossProduct(normal, UAxis); VAxis.Unitise();
            }
            else
            {
                Vector worldY = Vector.WorldY;
                _uAxis = worldY - (normal * worldY) * normal; UAxis.Unitise();
                _vAxis = Vector.CrossProduct(normal, UAxis); VAxis.Unitise();
            }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Plane"/> class by defining its origin and two linearly independent in-plane <see cref="Vector"/>.
        /// </summary>
        /// <param name="origin"> Origin <see cref="Point"/> of the <see cref="Plane"/>. </param>
        /// <param name="uAxis"> First <see cref="Vector"/> in-plane axis of the <see cref="Plane"/>. </param>
        /// <param name="vAxis"> Second <see cref="Vector"/> in-plane axis of the <see cref="Plane"/>. </param>
        /// <exception cref="ArgumentException"> 
        /// <para> The axes cannot have a length of zero. </para>
        /// <para> - or - </para>
        /// <para> The in-plane axes are not linearly independent. </para>
        /// </exception>
        public Plane(Point origin, Vector uAxis, Vector vAxis)
        {
            // Verifications
            if (uAxis.Length() == 0d || vAxis.Length() == 0d) { throw new ArgumentException("The in-plane axes cannot have a length of zero."); }
            if (Vector.AreParallel(uAxis, vAxis)) { throw new ArgumentException("The in-plane axes are not linearly independent."); }

            // Initialisation
            Origin = origin;

            _uAxis = uAxis;
            _vAxis = vAxis;
            _normal = Vector.CrossProduct(_uAxis, _vAxis); _normal.Unitise();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Plane"/> class by defining its origin,
        /// two linearly independent in-plane <see cref="Vector"/> and a normal <see cref="Vector"/> orthogonal to the previous ones.
        /// </summary>
        /// <param name="origin"> Origin <see cref="Point"/> of the <see cref="Plane"/>. </param>
        /// <param name="uAxis"> First <see cref="Vector"/> in-plane axis of the <see cref="Plane"/>. </param>
        /// <param name="vAxis"> Second <see cref="Vector"/> in-plane axis of the <see cref="Plane"/>. </param>
        /// <param name="normal"> Normal <see cref="Vector"/> of the <see cref="Plane"/>. </param>
        /// <exception cref="ArgumentException"> 
        /// <para> The axes cannot have a length of zero. </para>
        /// <para> - or - </para>
        /// <para> The in-plane axes are not linearly independent. </para>
        /// <para> - or - </para>
        /// <para> The normal axis is not orthogonal to the in-plane axes. </para>
        /// </exception>
        public Plane(Point origin, Vector uAxis, Vector vAxis, Vector normal)
        {
            // Verifications
            if (uAxis.Length() == 0d || vAxis.Length() == 0d || normal.Length() == 0d) 
            { 
                throw new ArgumentException("The axes cannot have a length of zero."); 
            }
            else if (Vector.AreParallel(vAxis, uAxis)) 
            { 
                throw new ArgumentException("The in-plane axes are not linearly independent."); 
            }
            else if (!Vector.AreOrthogonal(uAxis, normal) || !Vector.AreOrthogonal(vAxis, normal))
            {
                throw new ArgumentException("The normal axis is not orthogonal to the in-plane axes.");
            }

            // Initialisation
            Origin = origin;

            _uAxis = uAxis;
            _vAxis = vAxis;
            _normal = normal;
        }


        /// <summary>
        /// Initialises a new instance of the <see cref="Plane"/> class from another <see cref="Plane"/>.
        /// </summary>
        /// <param name="plane"> <see cref="Plane"/> to copy. </param>
        public Plane(Plane plane)
        {
            // Initialisation
            Origin = plane.Origin;

            _uAxis = plane.UAxis;
            _vAxis = plane.VAxis;
            _normal = plane.Normal;
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Gets the <see cref="Plane"/> centered in (0.0, 0.0, 0.0) with WorldZ as its normal axis.
        /// </summary>
        public static Plane WorldXY
        {
            get { return new Plane(Point.Zero, Vector.WorldX, Vector.WorldY, Vector.WorldZ); }
        }

        /// <summary>
        /// Gets the <see cref="Plane"/> centered in (0.0, 0.0, 0.0) with WorldX as its normal axis.
        /// </summary>
        public static Plane WorldYZ
        {
            get { return new Plane(Point.Zero, Vector.WorldY, Vector.WorldZ, Vector.WorldX); }
        }

        /// <summary>
        /// Gets the <see cref="Plane"/> centered in (0.0, 0.0, 0.0) with WorldY as its normal axis.
        /// </summary>
        public static Plane WorldZX
        {
            get { return new Plane(Point.Zero, Vector.WorldZ, Vector.WorldX, Vector.WorldY); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Computes the orthogonal projection of the given <see cref="Point"/> on the current <see cref="Plane"/>.
        /// </summary>
        /// <param name="point"> <see cref="Point"/> to project. </param>
        /// <returns> The closest <see cref="Point"/> on the <see cref="Plane"/>. </returns>
        public Point ClosestPoint(Point point)
        {
            double dZ = Vector.DotProduct(Origin - point, Normal);
            return point + (dZ * Normal);
        }


        /// <summary>
        /// Evaluates whether the current <see cref="Plane"/> is memberwise equal to another <see cref="Plane"/>.
        /// </summary>
        /// <param name="other"> <see cref="Plane"/> to compare with. </param>
        /// <returns> <see langword="true"/> if the two <see cref="Plane"/> are equal, <see langword="false"/> otherwise. </returns>
        public bool Equals(Plane other)
        {
            return Origin.Equals(other.Origin) && UAxis.Equals(other.UAxis) && VAxis.Equals(other.VAxis) && Normal.Equals(other.Normal);
        }

        /// <inheritdoc/>
        public bool GeometricallyEquals(Plane other)
        {
            return (Origin.Equals(other.Origin) || Vector.AreOrthogonal(Origin - other.Origin, Normal))
                && Vector.AreParallel(Normal, other.Normal);
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
            return $"Plane at {Origin}, of normal {Normal}.";
        }

        #endregion
    }
}
