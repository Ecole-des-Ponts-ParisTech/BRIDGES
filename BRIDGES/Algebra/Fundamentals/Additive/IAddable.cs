﻿using System;


namespace BRIDGES.Algebra.Fundamentals
{
    /// <summary>
    /// Interface defining a method for the addition of two operands (i.e. an internal binary operation).
    /// </summary>
    /// <typeparam name="T"> Type of the elements of the additive set. </typeparam>
    public interface IAddable<T>
        where T : IAddable<T>
    {
        #region Properties

        /// <summary>
        /// Evaluates whether the addition is associative : a + (b + c) = (a + b) + c.
        /// </summary>
        /// <remarks> 
        /// This property should be implemented explicitely as it is only informative.<br/>
        /// It also helps prevents any confusion with the homonymous property of the multiplication.
        /// </remarks>
        bool IsAssociative { get; }

        /// <summary>
        /// Evaluates whether the addition is commutative : a + b = b + a.
        /// </summary>
        /// <remarks> 
        /// This property should be implemented explicitely as it is only informative.<br/>
        /// It also helps prevents any confusion with the homonymous property of the multiplication.
        /// </remarks>
        bool IsCommutative { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Computes the addition of the current element with another element on the right.
        /// </summary>
        /// <param name="right"> Element to add with on the right. </param>
        /// <returns> The new element resulting from the addition. </returns>
        T Add(T right);

        #endregion
    }
}
