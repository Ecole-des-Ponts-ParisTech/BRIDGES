using System;
using System.Collections.Generic;

using BRIDGES.LinearAlgebra.Matrices.Storage;


namespace BRIDGES.Solvers.GuidedProjection.Interfaces
{

    /// <summary>
    /// Interface defining a linearised constraint type for the <see cref="GuidedProjectionAlgorithm"/>
    /// </summary>
    public interface ILinearisedConstraintType
    {
        #region Methods

        /// <summary>
        /// Defines the process to calcualte the linearised constraint.
        /// </summary>
        /// <param name="xReduced"> Components of the local vector xReduced formed from the constraint variables.</param>
        /// <returns> The linearised constraint's local Hi, local Bi.</returns>
        (DictionaryOfKeys, Dictionary<int,double>) CalculateConstraint(double[] xReduced);

        #endregion
    }
}
