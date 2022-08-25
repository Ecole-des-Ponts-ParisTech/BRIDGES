using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.LinearAlgebra.Vectors;
using BRIDGES.LinearAlgebra.Matrices;
using BRIDGES.LinearAlgebra.Matrices.Sparse;
using Stor = BRIDGES.LinearAlgebra.Matrices.Storage;


namespace BRIDGES.Test.LinearAlgebra.Matrices
{
    /// <summary>
    /// Class testing the members of the <see cref="Matrix"/> class.
    /// </summary>
    [TestClass]
    public class MatrixTest
    {
        #region Behavior

        /// <summary>
        /// Tests that <see cref="Matrix"/> is reference type.
        /// </summary>
        [TestMethod("Behavior IsReference")]
        public void DenseMatrix_IsReference()
        {
            // Arrange
            Matrix denseMatrix = new DenseMatrix(2, 2, new double[] { 1.0, 2.0, 3.0, 4.0 });
            Matrix otherDenseMatrix = new DenseMatrix(1, 2, new double[] { 2.0, 4.0 });

            Matrix crsMatrix = new CompressedRow(2, 2, new int[3] { 0, 2, 4 }, new List<int> { 0, 1, 0, 1 }, new List<double> { 1.0, 2.0, 3.0, 4.0 });
            Matrix otherCrsMatrix = new CompressedRow(1, 2, new int[2] { 0, 2 }, new List<int> { 0, 1 }, new List<double> { 2.0, 4.0 });

            Matrix ccsMatrix = new CompressedColumn(2, 2, new int[3] { 0, 2, 4 }, new List<int> { 0, 1, 0, 1 }, new List<double> { 1.0, 3.0, 2.0, 4.0 });
            Matrix otherCcsMatrix = new CompressedColumn(1, 2, new int[3] { 0, 1, 2 }, new List<int> { 0, 1 }, new List<double> { 2.0, 4.0 });

            //Act
            otherDenseMatrix = denseMatrix;
            otherCrsMatrix = crsMatrix;
            otherCcsMatrix = ccsMatrix;

            // Assert
            Assert.AreEqual(denseMatrix, otherDenseMatrix);
            Assert.AreEqual(crsMatrix, otherCrsMatrix);
            Assert.AreEqual(ccsMatrix, otherCcsMatrix);

            Assert.AreSame(denseMatrix, otherDenseMatrix);
            Assert.AreSame(crsMatrix, otherCrsMatrix);
            Assert.AreSame(ccsMatrix, otherCcsMatrix);
        }

        #endregion


        #region Properties

