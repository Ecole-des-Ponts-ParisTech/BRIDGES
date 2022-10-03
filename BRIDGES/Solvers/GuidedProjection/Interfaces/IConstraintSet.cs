using System;
using System.Collections.Generic;


namespace BRIDGES.Solvers.GuidedProjection.Interfaces
{
    /// <summary>
    /// Interface defining a set of constraints for the <see cref="GuidedProjectionAlgorithm"/>.
    /// </summary>
    /// <remarks> The interface contains the constraint set's information intended for the <see cref="GuidedProjectionAlgorithm"/>. </remarks>
    internal interface IConstraintSet
    {
        #region Properties

        /// <summary>
        /// Gets the index of the set in the <see cref="GuidedProjectionAlgorithm"/>'s list of constraint sets.
        /// </summary>
        int SetIndex { get; }

        /// <summary>
        /// Gets the number of constraints in the set. 
        /// </summary>
        int ConstraintCount { get; }


        /// <summary>
        /// Gets the constraint at the given index in the set.
        /// </summary>
        /// <param name="index"> Index of the constraint in the set. </param>
        /// <returns> The constraint a the given index. </returns>
        IConstraint this[int index] { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Computes the local symmetric matrix Hi and the local vector Bi defined on xReduced.<br/>
        /// and translates them into the global Hi and global Bi defined on X.
        /// </summary>
        void ComputeAndGlobalise();

        #endregion
    }
}

