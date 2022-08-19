using System;


namespace BRIDGES.LinearAlgebra.Matrices
{
    /// <summary>
    /// Class defining a sparse matrix.
    /// </summary>
    public abstract class SparseMatrix : Matrix
    {
        #region Properties

        /// <inheritdoc/>
        public override int RowCount { get; }

        /// <inheritdoc/>
        public override int ColumnCount { get; }

        /// <summary>
        /// Gets the number of non-zero values in the current sparse matrix.
        /// </summary>
        public int NonZeroCount { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="DenseMatrix"/> class.
        /// </summary>
        private SparseMatrix()
            : base()
        {
            /* Do nothing */
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SparseMatrix"/> class by defining its number of row and column.
        /// </summary>
        /// <param name="rowCount"> Number of rows of the <see cref="SparseMatrix"/>. </param>
        /// <param name="columnCount"> Number of columns of the <see cref="SparseMatrix"/>. </param>
        public SparseMatrix(int rowCount, int columnCount)
        {
            NonZeroCount = 0;

            RowCount = rowCount;
            ColumnCount = columnCount;
        }

        #endregion
    }
}
