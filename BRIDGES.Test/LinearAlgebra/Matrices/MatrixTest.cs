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
        public void Matrix_IsReference()
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
            Matrix denseMatrix = new DenseMatrix(2, 3, new double[] { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 });
            Matrix crsMatrix = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 });
            Matrix ccsMatrix = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
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
            Matrix denseMatrix = new DenseMatrix(2, 3, new double[] { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 });
            Matrix crsMatrix = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 });
            Matrix ccsMatrix = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
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

        /// <summary>
        /// Tests the static method <see cref="Matrix.Multiply(double, Matrix)"/>.
        /// </summary>
        [TestMethod("Static Multiply(double,Matrix)")]
        public void Static_Multiply_Double_Matrix()
        {
            // Arrange
            double factor = -2.5;
            Matrix denseOperand = new DenseMatrix(3, 2, new double[] { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            Matrix crsOperand = new CompressedRow(3, 2, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            Matrix ccsOperand = new CompressedColumn(3, 2, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 2.0, -4.0, 3.0, -5.0, 1.0 });

            DenseMatrix result = new DenseMatrix(3, 2, new double[] { -10.0, -7.5, -5.0, 12.5, 10.0, -2.5 });

            // Act
            Matrix denseMatrix = Matrix.Multiply(factor, denseOperand);
            Matrix crsMatrix = Matrix.Multiply(factor, crsOperand);
            Matrix ccsMatrix = Matrix.Multiply(factor, ccsOperand);

            // Assert
            Assert.AreEqual(result.RowCount, denseMatrix.RowCount);
            Assert.AreEqual(result.RowCount, crsMatrix.RowCount);
            Assert.AreEqual(result.RowCount, ccsMatrix.RowCount);

            Assert.AreEqual(result.ColumnCount, denseMatrix.ColumnCount);
            Assert.AreEqual(result.ColumnCount, crsMatrix.ColumnCount);
            Assert.AreEqual(result.ColumnCount, ccsMatrix.ColumnCount);

            for (int i_R = 0; i_R < result.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < result.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(result[i_R, i_C] - denseMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(result[i_R, i_C] - crsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(result[i_R, i_C] - ccsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="Matrix.Multiply(Matrix, double)"/>.
        /// </summary>
        [TestMethod("Static Multiply(Matrix,double)")]
        public void Static_Multiply_Matrix_Double()
        {
            // Arrange
            Matrix denseOperand = new DenseMatrix(3, 2, new double[] { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            Matrix crsOperand = new CompressedRow(3, 2, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            Matrix ccsOperand = new CompressedColumn(3, 2, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 2.0, -4.0, 3.0, -5.0, 1.0 });
            double factor = -2.5;

            DenseMatrix result = new DenseMatrix(3, 2, new double[] { -10.0, -7.5, -5.0, 12.5, 10.0, -2.5 });

            // Act
            Matrix denseMatrix = Matrix.Multiply(denseOperand, factor);
            Matrix crsMatrix = Matrix.Multiply(crsOperand, factor);
            Matrix ccsMatrix = Matrix.Multiply(ccsOperand, factor);

            // Assert
            Assert.AreEqual(result.RowCount, denseMatrix.RowCount);
            Assert.AreEqual(result.RowCount, crsMatrix.RowCount);
            Assert.AreEqual(result.RowCount, ccsMatrix.RowCount);

            Assert.AreEqual(result.ColumnCount, denseMatrix.ColumnCount);
            Assert.AreEqual(result.ColumnCount, crsMatrix.ColumnCount);
            Assert.AreEqual(result.ColumnCount, ccsMatrix.ColumnCount);

            for (int i_R = 0; i_R < result.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < result.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(result[i_R, i_C] - denseMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(result[i_R, i_C] - crsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(result[i_R, i_C] - ccsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }


        /// <summary>
        /// Tests the static method <see cref="Matrix.Divide(Matrix, double)"/>.
        /// </summary>
        [TestMethod("Static Divide(Matrix,double)")]
        public void Static_Divide_Matrix_Double()
        {
            // Arrange
            Matrix denseOperand = new DenseMatrix(2, 1, new double[] { 4.0, 3.0 });
            Matrix crsOperand = new CompressedRow(2, 1, new int[3] { 0, 1, 2 },
                new List<int> { 0, 0 }, new List<double> { 4.0, 3.0 });
            Matrix ccsOperand = new CompressedColumn(2, 1, new int[2] { 0, 2 },
                new List<int> { 0, 1 }, new List<double> { 4.0, 3.0 });
            double divisor = -2.0;

            DenseMatrix result = new DenseMatrix(2, 1, new double[] { -2.0, -1.5 });

            // Act
            Matrix denseMatrix = Matrix.Divide(denseOperand, divisor);
            Matrix crsMatrix = Matrix.Divide(crsOperand, divisor);
            Matrix ccsMatrix = Matrix.Divide(ccsOperand, divisor);

            // Assert
            Assert.AreEqual(result.RowCount, denseMatrix.RowCount);
            Assert.AreEqual(result.RowCount, crsMatrix.RowCount);
            Assert.AreEqual(result.RowCount, ccsMatrix.RowCount);

            Assert.AreEqual(result.ColumnCount, denseMatrix.ColumnCount);
            Assert.AreEqual(result.ColumnCount, crsMatrix.ColumnCount);
            Assert.AreEqual(result.ColumnCount, ccsMatrix.ColumnCount);

            for (int i_R = 0; i_R < result.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < result.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(result[i_R, i_C] - denseMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(result[i_R, i_C] - crsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(result[i_R, i_C] - ccsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }


        /******************** Other Operations ********************/

        /// <summary>
        /// Tests the static method <see cref="Matrix.Multiply(Matrix, Vector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(Matrix,Vector)")]
        public void Static_Multiply_Matrix_Vector()
        {
            // Arrange
            Matrix denseMatrix = new DenseMatrix(3, 2, new double[] { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            Matrix crsMatrix = new CompressedRow(3, 2, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            Matrix ccsMatrix = new CompressedColumn(3, 2, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 2.0, -4.0, 3.0, -5.0, 1.0 });

            Vector denseVector = new DenseVector(new double[2] { -2.0, 6.0 });
            Vector sparseVector = new SparseVector(2, new int[2] { 0, 1 }, new double[2] { -2.0, 6.0 });

            Vector result = new DenseVector(new double[3] { 10.0, -34.0, 14.0 });

            //Act
            Vector denseDenseVector = Matrix.Multiply(denseMatrix, denseVector);
            Vector crsDenseVector = Matrix.Multiply(crsMatrix, denseVector);
            Vector ccsDenseVector = Matrix.Multiply(ccsMatrix, denseVector);

            Vector denseSparseVector = Matrix.Multiply(denseMatrix, sparseVector);
            Vector crsSparseVector = Matrix.Multiply(crsMatrix, sparseVector);
            Vector ccsSparseVector = Matrix.Multiply(ccsMatrix, sparseVector);

            // Assert
            Assert.AreEqual(result.Size, denseDenseVector.Size);
            Assert.AreEqual(result.Size, crsDenseVector.Size);
            Assert.AreEqual(result.Size, ccsDenseVector.Size);

            Assert.AreEqual(result.Size, denseSparseVector.Size);
            Assert.AreEqual(result.Size, crsSparseVector.Size);
            Assert.AreEqual(result.Size, ccsSparseVector.Size);

            for (int i_R = 0; i_R < result.Size; i_R++)
            {
                Assert.IsTrue(Math.Abs(result[i_R] - denseDenseVector[i_R]) < Settings.AbsolutePrecision);
                Assert.IsTrue(Math.Abs(result[i_R] - crsDenseVector[i_R]) < Settings.AbsolutePrecision);
                Assert.IsTrue(Math.Abs(result[i_R] - ccsDenseVector[i_R]) < Settings.AbsolutePrecision);

                Assert.IsTrue(Math.Abs(result[i_R] - denseSparseVector[i_R]) < Settings.AbsolutePrecision);
                Assert.IsTrue(Math.Abs(result[i_R] - crsSparseVector[i_R]) < Settings.AbsolutePrecision);
                Assert.IsTrue(Math.Abs(result[i_R] - ccsSparseVector[i_R]) < Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="Matrix.Multiply(Matrix, DenseVector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(Matrix,DenseVector)")]
        public void Static_Multiply_Matrix_DenseVector()
        {
            // Arrange
            Matrix denseMatrix = new DenseMatrix(3, 2, new double[] { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            Matrix crsMatrix = new CompressedRow(3, 2, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            Matrix ccsMatrix = new CompressedColumn(3, 2, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 2.0, -4.0, 3.0, -5.0, 1.0 });

            DenseVector denseVector = new DenseVector(new double[2] { -2.0, 6.0 });

            DenseVector result = new DenseVector(new double[3] { 10.0, -34.0, 14.0 });

            //Act
            DenseVector denseDenseVector = Matrix.Multiply(denseMatrix, denseVector);
            DenseVector crsDenseVector = Matrix.Multiply(crsMatrix, denseVector);
            DenseVector ccsDenseVector = Matrix.Multiply(ccsMatrix, denseVector);

            // Assert
            Assert.AreEqual(result.Size, denseDenseVector.Size);
            Assert.AreEqual(result.Size, crsDenseVector.Size);
            Assert.AreEqual(result.Size, ccsDenseVector.Size);

            for (int i_R = 0; i_R < result.Size; i_R++)
            {
                Assert.IsTrue(Math.Abs(result[i_R] - denseDenseVector[i_R]) < Settings.AbsolutePrecision);
                Assert.IsTrue(Math.Abs(result[i_R] - crsDenseVector[i_R]) < Settings.AbsolutePrecision);
                Assert.IsTrue(Math.Abs(result[i_R] - ccsDenseVector[i_R]) < Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="Matrix.Multiply(Matrix, SparseVector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(Matrix,SparseVector)")]
        public void Static_Multiply_Matrix_SparseVector()
        {
            // Arrange
            Matrix denseMatrix = new DenseMatrix(3, 2, new double[] { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            Matrix crsMatrix = new CompressedRow(3, 2, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            Matrix ccsMatrix = new CompressedColumn(3, 2, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 2.0, -4.0, 3.0, -5.0, 1.0 });

            SparseVector sparseVector = new SparseVector(2, new int[2] { 0, 1 }, new double[2] { -2.0, 6.0 });

            SparseVector result = new SparseVector(3, new int[3] { 0, 1, 2 }, new double[3] { 10.0, -34.0, 14.0 });

            // Act
            Vector denseSparseVector = Matrix.Multiply(denseMatrix, sparseVector);
            Vector crsSparseVector = Matrix.Multiply(crsMatrix, sparseVector);
            Vector ccsSparseVector = Matrix.Multiply(ccsMatrix, sparseVector);


            // Assert
            Assert.AreEqual(result.Size, denseSparseVector.Size);
            Assert.AreEqual(result.Size, crsSparseVector.Size);
            Assert.AreEqual(result.Size, ccsSparseVector.Size);

            for (int i_R = 0; i_R < result.Size; i_R++)
            {
                Assert.IsTrue(Math.Abs(result[i_R] - denseSparseVector[i_R]) < Settings.AbsolutePrecision);
                Assert.IsTrue(Math.Abs(result[i_R] - crsSparseVector[i_R]) < Settings.AbsolutePrecision);
                Assert.IsTrue(Math.Abs(result[i_R] - ccsSparseVector[i_R]) < Settings.AbsolutePrecision);
            }
        }


        /// <summary>
        /// Tests the static method <see cref="Matrix.TransposeMultiply(Matrix, Vector)"/>.
        /// </summary>
        [TestMethod("Static TransposeMultiply(Matrix,Vector)")]
        public void Static_TransposeMultiply_Matrix_Vector()
        {
            // Arrange
            Matrix denseMatrix = new DenseMatrix(3, 2, new double[] { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            Matrix crsMatrix = new CompressedRow(3, 2, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            Matrix ccsMatrix = new CompressedColumn(3, 2, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 2.0, -4.0, 3.0, -5.0, 1.0 });

            Vector denseVector = new DenseVector(new double[3] { -2.0, 0.0, 6.0 });
            Vector sparseVector = new SparseVector(3, new int[2] { 0, 2 }, new double[2] { -2.0, 6.0 });

            DenseVector result = new DenseVector(new double[2] { -32.0, 0.0 });

            //Act
            Vector denseDenseVector = Matrix.TransposeMultiply(denseMatrix, denseVector);
            Vector crsDenseVector = Matrix.TransposeMultiply(crsMatrix, denseVector);
            Vector ccsDenseVector = Matrix.TransposeMultiply(ccsMatrix, denseVector);

            Vector denseSparseVector = Matrix.TransposeMultiply(denseMatrix, sparseVector);
            Vector crsSparseVector = Matrix.TransposeMultiply(crsMatrix, sparseVector);
            Vector ccsSparseVector = Matrix.TransposeMultiply(ccsMatrix, sparseVector);

            // Assert
            Assert.AreEqual(result.Size, denseDenseVector.Size);
            Assert.AreEqual(result.Size, crsDenseVector.Size);
            Assert.AreEqual(result.Size, ccsDenseVector.Size);

            Assert.AreEqual(result.Size, denseSparseVector.Size);
            Assert.AreEqual(result.Size, crsSparseVector.Size);
            Assert.AreEqual(result.Size, ccsSparseVector.Size);

            for (int i_R = 0; i_R < result.Size; i_R++)
            {
                Assert.IsTrue(Math.Abs(result[i_R] - denseDenseVector[i_R]) < Settings.AbsolutePrecision);
                Assert.IsTrue(Math.Abs(result[i_R] - crsDenseVector[i_R]) < Settings.AbsolutePrecision);
                Assert.IsTrue(Math.Abs(result[i_R] - ccsDenseVector[i_R]) < Settings.AbsolutePrecision);

                Assert.IsTrue(Math.Abs(result[i_R] - denseSparseVector[i_R]) < Settings.AbsolutePrecision);
                Assert.IsTrue(Math.Abs(result[i_R] - crsSparseVector[i_R]) < Settings.AbsolutePrecision);
                Assert.IsTrue(Math.Abs(result[i_R] - ccsSparseVector[i_R]) < Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="Matrix.TransposeMultiply(Matrix, DenseVector)"/>.
        /// </summary>
        [TestMethod("Static TransposeMultiply(Matrix,DenseVector)")]
        public void Static_TransposeMultiply_Matrix_DenseVector()
        {
            // Arrange
            Matrix denseMatrix = new DenseMatrix(3, 2, new double[] { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            Matrix crsMatrix = new CompressedRow(3, 2, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            Matrix ccsMatrix = new CompressedColumn(3, 2, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 2.0, -4.0, 3.0, -5.0, 1.0 });

            DenseVector denseVector = new DenseVector(new double[3] { -2.0, 0.0, 6.0 });

            DenseVector result = new DenseVector(new double[2] { -32.0, 0.0 });

            //Act
            DenseVector denseDenseVector = Matrix.TransposeMultiply(denseMatrix, denseVector);
            DenseVector crsDenseVector = Matrix.TransposeMultiply(crsMatrix, denseVector);
            DenseVector ccsDenseVector = Matrix.TransposeMultiply(ccsMatrix, denseVector);

            // Assert
            Assert.AreEqual(result.Size, denseDenseVector.Size);
            Assert.AreEqual(result.Size, crsDenseVector.Size);
            Assert.AreEqual(result.Size, ccsDenseVector.Size);

            for (int i_R = 0; i_R < result.Size; i_R++)
            {
                Assert.IsTrue(Math.Abs(result[i_R] - denseDenseVector[i_R]) < Settings.AbsolutePrecision);
                Assert.IsTrue(Math.Abs(result[i_R] - crsDenseVector[i_R]) < Settings.AbsolutePrecision);
                Assert.IsTrue(Math.Abs(result[i_R] - ccsDenseVector[i_R]) < Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="Matrix.TransposeMultiply(Matrix, SparseVector)"/>.
        /// </summary>
        [TestMethod("Static TransposeMultiply(Matrix,SparseVector)")]
        public void Static_TransposeMultiply_Matrix_SparseVector()
        {
            // Arrange
            Matrix denseMatrix = new DenseMatrix(3, 2, new double[] { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            Matrix crsMatrix = new CompressedRow(3, 2, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            Matrix ccsMatrix = new CompressedColumn(3, 2, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 2.0, -4.0, 3.0, -5.0, 1.0 });

            SparseVector sparseVector = new SparseVector(3, new int[2] { 0, 2 }, new double[2] { -2.0, 6.0 });

            DenseVector result = new DenseVector(new double[2] { -32.0, 0.0 });

            //Act
            Vector denseSparseVector = Matrix.TransposeMultiply(denseMatrix, sparseVector);
            Vector crsSparseVector = Matrix.TransposeMultiply(crsMatrix, sparseVector);
            Vector ccsSparseVector = Matrix.TransposeMultiply(ccsMatrix, sparseVector);

            // Assert
            Assert.AreEqual(result.Size, denseSparseVector.Size);
            Assert.AreEqual(result.Size, crsSparseVector.Size);
            Assert.AreEqual(result.Size, ccsSparseVector.Size);

            for (int i_R = 0; i_R < result.Size; i_R++)
            {
                Assert.IsTrue(Math.Abs(result[i_R] - denseSparseVector[i_R]) < Settings.AbsolutePrecision);
                Assert.IsTrue(Math.Abs(result[i_R] - crsSparseVector[i_R]) < Settings.AbsolutePrecision);
                Assert.IsTrue(Math.Abs(result[i_R] - ccsSparseVector[i_R]) < Settings.AbsolutePrecision);
            }
        }

        #endregion
    }
}
