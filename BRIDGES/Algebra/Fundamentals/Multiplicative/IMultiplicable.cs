using System;


namespace BRIDGES.Algebra.Fundamentals
{
    /// <summary>
    /// Interface defining the multiplication of two operands, i.e. an internal binary operation.
    /// </summary>
    /// <typeparam name="T"> Type of the elements in the multiplicative set. </typeparam>
    public interface IMultiplicable<T>
        where T : IMultiplicable<T>
    {
        #region Properties

        /// <summary>
        /// Evaluates whether the multiplication is associative : a * (b * c) = (a * b) * c.
        /// </summary>
        /// <remarks> 
        /// This property should be implemented explicitely as it is only informative.<br/>
        /// It also helps prevents any confusion with the homonymous property of the addition.
        /// </remarks>
        bool IsAssociative { get; }

        /// <summary>
        /// Evaluates whether the multiplication is commutative : a * b = b * a.
        /// </summary>
        /// <remarks> 
        /// This property should be implemented explicitely as it is only informative.<br/>
        /// It also helps prevents any confusion with the homonymous property of the addition.
        /// </remarks>
        bool IsCommutative { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Multiplies the current element with another element.
        /// </summary>
        /// <param name="right"> Element to multiply with on the right. </param>
        /// <returns> The new element resulting from the multiplication. </returns>
        T Multiply(T right);

        #endregion
    }
}
