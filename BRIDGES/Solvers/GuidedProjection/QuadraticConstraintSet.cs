using System;
using System.Collections.Generic;

using BRIDGES.LinearAlgebra.Matrices.Storage;
using BRIDGES.Solvers.GuidedProjection.Abstracts;
using BRIDGES.Solvers.GuidedProjection.Interfaces;


namespace BRIDGES.Solvers.GuidedProjection
{
    /// <summary>
    /// Class defining a set of quadratic constraints for the <see cref="GuidedProjectionAlgorithm"/>.
    /// </summary>
    public class QuadraticConstraintSet : ConstraintSet<IQuadraticConstraintType>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QuadraticConstraintSet"/> class.
        /// </summary>
        /// <param name="constraintType"> Instance of a quadratic constraint type for the set. </param>
        /// <param name="setIndex"> Index of the set in the <see cref="GuidedProjectionAlgorithm"/>'s list of constraint sets. </param>
        internal QuadraticConstraintSet(IQuadraticConstraintType constraintType, int setIndex) : base(constraintType, setIndex)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuadraticConstraintSet"/> class.
        /// </summary>
        /// <param name="constraintType"> Instance of a quadratic constraint type for the set. </param>
        /// <param name="setIndex"> Index of the set in the <see cref="GuidedProjectionAlgorithm"/>'s list of constraint sets. </param>
        /// <param name="setCapacity"> Indicative number of quadratic constraints that the set can initially store. </param>
        internal QuadraticConstraintSet(IQuadraticConstraintType constraintType, int setIndex, int setCapacity) : base(constraintType, setIndex,  setCapacity)
        {
        }

        #endregion

        #region Methods

        /// <inheritdoc cref="IConstraintSet.ComputeAndGlobalise"/>
        internal override void ComputeAndGlobalise()
        {
            bool isLocalHiInstanciatedOnSet = !(_constraintType.LocalHi is null) && _constraintType.LocalHi._values.Count != 0;
            bool isLocalBiInstanciatedOnSet = !(_constraintType.LocalBi is null) && _constraintType.LocalBi.Count != 0;

            // Both localHi and local Bi are instanciated on the set.
            if (isLocalHiInstanciatedOnSet && isLocalBiInstanciatedOnSet)
            {
                for (int i_Constraint = 0; i_Constraint < ConstraintCount; i_Constraint++)
                {
                    Constraint quadraticConstraint = _constraints[i_Constraint];

                    List<int> converter = CreateConverter(quadraticConstraint.GetVariables());

                    quadraticConstraint.GlobalHi = GlobaliseHi(_constraintType.LocalHi, converter);
                    quadraticConstraint.GlobalBi = GlobaliseBi(_constraintType.LocalBi, converter);
                }
            }
            // Only localHi is instanciated on the set.
            else if (isLocalHiInstanciatedOnSet && !isLocalBiInstanciatedOnSet)
            {
                for (int i_Constraint = 0; i_Constraint < ConstraintCount; i_Constraint++)
                {
                    Constraint quadraticConstraint = _constraints[i_Constraint];

                    List<int> converter = CreateConverter(quadraticConstraint.GetVariables());

                    quadraticConstraint.GlobalHi = GlobaliseHi(_constraintType.LocalHi, converter);
                }
            }
            // Only localBi is instanciated on the set.
            else if (isLocalHiInstanciatedOnSet && !isLocalBiInstanciatedOnSet)
            {
                for (int i_Constraint = 0; i_Constraint < ConstraintCount; i_Constraint++)
                {
                    Constraint quadraticConstraint = _constraints[i_Constraint];

                    List<int> converter = CreateConverter(quadraticConstraint.GetVariables());

                    quadraticConstraint.GlobalBi = GlobaliseBi(_constraintType.LocalBi, converter);
                }
            }
        }

        #endregion
    }
}