using System;
using System.Collections.Generic;

using BRIDGES.LinearAlgebra.Matrices.Storage;

using BRIDGES.Solvers.GuidedProjection.Interfaces;


namespace BRIDGES.Solvers.GuidedProjection.QuadraticConstraintTypes
{
    /// <summary>
    /// Constraint enforcing a value variable <em>l</em> to be lower than an upper bound <em>σ</em> using a dummy value variable <em>λ</em>.
    /// </summary>
    /// <remarks> The vector xReduced = [l, λ], and Ci = σ.</remarks>
    public class UpperBound : IQuadraticConstraintType
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
        /// Initialises a new instance of the <see cref="UpperBound"/> class.
        /// </summary>
        /// <param name="upperBound"> Value of the upper bound of the constraint. </param>
        public UpperBound(double upperBound)
        {
            /******************** Define LocalHi ********************/
            LocalHi = new DictionaryOfKeys();
            LocalHi.Add(-2.0, 1, 1);


            /******************** Define LocalBi ********************/

            LocalBi = new Dictionary<int, double>();
            LocalBi.Add(0, -1.0);


            /******************** Define Ci ********************/
            Ci = upperBound;
        }

        #endregion
    }
}
