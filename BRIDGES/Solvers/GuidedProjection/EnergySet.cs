using System;
using System.Collections.Generic;

using BRIDGES.Solvers.GuidedProjection.Interfaces;


namespace BRIDGES.Solvers.GuidedProjection
{
    /// <summary>
    /// Class defining a set of energy for the <see cref="GuidedProjectionAlgorithm"/>.
    /// </summary>
    /// <typeparam name="TEnergy"> Energy type of the set. </typeparam>
    public class EnergySet<TEnergy> : IEnergySet
        where TEnergy : IEnergyType, new()
    {
        #region Fields

        /// <summary>
        /// List of energies of the set.
        /// </summary>
        internal List<Energy> _energies;

        /// <summary>
        /// Class defining the local vector Ki and Si for the set.
        /// </summary>
        TEnergy _energyType;

        #endregion

        #region Properties

        /// <inheritdoc cref="IEnergySet.SetIndex"/>
        public int SetIndex { get; }

        /// <inheritdoc cref="IEnergySet.EnergyCount"/>
        public int EnergyCount { get; }


        /// <summary>
        /// Gets the energy at the given index in the set.
        /// </summary>
        /// <param name="index"> Index of the energy in the set. </param>
        /// <returns> The energy a the given index. </returns>
        public Energy this[int index]
        {
            get { return _energies[index]; }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EnergySet{TEnergy}"/> class.
        /// </summary>
        /// <param name="energyType"> Instance of an energy type for the set. </param>
        /// <param name="setIndex"> Index of the set in the <see cref="GuidedProjectionAlgorithm"/>'s list of energy sets. </param>
        internal EnergySet(TEnergy energyType, int setIndex)
        {
            // Instanciate Fields
            _energies = new List<Energy>();

            // Initialise Fields
            _energyType = energyType;

            // Initialise Properties
            SetIndex = setIndex;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnergySet{TEnergy}"/> class.
        /// </summary>
        /// <param name="energyType"> Instance of an energy type for the set. </param>
        /// <param name="setIndex"> Index of the set in the <see cref="GuidedProjectionAlgorithm"/>'s list of energy sets.</param>
        /// <param name="setCapacity"> Indicative number of quadratic constraints that the set can initially store. </param>
        internal EnergySet(TEnergy energyType, int setIndex, int setCapacity)
        {
            // Instanciate Fields
            _energies = new List<Energy>(setCapacity);

            // Initialise Fields
            _energyType = energyType;

            // Initialise Properties
            SetIndex = setIndex;
        }

        #endregion

        #region Methods

        /// <inheritdoc cref="IEnergySet.AddEnergy(double)"/>
        public Energy AddEnergy(double weight = 1.0)
        {
            Energy energy = new Energy(weight);
            _energies.Add(energy);
            return energy;
        }

        /// <inheritdoc cref="IEnergySet.ComputeAndGlobalise"/>
        internal void ComputeAndGlobalise()
        {
            if (!(_energyType.LocalKi is null) && _energyType.LocalKi.Count != 0)
            {
                for (int i_Energy = 0; i_Energy < EnergyCount; i_Energy++)
                {
                    Energy energy = _energies[i_Energy];
                    LocalToGlobalKi(energy, _energyType.LocalKi);
                }
            }

            if (_energyType.Si != 0.0)
            {
                for (int i_Energy = 0; i_Energy < EnergyCount; i_Energy++)
                {
                    Energy energy = _energies[i_Energy];
                    LocalToGlobalSi(energy, _energyType.Si);
                }
            }
        }


        /// <summary>
        /// Creates the global Ki of the contraint from the localKi.
        /// </summary>
        /// <param name="energy"> Energy for which the global Ki is computed. </param>
        /// <param name="localKi"> Local Ki to globalise. </param>
        private void LocalToGlobalKi(Energy energy, Dictionary<int, double> localKi)
        {
            /******************** Translator for the column indices ********************/

            List<(VariableSet, int)> variablesKi = energy.GetVariablesKi();

            // Create the translator
            List<int> translator = new List<int>();
            for (int i_Variable = 0; i_Variable < variablesKi.Count; i_Variable++)
            {
                int firstRank = variablesKi[i_Variable].Item1.FirstRank;
                int variableDimension = variablesKi[i_Variable].Item1.VariableDimension;

                int variableIndex = variablesKi[i_Variable].Item2;

                int startIndex = firstRank + (variableDimension * variableIndex);

                for (int i_Component = 0; i_Component < variableDimension; i_Component++)
                {
                    translator.Add(startIndex + i_Component);
                }
            }

            /******************** Create global Ki ********************/

            Dictionary<int, double> globalKi = new Dictionary<int, double>();

            // Iterate on the keys (RowIndex) of the sparse vector.
            foreach (int key in _energyType.LocalKi.Keys)
            {
                globalKi.Add(translator[key], _energyType.LocalKi[key]);
            }

            energy.GlobalKi = globalKi;
        }

        /// <summary>
        /// Spreads the constant Si defined on the set into each energy.
        /// </summary>
        private void LocalToGlobalSi(Energy energy, double Si)
        {
            energy.Si = Si;
        }


        #endregion


        #region IEnergySet

        /**************************************** Properties ****************************************/

        IEnergy IEnergySet.this[int index]
        { 
            get { return _energies[index]; } 
        }

        /**************************************** Methods ****************************************/

        /// <inheritdoc cref="IEnergySet.AddEnergy(double)"/>
        IEnergy IEnergySet.AddEnergy(double weight)
        {
            return AddEnergy(weight);
        }

        /// <inheritdoc cref="IEnergySet.ComputeAndGlobalise"/>
        void IEnergySet.ComputeAndGlobalise()
        {
            ComputeAndGlobalise();
        }

        #endregion
    }
}
