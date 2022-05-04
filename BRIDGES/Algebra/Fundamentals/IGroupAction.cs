﻿using System;


namespace BRIDGES.Algebra.Fundamentals
{
    /// <summary>
    /// Interface defining a group action of a field on a set.
    /// </summary>
    /// <typeparam name="T"> Type of the elements in the set. </typeparam>
    /// <typeparam name="TValue"> Type of the elements in the field. </typeparam>
    internal interface IGroupAction<T,TValue>
    {
        #region Methods

        /// <summary>
        /// Multiplies the current element with a scalar.
        /// </summary>
        /// <param name="factor"> The scalar to multiply with. </param>
        /// <returns> The new element resulting from the scalar multiplication. </returns>
        T Multiply(TValue factor);

        /// <summary>
        /// Divides the current element with a scalar.
        /// </summary>
        /// <param name="divisor"> The scalar to divide with. </param>
        /// <returns> The new element resulting from the scalar dividion. </returns>
        T Divide(TValue divisor);

        #endregion
    }
}
