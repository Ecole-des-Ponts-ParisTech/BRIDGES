using System;


namespace BRIDGES.Geometry.Kernel
{
    /// <summary>
    /// Enumeration defining the format of the curve parameter.
    /// </summary>
    public enum CurveParameterFormat
    {
        /// <summary>
        /// The curve is spanned by a parameter ranging from <see cref="ICurve{TPoint}.DomainStart"/> to <see cref="ICurve{TPoint}.DomainEnd"/>.
        /// </summary>
        Normalised,

        /// <summary>
        /// The curve is spanned by a parameter from 0.0 (start) to L (end), where L is the length of the curve. 
        /// </summary>
        ArcLength
    }


    /// <summary>
    /// Interface defining a curve.
    /// </summary>
    /// <typeparam name="TPoint"> Point type of geometric space. </typeparam>
    internal interface ICurve<TPoint>
    {
        #region Properties

        /// <summary>
        /// Gets a boolean evaluating whether the current curve is closed or not;
        /// </summary>
        bool IsClosed { get; }


        /// <summary>
        /// Gets the start point of the current curve.
        /// </summary>
        TPoint StartPoint { get; }

        /// <summary>
        /// Gets the end point of the current curve.
        /// </summary>
        TPoint EndPoint { get; }


        /// <summary>
        /// Gets the start value of the current curve's domain.
        /// </summary>
        double DomainStart { get; }

        /// <summary>
        /// Gets the end value of the current curve's domain.
        /// </summary>
        double DomainEnd { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Computes the length of the current <see cref="ICurve{TPoint}"/>.
        /// </summary>
        /// <returns> The length of the current <see cref="ICurve{TPoint}"/>. </returns>
        double Length();

        /// <summary>
        /// Flips the direction of the current <see cref="ICurve{TPoint}"/>
        /// </summary>
        void Flip();

        /// <summary>
        /// Evaluates the current <see cref="ICurve{TPoint}"/> at the given parameter.
        /// </summary>
        /// <param name="parameter"> Value of the parameter. </param>
        /// <param name="format"> Format of the parameter. </param>
        /// <returns> The <typeparamref name="TPoint"/> on the <see cref="ICurve{TPoint}"/> at the given parameter. </returns>
        TPoint PointAt(double parameter, CurveParameterFormat format);

        #endregion
    }
}
