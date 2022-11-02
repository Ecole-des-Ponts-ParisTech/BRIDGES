using System;
using System.Collections.Generic;

using BRIDGES.LinearAlgebra.Vectors;

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
        private IEnergyType _energyType { get; }

        /// <summary>
        /// Variables composing the local vector xReduced on which the <see cref="_energyType"/> is defined.
        /// </summary>
        /// <remarks> The first component corresponds to the variable set and the second to the index of the variable in the set. </remarks>
        private List<(VariableSet, int)> _variables;
        
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the weight of the energy.
        /// </summary>
        public double Weight { get; internal set; }


        /// <summary>
        /// Gets the vector Ki defined on x.
        /// </summary>
        public SparseVector GlobalKi { get; private set; }

        /// <summary>
        /// Gets the scalar value Si of the energy.
        /// </summary>
        public double Si 
        { 
            get { return _energyType.Si; }
        }

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
            _energyType = energyType;
            _variables = variablesKi;

            // Initialise Properties
            Weight = weight;
        }

        #endregion

        #region Other Methods

        /// <summary>
        /// Translates the local members of the energy defined on xReduced, into the global members defined on x.
        /// </summary>
        /// <param name="size"> Number of component of the global vector x. </param>
        internal void Complete(int size)
        {
            if (!(_energyType.LocalKi is null) &&  _energyType.LocalKi.NonZerosCount != 0) { CompleteKi(size); }
        }


        /// <summary>
        /// Translates the reduced vector <see cref="IEnergyType.LocalKi"/> defined on xReduced, into <see cref="GlobalKi"/> defined on x.
        /// </summary>
        /// <param name="size"> Number of component of the global vector x. </param>
        private void CompleteKi(int size)
        {
            /******************** Translator for the column indices ********************/

            // Create the translator
            List<int> converter = new List<int>();
            for (int i_Variable = 0; i_Variable < _variables.Count; i_Variable++)
            {
                int firstRank = _variables[i_Variable].Item1.FirstRank;
                int variableDimension = _variables[i_Variable].Item1.VariableDimension;

                int variableIndex = _variables[i_Variable].Item2;

                int startIndex = firstRank + (variableDimension * variableIndex);

                for (int i_Component = 0; i_Component < variableDimension; i_Component++)
                {
                    converter.Add(startIndex + i_Component);
                }
            }

            /******************** Create global Ki ********************/

            int nonZerosCount = _energyType.LocalKi.NonZerosCount;
            Dictionary<int, double> components = new Dictionary<int, double>(nonZerosCount);

            foreach(var component in _energyType.LocalKi.GetNonZeros())
            {
                components.Add(converter[component.RowIndex], component.Value);
            }

            GlobalKi = new SparseVector(size , ref components);
        }

        #endregion
    }
}
