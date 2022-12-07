using System;

using Alg_Fund = BRIDGES.Algebra.Fundamentals;
using Alg_Set = BRIDGES.Algebra.Sets;
using Alg_Meas = BRIDGES.Algebra.Measure;

using Geo_Ker = BRIDGES.Geometry.Kernel;


namespace BRIDGES.Geometry.Euclidean3D
{
    /// <summary>
    /// Structure defining a point in three-dimensional euclidean space.
    /// </summary>
    public struct Point
        : Alg_Set.Additive.IAbelianGroup<Point>, Alg_Set.IGroupAction<double, Point>,
          Alg_Meas.IDotProduct<double, Point>,
          Geo_Ker.IAnalytic<double>,
          IEquatable<Point>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the first coordinate of the current <see cref="Point"/>.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the second coordinate of the current <see cref="Point"/>.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets the third coordinate of the current <see cref="Point"/>.
        /// </summary>
        public double Z { get; set; }


        /// <summary>
        /// Gets or sets the value of the coordinate at the given index.
        /// </summary>
        /// <param name="index"> Index of the coordinate to get or set. </param>
        /// <returns> The value of the coordinate at the given index. </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The given index is lower than zero or exceeds the number of coordinates of the point.
        /// For a three-dimensional euclidean point, the index can range from zero to two.
        /// </exception>
        public double this[int index]
        {
            get
            {
                if (index == 0) { return X; }
                else if (index == 1) { return Y; }
                else if (index == 2) { return Z; }
                else
                {
                    throw new ArgumentOutOfRangeException("The given index is lower than zero or exceeds the number of coordinates of the point." +
                        "For a three-dimensional euclidean point, the index can range from zero to two.");
                }
            }

            set
            {
                if (index == 0) { X = value; }
                else if (index == 1) { Y = value; }
                else if (index == 2) { Z = value; }
                else
                {
                    throw new ArgumentOutOfRangeException("The given index is lower than zero or exceeds the number of coordinates of the point." +
                        "For a three-dimensional euclidean point, the index can range from zero to two.");
                }
            }
        }

