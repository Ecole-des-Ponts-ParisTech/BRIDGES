using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.LinearAlgebra.Vectors;
using BRIDGES.LinearAlgebra.Matrices;
using BRIDGES.LinearAlgebra.Matrices.Sparse;
using Stor = BRIDGES.LinearAlgebra.Matrices.Storage;


namespace BRIDGES.Test.LinearAlgebra.Matrices.Sparse
{
    /// <summary>
    /// Class testing the members of the <see cref="CompressedRow"/> class.
    /// </summary>
    [TestClass]
    public class CompressedRowTest
    {
        #region Behavior

        /// <summary>
        /// Tests that <see cref="CompressedRow"/> is reference type.
        /// </summary>
        [TestMethod("Behavior IsReference")]
        public void CompressedRow_IsReference()
        {
            // Arrange
            CompressedRow matrix = new CompressedRow(2, 2, new int[3] { 0, 2, 4 }, new List<int> { 0, 1, 0, 1 }, new List<double> { 1.0, 2.0, 3.0, 4.0 });
            CompressedRow otherMatrix = new CompressedRow(1, 2, new int[2] { 0, 2 }, new List<int> { 0, 1 }, new List<double> { 2.0, 4.0 });
            //Act
            otherMatrix = matrix;
            // Assert
            Assert.AreEqual(matrix, otherMatrix);
            Assert.AreSame(matrix, otherMatrix);
        }

        #endregion


        #region Properties

        /// <summary>
        /// Tests the <see cref="CompressedRow.RowCount"/> and <see cref="CompressedRow.ColumnCount"/> properties.
        /// </summary>
        [TestMethod("Property RowCount & ColumnCount")]
        public void RowAndColumnCount()
        {
            // Arrange
            CompressedRow matrix = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 });

            // Act

