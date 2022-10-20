using System;
using System.Collections.Generic;

using BRIDGES.LinearAlgebra.Matrices.Storage;


namespace BRIDGES.Solvers.GuidedProjection.Interfaces
{
    /// <summary>
    /// Interface defining a quadratic constraint type for the <see cref="GuidedProjectionAlgorithm"/>.
    /// </summary>
    public interface IQuadraticConstraintType
    {
        #region Properties

        /// <summary>
        /// Gets the local symmetric matrix Hi of the energy.
        /// </summary>
        DictionaryOfKeys LocalHi { get; }

        /// <summary>
        /// Gets the local vector Bi of the energy.
        /// </summary>
        Dictionary<int, double> LocalBi { get; }

        /// <summary>
        /// Gets the scalar value Ci of the energy.
        /// </summary>
        double Ci { get; }

        #endregion
    }
}
