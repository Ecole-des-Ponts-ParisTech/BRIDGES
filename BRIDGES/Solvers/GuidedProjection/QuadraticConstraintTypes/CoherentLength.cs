using System;
using System.Collections.Generic;

using BRIDGES.LinearAlgebra.Matrices.Storage;
using BRIDGES.Solvers.GuidedProjection.Interfaces;


namespace BRIDGES.Solvers.GuidedProjection.QuadraticConstraints
{
    /// <summary>
    /// Class defining a quadratic constraint type for the <see cref="GuidedProjectionAlgorithm"/>.<br/>
    /// The constraint enforces a length variable lij to match with the distance between two point variables (pi,pj).
    /// </summary>
    /// <remarks> The vector xReduced is (pi, pj, lij).</remarks>
    public class CoherentLength : IQuadraticConstraintType
    {
        #region Properties

        /// <inheritdoc cref="IQuadraticConstraintType.LocalHi"/>
        public DictionaryOfKeys LocalHi { get; }

        /// <inheritdoc cref="IQuadraticConstraintType.LocalBi"/>
        public Dictionary<int, double> LocalBi { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CoherentLength"/> class.
        /// </summary>
        /// <param name="spaceDimension"> Dimension of the space containing the points. </param>
        public CoherentLength(int spaceDimension = 3)
        {
            /******************** Define LocalHi ********************/
            LocalHi = new DictionaryOfKeys();
            for (int i = 0; i < spaceDimension; i++)
            {
                LocalHi.Add(2, i, i); LocalHi.Add(2, spaceDimension + i, spaceDimension + i);
                LocalHi.Add(-2, spaceDimension + i, i); LocalHi.Add(-2, i, spaceDimension + i);
            }
            LocalHi.Add(-2, 2 * spaceDimension, 2 * spaceDimension);

            /******************** Define LocalBi ********************/
            LocalBi = null;
        }

        #endregion
    }
}
