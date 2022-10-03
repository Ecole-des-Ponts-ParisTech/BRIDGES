using System;
using System.Collections.Generic;


namespace BRIDGES.Solvers.GuidedProjection.Interfaces
{
    /// <summary>
    /// Interface defining an energy for the <see cref="GuidedProjectionAlgorithm"/>.
    /// </summary>
    internal interface IEnergy
    {
        #region Properties

        /// <summary>
        /// Value of the weight for the energy.
        /// </summary>
        double Weight { get; }


        /// <summary>
        /// Vector Ki defined on X.
        /// </summary>
        Dictionary<int, double> GlobalKi { get; }

        /// <summary>
        /// Value Si for the energy.
        /// </summary>
        double? Si { get; }

        #endregion
    }
}
