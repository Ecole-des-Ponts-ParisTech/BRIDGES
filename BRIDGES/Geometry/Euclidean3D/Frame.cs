using System;


namespace BRIDGES.Geometry.Euclidean3D
{
    /// <summary>
    /// Class defining a frame in three-dimensional euclidean space.<br/>
    /// It is defined by an origin <see cref="Point"/> and an ordered set of linearly independent <see cref="Vector"/>.
    /// </summary>
    /// <remarks> For an ordered set of linearly independent <see cref="Vector"/> without an origin, refer to <see cref="Basis"/>. </remarks>
    public class Frame
    {
        #region Fields

        /// <summary>
        /// Axes of the current <see cref="Frame"/>.
        /// </summary>
        private Vector[] _axes;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of axes of the current <see cref="Frame"/>.
        /// </summary>
        public int Dimension { get { return _axes.Length; } }

        /// <summary>
        /// Gets or sets the origin <see cref="Point"/> of the current <see cref="Frame"/>.
        /// </summary>
        public Point Origin { get; set; }


        /// <summary>
        /// Gets the first axis of the current <see cref="Frame"/>.
        /// </summary>
        public Vector XAxis { get { return _axes[0]; } }

        /// <summary>
        /// Gets the second axis of the current <see cref="Frame"/>.
        /// </summary>
        public Vector YAxis { get { return _axes[1]; } }

        /// <summary>
        /// Gets the third axis of the current <see cref="Frame"/>.
        /// </summary>
        public Vector ZAxis { get { return _axes[2]; } }

        /// <summary>
        /// Gets the axis of the current <see cref="Frame"/> at the given index.
        /// </summary>
        /// <param name="index"> Index of the axis to retrieve. </param>
        /// <returns> The axis at the given index. </returns>
        public Vector this[int index] { get { return _axes[index]; } }

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
            // Verification : Orthogonal Vectors
            if ( !Vector.AreOrthogonal(yAxis, xAxis) || !Vector.AreOrthogonal(zAxis, xAxis) || !Vector.AreOrthogonal(zAxis, yAxis))
            {
                throw new ArgumentException("The given axes are not linearly independent.");
            }

            // Instanciation of the Fields
            _axes = new Vector[3];

            // Initialisation of the Fields
            _axes[0] = xAxis;
            _axes[1] = yAxis;
            _axes[2] = zAxis;

            // Initialisation of the Property
            Origin = origin;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Frame"/> class by defining its origin and axes.
        /// </summary>
        /// <param name="origin"> Origin <see cref="Point"/>. </param>
        /// <param name="axes"> <see cref="Vector"/> axes. </param>
        /// <exception cref="RankException"> The number of axes given is different from three, the dimension of the space. </exception>
        /// <exception cref="ArgumentException"> The given axes are not linearly independent. </exception>
        public Frame(Point origin, Vector[] axes)
        {
            // Verification : Number of Axes
            if (axes.Length != 3) 
            {
                throw new RankException("The number of axes given is different from three, the dimension of the space.");
            }

            // Verification : Orthogonal Vectors
            if (!Vector.AreOrthogonal(axes[0], axes[1]) || !Vector.AreOrthogonal(axes[0], axes[2]) || !Vector.AreOrthogonal(axes[1], axes[2]))
            {
                throw new ArgumentException("The given axes are not linearly independent.");
            }

            // Instanciation of the fields
            _axes = new Vector[3];

            // Initialisation of the fields
            _axes[0] = axes[0];
            _axes[1] = axes[1];
            _axes[2] = axes[2];

            // Initialisation of the Property
            Origin = origin;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Frame"/> class from another <see cref="Frame"/>.
        /// </summary>
        /// <param name="frame"> <see cref="Frame"/> to copy. </param>
        public Frame (Frame frame)
        {
            Origin = frame.Origin;
            _axes = new Vector[3] { frame.XAxis, frame.YAxis, frame.ZAxis };
        }

        #endregion

        #region Methods

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
            return Origin.Equals(other.Origin) && _axes[0].Equals(other.XAxis)
                && _axes[1].Equals(other.YAxis) && _axes[2].Equals(other.ZAxis);
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
            return $"A frame at {Origin}, of dimension {Dimension}.";
        }

        #endregion
    }
}
