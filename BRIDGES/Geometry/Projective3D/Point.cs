using System;

using Alg_Fund = BRIDGES.Algebra.Fundamentals;
using Alg_Set = BRIDGES.Algebra.Sets;

using Geo_Ker = BRIDGES.Geometry.Kernel;


namespace BRIDGES.Geometry.Projective3D
{
    /// <summary>
    /// Structure defining a point in three-dimensional projective space.
    /// </summary>
    public struct Point
        : Alg_Set.Additive.IAbelianGroup<Point>, Alg_Set.IGroupAction<double, Point>,
          Geo_Ker.IAnalytic<double>,
          IEquatable<Point>
    {
        #region Fields

        /// <summary>
        /// Coordinates of the current <see cref="Point"/>.
        /// </summary>
        private double[] _coordinates;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value of the coordinate at the given index.
        /// </summary>
        /// <param name="index"> Index of the coordinate to get or set. </param>
        /// <returns> The value of the coordinate at the given index. </returns>
        public double this[int index]
        {
            get{ return _coordinates[index]; }

            set { _coordinates[index] = value; }
        }

        /// <summary>
        /// Gets the dimension of the current <see cref="Point"/>'s projective space.
        /// </summary>
        public int Dimension { get { return 3; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Point"/> structure by defining its three coordinates.
        /// </summary>
        /// <param name="x0"> Value of the first coordinate. </param>
        /// <param name="x1"> Value of the second coordinate. </param>
        /// <param name="x2"> Value of the third coordinate. </param>
        /// <param name="x3"> Value of the fourth coordinate. </param>
        public Point(double x0, double x1, double x2, double x3)
        {
            _coordinates = new double[4] { x0, x1, x2, x3 };
        }

        /// <summary>
        /// Initialises a new instance of <see cref="Point"/> structure by defining its coordinates.
        /// </summary>
        /// <param name="coordinates"> Value of the coordinates. </param>
        /// <exception cref="ArgumentOutOfRangeException"> The length of the coordinate array is different from four, the number of coordinates of points in three-dimensional projective space. </exception>
        public Point(double[] coordinates)
        {
            // Verifications
            if (coordinates.Length != 4)
            {
                throw new ArgumentOutOfRangeException("The length of the coordinate array is different from four," +
                "the number of coordinates of points in three-dimensional projective space.");
            }

            _coordinates = new double[4] { coordinates[0], coordinates[1], coordinates[2], coordinates[3] };
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Point"/> structure from another <see cref="Point"/>.
        /// </summary>
        /// <param name="point"> <see cref="Point"/> to copy. </param>
        public Point(Point point)
        {
            _coordinates = new double[4] { point._coordinates[0], point._coordinates[1], point._coordinates[2], point._coordinates[3] };
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Gets a new <see cref="Point"/> with coordinates (0.0, 0.0, 0.0, 0.0)
        /// </summary>
        public static Point Zero
        {
            get { return new Point(0.0, 0.0, 0.0, 0.0); }
        }

        #endregion

        #region Static Methods

        /******************** Algebraic Additive Group ********************/

        /// <inheritdoc cref="operator +(Point, Point)"/>
        public static Point Add(Point left, Point right)
        {
            return new Point(left._coordinates[0] + right._coordinates[0], left._coordinates[1] + right._coordinates[1],
                left._coordinates[2] + right._coordinates[2], left._coordinates[3] + right._coordinates[3]);
        }

        /// <inheritdoc cref="operator -(Point, Point)"/>
        public static Point Subtract(Point left, Point right)
        {
            return new Point(left._coordinates[0] - right._coordinates[0], left._coordinates[1] - right._coordinates[1],
                left._coordinates[2] - right._coordinates[2], left._coordinates[3] - right._coordinates[3]);
        }


        /******************** Group Action ********************/

        /// <inheritdoc cref="Point.operator *(double, Point)"/>
        public static Point Multiply(double factor, Point operand)
        {
            return new Point(factor * operand._coordinates[0], factor * operand._coordinates[1], factor * operand._coordinates[2], factor * operand._coordinates[3]);
        }

        /// <inheritdoc cref="Point.operator /(Point, double)"/>
        public static Point Divide(Point operand, double divisor)
        {
            return new Point(operand._coordinates[0] / divisor, operand._coordinates[1] / divisor, operand._coordinates[2] / divisor, operand._coordinates[3] / divisor);
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
            return new Point(left._coordinates[0] + right._coordinates[0], left._coordinates[1] + right._coordinates[1],
                left._coordinates[2] + right._coordinates[2], left._coordinates[3] + right._coordinates[3]);
        }

        /// <summary>
        /// Computes the subtraction of two <see cref="Point"/>.
        /// </summary>
        /// <param name="left"> <see cref="Point"/> to subtract. </param>
        /// <param name="right"> <see cref="Point"/> to subtract with. </param>
        /// <returns> The new <see cref="Point"/> resulting from the subtraction. </returns>
        public static Point operator -(Point left, Point right)
        {
            return new Point(left._coordinates[0] - right._coordinates[0], left._coordinates[1] - right._coordinates[1],
                left._coordinates[2] - right._coordinates[2], left._coordinates[3] - right._coordinates[3]);
        }

        /// <summary>
        /// Computes the opposite of the given <see cref="Point"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Point"/> to be opposed. </param>
        /// <returns> The new <see cref="Point"/>, opposite of the initial one. </returns>
        public static Point operator -(Point operand)
        {
            return new Point(-operand._coordinates[0], -operand._coordinates[1], -operand._coordinates[2], -operand._coordinates[3]);
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
            return new Point(factor * operand._coordinates[0], factor * operand._coordinates[1], factor * operand._coordinates[2], factor * operand._coordinates[3]);
        }

        /// <inheritdoc cref="Point.operator *(double, Point)"/>
        public static Point operator *(Point operand, double factor)
        {
            return new Point(operand._coordinates[0] * factor, operand._coordinates[1] * factor, operand._coordinates[2] * factor, operand._coordinates[3] * factor);
        }

        /// <summary>
        /// Computes the scalar division of a <see cref="Point"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="operand"> <see cref="Point"/> to divide. </param>
        /// <param name="divisor"> <see cref="double"/>-precision real number to divide with. </param>
        /// <returns> The new <see cref="Point"/> resulting from the scalar division. </returns>
        public static Point operator /(Point operand, double divisor)
        {
            return new Point(operand._coordinates[0] / divisor, operand._coordinates[1] / divisor, operand._coordinates[2] / divisor, operand._coordinates[3] / divisor);
        }

        #endregion

        #region Casts

        /// <summary>
        /// Casts a <see cref="Euclidean3D.Point"/> into a <see cref="Point"/>.
        /// </summary>
        /// <param name="point"> <see cref="Euclidean3D.Point"/> to cast. </param>
        /// <returns> The new <see cref="Point"/> resulting from the cast. </returns>
        public static explicit operator Point(Euclidean3D.Point point)
        {
            return new Point(point.X, point.Y, point.Z, 1.0);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the coordinates of the current <see cref="Point"/>.
        /// </summary>
        /// <returns> The array representation of the <see cref="Point"/>'s coordinates. </returns>
        public double[] GetCoordinates()
        {
            return new double[4] { _coordinates[0], _coordinates[1], _coordinates[2], _coordinates[3] };
        }


        /// <summary>
        /// Evaluates whether the current <see cref="Point"/> is equal to another <see cref="Point"/>.
        /// </summary>
        /// <remarks> 
        /// Two <see cref="Point"/> are equal if their coordinates are equal.
        /// </remarks>
        /// <param name="other"> <see cref="Point"/> to compare with. </param>
        /// <returns> <see langword="true"/> if the two <see cref="Point"/> are equal, <see langword="false"/> otherwise. </returns>
        public bool Equals(Point other)
        {
            int i_C;
            double ratio = 0.0;

            // Calculate a potential common ratio.
            for (i_C = 0; i_C < 4; i_C++)
            {
                if (_coordinates[i_C] != 0.0)
                {
                    if (other._coordinates[i_C] == 0.0) { return false; }
                    else { ratio = other._coordinates[i_C] / _coordinates[i_C]; break; }
                }
                else if (other._coordinates[i_C] != 0.0) { return false; }
            }

            // Evaluates whether the ratio is common or not.
            for (int i = i_C + 1; i < 4; i++)
            {
                if (_coordinates[i] != 0.0 && ratio != other._coordinates[i] / _coordinates[i]) { return false; }
                else if (other._coordinates[i] != 0.0) { return false; }
            }

            return true;
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
            return $"({_coordinates[0]},{_coordinates[1]},{_coordinates[2]},{_coordinates[3]})";
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
        Point Alg_Fund.IAddable<Point>.Add(Point right)
        {
            return new Point(_coordinates[0] + right._coordinates[0], _coordinates[1] + right._coordinates[1],
                _coordinates[2] + right._coordinates[2], _coordinates[3] + right._coordinates[3]);
        }

        /// <inheritdoc/>
        Point Alg_Fund.ISubtractable<Point>.Subtract(Point right)
        {
            return new Point(_coordinates[0] - right._coordinates[0], _coordinates[1] - right._coordinates[1],
                _coordinates[2] - right._coordinates[2], _coordinates[3] - right._coordinates[3]);
        }

        /// <inheritdoc/>
        bool Alg_Set.Additive.IGroup<Point>.Opposite()
        {
            _coordinates[0] = -_coordinates[0];
            _coordinates[1] = -_coordinates[1];
            _coordinates[2] = -_coordinates[2];
            _coordinates[3] = -_coordinates[3];

            return true;
        }

        /// <inheritdoc/>
        Point Alg_Fund.IZeroable<Point>.Zero() { return Point.Zero; }

        #endregion

        #region Explicit : IGroupAction<Double,Point>

        /******************** Methods ********************/

        /// <inheritdoc/>
        Point Alg_Set.IGroupAction<double, Point>.Multiply(double factor) 
        {
            return new Point(factor * _coordinates[0], factor * _coordinates[1], factor * _coordinates[2], factor * _coordinates[3]); 
        }

        /// <inheritdoc/>
        Point Alg_Set.IGroupAction<double, Point>.Divide(double divisor) 
        {
            return new Point(_coordinates[0] / divisor, _coordinates[1] / divisor, _coordinates[2] / divisor, _coordinates[3] / divisor); 
        }

        #endregion

    }
}
