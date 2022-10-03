using System;
using System.Collections.Generic;

using BRIDGES.LinearAlgebra.Matrices.Storage;
using BRIDGES.Solvers.GuidedProjection.Interfaces;


namespace BRIDGES.Solvers.GuidedProjection.QuadraticConstraints
{
    /// <summary>
    /// Class defining a quadratic constraint type for the <see cref="GuidedProjectionAlgorithm"/>.<br/>
    /// The constraint enforces a segment defined from two point variables (pi,pj) to be orthogonal to a vector variable v.
    /// </summary>
    /// <remarks> The vector xReduced is (pi, pj, v).</remarks>
    public class SegmentOrthogonality : IQuadraticConstraintType
    {
        #region Properties

        /// <inheritdoc cref="IQuadraticConstraintType.LocalHi"/>
        public DictionaryOfKeys LocalHi { get; }

        /// <inheritdoc cref="IQuadraticConstraintType.LocalBi"/>
        public Dictionary<int, double> LocalBi { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentOrthogonality"/> class.
        /// </summary>
        /// <param name="spaceDimension">Dimension of the space containing the points and vector.</param>
        public SegmentOrthogonality(int spaceDimension = 3)
        {
            /******************** Define LocalHi ********************/

            LocalHi = new DictionaryOfKeys();
            for (int i = 0; i < spaceDimension; i++)
            {
                LocalHi.Add(1, i, spaceDimension + i); LocalHi.Add(-1, i, (2 * spaceDimension) + i);
                LocalHi.Add(-1, (2 * spaceDimension) + i, i); LocalHi.Add(1, (2 * spaceDimension) + i, spaceDimension + i);
            }

            /******************** Define LocalBi ********************/
            LocalBi = null;
        }

        #endregion
    }
}
