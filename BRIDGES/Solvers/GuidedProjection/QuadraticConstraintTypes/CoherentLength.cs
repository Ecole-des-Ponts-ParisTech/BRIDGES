using System;

using BRIDGES.LinearAlgebra.Vectors;
using BRIDGES.LinearAlgebra.Matrices;
using BRIDGES.LinearAlgebra.Matrices.Sparse;
using BRIDGES.Solvers.GuidedProjection.Interfaces;


namespace BRIDGES.Solvers.GuidedProjection.QuadraticConstraintTypes
{
    /// <summary>
    /// Constraint enforcing a scalar variable <em>l</em> to match with the distance between two point variables, <em>pi</em> and <em>pj</em>.
    /// </summary>
    /// <remarks> The vector xReduced = [pi, pj, l].</remarks>
    public class CoherentLength : IQuadraticConstraintType
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
        /// Initialises a new instance of the <see cref="CoherentLength"/> class.
        /// </summary>
        /// <param name="spaceDimension"> Dimension of the space containing the points. </param>
        public CoherentLength(int spaceDimension = 3)
        {
            /******************** Define LocalHi ********************/

            int[] columnPointers = new int[(2 * spaceDimension) + 2];
            int[] rowIndices = new int[(4 * spaceDimension) + 1];
            double[] values = new double[(4 * spaceDimension) + 1];

            columnPointers[0] = 0;
            for (int i_C = 0; i_C < spaceDimension; i_C++)
            {
                columnPointers[i_C + 1] = 2 * (i_C + 1);
                rowIndices[(2 * i_C)] = i_C; rowIndices[(2 * i_C) + 1] = spaceDimension + i_C;
                values[(2 * i_C)] = 2.0; values[(2 * i_C) + 1] = -2.0;

                columnPointers[spaceDimension + i_C + 1] = 2 * (spaceDimension + i_C + 1);
                rowIndices[(2 * (spaceDimension + i_C))] = i_C; rowIndices[(2 * (spaceDimension + i_C)) + 1] = spaceDimension + i_C;
                values[(2 * (spaceDimension + i_C))] = -2.0; values[(2 * (spaceDimension + i_C)) + 1] = 2.0;
            }

            columnPointers[(2 * spaceDimension) + 1] = (4 * spaceDimension) + 1;
            rowIndices[(4 * spaceDimension)] = (2 * spaceDimension);
            values[(4 * spaceDimension)] = -2.0;

            LocalHi = new CompressedColumn((2 * spaceDimension) + 1, (2 * spaceDimension) + 1, columnPointers, rowIndices, values);


            /******************** Define LocalBi ********************/

            LocalBi = null ;


            /******************** Define Ci ********************/
            Ci = 0.0;
        }

        #endregion
    }
}
