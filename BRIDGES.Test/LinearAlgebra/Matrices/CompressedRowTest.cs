using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.LinearAlgebra.Matrices.Sparse;
using Stor = BRIDGES.LinearAlgebra.Matrices.Storage;


namespace BRIDGES.Test.LinearAlgebra.Matrices
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

        /******************** Group Action ********************/

        /// <summary>
        /// Tests the static method <see cref="CompressedRow.Multiply(double, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Add(double,CompressedRow)")]
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
        [TestMethod("Static Add(CompressedRow,double)")]
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

        #endregion
    }
}
