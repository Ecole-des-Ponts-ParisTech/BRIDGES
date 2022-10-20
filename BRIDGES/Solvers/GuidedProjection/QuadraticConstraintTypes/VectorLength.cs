using System;
using System.Collections.Generic;

using BRIDGES.LinearAlgebra.Matrices.Storage;
using BRIDGES.Solvers.GuidedProjection.Interfaces;


namespace BRIDGES.Solvers.GuidedProjection.QuadraticConstraintTypes
{
    /// <summary>
    /// Constraint enforcing a vector variable <em>v</em> to have a given length <em>l</em> (computed with euclidean norm).
    /// </summary>
    /// <remarks> The vector xReduced = [v], and Ci = l<sup>2</sup>.</remarks>
    public class VectorLength : IQuadraticConstraintType
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
        /// Initialises a new instance of the <see cref="VectorLength"/> class.
        /// </summary>
        /// <param name="targetLength"> Target length for the vector. </param>
        /// <param name="spaceDimension"> Dimension of the space containing the vector. </param>
        public VectorLength(double targetLength, int spaceDimension = 3)
        {
            /******************** Define LocalHi ********************/
            LocalHi = new DictionaryOfKeys();
            for (int i = 0; i < spaceDimension; i++) { LocalHi.Add(-2.0, spaceDimension, spaceDimension); }


            /******************** Define LocalBi ********************/
            LocalBi = null;


            /******************** Define Ci ********************/
            Ci = targetLength * targetLength;
        }

        #endregion
    }
}
