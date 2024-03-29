﻿using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.LinearAlgebra.Vectors;
using BRIDGES.LinearAlgebra.Matrices;
using BRIDGES.LinearAlgebra.Matrices.Sparse;
using Stor = BRIDGES.LinearAlgebra.Matrices.Storage;


namespace BRIDGES.Test.LinearAlgebra.Matrices.Sparse
{
    /// <summary>
    /// Class testing the members of the <see cref="CompressedColumn"/> class.
    /// </summary>
    [TestClass]
    public class CompressedColumnTest
    {
        #region Behavior

        /// <summary>
        /// Tests that <see cref="CompressedColumn"/> is reference type.
        /// </summary>
        [TestMethod("Behavior IsReference")]
        public void CompressedColumn_IsReference()
        {
            // Arrange
            CompressedColumn matrix = new CompressedColumn(2, 2, new int[3] { 0, 2, 4 }, new List<int> { 0, 1, 0, 1 }, new List<double> { 1.0, 3.0, 2.0, 4.0 });
            CompressedColumn otherMatrix = new CompressedColumn(1, 2, new int[3] { 0, 1, 2 }, new List<int> { 0, 1 }, new List<double> { 2.0, 4.0 });
            //Act
            otherMatrix = matrix;
            // Assert
            Assert.AreEqual(matrix, otherMatrix);
            Assert.AreSame(matrix, otherMatrix);
        }

        #endregion


        #region Properties

        /// <summary>
        /// Tests the <see cref="CompressedColumn.RowCount"/> and <see cref="CompressedColumn.ColumnCount"/> properties.
        /// </summary>
        [TestMethod("Property RowCount & ColumnCount")]
        public void RowAndColumnCount()
        {
            // Arrange
            CompressedColumn matrix = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 1.0, 4.0, 2.0, 5.0, 3.0, 6.0 });

            // Act

