using System;

using BRIDGES.LinearAlgebra.Vectors;


namespace BRIDGES.Solvers.GuidedProjection.Interfaces
{
    /// <summary>
    /// Interface defining an energy type for the <see cref="GuidedProjectionAlgorithm"/>.
    /// </summary>
    public interface IEnergyType
    {
        #region Properties

        /// <summary>
        /// Gets the local vector Ki of the energy.
        /// </summary>
        SparseVector LocalKi { get; }

        /// <summary>
        /// Gets the scalar value Si of the energy.
        /// </summary>
        double Si { get; }

        #endregion
    }
}
