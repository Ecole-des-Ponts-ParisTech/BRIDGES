using System;
using System.Collections.Generic;

using BRIDGES.LinearAlgebra.Vectors;

using BRIDGES.Solvers.GuidedProjection.Interfaces;


namespace BRIDGES.Solvers.GuidedProjection
{
    /// <summary>
    /// Class defining a linearised constraint for the <see cref="GuidedProjectionAlgorithm"/>.
    /// </summary>
    public sealed class LinearisedConstraint : QuadraticConstraint
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="LinearisedConstraint"/> class.
        /// </summary>
        /// <param name="constraintType"> Constraint type defining the constraint locally.  </param>
        /// <param name="variables"> Variables composing the reduced vector xReduced on which the local symmetric matrix Hi and the local vector Bi are defined.</param>
        /// <param name="weight"> Weight of the constraint. </param>
        internal LinearisedConstraint(ILinearisedConstraintType constraintType, List<(VariableSet, int)> variables, double weight)
        : base(constraintType, variables, weight)
        {
            /* Do nothing */
        }

        #endregion

        #region Other Methods

        /// <summary>
        /// Updates the local members (LocalHi, LocalBi) of the linearised constraint using x,
        /// </summary>
        /// <param name="x"> Global vector x at the current iteration. </param>
        internal void Update(DenseVector x)
        {
            double[] xReduced = GetXReduced(x);

            ILinearisedConstraintType linearisedConstraintType = constraintType as ILinearisedConstraintType;
            linearisedConstraintType.UpdateLocal(xReduced);
        }


        /// <summary>
        /// Retrieves the components of the local vector xReduced from its global equivalent x.
        /// </summary>
        /// <param name="x"> The global vector x. </param>
        /// <returns> the components of xReduced </returns>
        private double[] GetXReduced(in DenseVector x)
        {
            List<double> result = new List<double>();
            for (int i_LocalVariable = 0; i_LocalVariable < variables.Count; i_LocalVariable++)
            {
                VariableSet variableSet = variables[i_LocalVariable].Set;
                int variableIndex = variables[i_LocalVariable].Index;

                result.AddRange(variableSet.GetVariable(variableIndex));
            }

            return result.ToArray();
        }

        #endregion
    }
}
