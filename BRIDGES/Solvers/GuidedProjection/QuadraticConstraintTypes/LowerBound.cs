using System;
using System.Collections.Generic;

using BRIDGES.LinearAlgebra.Vectors;
using BRIDGES.LinearAlgebra.Matrices;
using BRIDGES.LinearAlgebra.Matrices.Sparse;
using BRIDGES.Solvers.GuidedProjection.Interfaces;


namespace BRIDGES.Solvers.GuidedProjection.QuadraticConstraintTypes
{
    /// <summary>
    /// Constraint enforcing a value variable <em>l</em> to be higher than a lower bound <em>σ</em> using a dummy value variable <em>λ</em>.
    /// </summary>
    /// <remarks> The vector xReduced = [l, λ], and Ci = σ.</remarks>
    public class LowerBound : IQuadraticConstraintType
    {
        #region Properties

        /// <inheritdoc cref="IQuadraticConstraintType.LocalHi"/>
        public SparseMatrix LocalHi { get; }

        /// <inheritdoc cref="IQuadraticConstraintType.LocalBi"/>
        public SparseVector LocalBi { get; }

        /// <inheritdoc cref="IQuadraticConstraintType.Ci"/>
        public double Ci { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="LowerBound"/> class.
        /// </summary>
        /// <param name="lowerBound"> Value of the lower bound of the constraint. </param>
        public LowerBound(double lowerBound)
        {
            /******************** Define LocalHi ********************/

            int[] columnPointers = new int[3];
            int[] rowIndices = new int[1];
            double[] values = new double[1];

            columnPointers[0] = 0;
            columnPointers[1] = 0;
            columnPointers[2] = 1; rowIndices[0] = 1; values[0] = 2.0;

            LocalHi = new CompressedColumn(2, 2, columnPointers, rowIndices, values);


            /******************** Define LocalBi ********************/

            Dictionary<int, double> components = new Dictionary<int, double>();
            components.Add(0, -1.0);

            LocalBi = new SparseVector(2, ref components);


            /******************** Define Ci ********************/
            Ci = lowerBound;
        }

        #endregion
    }
}
