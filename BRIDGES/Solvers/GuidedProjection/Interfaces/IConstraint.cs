using System;
using System.Collections.Generic;

using BRIDGES.LinearAlgebra.Matrices.Storage;


namespace BRIDGES.Solvers.GuidedProjection.Interfaces
{
    /// <summary>
    /// Interface defining a constraint for the <see cref="GuidedProjectionAlgorithm"/>.
    /// </summary>
    /// <remarks> The interface contains the constraint's information intended for the <see cref="GuidedProjectionAlgorithm"/>. </remarks>
    internal interface IConstraint
    {
        #region Properties

        /// <summary>
        /// Value of the weight for the constraint.
        /// </summary>
         double Weight { get; }


        /// <summary>
        /// Symmetric matrix Hi defined on X.
        /// </summary>
        DictionaryOfKeys GlobalHi { get; }

        /// <summary>
        /// Vector Bi defined on X.
        /// </summary>
        Dictionary<int, double> GlobalBi { get; }

        /// <summary>
        /// Value Ci for the constraint.
        /// </summary>
        double? Ci { get; }

        #endregion
    }
}
