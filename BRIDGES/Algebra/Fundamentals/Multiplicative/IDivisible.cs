using System;


namespace BRIDGES.Algebra.Fundamentals
{
    /// <summary>
    /// Interface defining a method for the division of two operands (i.e. the multiplication with an element's inverse value).
    /// </summary>
    /// <typeparam name="T"> Type of the elements in the multiplicative set. </typeparam>
    /// <remarks>
    /// The existence of an inverse is assumed but not its unicity. The left and right inverse can differ from one another.
    /// </remarks>
    public interface IDivisible<T> : IMultiplicable<T>
        where T : IDivisible<T>
    {
        #region Methods

        /// <summary>
        /// Computes the division of the current element with another element on the right.
        /// </summary>
        /// <param name="right"> Element to divide with on the right. </param>
        /// <returns> The new element resulting from the division. </returns>
        T Divide(T right);

        #endregion
    }
}
