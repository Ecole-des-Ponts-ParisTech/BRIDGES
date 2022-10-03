using System;
using System.Collections.Generic;

using BRIDGES.Solvers.GuidedProjection.Interfaces;


namespace BRIDGES.Solvers.GuidedProjection
{
    /// <summary>
    /// Class defining a energy for the <see cref="GuidedProjectionAlgorithm"/>.
    /// </summary>
    public class Energy : IEnergy
    {
        #region Fields

        /// <summary>
        /// Variables composing the local vector x which multiplies the local vector Ki on the right.
        /// </summary>
        /// <remarks> The first component corresponds to the variable set and the second to the index of the variable in the set. </remarks>
        private List<(VariableSet, int)> _variablesKi;

        #endregion

        #region Properties

        /// <inheritdoc cref="IEnergy.Weight"/>
        public double Weight { get; set; }


        /// <inheritdoc cref="IEnergy.GlobalKi"/>
        public Dictionary<int, double> GlobalKi { get; internal set; }

        /// <inheritdoc cref="IEnergy.Si"/>
        public double? Si { get; internal set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Energy"/> class.
        /// <param name="weight"> Weight for the energy. </param>
        /// </summary>
        internal Energy(double weight = 1.0)
        {
            // Initialise Properties
            Weight = weight;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the variables involved in the energy.
        /// </summary>
        /// <returns> The variables involved in the energy. </returns>
        internal List<(VariableSet, int)> GetVariablesKi() => _variablesKi;

        /// <summary>
        /// Defines the variables involved in the energy.
        /// </summary>
        /// <param name="variablesKi"> Variables composing the local vector x which multiplies the local vector Ki on the right.</param>
        public void DefineKiVariables(List<(VariableSet, int)> variablesKi)
        {
            _variablesKi = variablesKi;
        }

        #endregion
    }
}
