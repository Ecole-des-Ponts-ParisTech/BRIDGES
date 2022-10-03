using System;
using System.Collections.Generic;


namespace BRIDGES.Solvers.GuidedProjection.Interfaces
{
    /// <summary>
    /// Interface defining a set of energy for the <see cref="GuidedProjectionAlgorithm"/>.
    /// </summary>
    internal interface IEnergySet
    {
        #region Properties

        /// <summary>
        /// Gets the index of the set in the <see cref="GuidedProjectionAlgorithm"/>'s list of energy sets.
        /// </summary>
        int SetIndex { get; }

        /// <summary>
        /// Gets the number of energies in the set. 
        /// </summary>
        int EnergyCount { get; }


        /// <summary>
        /// Gets the energy at the given index in the set.
        /// </summary>
        /// <param name="index">Index of the energy in the set.</param>
        /// <returns>The energy a the given index.</returns>
        IEnergy this[int index] { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds an energy to the set.
        /// </summary>
        /// <param name="weight"> Weight for the energy. </param>
        /// <returns> The newly created energy. </returns>
        IEnergy AddEnergy(double weight = 1.0);

        /// <summary>
        /// Computes the local vector Ki and the value Si if needed, and translates the local Ki into the global Ki defined on X.
        /// </summary>
        void ComputeAndGlobalise();

        #endregion
    }
}
