using System;
using System.Collections.Generic;
using System.Text;

namespace BRIDGES.Algebra.Measure
{
    /// <summary>
    /// Interface defining a method computing the distance between two elements.
    /// </summary>
    /// <typeparam name="T"> Type of the elements in the metric set. </typeparam>
    public interface IMetric<T>
    {
        #region Methods

        /// <summary>
        /// Computes the distance of the current element to another element.
        /// </summary>
        /// <param name="other"> Element to evaluate the distance to. </param>
        /// <returns> The value of the distance between the two elements. </returns>
        double DistanceTo(T other);

        #endregion
    }
}