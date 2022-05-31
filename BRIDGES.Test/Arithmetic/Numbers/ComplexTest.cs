using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.Arithmetic.Numbers;
using BRIDGES.Algebra.Fundamentals;
using BRIDGES.Algebra.Structures;


namespace BRIDGES.Test.Arithmetic.Numbers
{
    /// <summary>
    /// Class testing the members of the <see cref="Complex"/> class.
    /// </summary>
    [TestClass]
    public class ComplexTest
    {
        #region Behavior

        /// <summary>
        /// Tests that <see cref="Complex"/> are not reference type.
        /// </summary>
        [TestMethod("Behavior IsNotReference")]
        public void IsNotReference()
        {
            // Arrange
            Complex complexA = new Complex(1.0, 2.0);
            Complex complexB = new Complex(3.0, 4.0);
            //Act
            complexA = complexB;
            // Assert
            Assert.IsTrue(complexB.Equals(complexA));
            Assert.AreNotSame(complexA, complexB);
        }

        #endregion


        #region Properties

        /// <summary>
        /// Tests the initialisation of the <see cref="Complex.RealPart"/> and the <see cref="Complex.ImaginaryPart"/> properties.
        /// </summary>
        [TestMethod("Property RealPart & ImaginaryPart")]
        public void RealPartAndImaginaryPart()
        {
            // Assign
            Complex complex = new Complex(2.0, 5.0);
            // Act

            // Assert
            Assert.AreEqual(2.0, complex.RealPart, Settings.AbsolutePrecision);
            Assert.AreEqual(5.0, complex.ImaginaryPart, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the computation of the <see cref="Complex.Modulus"/> and the <see cref="Complex.Argument"/> properties.
        /// </summary>
        [TestMethod("Property Modulus & Argument")]
        public void ModulusAndArgument()
        {
            // Assign
            Complex complex = new Complex(Math.Sqrt(1.0 / 2.0), Math.Sqrt(1.0 / 2.0));
            // Act

            // Assert
            Assert.AreEqual(1.0, complex.Modulus, Settings.AbsolutePrecision);
            Assert.AreEqual(Math.PI / 4.0, complex.Argument, Settings.AngularPrecision);
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Tests the initialisation of the <see cref="Complex"/> number corresponding to the additive neutral element.
        /// </summary>
        [TestMethod("Static Zero()")]
        public void Static_Zero()
        {
            // Arrange
            Complex result = new Complex(0.0, 0.0);
            // Act
            Complex complex = Complex.Zero;
            // Assert
            Assert.IsTrue(complex.Equals(result));
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="Complex"/> number corresponding to the multiplicative neutral element.
        /// </summary>
        [TestMethod("Static One()")]
        public void Static_One()
        {
            // Arrange
            Complex result = new Complex(1.0, 0.0);
            // Act
            Complex complex = Complex.One;
            // Assert
            Assert.IsTrue(complex.Equals(result));
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="Complex"/> number corresponding to unit imaginary element.
        /// </summary>
        [TestMethod("Static ImaginaryOne()")]
        public void Static_ImaginaryOne()
        {
            // Arrange
            Complex result = new Complex(0.0, 1.0);
            // Act
            Complex complex = Complex.ImaginaryOne;
            // Assert
            Assert.IsTrue(complex.Equals(result));
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Tests the initialisation of a <see cref="Complex"/> number its from polar coordinates.
        /// </summary>
        [TestMethod("Static FromPolarCoordinates(Double,Double)")]
        public void Static_FromPolarCoordinates()
        {
            // Arrange
            Complex complex = Complex.FromPolarCoordinates(2.0, Math.PI / 6.0);
            // Act
            double real = Math.Sqrt(3.0);
            double imaginary = 1.0;
            // Assert
            Assert.AreEqual(real, complex.RealPart, Settings.AbsolutePrecision);
            Assert.AreEqual(imaginary, complex.ImaginaryPart, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the computation of the conjugate of a given <see cref="Complex"/> number.
        /// </summary>
        [TestMethod("Static Conjugate(Complex)")]
        public void Static_Conjugate_Complex()
        {
            // Arrange
            Complex complex = new Complex(3.0, 1.0);
            Complex result = new Complex(3.0, -1.0);
            // Act
            Complex conjugate = Complex.Conjugate(complex);
            // Assert
            Assert.IsTrue(conjugate.Equals(result));
        }


        /******************** Algebraic Field ********************/

        /// <summary>
        /// Tests the static addition of two <see cref="Complex"/> numbers.
        /// </summary>
        [TestMethod("Static Add(Complex,Complex)")]
        public void Static_Add_Complex_Complex()
        {
            // Arrange
            Complex complexA = new Complex(1.5, 6.0);
            Complex complexB = new Complex(5.5, 2.0);
            Complex result = new Complex(7.0, 8.0);
            //Act
            Complex otherComplex = Complex.Add(complexA, complexB);
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }

        /// <summary>
        /// Tests the static subtraction of two <see cref="Complex"/> numbers.
        /// </summary>
        [TestMethod("Static Subtract(Complex,Complex)")]
        public void Static_Substract_Complex_Complex()
        {
            // Arrange
            Complex complexA = new Complex(1.5, 6.0);
            Complex complexB = new Complex(5.5, 2.0);
            Complex result = new Complex(-4.0, 4.0);
            //Act
            Complex otherComplex = Complex.Subtract(complexA, complexB);
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }

        /// <summary>
        /// Tests the computation of the opposite of a given <see cref="Complex"/> number.
        /// </summary>
        [TestMethod("Static Opposite(Complex)")]
        public void Static_Opposite_Complex()
        {
            // Arrange
            Complex complex = new Complex(1.0, -5.0);
            Complex result = new Complex(-1.0, 5.0);
            // Act
             Complex otherComplex = Complex.Opposite(complex);
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }


        /// <summary>
        /// Tests the static multiplication of two <see cref="Complex"/> numbers.
        /// </summary>
        [TestMethod("Static Multiply(Complex,Complex)")]
        public void Static_Multiply_Complex_Complex()
        {
            // Arrange
            Complex complexA = new Complex(1.0, 2.5);
            Complex complexB = new Complex(2.0, 3.0);
            Complex result = new Complex(-5.5, 8.0);
            //Act
            Complex otherComplex = Complex.Multiply(complexA, complexB);
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }

        /// <summary>
        /// Tests the static division of two <see cref="Complex"/> numbers.
        /// </summary>
        [TestMethod("Static Divide(Complex,Complex)")]
        public void Static_Divide_Complex_Complex()
        {
            // Arrange
            Complex complexA = new Complex(4.0, 3.0);
            Complex complexB = new Complex(2.0, 1.0);
            Complex result = new Complex(11.0 / 5.0, 2.0 / 5.0);
            // Act
            Complex otherComplex = Complex.Divide(complexA, complexB);
            // Assert 
            Assert.IsTrue(otherComplex.Equals(result));
        }

        /// <summary>
        /// Tests the computation of the inverse of a given <see cref="Complex"/> number.
        /// </summary>
        [TestMethod("Static Inverse(Complex)")]
        public void Static_Inverse_Complex()
        {
            // Arrange
            Complex complex = new Complex(2.0, 3.0);
            Complex result = new Complex(2.0 / 13.0, -3.0 / 13.0);
            // Act
            Complex otherComplex = Complex.Inverse(complex);
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }


        /******************** Real Embedding ********************/

        /// <summary>
        /// Tests the static addition of a <see cref="Complex"/> number with a <see cref="Real"/> number.
        /// </summary>
        [TestMethod("Static Add(Complex,Real)")]
        public void Static_Add_Complex_Real()
        {
            // Arrange
            Complex complex = new Complex(1.5, 6.0);
            Real real = new Real(10.0);
            Complex result = new Complex(11.5, 6.0);
            //Act
            Complex otherComplex = Complex.Add(complex, real);
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }

        /// <summary>
        /// Tests the static addition of a <see cref="Real"/> number with a <see cref="Complex"/> number.
        /// </summary>
        [TestMethod("Static Add(Real,Complex)")]
        public void Static_Add_Real_Complex()
        {
            // Arrange
            Real real = new Real(10.0);
            Complex complex = new Complex(1.5, 6.0);
            Complex result = new Complex(11.5, 6.0);
            //Act
            Complex otherComplex = Complex.Add(real, complex);
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }


        /// <summary>
        /// Tests the static subtraction of a <see cref="Complex"/> number with a <see cref="Real"/> number.
        /// </summary>
        [TestMethod("Static Subtract(Complex,Real)")]
        public void Static_Subtract_Complex_Real()
        {
            // Arrange
            Complex complex = new Complex(11.5, 5.0);
            Real real = new Real(10.0);
            Complex result = new Complex(1.5, 5.0);
            //Act
            Complex otherComplex = Complex.Subtract(complex, real);
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }

        /// <summary>
        /// Tests the static subtraction of a <see cref="Real"/> number with a <see cref="Complex"/> number.
        /// </summary>
        [TestMethod("Static Subtract(Real,Complex)")]
        public void Static_Subtract_Real_Complex()
        {
            // Arrange
            Real real = new Real(10.0);
            Complex complex = new Complex(1.5, 6.0);
            Complex result = new Complex(8.5, -6.0);
            //Act
            Complex otherComplex = Complex.Subtract(real, complex);
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }


        /// <summary>
        /// Tests the static multiplication of a <see cref="Complex"/> number with a <see cref="Real"/> number.
        /// </summary>
        [TestMethod("Static Multiply(Complex,Real)")]
        public void Static_Multiply_Complex_Real()
        {
            // Arrange
            Complex complex = new Complex(1.0, 2.5);
            Real real = new Real(4.0);
            Complex result = new Complex(4.0, 10.0);
            //Act
            Complex otherComplex = Complex.Multiply(complex, real);
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }

        /// <summary>
        /// Tests the static multiplication of a <see cref="Real"/> number with a <see cref="Complex"/> number.
        /// </summary>
        [TestMethod("Static Multiply(Real,Complex)")]
        public void Static_Multiply_Real_Complex()
        {
            // Arrange
            Real real = new Real(4.0);
            Complex complex = new Complex(1.0, 2.5);
            Complex result = new Complex(4.0, 10.0);
            //Act
            Complex otherComplex = Complex.Multiply(real, complex);
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }


        /// <summary>
        /// Tests the static division of a <see cref="Complex"/> number by a <see cref="Real"/> number.
        /// </summary>
        [TestMethod("Static Divide(Complex,Real)")]
        public void Static_Divide_Complex_Real()
        {
            // Arrange
            Complex complex = new Complex(1.0, 6.0);
            Real real = new Real(4.0);
            Complex result = new Complex(0.25, 1.5);
            //Act
            Complex otherComplex = Complex.Divide(complex, real);
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }

        /// <summary>
        /// Tests the static division of a <see cref="Real"/> number by a <see cref="Complex"/> number.
        /// </summary>
        [TestMethod("Static Divide(Real,Complex)")]
        public void Static_Divide_Real_Complex()
        {
            // Arrange
            Real real = new Real(5.0);
            Complex complex = new Complex(1.0, 3.0);
            Complex result = new Complex(0.5, -1.5);
            // Act
            Complex otherComplex = Complex.Divide(real, complex);
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Tests the static multiplication of a <see cref="double"/>-precision real number with a <see cref="Complex"/> number.
        /// </summary>
        [TestMethod("Static Multiply(Double,Complex)")]
        public void Static_Multiply_Double_Complex()
        {
            // Arrange
            double number = 4.0;
            Complex complex = new Complex(1.0, 2.5);
            Complex result = new Complex(4.0, 10.0);
            //Act
            Complex otherComplex = Complex.Multiply(number, complex);
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }

        /// <summary>
        /// Tests the static division of a <see cref="Complex"/> number by a <see cref="double"/>-precision real number.
        /// </summary>
        [TestMethod("Static Divide(Complex,Double)")]
        public void Static_Divide_Complex_Double()
        {
            // Arrange
            Complex complex = new Complex(1.0, 6.0);
            double number = 4.0;
            Complex result = new Complex(0.25, 1.5);
            //Act
            Complex otherComplex = Complex.Divide(complex, number);
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }

        #endregion

        #region Operators

        /******************** Algebraic Field ********************/

        /// <summary>
        /// Tests the static addition operator of two <see cref="Complex"/> numbers.
        /// </summary>
        [TestMethod("Operator Add(Complex,Complex)")]
        public void Operator_Add_Complex_Complex()
        {
            // Arrange
            Complex complexA = new Complex(1.5, 6.0);
            Complex complexB = new Complex(5.5, 2.0);
            Complex result = new Complex(7.0, 8.0);
            //Act
            Complex otherComplex = complexA + complexB;
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }

        /// <summary>
        /// Tests the static Subtraction operator of two <see cref="Complex"/> numbers.
        /// </summary>
        [TestMethod("Operator Subtract(Complex,Complex)")]
        public void Operator_Substract_Complex_Complex()
        {
            // Arrange
            Complex complexA = new Complex(1.5, 6.0);
            Complex complexB = new Complex(5.5, 2.0);
            Complex result = new Complex(-4.0, 4.0);
            //Act
            Complex otherComplex = complexA - complexB;
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }

        /// <summary>
        /// Tests the static opposite operator of a <see cref="Complex"/> number.
        /// </summary>
        [TestMethod("Operator Subtract(Complex)")]
        public void Operator_Substract_Complex()
        {
            // Arrange
            Complex complex = new Complex(5.0, -15.0);
            Complex result = new Complex(-5.0, 15.0);
            // Act
            Complex otherComplex = -complex;
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }

        /// <summary>
        /// Tests the static multiplication operator of two <see cref="Complex"/> numbers.
        /// </summary>
        [TestMethod("Operator Multiply(Complex,Complex)")]
        public void Operator_Multiply_Complex_Complex()
        {
            // Arrange
            Complex complexA = new Complex(1.0, 2.5);
            Complex complexB = new Complex(2.0, 3.0);
            Complex result = new Complex(-5.5, 8.0);
            //Act
            Complex otherComplex = complexA * complexB;
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }

        /// <summary>
        /// Tests the static division of operator two <see cref="Complex"/> numbers.
        /// </summary>
        [TestMethod("Operator Divide(Complex,Complex)")]
        public void Operator_Divide_Complex_Complex()
        {
            // Arrange
            Complex complexA = new Complex(4.0, 3.0);
            Complex complexB = new Complex(2.0, 1.0);
            Complex result = new Complex(11.0 / 5.0, 2.0 / 5.0);
            // Act
            Complex otherComplex = complexA / complexB;
            // Assert 
            Assert.IsTrue(otherComplex.Equals(result));
        }


        /******************** Real Embedding ********************/

        /// <summary>
        /// Tests the static addition operator of a <see cref="Complex"/> number with a <see cref="Real"/> number.
        /// </summary>
        [TestMethod("Operator Add(Complex,Real)")]
        public void Operator_Add_Complex_Real()
        {
            // Arrange
            Complex complex = new Complex(1.5, 6.0);
            Real real = new Real(10.0);
            Complex result = new Complex(11.5, 6.0);
            //Act
            Complex otherComplex = complex + real;
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }

        /// <summary>
        /// Tests the static addition operator of a <see cref="Real"/> number with a <see cref="Complex"/> number.
        /// </summary>
        [TestMethod("Operator Add(Real,Complex)")]
        public void Operator_Add_Real_Complex()
        {
            // Arrange
            Real real = new Real(10.0);
            Complex complex = new Complex(1.5, 6.0);
            Complex result = new Complex(11.5, 6.0);
            //Act
            Complex otherComplex = real + complex;
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }


        /// <summary>
        /// Tests the static subtraction operator of a <see cref="Complex"/> number with a <see cref="Real"/> number.
        /// </summary>
        [TestMethod("Operator Subtract(Complex,Real)")]
        public void Operator_Subtract_Complex_Real()
        {
            // Arrange
            Complex complex = new Complex(11.5, 5.0);
            Real real = new Real(10.0);
            Complex result = new Complex(1.5, 5.0);
            //Act
            Complex otherComplex = complex - real;
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }

        /// <summary>
        /// Tests the static subtraction operator of a <see cref="Real"/> number with a <see cref="Complex"/> number.
        /// </summary>
        [TestMethod("Operator Subtract(Real,Complex)")]
        public void Operator_Subtract_Real_Complex()
        {
            // Arrange

            Real real = new Real(10.0);
            Complex complex = new Complex(1.5, 6.0);
            Complex result = new Complex(8.5, -6.0);
            //Act
            Complex otherComplex = real - complex;
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }


        /// <summary>
        /// Tests the static multiplication operator of a <see cref="Complex"/> number with a <see cref="Real"/> number.
        /// </summary>
        [TestMethod("Operator Multiply(Complex,Real)")]
        public void Operator_Multiply_Complex_Real()
        {
            // Arrange
            Complex complex = new Complex(1.0, 2.5);
            Real real = new Real(4.0);
            Complex result = new Complex(4.0, 10.0);
            //Act
            Complex otherComplex = complex * real;
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }

        /// <summary>
        /// Tests the static multiplication operator of a <see cref="Real"/> number with a <see cref="Complex"/> number.
        /// </summary>
        [TestMethod("Operator Multiply(Real,Complex)")]
        public void Operator_Multiply_Real_Complex()
        {
            // Arrange
            Real real = new Real(4.0);
            Complex complex = new Complex(1.0, 2.5);
            Complex result = new Complex(4.0, 10.0);
            //Act
            Complex otherComplex = real * complex;
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }


        /// <summary>
        /// Tests the static division operator of a <see cref="Complex"/> number by a <see cref="Real"/> number.
        /// </summary>
        [TestMethod("Operator Divide(Complex,Real)")]
        public void Operator_Divide_Complex_Real()
        {
            // Arrange
            Complex complex = new Complex(1.0, 6.0);
            Real real = new Real(4.0);
            Complex result = new Complex(0.25, 1.5);
            //Act
            Complex otherComplex = complex / real;
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }

        /// <summary>
        /// Tests the static division operator of a <see cref="Real"/> number by a <see cref="Complex"/> number.
        /// </summary>
        [TestMethod("Operator Divide(Real,Complex)")]
        public void Operator_Divide_Real_Complex()
        {
            // Arrange
            Real real = new Real(5.0);
            Complex complex = new Complex(1.0, 3.0);
            Complex result = new Complex(0.5, -1.5);
            // Act
            Complex otherComplex = real / complex;
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }
        

        /******************** double Embedding ********************/

        /// <summary>
        /// Tests the static addition operator of a <see cref="Complex"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        [TestMethod("Operator Add(Complex,Double)")]
        public void Operator_Add_Complex_Double()
        {
            // Arrange
            Complex complex = new Complex(1.5, 6.0);
            double number = 10.0;
            Complex result = new Complex(11.5, 6.0);
            //Act
            Complex otherComplex = complex + number;
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }

        /// <summary>
        /// Tests the static addition operator of a <see cref="double"/>-precision real number with a <see cref="Complex"/> number.
        /// </summary>
        [TestMethod("Operator Add(Double,Complex)")]
        public void Operator_Add_Double_Complex()
        {
            // Arrange
            double number = 10.0;
            Complex complex = new Complex(1.5, 6.0);
            Complex result = new Complex(11.5, 6.0);
            //Act
            Complex otherComplex = number + complex;
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }


        /// <summary>
        /// Tests the static subtraction operator of a <see cref="Complex"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        [TestMethod("Operator Subtract(Complex,Double)")]
        public void Operator_Subtract_Complex_Double()
        {
            // Arrange
            Complex complex = new Complex(11.5, 5.0);
            double number = 10.0;
            Complex result = new Complex(1.5, 5.0);
            //Act
            Complex otherComplex = complex - number;
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }

        /// <summary>
        /// Tests the static subtraction operator of a <see cref="double"/>-precision real number with a <see cref="Complex"/> number.
        /// </summary>
        [TestMethod("Operator Subtract(Double,Complex)")]
        public void Operator_Subtract_Double_Complex()
        {
            // Arrange
            double number = 10.0;
            Complex complex = new Complex(1.5, 6.0);
            Complex result = new Complex(8.5, -6.0);
            //Act
            Complex otherComplex = number - complex;
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }


        /// <summary>
        /// Tests the static multiplication operator of a <see cref="Complex"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        [TestMethod("Operator Multiply(Complex,Double)")]
        public void Operator_Multiply_Complex_Double()
        {
            // Arrange
            Complex complex = new Complex(1.0, 2.5);
            double number = 4.0;
            Complex result = new Complex(4.0, 10.0);
            //Act
            Complex otherComplex = complex * number;
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }

        /// <summary>
        /// Tests the static multiplication operator of a <see cref="double"/>-precision real number with a <see cref="Complex"/> number.
        /// </summary>
        [TestMethod("Operator Multiply(Double,Complex)")]
        public void Operator_Multiply_Double_Complex()
        {
            // Arrange
            double number = 4.0;
            Complex complex = new Complex(1.0, 2.5);
            Complex result = new Complex(4.0, 10.0);
            //Act
            Complex otherComplex = number * complex;
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }


        /// <summary>
        /// Tests the static division operator of a <see cref="Complex"/> number by a <see cref="double"/>-precision real number.
        /// </summary>
        [TestMethod("Operator Divide(Complex,Double)")]
        public void Operator_Divide_Complex_Double()
        {
            // Arrange
            Complex complex = new Complex(1.0, 6.0);
            double number = 4.0;
            Complex result = new Complex(0.25, 1.5);
            //Act
            Complex otherComplex = complex / number;
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }

        /// <summary>
        /// Tests the static division operator of a <see cref="double"/>-precision real number by a <see cref="Complex"/> number.
        /// </summary>
        [TestMethod("Operator Divide(Double,Complex)")]
        public void Operator_Divide_Double_Complex()
        {
            // Arrange
            double number = 5.0;
            Complex complex = new Complex(1.0, 3.0);
            Complex result = new Complex(0.5, -1.5);
            // Act
            Complex otherComplex = number / complex;
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }

        #endregion

        #region Casts
        /*
                /// <summary>
                /// Tests the implicit cast of a <see cref="Real"/> number into a <see cref="Complex"/> number.
                /// </summary>
                [TestMethod("Cast FromReal")]
                public void Cast_FromReal()
                {
                    // Arrange
                    Real real = new Real(10.0);
                    Complex result = new Complex(10.0, 0.0);
                    // Act
                    Complex complex = real;
                    // Assert
                    Assert.IsTrue(complex.Equals(result));
                }

                /// <summary>
                /// Tests the implicit cast of a <see cref="double"/>-precision real number into a <see cref="Complex"/> number.
                /// </summary>
                [TestMethod("Cast FromDouble")]
                public void Cast_FromDouble()
                {
                    // Arrange
                    double number = 20.0;
                    Complex result = new Complex(20.0, 0.0);
                    // Act
                    Complex complex = number;
                    // Assert
                    Assert.IsTrue(complex.Equals(result));
                }

                /// <summary>
                /// Tests the implicit cast of a <see cref="ValueTuple{T1,T2}"/> into a <see cref="Complex"/> number.
                /// </summary>
                [TestMethod("Cast FromValueTuple")]
                public void Cast_FromValueTuple()
                {
                    // Arrange
                    ValueTuple<double,double> pair = new ValueTuple<double, double>(2.0, 4.0);
                    Complex result = new Complex(2.0, 4.0);
                    // Act
                    Complex complex = pair;
                    // Assert
                    Assert.IsTrue(complex.Equals(result));
                }
        */
        #endregion

        #region Methods

        /// <summary>
        /// Tests the computation of the conjugate of the current <see cref="Complex"/> number.
        /// </summary>
        [TestMethod("Method Conjugate()")]
        public void Conjugate()
        {
            // Arrange
            Complex complex = new Complex(1.0, -5.0);
            Complex result = new Complex(1.0, 5.0);
            // Act
            complex.Conjugate();
            // Assert
            Assert.IsTrue(complex.Equals(result));
        }

        /// <summary>
        /// Tests the computation of the opposite of the current <see cref="Complex"/> number.
        /// </summary>
        [TestMethod("Method Opposite()")]
        public void Opposite()
        {
            // Arrange
            Complex complex = new Complex(1.0, -5.0);
            Complex result = new Complex(-1.0, 5.0);
            // Act
            complex.Opposite();
            // Assert
            Assert.IsTrue(complex.Equals(result));
        }

        /// <summary>
        /// Tests the computation of the inverse of the current <see cref="Complex"/> number.
        /// </summary>
        [TestMethod("Method Inverse()")]
        public void Inverse()
        {
            // Arrange
            Complex complex = new Complex(2.0, 3.0);
            Complex result = new Complex(2.0 / 13.0, -3.0 / 13.0);
            // Act
            complex.Inverse();
            // Assert
            Assert.IsTrue(complex.Equals(result));
        }


        /// <summary>
        /// Tests the computation of the norm of the current <see cref="Complex"/> number.
        /// </summary>
        [TestMethod("Method Norm()")]
        public void Norm()
        {
            // Arrange
            Complex complex = new Complex(3.0, 4.0);
            // Act
            double norm = complex.Norm();
            // Assert
            Assert.AreEqual(25.0, norm, Settings.AbsolutePrecision);
        }


        /// <summary>
        /// Tests the equality comparison of the current <see cref="Complex"/> number with another <see cref="Complex"/> number.
        /// </summary>
        [TestMethod("Method Equals(Complex)")]
        public void Equals_Complex()
        {
            // Arrange
            Complex complexA = new Complex(10.0, -4.0);
            Complex complexB = new Complex(10.0, -4.0);
            // Assert
            Assert.IsTrue(complexA.Equals(complexB));
        }

        #endregion



        #region Explicit Additive.IAbelianGroup<Complex>

        /******************** Properties ********************/

        /// <summary>
        /// Tests the <see cref="IAddable{T}.IsAssociative"/> property of <see cref="Complex"/> numbers.
        /// </summary>
        [TestMethod("AsIAddable<Complex> Property IsAssociative")]
        public void AsIAddable_IsAssociative()
        {
            // Arrange
            Complex complex = new Complex(1.0, -5.0);
            // Act
            IAddable<Complex> addable = (IAddable<Complex>)complex;
            // Assert
            Assert.IsTrue(addable.IsAssociative);
        }

        /// <summary>
        /// Tests the <see cref="IAddable{T}.IsCommutative"/> property of <see cref="Complex"/> numbers.
        /// </summary>
        [TestMethod("AsIAddable<Complex> Property IsCommutative")]
        public void AsIAddable_IsCommutative()
        {
            // Arrange
            Complex complex = new Complex(1.0, -5.0);
            IAddable<Complex> addable = (IAddable<Complex>)complex;
            // Act
            complex.Conjugate();
            // Assert
            Assert.IsTrue(addable.IsCommutative);
        }


        /******************** Methods ********************/

        /// <summary>
        /// Tests the <see cref="IAddable{T}.Add(T)"/> method 
        /// computing the addition of the current <see cref="Complex"/> number with another <see cref="Complex"/> number.
        /// </summary>
        [TestMethod("AsIAddable<Complex> Add(Complex)")]
        public void AsIAddable_Add_Complex()
        {
            // Arrange
            Complex complexA = new Complex(1.5, 6.0);
            Complex complexB = new Complex(5.5, 2.0);
            Complex result = new Complex(7.0, 8.0);
            //Act
            IAddable<Complex> addable = (IAddable<Complex>)complexA;
            Complex otherComplex = addable.Add(complexB);
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }

        /// <summary>
        /// Tests the <see cref="ISubtractable{T}.Subtract(T)"/> method 
        /// computing the subtraction of the current <see cref="Complex"/> number with another <see cref="Complex"/> number.
        /// </summary>
        [TestMethod("AsISubtractable<Complex> Subtract(Complex)")]
        public void AsISubtractable_Substract_Complex()
        {
            // Arrange
            Complex complexA = new Complex(1.5, 6.0);
            Complex complexB = new Complex(5.5, 2.0);
            Complex result = new Complex(-4.0, 4.0);
            //Act
            ISubtractable<Complex> subtractable = (ISubtractable<Complex>)complexA;
            Complex otherComplex = subtractable.Subtract(complexB);
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }

        /// <summary>
        /// Tests the <see cref="IZeroable{T}.Zero"/> method returning the additive neutral element of the <see cref="Complex"/> numbers.
        /// </summary>
        [TestMethod("AsIZeroable<Complex> Zero()")]
        public void AsIZeroable_Zero()
        {
            // Arrange
            Complex complex = new Complex(1.5, 6.0);
            Complex result = new Complex(0.0, 0.0);
            //Act
            IZeroable<Complex> zeroable = (IZeroable<Complex>)complex;
            Complex otherComplex = zeroable.Zero();
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }

        #endregion

        #region Explicit Additive.IAbelianGroup<Complex>

        /******************** Properties ********************/

        /// <summary>
        /// Tests the <see cref="IMultiplicable{T}.IsAssociative"/> property of <see cref="Complex"/> numbers.
        /// </summary>
        [TestMethod("AsIMultiplicable<Complex> Property IsAssociative")]
        public void AsIMultiplicable_IsAssociative()
        {
            // Arrange
            Complex complex = new Complex(1.0, -5.0);
            // Act
            IMultiplicable<Complex> multiplicable = (IMultiplicable<Complex>)complex;
            // Assert
            Assert.IsTrue(multiplicable.IsAssociative);
        }

        /// <summary>
        /// Tests the <see cref="IMultiplicable{T}.IsCommutative"/> property of the <see cref="Complex"/>.
        /// </summary>
        [TestMethod("AsIMultiplicable<Complex> Property IsCommutative")]
        public void AsIMultiplicable_IsCommutative()
        {
            // Arrange
            Complex complex = new Complex(1.0, -5.0);
            // Act
            IMultiplicable<Complex> multiplicable = (IMultiplicable<Complex>)complex;
            // Assert
            Assert.IsTrue(multiplicable.IsCommutative);
        }


        /******************** Methods ********************/

        /// <summary>
        /// Tests the <see cref="IMultiplicable{T}.Multiply(T)"/> method
        /// computing the multiplication of the current <see cref="Complex"/> number with another <see cref="Complex"/> number.
        /// </summary>
        [TestMethod("AsIMultiplicable<Complex> Multiply(Complex)")]
        public void AsIMultiplicable_Multiply_Complex()
        {
            // Arrange
            Complex complexA = new Complex(1.0, 2.5);
            Complex complexB = new Complex(2.0, 3.0);
            Complex result = new Complex(-5.5, 8.0);
            //Act
            IMultiplicable<Complex> multiplicable = (IMultiplicable<Complex>)complexA;
            Complex otherComplex = multiplicable.Multiply(complexB);
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }

        /// <summary>
        /// Tests the <see cref="IDivisible{T}.Divide(T)"/> method
        /// computing the division of the current <see cref="Complex"/> number with another <see cref="Complex"/> number.
        /// </summary>
        [TestMethod("AsIDivisible<Complex> Divide(Complex)")]
        public void AsIDivisible_Divide_Complex()
        {
            // Arrange
            Complex complexA = new Complex(4.0, 3.0);
            Complex complexB = new Complex(2.0, 1.0);
            Complex result = new Complex(11.0 / 5.0, 2.0 / 5.0);
            // Act
            IDivisible<Complex> divisible = (IDivisible<Complex>)complexA;
            Complex otherComplex = divisible.Divide(complexB);
            // Assert 
            Assert.IsTrue(otherComplex.Equals(result));
        }

        /// <summary>
        /// Tests the <see cref="IOneable{T}.One"/> method returning the multiplicative neutral element of the <see cref="Complex"/> numbers.
        /// </summary>
        [TestMethod("AsIOneable<Complex> One()")]
        public void AsIOneable_One()
        {
            // Arrange
            Complex complex = new Complex(1.5, 6.0);
            Complex result = new Complex(1.0, 0.0);
            //Act
            IOneable<Complex> oneable = (IOneable<Complex>)complex;
            Complex otherComplex = oneable.One();
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }

        #endregion

        #region Explicit IGroupAction<Complex,double>

        /// <summary>
        /// Tests the <see cref="IGroupAction{TValue, T}.Multiply(TValue)"/> method
        /// computing the scalar multiplication of the current <see cref="Complex"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        [TestMethod("AsIGroupAction<Double,Complex> Multiply(Double)")]
        public void AsIGroupAction_Multiply_Double()
        {
            // Arrange
            Complex complex = new Complex(1.0, 2.5);
            double number = 4.0;
            Complex result = new Complex(4.0, 10.0);
            //Act
            IGroupAction<double, Complex> groupActionable = (IGroupAction<double, Complex>)complex;
            Complex otherComplex = groupActionable.Multiply(number);
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }

        /// <summary>
        /// Tests the <see cref="IGroupAction{TValue,T}.Divide(TValue)"/> method
        /// computing the scalar division of the current <see cref="Complex"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        [TestMethod("AsIGroupAction<Double,Complex> Divide(Double)")]
        public void AsIGroupAction_Divide_Double()
        {
            // Arrange
            Complex complex = new Complex(1.0, 6.0);
            double number = 4.0;
            Complex result = new Complex(0.25, 1.5);
            //Act
            IGroupAction<double, Complex> groupActionable = (IGroupAction<double, Complex>)complex;
            Complex otherComplex = groupActionable.Divide(number);
            // Assert
            Assert.IsTrue(otherComplex.Equals(result));
        }

        #endregion
    }
}
