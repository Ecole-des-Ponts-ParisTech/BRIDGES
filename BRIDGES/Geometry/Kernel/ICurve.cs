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
    /// <typeparam name="TPoint"> Type of point in the geometric space. </typeparam>
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
        /// Gets the start value of the normalised domain for the current curve's.
        /// </summary>
        double DomainStart { get; }

        /// <summary>
        /// Gets the end value of the normalised domain for the current curve's.
        /// </summary>
        double DomainEnd { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Computes the length of the current curve.
        /// </summary>
        /// <returns> The length of the current curve. </returns>
        double Length();

        /// <summary>
        /// Flips the direction of the current curve.
        /// </summary>
        void Flip();

        /// <summary>
        /// Evaluates the current curve at the given parameter.
        /// </summary>
        /// <param name="t"> Parameter to evaluate the curve. </param>
        /// <param name="format"> Format of the parameter. </param>
        /// <returns> The point on the curve at the given parameter. </returns>
        TPoint PointAt(double t, CurveParameterFormat format);

        #endregion
    }
}
