using System;
using System.Collections.Generic;

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
        internal protected IQuadraticConstraintType constraintType;

        /// <summary>
        /// Variables composing the local vector xReduced on which the <see cref="constraintType"/> is defined.
        /// </summary>
        /// <remarks> The first item corresponds to the variable set and the second to the index of the variable in the set. </remarks>
        internal protected List<(VariableSet Set, int Index)> variables;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value of the weight for the constraint.
        /// </summary>
        public double Weight { get; internal set; }

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
            this.constraintType = constraintType;
            this.variables = variables;

            // Initialize Properties
            Weight = weight;
        }

        #endregion
    }
}
