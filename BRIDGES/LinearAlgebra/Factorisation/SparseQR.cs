using System;
using System.Reflection;
using System.Collections.Generic;

using BRIDGES.LinearAlgebra.Vectors;
using BRIDGES.LinearAlgebra.Matrices;
using BRIDGES.LinearAlgebra.Matrices.Sparse; 


namespace BRIDGES.LinearAlgebra.Factorisation
{
    /// <summary>
    /// Class describing the QR factorisation of a <see cref="CompressedColumn"/>
    /// </summary>
    public class SparseQR
    {
        #region Fields

        /// <summary>
        /// QR factorisation from <see cref="CSparse"/>.
        /// </summary>
        private CSparse.Double.Factorization.SparseQR _qr;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="SparseQR"/> class by computing the QR factorization of a <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="matrix"> Matrix over which the factorisation is performed. </param>
        public SparseQR(CompressedColumn matrix)
        {
            CSparse.Storage.CompressedColumnStorage<double> ccs = new CSparse.Double.SparseMatrix(matrix.RowCount, matrix.ColumnCount, matrix.Values(), matrix.RowIndices(), matrix.ColumnPointers());

            _qr = CSparse.Double.Factorization.SparseQR.Create(ccs, CSparse.ColumnOrdering.MinimumDegreeAtA);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Solves the linear system : <c>A·X=Y</c>
        /// </summary>
        /// <remarks> Since pre-factorisations are dedicated to speed, no exception handeling is provided.</remarks>
        /// <param name="vector"> <see cref="Vector"/> Y of the linear system. </param>
        /// <returns> The <see cref="Vector"/> X, solution of the system. </returns>
        public Vector Solve(Vector vector)
        {
            if (vector is DenseVector dense) { return Solve(dense); }
            else if (vector is SparseVector sparse) { return Solve(sparse); }
            else { throw new NotImplementedException($"The resolution of the linear system using a {vector.GetType()} as a {nameof(Vector)} is not implemented."); }
        }

        /// <summary>
        /// Solves the linear system : <c>A·X=Y</c>
        /// </summary>
        /// <remarks> Since pre-factorisations are dedicated to speed, no exception handeling is provided.</remarks>
        /// <param name="vector"> <see cref="DenseVector"/> Y of the linear system. </param>
        /// <returns> The <see cref="DenseVector"/> X, solution of the system. </returns>
        public DenseVector Solve(DenseVector vector)
        {
            double[] components = vector._components;
            double[] result = new double[vector.Size];

            _qr.Solve(components, result);

            return new DenseVector(result);
        }

        /// <summary>
        /// Solves the linear system : <c>A·X=Y</c>
        /// </summary>
        /// <remarks> Since pre-factorisations are dedicated to speed, no exception handeling is provided.</remarks>
        /// <param name="vector"> <see cref="SparseVector"/> Y of the linear system. </param>
        /// <returns> The <see cref="Vector"/> X, solution of the system. </returns>
        public SparseVector Solve(SparseVector vector)
        {
            int size = vector.Size;

            double[] components = vector.ToArray();
            double[] result = new double[size];

            _qr.Solve(components, result);

            SparseVector sparse = new SparseVector(size);
            for (int i_R = 0; i_R < size; i_R++)
            {
                if (result[i_R] != 0) { sparse[i_R] = result[i_R]; }
            }

            return sparse;
        }

        /// <summary>
        /// Solves the linear system : <c>A·X=Y</c>
        /// </summary>
        /// <remarks> Since pre-factorisations are dedicated to speed, no exception handeling is provided.</remarks>
        /// <param name="components"> <see cref="double"/>-precision array representing the Y vector of the linear system. </param>
        /// <returns> The <see cref="double"/>-precision array representing the X vector, solution of the system. </returns>
        public double[] Solve(double[] components)
        {
            double[] result = new double[components.Length];

            _qr.Solve(components, result);

            return result;
        }

        #endregion

        #region Other Methods

        /// <summary>
        /// Get the symbolic storage S of the current QR factorisation.
        /// </summary>
        /// <returns> The symbolic storage S. </returns>
        internal CSparse.Factorization.SymbolicFactorization GetS()
        {
            Type type = _qr.GetType();
            BindingFlags PRIVATE = BindingFlags.Instance | BindingFlags.NonPublic;

            FieldInfo fieldInfo = type.GetField("S", PRIVATE);

            return (CSparse.Factorization.SymbolicFactorization)fieldInfo.GetValue(_qr);
        }

        /// <summary>
        /// Gets the matrix R of the QR factorisation.
        /// </summary>
        /// <returns> The matrix R. </returns>
        internal CSparse.Double.SparseMatrix GetR()
        {
            Type type = _qr.GetType();
            BindingFlags PRIVATE = BindingFlags.Instance | BindingFlags.NonPublic;

            return (CSparse.Double.SparseMatrix)type.GetField("R", PRIVATE).GetValue(_qr);
        }

        /// <summary>
        /// Gets the matrix H containing the Householder vectors of the QR factorisation.
        /// </summary>
        /// <returns> The matrix H. </returns>
        internal CSparse.Double.SparseMatrix GetH()
        {
            Type type = _qr.GetType();
            BindingFlags PRIVATE = BindingFlags.Instance | BindingFlags.NonPublic;

            return (CSparse.Double.SparseMatrix)type.GetField("Q", PRIVATE).GetValue(_qr);
        }

        /// <summary>
        /// Get the inverse row permutation of the QR factorization.
        /// </summary>
        /// <returns> The inverse row permutation. </returns>
        internal int[] GetInverseRowPermutation()
        {
            CSparse.Factorization.SymbolicFactorization S = GetS();
            CSparse.Double.SparseMatrix H = GetH();

            int[] pInv = S.pinv;

            if (pInv.Length != H.RowCount)
            {
                int[] reducedPInv = new int[H.RowCount];
                for (int i_R = 0; i_R < reducedPInv.Length; i_R++)
                {
                    reducedPInv[i_R] = pInv[i_R];
                }
                pInv = reducedPInv;
            }

            return pInv;
        }

        /// <summary>
        /// Get the column permutation of the QR factorization.
        /// </summary>
        /// <returns> The column permutation. </returns>
        internal int[]  GetColumnPermutation()
        {
            CSparse.Factorization.SymbolicFactorization S = GetS();

            return S.q;
        }

        /// <summary>
        /// Gets the scaling factors for Householder vectors in H.
        /// </summary>
        /// <returns> The <see cref="double"/>-precision array containing the scaling factors. </returns>
        internal double[] GetBeta()
        {
            Type type = _qr.GetType();
            BindingFlags PRIVATE = BindingFlags.Instance | BindingFlags.NonPublic;

            return (double[])type.GetField("beta", PRIVATE).GetValue(_qr);
        }


        /// <summary>
        /// Compute the matrix Q of the QR factorisation.
        /// </summary>
        /// <returns> The matrix Q. </returns>
        /// <exception cref="ArgumentException"> Invalid dimensions. </exception>
        internal CSparse.Double.SparseMatrix ComputeQ()
        {
            CSparse.Double.SparseMatrix H = GetH();
            double[] beta = GetBeta();

            int rowCount = H.RowCount;
            int columnCount = H.ColumnCount;

            // Get the max number of non-zero component in a Householder column.
            var columnPointer = H.ColumnPointers;

            int maxNZ = 0;
            int startPointer = 0, endPointer;
            for (int i_C = 0; i_C < columnCount; i_C++)
            {
                endPointer = columnPointer[i_C + 1];
                maxNZ = Math.Max(endPointer - startPointer, maxNZ);
                startPointer = endPointer;
            }

            // Dyadic product will have at most nnz^2 entries.
            maxNZ = maxNZ * maxNZ;

            if (!CheckStorageRequirements(rowCount, columnCount, maxNZ))
            {
                //Console.WriteLine("Exit.");
            }

            // DyadicProduct assumes row indices to be sorted.
            CSparse.Helper.SortIndices(H);

            // Allocate all storage outside the loop.
            var E = CSparse.Double.SparseMatrix.CreateIdentity(rowCount);
            var D = new CSparse.Double.SparseMatrix(rowCount, rowCount, maxNZ);
            var P = new CSparse.Double.SparseMatrix(rowCount, rowCount, maxNZ);
            var Q = (CSparse.Double.SparseMatrix)E.Clone();

            var Qtemp = new CSparse.Double.SparseMatrix(rowCount, rowCount, maxNZ);

            // Disable auto-trim for matrix addition and multiplication.
            CSparse.Double.SparseMatrix.AutoTrimStorage = false;

            for (int i_C = 0; i_C < columnCount; i_C++)
            {
                DyadicProduct(i_C, H, ref D);

                maxNZ = E.NonZerosCount + D.NonZerosCount;

                if (maxNZ > Qtemp.Values.Length)
                {
                    Array.Resize(ref Qtemp.RowIndices, 2 * maxNZ);
                    Array.Resize(ref Qtemp.Values, 2 * maxNZ);
                }

                E.Add(1.0, - beta[i_C], D, Qtemp);

                Q.Multiply(Qtemp, P);

                #region Copy P to Q

                {
                    int m = P.RowCount;
                    int n = P.ColumnCount;
                    int nnz = P.NonZerosCount;

                    if (Q.RowCount != m || Q.ColumnCount != n)
                    {
                        throw new ArgumentException("Invalid dimensions.", nameof(Q));
                    }

                    if (Q.NonZerosCount < nnz)
                    {
                        Q.RowIndices = (int[])P.RowIndices.Clone();
                        Q.Values = (double[])P.Values.Clone();
                    }
                    else
                    {
                        Array.Copy(P.RowIndices, 0, Q.RowIndices, 0, nnz);
                        Array.Copy(P.Values, 0, Q.Values, 0, nnz);
                    }

                    P.ColumnPointers.CopyTo(Q.ColumnPointers, 0);
                }

                #endregion
            }

            return Q;
        }


        /// <summary>
        /// Evaluates whether the matrix will be sparse or not.
        /// </summary>
        /// <param name="rowCount"> Number of rows. </param>
        /// <param name="columnCount"> Number of columns.</param>
        /// <param name="NonZeroCount"> Number of non-zero components. </param>
        /// <returns></returns>
        private static bool CheckStorageRequirements(int rowCount, int columnCount, int NonZeroCount)
        {
            const int NZ_THRESHOLD = 100 * 100; // Matrix dimensions > 100 x 100

            if (NonZeroCount > NZ_THRESHOLD && NonZeroCount > rowCount * columnCount / 2)
            {
                Console.WriteLine("WARNING: matrix Q will be dense.");
            }

            const int MAX_BYTES = 200 * 1024 * 1024; // 200 MB

            return ((columnCount + 1) * 4 + NonZeroCount * 4 + NonZeroCount * 8) < MAX_BYTES;

        }

        /// <summary>
        /// Performs a dyadic product
        /// </summary>
        /// <param name="index">The current column index.</param>
        /// <param name="H">The Householder matrix.</param>
        /// <param name="Q"> The Q matrix of the QR factorization. </param>
        /// <remarks> Before calling this method :
        /// <list type="bullet">
        /// <item> The row indices of the Householder matrix <paramref name="H"/> have to be sorted (in increasing order). </item>
        /// <item> The output matrix <paramref name="Q"/> has to provide enough space to store the non-zero entries. </item>
        /// </list>
        /// </remarks>
        private static void DyadicProduct(int index, CSparse.Double.SparseMatrix H, ref CSparse.Double.SparseMatrix Q)
        {
            const double EPS = 1e-12;

            // The Householder matrix
            var hp = H.ColumnPointers;
            var hi = H.RowIndices;
            var hx = H.Values;

            // The Q matrix for current column of H.
            var qp = Q.ColumnPointers;
            var qi = Q.RowIndices;
            var qx = Q.Values;

            int columns = Q.ColumnCount;

            // Current non-zero count.
            int nz = 0;

            // Reset the column pointers.
            for (int j = 0; j < columns; j++)
            {
                qp[j] = 0;
            }

            int start = hp[index];
            int end = hp[index + 1];

            // Let v be the i-th column of the Householder Matrix (lower triangle).
            // Set the bottom-right submatrix of Q to v*v'.

            // Traverse v'.
            for (int j = start; j < end; j++)
            {
                // Current value of v'.
                double a = hx[j];

                if (Math.Abs(a) > EPS)
                {
                    // Traverse v.
                    for (int k = start; k < end; k++)
                    {
                        qi[nz] = hi[k];
                        qx[nz] = a * hx[k];

                        nz++;
                    }
                }

                // Store the nz count of current column.
                qp[hi[j] + 1] = nz;
            }

            qp[columns] = nz;

            // There might be gaps in the column indices of Q.
            nz = qp[index];
            for (int j = index + 1; j < columns; j++)
            {
                if (qp[j] > 0)
                {
                    nz = qp[j];
                }
                else
                {
                    qp[j] = nz;
                }
            }

        }

        #endregion
    }
}
