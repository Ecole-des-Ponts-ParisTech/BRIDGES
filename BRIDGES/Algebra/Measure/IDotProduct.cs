using System;
using System.Collections.Generic;
using System.Text;

namespace BRIDGES.Algebra.Measure
{
    /// <summary>
    /// Interface defining a method computing the dot product of two elements.
    /// </summary>
    /// <typeparam name="TValue"> Type of the elements in the field. </typeparam>
    /// <typeparam name="T"> Type of the elements in the pre-hilbertian set. </typeparam>
    public interface IDotProduct<TValue, T> : INorm<T>
    {
        #region Methods

        /// <summary>
        /// Computes the dot product of the current element with another element.
        /// </summary>
        /// <param name="operand"> Right element of the dot product. </param>
        /// <returns> The value of the dot product of the two elements. </returns>
        TValue DotProduct(T operand);

        /// <summary>
        /// Computes the angle between the current element and another element.
        /// </summary>
        /// <param name="other"> Element to compare with. </param>
        /// <returns> The value of the angle (in radians). </returns>
        double AngleWith(T other);

        #endregion

    }
}
