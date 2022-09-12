using System;


namespace BRIDGES.Algebra.Fundamentals
{
    /// <summary>
    /// Interface defining a method returning the neutral element of the addition.
    /// </summary>
    /// <typeparam name="T"> Type of the elements of the additive set. </typeparam>
    public interface IZeroable<T>
        where T : IZeroable<T>
    {
        #region Methods

        /// <summary>
        /// Returns the neutral element of the addition.
        /// </summary>
        /// <returns> The neutral element of the addition. </returns>
        T Zero();

        #endregion
    }
}
