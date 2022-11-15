using System;
using System.Collections.Generic;


namespace BRIDGES.Solvers.GuidedProjection.Interfaces
{
    /// <summary>
    /// Interface defining a linearised constraint type for the <see cref="GuidedProjectionAlgorithm"/>.
    /// </summary>
    public interface ILinearisedConstraintType : IQuadraticConstraintType
    {
        #region Methods

        /// <summary>
        /// Updates the local members (LocalHi, LocalBi) of the linearised constraint using xReduced.
        /// </summary>
        /// <param name="xReduced"> Actualized components of the local vector xReduced formed from the constraint variables. </param>
        void UpdateLocal(double[] xReduced);

        #endregion
    }
}
