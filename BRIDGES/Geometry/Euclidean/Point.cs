using System;


namespace BRIDGES.Geometry.Euclidean
{
    /// <summary>
    /// Structure defining a point in three-dimensional Euclidean space.
    /// </summary>
    public struct Point
    {
        #region Properties

        /// <summary>
        /// Gets or sets the first component of the <see cref="Point"/>.
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// Gets or sets the second component of the <see cref="Point"/>.
        /// </summary>
        public double Y { get; set; }
        /// <summary>
        /// Gets or sets the third component of the <see cref="Point"/>.
        /// </summary>
        public double Z { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Point"/> structure by defining its three coordinate values.
        /// </summary>
        /// <param name="x"> Value of the first component. </param>
        /// <param name="y"> Value of the second component. </param>
        /// <param name="z"> Value of the third component. </param>
        public Point(double x, double y, double z)
        {
            // Initialises properties
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Point"/> structure by copying the coordinates from another <see cref="Point"/>.
        /// </summary>
        /// <param name="point"> Point whose coordinates to copy. </param>
        public Point(Point point)
        {
            // Initialises properties
            X = point.X;
            Y = point.Y;
            Z = point.Z;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Point"/> structure by defining its coordinates from an array.
        /// </summary>
        /// <param name="coordinates"> Values of the coordinates. </param>
        /// <exception cref="ArgumentOutOfRangeException"> The length of the array is different from the number of coordinates of a three-dimensional euclidean point. </exception>
        private Point(double[] coordinates)
        {
            // Verifications
            if (coordinates.Length != 3) { throw new ArgumentOutOfRangeException("coordinates", "The length of the array is different from the number of coordinates of a three-dimensional euclidean point."); }

            X = coordinates[0];
            Y = coordinates[1];
            Z = coordinates[2];
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Computes the componentwise addition of two <see cref="Point"/>.
        /// </summary>
        /// <param name="pointA"> <see cref="Point"/> for the addition. </param>
        /// <param name="pointB"> <see cref="Point"/> for the addition. </param>
        /// <returns> The new <see cref="Point"/> resulting from the addition of the two <see cref="Point"/>. </returns>
        public static Point Add(Point pointA, Point pointB) => new Point(pointA.X + pointB.X,
            pointA.Y + pointB.Y, pointA.Z + pointB.Z);

        /// <summary>
        /// Computes the componentwise subtraction of two <see cref="Point"/>.
        /// </summary>
        /// <param name="pointA"> <see cref="Point"/> for the subtraction. </param>
        /// <param name="pointB"> <see cref="Point"/> for the subtraction. </param>
        /// <returns> The new <see cref="Point"/> resulting from the subtraction of the two <see cref="Point"/>. </returns>
        public static Point Subtract(Point pointA, Point pointB) => new Point(pointA.X - pointB.X,
            pointA.Y - pointB.Y, pointA.Z - pointB.Z);

        /// <summary>
        /// Computes the componentwise multiplication of a <see cref="Point"/> with a <see cref="double"/> value.
        /// </summary>
        /// <param name="factor"> Value to multiply with. </param>
        /// <param name="point"> <see cref="Point"/> to multiply. </param>
        /// <returns> The new <see cref="Point"/> resulting from the multiplication of the <see cref="Point"/> with the <see cref="double"/> value. </returns>
        public static Point Multiply(double factor, Point point) => new Point(point.X * factor,
            point.Y * factor, point.Z * factor);

        /// <summary>
        /// Computes the componentwise division of a <see cref="Point"/> by a <see cref="double"/> value.
        /// </summary>
        /// <param name="point"> <see cref="Point"/> to divide. </param>
        /// <param name="divisor"> Value to divide with. </param>
        /// <returns> The new <see cref="Point"/> resulting from the division of the <see cref="Point"/> with the <see cref="double"/> value. </returns>
        public static Point Divide(Point point, double divisor) => new Point(point.X / divisor,
            point.Y / divisor, point.Z / divisor);


        /// <summary>
        /// Computes the dot product of two <see cref="Point"/>.
        /// </summary>
        /// <param name="pointA"> <see cref="Point"/> for the dot product. </param>
        /// <param name="pointB"> <see cref="Point"/> for the dot product. </param>
        /// <returns> The value resulting from the dot product of the two <see cref="Point"/>. </returns>
        public static double DotProduct(Point pointA, Point pointB) => pointA.X * pointB.X
            + pointA.Y * pointB.Y + pointA.Z * pointB.Z;

        /// <summary>
        /// Computes the cross product of two <see cref="Point"/>.
        /// </summary>
        /// <param name="pointA"> Left <see cref="Point"/> for the cross product. </param>
        /// <param name="pointB"> Right <see cref="Point"/> for the cross product. </param>
        /// <returns> The new <see cref="Point"/> resulting from the cross product of the two <see cref="Point"/>. </returns>
        public static Point CrossProduct(Point pointA, Point pointB) =>
            new Point((pointA.Y * pointB.Z) - (pointA.Z * pointB.Y),
            (pointA.Z * pointB.X) - (pointA.X * pointB.Z),
            (pointA.X * pointB.Y) - (pointA.Y * pointB.X));


        /// <summary>
        /// Computes an inversion of the current point in a sphere with a certain ratio.
        /// </summary>
        /// <param name="point"> <see cref="Point"/> to operate from. </param>
        /// <param name="center"> Center of inversion. </param>
        /// <param name="ratio"> Ratio of inversion. </param>
        /// <returns> The new <see cref="Point"/> resulting from the inversion of the given <see cref="Point"/>. </returns>
        public static Point MobiusInversion(Point point, Point center, double ratio)
        {
            if (center.Equals(point))
            {
                throw new ArgumentException("The center of inversion should be different from the point to transform.");
            }
            else
            {
                Point diff = point - center;
                double dividende = DotProduct(diff, diff);
                Point newPoint = diff * (ratio / dividende);

                return newPoint;
            }
        }

        #endregion

        #region Operators

        /// <inheritdoc cref="Add(Point, Point)"/>
        public static Point operator +(Point pointA, Point pointB) => Add(pointA, pointB);

        /// <inheritdoc cref="Subtract(Point, Point)"/>
        public static Point operator -(Point pointA, Point pointB) => Subtract(pointA, pointB);

        /// <summary>
        /// Computes the opposite of the <see cref="Point"/>.
        /// </summary>
        /// <param name="pointA"> <see cref="Point"/> to operate from. </param>
        /// <returns> The new <see cref="Point"/> whose coordinates are the opposite of the given <see cref="Point"/>. </returns>
        public static Point operator -(Point pointA) => new Point(-pointA.X, -pointA.Y, -pointA.Z);

        /// <inheritdoc cref="Multiply(double, Point)"/>
        public static Point operator *(double factor, Point point) => Multiply(factor, point);
        /// <inheritdoc cref="Multiply(double, Point)"/>
        public static Point operator *(Point point, double factor) => Multiply(factor, point);

        /// <inheritdoc cref="DotProduct(Point, Point)"/>
        public static double operator *(Point pointA, Point pointB) => DotProduct(pointA, pointB);

        /// <inheritdoc cref="Divide(Point, double)"/>
        public static Point operator /(Point point, double divisor) => Divide(point, divisor);

        #endregion

        #region Methods

        /// <summary>
        /// Computes the distance of the current <see cref="Point"/> to a <see cref="Point"/>.
        /// </summary>
        /// <param name="point"> <see cref="Point"/> to compute the distance to. </param>
        /// <returns> The value corresponding to the distance between the two <see cref="Point"/> in euclidean space.</returns>
        public double DistanceTo(Point point)
        {
            Point diff = this - point;
            return Math.Sqrt((diff.X * diff.X) + (diff.Y * diff.Y) + (diff.Z * diff.Z));
        }

        #endregion


        #region Override : Object

        /// <summary>
        /// Determines whether two object instances are equal or not.
        /// </summary>
        /// <param name="obj"> <see cref="object"/> to compare with. </param>
        /// <returns> <see langword="true"/> if the objects are equal, <see langword="false"/> otherwise. </returns>
        public override bool Equals(object obj) => obj is Point euclideanVector ? Equals(euclideanVector) : false;

        /// <summary>
        /// Returns a string description of the <see cref="Point"/>.
        /// </summary>
        /// <returns> The string description of the <see cref="Point"/>. </returns>
        public override string ToString()
        {
            return "Point at (" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
        }

        /// <summary>
        /// Returns a hash code for this <see cref="Point"/>.
        /// </summary>
        /// <returns> The hash code for this <see cref="Point"/>. </returns>
        public override int GetHashCode()
        {
            var hashCode = -307843816;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            return hashCode;
        }

        #endregion

        #region Inheritance : IEquatable

        /// <summary>
        /// Determines whether the two point instances are equal.
        /// </summary>
        /// <param name="point"> <see cref="Point"/> to compare with. </param>
        /// <returns> <see langword="true"/> if the two <see cref="Point"/> are equal, <see langword="false"/> otherwise. </returns>
        public bool Equals(Point point)
        {
            return DotProduct(this - point, this - point) < Settings.AbsolutePrecision;
        }

        #endregion

    }
}
