using System;


namespace BRIDGES.Geometry.Kernel
{
    /// <summary>
    /// Interface defining a basic element in analytic geometry.
    /// </summary>
    /// <typeparam name="TValue"> Type of the coordinate values. </typeparam>
    internal interface IAnalytic<TValue>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the value of the coordinate at the given index.
        /// </summary>
        /// <param name="index"> Index of the coordinate to get or set. </param>
        /// <returns> The value of the coordinate at the given index. </returns>
        TValue this[int index] { get; set; }

        /// <summary>
        /// Gets the number of coordinates of the current element.
        /// </summary>
        int Dimension { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the coordinates of the current element.
        /// </summary>
        /// <returns> The array representation of the element's coordinates. </returns>
        TValue[] GetCoordinates();

        #endregion
    }
}
