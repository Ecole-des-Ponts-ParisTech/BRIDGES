using System;


namespace BRIDGES.Algebra.Fundamentals
{
    /// <summary>
    /// Interface defining a method returning the neutral element of the multiplication.
    /// </summary>
    /// <typeparam name="T"> Type of the elements in the multiplicative set. </typeparam>
    public interface IOneable<T> : IMultiplicable<T>
        where T : IOneable<T>
    {
        #region Methods

        /// <summary>
        /// Returns the neutral element of the multiplication.
        /// </summary>
        /// <returns> The neutral element of the multiplication. </returns>
        T One();

        #endregion
    }
}
