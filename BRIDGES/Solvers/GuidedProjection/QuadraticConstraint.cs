using System;
using System.Collections.Generic;

using BRIDGES.LinearAlgebra.Vectors;
using BRIDGES.LinearAlgebra.Matrices;
using BRIDGES.LinearAlgebra.Matrices.Sparse;
using BRIDGES.LinearAlgebra.Matrices.Storage;

using BRIDGES.Solvers.GuidedProjection.Interfaces;

namespace BRIDGES.Solvers.GuidedProjection
{
    /// <summary>
    /// Class defining a quadratic constraint for the <see cref="GuidedProjectionAlgorithm"/>.
    /// </summary>
    public class QuadraticConstraint
    {
        #region Fields

        /// /// <summary>
        /// Constraint type defining the reduced matrix <see cref="IQuadraticConstraintType.LocalHi"/>,
        /// the reduced vector <see cref="IQuadraticConstraintType.LocalBi"/> and the scalar value <see cref="IQuadraticConstraintType.Ci"/>.
        /// </summary>
        protected IQuadraticConstraintType _constraintType { get; }

        /// <summary>
        /// Variables composing the local vector xReduced on which the <see cref="_constraintType"/> is defined.
        /// </summary>
        /// <remarks> The first item corresponds to the variable set and the second to the index of the variable in the set. </remarks>
        protected List<(VariableSet, int)> _variables;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value of the weight for the constraint.
        /// </summary>
        public double Weight { get; internal set; }


        /// <summary>
        /// Gets the symmetric matrix Hi defined on x.
        /// </summary>
        public CompressedColumn GlobalHi { get; internal set; }

        /// <summary>
        /// Gets the vector Bi defined on x.
        /// </summary>
        public SparseVector GlobalBi { get; internal set; }

        /// <summary>
        /// Gets the scalar value Ci of the constraint.
        /// </summary>
        public double Ci
        {
            get { return _constraintType.Ci; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="QuadraticConstraint"/> class.
        /// </summary>
        /// <param name="constraintType"> Constraint type defining the constraint locally.  </param>
        /// <param name="variables"> Variables composing the reduced vector xReduced on which the local symmetric matrix Hi and the local vector Bi are defined.</param>
        /// <param name="weight"> Weight of the constraint. </param>
        internal QuadraticConstraint(IQuadraticConstraintType constraintType, List<(VariableSet, int)> variables, double weight)
        {
            // Initialize Properties
            _constraintType = constraintType;
            _variables = variables;

            // Initialize Properties
            Weight = weight;
        }

        #endregion

        #region Other Methods

        /// <summary>
        /// Translates the local members of the constraint, defined on xReduced, into the global members defined on x.
        /// </summary>
        /// <param name="size"> Number of component of the global vector x. </param>
        internal void Complete(int size)
        {
            List<int> converter = CreateConverter(_variables);

            if (!(_constraintType.LocalHi is null) && _constraintType.LocalHi.NonZerosCount != 0)
            {
                CompleteHi(size, converter);
            }
            if (!(_constraintType.LocalBi is null) && _constraintType.LocalBi.NonZerosCount != 0)
            {
                CompleteBi(size, converter);
            }
        }


        /// <summary>
        /// Creates a converter translating the local indices of a constraint defined on xReduced into global indices defined on X.
        /// </summary>
        /// <param name="variables"> Variables composing the local vector xReduced of the constraint. </param>
        /// <returns> The converter for the globalisation of the constraint. </returns>
        private protected List<int> CreateConverter(List<(VariableSet, int)> variables)
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
        /// Translates the local Hi defined on xReduced, into the global Hi defined on x.
        /// </summary>
        /// <param name="size"> Number of component of the global vector x. </param>
        /// <param name="converter"> Converter translating Hi's row and column indices. </param>
        /// <returns> The global symmetric matrix Hi defined on x. </returns>
        private protected void CompleteHi(int size, List<int> converter)
        {
            int nonZerosCount = _constraintType.LocalHi.NonZerosCount;
            DictionaryOfKeys dok_GlobalHi = new DictionaryOfKeys();

            foreach(var component in _constraintType.LocalHi.GetNonZeros())
            {
                dok_GlobalHi.Add(component.Value, component.RowIndex, component.ColumnIndex);
            }

            GlobalHi = new CompressedColumn(size, size, dok_GlobalHi);
        }

        /// <summary>
        /// Translates the local Bi defined on xReduced, into the global Bi defined on x.
        /// </summary>
        /// <param name="size"> Number of component of the global vector x. </param>
        /// <param name="converter"> Converter translating Bi's row indices. </param>
        /// <returns> The global vector Bi defined on X. </returns>
        private protected void CompleteBi(int size, List<int> converter)
        {
            int nonZerosCount = _constraintType.LocalBi.NonZerosCount;
            Dictionary<int, double> components = new Dictionary<int, double>(nonZerosCount);

            foreach (var component in _constraintType.LocalBi.GetNonZeros())
            {
                components.Add(converter[component.RowIndex], component.Value);
            }

            GlobalBi = new SparseVector(size, ref components);
        }

        #endregion
    }
}
