using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.Arithmetic.Numbers;
using BRIDGES.Algebra.Fundamentals;

namespace BRIDGES.Test.Arithmetic.Numbers
{
    /// <summary>
    /// Class testing the members of the <see cref="Real"/> class.
    /// </summary>
    [TestClass]
    public class RealTest
    {
        #region Behavior

        /// <summary>
        /// Tests that <see cref="Real"/> are not reference type.
        /// </summary>
        [TestMethod("Behavior IsNotReference")]
        public void IsNotReference()
        {
            // Arrange
            Real realA = new Real(1.0);
            Real realB = new Real(3.0);
            //Act
            realA = realB;
            // Assert
            Assert.IsTrue(realB.Equals(realA));
            Assert.AreNotSame(realA, realB);
        }

        #endregion


        #region Properties

        /// <summary>
        /// Tests the initialisation of the <see cref="Real.Value"/> property.
        /// </summary>
        [TestMethod("Property Value")]
        public void Value()
        {
            // Assign
            Real real = new Real(2.5);
            // Act

            // Assert
            Assert.AreEqual(2.5, real.Value, Settings.AbsolutePrecision);
        }

        #endregion

        #region Static Members

        /// <summary>
        /// Tests the initialisation of the <see cref="Real"/> corresponding to the additive neutral element.
        /// </summary>
        [TestMethod("Static Zero()")]
        public void Static_Zero()
        {
            // Arrange
            Real result = new Real(0.0);
            // Act
            Real real = Real.Zero();
            // Assert
            Assert.IsTrue(real.Equals(result));
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="Real"/> corresponding to the multiplicative neutral element.
        /// </summary>
        [TestMethod("Static One()")]
        public void Static_One()
        {
            // Arrange
            Real result = new Real(1.0);
            // Act
            Real real = Real.One();
            // Assert
            Assert.IsTrue(real.Equals(result));
        }

        #endregion

        #region Static Methods

        /******************** Algebraic Field ********************/

        /// <summary>
        /// Tests the static addition of two <see cref="Real"/>.
        /// </summary>
        [TestMethod("Static Add(Real,Real)")]
        public void Static_Add_Real_Real()
        {
            // Arrange
            Real realA = new Real(1.5);
            Real realB = new Real(5.5);
            Real result = new Real(7.0);
            //Act
            Real otherReal = Real.Add(realA, realB);
            // Assert
            Assert.IsTrue(otherReal.Equals(result));
        }

        /// <summary>
        /// Tests the static subtraction of two <see cref="Real"/>.
        /// </summary>
        [TestMethod("Static Subtract(Real,Real)")]
        public void Static_Substract_Real_Real()
        {
            // Arrange
            Real realA = new Real(1.5);
            Real realB = new Real(5.5);
            Real result = new Real(-4.0);
            //Act
            Real otherReal = Real.Subtract(realA, realB);
            // Assert
            Assert.IsTrue(otherReal.Equals(result));
        }


        /// <summary>
        /// Tests the static multiplication of two <see cref="Real"/>.
        /// </summary>
        [TestMethod("Static Multiply(Real,Real)")]
        public void Static_Multiply_Real_Real()
        {
            // Arrange
            Real realA = new Real(2.4);
            Real realB = new Real(2.0);
            Real result = new Real(4.8);
            //Act
            Real otherReal = Real.Multiply(realA, realB);
            // Assert
            Assert.IsTrue(otherReal.Equals(result));
        }

        /// <summary>
        /// Tests the static division of two <see cref="Real"/>.
        /// </summary>
        [TestMethod("Static Divide(Real,Real)")]
        public void Static_Divide_Real_Real()
        {
            // Arrange
            Real realA = new Real(4.9);
            Real realB = new Real(2.0);
            Real result = new Real(2.45);
            // Act
            Real otherReal = Real.Divide(realA, realB);
            // Assert 
            Assert.IsTrue(otherReal.Equals(result));
        }

        #endregion

        #region Operators

        /******************** Algebraic Field ********************/

        /// <summary>
        /// Tests the static addition operator of two <see cref="Real"/>.
        /// </summary>
        [TestMethod("Operator Add(Real,Real)")]
        public void Operator_Add_Real_Real()
        {
            // Arrange
            Real realA = new Real(1.5);
            Real realB = new Real(5.5);
            Real result = new Real(7.0);
            //Act
            Real otherReal = realA + realB;
            // Assert
            Assert.IsTrue(otherReal.Equals(result));
        }

        /// <summary>
        /// Tests the static Subtraction operator of two <see cref="Real"/>.
        /// </summary>
        [TestMethod("Operator Subtract(Real,Real)")]
        public void Operator_Substract_Real_Real()
        {
            // Arrange
            Real realA = new Real(1.5);
            Real realB = new Real(5.5);
            Real result = new Real(-4.0);
            //Act
            Real otherReal = realA - realB;
            // Assert
            Assert.IsTrue(otherReal.Equals(result));
        }

        /// <summary>
        /// Tests the static multiplication operator of two <see cref="Real"/>.
        /// </summary>
        [TestMethod("Operator Multiply(Real,Real)")]
        public void Operator_Multiply_Real_Real()
        {
            // Arrange
            Real realA = new Real(1.2);
            Real realB = new Real(-2.5);
            Real result = new Real(-3.0);
            //Act
            Real otherReal = realA * realB;
            // Assert
            Assert.IsTrue(otherReal.Equals(result));
        }

        /// <summary>
        /// Tests the static division of operator two <see cref="Real"/>.
        /// </summary>
        [TestMethod("Operator Divide(Real,Real)")]
        public void Operator_Divide_Real_Real()
        {
            // Arrange
            Real realA = new Real(5.0);
            Real realB = new Real(2.0);
            Real result = new Real(2.5);
            // Act
            Real otherReal = realA / realB;
            // Assert 
            Assert.IsTrue(otherReal.Equals(result));
        }


        /******************** double Embedding ********************/

        /// <summary>
        /// Tests the static addition operator of a <see cref="Real"/> with a <see cref="double"/>.
        /// </summary>
        [TestMethod("Operator Add(Real,Double)")]
        public void Operator_Add_Real_Double()
        {
            // Arrange
            Real real = new Real(1.5);
            double number = 10.0;
            Real result = new Real(11.5);
            //Act
            Real otherReal = real + number;
            // Assert
            Assert.IsTrue(otherReal.Equals(result));
        }

        /// <summary>
        /// Tests the static addition operator of a <see cref="double"/> with a <see cref="Real"/>.
        /// </summary>
        [TestMethod("Operator Add(Double,Real)")]
        public void Operator_Add_Double_Real()
        {
            // Arrange
            double number = 10.0;
            Real real = new Real(1.5);
            Real result = new Real(11.5);
            //Act
            Real otherReal = number + real;
            // Assert
            Assert.IsTrue(otherReal.Equals(result));
        }


        /// <summary>
        /// Tests the static subtraction operator of a <see cref="Real"/> with a <see cref="double"/>.
        /// </summary>
        [TestMethod("Operator Subtract(Real,Double)")]
        public void Operator_Subtract_Real_Double()
        {
            // Arrange
            Real real = new Real(11.5);
            double number = 10.0;
            Real result = new Real(1.5);
            //Act
            Real otherReal = real - number;
            // Assert
            Assert.IsTrue(otherReal.Equals(result));
        }

        /// <summary>
        /// Tests the static subtraction operator of a <see cref="double"/> with a <see cref="Real"/>.
        /// </summary>
        [TestMethod("Operator Subtract(Double,Real)")]
        public void Operator_Subtract_Double_Real()
        {
            // Arrange
            double number = 10.0;
            Real real = new Real(1.5);
            Real result = new Real(8.5);
            //Act
            Real otherReal = number - real;
            // Assert
            Assert.IsTrue(otherReal.Equals(result));
        }


        /// <summary>
        /// Tests the static multiplication operator of a <see cref="Real"/> with a <see cref="double"/>.
        /// </summary>
        [TestMethod("Operator Multiply(Real,Double)")]
        public void Operator_Multiply_Real_Double()
        {
            // Arrange
            Real real = new Real(2.0);
            double number = 4.0;
            Real result = new Real(8.0);
            //Act
            Real otherReal = real * number;
            // Assert
            Assert.IsTrue(otherReal.Equals(result));
        }

        /// <summary>
        /// Tests the static multiplication operator of a <see cref="double"/> with a <see cref="Real"/>.
        /// </summary>
        [TestMethod("Operator Multiply(Double,Real)")]
        public void Operator_Multiply_Double_Real()
        {
            // Arrange
            double number = 4.0;
            Real real = new Real(0.75);
            Real result = new Real(3.0);
            //Act
            Real otherReal = number * real;
            // Assert
            Assert.IsTrue(otherReal.Equals(result));
        }


        /// <summary>
        /// Tests the static division operator of a <see cref="Real"/> by a <see cref="double"/>.
        /// </summary>
        [TestMethod("Operator Divide(Real,Double)")]
        public void Operator_Divide_Real_Double()
        {
            // Arrange
            Real real = new Real(2.5);
            double number = -5.0;
            Real result = new Real(-0.5);
            //Act
            Real otherReal = real / number;
            // Assert
            Assert.IsTrue(otherReal.Equals(result));
        }

        /// <summary>
        /// Tests the static division operator of a <see cref="double"/> by a <see cref="Real"/>.
        /// </summary>
        [TestMethod("Operator Divide(Double,Real)")]
        public void Operator_Divide_Double_Real()
        {
            // Arrange
            double number = 5.0;
            Real real = new Real(2.0);
            Real result = new Real(2.5);
            // Act
            Real otherReal = number / real;
            // Assert
            Assert.IsTrue(otherReal.Equals(result));
        }

        #endregion

        #region Casts
        /*
                /// <summary>
                /// Tests the implicit cast of a <see cref="Real"/> into a <see cref="double"/>.
                /// </summary>
                [TestMethod("Cast ToDouble")]
                public void Cast_ToDouble()
                {
                    // Arrange
                    Real real = new Real(5.0);
                    // Act
                    double number = real;
                    // Assert
                    Assert.AreEqual(number, real.Value, Settings.AbsolutePrecision);
                }

                /// <summary>
                /// Tests the implicit cast of a <see cref="double"/> into a <see cref="Real"/>.
                /// </summary>
                [TestMethod("Cast FromDouble")]
                public void Cast_FromDouble()
                {
                    // Arrange
                    double number = -5.0;
                    //Act
                    Real real = number;
                    // Assert
                    Assert.AreEqual(real.Value, number, Settings.AbsolutePrecision);
                }
        */
        #endregion

        #region Methods

        /// <summary>
        /// Tests the computation of the opposite of the current <see cref="Real"/>.
        /// </summary>
        [TestMethod("Method Opposite()")]
        public void Opposite()
        {
            // Arrange
            Real real = new Real(5.4);
            Real result = new Real(-5.4);
            // Act
            real.Opposite();
            // Assert
            Assert.IsTrue(real.Equals(result));
        }

        /// <summary>
        /// Tests the computation of the inverse of the current <see cref="Real"/>.
        /// </summary>
        [TestMethod("Method Inverse()")]
        public void Inverse()
        {
            // Arrange
            Real real = new Real(4.0);
            Real result = new Real(0.25);
            // Act
            real.Inverse();
            // Assert
            Assert.IsTrue(real.Equals(result));
        }


        /// <summary>
        /// Tests the equality comparison of the current <see cref="Real"/> with another <see cref="Real"/>.
        /// </summary>
        [TestMethod("Method Equals(Real)")]
        public void Equals_Real()
        {
            // Arrange
            Real realA = new Real(9.85);
            Real realB = new Real(realA);
            // Assert
            Assert.IsTrue(realA.Equals(realB));
        }

        #endregion


        #region Explicit IField<Real>

        /******************** Properties ********************/

        /// <summary>
        /// Tests the <see cref="IAddable{T}.IsAssociative"/> property of the <see cref="Real"/>.
        /// </summary>
        [TestMethod("AsIAddable<Real> Property IsAssociative")]
        public void AsIAddable_IsAssociative()
        {
            // Arrange
            Real real = new Real(1.0);
            // Act
            IAddable<Real> addable = (IAddable<Real>)real;
            // Assert
            Assert.IsTrue(addable.IsAssociative);
        }

        /// <summary>
        /// Tests the <see cref="IAddable{T}.IsCommutative"/> property of the <see cref="Real"/>.
        /// </summary>
        [TestMethod("AsIAddable<Real> Property IsCommutative")]
        public void AsIAddable_IsCommutative()
        {
            // Arrange
            Real real = new Real(1.0);
            // Act
            IAddable<Real> addable = (IAddable<Real>)real;
            // Assert
            Assert.IsTrue(addable.IsCommutative);
        }


        /// <summary>
        /// Tests the <see cref="IMultiplicable{T}.IsAssociative"/> property of the <see cref="Real"/>.
        /// </summary>
        [TestMethod("AsIMultiplicable<Real> Property IsAssociative")]
        public void AsIMultiplicable_IsAssociative()
        {
            // Arrange
            Real real = new Real(1.0);
            // Act
            IMultiplicable<Real> multiplicable = (IMultiplicable<Real>)real;
            // Assert
            Assert.IsTrue(multiplicable.IsAssociative);
        }

        /// <summary>
        /// Tests the <see cref="IMultiplicable{T}.IsCommutative"/> property of the <see cref="Real"/>.
        /// </summary>
        [TestMethod("AsIMultiplicable<Real> Property IsCommutative")]
        public void AsIMultiplicable_IsCommutative()
        {
            // Arrange
            Real real = new Real(1.0);
            // Act
            IMultiplicable<Real> multiplicable = (IMultiplicable<Real>)real;
            // Assert
            Assert.IsTrue(multiplicable.IsCommutative);
        }


        /******************** Methods ********************/

        /// <summary>
        /// Tests the <see cref="IAddable{T}.Add(T)"/> method of the <see cref="Real"/>.
        /// </summary>
        [TestMethod("AsIAddable<Real> Add(Real)")]
        public void AsIAddable_Add_Real()
        {
            // Arrange
            Real realA = new Real(1.5);
            Real realB = new Real(5.5);
            Real result = new Real(7.0);
            //Act
            IAddable<Real> addable = (IAddable<Real>)realA;
            Real otherReal = addable.Add(realB);
            // Assert
            Assert.IsTrue(otherReal.Equals(result));
        }

        /// <summary>
        /// Tests the <see cref="ISubtractable{T}.Subtract(T)"/> method of the <see cref="Real"/>.
        /// </summary>
        [TestMethod("AsISubtractable<Real> Subtract(Real)")]
        public void AsISubtractable_Substract_Real()
        {
            // Arrange
            Real realA = new Real(1.5);
            Real realB = new Real(5.5);
            Real result = new Real(-4.0);
            //Act
            ISubtractable<Real> subtractable = (ISubtractable<Real>)realA;
            Real otherReal = subtractable.Subtract(realB);
            // Assert
            Assert.IsTrue(otherReal.Equals(result));
        }

        /// <summary>
        /// Tests the <see cref="IZeroable{T}.Zero"/> method of the <see cref="Real"/>.
        /// </summary>
        [TestMethod("AsIZeroable<Real> Zero()")]
        public void AsIZeroable_Zero()
        {
            // Arrange
            Real real = new Real(1.5);
            Real result = new Real(0.0);
            //Act
            IZeroable<Real> zeroable = (IZeroable<Real>)real;
            Real otherReal = zeroable.Zero();
            // Assert
            Assert.IsTrue(otherReal.Equals(result));
        }


        /// <summary>
        /// Tests the <see cref="IMultiplicable{T}.Multiply(T)"/> method of the <see cref="Real"/>.
        /// </summary>
        [TestMethod("AsIMultiplicable<Real> Multiply(Real)")]
        public void AsIMultiplicable_Multiply_Real()
        {
            // Arrange
            Real realA = new Real(1.5);
            Real realB = new Real(-2.0);
            Real result = new Real(-3.0);
            //Act
            IMultiplicable<Real> multiplicable = (IMultiplicable<Real>)realA;
            Real otherReal = multiplicable.Multiply(realB);
            // Assert
            Assert.IsTrue(otherReal.Equals(result));
        }

        /// <summary>
        /// Tests the <see cref="IDivisible{T}.Divide(T)"/> method of the <see cref="Real"/>.
        /// </summary>
        [TestMethod("AsIDivisible<Real> Divide(Real)")]
        public void AsIDivisible_Divide_Real()
        {
            // Arrange
            Real realA = new Real(8.0);
            Real realB = new Real(2.0);
            Real result = new Real(4.0);
            // Act
            IDivisible<Real> divisible = (IDivisible<Real>)realA;
            Real otherReal = divisible.Divide(realB);
            // Assert 
            Assert.IsTrue(otherReal.Equals(result));
        }

        /// <summary>
        /// Tests the <see cref="IOneable{T}.One"/> method of the <see cref="Real"/>.
        /// </summary>
        [TestMethod("AsIOneable<Real> One()")]
        public void AsIOneable_One()
        {
            // Arrange
            Real real = new Real(2.5);
            Real result = new Real(1.0);
            //Act
            IOneable<Real> oneable = (IOneable<Real>)real;
            Real otherReal = oneable.One();
            // Assert
            Assert.IsTrue(otherReal.Equals(result));
        }

        #endregion
    }
}
