using System;


namespace BRIDGES.Geometry.Euclidean3D
{
    /// <summary>
    /// Class defining a frame in three-dimensional euclidean space.
    /// </summary>
    /// <remarks> For an ordered set of linearly independent <see cref="Vector"/> without an origin, refer to <see cref="Basis"/>. </remarks>
    public class Frame : IEquatable<Frame>
    {
        #region Properties

        /// <summary>
        /// Gets the number of axes of the current <see cref="Frame"/>.
        /// </summary>
        public int Dimension { get { return 3; } }


        /// <summary>
        /// Gets or sets the origin <see cref="Point"/> of the current <see cref="Frame"/>.
        /// </summary>
        public Point Origin { get; set; }

        /// <summary>
        /// Gets the first axis of the current <see cref="Frame"/>.
        /// </summary>
        public Vector XAxis { get; private set; }

        /// <summary>
        /// Gets the second axis of the current <see cref="Frame"/>.
        /// </summary>
        public Vector YAxis { get; private set; }

        /// <summary>
        /// Gets the third axis of the current <see cref="Frame"/>.
        /// </summary>
        public Vector ZAxis { get; private set; }


        /// <summary>
        /// Gets the axis of the current <see cref="Frame"/> at the given index.
        /// </summary>
        /// <param name="index"> Zero-based index of the axis to retrieve. </param>
        /// <returns> The axis at the given index. </returns>
        public Vector this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return XAxis;
                    case 1:
                        return YAxis;
                    case 2:
                        return ZAxis;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index), "The index of the axis must be between 0 and 2.");
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Frame"/> class by defining its origin and three linearly independent axes.
        /// </summary>
        /// <param name="origin"> Origin <see cref="Point"/> of the <see cref="Frame"/>. </param>
        /// <param name="xAxis"> First <see cref="Vector"/> axis  of the <see cref="Frame"/>. </param>
        /// <param name="yAxis"> Second <see cref="Vector"/> axis of the <see cref="Frame"/>. </param>
        /// <param name="zAxis"> Third <see cref="Vector"/> axis of the <see cref="Frame"/>. </param>
        /// <exception cref="ArgumentException"> The given axes are not linearly independent. </exception>
        public Frame(Point origin, Vector xAxis, Vector yAxis, Vector zAxis)
        {
            // Verification : Linearly independent
            if ( Vector.AreParallel(yAxis, xAxis) || Vector.AreParallel(zAxis, xAxis) || Vector.AreParallel(zAxis, yAxis))
            {
                throw new ArgumentException("The given axes are not linearly independent.");
            }

            // Initialisation
            Origin = origin;
            XAxis = xAxis;
            YAxis = yAxis;
            ZAxis = zAxis;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Frame"/> class by defining its origin and axes.
        /// </summary>
        /// <param name="origin"> Origin <see cref="Point"/>. </param>
        /// <param name="axes"> <see cref="Vector"/> axes. </param>
        /// <exception cref="RankException"> The number of axes is different from three, the dimension of the space. </exception>
        /// <exception cref="ArgumentException"> The given axes are not linearly independent. </exception>
        public Frame(Point origin, Vector[] axes)
        {
            // Verifications
            if (axes.Length != 3) 
            { 
                throw new RankException("The number of axes given is different from three, the dimension of the space."); 
            }
            else if (Vector.AreParallel(axes[0], axes[1]) || Vector.AreParallel(axes[0], axes[2]) || Vector.AreParallel(axes[1], axes[2]))
            {
                throw new ArgumentException("The given axes are not linearly independent.");
            }

            // Initialisation of the Property
            Origin = origin;
            XAxis = axes[0];
            YAxis = axes[1];
            ZAxis = axes[2];

        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Frame"/> class from another <see cref="Frame"/>.
        /// </summary>
        /// <param name="frame"> <see cref="Frame"/> to copy. </param>
        public Frame (Frame frame)
        {
            Origin = frame.Origin;
            XAxis = frame.XAxis;
            YAxis = frame.YAxis;
            ZAxis = frame.ZAxis;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Evaluates whether a the axis of a frame are orthogonal to one another.
        /// </summary>
        /// <param name="frame"> Frame to evaluate.</param>
        /// <returns> <see langword="true"/> if the axes are orthogonal, <see langword="false"/> otherwise.</returns>
        public static bool IsOrthogonal(Frame frame)
        {
            Vector[] axes = new Vector[3] { frame.XAxis, frame.YAxis, frame.ZAxis };

            for (int i = 0; i < axes.Length; i++)
            {
                if (axes[i].SquaredLength() < Settings.AbsolutePrecision) { return false; }

                for (int j = i + 1; j < axes.Length; j++)
                {
                    if (Math.Abs(Vector.DotProduct(axes[i], axes[j])) < Settings.AbsolutePrecision) { return false; }
                }
            }

            return true;
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Evaluates whether the current <see cref="Frame"/> is equal to another <see cref="Frame"/>.
        /// </summary>
        /// <remarks> 
        /// Two <see cref="Frame"/> are equal if their origin <see cref="Point"/> and their corresponding <see cref="Vector"/> axes are equal.
        /// </remarks>
        /// <param name="other"> <see cref="Frame"/> to compare with. </param>
        /// <returns> <see langword="true"/> if the two <see cref="Frame"/> are equal, <see langword="false"/> otherwise. </returns>
        public bool Equals(Frame other)
        {
            return Origin.Equals(other.Origin) && XAxis.Equals(other.XAxis)
                && YAxis.Equals(other.YAxis) && ZAxis.Equals(other.ZAxis);
        }

        #endregion


        #region Override : Object

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is Frame frame && Equals(frame);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"Frame at {Origin}, of dimension {Dimension}.";
        }

        #endregion
    }
}