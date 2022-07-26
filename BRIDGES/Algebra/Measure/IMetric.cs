using System;
using System.Collections.Generic;
using System.Text;

namespace BRIDGES.Algebra.Measure
{
    /// <summary>
    /// Interface defining the distance between two <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"> Type of the elements in the set. </typeparam>
    public interface IMetric<T>
    {
        #region Methods

        /// <summary>
        /// Computes the distance of the current <typeparamref name="T"/> to another <typeparamref name="T"/>.
        /// </summary>
        /// <param name="other"> <typeparamref name="T"/> to evaluate the distance to. </param>
        /// <returns> The value of the distance between the two <typeparamref name="T"/>. </returns>
        double DistanceTo(T other);

        #endregion
    }
}