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
    /// Class testing the members of the <see cref="DenseMatrix"/> class.
    /// </summary>
    [TestClass]
    public class DenseMatrixTest
    {
        #region Behavior

        /// <summary>
        /// Tests that <see cref="DenseMatrix"/> is reference type.
        /// </summary>
        [TestMethod("Behavior IsReference")]
        public void DenseMatrix_IsReference()
        {
            // Arrange
            DenseMatrix matrix = new DenseMatrix(2, 2, new double[] { 1.0, 2.0, 3.0, 4.0 });
            DenseMatrix otherMatrix = new DenseMatrix(1, 2, new double[] { 2.0, 4.0 });
            //Act
            otherMatrix = matrix;
            // Assert
            Assert.AreEqual(matrix, otherMatrix);
            Assert.AreSame(matrix, otherMatrix);
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
            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 });

            // Act

            // Assert
            Assert.AreEqual(2, matrix.RowCount);
            Assert.AreEqual(3, matrix.ColumnCount);
        }

        /// <summary>
        /// Tests the <see cref="DenseMatrix"/> indexer property.
        /// </summary>
        [TestMethod("Property this[int,int]")]
        public void Index_Int_Int()
        {
            // Arrange
            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 });

            //Act

            // Assert
            Assert.AreEqual(1.0, matrix[0, 0]);
            Assert.AreEqual(2.0, matrix[0, 1]);
            Assert.AreEqual(3.0, matrix[0, 2]);
            Assert.AreEqual(4.0, matrix[1, 0]);
            Assert.AreEqual(5.0, matrix[1, 1]);
            Assert.AreEqual(6.0, matrix[1, 2]);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Tests the initialisation of the <see cref="DenseMatrix"/> from its number of row and column.
        /// </summary>
        [TestMethod("Constructor(Int,Int)")]
        public void Constructor_Int_Int()
        {
            // Arrange

            // Act
            DenseMatrix matrix = new DenseMatrix(2, 2);

            // Assert
            Assert.IsTrue(Math.Abs(matrix[0, 0]) < Settings.AbsolutePrecision);
            Assert.IsTrue(Math.Abs(matrix[0, 1]) < Settings.AbsolutePrecision);
            Assert.IsTrue(Math.Abs(matrix[1, 0]) < Settings.AbsolutePrecision);
            Assert.IsTrue(Math.Abs(matrix[1, 1]) < Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="DenseMatrix"/> from its size and components.
        /// </summary>
        [TestMethod("Constructor(Int,Int,Double)")]
        public void Constructor_Int_Int_Double()
        {
            // Arrange

            // Act
            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 });

            // Assert
            Assert.IsTrue(Math.Abs(matrix[0, 0] - 1.0) < Settings.AbsolutePrecision);
            Assert.IsTrue(Math.Abs(matrix[0, 1] - 2.0) < Settings.AbsolutePrecision);
            Assert.IsTrue(Math.Abs(matrix[0, 2] - 3.0) < Settings.AbsolutePrecision);
            Assert.IsTrue(Math.Abs(matrix[1, 0] - 4.0) < Settings.AbsolutePrecision);
            Assert.IsTrue(Math.Abs(matrix[1, 1] - 5.0) < Settings.AbsolutePrecision);
            Assert.IsTrue(Math.Abs(matrix[1, 2] - 6.0) < Settings.AbsolutePrecision);
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Tests the static property <see cref="DenseMatrix.Zero"/>.
        /// </summary>
        [TestMethod("Static Zero")]
        public void Static_Zero()
        {
            // Arrange

            // Act
            DenseMatrix matrix = DenseMatrix.Zero(5, 4);

            // Assert
            for (int r = 0; r < 5; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    Assert.AreEqual(0.0, matrix[r, c]);
                }
            }
        }

        /// <summary>
        /// Tests the static property <see cref="DenseMatrix.Identity"/>.
        /// </summary>
        [TestMethod("Static Identity")]
        public void Static_Identity()
        {
            // Arrange

            // Act
            DenseMatrix matrix = DenseMatrix.Identity(4);

            // Assert
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    if (r == c) { Assert.AreEqual(1.0, matrix[r, c]); }
                    else { Assert.AreEqual(0.0, matrix[r, c]); }
                }
            }
        }

        #endregion

        #region Static Methods

        /******************** Algebraic Near Ring ********************/

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Add(DenseMatrix, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Add(DenseMatrix,DenseMatrix)")]
        public void Static_Add_DenseMatrix_DenseMatrix()
        {
            // Arrange
            DenseMatrix left = new DenseMatrix(2, 3, new double[] { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            DenseMatrix right = new DenseMatrix(2, 3, new double[] { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { 5.0, 5.0, 5.0, 10.0, 10.0, 10.0 });

            //Act
            DenseMatrix otherMatrix = DenseMatrix.Add(left, right);

            // Assert
            Assert.AreEqual(matrix.RowCount, otherMatrix.RowCount);
            Assert.AreEqual(matrix.ColumnCount, otherMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
            
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Subtract(DenseMatrix, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Subtract(DenseMatrix,DenseMatrix)")]
        public void Static_Subtract_DenseMatrix_DenseMatrix()
        {
            // Arrange
            DenseMatrix left = new DenseMatrix(2, 3, new double[] { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            DenseMatrix right = new DenseMatrix(2, 3, new double[] { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { -3.0, -1.0, 1.0, 0.0, 2.0, 4.0 });

            //Act
            DenseMatrix otherMatrix = DenseMatrix.Subtract(left, right);

            // Assert
            Assert.AreEqual(matrix.RowCount, otherMatrix.RowCount);
            Assert.AreEqual(matrix.ColumnCount, otherMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }


        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Multiply(DenseMatrix, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Multiply(DenseMatrix,DenseMatrix)")]
        public void Static_Multiply_DenseMatrix_DenseMatrix()
        {
            // Arrange
            DenseMatrix left = new DenseMatrix(4, 2, new double[] { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0, 9.0, 8.0 });
            DenseMatrix right = new DenseMatrix(2, 3, new double[] { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(4, 3, new double[] { 14.0, 11.0, 8.0, 37.0, 29.0, 21.0, 59.0, 46.0, 33.0, 76.0, 59.0, 42.0 });

            //Act
            DenseMatrix otherMatrix = DenseMatrix.Multiply(left, right);

            // Assert
            Assert.AreEqual(matrix.RowCount, otherMatrix.RowCount);
            Assert.AreEqual(matrix.ColumnCount, otherMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }


        /******************** Matrix Embedding ********************/

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Add(DenseMatrix, BRIDGES.LinearAlgebra.Matrices.Matrix)"/>.
        /// </summary>
        [TestMethod("Static Add(DenseMatrix,Matrix)")]
        public void Static_Add_DenseMatrix_Matrix()
        {
            // Arrange
            DenseMatrix left = new DenseMatrix(2, 3, new double[] { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            BRIDGES.LinearAlgebra.Matrices.Matrix crsRight = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });
            BRIDGES.LinearAlgebra.Matrices.Matrix ccsright = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new int[6] { 0, 1, 0, 1, 0, 1 }, new double[6] { 4.0, 5.0, 3.0, 4.0, 2.0, 3.0 });
            BRIDGES.LinearAlgebra.Matrices.Matrix denseRight = new DenseMatrix(2, 3, new double[] { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { 5.0, 5.0, 5.0, 10.0, 10.0, 10.0 });

            //Act
            DenseMatrix otherCrsMatrix = DenseMatrix.Add(left, crsRight);
            DenseMatrix otherCcsMatrix = DenseMatrix.Add(left, ccsright);
            DenseMatrix otherDenseMatrix = DenseMatrix.Add(left, denseRight);

            // Assert
            Assert.AreEqual(matrix.RowCount, otherCrsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, otherCcsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, otherDenseMatrix.RowCount);

            Assert.AreEqual(matrix.ColumnCount, otherCrsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, otherCcsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, otherDenseMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherDenseMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Add(BRIDGES.LinearAlgebra.Matrices.Matrix, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Add(Matrix,DenseMatrix)")]
        public void Static_Add_Matrix_DenseMatrix()
        {
            // Arrange
            BRIDGES.LinearAlgebra.Matrices.Matrix crsLeft = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            BRIDGES.LinearAlgebra.Matrices.Matrix ccsLeft = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new int[6] { 0, 1, 0, 1, 0, 1 }, new double[6] { 1.0, 5.0, 2.0, 6.0, 3.0, 7.0 });
            BRIDGES.LinearAlgebra.Matrices.Matrix denseLeft = new DenseMatrix(2, 3, new double[] { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            DenseMatrix right = new DenseMatrix(2, 3, new double[] { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { 5.0, 5.0, 5.0, 10.0, 10.0, 10.0 });

            //Act
            DenseMatrix otherCrsMatrix = DenseMatrix.Add(crsLeft, right);
            DenseMatrix otherCcsMatrix = DenseMatrix.Add(ccsLeft, right);
            DenseMatrix otherDenseMatrix = DenseMatrix.Add(denseLeft, right);

            // Assert
            Assert.AreEqual(matrix.RowCount, otherCrsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, otherCcsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, otherDenseMatrix.RowCount);

            Assert.AreEqual(matrix.ColumnCount, otherCrsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, otherCcsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, otherDenseMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherDenseMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }

        }


        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Subtract(DenseMatrix, BRIDGES.LinearAlgebra.Matrices.Matrix)"/>.
        /// </summary>
        [TestMethod("Static Subtract(DenseMatrix,Matrix)")]
        public void Static_Subtract_DenseMatrix_Matrix()
        {
            // Arrange
            DenseMatrix left = new DenseMatrix(2, 3, new double[] { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            BRIDGES.LinearAlgebra.Matrices.Matrix crsRight = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });
            BRIDGES.LinearAlgebra.Matrices.Matrix ccsright = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new int[6] { 0, 1, 0, 1, 0, 1 }, new double[6] { 4.0, 5.0, 3.0, 4.0, 2.0, 3.0 });
            BRIDGES.LinearAlgebra.Matrices.Matrix denseRight = new DenseMatrix(2, 3, new double[] { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { -3.0, -1.0, 1.0, 0.0, 2.0, 4.0 });

            //Act
            DenseMatrix otherCrsMatrix = DenseMatrix.Subtract(left, crsRight);
            DenseMatrix otherCcsMatrix = DenseMatrix.Subtract(left, ccsright);
            DenseMatrix otherDenseMatrix = DenseMatrix.Subtract(left, denseRight);

            // Assert
            Assert.AreEqual(matrix.RowCount, otherCrsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, otherCcsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, otherDenseMatrix.RowCount);

            Assert.AreEqual(matrix.ColumnCount, otherCrsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, otherCcsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, otherDenseMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherDenseMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Subtract(BRIDGES.LinearAlgebra.Matrices.Matrix, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Subtract(Matrix,DenseMatrix)")]
        public void Static_Subtract_Matrix_DenseMatrix()
        {
            // Arrange
            BRIDGES.LinearAlgebra.Matrices.Matrix crsLeft = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            BRIDGES.LinearAlgebra.Matrices.Matrix ccsLeft = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new int[6] { 0, 1, 0, 1, 0, 1 }, new double[6] { 1.0, 5.0, 2.0, 6.0, 3.0, 7.0 });
            BRIDGES.LinearAlgebra.Matrices.Matrix denseLeft = new DenseMatrix(2, 3, new double[] { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            DenseMatrix right = new DenseMatrix(2, 3, new double[] { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { -3.0, -1.0, 1.0, 0.0, 2.0, 4.0 });

            //Act
            DenseMatrix otherCrsMatrix = DenseMatrix.Subtract(crsLeft, right);
            DenseMatrix otherCcsMatrix = DenseMatrix.Subtract(ccsLeft, right);
            DenseMatrix otherDenseMatrix = DenseMatrix.Subtract(denseLeft, right);

            // Assert
            Assert.AreEqual(matrix.RowCount, otherCrsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, otherCcsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, otherDenseMatrix.RowCount);

            Assert.AreEqual(matrix.ColumnCount, otherCrsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, otherCcsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, otherDenseMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherDenseMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }

        }


        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Multiply(DenseMatrix, BRIDGES.LinearAlgebra.Matrices.Matrix)"/>.
        /// </summary>
        [TestMethod("Static Multiply(DenseMatrix,Matrix)")]
        public void Static_Multiply_DenseMatrix_Matrix()
        {
            // Arrange
            DenseMatrix left = new DenseMatrix(4, 2, new double[] { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0, 9.0, 8.0 });
            BRIDGES.LinearAlgebra.Matrices.Matrix crsRight = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });
            BRIDGES.LinearAlgebra.Matrices.Matrix ccsRight = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new int[6] { 0, 1, 0, 1, 0, 1 }, new double[6] { 4.0, 5.0, 3.0, 4.0, 2.0, 3.0 });
            BRIDGES.LinearAlgebra.Matrices.Matrix denseRight = new DenseMatrix(2, 3, new double[] { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(4, 3, new double[] { 14.0, 11.0, 8.0, 37.0, 29.0, 21.0, 59.0, 46.0, 33.0, 76.0, 59.0, 42.0 });

            //Act
            DenseMatrix otherCrsMatrix = DenseMatrix.Multiply(left, crsRight);
            DenseMatrix otherCcsMatrix = DenseMatrix.Multiply(left, ccsRight);
            DenseMatrix otherDenseMatrix = DenseMatrix.Multiply(left, denseRight);

            // Assert
            Assert.AreEqual(matrix.RowCount, otherCrsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, otherCcsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, otherDenseMatrix.RowCount);

            Assert.AreEqual(matrix.ColumnCount, otherCrsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, otherCcsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, otherDenseMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherDenseMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Multiply(BRIDGES.LinearAlgebra.Matrices.Matrix, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Multiply(Matrix,DenseMatrix)")]
        public void Static_Multiply_Matrix_DenseMatrix()
        {
            // Arrange
            BRIDGES.LinearAlgebra.Matrices.Matrix crsLeft = new CompressedRow(4, 2, new int[5] { 0, 2, 4, 6, 8 },
                new List<int> { 0, 1, 0, 1, 0, 1, 0, 1 }, new List<double> { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0, 9.0, 8.0 });
            BRIDGES.LinearAlgebra.Matrices.Matrix ccsLeft = new CompressedColumn(4, 2, new int[3] { 0, 4, 8 },
                new int[8] { 0, 1, 2, 3, 0, 1, 2, 3 }, new double[8] { 1.0, 3.0, 6.0, 9.0, 2.0, 5.0, 7.0, 8.0 });
            BRIDGES.LinearAlgebra.Matrices.Matrix denseLeft = new DenseMatrix(4, 2, new double[] { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0, 9.0, 8.0 });
            DenseMatrix right = new DenseMatrix(2, 3, new double[] { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(4, 3, new double[] { 14.0, 11.0, 8.0, 37.0, 29.0, 21.0, 59.0, 46.0, 33.0, 76.0, 59.0, 42.0 });

            //Act
            DenseMatrix otherCrsMatrix = DenseMatrix.Multiply(crsLeft, right);
            DenseMatrix otherCcsMatrix = DenseMatrix.Multiply(ccsLeft, right);
            DenseMatrix otherDenseMatrix = DenseMatrix.Multiply(denseLeft, right);

            // Assert
            Assert.AreEqual(matrix.RowCount, otherCrsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, otherCcsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, otherDenseMatrix.RowCount);

            Assert.AreEqual(matrix.ColumnCount, otherCrsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, otherCcsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, otherDenseMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherDenseMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }



        /******************** Sparse Matrix Embedding ********************/

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Add(DenseMatrix, Matrix)"/>.
        /// </summary>
        [TestMethod("Static Add(DenseMatrix,SparseMatrix)")]
        public void Static_Add_DenseMatrix_SparseMatrix()
        {
            // Arrange
            DenseMatrix left = new DenseMatrix(2, 3, new double[] { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            Matrix crsRight = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });
            Matrix ccsright = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new int[6] { 0, 1, 0, 1, 0, 1 }, new double[6] { 4.0, 5.0, 3.0, 4.0, 2.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { 5.0, 5.0, 5.0, 10.0, 10.0, 10.0 });

            //Act
            DenseMatrix otherCrsMatrix = DenseMatrix.Add(left, crsRight);
            DenseMatrix otherCcsMatrix = DenseMatrix.Add(left, ccsright);

            // Assert
            Assert.AreEqual(matrix.RowCount, otherCrsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, otherCcsMatrix.RowCount);

            Assert.AreEqual(matrix.ColumnCount, otherCrsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, otherCcsMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Add(Matrix, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Add(SparseMatrix,DenseMatrix)")]
        public void Static_Add_SparseMatrix_DenseMatrix()
        {
            // Arrange
            Matrix crsLeft = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            Matrix ccsLeft = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new int[6] { 0, 1, 0, 1, 0, 1 }, new double[6] { 1.0, 5.0, 2.0, 6.0, 3.0, 7.0 });
            DenseMatrix right = new DenseMatrix(2, 3, new double[] { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { 5.0, 5.0, 5.0, 10.0, 10.0, 10.0 });

            //Act
            DenseMatrix otherCrsMatrix = DenseMatrix.Add(crsLeft, right);
            DenseMatrix otherCcsMatrix = DenseMatrix.Add(ccsLeft, right);

            // Assert
            Assert.AreEqual(matrix.RowCount, otherCrsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, otherCcsMatrix.RowCount);

            Assert.AreEqual(matrix.ColumnCount, otherCrsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, otherCcsMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }

        }


        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Subtract(DenseMatrix, Matrix)"/>.
        /// </summary>
        [TestMethod("Static Subtract(DenseMatrix,SparseMatrix)")]
        public void Static_Subtract_DenseMatrix_SparseMatrix()
        {
            // Arrange
            DenseMatrix left = new DenseMatrix(2, 3, new double[] { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            Matrix crsRight = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });
            Matrix ccsright = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new int[6] { 0, 1, 0, 1, 0, 1 }, new double[6] { 4.0, 5.0, 3.0, 4.0, 2.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { -3.0, -1.0, 1.0, 0.0, 2.0, 4.0 });

            //Act
            DenseMatrix otherCrsMatrix = DenseMatrix.Subtract(left, crsRight);
            DenseMatrix otherCcsMatrix = DenseMatrix.Subtract(left, ccsright);

            // Assert
            Assert.AreEqual(matrix.RowCount, otherCrsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, otherCcsMatrix.RowCount);

            Assert.AreEqual(matrix.ColumnCount, otherCrsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, otherCcsMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Subtract(Matrix, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Subtract(SparseMatrix,DenseMatrix)")]
        public void Static_Subtract_SparseMatrix_DenseMatrix()
        {
            // Arrange
            Matrix crsLeft = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            Matrix ccsLeft = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new int[6] { 0, 1, 0, 1, 0, 1 }, new double[6] { 1.0, 5.0, 2.0, 6.0, 3.0, 7.0 });
            DenseMatrix right = new DenseMatrix(2, 3, new double[] { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { -3.0, -1.0, 1.0, 0.0, 2.0, 4.0 });

            //Act
            DenseMatrix otherCrsMatrix = DenseMatrix.Subtract(crsLeft, right);
            DenseMatrix otherCcsMatrix = DenseMatrix.Subtract(ccsLeft, right);

            // Assert
            Assert.AreEqual(matrix.RowCount, otherCrsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, otherCcsMatrix.RowCount);

            Assert.AreEqual(matrix.ColumnCount, otherCrsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, otherCcsMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }

        }


        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Multiply(DenseMatrix, Matrix)"/>.
        /// </summary>
        [TestMethod("Static Multiply(DenseMatrix,SparseMatrix)")]
        public void Static_Multiply_DenseMatrix_SparseMatrix()
        {
            // Arrange
            DenseMatrix left = new DenseMatrix(4, 2, new double[] { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0, 9.0, 8.0 });
            Matrix crsRight = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });
            Matrix ccsRight = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new int[6] { 0, 1, 0, 1, 0, 1 }, new double[6] { 4.0, 5.0, 3.0, 4.0, 2.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(4, 3, new double[] { 14.0, 11.0, 8.0, 37.0, 29.0, 21.0, 59.0, 46.0, 33.0, 76.0, 59.0, 42.0 });

            //Act
            DenseMatrix otherCrsMatrix = DenseMatrix.Multiply(left, crsRight);
            DenseMatrix otherCcsMatrix = DenseMatrix.Multiply(left, ccsRight);

            // Assert
            Assert.AreEqual(matrix.RowCount, otherCrsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, otherCcsMatrix.RowCount);

            Assert.AreEqual(matrix.ColumnCount, otherCrsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, otherCcsMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Multiply(Matrix, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Multiply(SparseMatrix,DenseMatrix)")]
        public void Static_Multiply_SparseMatrix_DenseMatrix()
        {
            // Arrange
            Matrix crsLeft = new CompressedRow(4, 2, new int[5] { 0, 2, 4, 6, 8 },
                new List<int> { 0, 1, 0, 1, 0, 1, 0, 1 }, new List<double> { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0, 9.0, 8.0 });
            Matrix ccsLeft = new CompressedColumn(4, 2, new int[3] { 0, 4, 8 },
                new int[8] { 0, 1, 2, 3, 0, 1, 2, 3 }, new double[8] { 1.0, 3.0, 6.0, 9.0, 2.0, 5.0, 7.0, 8.0 });
            DenseMatrix right = new DenseMatrix(2, 3, new double[] { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(4, 3, new double[] { 14.0, 11.0, 8.0, 37.0, 29.0, 21.0, 59.0, 46.0, 33.0, 76.0, 59.0, 42.0 });

            //Act
            DenseMatrix otherCrsMatrix = DenseMatrix.Multiply(crsLeft, right);
            DenseMatrix otherCcsMatrix = DenseMatrix.Multiply(ccsLeft, right);

            // Assert
            Assert.AreEqual(matrix.RowCount, otherCrsMatrix.RowCount);
            Assert.AreEqual(matrix.RowCount, otherCcsMatrix.RowCount);

            Assert.AreEqual(matrix.ColumnCount, otherCrsMatrix.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, otherCcsMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }


        /******************** CompressedColumn Embedding ********************/

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Add(DenseMatrix, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Add(DenseMatrix,CompressedColumn)")]
        public void Static_Add_DenseMatrix_CompressedColumn()
        {
            // Arrange
            DenseMatrix left = new DenseMatrix(2, 3, new double[] { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            CompressedColumn ccsright = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new int[6] { 0, 1, 0, 1, 0, 1 }, new double[6] { 4.0, 5.0, 3.0, 4.0, 2.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { 5.0, 5.0, 5.0, 10.0, 10.0, 10.0 });

            //Act
            DenseMatrix otherCcsMatrix = DenseMatrix.Add(left, ccsright);

            // Assert
            Assert.AreEqual(matrix.RowCount, otherCcsMatrix.RowCount);
            Assert.AreEqual(matrix.ColumnCount, otherCcsMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Add(CompressedColumn, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Add(CompressedColumn,DenseMatrix)")]
        public void Static_Add_CompressedColumn_DenseMatrix()
        {
            // Arrange
            CompressedColumn ccsLeft = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new int[6] { 0, 1, 0, 1, 0, 1 }, new double[6] { 1.0, 5.0, 2.0, 6.0, 3.0, 7.0 });
            DenseMatrix right = new DenseMatrix(2, 3, new double[] { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { 5.0, 5.0, 5.0, 10.0, 10.0, 10.0 });

            //Act
            DenseMatrix otherCcsMatrix = DenseMatrix.Add(ccsLeft, right);

            // Assert
            Assert.AreEqual(matrix.RowCount, otherCcsMatrix.RowCount);
            Assert.AreEqual(matrix.ColumnCount, otherCcsMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }

        }


        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Subtract(DenseMatrix, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Subtract(DenseMatrix,CompressedColumn)")]
        public void Static_Subtract_DenseMatrix_CompressedColumn()
        {
            // Arrange
            DenseMatrix left = new DenseMatrix(2, 3, new double[] { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            CompressedColumn ccsright = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new int[6] { 0, 1, 0, 1, 0, 1 }, new double[6] { 4.0, 5.0, 3.0, 4.0, 2.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { -3.0, -1.0, 1.0, 0.0, 2.0, 4.0 });

            //Act
            DenseMatrix otherCcsMatrix = DenseMatrix.Subtract(left, ccsright);

            // Assert
            Assert.AreEqual(matrix.RowCount, otherCcsMatrix.RowCount);
            Assert.AreEqual(matrix.ColumnCount, otherCcsMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Subtract(CompressedColumn, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Subtract(CompressedColumn,DenseMatrix)")]
        public void Static_Subtract_CompressedColumn_DenseMatrix()
        {
            // Arrange
            CompressedColumn ccsLeft = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new int[6] { 0, 1, 0, 1, 0, 1 }, new double[6] { 1.0, 5.0, 2.0, 6.0, 3.0, 7.0 });
            DenseMatrix right = new DenseMatrix(2, 3, new double[] { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { -3.0, -1.0, 1.0, 0.0, 2.0, 4.0 });

            //Act
            DenseMatrix otherCcsMatrix = DenseMatrix.Subtract(ccsLeft, right);

            // Assert
            Assert.AreEqual(matrix.RowCount, otherCcsMatrix.RowCount);
            Assert.AreEqual(matrix.ColumnCount, otherCcsMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }

        }


        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Multiply(DenseMatrix, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Multiply(DenseMatrix,CompressedColumn)")]
        public void Static_Multiply_DenseMatrix_CompressedColumn()
        {
            // Arrange
            DenseMatrix left = new DenseMatrix(4, 2, new double[] { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0, 9.0, 8.0 });
            CompressedColumn ccsRight = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new int[6] { 0, 1, 0, 1, 0, 1 }, new double[6] { 4.0, 5.0, 3.0, 4.0, 2.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(4, 3, new double[] { 14.0, 11.0, 8.0, 37.0, 29.0, 21.0, 59.0, 46.0, 33.0, 76.0, 59.0, 42.0 });

            //Act
            DenseMatrix otherCcsMatrix = DenseMatrix.Multiply(left, ccsRight);

            // Assert
            Assert.AreEqual(matrix.RowCount, otherCcsMatrix.RowCount);
            Assert.AreEqual(matrix.ColumnCount, otherCcsMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Multiply(CompressedColumn, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedColumn,DenseMatrix)")]
        public void Static_Multiply_CompressedColumn_DenseMatrix()
        {
            // Arrange

            CompressedColumn ccsLeft = new CompressedColumn(4, 2, new int[3] { 0, 4, 8 },
                new int[8] { 0, 1, 2, 3, 0, 1, 2, 3 }, new double[8] { 1.0, 3.0, 6.0, 9.0, 2.0, 5.0, 7.0, 8.0 });
            DenseMatrix right = new DenseMatrix(2, 3, new double[] { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(4, 3, new double[] { 14.0, 11.0, 8.0, 37.0, 29.0, 21.0, 59.0, 46.0, 33.0, 76.0, 59.0, 42.0 });

            //Act
            DenseMatrix otherCcsMatrix = DenseMatrix.Multiply(ccsLeft, right);

            // Assert
            Assert.AreEqual(matrix.RowCount, otherCcsMatrix.RowCount);
            Assert.AreEqual(matrix.ColumnCount, otherCcsMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCcsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }


        /******************** CompressedRow Embedding ********************/

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Add(DenseMatrix, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Add(DenseMatrix,CompressedRow)")]
        public void Static_Add_DenseMatrix_CompressedRow()
        {
            // Arrange
            DenseMatrix left = new DenseMatrix(2, 3, new double[] { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            CompressedRow crsRight = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { 5.0, 5.0, 5.0, 10.0, 10.0, 10.0 });

            //Act
            DenseMatrix otherCrsMatrix = DenseMatrix.Add(left, crsRight);

            // Assert
            Assert.AreEqual(matrix.RowCount, otherCrsMatrix.RowCount);
            Assert.AreEqual(matrix.ColumnCount, otherCrsMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Add(CompressedRow, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Add(CompressedRow,DenseMatrix)")]
        public void Static_Add_CompressedRow_DenseMatrix()
        {
            // Arrange
            CompressedRow crsLeft = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            DenseMatrix right = new DenseMatrix(2, 3, new double[] { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { 5.0, 5.0, 5.0, 10.0, 10.0, 10.0 });

            //Act
            DenseMatrix otherCrsMatrix = DenseMatrix.Add(crsLeft, right);

            // Assert
            Assert.AreEqual(matrix.RowCount, otherCrsMatrix.RowCount);
            Assert.AreEqual(matrix.ColumnCount, otherCrsMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }

        }


        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Subtract(DenseMatrix, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Subtract(DenseMatrix,CompressedRow)")]
        public void Static_Subtract_DenseMatrix_CompressedRow()
        {
            // Arrange
            DenseMatrix left = new DenseMatrix(2, 3, new double[] { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            CompressedRow crsRight = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { -3.0, -1.0, 1.0, 0.0, 2.0, 4.0 });

            //Act
            DenseMatrix otherCrsMatrix = DenseMatrix.Subtract(left, crsRight);

            // Assert
            Assert.AreEqual(matrix.RowCount, otherCrsMatrix.RowCount);
            Assert.AreEqual(matrix.ColumnCount, otherCrsMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Subtract(CompressedRow, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Subtract(CompressedRow,DenseMatrix)")]
        public void Static_Subtract_CompressedRow_DenseMatrix()
        {
            // Arrange
            CompressedRow crsLeft = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            DenseMatrix right = new DenseMatrix(2, 3, new double[] { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { -3.0, -1.0, 1.0, 0.0, 2.0, 4.0 });

            //Act
            DenseMatrix otherCrsMatrix = DenseMatrix.Subtract(crsLeft, right);

            // Assert
            Assert.AreEqual(matrix.RowCount, otherCrsMatrix.RowCount);
            Assert.AreEqual(matrix.ColumnCount, otherCrsMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }

        }


        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Multiply(DenseMatrix, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Multiply(DenseMatrix,CompressedRow)")]
        public void Static_Multiply_DenseMatrix_CompressedRow()
        {
            // Arrange
            DenseMatrix left = new DenseMatrix(4, 2, new double[] { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0, 9.0, 8.0 });
            CompressedRow crsRight = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });
            
            DenseMatrix matrix = new DenseMatrix(4, 3, new double[] { 14.0, 11.0, 8.0, 37.0, 29.0, 21.0, 59.0, 46.0, 33.0, 76.0, 59.0, 42.0 });

            //Act
            DenseMatrix otherCrsMatrix = DenseMatrix.Multiply(left, crsRight);

            // Assert
            Assert.AreEqual(matrix.RowCount, otherCrsMatrix.RowCount);
            Assert.AreEqual(matrix.ColumnCount, otherCrsMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Multiply(CompressedRow, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedRow,DenseMatrix)")]
        public void Static_Multiply_CompressedRow_DenseMatrix()
        {
            // Arrange
            CompressedRow crsLeft = new CompressedRow(4, 2, new int[5] { 0, 2, 4, 6, 8 },
                new List<int> { 0, 1, 0, 1, 0, 1, 0, 1 }, new List<double> { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0, 9.0, 8.0 });
            DenseMatrix right = new DenseMatrix(2, 3, new double[] { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(4, 3, new double[] { 14.0, 11.0, 8.0, 37.0, 29.0, 21.0, 59.0, 46.0, 33.0, 76.0, 59.0, 42.0 });

            //Act
            DenseMatrix otherCrsMatrix = DenseMatrix.Multiply(crsLeft, right);

            // Assert
            Assert.AreEqual(matrix.RowCount, otherCrsMatrix.RowCount);
            Assert.AreEqual(matrix.ColumnCount, otherCrsMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherCrsMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Multiply(double, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Multiply(double,DenseMatrix)")]
        public void Static_Multiply_Double_DenseMatrix()
        {
            // Arrange
            double factor = -2.5;
            DenseMatrix operand = new DenseMatrix(3, 2, new double[] { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });

            DenseMatrix matrix = new DenseMatrix(3, 2, new double[] { -10.0, -7.5, -5.0, 12.5, 10.0, -2.5 });

            //Act
            DenseMatrix otherMatrix = DenseMatrix.Multiply(factor, operand);

            // Assert
            Assert.AreEqual(matrix.RowCount, otherMatrix.RowCount);
            Assert.AreEqual(matrix.ColumnCount, otherMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Multiply(DenseMatrix, double)"/>.
        /// </summary>
        [TestMethod("Static Multiply(DenseMatrix,double)")]
        public void Static_Multiply_DenseMatrix_Double()
        {
            // Arrange
            DenseMatrix operand = new DenseMatrix(3, 2, new double[] { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            double factor = -2.5;

            DenseMatrix matrix = new DenseMatrix(3, 2, new double[] { -10.0, -7.5, -5.0, 12.5, 10.0, -2.5 });

            //Act
            DenseMatrix otherMatrix = DenseMatrix.Multiply(operand, factor);

            // Assert
            Assert.AreEqual(matrix.RowCount, otherMatrix.RowCount);
            Assert.AreEqual(matrix.ColumnCount, otherMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }


        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Divide(DenseMatrix, double)"/>.
        /// </summary>
        [TestMethod("Static Divide(DenseMatrix,double)")]
        public void Static_Divide_DenseMatrix_Double()
        {
            // Arrange
            DenseMatrix operand = new DenseMatrix(2, 1, new double[] { 4.0, 3.0 });
            double divisor = -2.0;

            DenseMatrix matrix = new DenseMatrix(2, 1, new double[] { -2.0, -1.5 });

            //Act
            DenseMatrix otherMatrix = DenseMatrix.Divide(operand, divisor);

            // Assert
            Assert.AreEqual(matrix.RowCount, otherMatrix.RowCount);
            Assert.AreEqual(matrix.ColumnCount, otherMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }


        /******************** Other Operations ********************/

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Multiply(DenseMatrix, Vector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(DenseMatrix,Vector)")]
        public void Static_Multiply_DenseMatrix_Vector()
        {
            // Arrange
            DenseMatrix matrix = new DenseMatrix(3, 2, new double[] { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            Vector denseVector = new DenseVector(new double[2] { -2.0, 6.0 });
            Vector sparseVector = new SparseVector(2, new int[2] { 0, 1 }, new double[2] { -2.0, 6.0 });

            Vector result = new DenseVector(new double[3] { 10.0, -34.0, 14.0 });

            //Act
            Vector otherDenseVector = DenseMatrix.Multiply(matrix, denseVector);
            Vector otherSparseVector = DenseMatrix.Multiply(matrix, sparseVector);

            // Assert
            Assert.AreEqual(result.Size, otherDenseVector.Size);
            Assert.AreEqual(result.Size, otherSparseVector.Size);

            for (int i_R = 0; i_R < result.Size; i_R++)
            {
                Assert.IsTrue(Math.Abs(result[i_R] - otherDenseVector[i_R]) < Settings.AbsolutePrecision);
                Assert.IsTrue(Math.Abs(result[i_R] - otherSparseVector[i_R]) < Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Multiply(DenseMatrix, DenseVector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(DenseMatrix,DenseVector)")]
        public void Static_Multiply_DenseMatrix_DenseVector()
        {
            // Arrange
            DenseMatrix matrix = new DenseMatrix(3, 2, new double[] { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            DenseVector denseVector = new DenseVector(new double[2] { -2.0, 6.0 });

            DenseVector result = new DenseVector(new double[3] { 10.0, -34.0, 14.0 });

            //Act
            DenseVector otherDenseVector = DenseMatrix.Multiply(matrix, denseVector);

            // Assert
            Assert.AreEqual(result.Size, otherDenseVector.Size);

            for (int i_R = 0; i_R < result.Size; i_R++)
            {
                Assert.IsTrue(Math.Abs(result[i_R] - otherDenseVector[i_R]) < Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Multiply(DenseMatrix, SparseVector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(DenseMatrix,SparseVector)")]
        public void Static_Multiply_DenseMatrix_SparseVector()
        {
            // Arrange
            DenseMatrix matrix = new DenseMatrix(3, 2, new double[] { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            SparseVector sparseVector = new SparseVector(2, new int[2] { 0, 1 }, new double[2] { -2.0, 6.0 });

            SparseVector result = new SparseVector(3, new int[3] { 0, 1, 2 }, new double[3] { 10.0, -34.0, 14.0 });

            // Act
            DenseVector otherSparseVector = DenseMatrix.Multiply(matrix, sparseVector);

            // Assert
            Assert.AreEqual(result.Size, otherSparseVector.Size);

            for (int i_R = 0; i_R < result.Size; i_R++)
            {
                Assert.IsTrue(Math.Abs(result[i_R] - otherSparseVector[i_R]) < Settings.AbsolutePrecision);
            }
        }


        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.TransposeMultiply(DenseMatrix, Vector)"/>.
        /// </summary>
        [TestMethod("Static TransposeMultiply(DenseMatrix,Vector)")]
        public void Static_TransposeMultiply_DenseMatrix_Vector()
        {
            // Arrange
            DenseMatrix matrix = new DenseMatrix(3, 2, new double[] { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            DenseVector denseVector = new DenseVector(new double[3] { -2.0, 0.0, 6.0 });
            SparseVector sparseVector = new SparseVector(3, new int[2] { 0, 2 }, new double[2] { -2.0, 6.0 });

            DenseVector result = new DenseVector(new double[2] { -32.0, 0.0 });

            //Act
            DenseVector otherDenseVector = DenseMatrix.TransposeMultiply(matrix, denseVector);
            DenseVector otherSparseVector = DenseMatrix.TransposeMultiply(matrix, sparseVector);

            // Assert
            Assert.AreEqual(result.Size, otherDenseVector.Size);
            Assert.AreEqual(result.Size, otherSparseVector.Size);

            for (int i_R = 0; i_R < result.Size; i_R++)
            {
                Assert.IsTrue(Math.Abs(result[i_R] - otherDenseVector[i_R]) < Settings.AbsolutePrecision);
                Assert.IsTrue(Math.Abs(result[i_R] - otherSparseVector[i_R]) < Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.TransposeMultiply(DenseMatrix, DenseVector)"/>.
        /// </summary>
        [TestMethod("Static TransposeMultiply(DenseMatrix,DenseVector)")]
        public void Static_TransposeMultiply_DenseMatrix_DenseVector()
        {
            // Arrange
            DenseMatrix matrix = new DenseMatrix(3, 2, new double[] { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            DenseVector denseVector = new DenseVector(new double[3] { -2.0, 0.0, 6.0 });

            DenseVector result = new DenseVector(new double[2] { -32.0, 0.0 });

            //Act
            DenseVector otherDenseVector = DenseMatrix.TransposeMultiply(matrix, denseVector);

            // Assert
            Assert.AreEqual(result.Size, otherDenseVector.Size);

            for (int i_R = 0; i_R < result.Size; i_R++)
            {
                Assert.IsTrue(Math.Abs(result[i_R] - otherDenseVector[i_R]) < Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.TransposeMultiply(DenseMatrix, SparseVector)"/>.
        /// </summary>
        [TestMethod("Static TransposeMultiply(DenseMatrix,SparseVector)")]
        public void Static_TransposeMultiply_DenseMatrix_SparseVector()
        {
            // Arrange
            DenseMatrix matrix = new DenseMatrix(3, 2, new double[] { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            SparseVector sparseVector = new SparseVector(3, new int[2] { 0, 2 }, new double[2] { -2.0, 6.0 });

            DenseVector result = new DenseVector(new double[2] { -32.0, 0.0 });

            //Act
            DenseVector otherSparseVector = DenseMatrix.TransposeMultiply(matrix, sparseVector);

            // Assert
            Assert.AreEqual(result.Size, otherSparseVector.Size);

            for (int i_R = 0; i_R < result.Size; i_R++)
            {
                Assert.IsTrue(Math.Abs(result[i_R] - otherSparseVector[i_R]) < Settings.AbsolutePrecision);
            }
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Tests the method <see cref="DenseMatrix.Transpose()"/>.
        /// </summary>
        [TestMethod("Method Transpose()")]
        public void Transpose()
        {
            // Arrange
            DenseMatrix otherMatrix = new DenseMatrix(4, 2, new double[] { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0, 9.0, 8.0 });

            DenseMatrix matrix = new DenseMatrix(2, 4, new double[] { 1.0, 3.0, 6.0, 9.0, 2.0, 5.0, 7.0, 8.0 });

            // Act 
            matrix.Transpose();

            // Assert
            Assert.AreEqual(matrix.RowCount, otherMatrix.RowCount);
            Assert.AreEqual(matrix.ColumnCount, otherMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }

        #endregion

    }
}
