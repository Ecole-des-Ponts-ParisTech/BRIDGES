using System;


namespace BRIDGES.Geometry.Euclidean3D
{
    /// <summary>
    /// Class defining a basis in three-dimensional euclidean space.<br/>
    /// </summary>
    /// <remarks> For a basis placed at an origin <see cref="Point"/>, refer to <see cref="Frame"/>. </remarks>
    public class Basis : IEquatable<Basis>
    {
        #region Fields

        /// <summary>
        /// Axes of the current <see cref="Basis"/>.
        /// </summary>
        private Vector[] _axes;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of axes of the current <see cref="Basis"/>.
        /// </summary>
        public int Dimension { get { return _axes.Length; } }


        /// <summary>
        /// Gets the first axis of the current <see cref="Basis"/>.
        /// </summary>
        public Vector XAxis { get { return _axes[0]; } }

        /// <summary>
        /// Gets the second axis of the current <see cref="Basis"/>.
        /// </summary>
        public Vector YAxis { get { return _axes[1]; } }

        /// <summary>
        /// Gets the third axis of the current <see cref="Basis"/>.
        /// </summary>
        public Vector ZAxis { get { return _axes[2]; } }


        /// <summary>
        /// Gets the axis of the current <see cref="Basis"/> at the given index.
        /// </summary>
        /// <param name="index"> Index of the axis to retrieve. </param>
        /// <returns> The axis at the given index. </returns>
        public Vector this[int index] { get { return _axes[index]; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Basis"/> class by defining its three linearly independent axes.
        /// </summary>
        /// <param name="xAxis"> First axis  of the <see cref="Basis"/>. </param>
        /// <param name="yAxis"> Second axis of the <see cref="Basis"/>. </param>
        /// <param name="zAxis"> Third axis of the <see cref="Basis"/>. </param>
        /// <exception cref="ArgumentException"> The given axes are not linearly independent. </exception>
        public Basis(Vector xAxis, Vector yAxis, Vector zAxis)
        {
            // Verification : Linearly independent
            if (Vector.AreParallel(yAxis, xAxis) || Vector.AreParallel(zAxis, xAxis) || Vector.AreParallel(zAxis, yAxis))
            {
                throw new ArgumentException("The given axes are not linearly independent.");
            }

            // Instanciation of the fields
            _axes = new Vector[3];

            // Initialisation of the fields
            _axes[0] = xAxis;
            _axes[1] = yAxis;
            _axes[2] = zAxis;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Basis"/> class by defining its axes.
        /// </summary>
        /// <param name="axes"> Set of axes. </param>
        /// <exception cref="RankException"> The number of axes given is different from three, the dimension of the space. </exception>
        /// <exception cref="ArgumentException"> The given axes are not linearly independent. </exception>
        public Basis(Vector[] axes)
        {
            // Verification : Number of Axes
            if (axes.Length != 3)
            {
                throw new RankException("The number of axes given is different from three, the dimension of the space.");
            }

            // Verification : Linearly independent
            if (Vector.AreParallel(axes[0], axes[1]) || Vector.AreParallel(axes[0], axes[2]) || Vector.AreParallel(axes[1], axes[2]))
            {
                throw new ArgumentException("The given axes are not linearly independent.");
            }

            // Instanciation of the fields
            _axes = new Vector[3];

            // Initialisation of the fields
            _axes[0] = axes[0];
            _axes[1] = axes[1];
            _axes[2] = axes[2];
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Basis"/> class from another <see cref="Basis"/>.
        /// </summary>
        /// <param name="basis"> <see cref="Basis"/> to copy. </param>
        public Basis(Basis basis)
        {
            _axes = new Vector[3] { basis.XAxis, basis.YAxis, basis.ZAxis };
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Evaluates whether a the axis of a basis are orthogonal to one another.
        /// </summary>
        /// <param name="basis"> Basis to evaluate.</param>
        /// <returns> <see langword="true"/> if the axes are orthogonal, <see langword="false"/> otherwise.</returns>
        public static bool IsOrthogonal(Basis basis)
        {
            Vector[] axes = basis._axes;

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

        #region Methods

        /// <summary>
        /// Evaluates whether the current <see cref="Basis"/> is equal to another <see cref="Basis"/>.
        /// </summary>
        /// <remarks> 
        /// Two <see cref="Basis"/> are equal if their corresponding <see cref="Vector"/> axes are equal.
        /// </remarks>
        /// <param name="other"> <see cref="Basis"/> to compare with. </param>
        /// <returns> <see langword="true"/> if the two <see cref="Basis"/> are equal, <see langword="false"/> otherwise. </returns>
        public bool Equals(Basis other)
        {
            return _axes[0].Equals(other.XAxis) && _axes[1].Equals(other.YAxis) && _axes[2].Equals(other.ZAxis);
        }

        #endregion


        #region Override : Object

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is Basis basis && Equals(basis);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"Basis of dimension {Dimension}.";
        }

        #endregion
    }
}