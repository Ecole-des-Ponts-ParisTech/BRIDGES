using System;
using System.Collections.Generic;
using System.Text;

namespace BRIDGES.Algebra.Measure
{
    /// <summary>
    /// Interface defining the dot product of two <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="TValue"> Type of the elements in the field. </typeparam>
    /// <typeparam name="T"> Type of the elements in the set. </typeparam>
    internal interface IDotProduct<TValue, T> : INorm<T>
    {
        #region Methods

        /// <summary>
        /// Computes the dot product of the current <typeparamref name="T"/> with another <typeparamref name="T"/>.
        /// </summary>
        /// <param name="other"> Right <typeparamref name="T"/> of the dot product. </param>
        /// <returns> The value of the dot product of the two <typeparamref name="T"/>. </returns>
        TValue DotProduct(T other);

        /// <summary>
        /// Computes the angle between the current <typeparamref name="T"/> and another <typeparamref name="T"/>.
        /// </summary>
        /// <param name="other"> <typeparamref name="T"/> to compare with. </param>
        /// <returns> The value of the angle (in radians). </returns>
        double AngleWith(T other);

        #endregion

    }
}
