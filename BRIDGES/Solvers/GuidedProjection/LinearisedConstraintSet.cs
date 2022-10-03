using System;
using System.Collections.Generic;

using BRIDGES.LinearAlgebra.Matrices.Storage;
using BRIDGES.Solvers.GuidedProjection.Abstracts;
using BRIDGES.Solvers.GuidedProjection.Interfaces;


namespace BRIDGES.Solvers.GuidedProjection
{
    /// <summary>
    /// Class defining a set of linearised constraints for the <see cref="GuidedProjectionAlgorithm"/>.
    /// </summary>
    public class LinearisedConstraintSet : ConstraintSet<ILinearisedConstraintType>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearisedConstraintSet"/> class.
        /// </summary>
        /// <param name="constraintType"> Instance of a linearised constraint type for the set. </param>
        /// <param name="setIndex"> Index of the set in the <see cref="GuidedProjectionAlgorithm"/>'s list of constraint sets. </param>
        internal LinearisedConstraintSet(ILinearisedConstraintType constraintType, int setIndex) 
            : base(constraintType, setIndex)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearisedConstraintSet"/> class.
        /// </summary>
        /// <param name="constraintType"> Instance of a linearised constraint type for the set. </param>
        /// <param name="setIndex"> Index of the set in the <see cref="GuidedProjectionAlgorithm"/>'s list of constraint sets. </param>
        /// <param name="setCapacity"> Indicative number of linearised constraints that the set can initially store. </param>
        internal LinearisedConstraintSet(ILinearisedConstraintType constraintType, int setIndex, int setCapacity) 
            : base(constraintType, setIndex, setCapacity)
        { 
        }

        #endregion

        #region Methods

        /// <inheritdoc cref="IConstraintSet.ComputeAndGlobalise"/>
        internal override void ComputeAndGlobalise()
        {
            for (int i_Constraint = 0; i_Constraint < ConstraintCount; i_Constraint++)
            {
                Constraint linearisedConstraint = _constraints[i_Constraint];

                /******************** Compute the linearised constraint ********************/

                DictionaryOfKeys localHi; Dictionary<int, double> localBi;
                (localHi, localBi) = CalculateConstraint(linearisedConstraint);

                /******************** Converts from Local to Global ********************/

                List<int> converter = CreateConverter(linearisedConstraint.GetVariables());

                if (!(localHi is null) && localHi._values.Count != 0) 
                {
                    linearisedConstraint.GlobalHi = GlobaliseHi(localHi, converter); 
                }
                if (!(localBi is null) && localBi.Count != 0)
                {
                    linearisedConstraint.GlobalBi = GlobaliseBi(localBi, converter);
                }
            }
        }


        /// <summary>
        /// Gathers the components values of the local Hi's and local Bi's variables, an compute the local Hi, local Bi and Ci of the linearised constraint.
        /// </summary>
        /// <param name="linearisedConstraint"> Linearised constraint to calculate. </param>
        /// <returns> The linearised constraint's local Hi, local Bi, and Ci. </returns>
        private (DictionaryOfKeys, Dictionary<int, double>) CalculateConstraint(Constraint linearisedConstraint)
        {
            /******************** Get Components of xReduced  ********************/

            List<double> xReduced = new List<double>();

            List<(VariableSet, int)> variablesHi = linearisedConstraint.GetVariables();
            for (int i_Variable = 0; i_Variable < variablesHi.Count; i_Variable++)
            {
                VariableSet variableSet = variablesHi[i_Variable].Item1;
                int variableIndex = variablesHi[i_Variable].Item2;
                xReduced.AddRange(variableSet.GetVariable(variableIndex));
            }

            /******************** Compute Linearised Constraint ********************/

            return _constraintType.CalculateConstraint(xReduced.ToArray());
        }

        #endregion

    }
}
