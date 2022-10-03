using System;
using System.Collections.Generic;

using BRIDGES.LinearAlgebra.Matrices.Storage;
using BRIDGES.Solvers.GuidedProjection.Interfaces;


namespace BRIDGES.Solvers.GuidedProjection.QuadraticConstraints
{
    /// <summary>
    /// Class defining a quadratic constraint type for the <see cref="GuidedProjectionAlgorithm"/>.<br/>
    /// The constraint enforces a value variable lij to be lower than an upper bound l thanks to a dummy value variables μij.
    /// </summary>
    /// <remarks> The vector xReduced is (μij, lij), and Ci = l.</remarks>
    public class UpperBound : IQuadraticConstraintType
    {
        #region Properties

        /// <inheritdoc cref="IQuadraticConstraintType.LocalHi"/>
        public DictionaryOfKeys LocalHi { get; }

        /// <inheritdoc cref="IQuadraticConstraintType.LocalBi"/>
        public Dictionary<int, double> LocalBi { get; }

        /// <summary>
        /// Gets the upper bound of constraint.
        /// </summary>
        public double Ci { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UpperBound"/> class.
        /// </summary>
        public UpperBound()
        {
            /******************** Define LocalHi ********************/
            LocalHi = new DictionaryOfKeys();
            LocalHi.Add(-2, 0, 0);

            /******************** Define LocalBi ********************/
            LocalBi = new Dictionary<int, double> { { 1, -1 } }; ;
        }

        #endregion
    }
}
