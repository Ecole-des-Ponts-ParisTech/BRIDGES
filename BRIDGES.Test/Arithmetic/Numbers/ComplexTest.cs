using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.Arithmetic.Numbers;

namespace BRIDGES.Test.Arithmetic.Numbers
{
    [TestClass]
    public class ComplexTest
    {
        #region Properties

        [TestMethod]
        public void Complex_RealAndImaginary()
        {
            // Act
            Complex complex = new Complex(2.0, 5.0);
            // Assert
            Assert.AreEqual(2.0, complex.RealPart);
            Assert.AreEqual(5.0, complex.ImaginaryPart);
        }

        [TestMethod]
        public void Complex_ModulusAndArgument()
        {
            // Act
            Complex complex = new Complex(Math.Sqrt(1.0 / 2.0), Math.Sqrt(1.0 / 2.0));
            // Assert
            Assert.AreEqual(1.0, complex.Modulus);
            Assert.AreEqual(Math.PI / 4.0, complex.Argument);
        }

        #endregion

        #region Static Members

        [TestMethod]
        public void Complex_Zero()
        {
            // Arrange
            Complex complex = new Complex(0.0, 0.0);
            // Act
            Complex otherComplex = Complex.Zero();
            // Assert
            Assert.IsTrue(complex.Equals(otherComplex));
        }

        [TestMethod]
        public void Complex_One()
        {
            // Arrange
            Complex complex = new Complex(1.0, 0.0);
            // Act
            Complex otherComplex = Complex.One();
            // Assert
            Assert.IsTrue(complex.Equals(otherComplex));
        }

        [TestMethod]
        public void Complex_ImaginaryOne()
        {
            // Arrange
            Complex complex = new Complex(0.0, 1.0);
            // Act
            Complex otherComplex = Complex.ImaginaryOne();
            // Assert
            Assert.IsTrue(complex.Equals(otherComplex));
        }

        #endregion
    }
}
