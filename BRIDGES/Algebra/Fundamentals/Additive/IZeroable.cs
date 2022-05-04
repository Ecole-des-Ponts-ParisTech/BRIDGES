using System;


namespace BRIDGES.Algebra.Fundamentals
{
    /// <summary>
    /// Interfaces defining the neutral element of the addition.
    /// </summary>
    /// <typeparam name="T"> Type of the elements in the additive set. </typeparam>
    internal interface IZeroable<T>
        where T : IZeroable<T>
    {
        #region Methods

        /// <summary>
        /// Gets the neutral element of the addition.
        /// </summary>
        /// <returns> The neutral element of the addition. </returns>
        T Zero();

        #endregion
    }
}
