using System;
using System.Collections.Generic;
using System.Text;

namespace BRIDGES.Algebra.Measure
{
    /// <summary>
    /// Interface defining the norm of a <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"> Type of the elements in the normed set. </typeparam>
    public interface INorm<T> : IMetric<T>
    {
        #region Methods

        /// <summary>
        /// Computes the norm the current <typeparamref name="T"/>.
        /// </summary>
        /// <returns> The value of the norm. </returns>
        double Norm();

        /// <summary>
        /// Unitises the current <typeparamref name="T"/>.
        /// </summary>
        void Unitise();

        #endregion
    }
}
