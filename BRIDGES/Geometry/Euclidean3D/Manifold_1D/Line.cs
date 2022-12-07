using System;

using Geo_Ker = BRIDGES.Geometry.Kernel;


namespace BRIDGES.Geometry.Euclidean3D
{
    /// <summary>
    /// Structure defining a line in three-dimensional euclidean space.<br/>
    /// It is defined by a start point and an axis (infinite length).
    /// </summary>
    /// <remarks> For a finite line, refer to <see cref="Segment"/>. </remarks>
    public struct Line : IEquatable<Line>, Geo_Ker.IGeometricallyEquatable<Line>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the origin of the current <see cref="Line"/>.
        /// </summary>
        public Point Origin { get; set; }

        /// <summary>
        /// Gets or sets the axis of the current <see cref="Line"/>.
        /// </summary>
        public Vector Axis { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Line"/> structure by defining its origin and axis.
        /// </summary>
        /// <param name="origin"> Origin <see cref="Point"/> of the <see cref="Line"/>. </param>
        /// <param name="axis"> Axis of the <see cref="Line"/>. </param>
        public Line(Point origin, Vector axis)
        {
            Origin = origin;
            Axis = axis;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Line"/> structure from another <see cref="Line"/>.
        /// </summary>
        /// <param name="line"> <see cref="Line"/> to copy. </param>
        public Line(Line line)
        {
            Origin = line.Origin;
            Axis = line.Axis;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Flips the current <see cref="Line"/> by flipping its <see cref="Axis"/>.
        /// </summary>
        public void Flip()
        {
            Axis = -Axis;
        }


        /// <summary>
        /// Evaluates the current <see cref="Line"/> at the given parameter.
        /// </summary>
        /// <param name="t"> Parameter to evaluate the <see cref="Line"/>. </param>
        /// <param name="format"> Format of the parameter.
        /// <list type="bullet">
        /// <item>
        /// <term>Normalised</term>
        /// <description> The point at <paramref name="t"/> = 1.0 is <em>Origin + Axis</em>. </description>
        /// </item>
        /// <item>
        /// <term>ArcLength</term>
        /// <description> The point at <paramref name="t"/> = 1.0 is at distance 1.0 of <em>Origin</em> in the <em>Axis</em> direction. </description>
        /// </item>
        /// </list> </param>
        /// <returns> The <see cref="Point"/> on the <see cref="Line"/> at the given parameter. </returns>
        /// <exception cref="NotImplementedException"> The given format for the curve parameter is not implemented. </exception>
        public Point PointAt(double t, Geo_Ker.CurveParameterFormat format)
        {
            Vector axis = Axis;

            if (format == Geo_Ker.CurveParameterFormat.ArcLength) { axis.Unitise(); }
            else if (format == Geo_Ker.CurveParameterFormat.Normalised) { /* Do Nothing */ }
            else { throw new NotImplementedException("The given format for the curve parameter is not implemented."); }

            return Origin + (t * axis);
        }


        /// <summary>
        /// Evaluates whether the current <see cref="Line"/> is memberwise equal to another <see cref="Line"/>.
        /// </summary>
        /// <param name="other"> <see cref="Line"/> to compare with. </param>
        /// <returns> <see langword="true"/> if the two <see cref="Line"/> are equal, <see langword="false"/> otherwise. </returns>
        public bool Equals(Line other)
        {
            return Origin.Equals(other.Origin) && Axis.Equals(other.Axis);
        }

        /// <inheritdoc/>
        public bool GeometricallyEquals(Line other)
        {
            return Vector.AreParallel(Axis, other.Axis) && 
                (Origin.Equals(other.Origin) || Vector.AreParallel(Origin - other.Origin, Axis));
        }

        #endregion
        

        #region Override : Object

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is Line line && Equals(line);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"Line starting at {Origin}, aligned with the direction {Axis}.";
        }

        #endregion
    }
}
