using System;
using System.Collections.Generic;

using BRIDGES.LinearAlgebra.Matrices.Storage;
using BRIDGES.Solvers.GuidedProjection.Interfaces;


namespace BRIDGES.Solvers.GuidedProjection.QuadraticConstraints
{
    /// <summary>
    /// Class defining a quadratic constraint type for the <see cref="GuidedProjectionAlgorithm"/>.<br/>
    /// The constraint enforces a vector variable v to have a length given l with respect to the euclidean norm.
    /// </summary>
    /// <remarks> The vector xReduced is (v) and Ci = l.</remarks>
    public class VectorLength : IQuadraticConstraintType
    {
        #region Properties

        /// <inheritdoc cref="IQuadraticConstraintType.LocalHi"/>
        public DictionaryOfKeys LocalHi { get; }

        /// <inheritdoc cref="IQuadraticConstraintType.LocalBi"/>
        public Dictionary<int, double> LocalBi { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorLength"/> class.
        /// </summary>
        /// <param name="spaceDimension">Dimension of the space containing the vector.</param>
        public VectorLength(int spaceDimension = 3)
        {
            /******************** Define LocalHi ********************/
            LocalHi = new DictionaryOfKeys();
            for (int i = 0; i < spaceDimension; i++) { LocalHi.Add(-2, spaceDimension, spaceDimension); }

            /******************** Define LocalBi ********************/
            LocalBi = null;
        }

        #endregion
    }
}