        /// <summary>
        /// Tests the <see cref="DenseMatrix.RowCount"/> and <see cref="DenseMatrix.ColumnCount"/> properties.
        /// </summary>
        [TestMethod("Property RowCount & ColumnCount")]
        public void RowAndColumnCount()
        {
            // Arrange
            DenseMatrix denseMatrix = new DenseMatrix(2, 3, new double[] { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 });
            CompressedRow crsMatrix = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 });
            CompressedColumn ccsMatrix = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 1.0, 4.0, 2.0, 5.0, 3.0, 6.0 });

            // Act

            // Assert
            Assert.AreEqual(2, denseMatrix.RowCount);
            Assert.AreEqual(2, crsMatrix.RowCount);
            Assert.AreEqual(2, ccsMatrix.RowCount);

            Assert.AreEqual(3, denseMatrix.ColumnCount);
            Assert.AreEqual(3, crsMatrix.ColumnCount);
            Assert.AreEqual(3, ccsMatrix.ColumnCount);
        }

        /// <summary>
        /// Tests the <see cref="DenseMatrix"/> indexer property.
        /// </summary>
        [TestMethod("Property this[int,int]")]
        public void Index_Int_Int()
        {
            // Arrange
            DenseMatrix denseMatrix = new DenseMatrix(2, 3, new double[] { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 });
            CompressedRow crsMatrix = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 });
            CompressedColumn ccsMatrix = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 1.0, 4.0, 2.0, 5.0, 3.0, 6.0 });

            //Act

            // Assert
            Assert.AreEqual(1.0, denseMatrix[0, 0]); Assert.AreEqual(1.0, crsMatrix[0, 0]); Assert.AreEqual(1.0, ccsMatrix[0, 0]);
            Assert.AreEqual(2.0, denseMatrix[0, 1]); Assert.AreEqual(2.0, crsMatrix[0, 1]); Assert.AreEqual(2.0, ccsMatrix[0, 1]);
            Assert.AreEqual(3.0, denseMatrix[0, 2]); Assert.AreEqual(3.0, crsMatrix[0, 2]); Assert.AreEqual(3.0, ccsMatrix[0, 2]);
            Assert.AreEqual(4.0, denseMatrix[1, 0]); Assert.AreEqual(4.0, crsMatrix[1, 0]); Assert.AreEqual(4.0, ccsMatrix[1, 0]);
            Assert.AreEqual(5.0, denseMatrix[1, 1]); Assert.AreEqual(5.0, crsMatrix[1, 1]); Assert.AreEqual(5.0, ccsMatrix[1, 1]);
            Assert.AreEqual(6.0, denseMatrix[1, 2]); Assert.AreEqual(6.0, crsMatrix[1, 2]); Assert.AreEqual(6.0, ccsMatrix[1, 2]);
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Tests the static property <see cref="Matrix.Zero"/>.
        /// </summary>
        [TestMethod("Static Zero")]
        public void Static_Zero()
        {
            // Arrange

            // Act
            Matrix denseMatrix = DenseMatrix.Zero(5, 4);
            Matrix crsMatrix = CompressedRow.Zero(5, 4);
            Matrix ccsMatrix = CompressedColumn.Zero(5, 4);

            // Assert
            Assert.AreEqual(5, denseMatrix.RowCount);
            Assert.AreEqual(5, crsMatrix.RowCount);
            Assert.AreEqual(5, ccsMatrix.RowCount);

            Assert.AreEqual(4, denseMatrix.ColumnCount);
            Assert.AreEqual(4, crsMatrix.ColumnCount);
            Assert.AreEqual(4, ccsMatrix.ColumnCount);

            for (int r = 0; r < 5; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    Assert.AreEqual(0.0, denseMatrix[r, c]);
                    Assert.AreEqual(0.0, crsMatrix[r, c]);
                    Assert.AreEqual(0.0, ccsMatrix[r, c]);
                }
            }
        }

        /// <summary>
        /// Tests the static property <see cref="Matrix.Identity"/>.
        /// </summary>
        [TestMethod("Static Identity")]
        public void Static_Identity()
        {
            // Arrange

            // Act
            Matrix denseMatrix = DenseMatrix.Identity(4);
            Matrix crsMatrix = CompressedRow.Identity(4);
            Matrix ccsMatrix = CompressedColumn.Identity(4);

            // Assert
            Assert.AreEqual(4, denseMatrix.RowCount);
            Assert.AreEqual(4, crsMatrix.RowCount);
            Assert.AreEqual(4, ccsMatrix.RowCount);

            Assert.AreEqual(4, denseMatrix.ColumnCount);
            Assert.AreEqual(4, crsMatrix.ColumnCount);
            Assert.AreEqual(4, ccsMatrix.ColumnCount);

            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    if (r == c) 
                    { 
                        Assert.AreEqual(1.0, denseMatrix[r, c]); Assert.AreEqual(1.0, crsMatrix[r, c]); Assert.AreEqual(1.0, ccsMatrix[r, c]);
                    }
                    else 
                    { 
                        Assert.AreEqual(0.0, denseMatrix[r, c]); Assert.AreEqual(0.0, crsMatrix[r, c]); Assert.AreEqual(0.0, ccsMatrix[r, c]);
                    }
                }
            }
        }

        #endregion

        #region Static Methods

        /******************** Algebraic Additive Group ********************/

        /// <summary>
        /// Tests the static method <see cref="Matrix.Add(Matrix, Matrix)"/>.
        /// </summary>
        [TestMethod("Static Add(Matrix,Matrix)")]
        public void Static_Add_Matrix_Matrix()
        {
            // Arrange
            Matrix denseLeft = new DenseMatrix(2, 3, new double[] { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            Matrix denseRight = new DenseMatrix(2, 3, new double[] { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            Matrix crsLeft = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            Matrix crsRight = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            Matrix ccsLeft = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 1.0, 5.0, 2.0, 6.0, 3.0, 7.0 });
            Matrix ccsRight = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 5.0, 3.0, 4.0, 2.0, 3.0 });


            Matrix matrix = new DenseMatrix(2, 3, new double[] { 5.0, 5.0, 5.0, 10.0, 10.0, 10.0 });

            //Act
            Matrix DenseDenseMatrix = Matrix.Add(denseLeft, denseRight);
            Matrix DenseCrsMatrix = Matrix.Add(denseLeft, crsRight);
            Matrix DenseCcsMatrix = Matrix.Add(denseLeft, ccsRight);

            Matrix CrsCrsMatrix = Matrix.Add(crsLeft, crsRight);
            Matrix CrsCcsMatrix = Matrix.Add(crsLeft, ccsRight);
            Matrix CrsDenseMatrix = Matrix.Add(crsLeft, denseRight);

            Matrix CcsCcsMatrix = Matrix.Add(ccsLeft, ccsRight);
            Matrix CcsDenseMatrix = Matrix.Add(ccsLeft, denseRight);
            Matrix CcsCrsMatrix = Matrix.Add(ccsLeft, crsRight);

            // Assert
            Assert.AreEqual(matrix.RowCount, DenseDenseMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, DenseCrsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, DenseCcsMatrix.RowCount);

            Assert.AreEqual(matrix.RowCount, CrsCrsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, CrsCcsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, CrsDenseMatrix.RowCount);

            Assert.AreEqual(matrix.RowCount, CcsCcsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, CcsDenseMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, CcsCrsMatrix.RowCount);


            Assert.AreEqual(matrix.ColumnCount, DenseDenseMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, DenseCrsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, DenseCcsMatrix.ColumnCount);

            Assert.AreEqual(matrix.ColumnCount, CrsCrsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, CrsCcsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, CrsDenseMatrix.ColumnCount);

            Assert.AreEqual(matrix.ColumnCount, CcsCcsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, CcsDenseMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, CcsCrsMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - DenseDenseMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - DenseCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - DenseCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);

                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - CrsCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - CrsCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - CrsDenseMatrix[i_R, i_C]) < Settings.AbsolutePrecision);

                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - CcsCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - CcsDenseMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - CcsCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }

        }

        /// <summary>
        /// Tests the static method <see cref="Matrix.Subtract(Matrix, Matrix)"/>.
        /// </summary>
        [TestMethod("Static Subtract(Matrix,Matrix)")]
        public void Static_Subtract_Matrix_Matrix()
        {
            // Arrange
            Matrix denseLeft = new DenseMatrix(2, 3, new double[] { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            Matrix denseRight = new DenseMatrix(2, 3, new double[] { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            Matrix crsLeft = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            Matrix crsRight = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            Matrix ccsLeft = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 1.0, 5.0, 2.0, 6.0, 3.0, 7.0 });
            Matrix ccsRight = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 5.0, 3.0, 4.0, 2.0, 3.0 });


            Matrix matrix = new DenseMatrix(2, 3, new double[] { -3.0, -1.0, 1.0, 0.0, 2.0, 4.0 });

            //Act
            Matrix DenseDenseMatrix = Matrix.Subtract(denseLeft, denseRight);
            Matrix DenseCrsMatrix = Matrix.Subtract(denseLeft, crsRight);
            Matrix DenseCcsMatrix = Matrix.Subtract(denseLeft, ccsRight);

            Matrix CrsCrsMatrix = Matrix.Subtract(crsLeft, crsRight);
            Matrix CrsCcsMatrix = Matrix.Subtract(crsLeft, ccsRight);
            Matrix CrsDenseMatrix = Matrix.Subtract(crsLeft, denseRight);

            Matrix CcsCcsMatrix = Matrix.Subtract(ccsLeft, ccsRight);
            Matrix CcsDenseMatrix = Matrix.Subtract(ccsLeft, denseRight);
            Matrix CcsCrsMatrix = Matrix.Subtract(ccsLeft, crsRight);

            // Assert
            Assert.AreEqual(matrix.RowCount, DenseDenseMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, DenseCrsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, DenseCcsMatrix.RowCount);

            Assert.AreEqual(matrix.RowCount, CrsCrsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, CrsCcsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, CrsDenseMatrix.RowCount);

            Assert.AreEqual(matrix.RowCount, CcsCcsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, CcsDenseMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, CcsCrsMatrix.RowCount);


            Assert.AreEqual(matrix.ColumnCount, DenseDenseMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, DenseCrsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, DenseCcsMatrix.ColumnCount);

            Assert.AreEqual(matrix.ColumnCount, CrsCrsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, CrsCcsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, CrsDenseMatrix.ColumnCount);

            Assert.AreEqual(matrix.ColumnCount, CcsCcsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, CcsDenseMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, CcsCrsMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - DenseDenseMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - DenseCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - DenseCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);

                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - CrsCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - CrsCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - CrsDenseMatrix[i_R, i_C]) < Settings.AbsolutePrecision);

                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - CcsCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - CcsDenseMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - CcsCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }


        /******************** Algebraic Multiplicative SemiGroup ********************/

        /// <summary>
        /// Tests the static method <see cref="Matrix.Multiply(Matrix, Matrix)"/>.
        /// </summary>
        [TestMethod("Static Multiply(Matrix,Matrix)")]
        public void Static_Multiply_Matrix_Matrix()
        {
            // Arrange
            Matrix denseLeft = new DenseMatrix(6, 5, new double[] { 0.0, 1.5, 0.0, 1.25, 0.0,
              0.0, 0.0, 0.0, 6.75, 0.0, /**/ 0.0, 0.0, 2.0, 0.0, 0.0, /**/ 0.0, 0.0, 0.0, 5.5, 0.0,
              0.0, 4.0, 3.5, 2.25, 0.0, /**/ 0.0, 0.0, 0.0, 0.0, 7.25 });
            Matrix denseRight = new DenseMatrix(5, 3, new double[] { 3.5, 0.0, 0.0,
                  0.0, 1.5, 0.0, /**/ 5.0, 0.0, 0.0, /**/ 2.0, 3.0, 4.0, /**/ 0.5, 2.5, 0.0 });

            Matrix crsLeft = new CompressedRow(6, 5, new int[7] { 0, 2, 3, 4, 5, 8, 9 },
                new List<int> { 1, 3, 3, 2, 3, 1, 2, 3, 4 }, new List<double> { 1.5, 1.25, 6.75, 2.0, 5.5, 4.0, 3.5, 2.25, 7.25 });
            Matrix crsRight = new CompressedRow(5, 3, new int[6] { 0, 1, 2, 3, 6, 8 },
                new List<int> { 0, 1, 0, 0, 1, 2, 0, 1 }, new List<double> { 3.5, 1.5, 5.0, 2.0, 3.0, 4.0, 0.5, 2.5 });

            Matrix ccsLeft = new CompressedColumn(6, 5, new int[6] { 0, 0, 2, 4, 8, 9 },
                new List<int> { 0, 4, 2, 4, 0, 1, 3, 4, 5 }, new List<double> { 1.5, 4.0, 2.0, 3.5, 1.25, 6.75, 5.5, 2.25, 7.25 });
            Matrix ccsRight = new CompressedColumn(5, 3, new int[4] { 0, 4, 7, 8 },
                new List<int> { 0, 2, 3, 4, 1, 3, 4, 3 }, new List<double> { 3.5, 5.0, 2.0, 0.5, 1.5, 3.0, 2.5, 4.0 });


            CompressedRow matrix = new CompressedRow(6, 3, new int[7] { 0, 3, 6, 7, 10, 13, 15 },
                new List<int> { 0, 1, 2, 0, 1, 2, 0, 0, 1, 2, 0, 1, 2, 0, 1 },
                new List<double> { 2.5, 6.0, 5.0, 13.5, 20.25, 27.0, 10.0, 11.0, 16.5, 22.0, 22.0, 12.75, 9.0, 3.625, 18.125 });

            //Act
            Matrix DenseDenseMatrix = Matrix.Multiply(denseLeft, denseRight);
            Matrix DenseCrsMatrix = Matrix.Multiply(denseLeft, crsRight);
            Matrix DenseCcsMatrix = Matrix.Multiply(denseLeft, ccsRight);

            Matrix CrsCrsMatrix = Matrix.Multiply(crsLeft, crsRight);
            Matrix CrsCcsMatrix = Matrix.Multiply(crsLeft, ccsRight);
            Matrix CrsDenseMatrix = Matrix.Multiply(crsLeft, denseRight);

            Matrix CcsCcsMatrix = Matrix.Multiply(ccsLeft, ccsRight);
            Matrix CcsDenseMatrix = Matrix.Multiply(ccsLeft, denseRight);
            Matrix CcsCrsMatrix = Matrix.Multiply(ccsLeft, crsRight);

            // Assert
            Assert.AreEqual(matrix.RowCount, DenseDenseMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, DenseCrsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, DenseCcsMatrix.RowCount);

            Assert.AreEqual(matrix.RowCount, CrsCrsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, CrsCcsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, CrsDenseMatrix.RowCount);

            Assert.AreEqual(matrix.RowCount, CcsCcsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, CcsDenseMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, CcsCrsMatrix.RowCount);


            Assert.AreEqual(matrix.ColumnCount, DenseDenseMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, DenseCrsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, DenseCcsMatrix.ColumnCount);

            Assert.AreEqual(matrix.ColumnCount, CrsCrsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, CrsCcsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, CrsDenseMatrix.ColumnCount);

            Assert.AreEqual(matrix.ColumnCount, CcsCcsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, CcsDenseMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, CcsCrsMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - DenseDenseMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - DenseCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - DenseCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);

                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - CrsCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - CrsCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - CrsDenseMatrix[i_R, i_C]) < Settings.AbsolutePrecision);

                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - CcsCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - CcsDenseMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - CcsCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }


        /******************** Group Action ********************/



        #endregion
    }
}
