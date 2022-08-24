using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.LinearAlgebra.Matrices.Sparse;
using Stor = BRIDGES.LinearAlgebra.Matrices.Storage;


namespace BRIDGES.Test.LinearAlgebra.Matrices
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
            CompressedColumn matrix = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6}, 
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

        /******************** Group Action ********************/

        /// <summary>
        /// Tests the static method <see cref="CompressedColumn.Multiply(double, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Add(double,CompressedColumn)")]
        public void Static_Multiply_Double_CompressedColumn()
        {
            // Arrange
            double factor = -2.5;
            CompressedColumn operand = new CompressedColumn(3, 2, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, -5.0, 3.0, -4.0, 2.0, 1.0 });

            CompressedColumn matrix = new CompressedColumn(3, 2, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { -10.0, 12.5, -7.5, 10.0, -5.0, -2.5 });

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
        [TestMethod("Static Add(CompressedColumn,double)")]
        public void Static_Multiply_CompressedColumn_Double()
        {
            // Arrange
            CompressedColumn operand = new CompressedColumn(3, 2, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { 4.0, -5.0, 3.0, -4.0, 2.0, 1.0 });
            double factor = -2.5;

            CompressedColumn matrix = new CompressedColumn(3, 2, new int[3] { 0, 3, 6 },
                new List<int> { 0, 1, 2, 0, 1, 2 }, new List<double> { -10.0, 12.5, -7.5, 10.0, -5.0, -2.5 });

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

        #endregion
    }
}
