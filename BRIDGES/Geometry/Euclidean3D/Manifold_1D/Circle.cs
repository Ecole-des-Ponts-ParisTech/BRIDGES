using System;

using Geo_Ker = BRIDGES.Geometry.Kernel;


namespace BRIDGES.Geometry.Euclidean3D
{
    /// <summary>
    /// Structure defining a circle curve in three-dimensional euclidean space.
    /// </summary>
    public struct Circle : IEquatable<Circle>,
        Geo_Ker.ICurve<Point>, Geo_Ker.IGeometricallyEquatable<Circle>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the radius of the current <see cref="Circle"/>.
        /// </summary>
        public double Radius { get; set; }

        /// <summary>
        /// Gets or sets the plane defining the centre and the orientation of the current <see cref="Circle"/> 
        /// </summary>
        public Plane Plane { get; set; }


        /// <summary>
        /// Gets or sets the centre of the current <see cref="Circle"/>.
        /// </summary>
        public Point Centre 
        { 
            get { return Plane.Origin; } 
            set { Plane.Origin = value; }
        }

        
        /// <summary>
        /// Gets a boolean evaluating whether the current <see cref="Circle"/> is closed or not;
        /// </summary>
        public bool IsClosed { get { return true; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Circle"/> structure, parallel to the WorldXY plane, by defining its centre and radius.
        /// </summary>
        /// <param name="centre"> Centre of the <see cref="Circle"/>. </param>
        /// <param name="radius"> Radius of the <see cref="Circle"/>. </param>
        public Circle(Point centre, double radius)
        {
            // Initialisation
            Plane = new Plane(centre, Vector.WorldX, Vector.WorldY, Vector.WorldZ);
            Radius = radius;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Circle"/> structure by defining its centre, orientation and radius.
        /// </summary>
        /// <param name="plane"> <see cref="Euclidean3D.Plane"/> defining the centre and the orientation of the <see cref="Circle"/>. </param>
        /// <param name="radius"> Radius of the <see cref="Circle"/>. </param>
        public Circle(Plane plane, double radius)
        {
            Plane = new Plane(plane);
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
            Plane = new Plane(Plane.Origin, Plane.UAxis, -Plane.VAxis, -Plane.Normal);
        }


        /// <summary>
        /// Evaluates the current <see cref="Circle"/> at the given angle.
        /// </summary>
        /// <param name="angle"> Value of the anlgle (in radians). </param>
        /// <returns> The <see cref="Point"/> on the <see cref="Circle"/> at the given angle. </returns>
        public Point PointAt(double angle)
        {
            Vector xAxis = Plane.UAxis; xAxis.Unitise();
            Vector yAxis = Vector.CrossProduct(Plane.Normal, Plane.UAxis); yAxis.Unitise();

            return Centre + ((Radius * Math.Cos(angle)) * xAxis) + ((Radius * Math.Sin(angle)) * yAxis);
        }

        /// <summary>
        /// Evaluates the current <see cref="Line"/> at the given parameter.
        /// </summary>
        /// <param name="t"> Parameter to evaluate the <see cref="Line"/>. </param>
        /// <param name="format"> Format of the parameter. 
        /// <list type="bullet">
        /// <item>
        /// <term>Normalised</term>
        /// <description> The point at <paramref name="t"/> = 0.5 is diametrically opposed to the start point at <paramref name="t"/> = 0.0. </description>
        /// </item>
        /// <item>
        /// <term>ArcLength</term>
        /// <description> The point at <paramref name="t"/> = <see cref="Math.PI"/> is diametrically opposed to the start point at <paramref name="t"/> = 0.0. </description>
        /// </item>
        /// </list> </param>
        /// <returns> The <see cref="Point"/> on the <see cref="Circle"/> at the given parameter. </returns>
        /// <exception cref="NotImplementedException"> The given format for the curve parameter is not implemented. </exception>
        public Point PointAt(double t, Geo_Ker.CurveParameterFormat format)
        {
            Vector xAxis = Plane.UAxis; xAxis.Unitise();
            Vector yAxis = Vector.CrossProduct(Plane.Normal, Plane.UAxis); yAxis.Unitise();

            if (format == Geo_Ker.CurveParameterFormat.ArcLength)
            {
                double angle = (t / Radius) % (2 * Math.PI);
                
                return Centre + ((Radius * Math.Cos(angle)) * xAxis) + ((Radius * Math.Sin(angle)) * yAxis);
            }
            else if (format == Geo_Ker.CurveParameterFormat.Normalised)
            {
                double angle = (2 * Math.PI * t) % (2 * Math.PI);
                return Centre + ((Radius * Math.Cos(angle)) * xAxis) + ((Radius * Math.Sin(angle)) * yAxis);
            }
            else { throw new NotImplementedException("The given format for the curve parameter is not implemented."); }
        }


        /// <summary>
        /// Evaluates whether the current <see cref="Circle"/> is memberwise equal to another <see cref="Circle"/>.
        /// </summary>
        /// <remarks> 
        /// Two <see cref="Circle"/> are equal if their centre, radius and plane are equal.
        /// </remarks>
        /// <param name="other"> <see cref="Circle"/> to compare with. </param>
        /// <returns> <see langword="true"/> if the two <see cref="Circle"/> are equal, <see langword="false"/> otherwise. </returns>
        public bool Equals(Circle other)
        {
            return Centre.Equals(other.Centre) && (Radius - other.Radius < Settings.AbsolutePrecision)
                && Vector.AreParallel(Plane.UAxis, other.Plane.UAxis) && Vector.AreParallel(Plane.VAxis, other.Plane.VAxis)
                && Vector.AreParallel(Plane.Normal, other.Plane.Normal);
        }

        /// <inheritdoc/>
        public bool GeometricallyEquals(Circle other)
        {
            return Centre.Equals(other.Centre) && (Radius - other.Radius < Settings.AbsolutePrecision)
                && Vector.AreParallel(Plane.Normal, other.Plane.Normal);
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
        Point Geo_Ker.ICurve<Point>.StartPoint 
        { 
            get { return Centre + ((Radius / Plane.UAxis.Length()) * Plane.UAxis); } 
        }

        /// <inheritdoc/>
        Point Geo_Ker.ICurve<Point>.EndPoint
        {
            get { return Centre + ((Radius / Plane.UAxis.Length()) * Plane.UAxis); }
        }

        /// <inheritdoc/>
        double Geo_Ker.ICurve<Point>.DomainStart
        {
            get { return 0.0; }
        }

        /// <inheritdoc/>
        double Geo_Ker.ICurve<Point>.DomainEnd
        {
            get { return 1.0; }
        }

        /******************** Method ********************/

        /// <inheritdoc/>
        double Geo_Ker.ICurve<Point>.Length()
        {
            return 2 * Math.PI * Radius;
        }

        #endregion
    }
}
