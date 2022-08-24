using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.LinearAlgebra.Matrices;


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


        /******************** Group Action ********************/

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Multiply(double, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Add(double,DenseMatrix)")]
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
        [TestMethod("Static Add(DenseMatrix,double)")]
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
