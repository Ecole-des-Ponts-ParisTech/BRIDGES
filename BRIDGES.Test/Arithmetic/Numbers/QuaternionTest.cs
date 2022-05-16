using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.Arithmetic.Numbers;
using BRIDGES.Algebra.Fundamentals;


namespace BRIDGES.Test.Arithmetic.Numbers
{
    /// <summary>
    /// Class testing the members of the <see cref="Quaternion"/> class.
    /// </summary>
    [TestClass]
    public class QuaternionTest
    {
        #region Behavior

        /// <summary>
        /// Tests that <see cref="Quaternion"/> are not reference type.
        /// </summary>
        [TestMethod("Behavior IsNotReference")]
        public void IsNotReference()
        {
            // Arrange
            Quaternion quaternionA = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion quaternionB = new Quaternion(6.0, 7.0, 8.0, 9.0);
            //Act
            quaternionA = quaternionB;
            // Assert
            Assert.IsTrue(quaternionA.Equals(quaternionB));
            Assert.AreNotSame(quaternionA, quaternionB);
        }

        #endregion


        #region Properties

        /// <summary>
        /// Tests the initialisation of the <see cref="Quaternion.ScalarPart"/>, <see cref="Quaternion.I"/>, <see cref="Quaternion.J"/> and <see cref="Quaternion.K"/> properties.
        /// </summary>
        [TestMethod("Property ScalarPart, I, J & K")]
        public void RealPartAndImaginaryPart()
        {
            // Assign
            Quaternion quaternion = new Quaternion(2.0, 4.0, 6.0, 8.0);
            // Act

            // Assert
            Assert.AreEqual(2.0, quaternion.ScalarPart, Settings.AbsolutePrecision);
            Assert.AreEqual(4.0, quaternion.I, Settings.AbsolutePrecision);
            Assert.AreEqual(6.0, quaternion.J, Settings.AbsolutePrecision);
            Assert.AreEqual(8.0, quaternion.K, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the computation of the <see cref="Quaternion.VectorPart"/> property.
        /// </summary>
        [TestMethod("Property VectorPart")]
        public void VectorPart()
        {
            // Assign
            Quaternion quaternion = new Quaternion(2.0, 4.0, 6.0, 8.0);
            // Act
            double[] vectorPart = quaternion.VectorPart;
            // Assert
            Assert.IsTrue(vectorPart.Length == 3);
            Assert.AreEqual(4.0, vectorPart[0], Settings.AbsolutePrecision);
            Assert.AreEqual(6.0, vectorPart[1], Settings.AbsolutePrecision);
            Assert.AreEqual(8.0, vectorPart[2], Settings.AbsolutePrecision);
        }

        #endregion

        #region Static Members

        /// <summary>
        /// Tests the initialisation of the <see cref="Quaternion"/> corresponding to the additive neutral element.
        /// </summary>
        [TestMethod("Static Zero()")]
        public void Static_Zero()
        {
            // Arrange
            Quaternion result = new Quaternion(0.0, 0.0, 0.0, 0.0);
            // Act
            Quaternion quaternion = Quaternion.Zero();
            // Assert
            Assert.IsTrue(quaternion.Equals(result));
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="Quaternion"/> corresponding to the multiplicative neutral element.
        /// </summary>
        [TestMethod("Static One()")]
        public void Static_One()
        {
            // Arrange
            Quaternion result = new Quaternion(1.0, 0.0, 0.0, 0.0);
            // Act
            Quaternion quaternion = Quaternion.One();
            // Assert
            Assert.IsTrue(quaternion.Equals(result));
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Tests the computation of the conjugate of a given <see cref="Quaternion"/>.
        /// </summary>
        [TestMethod("Static Conjugate(Quaternion)")]
        public void Static_Conjugate_Quaternion()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion result = new Quaternion(1.0, -2.0, -3.0, -4.0);
            // Act
            Quaternion conjugate = Quaternion.Conjugate(quaternion);
            // Assert
            Assert.IsTrue(conjugate.Equals(result));
        }


        /******************** Algebraic Field ********************/

        /// <summary>
        /// Tests the static addition of two <see cref="Quaternion"/>.
        /// </summary>
        [TestMethod("Static Add(Quaternion,Quaternion)")]
        public void Static_Add_Quaternion_Quaternion()
        {
            // Arrange
            Quaternion quaternionA = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion quaternionB = new Quaternion(6.0, 7.0, 8.0, 9.0);
            Quaternion result = new Quaternion(7.0, 9.0, 11.0, 13.0);
            //Act
            Quaternion otherQuaternion = Quaternion.Add(quaternionA, quaternionB);
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the static subtraction of two <see cref="Quaternion"/>.
        /// </summary>
        [TestMethod("Static Subtract(Quaternion,Quaternion)")]
        public void Static_Substract_Quaternion_Quaternion()
        {
            // Arrange
            Quaternion quaternionA = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion quaternionB = new Quaternion(9.0, -8.0, 7.0, 6.0);
            Quaternion result = new Quaternion(-8.0, 10.0, -4.0, -2.0);
            //Act
            Quaternion otherQuaternion = Quaternion.Subtract(quaternionA, quaternionB);
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the computation of the opposite of a given <see cref="Quaternion"/>.
        /// </summary>
        [TestMethod("Static Opposite(Quaternion)")]
        public void Static_Opposite_Quaternion()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion result = new Quaternion(-1.0, -2.0, -3.0, -4.0);
            // Act
            Quaternion otherQuaternion = Quaternion.Opposite(quaternion);
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }


        /// <summary>
        /// Tests the static multiplication of two <see cref="Quaternion"/>.
        /// </summary>
        [TestMethod("Static Multiply(Quaternion,Quaternion)")]
        public void Static_Multiply_Quaternion_Quaternion()
        {
            // Arrange
            Quaternion quaternionA = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion quaternionB = new Quaternion(6.0, 7.0, 8.0, 9.0);
            Quaternion result = new Quaternion(
                1.0 * 6.0 - 2.0 * 7.0 - 3.0 * 8.0 - 4.0 * 9.0, 
                1.0 * 7.0 + 2.0 * 6.0 + 3.0 * 9.0 - 4.0 * 8.0, 
                1.0 * 8.0 - 2.0 * 9.0 + 3.0 * 6.0 + 4.0 * 7.0, 
                1.0 * 9.0 + 2.0 * 8.0 - 3.0 * 7.0 + 4.0 * 6.0);
            //Act
            Quaternion otherQuaternion = Quaternion.Multiply(quaternionA, quaternionB);
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the static division of two <see cref="Quaternion"/>.
        /// </summary>
        [TestMethod("Static Divide(Quaternion,Quaternion)")]
        public void Static_Divide_Quaternion_Quaternion()
        {
            // Arrange
            Quaternion quaternionA = new Quaternion(6.0, 7.0, 8.0, 9.0);
            Quaternion quaternionB = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion result = new Quaternion(
                 (6.0 * 1.0 + 7.0 * 2.0 + 8.0 * 3.0 + 9.0 * 4.0) / 30.0,
                (-6.0 * 2.0 + 7.0 * 1.0 - 8.0 * 4.0 + 9.0 * 3.0) / 30.0,
                (-6.0 * 3.0 + 7.0 * 4.0 + 8.0 * 1.0 - 9.0 * 2.0) / 30.0,
                (-6.0 * 4.0 - 7.0 * 3.0 + 8.0 * 2.0 + 9.0 * 1.0) / 30.0);
            // Act
            Quaternion otherQuaternion = Quaternion.Divide(quaternionA, quaternionB);
            // Assert 
            Assert.IsTrue(otherQuaternion.Equals(result));
        }
        
        /// <summary>
        /// Tests the computation of the inverse of a given <see cref="Quaternion"/>.
        /// </summary>
        [TestMethod("Static Inverse(Quaternion)")]
        public void Static_Inverse_Quaternion()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion result = new Quaternion(1.0 / 30.0, -2.0 / 30.0, -3.0 / 30.0, -4.0 / 30.0);
            // Act
            Quaternion otherQuaternion = Quaternion.Inverse(quaternion);
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }


        /******************** Complex Embedding ********************/

        /// <summary>
        /// Tests the static addition of a <see cref="Quaternion"/> with a <see cref="Complex"/>.
        /// </summary>
        [TestMethod("Static Add(Quaternion,Complex)")]
        public void Static_Add_Quaternion_Complex()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Complex complex = new Complex(8.0, 9.0);
            Quaternion result = new Quaternion(9.0, 11.0, 3.0, 4.0);
            //Act
            Quaternion otherQuaternion = Quaternion.Add(quaternion, complex);
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the static addition of a <see cref="Complex"/> with a <see cref="Quaternion"/>.
        /// </summary>
        [TestMethod("Static Add(Complex,Quaternion)")]
        public void Static_Add_Complex_Quaternion()
        {
            // Arrange
            Complex complex = new Complex(8.0, 9.0);
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion result = new Quaternion(9.0, 11.0, 3.0, 4.0);
            //Act
            Quaternion otherQuaternion = Quaternion.Add(complex, quaternion);
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }


        /// <summary>
        /// Tests the static subtraction of a <see cref="Quaternion"/> with a <see cref="Complex"/>.
        /// </summary>
        [TestMethod("Static Subtract(Quaternion,Complex)")]
        public void Static_Subtract_Quaternion_Complex()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Complex complex = new Complex(9.0, 8.0);
            Quaternion result = new Quaternion(-8.0, -6.0, 3.0, 4.0);
            //Act
            Quaternion otherQuaternion = Quaternion.Subtract(quaternion, complex);
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the static subtraction of a <see cref="Complex"/> with a <see cref="Quaternion"/>.
        /// </summary>
        [TestMethod("Static Subtract(Complex,Quaternion)")]
        public void Static_Subtract_Complex_Quaternion()
        {
            // Arrange
            Complex complex = new Complex(9.0, 8.0);
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion result = new Quaternion(8.0, 6.0, -3.0, -4.0);
            //Act
            Quaternion otherQuaternion = Quaternion.Subtract(complex, quaternion);
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }


        /// <summary>
        /// Tests the static multiplication of a <see cref="Quaternion"/> with a <see cref="Complex"/>.
        /// </summary>
        [TestMethod("Static Multiply(Quaternion,Complex)")]
        public void Static_Multiply_Quaternion_Complex()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Complex complex = new Complex(9.0, 8.0);
            Quaternion result = new Quaternion( 1.0 * 9.0 - 2.0 * 8.0, 1.0 * 8.0 + 2.0 * 9.0,
                  3.0 * 9.0 + 4.0 * 8.0, - 3.0 * 8.0 + 4.0 * 9.0);
            //Act
            Quaternion otherQuaternion = Quaternion.Multiply(quaternion, complex);
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the static multiplication of a <see cref="Complex"/> with a <see cref="Quaternion"/>.
        /// </summary>
        [TestMethod("Static Multiply(Complex,Quaternion)")]
        public void Static_Multiply_Complex_Quaternion()
        {
            // Arrange
            Complex complex = new Complex(9.0, 8.0);
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion result = new Quaternion(1.0 * 9.0 - 2.0 * 8.0, 1.0 * 8.0 + 2.0 * 9.0,
                  3.0 * 9.0 + 4.0 * 8.0, -3.0 * 8.0 + 4.0 * 9.0);
            //Act
            Quaternion otherQuaternion = Quaternion.Multiply(complex, quaternion);
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }


        /// <summary>
        /// Tests the static division of a <see cref="Quaternion"/> by a <see cref="Complex"/>.
        /// </summary>
        [TestMethod("Static Divide(Quaternion,Complex)")]
        public void Static_Divide_Quaternion_Complex()
        {
            // Arrange
            Quaternion quaternion= new Quaternion(6.0, 7.0, 8.0, 9.0);
            Complex complex = new Complex(1.0, 2.0);
            Quaternion result = new Quaternion(
                (6.0 * 1.0 + 7.0 * 2.0) / 5.0,
                (-6.0 * 2.0 + 7.0 * 1.0) / 5.0,
                (8.0 * 1.0 - 9.0 * 2.0) / 5.0,
                (8.0 * 2.0 + 9.0 * 1.0) / 5.0);
            // Act
            Quaternion otherQuaternion = Quaternion.Divide(quaternion, complex);
            // Assert 
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the static division of a <see cref="Complex"/> by a <see cref="Quaternion"/>.
        /// </summary>
        [TestMethod("Static Divide(Complex,Quaternion)")]
        public void Static_Divide_Complex_Quaternion()
        {
            // Arrange
            Complex complex = new Complex(9.0, 8.0);
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion result = new Quaternion(
                 (9.0 * 1.0 + 8.0 * 2.0) / 30.0,
                (-9.0 * 2.0 + 8.0 * 1.0) / 30.0,
                (-9.0 * 3.0 + 8.0 * 4.0) / 30.0,
                (-9.0 * 4.0 - 8.0 * 3.0) / 30.0);
            // Act
            Quaternion otherQuaternion = Quaternion.Divide(complex, quaternion);
            // Assert 
            Assert.IsTrue(otherQuaternion.Equals(result));
        }


        /******************** Real Embedding ********************/

        /// <summary>
        /// Tests the static addition of a <see cref="Quaternion"/> with a <see cref="Real"/>.
        /// </summary>
        [TestMethod("Static Add(Quaternion,Real)")]
        public void Static_Add_Quaternion_Real()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0 ,4.0);
            Real real = new Real(10.0);
            Quaternion result = new Quaternion(11.0, 2.0, 3.0, 4.0);
            //Act
            Quaternion otherQuaternion = Quaternion.Add(quaternion, real);
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the static addition of a <see cref="Real"/> with a <see cref="Quaternion"/>.
        /// </summary>
        [TestMethod("Static Add(Real,Quaternion)")]
        public void Static_Add_Real_Quaternion()
        {
            // Arrange
            Real real = new Real(10.0);
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion result = new Quaternion(11.0, 2.0, 3.0, 4.0);
            //Act
            Quaternion otherQuaternion = Quaternion.Add(real, quaternion);
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }


        /// <summary>
        /// Tests the static subtraction of a <see cref="Quaternion"/> with a <see cref="Real"/>.
        /// </summary>
        [TestMethod("Static Subtract(Quaternion,Real)")]
        public void Static_Subtract_Quaternion_Real()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Real real = new Real(10.0);
            Quaternion result = new Quaternion(-9.0, 2.0, 3.0, 4.0);
            //Act
            Quaternion otherQuaternion = Quaternion.Subtract(quaternion, real);
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the static subtraction of a <see cref="Real"/> with a <see cref="Quaternion"/>.
        /// </summary>
        [TestMethod("Static Subtract(Real,Quaternion)")]
        public void Static_Subtract_Real_Quaternion()
        {
            // Arrange
            Real real = new Real(10.0);
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion result = new Quaternion(9.0, -2.0, -3.0, -4.0);
            //Act
            Quaternion otherQuaternion = Quaternion.Subtract(real, quaternion);
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }


        /// <summary>
        /// Tests the static multiplication of a <see cref="Quaternion"/> with a <see cref="Real"/>.
        /// </summary>
        [TestMethod("Static Multiply(Quaternion,Real)")]
        public void Static_Multiply_Quaternion_Real()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Real real = new Real(4.0);
            Quaternion result = new Quaternion(4.0, 8.0, 12.0, 16.0);
            //Act
            Quaternion otherQuaternion = Quaternion.Multiply(quaternion, real);
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the static multiplication of a <see cref="Real"/> with a <see cref="Quaternion"/>.
        /// </summary>
        [TestMethod("Static Multiply(Real,Quaternion)")]
        public void Static_Multiply_Real_Quaternion()
        {
            // Arrange
            Real real = new Real(5.0);
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion result = new Quaternion(5.0, 10.0, 15.0, 20.0);
            //Act
            Quaternion otherQuaternion = Quaternion.Multiply(real, quaternion);
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }


        /// <summary>
        /// Tests the static division of a <see cref="Quaternion"/> by a <see cref="Real"/>.
        /// </summary>
        [TestMethod("Static Divide(Quaternion,Real)")]
        public void Static_Divide_Quaternion_Real()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Real real = new Real(4.0);
            Quaternion result = new Quaternion(0.25, 0.5, 0.75, 1.0);
            //Act
            Quaternion otherQuaternion = Quaternion.Divide(quaternion, real);
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the static division of a <see cref="Real"/> by a <see cref="Quaternion"/>.
        /// </summary>
        [TestMethod("Static Divide(Real,Quaternion)")]
        public void Static_Divide_Real_Quaternion()
        {
            // Arrange
            Real real = new Real(45.0);
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion result = new Quaternion(1.5, -3.0, -4.5, -6.0);
            // Act
            Quaternion otherQuaternion = Quaternion.Divide(real, quaternion);
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Tests the static multiplication of a <see cref="double"/>-precision real number with a <see cref="Quaternion"/>.
        /// </summary>
        [TestMethod("Static Multiply(Double,CoQuaternionmplex)")]
        public void Static_Multiply_Double_Quaternion()
        {
            // Arrange
            double number = 4.0;
            Quaternion quaternion = new Quaternion(1.25, 2.5, 3.75, -5.0);
            Quaternion result = new Quaternion(5.0, 10.0, 15.0, -20.0);
            //Act
            Quaternion otherQuaternion = Quaternion.Multiply(number, quaternion);
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the static division of a <see cref="Quaternion"/> by a <see cref="double"/>-precision real number.
        /// </summary>
        [TestMethod("Static Divide(Quaternion,Double)")]
        public void Static_Divide_Quaternion_Double()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 4.0, 16.0, 64.0);
            double number = 4.0;
            Quaternion result = new Quaternion(0.25, 1.0, 4.0, 16.0);
            //Act
            Quaternion otherQuaternion = Quaternion.Divide(quaternion, number);
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        #endregion

        #region Operators

        /******************** Algebraic Field ********************/

        /// <summary>
        /// Tests the static addition operator of two <see cref="Quaternion"/>.
        /// </summary>
        [TestMethod("Operator Add(Quaternion,Quaternion)")]
        public void Operator_Add_Quaternion_Quaternion()
        {
            // Arrange
            Quaternion quaternionA = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion quaternionB = new Quaternion(6.0, 7.0, 8.0, 9.0);
            Quaternion result = new Quaternion(7.0, 9.0, 11.0, 13.0);
            //Act
            Quaternion otherQuaternion = Quaternion.Add(quaternionA, quaternionB);
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the static Subtraction operator of two <see cref="Quaternion"/>.
        /// </summary>
        [TestMethod("Operator Subtract(Quaternion,Quaternion)")]
        public void Operator_Substract_Quaternion_Quaternion()
        {
            // Arrange
            Quaternion quaternionA = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion quaternionB = new Quaternion(9.0, -8.0, 7.0, 6.0);
            Quaternion result = new Quaternion(-8.0, 10.0, -4.0, -2.0);
            //Act
            Quaternion otherQuaternion = Quaternion.Subtract(quaternionA, quaternionB);
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the static multiplication operator of two <see cref="Quaternion"/>.
        /// </summary>
        [TestMethod("Operator Multiply(Quaternion,Quaternion)")]
        public void Operator_Multiply_Quaternion_Quaternion()
        {
            // Arrange
            Quaternion quaternionA = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion quaternionB = new Quaternion(6.0, 7.0, 8.0, 9.0);
            Quaternion result = new Quaternion(
                1.0 * 6.0 - 2.0 * 7.0 - 3.0 * 8.0 - 4.0 * 9.0,
                1.0 * 7.0 + 2.0 * 6.0 + 3.0 * 9.0 - 4.0 * 8.0,
                1.0 * 8.0 - 2.0 * 9.0 + 3.0 * 6.0 + 4.0 * 7.0,
                1.0 * 9.0 + 2.0 * 8.0 - 3.0 * 7.0 + 4.0 * 6.0);
            //Act
            Quaternion otherQuaternion = Quaternion.Multiply(quaternionA, quaternionB);
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the static division of operator two <see cref="Quaternion"/>.
        /// </summary>
        [TestMethod("Operator Divide(Quaternion,Quaternion)")]
        public void Operator_Divide_Quaternion_Quaternion()
        {
            // Arrange
            Quaternion quaternionA = new Quaternion(6.0, 7.0, 8.0, 9.0);
            Quaternion quaternionB = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion result = new Quaternion(
                 (6.0 * 1.0 + 7.0 * 2.0 + 8.0 * 3.0 + 9.0 * 4.0) / 30.0,
                (-6.0 * 2.0 + 7.0 * 1.0 - 8.0 * 4.0 + 9.0 * 3.0) / 30.0,
                (-6.0 * 3.0 + 7.0 * 4.0 + 8.0 * 1.0 - 9.0 * 2.0) / 30.0,
                (-6.0 * 4.0 - 7.0 * 3.0 + 8.0 * 2.0 + 9.0 * 1.0) / 30.0);
            // Act
            Quaternion otherQuaternion = Quaternion.Divide(quaternionA, quaternionB);
            // Assert 
            Assert.IsTrue(otherQuaternion.Equals(result));
        }


        /******************** Complex Embedding ********************/


        /******************** Real Embedding ********************/


        /******************** double Embedding ********************/

        #endregion

        #region Methods

        #endregion
    }
}
