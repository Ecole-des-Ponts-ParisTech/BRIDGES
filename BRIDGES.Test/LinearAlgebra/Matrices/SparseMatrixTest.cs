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
    /// Class testing the members of the <see cref="SparseMatrix"/> class.
    /// </summary>
    [TestClass]
    public class SparseMatrixTest
    {
        #region Behavior

        /// <summary>
        /// Tests that <see cref="SparseMatrix"/> is reference type.
        /// </summary>
        [TestMethod("Behavior IsReference")]
        public void SparseMatrix_IsReference()
        {
            // Arrange
            Matrix crsMatrix = new CompressedRow(2, 2, new int[3] { 0, 2, 4 }, new List<int> { 0, 1, 0, 1 }, new List<double> { 1.0, 2.0, 3.0, 4.0 });
            Matrix otherCrsMatrix = new CompressedRow(1, 2, new int[2] { 0, 2 }, new List<int> { 0, 1 }, new List<double> { 2.0, 4.0 });

            Matrix ccsMatrix = new CompressedColumn(2, 2, new int[3] { 0, 2, 4 }, new List<int> { 0, 1, 0, 1 }, new List<double> { 1.0, 3.0, 2.0, 4.0 });
            Matrix otherCcsMatrix = new CompressedColumn(1, 2, new int[3] { 0, 1, 2 }, new List<int> { 0, 1 }, new List<double> { 2.0, 4.0 });

            //Act
            otherCrsMatrix = crsMatrix;
            otherCcsMatrix = ccsMatrix;

            // Assert
            Assert.AreEqual(crsMatrix, otherCrsMatrix);
            Assert.AreEqual(ccsMatrix, otherCcsMatrix);

            Assert.AreSame(crsMatrix, otherCrsMatrix);
            Assert.AreSame(ccsMatrix, otherCcsMatrix);
        }

        #endregion


        #region Properties

        /// <summary>
        /// Tests the <see cref="Matrix.RowCount"/> and <see cref="Matrix.ColumnCount"/> properties.
        /// </summary>
        [TestMethod("Property RowCount & ColumnCount")]
        public void RowAndColumnCount()
        {
            // Arrange
            SparseMatrix crsMatrix = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 });
            SparseMatrix ccsMatrix = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 1.0, 4.0, 2.0, 5.0, 3.0, 6.0 });

            // Act

            // Assert
            Assert.AreEqual(2, crsMatrix.RowCount);
            Assert.AreEqual(2, ccsMatrix.RowCount);

            Assert.AreEqual(3, crsMatrix.ColumnCount);
            Assert.AreEqual(3, ccsMatrix.ColumnCount);
        }

        /// <summary>
        /// Tests the <see cref="SparseMatrix"/> indexer property.
        /// </summary>
        [TestMethod("Property this[int,int]")]
        public void Index_Int_Int()
        {
            // Arrange
            SparseMatrix crsMatrix = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 });
            SparseMatrix ccsMatrix = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 1.0, 4.0, 2.0, 5.0, 3.0, 6.0 });

            //Act

            // Assert
            Assert.AreEqual(1.0, crsMatrix[0, 0]); Assert.AreEqual(1.0, ccsMatrix[0, 0]);
            Assert.AreEqual(2.0, crsMatrix[0, 1]); Assert.AreEqual(2.0, ccsMatrix[0, 1]);
            Assert.AreEqual(3.0, crsMatrix[0, 2]); Assert.AreEqual(3.0, ccsMatrix[0, 2]);
            Assert.AreEqual(4.0, crsMatrix[1, 0]); Assert.AreEqual(4.0, ccsMatrix[1, 0]);
            Assert.AreEqual(5.0, crsMatrix[1, 1]); Assert.AreEqual(5.0, ccsMatrix[1, 1]);
            Assert.AreEqual(6.0, crsMatrix[1, 2]); Assert.AreEqual(6.0, ccsMatrix[1, 2]);
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Tests the static property <see cref="SparseMatrix.Zero"/>.
        /// </summary>
        [TestMethod("Static Zero")]
        public void Static_Zero()
        {
            // Arrange

            // Act
            SparseMatrix crsMatrix = CompressedRow.Zero(5, 4);
            SparseMatrix ccsMatrix = CompressedColumn.Zero(5, 4);

            // Assert
            Assert.AreEqual(5, crsMatrix.RowCount);
            Assert.AreEqual(5, ccsMatrix.RowCount);

            Assert.AreEqual(4, crsMatrix.ColumnCount);
            Assert.AreEqual(4, ccsMatrix.ColumnCount);

            for (int r = 0; r < 5; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    Assert.AreEqual(0.0, crsMatrix[r, c]);
                    Assert.AreEqual(0.0, ccsMatrix[r, c]);
                }
            }
        }

        /// <summary>
        /// Tests the static property <see cref="SparseMatrix.Identity"/>.
        /// </summary>
        [TestMethod("Static Identity")]
        public void Static_Identity()
        {
            // Arrange

            // Act
            SparseMatrix crsMatrix = CompressedRow.Identity(4);
            SparseMatrix ccsMatrix = CompressedColumn.Identity(4);

            // Assert
            Assert.AreEqual(4, crsMatrix.RowCount);
            Assert.AreEqual(4, ccsMatrix.RowCount);

            Assert.AreEqual(4, crsMatrix.ColumnCount);
            Assert.AreEqual(4, ccsMatrix.ColumnCount);

            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    if (r == c)
                    {
                        Assert.AreEqual(1.0, crsMatrix[r, c]); Assert.AreEqual(1.0, ccsMatrix[r, c]);
                    }
                    else
                    {
                        Assert.AreEqual(0.0, crsMatrix[r, c]); Assert.AreEqual(0.0, ccsMatrix[r, c]);
                    }
                }
            }
        }

        #endregion

        #region Static Methods

        /******************** Algebraic Additive Group ********************/

        /// <summary>
        /// Tests the static method <see cref="SparseMatrix.Add(SparseMatrix, SparseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Add(SparseMatrix,SparseMatrix)")]
        public void Static_Add_SparseMatrix_SparseMatrix()
        {
            // Arrange
            SparseMatrix crsLeft = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            SparseMatrix crsRight = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            SparseMatrix ccsLeft = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 1.0, 5.0, 2.0, 6.0, 3.0, 7.0 });
            SparseMatrix ccsRight = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 5.0, 3.0, 4.0, 2.0, 3.0 });


            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { 5.0, 5.0, 5.0, 10.0, 10.0, 10.0 });

            //Act
            SparseMatrix CrsCrsMatrix = SparseMatrix.Add(crsLeft, crsRight);
            SparseMatrix CrsCcsMatrix = SparseMatrix.Add(crsLeft, ccsRight);

            SparseMatrix CcsCcsMatrix = SparseMatrix.Add(ccsLeft, ccsRight);
            SparseMatrix CcsCrsMatrix = SparseMatrix.Add(ccsLeft, crsRight);

            // Assert
            Assert.AreEqual(matrix.RowCount, CrsCrsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, CrsCcsMatrix.RowCount);

            Assert.AreEqual(matrix.RowCount, CcsCcsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, CcsCrsMatrix.RowCount);


            Assert.AreEqual(matrix.ColumnCount, CrsCrsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, CrsCcsMatrix.ColumnCount);

            Assert.AreEqual(matrix.ColumnCount, CcsCcsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, CcsCrsMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - CrsCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - CrsCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);

                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - CcsCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - CcsCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }

        }

        /// <summary>
        /// Tests the static method <see cref="SparseMatrix.Subtract(SparseMatrix, SparseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Subtract(SparseMatrix,SparseMatrix)")]
        public void Static_Subtract_SparseMatrix_SparseMatrix()
        {
            // Arrange
            SparseMatrix crsLeft = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            SparseMatrix crsRight = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            SparseMatrix ccsLeft = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 1.0, 5.0, 2.0, 6.0, 3.0, 7.0 });
            SparseMatrix ccsRight = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 5.0, 3.0, 4.0, 2.0, 3.0 });


            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { -3.0, -1.0, 1.0, 0.0, 2.0, 4.0 });

            //Act
            SparseMatrix CrsCrsMatrix = SparseMatrix.Subtract(crsLeft, crsRight);
            SparseMatrix CrsCcsMatrix = SparseMatrix.Subtract(crsLeft, ccsRight);

            SparseMatrix CcsCcsMatrix = SparseMatrix.Subtract(ccsLeft, ccsRight);
            SparseMatrix CcsCrsMatrix = SparseMatrix.Subtract(ccsLeft, crsRight);

            // Assert
            Assert.AreEqual(matrix.RowCount, CrsCrsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, CrsCcsMatrix.RowCount);

            Assert.AreEqual(matrix.RowCount, CcsCcsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, CcsCrsMatrix.RowCount);

            Assert.AreEqual(matrix.ColumnCount, CrsCrsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, CrsCcsMatrix.ColumnCount);

            Assert.AreEqual(matrix.ColumnCount, CcsCcsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, CcsCrsMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - CrsCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - CrsCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);

                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - CcsCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - CcsCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }


        /******************** Algebraic Multiplicative SemiGroup ********************/

        /// <summary>
        /// Tests the static method <see cref="SparseMatrix.Multiply(SparseMatrix, SparseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Multiply(SparseMatrix,SparseMatrix)")]
        public void Static_Multiply_SparseMatrix_SparseMatrix()
        {
            // Arrange
            SparseMatrix crsLeft = new CompressedRow(6, 5, new int[7] { 0, 2, 3, 4, 5, 8, 9 },
                new List<int> { 1, 3, 3, 2, 3, 1, 2, 3, 4 }, new List<double> { 1.5, 1.25, 6.75, 2.0, 5.5, 4.0, 3.5, 2.25, 7.25 });
            SparseMatrix crsRight = new CompressedRow(5, 3, new int[6] { 0, 1, 2, 3, 6, 8 },
                new List<int> { 0, 1, 0, 0, 1, 2, 0, 1 }, new List<double> { 3.5, 1.5, 5.0, 2.0, 3.0, 4.0, 0.5, 2.5 });

            SparseMatrix ccsLeft = new CompressedColumn(6, 5, new int[6] { 0, 0, 2, 4, 8, 9 },
                new List<int> { 0, 4, 2, 4, 0, 1, 3, 4, 5 }, new List<double> { 1.5, 4.0, 2.0, 3.5, 1.25, 6.75, 5.5, 2.25, 7.25 });
            SparseMatrix ccsRight = new CompressedColumn(5, 3, new int[4] { 0, 4, 7, 8 },
                new List<int> { 0, 2, 3, 4, 1, 3, 4, 3 }, new List<double> { 3.5, 5.0, 2.0, 0.5, 1.5, 3.0, 2.5, 4.0 });


            CompressedRow matrix = new CompressedRow(6, 3, new int[7] { 0, 3, 6, 7, 10, 13, 15 },
                new List<int> { 0, 1, 2, 0, 1, 2, 0, 0, 1, 2, 0, 1, 2, 0, 1 },
                new List<double> { 2.5, 6.0, 5.0, 13.5, 20.25, 27.0, 10.0, 11.0, 16.5, 22.0, 22.0, 12.75, 9.0, 3.625, 18.125 });

            //Act
            SparseMatrix CrsCrsMatrix = SparseMatrix.Multiply(crsLeft, crsRight);
            SparseMatrix CrsCcsMatrix = SparseMatrix.Multiply(crsLeft, ccsRight);

            SparseMatrix CcsCcsMatrix = SparseMatrix.Multiply(ccsLeft, ccsRight);
            SparseMatrix CcsCrsMatrix = SparseMatrix.Multiply(ccsLeft, crsRight);

            // Assert
            Assert.AreEqual(matrix.RowCount, CrsCrsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, CrsCcsMatrix.RowCount);

            Assert.AreEqual(matrix.RowCount, CcsCcsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, CcsCrsMatrix.RowCount);


            Assert.AreEqual(matrix.ColumnCount, CrsCrsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, CrsCcsMatrix.ColumnCount);

            Assert.AreEqual(matrix.ColumnCount, CcsCcsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, CcsCrsMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - CrsCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - CrsCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);

                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - CcsCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - CcsCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Tests the static method <see cref="SparseMatrix.Multiply(double, SparseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Multiply(double,SparseMatrix)")]
        public void Static_Multiply_Double_SparseMatrix()
        {
            // Arrange
            double factor = -2.5;
            SparseMatrix crsOperand = new CompressedRow(3, 2, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            SparseMatrix ccsOperand = new CompressedColumn(3, 2, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 2.0, -4.0, 3.0, -5.0, 1.0 });

            DenseMatrix result = new DenseMatrix(3, 2, new double[] { -10.0, -7.5, -5.0, 12.5, 10.0, -2.5 });

            // Act
            SparseMatrix crsMatrix = SparseMatrix.Multiply(factor, crsOperand);
            SparseMatrix ccsMatrix = SparseMatrix.Multiply(factor, ccsOperand);

            // Assert
            Assert.AreEqual(result.RowCount, crsMatrix.RowCount);
            Assert.AreEqual(result.RowCount, ccsMatrix.RowCount);

            Assert.AreEqual(result.ColumnCount, crsMatrix.ColumnCount);
            Assert.AreEqual(result.ColumnCount, ccsMatrix.ColumnCount);

            for (int i_R = 0; i_R < result.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < result.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(result[i_R, i_C] - crsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(result[i_R, i_C] - ccsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="SparseMatrix.Multiply(SparseMatrix, double)"/>.
        /// </summary>
        [TestMethod("Static Multiply(SparseMatrix,double)")]
        public void Static_Multiply_SparseMatrix_Double()
        {
            // Arrange
            SparseMatrix crsOperand = new CompressedRow(3, 2, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            SparseMatrix ccsOperand = new CompressedColumn(3, 2, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 2.0, -4.0, 3.0, -5.0, 1.0 });
            double factor = -2.5;

            DenseMatrix result = new DenseMatrix(3, 2, new double[] { -10.0, -7.5, -5.0, 12.5, 10.0, -2.5 });

            // Act
            SparseMatrix crsMatrix = SparseMatrix.Multiply(crsOperand, factor);
            SparseMatrix ccsMatrix = SparseMatrix.Multiply(ccsOperand, factor);

            // Assert
            Assert.AreEqual(result.RowCount, crsMatrix.RowCount);
            Assert.AreEqual(result.RowCount, ccsMatrix.RowCount);

            Assert.AreEqual(result.ColumnCount, crsMatrix.ColumnCount);
            Assert.AreEqual(result.ColumnCount, ccsMatrix.ColumnCount);

            for (int i_R = 0; i_R < result.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < result.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(result[i_R, i_C] - crsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(result[i_R, i_C] - ccsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }


        /// <summary>
        /// Tests the static method <see cref="SparseMatrix.Divide(SparseMatrix, double)"/>.
        /// </summary>
        [TestMethod("Static Divide(SparseMatrix,double)")]
        public void Static_Divide_SparseMatrix_Double()
        {
            // Arrange
            SparseMatrix crsOperand = new CompressedRow(2, 1, new int[3] { 0, 1, 2 },
                new List<int> { 0, 0 }, new List<double> { 4.0, 3.0 });
            SparseMatrix ccsOperand = new CompressedColumn(2, 1, new int[2] { 0, 2 },
                new List<int> { 0, 1 }, new List<double> { 4.0, 3.0 });
            double divisor = -2.0;

            DenseMatrix result = new DenseMatrix(2, 1, new double[] { -2.0, -1.5 });

            // Act
            SparseMatrix crsMatrix = SparseMatrix.Divide(crsOperand, divisor);
            SparseMatrix ccsMatrix = SparseMatrix.Divide(ccsOperand, divisor);

            // Assert
            Assert.AreEqual(result.RowCount, crsMatrix.RowCount);
            Assert.AreEqual(result.RowCount, ccsMatrix.RowCount);

            Assert.AreEqual(result.ColumnCount, crsMatrix.ColumnCount);
            Assert.AreEqual(result.ColumnCount, ccsMatrix.ColumnCount);

            for (int i_R = 0; i_R < result.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < result.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(result[i_R, i_C] - crsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(result[i_R, i_C] - ccsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }


        /******************** Other Operations ********************/

        /// <summary>
        /// Tests the static method <see cref="SparseMatrix.Multiply(SparseMatrix, Vector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(SparseMatrix,Vector)")]
        public void Static_Multiply_SparseMatrix_Vector()
        {
            // Arrange
            SparseMatrix crsMatrix = new CompressedRow(3, 2, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            SparseMatrix ccsMatrix = new CompressedColumn(3, 2, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 2.0, -4.0, 3.0, -5.0, 1.0 });

            Vector denseVector = new DenseVector(new double[2] { -2.0, 6.0 });
            Vector sparseVector = new SparseVector(2, new int[2] { 0, 1 }, new double[2] { -2.0, 6.0 });

            Vector result = new DenseVector(new double[3] { 10.0, -34.0, 14.0 });

            //Act
            Vector crsDenseVector = SparseMatrix.Multiply(crsMatrix, denseVector);
            Vector ccsDenseVector = SparseMatrix.Multiply(ccsMatrix, denseVector);

            Vector crsSparseVector = SparseMatrix.Multiply(crsMatrix, sparseVector);
            Vector ccsSparseVector = SparseMatrix.Multiply(ccsMatrix, sparseVector);

            // Assert
            Assert.AreEqual(result.Size, crsDenseVector.Size);
            Assert.AreEqual(result.Size, ccsDenseVector.Size);

            Assert.AreEqual(result.Size, crsSparseVector.Size);
            Assert.AreEqual(result.Size, ccsSparseVector.Size);

            for (int i_R = 0; i_R < result.Size; i_R++)
            {
                Assert.IsTrue(Math.Abs(result[i_R] - crsDenseVector[i_R]) < Settings.AbsolutePrecision);
                Assert.IsTrue(Math.Abs(result[i_R] - ccsDenseVector[i_R]) < Settings.AbsolutePrecision);

                Assert.IsTrue(Math.Abs(result[i_R] - crsSparseVector[i_R]) < Settings.AbsolutePrecision);
                Assert.IsTrue(Math.Abs(result[i_R] - ccsSparseVector[i_R]) < Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="SparseMatrix.Multiply(SparseMatrix, DenseVector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(SparseMatrix,DenseVector)")]
        public void Static_Multiply_SparseMatrix_DenseVector()
        {
            // Arrange
            SparseMatrix crsMatrix = new CompressedRow(3, 2, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            SparseMatrix ccsMatrix = new CompressedColumn(3, 2, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 2.0, -4.0, 3.0, -5.0, 1.0 });

            DenseVector denseVector = new DenseVector(new double[2] { -2.0, 6.0 });

            DenseVector result = new DenseVector(new double[3] { 10.0, -34.0, 14.0 });

            //Act
            DenseVector crsDenseVector = SparseMatrix.Multiply(crsMatrix, denseVector);
            DenseVector ccsDenseVector = SparseMatrix.Multiply(ccsMatrix, denseVector);

            // Assert
            Assert.AreEqual(result.Size, crsDenseVector.Size);
            Assert.AreEqual(result.Size, ccsDenseVector.Size);

            for (int i_R = 0; i_R < result.Size; i_R++)
            {
                Assert.IsTrue(Math.Abs(result[i_R] - crsDenseVector[i_R]) < Settings.AbsolutePrecision);
                Assert.IsTrue(Math.Abs(result[i_R] - ccsDenseVector[i_R]) < Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="SparseMatrix.Multiply(SparseMatrix, SparseVector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(SparseMatrix,SparseVector)")]
        public void Static_Multiply_SparseMatrix_SparseVector()
        {
            // Arrange
            SparseMatrix crsMatrix = new CompressedRow(3, 2, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            SparseMatrix ccsMatrix = new CompressedColumn(3, 2, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 2.0, -4.0, 3.0, -5.0, 1.0 });

            SparseVector sparseVector = new SparseVector(2, new int[2] { 0, 1 }, new double[2] { -2.0, 6.0 });

            SparseVector result = new SparseVector(3, new int[3] { 0, 1, 2 }, new double[3] { 10.0, -34.0, 14.0 });

            // Act
            SparseVector crsSparseVector = SparseMatrix.Multiply(crsMatrix, sparseVector);
            SparseVector ccsSparseVector = SparseMatrix.Multiply(ccsMatrix, sparseVector);


            // Assert
            Assert.AreEqual(result.Size, crsSparseVector.Size);
            Assert.AreEqual(result.Size, ccsSparseVector.Size);

            for (int i_R = 0; i_R < result.Size; i_R++)
            {
                Assert.IsTrue(Math.Abs(result[i_R] - crsSparseVector[i_R]) < Settings.AbsolutePrecision);
                Assert.IsTrue(Math.Abs(result[i_R] - ccsSparseVector[i_R]) < Settings.AbsolutePrecision);
            }
        }


        /// <summary>
        /// Tests the static method <see cref="SparseMatrix.TransposeMultiply(SparseMatrix, Vector)"/>.
        /// </summary>
        [TestMethod("Static TransposeMultiply(SparseMatrix,Vector)")]
        public void Static_TransposeMultiply_SparseMatrix_Vector()
        {
            // Arrange
            SparseMatrix crsMatrix = new CompressedRow(3, 2, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            SparseMatrix ccsMatrix = new CompressedColumn(3, 2, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 2.0, -4.0, 3.0, -5.0, 1.0 });

            Vector denseVector = new DenseVector(new double[3] { -2.0, 0.0, 6.0 });
            Vector sparseVector = new SparseVector(3, new int[2] { 0, 2 }, new double[2] { -2.0, 6.0 });

            DenseVector result = new DenseVector(new double[2] { -32.0, 0.0 });

            //Act
            Vector crsDenseVector = SparseMatrix.TransposeMultiply(crsMatrix, denseVector);
            Vector ccsDenseVector = SparseMatrix.TransposeMultiply(ccsMatrix, denseVector);

            Vector crsSparseVector = SparseMatrix.TransposeMultiply(crsMatrix, sparseVector);
            Vector ccsSparseVector = SparseMatrix.TransposeMultiply(ccsMatrix, sparseVector);

            // Assert
            Assert.AreEqual(result.Size, crsDenseVector.Size);
            Assert.AreEqual(result.Size, ccsDenseVector.Size);

            Assert.AreEqual(result.Size, crsSparseVector.Size);
            Assert.AreEqual(result.Size, ccsSparseVector.Size);

            for (int i_R = 0; i_R < result.Size; i_R++)
            {
                Assert.IsTrue(Math.Abs(result[i_R] - crsDenseVector[i_R]) < Settings.AbsolutePrecision);
                Assert.IsTrue(Math.Abs(result[i_R] - ccsDenseVector[i_R]) < Settings.AbsolutePrecision);

                Assert.IsTrue(Math.Abs(result[i_R] - crsSparseVector[i_R]) < Settings.AbsolutePrecision);
                Assert.IsTrue(Math.Abs(result[i_R] - ccsSparseVector[i_R]) < Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="SparseMatrix.TransposeMultiply(SparseMatrix, DenseVector)"/>.
        /// </summary>
        [TestMethod("Static TransposeMultiply(SparseMatrix,DenseVector)")]
        public void Static_TransposeMultiply_SparseMatrix_DenseVector()
        {
            // Arrange
            SparseMatrix crsMatrix = new CompressedRow(3, 2, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            SparseMatrix ccsMatrix = new CompressedColumn(3, 2, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 2.0, -4.0, 3.0, -5.0, 1.0 });

            DenseVector denseVector = new DenseVector(new double[3] { -2.0, 0.0, 6.0 });

            DenseVector result = new DenseVector(new double[2] { -32.0, 0.0 });

            //Act
            DenseVector crsDenseVector = SparseMatrix.TransposeMultiply(crsMatrix, denseVector);
            DenseVector ccsDenseVector = SparseMatrix.TransposeMultiply(ccsMatrix, denseVector);

            // Assert
            Assert.AreEqual(result.Size, crsDenseVector.Size);
            Assert.AreEqual(result.Size, ccsDenseVector.Size);

            for (int i_R = 0; i_R < result.Size; i_R++)
            {
                Assert.IsTrue(Math.Abs(result[i_R] - crsDenseVector[i_R]) < Settings.AbsolutePrecision);
                Assert.IsTrue(Math.Abs(result[i_R] - ccsDenseVector[i_R]) < Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="SparseMatrix.TransposeMultiply(SparseMatrix, SparseVector)"/>.
        /// </summary>
        [TestMethod("Static TransposeMultiply(SparseMatrix,SparseVector)")]
        public void Static_TransposeMultiply_SparseMatrix_SparseVector()
        {
            // Arrange
            SparseMatrix crsMatrix = new CompressedRow(3, 2, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            SparseMatrix ccsMatrix = new CompressedColumn(3, 2, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 2.0, -4.0, 3.0, -5.0, 1.0 });

            SparseVector sparseVector = new SparseVector(3, new int[2] { 0, 2 }, new double[2] { -2.0, 6.0 });

            DenseVector result = new DenseVector(new double[2] { -32.0, 0.0 });

            //Act
            Vector crsSparseVector = SparseMatrix.TransposeMultiply(crsMatrix, sparseVector);
            Vector ccsSparseVector = SparseMatrix.TransposeMultiply(ccsMatrix, sparseVector);

            // Assert
            Assert.AreEqual(result.Size, crsSparseVector.Size);
            Assert.AreEqual(result.Size, ccsSparseVector.Size);

            for (int i_R = 0; i_R < result.Size; i_R++)
            {
                Assert.IsTrue(Math.Abs(result[i_R] - crsSparseVector[i_R]) < Settings.AbsolutePrecision);
                Assert.IsTrue(Math.Abs(result[i_R] - ccsSparseVector[i_R]) < Settings.AbsolutePrecision);
            }
        }

        #endregion
    }
}