            // Assert
            Assert.AreEqual(2, matrix.RowCount);
            Assert.AreEqual(3, matrix.ColumnCount);
        }

        /// <summary>
        /// Tests the <see cref="CompressedColumn"/> indexer property.
        /// </summary>
        [TestMethod("Property this[int,int]")]
        public void Index_Int_Int()
        {
            // Arrange
            CompressedColumn matrix = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 1.0, 4.0, 2.0, 5.0, 3.0, 6.0 });

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
        /// Tests the initialisation of the <see cref="CompressedColumn"/> from its size and values.
        /// </summary>
        [TestMethod("Constructor(Int,Int,DictionaryOfKeys)")]
        public void Constructor_Int_Int_DictionaryOfKeys()
        {
            // Arrange
            Stor.DictionaryOfKeys dok = new Stor.DictionaryOfKeys();
            dok.Add(1.0, 0, 1);
            dok.Add(2.0, 1, 0);

            // Act
            CompressedColumn matrix = new CompressedColumn(2, 2, dok);

            // Assert
            Assert.IsTrue(Math.Abs(matrix[0, 0]) < Settings.AbsolutePrecision);
            Assert.IsTrue(Math.Abs(matrix[0, 1] - 1.0) < Settings.AbsolutePrecision);
            Assert.IsTrue(Math.Abs(matrix[1, 0] - 2.0) < Settings.AbsolutePrecision);
            Assert.IsTrue(Math.Abs(matrix[1, 1]) < Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="CompressedColumn"/> from its size and components.
        /// </summary>
        [TestMethod("Constructor(Int,Int,Double)")]
        public void Constructor_Int_Int_IntArray_IntList_DoubleList()
        {
            // Arrange
            int[] columnPointers = new int[4] { 0, 2, 4, 6 };
            List<int> rowIndices = new List<int> { 0, 1, 0, 1, 0, 1 };
            List<double> values = new List<double> { 1.0, 4.0, 2.0, 5.0, 3.0, 6.0 };

            // Act
            CompressedColumn matrix = new CompressedColumn(2, 3, columnPointers, rowIndices, values);

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
        /// Tests the static property <see cref="CompressedColumn.Zero"/>.
        /// </summary>
        [TestMethod("Static Zero")]
        public void Static_Zero()
        {
            // Arrange

            // Act
            CompressedColumn matrix = CompressedColumn.Zero(5, 4);

            // Assert
            Assert.AreEqual(5, matrix.RowCount);
            Assert.AreEqual(4, matrix.ColumnCount);

            Assert.AreEqual(0, matrix.NonZeroCount);

            for (int r = 0; r < 5; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    Assert.AreEqual(0.0, matrix[r, c]);
                }
            }
        }

        /// <summary>
        /// Tests the static property <see cref="CompressedColumn.Identity"/>.
        /// </summary>
        [TestMethod("Static Identity")]
        public void Static_Identity()
        {
            // Arrange

            // Act
            CompressedColumn matrix = CompressedColumn.Identity(4);

            // Assert
            Assert.AreEqual(4, matrix.RowCount);
            Assert.AreEqual(4, matrix.ColumnCount);

            Assert.AreEqual(4, matrix.NonZeroCount);

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
        /// Tests the static method <see cref="CompressedColumn.Add(CompressedColumn, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Add(CompressedColumn,CompressedColumn)")]
        public void Static_Add_CompressedColumn_CompressedColumn()
        {
            // Arrange
            CompressedColumn left = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 1.0, 5.0, 2.0, 6.0, 3.0, 7.0 });
            CompressedColumn right = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 5.0, 3.0, 4.0, 2.0, 3.0 });

            CompressedColumn matrix = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 5.0, 10.0, 5.0, 10.0, 5.0, 10.0 });

            //Act
            CompressedColumn otherMatrix = CompressedColumn.Add(left, right);

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
        /// Tests the static method <see cref="CompressedColumn.Subtract(CompressedColumn, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Subtract(CompressedColumn,CompressedColumn)")]
        public void Static_Subtract_CompressedColumn_CompressedColumn()
        {
            // Arrange
            CompressedColumn left = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 1.0, 5.0, 2.0, 6.0, 3.0, 7.0 });
            CompressedColumn right = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 5.0, 3.0, -4.0, -2.0, 3.0 });

            CompressedColumn matrix = new CompressedColumn(2, 3, new int[4] { 0, 1, 3, 5 },
                new List<int> { 0, 0, 1, 0, 1 }, new List<double> { -3.0, -1.0, 10.0, 5.0, 4.0 });

            //Act
            CompressedColumn otherMatrix = CompressedColumn.Subtract(left, right);

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
        /// Tests the static method <see cref="CompressedColumn.Multiply(CompressedColumn, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedColumn,CompressedColumn)")]
        public void Static_Multiply_CompressedColumn_CompressedColumn()
        {
            // Arrange
            CompressedColumn left = new CompressedColumn(6, 5, new int[6] { 0, 0, 2, 4, 8, 9 },
                new List<int> { 0, 4, 2, 4, 0, 1, 3, 4, 5 }, new List<double> { 1.5, 4.0, 2.0, 3.5, 1.25, 6.75, 5.5, 2.25, 7.25 });
            CompressedColumn right = new CompressedColumn(5, 3, new int[4] { 0, 4, 7, 8 },
                new List<int> { 0, 2, 3, 4, 1, 3, 4, 3 }, new List<double> { 3.5, 5.0, 2.0, 0.5, 1.5, 3.0, 2.5, 4.0 });

            CompressedColumn matrix = new CompressedColumn(6, 3, new int[4] { 0, 6, 11, 15 },
                new List<int> { 0, 1, 2, 3, 4, 5, 0, 1, 3, 4, 5, 0, 1, 3, 4 },
                new List<double> { 2.5, 13.5, 10.0, 11.0, 22.0, 3.625, 6.0, 20.25, 16.5, 12.75, 18.125, 5.0, 27.0, 22.0, 9.0 });

            //Act
            CompressedColumn otherMatrix = CompressedColumn.Multiply(left, right);

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
        /// Tests the static method <see cref="CompressedColumn.Add(CompressedColumn, SparseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Add(CompressedColumn,SparseMatrix)")]
        public void Static_Add_CompressedColumn_SparseMatrix()
        {
            // Arrange
            CompressedColumn left = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 1.0, 5.0, 2.0, 6.0, 3.0, 7.0 });
            SparseMatrix crsRight = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });
            SparseMatrix ccsright = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 5.0, 3.0, 4.0, 2.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { 5.0, 5.0, 5.0, 10.0, 10.0, 10.0 });

            //Act
            CompressedColumn otherCrsMatrix = CompressedColumn.Add(left, crsRight);
            CompressedColumn otherCcsMatrix = CompressedColumn.Add(left, ccsright);

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
        /// Tests the static method <see cref="CompressedColumn.Add(SparseMatrix, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Add(SparseMatrix,CompressedColumn)")]
        public void Static_Add_SparseMatrix_CompressedColumn()
        {
            // Arrange
            SparseMatrix crsLeft = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            SparseMatrix ccsLeft = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 1.0, 5.0, 2.0, 6.0, 3.0, 7.0 });
            CompressedColumn right = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 5.0, 3.0, 4.0, 2.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { 5.0, 5.0, 5.0, 10.0, 10.0, 10.0 });

            //Act
            CompressedColumn otherCrsMatrix = CompressedColumn.Add(crsLeft, right);
            CompressedColumn otherCcsMatrix = CompressedColumn.Add(ccsLeft, right);

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
        /// Tests the static method <see cref="CompressedColumn.Subtract(CompressedColumn, SparseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Subtract(CompressCompressedColumnedRow,SparseMatrix)")]
        public void Static_Subtract_CompressedColumn_SparseMatrix()
        {
            // Arrange
            CompressedColumn left = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 1.0, 5.0, 2.0, 6.0, 3.0, 7.0 });
            SparseMatrix crsRight = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });
            SparseMatrix ccsright = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 5.0, 3.0, 4.0, 2.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { -3.0, -1.0, 1.0, 0.0, 2.0, 4.0 });

            //Act
            CompressedColumn otherCrsMatrix = CompressedColumn.Subtract(left, crsRight);
            CompressedColumn otherCcsMatrix = CompressedColumn.Subtract(left, ccsright);

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
        /// Tests the static method <see cref="CompressedColumn.Subtract(SparseMatrix, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Subtract(SparseMatrix,CompressedColumn)")]
        public void Static_Subtract_SparseMatrix_CompressedColumn()
        {
            // Arrange
            SparseMatrix crsLeft = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            SparseMatrix ccsLeft = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 1.0, 5.0, 2.0, 6.0, 3.0, 7.0 });
            CompressedColumn right = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 5.0, 3.0, 4.0, 2.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { -3.0, -1.0, 1.0, 0.0, 2.0, 4.0 });

            //Act
            CompressedColumn otherCrsMatrix = CompressedColumn.Subtract(crsLeft, right);
            CompressedColumn otherCcsMatrix = CompressedColumn.Subtract(ccsLeft, right);

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
        /// Tests the static method <see cref="CompressedColumn.Multiply(CompressedColumn, SparseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedColumn,SparseMatrix)")]
        public void Static_Multiply_CompressedColumn_SparseMatrix()
        {
            // Arrange
            CompressedColumn left = new CompressedColumn(4, 2, new int[3] { 0, 4, 8 },
                new List<int> { 0, 1, 2, 3, 0, 1, 2, 3 }, new List<double> { 1.0, 3.0, 6.0, 9.0, 2.0, 5.0, 7.0, 8.0 });
            SparseMatrix crsRight = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });
            SparseMatrix ccsRight = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 5.0, 3.0, 4.0, 2.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(4, 3, new double[] { 14.0, 11.0, 8.0, 37.0, 29.0, 21.0, 59.0, 46.0, 33.0, 76.0, 59.0, 42.0 });

            //Act
            CompressedColumn otherCrsMatrix = CompressedColumn.Multiply(left, crsRight);
            CompressedColumn otherCcsMatrix = CompressedColumn.Multiply(left, ccsRight);

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
        /// Tests the static method <see cref="CompressedColumn.Multiply(SparseMatrix, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Multiply(SparseMatrix,CompressedColumn)")]
        public void Static_Multiply_SparseMatrix_CompressedColumn()
        {
            // Arrange
            SparseMatrix crsLeft = new CompressedRow(4, 2, new int[5] { 0, 2, 4, 6, 8 },
                new List<int> { 0, 1, 0, 1, 0, 1, 0, 1 }, new List<double> { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0, 9.0, 8.0 });
            SparseMatrix ccsLeft = new CompressedColumn(4, 2, new int[3] { 0, 4, 8 },
                new List<int> { 0, 1, 2, 3, 0, 1, 2, 3 }, new List<double> { 1.0, 3.0, 6.0, 9.0, 2.0, 5.0, 7.0, 8.0 });
            CompressedColumn right = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 5.0, 3.0, 4.0, 2.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(4, 3, new double[] { 14.0, 11.0, 8.0, 37.0, 29.0, 21.0, 59.0, 46.0, 33.0, 76.0, 59.0, 42.0 });

            //Act
            CompressedColumn otherCrsMatrix = CompressedColumn.Multiply(crsLeft, right);
            CompressedColumn otherCcsMatrix = CompressedColumn.Multiply(ccsLeft, right);

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
        /// Tests the static method <see cref="CompressedColumn.Add(CompressedColumn, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Add(CompressedColumn,CompressedRow)")]
        public void Static_Add_CompressedColumn_CompressedRow()
        {
            // Arrange
            CompressedColumn left = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 1.0, 5.0, 2.0, 6.0, 3.0, 7.0 });
            CompressedRow crsRight = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { 5.0, 5.0, 5.0, 10.0, 10.0, 10.0 });

            //Act
            CompressedColumn otherCcsMatrix = CompressedColumn.Add(left, crsRight);

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
        /// Tests the static method <see cref="CompressedColumn.Add(CompressedRow, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Add(CompressedRow,CompressedColumn)")]
        public void Static_Add_CompressedRow_CompressedColumn()
        {
            // Arrange
            CompressedRow crsLeft = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            CompressedColumn right = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 5.0, 3.0, 4.0, 2.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { 5.0, 5.0, 5.0, 10.0, 10.0, 10.0 });

            //Act
            CompressedColumn otherCcsMatrix = CompressedColumn.Add(crsLeft, right);

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
        /// Tests the static method <see cref="CompressedColumn.Subtract(CompressedColumn, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Subtract(CompressedColumn,CompressedRow)")]
        public void Static_Subtract_CompressedColumn_CompressedRow()
        {
            // Arrange
            CompressedColumn left = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 1.0, 5.0, 2.0, 6.0, 3.0, 7.0 });
            CompressedRow crsRight = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { -3.0, -1.0, 1.0, 0.0, 2.0, 4.0 });

            //Act
            CompressedColumn otherCcsMatrix = CompressedColumn.Subtract(left, crsRight);

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
        /// Tests the static method <see cref="CompressedColumn.Subtract(CompressedRow, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Subtract(CompressedRow,CompressedColumn)")]
        public void Static_Subtract_CompressedRow_CompressedColumn()
        {
            // Arrange
            CompressedRow crsLeft = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0 });
            CompressedColumn right = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 5.0, 3.0, 4.0, 2.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { -3.0, -1.0, 1.0, 0.0, 2.0, 4.0 });

            //Act
            CompressedColumn otherCcsMatrix = CompressedColumn.Subtract(crsLeft, right);

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
        /// Tests the static method <see cref="CompressedColumn.Multiply(CompressedColumn, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedColumn,CompressedRow)")]
        public void Static_Multiply_CompressedColumn_CompressedRow()
        {
            // Arrange
            CompressedColumn left = new CompressedColumn(4, 2, new int[3] { 0, 4, 8 },
                new List<int> { 0, 1, 2, 3, 0, 1, 2, 3 }, new List<double> { 1.0, 3.0, 6.0, 9.0, 2.0, 5.0, 7.0, 8.0 });
            CompressedRow crsRight = new CompressedRow(2, 3, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 3.0, 2.0, 5.0, 4.0, 3.0 });


            DenseMatrix matrix = new DenseMatrix(4, 3, new double[] { 14.0, 11.0, 8.0, 37.0, 29.0, 21.0, 59.0, 46.0, 33.0, 76.0, 59.0, 42.0 });

            //Act
            CompressedColumn otherCcsMatrix = CompressedColumn.Multiply(left, crsRight);

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
        /// Tests the static method <see cref="CompressedColumn.Multiply(CompressedRow, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedRow,CompressedColumn)")]
        public void Static_Multiply_CompressedRow_CompressedColumn()
        {
            // Arrange
            CompressedRow crsLeft = new CompressedRow(4, 2, new int[5] { 0, 2, 4, 6, 8 },
                new List<int> { 0, 1, 0, 1, 0, 1, 0, 1 }, new List<double> { 1.0, 2.0, 3.0, 5.0, 6.0, 7.0, 9.0, 8.0 });
            CompressedColumn right = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new List<int> { 0, 1, 0, 1, 0, 1 }, new List<double> { 4.0, 5.0, 3.0, 4.0, 2.0, 3.0 });

            DenseMatrix matrix = new DenseMatrix(4, 3, new double[] { 14.0, 11.0, 8.0, 37.0, 29.0, 21.0, 59.0, 46.0, 33.0, 76.0, 59.0, 42.0 });

            //Act
            CompressedColumn otherCcsMatrix = CompressedColumn.Multiply(crsLeft, right);

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
        /// Tests the static method <see cref="CompressedColumn.Multiply(double, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Multiply(double,CompressedColumn)")]
        public void Static_Multiply_Double_CompressedColumn()
        {
            // Arrange
            double factor = -2.5;
            CompressedColumn operand = new CompressedColumn(3, 2, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 2.0, -4.0, 3.0, -5.0, 1.0 });

            CompressedColumn matrix = new CompressedColumn(3, 2, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { -10.0, -5.0, 10.0, -7.5, 12.5, -2.5 });

            //Act
            CompressedColumn otherMatrix = CompressedColumn.Multiply(factor, operand);

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
        /// Tests the static method <see cref="CompressedColumn.Multiply(CompressedColumn, double)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedColumn,double)")]
        public void Static_Multiply_CompressedColumn_Double()
        {
            // Arrange
            CompressedColumn operand = new CompressedColumn(3, 2, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 2.0, -4.0, 3.0, -5.0, 1.0 });
            double factor = -2.5;

            CompressedColumn matrix = new CompressedColumn(3, 2, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { -10.0, -5.0, 10.0, -7.5, 12.5, -2.5 });

            //Act
            CompressedColumn otherMatrix = CompressedColumn.Multiply(operand, factor);

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
        /// Tests the static method <see cref="CompressedColumn.Divide(CompressedColumn, double)"/>.
        /// </summary>
        [TestMethod("Static Divide(CompressedColumn,double)")]
        public void Static_Divide_CompressedColumn_Double()
        {
            // Arrange
            CompressedColumn operand = new CompressedColumn(2, 1, new int[2] { 0, 2 },
                new List<int> { 0, 1 }, new List<double> { 4.0, 3.0 });
            double divisor = -2.0;

            CompressedColumn matrix = new CompressedColumn(2, 1, new int[2] { 0, 2 },
                new List<int> { 0, 1 }, new List<double> { -2.0, -1.5 });

            //Act
            CompressedColumn otherMatrix = CompressedColumn.Divide(operand, divisor);

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
        /// Tests the static method <see cref="CompressedColumn.Multiply(CompressedColumn, Vector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedColumn,Vector)")]
        public void Static_Multiply_CompressedColumn_Vector()
        {
            // Arrange
            CompressedColumn matrix = new CompressedColumn(3, 2, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 2.0, -4.0, 3.0, -5.0, 1.0 });
            Vector denseVector = new DenseVector(new double[2] { -2.0, 6.0 });
            Vector sparseVector = new SparseVector(2, new int[2] { 0, 1 }, new double[2] { -2.0, 6.0 });

            Vector result = new DenseVector(new double[3] { 10.0, -34.0, 14.0 });

            //Act
            Vector otherDenseVector = CompressedColumn.Multiply(matrix, denseVector);
            Vector otherSparseVector = CompressedColumn.Multiply(matrix, sparseVector);

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
        /// Tests the static method <see cref="CompressedColumn.Multiply(CompressedColumn, DenseVector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedColumn,DenseVector)")]
        public void Static_Multiply_CompressedColumn_DenseVector()
        {
            // Arrange
            CompressedColumn matrix = new CompressedColumn(3, 2, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 2.0, -4.0, 3.0, -5.0, 1.0 });
            DenseVector denseVector = new DenseVector(new double[2] { -2.0, 6.0 });

            DenseVector result = new DenseVector(new double[3] { 10.0, -34.0, 14.0 });

            //Act
            DenseVector otherDenseVector = CompressedColumn.Multiply(matrix, denseVector);

            // Assert
            Assert.AreEqual(result.Size, otherDenseVector.Size);

            for (int i_R = 0; i_R < result.Size; i_R++)
            {
                Assert.IsTrue(Math.Abs(result[i_R] - otherDenseVector[i_R]) < Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="CompressedColumn.Multiply(CompressedColumn, SparseVector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedColumn,SparseVector)")]
        public void Static_Multiply_CompressedColumn_SparseVector()
        {
            // Arrange
            CompressedColumn matrix = new CompressedColumn(3, 2, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 2.0, -4.0, 3.0, -5.0, 1.0 });
            SparseVector sparseVector = new SparseVector(2, new int[2] { 0, 1 }, new double[2] { -2.0, 6.0 });

            SparseVector result = new SparseVector(3, new int[3] { 0, 1, 2 }, new double[3] { 10.0, -34.0, 14.0 });

            // Act
            SparseVector otherSparseVector = CompressedColumn.Multiply(matrix, sparseVector);

            // Assert
            Assert.AreEqual(result.Size, otherSparseVector.Size);

            for (int i_R = 0; i_R < result.Size; i_R++)
            {
                Assert.IsTrue(Math.Abs(result[i_R] - otherSparseVector[i_R]) < Settings.AbsolutePrecision);
            }
        }


        /// <summary>
        /// Tests the static method <see cref="CompressedColumn.TransposeMultiply(CompressedColumn, Vector)"/>.
        /// </summary>
        [TestMethod("Static TransposeMultiply(CompressedColumn,Vector)")]
        public void Static_TransposeMultiply_CompressedColumn_Vector()
        {
            // Arrange
            CompressedColumn matrix = new CompressedColumn(3, 2, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 2.0, -4.0, 3.0, -5.0, 1.0 });
            DenseVector denseVector = new DenseVector(new double[3] { -2.0, 0.0, 6.0 });
            SparseVector sparseVector = new SparseVector(3, new int[2] { 0, 2 }, new double[2] { -2.0, 6.0 });

            DenseVector result = new DenseVector(new double[2] { -32.0, 0.0 });

            //Act
            DenseVector otherDenseVector = CompressedColumn.TransposeMultiply(matrix, denseVector);
            SparseVector otherSparseVector = CompressedColumn.TransposeMultiply(matrix, sparseVector);

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
        /// Tests the static method <see cref="CompressedColumn.TransposeMultiply(CompressedColumn, DenseVector)"/>.
        /// </summary>
        [TestMethod("Static TransposeMultiply(CompressedColumn,DenseVector)")]
        public void Static_TransposeMultiply_CompressedColumn_DenseVector()
        {
            // Arrange
            CompressedColumn matrix = new CompressedColumn(3, 2, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 2.0, -4.0, 3.0, -5.0, 1.0 });
            DenseVector denseVector = new DenseVector(new double[3] { -2.0, 0.0, 6.0 });

            DenseVector result = new DenseVector(new double[2] { -32.0, 0.0 });

            //Act
            DenseVector otherDenseVector = CompressedColumn.TransposeMultiply(matrix, denseVector);

            // Assert
            Assert.AreEqual(result.Size, otherDenseVector.Size);

            for (int i_R = 0; i_R < result.Size; i_R++)
            {
                Assert.IsTrue(Math.Abs(result[i_R] - otherDenseVector[i_R]) < Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="CompressedColumn.TransposeMultiply(CompressedColumn, SparseVector)"/>.
        /// </summary>
        [TestMethod("Static TransposeMultiply(CompressedColumn,SparseVector)")]
        public void Static_TransposeMultiply_CompressedColumn_SparseVector()
        {
            // Arrange
            CompressedColumn matrix = new CompressedColumn(3, 2, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, 2.0, -4.0, 3.0, -5.0, 1.0 });
            SparseVector sparseVector = new SparseVector(3, new int[2] { 0, 2 }, new double[2] { -2.0, 6.0 });

            DenseVector result = new DenseVector(new double[2] { -32.0, 0.0 });

            //Act
            SparseVector otherSparseVector = CompressedColumn.TransposeMultiply(matrix, sparseVector);

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
        /// Tests the method <see cref="CompressedColumn.Kernel()"/>.
        /// </summary>
        [TestMethod("Method Transpose() - More Row")]
        public void Transpose_HeightRectangular()
        {
            // Arrange
            CompressedColumn otherMatrix = new CompressedColumn(3, 2, new int[] { 0, 3, 5 },
                new List<int> { 0, 1, 2, 1, 2 }, new List<double> { -3.0, -1.0, 1.0, 2.0, 4.0 });

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { -3.0, -1.0, 1.0, 0.0, 2.0, 4.0 });

            // Act
            otherMatrix.Transpose();

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
        /// Tests the method <see cref="CompressedColumn.Kernel()"/>.
        /// </summary>
        [TestMethod("Method Transpose() - More Column")]
        public void Transpose_LongRectangular()
        {
            // Arrange
            CompressedColumn otherMatrix = new CompressedColumn(2, 3, new int[] { 0, 1, 3, 5 },
                new List<int> { 0, 0, 1, 0, 1 }, new List<double> { -3.0, -1.0, 2.0, 1.0, 4.0 });

            DenseMatrix matrix = new DenseMatrix(3, 2, new double[] { -3.0, 0.0, -1.0, 2.0, 1.0, 4.0 });

            // Act
            otherMatrix.Transpose();

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
        /// Tests the method <see cref="CompressedColumn.Kernel()"/>.
        /// </summary>
        [TestMethod("Method Kernel() - Square - A")]
        public void Kernel_Square_A()
        {
            // Arrange
            CompressedColumn ccs = new CompressedColumn(3, 3, new int[] { 0, 3, 6, 9 },
                new List<int> { 0, 1, 2, 0, 1, 2, 0, 1, 2 }, new List<double> { 1.0, 1.0, 1.0, 2.0, 0.0, 0.0, 3.0, 3.0, 3.0 });

            DenseVector[] result = new DenseVector[1];
            result[0] = new DenseVector(new double[3] { -3.0, 0.0, 1.0 });

            // Act
            DenseVector[] kernel = ccs.Kernel();

            // Assert
            Assert.AreEqual(result.Length, kernel.Length);

            for (int i = 0; i < result.Length; i++)
            {
                Assert.AreEqual(result[i].Size, kernel[i].Size);
            }

            // Iterate on each vector of the kernel.
            for (int i_K = 0; i_K < kernel.Length; i_K++)
            {
                DenseVector kernelVector = new DenseVector(kernel[i_K]);

                // Projects the kernel vector on the space generated by the expected result vectors 
                for (int i_R = 0; i_R < result.Length; i_R++)
                {
                    DenseVector resultVector = new DenseVector(result[i_R]);

                    double numerator = 0.0, denominator = 0.0;
                    for (int i = 0; i < resultVector.Size; i++)
                    {
                        numerator += resultVector[i] * kernelVector[i];
                        denominator += resultVector[i] * resultVector[i];
                    }
                    double ratio = numerator / denominator;

                    resultVector = DenseVector.Multiply(ratio, resultVector);

                    kernelVector = DenseVector.Subtract(kernelVector, resultVector);
                }

                for (int i_C = 0; i_C < kernelVector.Size; i_C++)
                {
                    Assert.IsTrue(Math.Abs(kernelVector[i_C]) < Settings.AbsolutePrecision, $"Expected {0.0}; Actual {kernelVector[i_C]}. ");
                }
            }
        }

        /// <summary>
        /// Tests the method <see cref="CompressedColumn.Kernel()"/>.
        /// </summary>
        [TestMethod("Method Kernel() - Square - B")]
        public void Kernel_Square_B()
        {
            // Arrange
            CompressedColumn ccs = new CompressedColumn(6, 6, new int[7] { 0, 6, 12, 18, 24, 26, 32 },
            new List<int>() { 0, 1, 2, 3, 4, 5, /**/ 0, 1, 2, 3, 4, 5, /**/ 0, 1, 2, 3, 4, 5, /**/ 0, 1, 2, 3, 4, 5, /**/ 0, 2, /**/ 0, 1, 2, 3, 4, 5 },
                new List<double> { 2.0, 1.0, 3.0, 1.0, 1.0, 1.0,/**/ 2.0, 1.0, 3.0, 1.0, 1.0, 1.0,/**/ -4.0, -2.0, -6.0, -2.0, -2.0, -2.0,
                /**/ 3.0, -3.0, -3.0, -3.0, -3.0, -3.0,/**/ -9.0, -6.0, /**/ 1.0, 2.0, 2.0, 2.0, 2.0, 2.0});

            DenseVector[] result = new DenseVector[3];
            result[0] = new DenseVector(new double[6] { -1.0, 1.0, 0.0, 0.0, 0.0, 0.0 });
            result[1] = new DenseVector(new double[6] { 2.0, 0.0, 1.0, 0.0, 0.0, 0.0 });
            result[2] = new DenseVector(new double[6] { 3.0, 0.0, 0.0, 1.0, 1.0, 0.0 });

            result = DenseVector.GramSchmidt(result);

            // Act
            DenseVector[] kernel = ccs.Kernel();

            // Assert
            Assert.AreEqual(result.Length, kernel.Length);

            for (int i = 0; i < result.Length; i++)
            {
                Assert.AreEqual(result[i].Size, kernel[i].Size);
            }

            // Iterate on each vector of the kernel.
            for (int i_K = 0; i_K < kernel.Length; i_K++)
            {
                DenseVector kernelVector = new DenseVector(kernel[i_K]);

                // Projects the kernel vector on the space generated by the expected result vectors 
                for (int i_R = 0; i_R < result.Length; i_R++)
                {
                    DenseVector resultVector = new DenseVector(result[i_R].ToArray());

                    double numerator = 0.0, denominator = 0.0;
                    for (int i = 0; i < resultVector.Size; i++)
                    {
                        numerator += resultVector[i] * kernelVector[i];
                        denominator += resultVector[i] * resultVector[i];
                    }
                    double ratio = numerator / denominator;

                    resultVector = DenseVector.Multiply(ratio, resultVector);

                    kernelVector = DenseVector.Subtract(kernelVector, resultVector);
                }

                for (int i_C = 0; i_C < kernelVector.Size; i_C++)
                {
                    Assert.IsTrue(Math.Abs(kernelVector[i_C]) < Settings.AbsolutePrecision,
                        $"Component {i_C} of vector {i_K} : Expected {0.0}; Actual {kernelVector[i_C]}.");
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="CompressedColumn.Kernel()"/>.
        /// </summary>
        [TestMethod("Method Kernel() - More Row - A")]
        public void Kernel_HeightRectangular_A()
        {
            CompressedColumn ccs = new CompressedColumn(4, 3, new int[] { 0, 2, 4, 8 },
                new List<int> { 0, 2, 1, 3, 0, 1, 2, 3 }, new List<double> { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 });

            DenseVector[] result = new DenseVector[1];
            result[0] = new DenseVector(new double[3] { 1.0, 1.0, -1.0 });

            //Act
            var kernel = ccs.Kernel();

            // Assert
            Assert.AreEqual(result.Length, kernel.Length);

            for (int i = 0; i < result.Length; i++)
            {
                Assert.AreEqual(result[i].Size, kernel[i].Size);
            }

            // Iterate on each vector of the kernel.
            for (int i_K = 0; i_K < kernel.Length; i_K++)
            {
                DenseVector kernelVector = new DenseVector(kernel[i_K]);

                // Projects the kernel vector on the space generated by the expected result vectors 
                for (int i_R = 0; i_R < result.Length; i_R++)
                {
                    DenseVector resultVector = new DenseVector(result[i_R]);

                    double numerator = 0.0, denominator = 0.0;
                    for (int i = 0; i < resultVector.Size; i++)
                    {
                        numerator += resultVector[i] * kernelVector[i];
                        denominator += resultVector[i] * resultVector[i];
                    }
                    double ratio = numerator / denominator;

                    resultVector = DenseVector.Multiply(ratio, resultVector);

                    kernelVector = DenseVector.Subtract(kernelVector, resultVector);
                }

                for (int i_C = 0; i_C < kernelVector.Size; i_C++)
                {
                    Assert.IsTrue(Math.Abs(kernelVector[i_C]) < Settings.AbsolutePrecision,
                        $"Component {i_C} of vector {i_K} : Expected {0.0}; Actual {kernelVector[i_C]}.");
                }
            }
        }

        /// <summary>
        /// Tests the method <see cref="CompressedColumn.Kernel()"/>.
        /// </summary>
        [TestMethod("Method Kernel() - More Row - B")]
        public void Kernel_HeightRectangular_B()
        {
            // Arrange
            CompressedColumn ccs = new CompressedColumn(6, 5, new int[] { 0, 5, 10, 13, 16, 21 },
                new List<int> { 0, 2, 3, 4, 5, /**/ 0, 2, 3, 4, 5, /**/ 1, 3, 5, /**/ 1, 3, 5, /**/ 0, 2, 3, 4, 5 },
                new List<double> { 1.0, 4.0, 1.0, 2.0, 1.0, /**/ 1.0, 2.0, 1.0, 2.0, 1.0, /**/ 1.0, 1.0, 2.0, /**/ -2.0, -2.0, -4.0, /**/ 1.0, 3.0, 1.0, 2.0, 1.0 });

            DenseVector[] result = new DenseVector[2];
            result[0] = new DenseVector(new double[5] { 0.0, 0.0, 2.0, 1.0, 0.0 });
            result[1] = new DenseVector(new double[5] { -0.5, -0.5, 0.0, 0.0, 1.0 });

            result = DenseVector.GramSchmidt(result);

            // Act
            DenseVector[] kernel = ccs.Kernel();

            // Assert
            Assert.AreEqual(result.Length, kernel.Length);

            for (int i = 0; i < result.Length; i++)
            {
                Assert.AreEqual(result[i].Size, kernel[i].Size);
            }

            // Iterate on each vector of the kernel.
            for (int i_K = 0; i_K < kernel.Length; i_K++)
            {
                DenseVector kernelVector = new DenseVector(kernel[i_K]);

                // Projects the kernel vector on the space generated by the expected result vectors 
                for (int i_R = 0; i_R < result.Length; i_R++)
                {
                    DenseVector resultVector = new DenseVector(result[i_R]);

                    double numerator = 0.0, denominator = 0.0;
                    for (int i = 0; i < resultVector.Size; i++)
                    {
                        numerator += resultVector[i] * kernelVector[i];
                        denominator += resultVector[i] * resultVector[i];
                    }
                    double ratio = numerator / denominator;

                    resultVector = DenseVector.Multiply(ratio, resultVector);

                    kernelVector = DenseVector.Subtract(kernelVector, resultVector);
                }

                for (int i_C = 0; i_C < kernelVector.Size; i_C++)
                {
                    Assert.IsTrue(Math.Abs(kernelVector[i_C]) < Settings.AbsolutePrecision,
                        $"Component {i_C} of vector {i_K} : Expected {0.0}; Actual {kernelVector[i_C]}.");
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="CompressedColumn.Kernel()"/>.
        /// </summary>
        [TestMethod("Method Kernel() - More Column - A")]
        public void Kernel_LongRectangular_A()
        {
            // Arrange
            CompressedColumn ccs = new CompressedColumn(2, 4, new int[] { 0, 2, 3, 5, 5 },
                new List<int> { 0, 1, 0, 0, 1 }, new List<double> { 1.0, 2.0, 3.0, 1.0, 2.0 });

            DenseVector[] result = new DenseVector[2];
            result[0] = new DenseVector(new double[4] { -1.0, 0.0, 1.0, 0.0 });
            result[1] = new DenseVector(new double[4] { 0.0, 0.0, 0.0, 1.0 });

            result = DenseVector.GramSchmidt(result);

            // Act
            DenseVector[] kernel = ccs.Kernel();

            // Assert
            Assert.AreEqual(result.Length, kernel.Length);

            for (int i = 0; i < result.Length; i++)
            {
                Assert.AreEqual(result[i].Size, kernel[i].Size);
            }

            // Iterate on each vector of the kernel.
            for (int i_K = 0; i_K < kernel.Length; i_K++)
            {
                DenseVector kernelVector = new DenseVector(kernel[i_K]);

                // Projects the kernel vector on the space generated by the expected result vectors 
                for (int i_R = 0; i_R < result.Length; i_R++)
                {
                    DenseVector resultVector = new DenseVector(result[i_R]);

                    double numerator = 0.0, denominator = 0.0;
                    for (int i = 0; i < resultVector.Size; i++)
                    {
                        numerator += resultVector[i] * kernelVector[i];
                        denominator += resultVector[i] * resultVector[i];
                    }
                    double ratio = numerator / denominator;

                    resultVector = DenseVector.Multiply(ratio, resultVector);

                    kernelVector = DenseVector.Subtract(kernelVector, resultVector);
                }

                for (int i_C = 0; i_C < kernelVector.Size; i_C++)
                {
                    Assert.IsTrue(Math.Abs(kernelVector[i_C]) < Settings.AbsolutePrecision, 
                        $"Component {i_C} of vector {i_K} : Expected {0.0}; Actual {kernelVector[i_C]}.");
                }
            }
        }

        /// <summary>
        /// Tests the method <see cref="CompressedColumn.Kernel()"/>.
        /// </summary>
        [TestMethod("Method Kernel() - More Column - B")]
        public void Kernel_LongRectangular_B()
        {
            // Arrange
            CompressedColumn ccs = new CompressedColumn(3, 6, new int[] { 0, 3, 6, 9, 12, 14, 17 },
                new List<int> { 0, 1, 2, /**/ 0, 1, 2, /**/ 0, 1, 2, /**/ 0, 1, 2, /**/ 0, 2, /**/ 0, 1, 2 },
                new List<double> { 2.0, 1.0, 3.0, /**/ 2.0, 1.0, 3.0, /**/ -4.0, -2.0, -6.0, /**/ 3.0, -3.0, -3.0, /**/ -9.0, -6.0, /**/ 1.0, 2.0, 2.0 });

            DenseVector[] result = new DenseVector[3];
            result[0] = new DenseVector(new double[6] { -1.0, 1.0, 0.0, 0.0, 0.0, 0.0 });
            result[1] = new DenseVector(new double[6] { 2.0, 0.0, 1.0, 0.0, 0.0, 0.0 });
            result[2] = new DenseVector(new double[6] { 3.0, 0.0, 0.0, 1.0, 1.0, 0.0 });

            result = DenseVector.GramSchmidt(result);

            // Act
            DenseVector[] kernel = ccs.Kernel();

            // Assert
            Assert.AreEqual(result.Length, kernel.Length);

            for (int i = 0; i < result.Length; i++)
            {
                Assert.AreEqual(result[i].Size, kernel[i].Size);
            }

            // Iterate on each vector of the kernel.
            for (int i_K = 0; i_K < kernel.Length; i_K++)
            {
                DenseVector kernelVector = new DenseVector(kernel[i_K]);

                // Projects the kernel vector on the space generated by the expected result vectors 
                for (int i_R = 0; i_R < result.Length; i_R++)
                {
                    DenseVector resultVector = new DenseVector(result[i_R]);

                    double numerator = 0.0, denominator = 0.0;
                    for (int i = 0; i < resultVector.Size; i++)
                    {
                        numerator += resultVector[i] * kernelVector[i];
                        denominator += resultVector[i] * resultVector[i];
                    }
                    double ratio = numerator / denominator;

                    resultVector = DenseVector.Multiply(ratio, resultVector);

                    kernelVector = DenseVector.Subtract(kernelVector, resultVector);
                }

                for (int i_C = 0; i_C < kernelVector.Size; i_C++)
                {
                    Assert.IsTrue(Math.Abs(kernelVector[i_C]) < Settings.AbsolutePrecision,
                        $"Component {i_C} of vector {i_K} : Expected {0.0}; Actual {kernelVector[i_C]}.");
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="CompressedColumn.ToCompressedRow()"/>.
        /// </summary>
        [TestMethod("Method ToCompressedRow()")]
        public void ToCompressedRow()
        {
            // Arrange
            CompressedColumn ccs = new CompressedColumn(6, 5, new int[6] { 0, 0, 2, 4, 8, 9 },
                new List<int> { 0, 4, 2, 4, 0, 1, 3, 4, 5 }, new List<double> { 1.5, 4.0, 2.0, 3.5, 1.25, 6.75, 5.5, 2.25, 7.25 });

            CompressedRow result = new CompressedRow(6, 5, new int[7] { 0, 2, 3, 4, 5, 8, 9 },
                new List<int> { 1, 3, 3, 2, 3, 1, 2, 3, 4 }, new List<double> { 1.5, 1.25, 6.75, 2.0, 5.5, 4.0, 3.5, 2.25, 7.25 });

            // Act
            CompressedRow crs = ccs.ToCompressedRow();

            // Assert
            Assert.AreEqual(result.RowCount, crs.RowCount);
            Assert.AreEqual(result.ColumnCount, crs.ColumnCount);

            for (int i_R = 0; i_R < result.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < result.ColumnCount; i_C++)
                {
                    Assert.IsTrue(Math.Abs(result[i_R, i_C] - crs[i_R, i_C]) < Settings.AbsolutePrecision);
                }
            }
        }

        #endregion
    }
}