        /// <summary>
        /// Gets the dimension of the current <see cref="Point"/>'s euclidean space.
        /// </summary>
        public int Dimension { get { return 3; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Point"/> structure by defining its three coordinates.
        /// </summary>
        /// <param name="x"> Value of the first coordinate. </param>
        /// <param name="y"> Value of the second coordinate. </param>
        /// <param name="z"> Value of the third coordinate. </param>
        public Point(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="Point"/> structure by defining its coordinates.
        /// </summary>
        /// <param name="coordinates"> Value of the coordinates. </param>
        /// <exception cref="ArgumentOutOfRangeException"> The length of the coordinate array is different from three, the dimension of points in three-dimensional euclidean space. </exception>
        public Point(double[] coordinates)
        {
            // Verifications
            if (coordinates.Length != 3) { throw new ArgumentOutOfRangeException("The length of the coordinate array is different from three," +
                "the dimension of points in three-dimensional euclidean space."); }

            X = coordinates[0];
            Y = coordinates[1];
            Z = coordinates[2];
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Point"/> structure from another <see cref="Point"/>.
        /// </summary>
        /// <param name="point"> <see cref="Point"/> to copy. </param>
        public Point(Point point)
        {
            X = point.X;
            Y = point.Y;
            Z = point.Z;
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Gets a new <see cref="Point"/> with coordinates (0.0, 0.0, 0.0)
        /// </summary>
        public static Point Zero
        {
            get { return new Point(0.0, 0.0, 0.0); }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Returns the cross product of two <see cref="Point"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Point"/> for the cross product.</param>
        /// <param name="right"> Right <see cref="Point"/> for the cross product.</param>
        /// <returns> A new <see cref="Point"/> resulting from the cross product of two <see cref="Point"/>.</returns>
        public static Point CrossProduct(Point left, Point right) =>
            new Point((left.Y * right.Z) - (left.Z * right.Y),
            (left.Z * right.X) - (left.X * right.Z),
            (left.X * right.Y) - (left.Y * right.X));


        /******************** Algebraic Additive Group ********************/

        /// <inheritdoc cref="operator +(Point, Point)"/>
        public static Point Add(Point left, Point right)
        {
            return new Point(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        /// <inheritdoc cref="operator -(Point, Point)"/>
        public static Point Subtract(Point left, Point right)
        {
            return new Point(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }


        /******************** Vector Embedding ********************/

        /// <inheritdoc cref="operator +(Point, Vector)"/>
        public static Point Add(Point point, Vector vector)
        {
            return new Point(point.X + vector.X, point.Y + vector.Y, point.Z + vector.Z);
        }

        /// <inheritdoc cref="operator -(Point, Vector)"/>
        public static Point Subtract(Point point, Vector vector)
        {
            return new Point(point.X - vector.X, point.Y - vector.Y, point.Z - vector.Z);
        }


        /******************** Group Action ********************/

        /// <inheritdoc cref="Point.operator *(double, Point)"/>
        public static Point Multiply(double factor, Point operand)
        {
            return new Point(factor * operand.X, factor * operand.Y, factor * operand.Z);
        }

        /// <inheritdoc cref="Point.operator /(Point, double)"/>
        public static Point Divide(Point operand, double divisor)
        {
            return new Point(operand.X / divisor, operand.Y / divisor, operand.Z / divisor);
        }


        /******************** Hilbert Space ********************/

        /// <inheritdoc cref="operator *(Point, Point)"/>
        public static double DotProduct(Point left, Point right)
        {
            return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
        }

        #endregion

        #region Operators

        /******************** Algebraic Additive Group ********************/

        /// <summary>
        /// Computes the addition of two <see cref="Point"/>.
        /// </summary>
        /// <param name="left"> <see cref="Point"/> for the addition. </param>
        /// <param name="right"> <see cref="Point"/> for the addition. </param>
        /// <returns> The new <see cref="Point"/> resulting from the addition. </returns>
        public static Point operator +(Point left, Point right)
        {
            return new Point(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        /// <summary>
        /// Computes the subtraction of two <see cref="Point"/>.
        /// </summary>
        /// <param name="left"> <see cref="Point"/> to subtract. </param>
        /// <param name="right"> <see cref="Point"/> to subtract with. </param>
        /// <returns> The new <see cref="Point"/> resulting from the subtraction. </returns>
        public static Point operator -(Point left, Point right)
        {
            return new Point(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        /// <summary>
        /// Computes the opposite of the given <see cref="Point"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Point"/> to be opposed. </param>
        /// <returns> The new <see cref="Point"/>, opposite of the initial one. </returns>
        public static Point operator -(Point operand)
        {
            return new Point(-operand.X, -operand.Y, -operand.Z);
        }


        /******************** Vector Embedding ********************/

        /// <summary>
        /// Computes the addition of a <see cref="Point"/> with a <see cref="Vector"/>.
        /// </summary>
        /// <param name="point"> <see cref="Point"/> for the addition. </param>
        /// <param name="vector"> <see cref="Vector"/> for the addition. </param>
        /// <returns> The new <see cref="Point"/> resulting from the addition. </returns>
        public static Point operator +(Point point, Vector vector)
        {
            return new Point(point.X + vector.X, point.Y + vector.Y, point.Z + vector.Z);
        }

        /// <summary>
        /// Computes the subtraction of a <see cref="Point"/> with a <see cref="Vector"/>.
        /// </summary>
        /// <param name="point"> <see cref="Point"/> to subtract. </param>
        /// <param name="vector"> <see cref="Vector"/> to subtract with. </param>
        /// <returns> The new <see cref="Point"/> resulting from the subtraction. </returns>
        public static Point operator -(Point point, Vector vector)
        {
            return new Point(point.X - vector.X, point.Y - vector.Y, point.Z - vector.Z);
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Computes the scalar multiplication of a <see cref="Point"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="factor"> <see cref="double"/>-precision real number. </param>
        /// <param name="operand"> <see cref="Point"/> to multiply. </param>
        /// <returns> The new <see cref="Point"/> resulting from the scalar multiplication. </returns>
        public static Point operator *(double factor, Point operand)
        {
            return new Point(factor * operand.X, factor * operand.Y, factor * operand.Z);
        }

        /// <inheritdoc cref="Point.operator *(double, Point)"/>
        public static Point operator *(Point operand, double factor)
        {
            return new Point(operand.X * factor, operand.Y * factor, operand.Z * factor);
        }

        /// <summary>
        /// Computes the scalar division of a <see cref="Point"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="operand"> <see cref="Point"/> to divide. </param>
        /// <param name="divisor"> <see cref="double"/>-precision real number to divide with. </param>
        /// <returns> The new <see cref="Point"/> resulting from the scalar division. </returns>
        public static Point operator /(Point operand, double divisor)
        {
            return new Point(operand.X / divisor, operand.Y / divisor, operand.Z / divisor);
        }


        /******************** Hilbert Space ********************/

        /// <summary>
        /// Computes the dot product of two <see cref="Point"/>.
        /// </summary>
        /// <param name="left"> <see cref="Point"/> for the dot product. </param>
        /// <param name="right"> <see cref="Point"/> for the dot product. </param>
        /// <returns> The new <see cref="Point"/> resulting from the dot product of two <see cref="Point"/>. </returns>
        public static double operator *(Point left, Point right)
        {
            return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
        }

        #endregion

        #region Casts

        /// <summary>
        /// Casts a <see cref="Vector"/> into a <see cref="Point"/>.
        /// </summary>
        /// <param name="vector"> <see cref="Vector"/> to cast. </param>
        /// <returns> The new <see cref="Point"/> resulting from the cast. </returns>
        public static implicit operator Point(Vector vector)
        {
            return new Point(vector.X, vector.Y, vector.Z);
        }

        /// <summary>
        /// Casts a <see cref="Projective3D.Point"/> into a <see cref="Point"/>.
        /// </summary>
        /// <param name="point"> <see cref="Projective3D.Point"/> to cast. </param>
        /// <returns> The new <see cref="Point"/> resulting from the cast. </returns>
        /// <exception cref="InvalidCastException"> The projective point cannot be converted. It represents an euclidean point at infinity. </exception>
        public static explicit operator Point(Projective3D.Point point)
        {
            if (point[3] == 0.0)
            {
                throw new InvalidCastException("The projective point cannot be converted because. It represents an euclidean point at infinity.");
            }

            return new Point(point[0] / point[3], point[1] / point[3], point[2] / point[3]);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the coordinates of the current <see cref="Point"/>.
        /// </summary>
        /// <returns> The array representation of the <see cref="Point"/>'s coordinates. </returns>
        public double[] GetCoordinates()
        {
            return new double[3] { X, Y, Z };
        }


        /// <summary>
        /// Computes the distance of the current <see cref="Point"/> to another <see cref="Point"/> (using the L2-norm).
        /// </summary>
        /// <param name="other"> <see cref="Point"/> to evaluate the distance to. </param>
        /// <returns> The value of the distance between the two <see cref="Point"/>. </returns>
        public double DistanceTo(Point other)
        {
            Point diff = this - other;
            return Math.Sqrt((diff.X * diff.X) + (diff.Y * diff.Y) + (diff.Z * diff.Z));
        }

        /// <summary>
        /// Computes the L2-norm the current <see cref="Point"/>.
        /// </summary>
        /// <returns> The value of the norm. </returns>
        public double Norm()
        {
            return Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
        }


        /// <summary>
        /// Evaluates whether the current <see cref="Point"/> is memberwise equal to another <see cref="Point"/>.
        /// </summary>
        /// <remarks> 
        /// Two <see cref="Point"/> are equal if their coordinates are equal.
        /// </remarks>
        /// <param name="other"> <see cref="Point"/> to compare with. </param>
        /// <returns> <see langword="true"/> if the two <see cref="Point"/> are equal, <see langword="false"/> otherwise. </returns>
        public bool Equals(Point other)
        {
            Point diff = this - other;

            double squareDistance = (diff.X * diff.X) + (diff.Y * diff.Y) + (diff.Z * diff.Z);

            return squareDistance < Settings.AbsolutePrecision;
        }

        #endregion


        #region Override : Object

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is Point point && Equals(point);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"({X},{Y},{Z})";
        }

        #endregion


        #region Explicit : Additive.IAbelianGroup<Point>

        /******************** Properties ********************/

        /// <inheritdoc/>
        bool Alg_Fund.IAddable<Point>.IsAssociative => true;

        /// <inheritdoc/>
        bool Alg_Fund.IAddable<Point>.IsCommutative => true;


        /******************** Methods ********************/

        /// <inheritdoc/>
        Point Alg_Fund.IAddable<Point>.Add(Point right) { return new Point(X + right.X, Y + right.Y, Z + right.Z); }

        /// <inheritdoc/>
        Point Alg_Fund.ISubtractable<Point>.Subtract(Point right) { return new Point(X - right.X, Y - right.Y, Z - right.Z); }

        /// <inheritdoc/>
        bool Alg_Set.Additive.IGroup<Point>.Opposite() 
        { 
            X = -X; 
            Y = -Y; 
            Z = -Z;

            return true;
        }

        /// <inheritdoc/>
        Point Alg_Fund.IZeroable<Point>.Zero() { return new Point(0.0, 0.0, 0.0); }

        #endregion

        #region Explicit : IGroupAction<Double,Point>

        /******************** Methods ********************/

        /// <inheritdoc/>
        Point Alg_Set.IGroupAction<double, Point>.Multiply(double factor) { return new Point(factor * X, factor * Y, factor * Z); }

        /// <inheritdoc/>
        Point Alg_Set.IGroupAction<double, Point>.Divide(double divisor) { return new Point(X / divisor, Y / divisor, Z / divisor); }

        #endregion

        #region Explicit : IDotProduct<Point>

        /******************** Methods ********************/

        /// <inheritdoc cref="Alg_Meas.INorm{T}.Unitise"/>
        /// <exception cref="DivideByZeroException"> The length of the current <see cref="Point"/> must be different than zero.</exception> 
        void Alg_Meas.INorm<Point>.Unitise()
        {
            double length = Math.Sqrt((X * X) + (Y * Y) + (Z * Z));

            if (length == 0.0)
            {
                throw new DivideByZeroException("The norm of the current point must be different than zero.");
            }

            X /= length; Y /= length; Z /= length;

            return;
        }


        /// <inheritdoc cref="Alg_Meas.IDotProduct{TValue,T}.DotProduct(T)"/>
        double Alg_Meas.IDotProduct<double,Point>.DotProduct(Point right)
        {
            return (X * right.X) + (Y * right.Y) + (Z * right.Z);
        }

        /// <inheritdoc cref="Alg_Meas.IDotProduct{TValue,T}.AngleWith(T)"/>
        double Alg_Meas.IDotProduct<double,Point>.AngleWith(Point other)
        {
            double dotProduct = (X * other.X) + (Y * other.Y) + (Z * other.Z);
            double length = Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
            double otherLength = Math.Sqrt((other.X * other.X) + (other.Y * other.Y) + (other.Z * other.Z));

            return Math.Acos(dotProduct / (length * otherLength));
        }

        #endregion
    }
}