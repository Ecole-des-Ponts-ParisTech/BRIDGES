using System;


namespace BRIDGES.Algebra.Fundamentals
{
    /// <summary>
    /// Interface defining the division, i.e. the multiplication with an element's inverse value (or multiplicative inverse).
    /// </summary>
    /// <typeparam name="T"> Type of the elements in the multiplicative set. </typeparam>
    /// <remarks>
    /// The existence of an inverse is assumed but not its unicity. The left and right inverse can differ from one another.
    /// </remarks>
    internal interface IDivisible<T> : IMultiplicable<T>
        where T : IDivisible<T>
    {
        #region Methods

        /// <summary>
        /// Divides the current element with another element.
        /// </summary>
        /// <param name="other"> Element to divide with. </param>
        /// <returns> The new element resulting from the division. </returns>
        T Divide(T other);

        #endregion
    }
}
