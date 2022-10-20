using System;
using System.Collections.Generic;

using BRIDGES.LinearAlgebra.Matrices.Storage;

using BRIDGES.Solvers.GuidedProjection.Interfaces;


namespace BRIDGES.Solvers.GuidedProjection.QuadraticConstraintTypes
{
    /// <summary>
    /// Constraint enforcing a scalar variable <em>l</em> to match with the distance between two point variables, <em>pi</em> and <em>pe</em>.
    /// </summary>
    /// <remarks> The vector xReduced = [pi, pj, l].</remarks>
    public class CoherentLength : IQuadraticConstraintType
    {
        #region Properties

        /// <inheritdoc cref="IQuadraticConstraintType.LocalHi"/>
        public DictionaryOfKeys LocalHi { get; }

        /// <inheritdoc cref="IQuadraticConstraintType.LocalBi"/>
        public Dictionary<int, double> LocalBi { get; }

        /// <inheritdoc cref="IQuadraticConstraintType.Ci"/>
        public double Ci { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="CoherentLength"/> class.
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

            LocalBi = null ;


            /******************** Define Ci ********************/
            Ci = 0.0;
        }

        #endregion
    }
}
