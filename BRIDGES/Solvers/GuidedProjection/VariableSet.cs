using System;
using System.Collections.Generic;


namespace BRIDGES.Solvers.GuidedProjection
{
    /// <summary>
    /// Class defining a set of variables with the same dimension (i.e. variables with the same number of components).
    /// </summary>
    public class VariableSet
    {
        #region Fields

        /// <summary>
        /// List of variables of the set. 
        /// </summary>
        private List<double> _variables;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the index of the set in the <see cref="GuidedProjectionAlgorithm"/>'s list of variable sets.
        /// </summary>
        public int SetIndex { get; private set; }

        /// <summary>
        /// Gets the common dimension of variables in the set.
        /// </summary>
        public int VariableDimension { get; private set; }

        /// <summary>
        /// Gets the number of variables in the set. 
        /// </summary>
        public int VariableCount { get; private set; }


        /// <summary>
        /// Gets the rank of the first component of the first variable in the set.
        /// </summary>
        internal int FirstRank { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableSet"/> class.
        /// </summary>
        /// <param name="setIndex"> Index of the set in the <see cref="GuidedProjectionAlgorithm"/>'s list of variable sets. </param>
        /// <param name="firstRank"> Rank of the first component of the first variable in the set. </param>
        /// <param name="variableDimension"> Dimension of the variables in the set. </param>
        internal VariableSet(int setIndex, int firstRank, int variableDimension)
        {
            // Instanciate Fields 
            _variables = new List<double>();

            // Initialise Properties
            FirstRank = firstRank;

            SetIndex = setIndex;
            VariableCount = 0;
            VariableDimension = variableDimension;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableSet"/> class.
        /// </summary>
        /// <param name="setIndex"> Index of the set in the <see cref="GuidedProjectionAlgorithm"/>'s list of VariableSet. </param>
        /// <param name="firstRank"> Index of the first component of the first variable in the set. </param>
        /// <param name="variableDimension"> Dimension of the variables contained in the set. </param>
        /// <param name="setCapacity"> Indicative capacity of the set. </param>
        internal VariableSet(int setIndex, int firstRank, int variableDimension, int setCapacity)
        {
            // Instanciate Fields 
            _variables = new List<double>(variableDimension * setCapacity);

            // Initialise Properties
            FirstRank = firstRank;

            SetIndex = setIndex;
            VariableCount = 0;
            VariableDimension = variableDimension;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the component at the given index in the set.
        /// </summary>
        /// <param name="componentIndex"> Index of the component to get. </param>
        /// <returns> The component at the given index in the set. </returns>
        internal double GetComponent(int componentIndex)
        {
            return _variables[componentIndex];
        }

        /// <summary>
        /// Sets the component at the given index in the set.
        /// </summary>
        /// <param name="componentIndex"> Index of the component to set. </param>
        /// <param name="value"> Value to set. </param>
        internal void SetComponent(int componentIndex, double value)
        {
            _variables[componentIndex] = value;
        }


        /// <summary>
        /// Adds a variable to the set. 
        /// </summary>
        /// <param name="components"> Components of the variables to add. </param>
        public void AddVariable(params double[] components)
        {
            if (components.Length != VariableDimension)
            {
                throw new ArgumentOutOfRangeException("The number of components for the new variable" +
                    "does not match the expected dimension of the variables of the set.");
            }

            _variables.AddRange(components);
            VariableCount++;
        }

        /// <summary>
        /// Returns the components of the variable at the given index.
        /// </summary>
        /// <param name="variableIndex"> Index of the variable to get. </param>
        /// <returns> The components of the variable at the index. </returns>
        public double[] GetVariable(int variableIndex)
        {
            double[] variable = new double[VariableDimension];
            int index = variableIndex * VariableDimension;
            for (int i = 0; i < VariableDimension; i++)
            {
                variable[i] = _variables[index + i];
            }
            return variable;
        }

        #endregion
    }
}
