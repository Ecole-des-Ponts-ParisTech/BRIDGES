using System;
using BRIDGES.Geometry.Kernel;
using Geo_Ker = BRIDGES.Geometry.Kernel;


namespace BRIDGES.Geometry.Euclidean3D
{
    /// <summary>
    /// Structure defining a circle curve in three-dimensional euclidean space.
    /// </summary>
    public struct Circle : Geo_Ker.ICurve<Point>
    {
        #region Fields

        /// <summary>
        /// Centre and orientation of the current <see cref="Circle"/>.
        /// </summary>
        private Plane _plane;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a boolean evaluating whether the current <see cref="Circle"/> is closed or not;
        /// </summary>
        public bool IsClosed { get { return true; } }


        /// <summary>
        /// Gets the centre of the current <see cref="Circle"/>.
        /// </summary>
        public Point Centre { get { return _plane.Origin; } }

        /// <summary>
        /// Gets or sets the radius of the circle.
        /// </summary>
        public double Radius { get; set; }

        /// <summary>
        /// Gets the normal <see cref="Vector"/> of the current <see cref="Circle"/>.
        /// </summary>
        public Vector Normal { get { return _plane.Normal; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Circle"/> structure, parallel to the WorldXY plane, by defining its centre and radius.
        /// </summary>
        /// <param name="centre"> Centre of the <see cref="Circle"/>. </param>
        /// <param name="radius"> Radius of the <see cref="Circle"/>. </param>
        public Circle(Point centre, double radius)
        {
            _plane = new Plane(centre, Vector.WorldX, Vector.WorldY, Vector.WorldZ);
            Radius = radius;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Circle"/> structure by defining its centre, orientation and radius.
        /// </summary>
        /// <param name="plane"> <see cref="Plane"/> for the centre and the orientation of the <see cref="Circle"/>. </param>
        /// <param name="radius"> Radius of the <see cref="Circle"/>. </param>
        public Circle(Plane plane, double radius)
        {
            _plane = new Plane(plane);
            Radius = radius;
        }
        

        #endregion

/*
        /// <summary>
        /// Initialises a new instance of the <see cref="Circle"/> structure by defining three <see cref="Point"/>.
        /// </summary>
        /// <param name="pointA"> First <see cref="Point"/> on the <see cref="Circle"/>. </param>
        /// <param name="pointB"> Second <see cref="Point"/> on the <see cref="Circle"/>. </param>
        /// <param name="pointC"> Third <see cref="Point"/> on the <see cref="Circle"/>. </param>
        /// <exception cref="ArgumentException"> The three points should not be aligned. </exception>
        public Circle(Point pointA, Point pointB, Point pointC)
        {
            // Verification : Non-colinearity
            if (Vector.AreParallel(pointB - pointA, pointB - pointC))
            {
                throw new ArgumentException("The three points should not be aligned.");
            }

            // See http://paulbourke.net/geometry/circlesphere/ for a comprehensive implementation in 2D.
            // Or ENPC framework for an implementation in 3D.
            throw new NotImplementedException("Waiting for the implementation of the intersection between Lines.");

        }
*/

        #region Methods

        /// <summary>
        /// Flips the current <see cref="Circle"/> by reversing the normal axis (and the in-plane YAxis).
        /// </summary>
        public void Flip()
        {
            _plane = new Plane(_plane.Origin, _plane.XAxis, -_plane.YAxis, -_plane.Normal);
        }


        /// <summary>
        /// Evaluates the current <see cref="Circle"/> at the given angle.
        /// </summary>
        /// <param name="angle"> Value of the anlgle (in radians). </param>
        /// <returns> The <see cref="Point"/> on the <see cref="Circle"/> at the given angle. </returns>
        public Point PointAt(double angle)
        {
            return Centre + ((Radius * Math.Cos(angle)) * _plane.XAxis) + ((Radius * Math.Sin(angle)) * Vector.CrossProduct(_plane.Normal, _plane.XAxis));
        }

        /// <summary>
        /// Evaluates the current <see cref="Circle"/> at the given parameter.
        /// </summary>
        /// <param name="parameter"> Value of the parameter. </param>
        /// <param name="format"> Format of the parameter. </param>
        /// <returns> The <see cref="Point"/> on the <see cref="Circle"/> at the given parameter. </returns>
        /// <exception cref="NotImplementedException"> The given format for the curve parameter is not implemented for the circle. </exception>
        public Point PointAt(double parameter, CurveParameterFormat format)
        {
            if (format == CurveParameterFormat.ArcLength)
            {
                double angle = (parameter / Radius) % (2 * Math.PI);
                return Centre + ((Radius * Math.Cos(angle)) * _plane.XAxis) + ((Radius * Math.Sin(angle)) * Vector.CrossProduct(_plane.Normal, _plane.XAxis));
            }
            else if (format == CurveParameterFormat.Normalised)
            {
                double angle = (2 * Math.PI * parameter) % (2 * Math.PI);
                return Centre + ((Radius * Math.Cos(angle)) * _plane.XAxis) + ((Radius * Math.Sin(angle)) * Vector.CrossProduct(_plane.Normal, _plane.XAxis));
            }
            else { throw new NotImplementedException("The given format for the curve parameter is not implemented for the circle."); }
        }


        /// <summary>
        /// Evaluates whether the current <see cref="Circle"/> is equal to another <see cref="Circle"/>.
        /// </summary>
        /// <remarks> 
        /// Two <see cref="Circle"/> are equal if their centre, plane and radius are equal.
        /// </remarks>
        /// <param name="other"> <see cref="Circle"/> to compare with. </param>
        /// <returns> <see langword="true"/> if the two <see cref="Circle"/> are equal, <see langword="false"/> otherwise. </returns>
        public bool Equals(Circle other)
        {
            return Centre.Equals(other.Centre) && Vector.AreParallel(Normal, other.Normal) 
                && (Radius - other.Radius < Settings.AbsolutePrecision);
        }

        #endregion
        

        #region Override : Object

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is Circle circle && Equals(circle);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"Circle at {Centre}, of radius {Radius}.";
        }

        #endregion


        #region Explicit : ICurve<Point>

        /******************** Properties ********************/

        /// <inheritdoc/>
        Point ICurve<Point>.StartPoint 
        { 
            get { return Centre + ((Radius / _plane.XAxis.Length()) * _plane.XAxis); } 
        }

        /// <inheritdoc/>
        Point ICurve<Point>.EndPoint
        {
            get { return Centre + ((Radius / _plane.XAxis.Length()) * _plane.XAxis); }
        }

        /// <inheritdoc/>
        double ICurve<Point>.DomainStart
        {
            get { return 0.0; }
        }

        /// <inheritdoc/>
        double ICurve<Point>.DomainEnd
        {
            get { return 2.0 * Math.PI; }
        }

        /******************** Method ********************/

        /// <inheritdoc/>
        double ICurve<Point>.Length()
        {
            return 2 * Math.PI * Radius;
        }

        #endregion
    }
}
