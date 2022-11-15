using System;
using System.Collections.Generic;

using BRIDGES.Solvers.GuidedProjection.Interfaces;


namespace BRIDGES.Solvers.GuidedProjection
{
    /// <summary>
    /// Class defining an energy for the <see cref="GuidedProjectionAlgorithm"/>.
    /// </summary>
    public class Energy
    {
        #region Fields

        /// <summary>
        /// Energy type defining the reduced vector <see cref="IEnergyType.LocalKi"/> and the scalar value <see cref="IEnergyType.Si"/>.
        /// </summary>
        internal protected IEnergyType energyType;

        /// <summary>
        /// Variables composing the local vector xReduced on which the <see cref="energyType"/> is defined.
        /// </summary>
        /// <remarks> The first component corresponds to the variable set and the second to the index of the variable in the set. </remarks>
        internal protected List<(VariableSet Set, int Index)> variables;
        
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the weight of the energy.
        /// </summary>
        public double Weight { get; internal set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Energy"/> class.
        /// </summary>
        /// <param name="energyType"> Energy type defining the energy locally. </param>
        /// <param name="variablesKi"> Variables composing the reduced vector xReduced. </param>
        /// <param name="weight"> Weight of the energy. </param>
        internal Energy(IEnergyType energyType, List<(VariableSet, int)> variablesKi, double weight)
        {
            // Initialise Fields
            this.energyType = energyType;
            this.variables = variablesKi;

            // Initialise Properties
            Weight = weight;
        }

        #endregion
    }
}
