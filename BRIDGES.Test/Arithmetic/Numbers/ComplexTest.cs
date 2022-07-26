using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.Arithmetic.Numbers;
using BRIDGES.Algebra.Fundamentals;
using BRIDGES.Algebra.Sets;


namespace BRIDGES.Test.Arithmetic.Numbers
{
    /// <summary>
    /// Class testing the members of the <see cref="Complex"/> structure.
    /// </summary>
    [TestClass]
    public class ComplexTest
    {
        #region Behavior

        /// <summary>
        /// Tests that <see cref="Complex"/> is not reference type.
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
        /// Tests the initialisation of the <see cref="Complex"/> from its real and imaginary part,
        /// and the <see cref="Complex.RealPart"/>, <see cref="Complex.ImaginaryPart"/> properties.
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
        /// Tests the <see cref="Complex.Modulus"/> and <see cref="Complex.Argument"/> properties.
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
        /// Tests the static property <see cref="Complex.Zero"/>.
        /// </summary>
        [TestMethod("Static Zero")]
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
        /// Tests the static property <see cref="Complex.One"/>.
        /// </summary>
        [TestMethod("Static One")]
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
        /// Tests the static property <see cref="Complex.ImaginaryOne"/>.
        /// </summary>
        [TestMethod("Static ImaginaryOne")]
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
        /// Tests the static method <see cref="Complex.FromPolarCoordinates(double, double)"/>.
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
        /// Tests the static method <see cref="Complex.Conjugate(Complex)"/>.
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
        /// Tests the static method <see cref="Complex.Add(Complex, Complex)"/>.
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
        /// Tests the static method <see cref="Complex.Subtract(Complex, Complex)"/>.
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
        /// Tests the static method <see cref="Complex.Opposite(Complex)"/>.
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
        /// Tests the static method <see cref="Complex.Multiply(Complex, Complex)"/>.
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
        /// Tests the static method <see cref="Complex.Divide(Complex, Complex)"/>.
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
        /// Tests the static method <see cref="Complex.Inverse(Complex)"/>.
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
        /// Tests the static method <see cref="Complex.Add(Complex, Real)"/>.
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
        /// Tests the static method <see cref="Complex.Add(Real, Complex)"/>.
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
        /// Tests the static method <see cref="Complex.Subtract(Complex, Real)"/>.
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
        /// Tests the static method <see cref="Complex.Subtract(Real, Complex)"/>.
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
        /// Tests the static method <see cref="Complex.Multiply(Complex, Real)"/>.
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
        /// Tests the static method <see cref="Complex.Multiply(Real, Complex)"/>.
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
        /// Tests the static method <see cref="Complex.Divide(Complex, Real)"/>.
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
        /// Tests the static method <see cref="Complex.Divide(Real, Complex)"/>.
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
        /// Tests the static method <see cref="Complex.Multiply(double, Complex)"/>.
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
        /// Tests the static method <see cref="Complex.Divide(Complex, double)"/>.
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
        /// Tests the static operator <see cref="Complex.operator +(Complex,Complex)"/>.
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
        /// Tests the static operator <see cref="Complex.operator -(Complex,Complex)"/>.
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
        /// Tests the static operator <see cref="Complex.operator -(Complex)"/>.
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
        /// Tests the static operator <see cref="Complex.operator *(Complex,Complex)"/>.
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
        /// Tests the static operator <see cref="Complex.operator /(Complex,Complex)"/>.
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
        /// Tests the static operator <see cref="Complex.operator +(Complex,Real)"/>.
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
        /// Tests the static operator <see cref="Complex.operator +(Real,Complex)"/>.
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
        /// Tests the static operator <see cref="Complex.operator -(Complex,Real)"/>.
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
        /// Tests the static operator <see cref="Complex.operator -(Real,Complex)"/>.
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
        /// Tests the static operator <see cref="Complex.operator *(Complex,Real)"/>.
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
        /// Tests the static operator <see cref="Complex.operator *(Real,Complex)"/>.
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
        /// Tests the static operator <see cref="Complex.operator /(Complex,Real)"/>.
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
        /// Tests the static operator <see cref="Complex.operator /(Real,Complex)"/>.
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
        /// Tests the static operator <see cref="Complex.operator +(Complex,double)"/>.
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
        /// Tests the static operator <see cref="Complex.operator +(double,Complex)"/>.
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
        /// Tests the static operator <see cref="Complex.operator -(Complex,double)"/>.
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
        /// Tests the static operator <see cref="Complex.operator -(double,Complex)"/>.
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
        /// Tests the static operator <see cref="Complex.operator *(Complex,double)"/>.
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
        /// Tests the static operator <see cref="Complex.operator *(double,Complex)"/>.
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
        /// Tests the static operator <see cref="Complex.operator /(Complex,double)"/>.
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
        /// Tests the static operator <see cref="Complex.operator /(double,Complex)"/>.
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
                /// Tests the implicit cast of a <see cref="Real"/> into a <see cref="Complex"/>.
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
                /// Tests the implicit cast of a <see cref="double"/> into a <see cref="Complex"/>.
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
                /// Tests the implicit cast of a <see cref="ValueTuple{T1,T2}"/> into a <see cref="Complex"/>.
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
        /// Tests the method <see cref="Complex.Conjugate()"/>.
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
        /// Tests the method <see cref="Complex.Opposite()"/>.
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
        /// Tests the method <see cref="Complex.Inverse()"/>.
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
        /// Tests the method <see cref="Complex.Norm()"/>.
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
        /// Tests the method <see cref="Complex.Equals(Complex)"/>.
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



        #region Explicit : Additive.IAbelianGroup<Complex>

        /******************** Properties ********************/

        /// <summary>
        /// Tests the <see cref="IAddable{T}.IsAssociative"/> property of <see cref="Complex"/>.
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
        /// Tests the <see cref="IAddable{T}.IsCommutative"/>  property of <see cref="Complex"/>.
        /// </summary>
        [TestMethod("AsIAddable<Complex> Property IsCommutative")]
        public void AsIAddable_IsCommutative()
        {
            // Arrange
            Complex complex = new Complex(1.0, -5.0);
            // Act
            IAddable<Complex> addable = (IAddable<Complex>)complex;
            // Assert
            Assert.IsTrue(addable.IsCommutative);
        }


        /******************** Methods ********************/

        /// <summary>
        /// Tests the <see cref="IAddable{T}.Add(T)"/> method of <see cref="Complex"/>.
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
        /// Tests the <see cref="ISubtractable{T}.Subtract(T)"/> method of <see cref="Complex"/>.
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
        /// Tests the <see cref="IZeroable{T}.Zero"/> method of <see cref="Complex"/>.
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

        #region Explicit : Additive.IAbelianGroup<Complex>

        /******************** Properties ********************/

        /// <summary>
        /// Tests the <see cref="IMultiplicable{T}.IsAssociative"/> property of <see cref="Complex"/>.
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
        /// Tests the <see cref="IMultiplicable{T}.Multiply(T)"/> method of <see cref="Complex"/>.
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
        /// Tests the <see cref="IDivisible{T}.Divide(T)"/> method of <see cref="Complex"/>.
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
        /// Tests the <see cref="IOneable{T}.One"/> method of <see cref="Complex"/>.
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

        #region Explicit : IGroupAction<Double,Complex>

        /// <summary>
        /// Tests the <see cref="IGroupAction{TValue, T}.Multiply(TValue)"/> method of <see cref="Complex"/>.
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
        /// Tests the <see cref="IGroupAction{TValue,T}.Divide(TValue)"/> method of <see cref="Complex"/>.
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
