using System;
using System.Collections.Generic;

using BRIDGES.LinearAlgebra.Matrices.Storage;
using BRIDGES.Solvers.GuidedProjection.Interfaces;


namespace BRIDGES.Solvers.GuidedProjection
{
    /// <summary>
    /// Class defining a constraint for the <see cref="GuidedProjectionAlgorithm"/>.
    /// </summary>
    public class Constraint : IConstraint
    {
        #region Fields

        /// <summary>
        /// Variables composing the reduced vector xReduced on which the local symmetric matrix Hi and the local vector Bi are defined.<br/>
        /// </summary>
        /// <remarks> The first item corresponds to the variable set and the second to the index of the variable in the set. </remarks>
        private List<(VariableSet, int)> _variables;

        #endregion

        #region Properties

        /// <inheritdoc cref="IConstraint.Weight"/>
        public double Weight { get; set; }


        /// <inheritdoc cref="IConstraint.GlobalHi"/>
        public DictionaryOfKeys GlobalHi { get; internal set; }

        /// <inheritdoc cref="IConstraint.GlobalBi"/>
        public Dictionary<int, double> GlobalBi { get; internal set; }

        /// <inheritdoc cref="IConstraint.Ci"/>
        public double? Ci { get; internal set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Constraint"/> class.
        /// </summary>
        /// <param name="variables"> Variables composing the reduced vector xReduced on which the local symmetric matrix Hi and the local vector Bi are defined.</param>
        /// <param name="ci"> Double value of the constraint. </param>
        /// <param name="weight"> Weight of the constraint. </param>
        internal Constraint(List<(VariableSet, int)> variables, double ci, double weight)
        {
            // Initialize Properties
            _variables = variables;

            // Initialize Properties
            Weight = weight;
            Ci = ci;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the variables composing the reduced vector xReduced of the constraint.
        /// </summary>
        /// <returns> The variables composing the reduced vector xReduced of the constraint. </returns>
        internal List<(VariableSet, int)> GetVariables() => _variables;

        #endregion
    }
}
