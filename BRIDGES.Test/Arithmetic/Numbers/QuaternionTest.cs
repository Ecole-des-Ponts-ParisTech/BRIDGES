using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.Arithmetic.Numbers;
using BRIDGES.Algebra.Fundamentals;
using BRIDGES.Algebra.Structures;


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

        #region Static Properties

        /// <summary>
        /// Tests the initialisation of the <see cref="Quaternion"/> number corresponding to the additive neutral element.
        /// </summary>
        [TestMethod("Static Zero()")]
        public void Static_Zero()
        {
            // Arrange
            Quaternion result = new Quaternion(0.0, 0.0, 0.0, 0.0);
            // Act
            Quaternion quaternion = Quaternion.Zero;
            // Assert
            Assert.IsTrue(quaternion.Equals(result));
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="Quaternion"/> number corresponding to the multiplicative neutral element.
        /// </summary>
        [TestMethod("Static One()")]
        public void Static_One()
        {
            // Arrange
            Quaternion result = new Quaternion(1.0, 0.0, 0.0, 0.0);
            // Act
            Quaternion quaternion = Quaternion.One;
            // Assert
            Assert.IsTrue(quaternion.Equals(result));
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Tests the computation of the conjugate of a given <see cref="Quaternion"/> number.
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
        /// Tests the static addition of two <see cref="Quaternion"/> numbers.
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
        /// Tests the static subtraction of two <see cref="Quaternion"/> numbers.
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
        /// Tests the computation of the opposite of a given <see cref="Quaternion"/> number.
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
        /// Tests the static multiplication of two <see cref="Quaternion"/> numbers.
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
        /// Tests the static division of two <see cref="Quaternion"/> numbers.
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
        /// Tests the computation of the inverse of a given <see cref="Quaternion"/> number.
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
        /// Tests the static addition of a <see cref="Quaternion"/> number with a <see cref="Complex"/> number.
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
        /// Tests the static addition of a <see cref="Complex"/> number with a <see cref="Quaternion"/> number.
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
        /// Tests the static subtraction of a <see cref="Quaternion"/> number with a <see cref="Complex"/> number.
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
        /// Tests the static subtraction of a <see cref="Complex"/> number with a <see cref="Quaternion"/> number.
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
        /// Tests the static multiplication of a <see cref="Quaternion"/> number with a <see cref="Complex"/> number.
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
        /// Tests the static multiplication of a <see cref="Complex"/> number with a <see cref="Quaternion"/> number.
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
        /// Tests the static division of a <see cref="Quaternion"/> number by a <see cref="Complex"/> number.
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
        /// Tests the static division of a <see cref="Complex"/> number by a <see cref="Quaternion"/> number.
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
        /// Tests the static addition of a <see cref="Quaternion"/> number with a <see cref="Real"/> number.
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
        /// Tests the static addition of a <see cref="Real"/> number with a <see cref="Quaternion"/> number.
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
        /// Tests the static subtraction of a <see cref="Quaternion"/> number with a <see cref="Real"/> number.
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
        /// Tests the static subtraction of a <see cref="Real"/> number with a <see cref="Quaternion"/> number.
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
        /// Tests the static multiplication of a <see cref="Quaternion"/> number with a <see cref="Real"/> number.
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
        /// Tests the static multiplication of a <see cref="Real"/> number with a <see cref="Quaternion"/> number.
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
        /// Tests the static division of a <see cref="Quaternion"/> number by a <see cref="Real"/> number.
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
        /// Tests the static division of a <see cref="Real"/> number by a <see cref="Quaternion"/> number.
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
        /// Tests the static multiplication of a <see cref="double"/>-precision real number with a <see cref="Quaternion"/> number.
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
        /// Tests the static division of a <see cref="Quaternion"/> number by a <see cref="double"/>-precision real number.
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
        /// Tests the static addition operator of two <see cref="Quaternion"/> numbers.
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
        /// Tests the static Subtraction operator of two <see cref="Quaternion"/> numbers.
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
        /// Tests the static opposite operator of a <see cref="Quaternion"/> number.
        /// </summary>
        [TestMethod("Operator Subtract(Quaternion)")]
        public void Operator_Substract_Quaternion()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(5.0, -15.0, -7.5, 2.5);
            Quaternion result = new Quaternion(-5.0, 15.0, 7.5, -2.5);
            // Act
            Quaternion otherQuaternion = -quaternion;
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the static multiplication operator of two <see cref="Quaternion"/> numbers.
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
        /// Tests the static division of operator two <see cref="Quaternion"/> numbers.
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

        /// <summary>
        /// Tests the static addition operator of a <see cref="Quaternion"/> number with a <see cref="Complex"/> number.
        /// </summary>
        [TestMethod("Operator Add(Quaternion,Complex)")]
        public void Operator_Add_Quaternion_Complex()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Complex complex = new Complex(8.0, 9.0);
            Quaternion result = new Quaternion(9.0, 11.0, 3.0, 4.0);
            //Act
            Quaternion otherQuaternion = quaternion + complex;
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the static addition operator of a <see cref="Complex"/> number with a <see cref="Quaternion"/> number.
        /// </summary>
        [TestMethod("Operator Add(Complex,Quaternion)")]
        public void Operator_Add_Complex_Quaternion()
        {
            // Arrange
            Complex complex = new Complex(8.0, 9.0);
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion result = new Quaternion(9.0, 11.0, 3.0, 4.0);
            //Act
            Quaternion otherQuaternion = complex + quaternion;
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }


        /// <summary>
        /// Tests the static subtraction operator of a <see cref="Quaternion"/> number with a <see cref="Complex"/> number.
        /// </summary>
        [TestMethod("Operator Subtract(Quaternion,Complex)")]
        public void Operator_Subtract_Quaternion_Complex()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Complex complex = new Complex(9.0, 8.0);
            Quaternion result = new Quaternion(-8.0, -6.0, 3.0, 4.0);
            //Act
            Quaternion otherQuaternion = quaternion - complex;
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the static subtraction operator of a <see cref="Complex"/> number with a <see cref="Quaternion"/> number.
        /// </summary>
        [TestMethod("Operator Subtract(Complex,Quaternion)")]
        public void Operator_Subtract_Complex_Quaternion()
        {
            // Arrange
            Complex complex = new Complex(9.0, 8.0);
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion result = new Quaternion(8.0, 6.0, -3.0, -4.0);
            //Act
            Quaternion otherQuaternion = complex - quaternion;
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }


        /// <summary>
        /// Tests the static multiplication operator of a <see cref="Quaternion"/> number with a <see cref="Complex"/> number.
        /// </summary>
        [TestMethod("Operator Multiply(Quaternion,Complex)")]
        public void Operator_Multiply_Quaternion_Complex()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Complex complex = new Complex(9.0, 8.0);
            Quaternion result = new Quaternion(1.0 * 9.0 - 2.0 * 8.0, 1.0 * 8.0 + 2.0 * 9.0,
                  3.0 * 9.0 + 4.0 * 8.0, -3.0 * 8.0 + 4.0 * 9.0);
            //Act
            Quaternion otherQuaternion = quaternion * complex;
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the static multiplication operator of a <see cref="Complex"/> number with a <see cref="Quaternion"/> number.
        /// </summary>
        [TestMethod("Operator Multiply(Complex,Quaternion)")]
        public void Operator_Multiply_Complex_Quaternion()
        {
            // Arrange
            Complex complex = new Complex(9.0, 8.0);
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion result = new Quaternion(1.0 * 9.0 - 2.0 * 8.0, 1.0 * 8.0 + 2.0 * 9.0,
                  3.0 * 9.0 + 4.0 * 8.0, -3.0 * 8.0 + 4.0 * 9.0);
            //Act
            Quaternion otherQuaternion = complex * quaternion;
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }


        /// <summary>
        /// Tests the static division operator of a <see cref="Quaternion"/> number by a <see cref="Complex"/> number.
        /// </summary>
        [TestMethod("Operator Divide(Quaternion,Complex)")]
        public void Operator_Divide_Quaternion_Complex()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(6.0, 7.0, 8.0, 9.0);
            Complex complex = new Complex(1.0, 2.0);
            Quaternion result = new Quaternion(
                (6.0 * 1.0 + 7.0 * 2.0) / 5.0,
                (-6.0 * 2.0 + 7.0 * 1.0) / 5.0,
                (8.0 * 1.0 - 9.0 * 2.0) / 5.0,
                (8.0 * 2.0 + 9.0 * 1.0) / 5.0);
            // Act
            Quaternion otherQuaternion = quaternion / complex;
            // Assert 
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the static division operator of a <see cref="Complex"/> number by a <see cref="Quaternion"/> number.
        /// </summary>
        [TestMethod("Operator Divide(Complex,Quaternion)")]
        public void Operator_Divide_Complex_Quaternion()
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
            Quaternion otherQuaternion = complex / quaternion;
            // Assert 
            Assert.IsTrue(otherQuaternion.Equals(result));
        }


        /******************** Real Embedding ********************/

        /// <summary>
        /// Tests the static addition operator of a <see cref="Quaternion"/> number with a <see cref="Real"/> number.
        /// </summary>
        [TestMethod("Operator Add(Quaternion,Real)")]
        public void Operator_Add_Quaternion_Real()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Real real = new Real(10.0);
            Quaternion result = new Quaternion(11.0, 2.0, 3.0, 4.0);
            //Act
            Quaternion otherQuaternion = quaternion + real;
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the static addition operator of a <see cref="Real"/> number with a <see cref="Quaternion"/> number.
        /// </summary>
        [TestMethod("Operator Add(Real,Quaternion)")]
        public void Operator_Add_Real_Quaternion()
        {
            // Arrange
            Real real = new Real(10.0);
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion result = new Quaternion(11.0, 2.0, 3.0, 4.0);
            //Act
            Quaternion otherQuaternion = real + quaternion;
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }


        /// <summary>
        /// Tests the static subtraction operator of a <see cref="Quaternion"/> number with a <see cref="Real"/> number.
        /// </summary>
        [TestMethod("Operator Subtract(Quaternion,Real)")]
        public void Operator_Subtract_Quaternion_Real()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Real real = new Real(10.0);
            Quaternion result = new Quaternion(-9.0, 2.0, 3.0, 4.0);
            //Act
            Quaternion otherQuaternion = quaternion - real;
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the static subtraction operator of a <see cref="Real"/> number with a <see cref="Quaternion"/> number.
        /// </summary>
        [TestMethod("Operator Subtract(Real,Quaternion)")]
        public void Operator_Subtract_Real_Quaternion()
        {
            // Arrange
            Real real = new Real(10.0);
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion result = new Quaternion(9.0, -2.0, -3.0, -4.0);
            //Act
            Quaternion otherQuaternion = real - quaternion;
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }


        /// <summary>
        /// Tests the static multiplication operator of a <see cref="Quaternion"/> number with a <see cref="Real"/> number.
        /// </summary>
        [TestMethod("Operator Multiply(Quaternion,Real)")]
        public void Operator_Multiply_Quaternion_Real()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Real real = new Real(4.0);
            Quaternion result = new Quaternion(4.0, 8.0, 12.0, 16.0);
            //Act
            Quaternion otherQuaternion = quaternion * real;
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the static multiplication operator of a <see cref="Real"/> number with a <see cref="Quaternion"/> number.
        /// </summary>
        [TestMethod("Operator Multiply(Real,Quaternion)")]
        public void Operator_Multiply_Real_Quaternion()
        {
            // Arrange
            Real real = new Real(5.0);
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion result = new Quaternion(5.0, 10.0, 15.0, 20.0);
            //Act
            Quaternion otherQuaternion = real * quaternion;
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }


        /// <summary>
        /// Tests the static division operator of a <see cref="Quaternion"/> number by a <see cref="Real"/> number.
        /// </summary>
        [TestMethod("Operator Divide(Quaternion,Real)")]
        public void Operator_Divide_Quaternion_Real()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Real real = new Real(4.0);
            Quaternion result = new Quaternion(0.25, 0.5, 0.75, 1.0);
            //Act
            Quaternion otherQuaternion = quaternion / real;
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the static division operator of a <see cref="Real"/> number by a <see cref="Quaternion"/> number.
        /// </summary>
        [TestMethod("Operator Divide(Real,Quaternion)")]
        public void Operator_Divide_Real_Quaternion()
        {
            // Arrange
            Real real = new Real(45.0);
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion result = new Quaternion(1.5, -3.0, -4.5, -6.0);
            // Act
            Quaternion otherQuaternion = real / quaternion;
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }


        /******************** double Embedding ********************/

        /// <summary>
        /// Tests the static addition operator of a <see cref="Quaternion"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        [TestMethod("Operator Add(Quaternion,Real)")]
        public void Operator_Add_Quaternion_Double()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            double number = 10.0;
            Quaternion result = new Quaternion(11.0, 2.0, 3.0, 4.0);
            //Act
            Quaternion otherQuaternion = quaternion + number;
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the static addition operator of a <see cref="double"/>-precision real number with a <see cref="Quaternion"/> number.
        /// </summary>
        [TestMethod("Operator Add(Real,Quaternion)")]
        public void Operator_Add_Double_Quaternion()
        {
            // Arrange
            double number = 10.0;
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion result = new Quaternion(11.0, 2.0, 3.0, 4.0);
            //Act
            Quaternion otherQuaternion = number + quaternion;
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }


        /// <summary>
        /// Tests the static subtraction operator of a <see cref="Quaternion"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        [TestMethod("Operator Subtract(Quaternion,Real)")]
        public void Operator_Subtract_Quaternion_Double()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            double number = 10.0;
            Quaternion result = new Quaternion(-9.0, 2.0, 3.0, 4.0);
            //Act
            Quaternion otherQuaternion = quaternion - number;
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the static subtraction operator of a <see cref="double"/>-precision real number with a <see cref="Quaternion"/> number.
        /// </summary>
        [TestMethod("Operator Subtract(Real,Quaternion)")]
        public void Operator_Subtract_Double_Quaternion()
        {
            // Arrange
            double number = 10.0;
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion result = new Quaternion(9.0, -2.0, -3.0, -4.0);
            //Act
            Quaternion otherQuaternion = number - quaternion;
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }


        /// <summary>
        /// Tests the static multiplication operator of a <see cref="Quaternion"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        [TestMethod("Operator Multiply(Quaternion,Real)")]
        public void Operator_Multiply_Quaternion_Double()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            double number = 4.0;
            Quaternion result = new Quaternion(4.0, 8.0, 12.0, 16.0);
            //Act
            Quaternion otherQuaternion = quaternion * number;
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the static multiplication operator of a <see cref="double"/>-precision real number with a <see cref="Quaternion"/> number.
        /// </summary>
        [TestMethod("Operator Multiply(Real,Quaternion)")]
        public void Operator_Multiply_Double_Quaternion()
        {
            // Arrange
            double number = 5.0;
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion result = new Quaternion(5.0, 10.0, 15.0, 20.0);
            //Act
            Quaternion otherQuaternion = number * quaternion;
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }


        /// <summary>
        /// Tests the static division operator of a <see cref="Quaternion"/> number by a <see cref="double"/>-precision real number.
        /// </summary>
        [TestMethod("Operator Divide(Quaternion,Real)")]
        public void Operator_Divide_Quaternion_Double()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            double number = 4.0;
            Quaternion result = new Quaternion(0.25, 0.5, 0.75, 1.0);
            //Act
            Quaternion otherQuaternion = quaternion / number;
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the static division operator of a <see cref="double"/>-precision real number by a <see cref="Quaternion"/> number.
        /// </summary>
        [TestMethod("Operator Divide(Real,Quaternion)")]
        public void Operator_Divide_Double_Quaternion()
        {
            // Arrange
            double number = 45.0;
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion result = new Quaternion(1.5, -3.0, -4.5, -6.0);
            // Act
            Quaternion otherQuaternion = number / quaternion;
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        #endregion

        #region Casts
/*
        /// <summary>
        /// Tests the implicit cast of a <see cref="Complex"/> number into a <see cref="Quaternion"/> number.
        /// </summary>
        [TestMethod("Cast FromComplex")]
        public void Cast_FromComplex()
        {
            // Arrange
            Complex complex = new Complex(1.0, 2.0);
            Quaternion result = new Quaternion(1.0, 2.0, 0.0, 0.0);
            // Act
            Quaternion quaternion = complex;
            // Assert
            Assert.IsTrue(quaternion.Equals(result));
        }

        /// <summary>
        /// Tests the implicit cast of a <see cref="Real"/> number into a <see cref="Quaternion"/> number.
        /// </summary>
        [TestMethod("Cast FromReal")]
        public void Cast_FromReal()
        {
            // Arrange
            Real real = new Real(10.0);
            Quaternion result = new Quaternion(10.0, 0.0, 0.0, 0.0);
            // Act
            Quaternion quaternion = real;
            // Assert
            Assert.IsTrue(quaternion.Equals(result));
        }

        /// <summary>
        /// Tests the implicit cast of a <see cref="double"/>-precision real number into a <see cref="Quaternion"/> number.
        /// </summary>
        [TestMethod("Cast FromDouble")]
        public void Cast_FromDouble()
        {
            // Arrange
            double number = 20.0;
            Quaternion result = new Quaternion(20.0, 0.0, 0.0, 0.0);
            // Act
            Quaternion quaternion = number;
            // Assert
            Assert.IsTrue(quaternion.Equals(result));
        }

        /// <summary>
        /// Tests the implicit cast of a <see cref="ValueTuple{T1,T2,T3,T4}"/> into a <see cref="Quaternion"/> number.
        /// </summary>
        [TestMethod("Cast FromValueTuple")]
        public void Cast_FromValueTuple()
        {
            // Arrange
            ValueTuple <double,double,double,double> quadruple = new ValueTuple<double, double, double, double>(2.0, 4.0, 6.0, 8.0);
            Quaternion result = new Quaternion(2.0, 4.0, 6.0, 8.0);
            // Act
            Quaternion quaternion = quadruple;
            // Assert
            Assert.IsTrue(quaternion.Equals(result));
        }
*/
        #endregion

        #region Methods

        /// <summary>
        /// Tests the computation of the conjugate of the current <see cref="Quaternion"/> number.
        /// </summary>
        [TestMethod("Method Conjugate()")]
        public void Conjugate()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion result = new Quaternion(1.0, -2.0, -3.0, -4.0);
            // Act
            quaternion.Conjugate();
            // Assert
            Assert.IsTrue(quaternion.Equals(result));
        }

        /// <summary>
        /// Tests the computation of the opposite of the current <see cref="Quaternion"/> number.
        /// </summary>
        [TestMethod("Method Opposite()")]
        public void Opposite()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion result = new Quaternion(-1.0, -2.0, -3.0, -4.0);
            // Act
            quaternion.Opposite();
            // Assert
            Assert.IsTrue(quaternion.Equals(result));
        }

        /// <summary>
        /// Tests the computation of the inverse of the current <see cref="Quaternion"/> number.
        /// </summary>
        [TestMethod("Method Inverse()")]
        public void Inverse()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion result = new Quaternion(1.0 / 30.0, -2.0 / 30.0, -3.0 / 30.0, -4.0 / 30.0);
            // Act
            quaternion.Inverse();
            // Assert
            Assert.IsTrue(quaternion.Equals(result));
        }


        /// <summary>
        /// Tests the computation of the norm of the current <see cref="Quaternion"/> number.
        /// </summary>
        [TestMethod("Method Norm()")]
        public void Norm()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            // Act
            double norm = quaternion.Norm();
            // Assert
            Assert.AreEqual(30.0, norm, Settings.AbsolutePrecision);
        }


        /// <summary>
        /// Tests the equality comparison of the current <see cref="Quaternion"/> number with another <see cref="Quaternion"/> number.
        /// </summary>
        [TestMethod("Method Equals(Quaternion)")]
        public void Equals_Quaternion()
        {
            // Arrange
            Quaternion quaternionA = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion quaternionB = new Quaternion(1.0, 2.0, 3.0, 4.0);
            // Assert
            Assert.IsTrue(quaternionA.Equals(quaternionB));
        }

        #endregion


        #region Explicit Additive.IAbelianGroup<Complex>

        /******************** Properties ********************/

        /// <summary>
        /// Tests the <see cref="IAddable{T}.IsAssociative"/> property of <see cref="Quaternion"/> numbers.
        /// </summary>
        [TestMethod("AsIAddable<Quaternion> Property IsAssociative")]
        public void AsIAddable_IsAssociative()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            // Act
            IAddable<Quaternion> addable = (IAddable<Quaternion>)quaternion;
            // Assert
            Assert.IsTrue(addable.IsAssociative);
        }

        /// <summary>
        /// Tests the <see cref="IAddable{T}.IsCommutative"/> property of <see cref="Quaternion"/> numbers.
        /// </summary>
        [TestMethod("AsIAddable<Quaternion> Property IsCommutative")]
        public void AsIAddable_IsCommutative()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            // Act
            IAddable<Quaternion> addable = (IAddable<Quaternion>)quaternion;
            // Assert
            Assert.IsTrue(addable.IsCommutative);
        }


        /******************** Methods ********************/

        /// <summary>
        /// Tests the <see cref="IAddable{T}.Add(T)"/> method 
        /// computing the addition of the current <see cref="Quaternion"/> number with another <see cref="Quaternion"/> number.
        /// </summary>
        [TestMethod("AsIAddable<Quaternion> Add(Quaternion)")]
        public void AsIAddable_Add_Quaternion()
        {
            // Arrange
            Quaternion quaternionA = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion quaternionB = new Quaternion(6.0, 7.0, 8.0, 9.0);
            Quaternion result = new Quaternion(7.0, 9.0, 11.0, 13.0);
            //Act
            IAddable<Quaternion> addable = (IAddable<Quaternion>)quaternionA;
            Quaternion otherQuaternion = addable.Add( quaternionB);
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the <see cref="ISubtractable{T}.Subtract(T)"/> method 
        /// computing the subtraction of the current <see cref="Quaternion"/> number with another <see cref="Quaternion"/> number.
        /// </summary>
        [TestMethod("AsISubtractable<Quaternion> Subtract(Quaternion)")]
        public void AsISubtractable_Substract_Quaternion()
        {
            // Arrange
            Quaternion quaternionA = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion quaternionB = new Quaternion(9.0, -8.0, 7.0, 6.0);
            Quaternion result = new Quaternion(-8.0, 10.0, -4.0, -2.0);
            //Act
            ISubtractable<Quaternion> subtractable = (ISubtractable<Quaternion>)quaternionA;
            Quaternion otherQuaternion = subtractable.Subtract(quaternionB);
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the <see cref="IZeroable{T}.Zero"/> method returning the additive neutral element of the <see cref="Quaternion"/> numbers.
        /// </summary>
        [TestMethod("AsIZeroable<Complex> Zero()")]
        public void AsIZeroable_Zero()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion result = new Quaternion(0.0, 0.0, 0.0, 0.0);
            // Act
            IZeroable<Quaternion> zeroable = (IZeroable<Quaternion>)quaternion;
            Quaternion otherQuaternion = zeroable.Zero();
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }
        
        #endregion

        #region Explicit Additive.IAbelianGroup<Complex>

        /******************** Properties ********************/

        /// <summary>
        /// Tests the <see cref="IMultiplicable{T}.IsAssociative"/> property of <see cref="Quaternion"/> numbers.
        /// </summary>
        [TestMethod("AsIMultiplicable<Quaternion> Property IsAssociative")]
        public void AsIMultiplicable_IsAssociative()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            // Act
            IMultiplicable<Quaternion> multiplicable = (IMultiplicable<Quaternion>)quaternion;
            // Assert
            Assert.IsTrue(multiplicable.IsAssociative);
        }

        /// <summary>
        /// Tests the <see cref="IMultiplicable{T}.IsCommutative"/> property of the <see cref="Quaternion"/>.
        /// </summary>
        [TestMethod("AsIMultiplicable<Quaternion> Property IsCommutative")]
        public void AsIMultiplicable_IsCommutative()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            // Act
            IMultiplicable<Quaternion> multiplicable = (IMultiplicable<Quaternion>)quaternion;
            // Assert
            Assert.IsFalse(multiplicable.IsCommutative);
        }


        /******************** Methods ********************/

        /// <summary>
        /// Tests the <see cref="IMultiplicable{T}.Multiply(T)"/> method 
        /// computing the multiplication of the current <see cref="Quaternion"/> number with another <see cref="Quaternion"/> number.
        /// </summary>
        [TestMethod("AsIMultiplicable<Quaternion> Multiply(Quaternion)")]
        public void AsIMultiplicable_Multiply_Quaternion()
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
            IMultiplicable<Quaternion> multiplicable = (IMultiplicable<Quaternion>)quaternionA;
            Quaternion otherQuaternion = multiplicable.Multiply(quaternionB);
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the <see cref="IDivisible{T}.Divide(T)"/> method 
        /// computing the division of the current <see cref="Quaternion"/> number with another <see cref="Quaternion"/> number.
        /// </summary>
        [TestMethod("AsIDivisible<Quaternion> Divide(Quaternion)")]
        public void AsIDivisible_Divide_Quaternion()
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
            IDivisible<Quaternion> divisible = (IDivisible<Quaternion>)quaternionA;
            Quaternion otherQuaternion = divisible.Divide(quaternionB);
            // Assert 
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the <see cref="IOneable{T}.One"/> method returning the multiplicative neutral element of the <see cref="Quaternion"/> numbers.
        /// </summary>
        [TestMethod("AsIOneable<Quaternion> One()")]
        public void AsIOneable_One()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion result = new Quaternion(1.0, 0.0, 0.0, 0.0);
            // Act
            IOneable<Quaternion> oneable = (IOneable<Quaternion>)quaternion;
            Quaternion otherQuaternion = oneable.One();
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        #endregion

        #region Explicit IGroupAction<Complex,double>

        /// <summary>
        /// Tests the <see cref="IGroupAction{TValue, T}.Multiply(TValue)"/> method
        /// computing the scalar multiplication of the current <see cref="Quaternion"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        [TestMethod("AsIGroupAction<Double,Quaternion> Multiply(Double)")]
        public void AsIGroupAction_Multiply_Double()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.5, -5.0, 10.0);
            double number = 4.0;
            Quaternion result = new Quaternion(4.0, 10.0, -20.0, 40.0);
            //Act
            IGroupAction <double, Quaternion> groupActionable = (IGroupAction<double, Quaternion>)quaternion;
            Quaternion otherQuaternion = groupActionable.Multiply(number);
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        /// <summary>
        /// Tests the <see cref="IGroupAction{TValue, T}.Divide(TValue)"/> method
        /// computing the scalar division of the current <see cref="Quaternion"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        [TestMethod("AsIGroupAction<Double,Quaternion> Divide(Double)")]
        public void AsIGroupAction_Divide_Double()
        {
            // Arrange
            Quaternion quaternion = new Quaternion(1.0, 2.5, 5.0, -10.0);
            double number = 4.0;
            Quaternion result = new Quaternion(0.25, 0.625, 1.25, -2.5);
            //Act
            IGroupAction<double, Quaternion> groupActionable = (IGroupAction<double, Quaternion>)quaternion;
            Quaternion otherQuaternion = groupActionable.Divide(number);
            // Assert
            Assert.IsTrue(otherQuaternion.Equals(result));
        }

        #endregion
    }
}
