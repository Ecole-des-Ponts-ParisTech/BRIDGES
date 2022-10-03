using System;
using System.Collections.Generic;

using BRIDGES.LinearAlgebra.Matrices.Storage;
using BRIDGES.Solvers.GuidedProjection.Interfaces;


namespace BRIDGES.Solvers.GuidedProjection.Abstracts
{
    /// <summary>
    /// Class defining a set of constraints for the <see cref="GuidedProjectionAlgorithm"/>.
    /// </summary>
    /// <typeparam name="TConstraint"> Constraint type of the set. </typeparam>
    /// 
    public abstract class ConstraintSet<TConstraint> : IConstraintSet
    {
        #region Fields

        /// <summary>
        /// List of constraints of the set.
        /// </summary>
        private protected List<Constraint> _constraints;

        /// <summary>
        /// Class defining the local symmetric matrix Hi and the local vector Bi defined on xReduced.
        /// </summary>
        private protected TConstraint _constraintType;

        #endregion

        #region Properties

        /// <inheritdoc cref="IConstraintSet.SetIndex"/>
        public int SetIndex { get; }

        /// <inheritdoc cref="IConstraintSet.ConstraintCount"/>
        public int ConstraintCount
        {
            get { return _constraints.Count; }
        }


        /// <summary>
        /// Gets the constraint at the given index in the set.
        /// </summary>
        /// <param name="index"> Index of the constraint in the set. </param>
        /// <returns> The constraint at the given index. </returns>
        public Constraint this[int index]
        {
            get { return _constraints[index]; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintSet{TConstraint}"/> class.
        /// </summary>
        /// <param name="constraintType"> Constraint type of the set. </param>
        /// <param name="setIndex"> Index of the set in the <see cref="GuidedProjectionAlgorithm"/>'s list of constraint sets. </param>
        internal ConstraintSet(TConstraint constraintType, int setIndex)
        {
            // Instanciate Fields
            _constraints = new List<Constraint>();

            // Initialise Fields
            _constraintType = constraintType;

            // Initialise Properties
            SetIndex = setIndex;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintSet{TConstraint}"/> class.
        /// </summary>
        /// <param name="constraintType"> Constraint type of the set. </param>
        /// <param name="setIndex"> Index of the set in the <see cref="GuidedProjectionAlgorithm"/>'s list of constraint sets. </param>
        /// <param name="setCapacity"> Indicative number of constraints that the set can initially store. </param>
        internal ConstraintSet(TConstraint constraintType, int setIndex, int setCapacity)
        {
            // Instanciate Fields
            _constraints = new List<Constraint>(setCapacity);

            // Initialise Fields
            _constraintType = constraintType;

            // Initialise Properties
            SetIndex = setIndex;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a constraint to the set.
        /// </summary>
        /// <param name="variables"> Variables composing the reduced vector xReduced on which the local symmetric matrix Hi and the local vector Bi are defined.</param>
        /// <param name="ci"> Double value of the constraint. </param>
        /// <param name="weight"> Weight of the constraint. </param>
        /// <returns></returns>
        public Constraint AddConstraint(List<(VariableSet, int)> variables, double ci = 0.0, double weight = 1.0)
        {
            Constraint constraint = new Constraint(variables, ci, weight);
            _constraints.Add(constraint);
            return constraint;
        }


        /// <inheritdoc cref="IConstraintSet.ComputeAndGlobalise"/>
        internal abstract void ComputeAndGlobalise();


        /// <summary>
        /// Creates a converter translating the local indices of a constraint defined on xReduced into global indices defined on X.
        /// </summary>
        /// <param name="variables"> Variables composing the reduced vector xReduced of the constraint. </param>
        /// <returns> The converter for the globalisation of the constraint. </returns>
        private protected List<int> CreateConverter(List<(VariableSet,int)> variables)
        {
            // Create the translator
            List<int> converter = new List<int>();
            for (int i_Variable = 0; i_Variable < variables.Count; i_Variable++)
            {
                int firstRank = variables[i_Variable].Item1.FirstRank;
                int variableDimension = variables[i_Variable].Item1.VariableDimension;

                int variableIndex = variables[i_Variable].Item2;

                int startIndex = firstRank + (variableDimension * variableIndex);

                for (int i_Component = 0; i_Component < variableDimension; i_Component++)
                {
                    converter.Add(startIndex + i_Component);
                }
            }

            return converter;
        }

        /// <summary>
        /// Globalises the local symmetric matrix Hi defined on xReduced.
        /// </summary>
        /// <param name="localHi"> Local symmetric matrix Hi defined on xReduced to globalise. </param>
        /// <param name="converter"> Converter translating Hi's row and column indices. </param>
        /// <returns> The global symmetric matrix Hi defined on X. </returns>
        private protected DictionaryOfKeys GlobaliseHi(DictionaryOfKeys localHi, List<int> converter)
        {
            Dictionary<(int, int), double> values = localHi._values;

            DictionaryOfKeys dok_GlobalHi = new DictionaryOfKeys();

            // Iterate on the keys (RowIndex, ColumnIndex) of the sparse matrix.
            foreach ((int, int) key in values.Keys) // *.Keys is an O(1) operation.
            {
                dok_GlobalHi.Add(values[key], converter[key.Item1], converter[key.Item2]);
            }

            return dok_GlobalHi;
        }

        /// <summary>
        /// Globalises the local vector Bi defined on xReduced.
        /// </summary>
        /// <param name="localBi"> Local vector Bi defined on xReduced to globalise. </param>
        /// <param name="converter"> Converter translating Bi's row indices. </param>
        /// <returns> The global vector Bi defined on X. </returns>
        private protected Dictionary<int,double> GlobaliseBi(Dictionary<int, double> localBi, List<int> converter)
        {
            Dictionary<int, double> globalBi = new Dictionary<int, double>();

            // Iterate on the keys (RowIndex) of the sparse vector.
            foreach (int key in localBi.Keys)
            {
                globalBi.Add(converter[key], localBi[key]);
            }

            return globalBi;
        }

        #endregion


        #region IQuadraticConstraintSet

        /**************************************** Properties ****************************************/

        IConstraint IConstraintSet.this[int index]
        {
            get { return _constraints[index]; }
        }

        /**************************************** Methods ****************************************/

        /// <inheritdoc cref="IConstraintSet.ComputeAndGlobalise()"/>
        void IConstraintSet.ComputeAndGlobalise()
        {
            ComputeAndGlobalise();
        }

        #endregion
    }
}

/*

/// <summary>
/// Translates the local symmetric matrix Hi and the local vector Bi defined on xReduced into the global Hi and global Bi defined on X.
/// </summary>
/// <param name="constraint"> Constraint for which the global Hi and global Bi are computed. </param>
/// <param name="localHi"> Local Hi to globalise. </param>
/// <param name="localBi"> Local Bi to globalise. </param>
private protected void LocalToGlobal(Constraint constraint, CoordinateList localHi, Dictionary<int, double> localBi)
{
    *//******************** Translator for row and column indices ********************//*

    List<(VariableSet, int)> variables = constraint.GetVariables();

    // Create the translator
    List<int> translator = new();
    for (int i_Variable = 0; i_Variable < variables.Count; i_Variable++)
    {
        int firstRank = variables[i_Variable].Item1.FirstRank;
        int variableDimension = variables[i_Variable].Item1.VariableDimension;

        int variableIndex = variables[i_Variable].Item2;

        int startIndex = firstRank + (variableDimension * variableIndex);

        for (int i_Component = 0; i_Component < variableDimension; i_Component++)
        {
            translator.Add(startIndex + i_Component);
        }
    }

    *//******************** Create GlobalHi ********************//*

    Dictionary<(int, int), double> values = localHi._values;

    CoordinateList dok_GlobalHi = new();

    // Iterate on the keys (RowIndex, ColumnIndex) of the sparse matrix.
    foreach ((int, int) key in values.Keys) // *.Keys is an O(1) operation.
    {
        dok_GlobalHi.Add(values[key], translator[key.Item1], translator[key.Item2]);
    }

    constraint.GlobalHi = dok_GlobalHi;

    *//******************** Create GlobalBi ********************//*

    Dictionary<int, double> globalBi = new();

    // Iterate on the keys (RowIndex) of the sparse vector.
    foreach (int key in localBi.Keys)
    {
        globalBi.Add(translator[key], localBi[key]);
    }

    constraint.GlobalBi = globalBi;
}
*/