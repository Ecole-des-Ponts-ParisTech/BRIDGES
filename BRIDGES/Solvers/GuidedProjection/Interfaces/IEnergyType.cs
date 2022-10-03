using System;
using System.Collections.Generic;


namespace BRIDGES.Solvers.GuidedProjection.Interfaces
{
    /// <summary>
    /// Interface defining an energy type for the <see cref="GuidedProjectionAlgorithm"/>
    /// </summary>
    public interface IEnergyType
    {
        #region Properties

        /// <summary>
        /// Gets the local vector Ki.
        /// </summary>
        Dictionary<int, double> LocalKi { get; }

        /// <summary>
        /// Gets the value Si.
        /// </summary>
        double Si { get; }

        #endregion
    }
}