            // Assert
            Assert.AreEqual(2, matrix.RowCount);
            Assert.AreEqual(3, matrix.ColumnCount);
        }

        /// <summary>
        /// Tests the <see cref="CompressedRow"/> indexer property.
        /// </summary>
        [TestMethod("Property this[int,int]")]
        public void Index_Int_Int()
        {
            // Arrange
            CompressedRow matrix = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 });

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
        /// Tests the initialisation of the <see cref="CompressedRow"/> from its size and values.
        /// </summary>
        [TestMethod("Constructor(Int,Int,DictionaryOfKeys)")]
        public void Constructor_Int_Int_DictionaryOfKeys()
        {
            // Arrange
            Stor.DictionaryOfKeys dok = new Stor.DictionaryOfKeys();
            dok.Add(1.0, 0, 1);
            dok.Add(2.0, 1, 0);

            // Act
            CompressedRow matrix = new CompressedRow(2, 2, dok);

            // Assert
            Assert.IsTrue(Math.Abs(matrix[0, 0]) < Settings.AbsolutePrecision);
            Assert.IsTrue(Math.Abs(matrix[0, 1] - 1.0) < Settings.AbsolutePrecision);
            Assert.IsTrue(Math.Abs(matrix[1, 0] - 2.0) < Settings.AbsolutePrecision);
            Assert.IsTrue(Math.Abs(matrix[1, 1]) < Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="CompressedRow"/> from its size and components.
        /// </summary>
        [TestMethod("Constructor(Int,Int,Double)")]
        public void Constructor_Int_Int_IntArray_IntList_DoubleList()
        {
            // Arrange
            int[] rowPointers = new int[3] { 0, 3, 6 };
            List<int> columnIndices = new List<int> { 0, 1, 2, 0, 1, 2 };
            List<double> values = new List<double> { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 };

            // Act
            CompressedRow matrix = new CompressedRow(2, 3, rowPointers, columnIndices, values);

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
        /// Tests the static property <see cref="CompressedRow.Zero"/>.
        /// </summary>
        [TestMethod("Static Zero")]
        public void Static_Zero()
        {
            // Arrange

            // Act
            CompressedRow matrix = CompressedRow.Zero(5, 4);

            // Assert
            Assert.AreEqual(5, matrix.RowCount);
            Assert.AreEqual(4, matrix.ColumnCount);

            Assert.AreEqual(0, matrix.NonZerosCount);

            for (int r = 0; r < 5; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    Assert.AreEqual(0.0, matrix[r, c]);
                }
            }
        }

        /// <summary>
        /// Tests the static property <see cref="CompressedRow.Identity"/>.
        /// </summary>
        [TestMethod("Static Identity")]
        public void Static_Identity()
        {
            // Arrange

            // Act
            CompressedRow matrix = CompressedRow.Identity(4);

            // Assert
            Assert.AreEqual(4, matrix.RowCount);
            Assert.AreEqual(4, matrix.ColumnCount);

            Assert.AreEqual(4, matrix.NonZerosCount);

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
        /// Tests the static method <see cref="CompressedRow.Add(CompressedRow, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Add(CompressedRow,CompressedRow)")]
        public void Static_Add_CompressedRow_CompressedRow()
        {
            // Arrange
            CompressedRow left = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            CompressedRow right = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            CompressedRow matrix = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 5.0, 5.0, 5.0, 10.0, 10.0, 10.0 });

            //Act
            CompressedRow otherMatrix = CompressedRow.Add(left, right);

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
        /// Tests the static method <see cref="CompressedRow.Subtract(CompressedRow, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Subtract(CompressedRow,CompressedRow)")]
        public void Static_Subtract_CompressedRow_CompressedRow()
        {
            // Arrange
            CompressedRow left = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            CompressedRow right = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            CompressedRow matrix = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { -3.0, -1.0, 1.0, 0.0, 2.0, 4.0 });

            //Act
            CompressedRow otherMatrix = CompressedRow.Subtract(left, right);

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
        /// Tests the static method <see cref="CompressedRow.Multiply(CompressedRow, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedRow,CompressedRow)")]
        public void Static_Multiply_CompressedRow_CompressedRow()
        {
            // Arrange
            CompressedRow left = new CompressedRow(6, 5, new int[7] { 0, 2, 3, 4, 5, 8, 9 },
                new List<int> { 1, 3, 3, 2, 3, 1, 2, 3, 4 }, new List<double> { 1.5, 1.25, 6.75, 2.0, 5.5, 4.0, 3.5, 2.25, 7.25 });
            CompressedRow right = new CompressedRow(5, 3, new int[6] { 0, 1, 2, 3, 6, 8 },
                new List<int> { 0, 1, 0, 0, 1, 2, 0, 1 }, new List<double> { 3.5, 1.5, 5.0, 2.0, 3.0, 4.0, 0.5, 2.5 });

            CompressedRow matrix = new CompressedRow(6, 3, new int[7] { 0, 3, 6, 7, 10, 13, 15 },
                new List<int> { 0, 1, 2, 0, 1, 2, 0, 0, 1, 2, 0, 1, 2, 0, 1 },
                new List<double> { 2.5, 6.0, 5.0, 13.5, 20.25, 27.0, 10.0, 11.0, 16.5, 22.0, 22.0, 12.75, 9.0, 3.625, 18.125 });

            //Act
            CompressedRow otherMatrix = CompressedRow.Multiply(left, right);

            // Assert
            Assert.AreEqual(matrix.RowCount, otherMatrix.RowCount);
            Assert.AreEqual(matrix.ColumnCount, otherMatrix.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.AreEqual(matrix[i_R, i_C], otherMatrix[i_R, i_C]);
                    //Assert.IsTrue(Math.Abs(matrix[i_R, i_C] - otherMatrix[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }


        /******************** Sparse Matrix Embedding ********************/

        /// <summary>
        /// Tests the static method <see cref="CompressedRow.Add(CompressedRow, SparseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Add(CompressedRow,SparseMatrix)")]
        public void Static_Add_CompressedRow_SparseMatrix()
        {
            // Arrange
            CompressedRow left = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            SparseMatrix crsRight = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });
            SparseMatrix ccsright = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new int[6] { 0, 1, 0, 1, 0, 1 }, new double[6] { 4.0, 5.0, 3.0, 4.0, 2.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { 5.0, 5.0, 5.0, 10.0, 10.0, 10.0 });

            //Act
            CompressedRow otherCrsMatrix = CompressedRow.Add(left, crsRight);
            CompressedRow otherCcsMatrix = CompressedRow.Add(left, ccsright);

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
        /// Tests the static method <see cref="CompressedRow.Add(SparseMatrix, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Add(SparseMatrix,CompressedRow)")]
        public void Static_Add_SparseMatrix_CompressedRow()
        {
            // Arrange
            SparseMatrix crsLeft = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            SparseMatrix ccsLeft = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new int[6] { 0, 1, 0, 1, 0, 1 }, new double[6] { 1.0, 5.0, 2.0, 6.0, 3.0, 7.0 });
            CompressedRow right = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { 5.0, 5.0, 5.0, 10.0, 10.0, 10.0 });

            //Act
            CompressedRow otherCrsMatrix = CompressedRow.Add(crsLeft, right);
            CompressedRow otherCcsMatrix = CompressedRow.Add(ccsLeft, right);

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
        /// Tests the static method <see cref="CompressedRow.Subtract(CompressedRow, SparseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Subtract(CompressedRow,SparseMatrix)")]
        public void Static_Subtract_CompressedRow_SparseMatrix()
        {
            // Arrange
            CompressedRow left = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            SparseMatrix crsRight = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });
            SparseMatrix ccsright = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new int[6] { 0, 1, 0, 1, 0, 1 }, new double[6] { 4.0, 5.0, 3.0, 4.0, 2.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { -3.0, -1.0, 1.0, 0.0, 2.0, 4.0 });

            //Act
            CompressedRow otherCrsMatrix = CompressedRow.Subtract(left, crsRight);
            CompressedRow otherCcsMatrix = CompressedRow.Subtract(left, ccsright);

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
        /// Tests the static method <see cref="CompressedRow.Subtract(SparseMatrix, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Subtract(SparseMatrix,CompressedRow)")]
        public void Static_Subtract_SparseMatrix_CompressedRow()
        {
            // Arrange
            SparseMatrix crsLeft = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            SparseMatrix ccsLeft = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new int[6] { 0, 1, 0, 1, 0, 1 }, new double[6] { 1.0, 5.0, 2.0, 6.0, 3.0, 7.0 });
            CompressedRow right = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { -3.0, -1.0, 1.0, 0.0, 2.0, 4.0 });

            //Act
            CompressedRow otherCrsMatrix = CompressedRow.Subtract(crsLeft, right);
            CompressedRow otherCcsMatrix = CompressedRow.Subtract(ccsLeft, right);

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
        /// Tests the static method <see cref="CompressedRow.Multiply(CompressedRow, SparseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedRow,SparseMatrix)")]
        public void Static_Multiply_CompressedRow_SparseMatrix()
        {
            // Arrange
            CompressedRow left = new CompressedRow(4, 2, new int[5] { 0, 2, 4, 6, 8 },
                new List<int> { 0, 1, 0, 1, 0, 1, 0, 1 }, new List<double> { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0, 9.0, 8.0 });
            SparseMatrix crsRight = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });
            SparseMatrix ccsRight = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new int[6] { 0, 1, 0, 1, 0, 1 }, new double[6] { 4.0, 5.0, 3.0, 4.0, 2.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(4, 3, new double[] { 14.0, 11.0, 8.0, 37.0, 29.0, 21.0, 59.0, 46.0, 33.0, 76.0, 59.0, 42.0 });

            //Act
            CompressedRow otherCrsMatrix = CompressedRow.Multiply(left, crsRight);
            CompressedRow otherCcsMatrix = CompressedRow.Multiply(left, ccsRight);

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
        /// Tests the static method <see cref="CompressedRow.Multiply(SparseMatrix, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Multiply(SparseMatrix,CompressedRow)")]
        public void Static_Multiply_SparseMatrix_CompressedRow()
        {
            // Arrange
            SparseMatrix crsLeft = new CompressedRow(4, 2, new int[5] { 0, 2, 4, 6, 8 },
                new List<int> { 0, 1, 0, 1, 0, 1, 0, 1 }, new List<double> { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0, 9.0, 8.0 });
            SparseMatrix ccsLeft = new CompressedColumn(4, 2, new int[3] { 0, 4, 8 },
                new int[8] { 0, 1, 2, 3, 0, 1, 2, 3 }, new double[8] { 1.0, 3.0, 6.0, 9.0, 2.0, 5.0, 7.0, 8.0 });
            CompressedRow right = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(4, 3, new double[] { 14.0, 11.0, 8.0, 37.0, 29.0, 21.0, 59.0, 46.0, 33.0, 76.0, 59.0, 42.0 });

            //Act
            CompressedRow otherCrsMatrix = CompressedRow.Multiply(crsLeft, right);
            CompressedRow otherCcsMatrix = CompressedRow.Multiply(ccsLeft, right);

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
        /// Tests the static method <see cref="CompressedRow.Add(CompressedRow, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Add(CompressedRow,CompressedColumn)")]
        public void Static_Add_CompressedRow_CompressedColumn()
        {
            // Arrange
            CompressedRow left = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            CompressedColumn ccsright = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new int[6] { 0, 1, 0, 1, 0, 1 }, new double[6] { 4.0, 5.0, 3.0, 4.0, 2.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { 5.0, 5.0, 5.0, 10.0, 10.0, 10.0 });

            //Act
            CompressedRow otherCrsMatrix = CompressedRow.Add(left, ccsright);

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
        /// Tests the static method <see cref="CompressedRow.Add(CompressedColumn, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Add(CompressedColumn,CompressedRow)")]
        public void Static_Add_CompressedColumn_CompressedRow()
        {
            // Arrange
            CompressedColumn ccsLeft = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new int[6] { 0, 1, 0, 1, 0, 1 }, new double[6] { 1.0, 5.0, 2.0, 6.0, 3.0, 7.0 });
            CompressedRow right = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { 5.0, 5.0, 5.0, 10.0, 10.0, 10.0 });

            //Act
            CompressedRow otherCrsMatrix = CompressedRow.Add(ccsLeft, right);

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
        /// Tests the static method <see cref="CompressedRow.Subtract(CompressedRow, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Subtract(CompressedRow,CompressedColumn)")]
        public void Static_Subtract_CompressedRow_CompressedColumn()
        {
            // Arrange
            CompressedRow left = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            CompressedColumn ccsRight = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new int[6] { 0, 1, 0, 1, 0, 1 }, new double[6] { 4.0, 5.0, 3.0, 4.0, 2.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { -3.0, -1.0, 1.0, 0.0, 2.0, 4.0 });

            //Act
            CompressedRow otherCrsMatrix = CompressedRow.Subtract(left, ccsRight);

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
        /// Tests the static method <see cref="CompressedRow.Subtract(CompressedColumn, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Subtract(CompressedColumn,CompressedRow)")]
        public void Static_Subtract_CompressedColumn_CompressedRow()
        {
            // Arrange
            CompressedColumn ccsLeft = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new int[6] { 0, 1, 0, 1, 0, 1 }, new double[6] { 1.0, 5.0, 2.0, 6.0, 3.0, 7.0 });
            CompressedRow right = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { -3.0, -1.0, 1.0, 0.0, 2.0, 4.0 });

            //Act
            CompressedRow otherCrsMatrix = CompressedRow.Subtract(ccsLeft, right);

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
        /// Tests the static method <see cref="CompressedRow.Multiply(CompressedRow, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedRow,CompressedColumn)")]
        public void Static_Multiply_CompressedRow_CompressedColumn()
        {
            // Arrange
            CompressedRow left = new CompressedRow(4, 2, new int[5] { 0, 2, 4, 6, 8 },
                new List<int> { 0, 1, 0, 1, 0, 1, 0, 1 }, new List<double> { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0, 9.0, 8.0 });
            CompressedColumn ccsRight = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new int[6] { 0, 1, 0, 1, 0, 1 }, new double[6] { 4.0, 5.0, 3.0, 4.0, 2.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(4, 3, new double[] { 14.0, 11.0, 8.0, 37.0, 29.0, 21.0, 59.0, 46.0, 33.0, 76.0, 59.0, 42.0 });

            //Act
            CompressedRow otherCcsMatrix = CompressedRow.Multiply(left, ccsRight);

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
        /// Tests the static method <see cref="CompressedRow.Multiply(CompressedColumn, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedColumn,CompressedRow)")]
        public void Static_Multiply_CompressedColumn_CompressedRow()
        {
            // Arrange
            CompressedColumn ccsLeft = new CompressedColumn(4, 2, new int[3] { 0, 4, 8 },
                new int[8] { 0, 1, 2, 3, 0, 1, 2, 3 }, new double[8] { 1.0, 3.0, 6.0, 9.0, 2.0, 5.0, 7.0, 8.0 });
            CompressedRow right = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });


            DenseMatrix matrix = new DenseMatrix(4, 3, new double[] { 14.0, 11.0, 8.0, 37.0, 29.0, 21.0, 59.0, 46.0, 33.0, 76.0, 59.0, 42.0 });

            //Act
            CompressedRow otherCcsMatrix = CompressedRow.Multiply(ccsLeft, right);

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


        /******************** Group Action ********************/

        /// <summary>
        /// Tests the static method <see cref="CompressedRow.Multiply(double, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Multiply(double,CompressedRow)")]
        public void Static_Multiply_Double_CompressedRow()
        {
            // Arrange
            double factor = -2.5;
            CompressedRow operand = new CompressedRow(3, 2, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });

            CompressedRow matrix = new CompressedRow(3, 2, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { -10.0, -7.5, -5.0, 12.5, 10.0, -2.5 });

            //Act
            CompressedRow otherMatrix = CompressedRow.Multiply(factor, operand);

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
        /// Tests the static method <see cref="CompressedRow.Multiply(CompressedRow, double)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedRow,double)")]
        public void Static_Multiply_CompressedRow_Double()
        {
            // Arrange
            CompressedRow operand = new CompressedRow(3, 2, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            double factor = -2.5;

            CompressedRow matrix = new CompressedRow(3, 2, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { -10.0, -7.5, -5.0, 12.5, 10.0, -2.5 });

            //Act
            CompressedRow otherMatrix = CompressedRow.Multiply(operand, factor);

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
        /// Tests the static method <see cref="CompressedRow.Divide(CompressedRow, double)"/>.
        /// </summary>
        [TestMethod("Static Divide(CompressedRow,double)")]
        public void Static_Divide_CompressedRow_Double()
        {
            // Arrange
            CompressedRow operand = new CompressedRow(2, 1, new int[3] { 0, 1, 2 },
                new List<int> { 0, 0 }, new List<double> { 4.0, 3.0 });
            double divisor = -2.0;

            CompressedRow matrix = new CompressedRow(2, 1, new int[3] { 0, 1, 2 },
                new List<int> { 0, 0 }, new List<double> { -2.0, -1.5 });

            //Act
            CompressedRow otherMatrix = CompressedRow.Divide(operand, divisor);

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
        /// Tests the static method <see cref="CompressedRow.Multiply(CompressedRow, Vector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedRow,Vector)")]
        public void Static_Multiply_CompressedRow_Vector()
        {
            // Arrange
            CompressedRow matrix = new CompressedRow(3, 2, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            Vector denseVector = new DenseVector(new double[2] { -2.0, 6.0 });
            Vector sparseVector = new SparseVector(2, new int[2] { 0, 1 }, new double[2] { -2.0, 6.0 });

            Vector result = new DenseVector(new double[3] { 10.0, -34.0, 14.0 });

            //Act
            Vector otherDenseVector = CompressedRow.Multiply(matrix, denseVector);
            Vector otherSparseVector = CompressedRow.Multiply(matrix, sparseVector);

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
        /// Tests the static method <see cref="CompressedRow.Multiply(CompressedRow, DenseVector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedRow,DenseVector)")]
        public void Static_Multiply_CompressedRow_DenseVector()
        {
            // Arrange
            CompressedRow matrix = new CompressedRow(3, 2, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            DenseVector denseVector = new DenseVector(new double[2] { -2.0, 6.0 });

            DenseVector result = new DenseVector(new double[3] { 10.0, -34.0, 14.0 });

            //Act
            DenseVector otherDenseVector = CompressedRow.Multiply(matrix, denseVector);

            // Assert
            Assert.AreEqual(result.Size, otherDenseVector.Size);

            for (int i_R = 0; i_R < result.Size; i_R++)
            {
                Assert.IsTrue(Math.Abs(result[i_R] - otherDenseVector[i_R]) < Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="CompressedRow.Multiply(CompressedRow, SparseVector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedRow,SparseVector)")]
        public void Static_Multiply_CompressedRow_SparseVector()
        {
            // Arrange
            CompressedRow matrix = new CompressedRow(3, 2, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            SparseVector sparseVector = new SparseVector(2, new int[2] { 0, 1 }, new double[2] { -2.0, 6.0 });

            SparseVector result = new SparseVector(3, new int[3] { 0, 1, 2 }, new double[3] { 10.0, -34.0, 14.0 });

            // Act
            SparseVector otherSparseVector = CompressedRow.Multiply(matrix, sparseVector);

            // Assert
            Assert.AreEqual(result.Size, otherSparseVector.Size);

            for (int i_R = 0; i_R < result.Size; i_R++)
            {
                Assert.IsTrue(Math.Abs(result[i_R] - otherSparseVector[i_R]) < Settings.AbsolutePrecision);
            }
        }


        /// <summary>
        /// Tests the static method <see cref="CompressedRow.TransposeMultiply(CompressedRow, Vector)"/>.
        /// </summary>
        [TestMethod("Static TransposeMultiply(CompressedRow,Vector)")]
        public void Static_TransposeMultiply_CompressedRow_Vector()
        {
            // Arrange
            CompressedRow matrix = new CompressedRow(3, 2, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            DenseVector denseVector = new DenseVector(new double[3] { -2.0, 0.0, 6.0 });
            SparseVector sparseVector = new SparseVector(3, new int[2] { 0, 2 }, new double[2] { -2.0, 6.0 });

            DenseVector result = new DenseVector(new double[2] { -32.0, 0.0 });

            //Act
            DenseVector otherDenseVector = CompressedRow.TransposeMultiply(matrix, denseVector);
            SparseVector otherSparseVector = CompressedRow.TransposeMultiply(matrix, sparseVector);

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
        /// Tests the static method <see cref="CompressedRow.TransposeMultiply(CompressedRow, DenseVector)"/>.
        /// </summary>
        [TestMethod("Static TransposeMultiply(CompressedRow,DenseVector)")]
        public void Static_TransposeMultiply_CompressedRow_DenseVector()
        {
            // Arrange
            CompressedRow matrix = new CompressedRow(3, 2, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            DenseVector denseVector = new DenseVector(new double[3] { -2.0, 0.0, 6.0 });

            DenseVector result = new DenseVector(new double[2] { -32.0, 0.0 });

            //Act
            DenseVector otherDenseVector = CompressedRow.TransposeMultiply(matrix, denseVector);

            // Assert
            Assert.AreEqual(result.Size, otherDenseVector.Size);

            for (int i_R = 0; i_R < result.Size; i_R++)
            {
                Assert.IsTrue(Math.Abs(result[i_R] - otherDenseVector[i_R]) < Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="CompressedRow.TransposeMultiply(CompressedRow, SparseVector)"/>.
        /// </summary>
        [TestMethod("Static TransposeMultiply(CompressedRow,SparseVector)")]
        public void Static_TransposeMultiply_CompressedRow_SparseVector()
        {
            // Arrange
            CompressedRow matrix = new CompressedRow(3, 2, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });
            SparseVector sparseVector = new SparseVector(3, new int[2] { 0, 2 }, new double[2] { -2.0, 6.0 });

            DenseVector result = new DenseVector(new double[2] { -32.0, 0.0 });

            //Act
            SparseVector otherSparseVector = CompressedRow.TransposeMultiply(matrix, sparseVector);

            // Assert
            Assert.AreEqual(result.Size, otherSparseVector.Size);

            for (int i_R = 0; i_R < result.Size; i_R++)
            {
                Assert.IsTrue(Math.Abs(result[i_R] - otherSparseVector[i_R]) < Settings.AbsolutePrecision);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Tests the method <see cref="CompressedRow.ToCompressedColumn()"/>.
        /// </summary>
        [TestMethod("Method ToCompressedColumn()")]
        public void ToCompressedColumn()
        {
            // Arrange
            CompressedRow crs = new CompressedRow(6, 5, new int[7] { 0, 2, 3, 4, 5, 8, 9 },
                new List<int> { 1, 3, 3, 2, 3, 1, 2, 3, 4 }, new List<double> { 1.5, 1.25, 6.75, 2.0, 5.5, 4.0, 3.5, 2.25, 7.25 });

            CompressedColumn result = new CompressedColumn(6, 5, new int[6] { 0, 0, 2, 4, 8, 9 },
                new int[9] { 0, 4, 2, 4, 0, 1, 3, 4, 5 }, new double[9] { 1.5, 4.0, 2.0, 3.5, 1.25, 6.75, 5.5, 2.25, 7.25 });

            // Act
            CompressedColumn ccs = crs.ToCompressedColumn();

            // Assert
            Assert.AreEqual(result.RowCount, ccs.RowCount);
            Assert.AreEqual(result.ColumnCount, ccs.ColumnCount);

            for (int i_R = 0; i_R < result.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < result.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(result[i_R, i_C] - ccs[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }

        #endregion
    }
}
