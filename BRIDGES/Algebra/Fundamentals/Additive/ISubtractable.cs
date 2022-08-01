using System;


namespace BRIDGES.Algebra.Fundamentals
{
    /// <summary>
    /// Interface defining the subtraction, i.e. the addition with an element's opposite value (or additive inverse).
    /// </summary>
    /// <typeparam name="T"> Type of the elements in the additive set. </typeparam>
    /// <remarks>
    /// The existence of an opposite is assumed but not its unicity. The left and right opposite can differ from one another.
    /// </remarks>
    public interface ISubtractable<T>
        where T : ISubtractable<T>
    {
        #region Methods

        /// <summary>
        /// Subtracts the current element with another element.
        /// </summary>
        /// <param name="right"> Element to subtract with on the right. </param>
        /// <returns> The new element resulting from the subtraction. </returns>
        T Subtract(T right);

        #endregion
    }
}
