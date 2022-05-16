using System;

using Alg_Fund = BRIDGES.Algebra.Fundamentals;
using Alg_Str = BRIDGES.Algebra.Structures;


namespace BRIDGES.Arithmetic.Numbers
{
    /// <summary>
    /// Structure defining a complex number.
    /// </summary>
    public struct Complex
        : Alg_Str.Additive.IAbelianGroup<Complex>, Alg_Str.Multiplicative.IAbelianGroup<Complex>, Alg_Fund.IGroupAction<Complex, double>, 
          IEquatable<Complex>
    {
        #region Properties

        /******************** Cartesian Coordinates ********************/

        /// <summary>
        /// Gets the real component of the current <see cref="Complex"/> number.
        /// </summary>
        public double RealPart { get; private set; }

        /// <summary>
        /// Gets the imaginary component of the current <see cref="Complex"/> number.
        /// </summary>
        public double ImaginaryPart { get; private set; }

        /******************** Polar Coordinates ********************/

        /// <summary>
        /// Gets the modulus of the current <see cref="Complex"/> number.
        /// </summary>
        public double Modulus
        {
            get { return Math.Sqrt((RealPart * RealPart) + (ImaginaryPart * ImaginaryPart)); }
        }

        /// <summary>
        /// Gets the argument of the current <see cref="Complex"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"> The argument of the complex is not defined. The modulus must be none zero to access the argument of a complex. </exception>
        public double Argument
        {
            get
            {
                // Computes the norm (square of the modulus)
                if ((RealPart * RealPart) + (ImaginaryPart * ImaginaryPart) == 0)
                {
                    throw new InvalidOperationException("The argument of the complex is not defined. The modulus must be none zero to access the argument of a complex.");
                }
                else if (ImaginaryPart < 0) { return -Math.Acos(RealPart / Modulus); }
                else { return Math.Acos(RealPart / Modulus); }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of <see cref="Complex"/> structure by defining it real and imaginary components.
        /// </summary>
        /// <param name="real"> Value of the real component. </param>
        /// <param name="imaginary"> Value of the imaginary component. </param>
        public Complex(double real, double imaginary)
        {
            RealPart = real;
            ImaginaryPart = imaginary;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="Complex"/> structure from another <see cref="Complex"/> numbers.
        /// </summary>
        /// <param name="complex"> <see cref="Complex"/> number to copy. </param>
        public Complex(Complex complex)
        {
            RealPart = complex.RealPart;
            ImaginaryPart = complex.ImaginaryPart;
        }

        #endregion

        #region Static Members

        /// <summary>
        /// Gets a new instance of the <see cref="Complex"/> structure equal to the additive neutral element.<br/>
        /// It corresponds to the <see cref="Complex"/> with a real number equal to zero and an imaginary number equal to zero : (0.0, 0.0).
        /// </summary>
        /// <returns> The new <see cref="Complex"/> number equal to zero. </returns>
        public static Complex Zero() { return new Complex(0.0, 0.0); }

        /// <summary>
        /// Gets a new instance of the <see cref="Complex"/> structure equal to the multiplicative neutral element.<br/>
        /// It corresponds to the <see cref="Complex"/> with a real number equal to one and an imaginary number equal to zero : (1.0, 0.0).
        /// </summary>
        /// <returns> The new <see cref="Complex"/> number equal to one. </returns>
        public static Complex One() { return new Complex(1.0, 0.0); }

        /// <summary>
        /// Gets a new instance of the <see cref="Complex"/> structure equal to the unit imaginary element.<br/>
        /// It corresponds to the <see cref="Complex"/> with a real number equal to zero and an imaginary number equal to one : (0.0, 1.0).
        /// </summary>
        /// <returns> The new <see cref="Complex"/> number equal to imaginary one. </returns>
        public static Complex ImaginaryOne() => new Complex(0.0, 1.0);

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets a new <see cref="Complex"/> number by defining its polar coordinates.
        /// </summary>
        /// <param name="modulus"> Value of the modulus. </param>
        /// <param name="argument"> Value of the argument (in radians). </param>
        /// <returns> The new <see cref="Complex"/> number with the given polar coordinates. </returns>
        public static Complex FromPolarCoordinates(double modulus, double argument)
        {
            return new Complex(modulus * Math.Cos(argument), modulus * Math.Sin(argument));
        }


        /// <summary>
        /// Gets the conjugate value of a <see cref="Complex"/> number.
        /// </summary>
        /// <param name="complex"> <see cref="Complex"/> number to conjugate. </param>
        /// <returns> The new <see cref="Complex"/> number, conjugate of the initial one. </returns>
        public static Complex Conjugate(Complex complex) { return new Complex(complex.RealPart, -complex.ImaginaryPart); }


        /******************** Algebraic Field ********************/

        /// <inheritdoc cref="operator +(Complex, Complex)"/>
        public static Complex Add(Complex complexA, Complex complexB)
        { 
            return new Complex(complexA.RealPart + complexB.RealPart, complexA.ImaginaryPart + complexB.ImaginaryPart); 
        }

        /// <inheritdoc cref="operator -(Complex, Complex)"/>
        public static Complex Subtract(Complex complexA, Complex complexB)
        { 
            return new Complex(complexA.RealPart - complexB.RealPart, complexA.ImaginaryPart - complexB.ImaginaryPart); 
        }

        /// <summary>
        /// Computes the opposite of the given <see cref="Complex"/> number.
        /// </summary>
        /// <returns> The new <see cref="Complex"/> number, opposite of the initial one. </returns>
        public static Complex Opposite(Complex complex) { return new Complex(-complex.RealPart, -complex.ImaginaryPart); }


        /// <inheritdoc cref="operator *(Complex, Complex)"/>
        public static Complex Multiply(Complex complexA, Complex complexB) 
        {
            return new Complex((complexA.RealPart * complexB.RealPart) - (complexA.ImaginaryPart * complexB.ImaginaryPart),
                (complexA.RealPart * complexB.ImaginaryPart) + (complexA.ImaginaryPart * complexB.RealPart));
        }

        /// <inheritdoc cref="operator /(Complex, Complex)"/>
        public static Complex Divide(Complex complexA, Complex complexB)
        {
            double normB = ((complexB.RealPart * complexB.RealPart) + (complexB.ImaginaryPart * complexB.ImaginaryPart));

            return new Complex(((complexA.RealPart * complexB.RealPart) + (complexA.ImaginaryPart * complexB.ImaginaryPart)) / normB,
                ((complexA.ImaginaryPart * complexB.RealPart) - (complexA.RealPart * complexB.ImaginaryPart)) / normB);
        }

        /// <summary>
        /// Computes the inverse of the given <see cref="Complex"/> number.
        /// </summary>
        /// <returns> The new <see cref="Complex"/> number, inverse of the initial one. </returns>
        public static Complex Inverse(Complex complex)
        {
            double norm = (complex.RealPart * complex.RealPart) + (complex.ImaginaryPart * complex.ImaginaryPart);

            return new Complex(complex.RealPart / norm, -complex.ImaginaryPart / norm);
        }


        /******************** Real Embedding ********************/

        /// <inheritdoc cref="operator +(Complex, Real)"/>
        public static Complex Add(Complex complex, Real real) { return new Complex(complex.RealPart + real.Value, complex.ImaginaryPart); }

        /// <inheritdoc cref="operator +(Real, Complex)"/>
        public static Complex Add(Real real, Complex complex) { return new Complex(real.Value + complex.RealPart, complex.ImaginaryPart); }


        /// <inheritdoc cref="operator -(Complex, Real)"/>
        public static Complex Subtract(Complex complex, Real real) { return new Complex(complex.RealPart - real.Value, complex.ImaginaryPart); }

        /// <inheritdoc cref="operator -(Real, Complex)"/>
        public static Complex Subtract(Real real, Complex complex) { return new Complex(real.Value - complex.RealPart , -complex.ImaginaryPart); }


        /// <inheritdoc cref="operator *(Complex, Real)"/>
        public static Complex Multiply(Complex complex, Real real) { return new Complex(complex.RealPart * real.Value, complex.ImaginaryPart * real.Value); }

        /// <inheritdoc cref="operator *(Real, Complex)"/>
        public static Complex Multiply(Real real, Complex complex) { return new Complex(real.Value * complex.RealPart, real.Value * complex.ImaginaryPart); }


        /// <inheritdoc cref="operator /(Complex, Real)"/>
        public static Complex Divide(Complex complex, Real real) { return new Complex(complex.RealPart / real.Value, complex.ImaginaryPart / real.Value); }

        /// <inheritdoc cref="operator /(Real, Complex)"/>
        public static Complex Divide(Real real, Complex complex)
        {
            double norm = ((complex.RealPart * complex.RealPart) + (complex.ImaginaryPart * complex.ImaginaryPart));

            return new Complex(real.Value * (complex.RealPart / norm), real.Value * (-complex.ImaginaryPart / norm));
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Computes the scalar multiplication of a <see cref="Complex"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="factor"> <see cref="double"/>-precision real number. </param>
        /// <param name="complex"> <see cref="Complex"/> number to multiply. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the scalar multiplication. </returns>
        public static Complex Multiply(double factor, Complex complex) { return new Complex(factor * complex.RealPart, factor * complex.ImaginaryPart); }

        /// <summary>
        /// Computes the scalar division of a <see cref="Complex"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="complex"> <see cref="Complex"/> number to divide. </param>
        /// <param name="divisor"> <see cref="double"/>-precision real number to divide with. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the scalar division. </returns>
        public static Complex Divide(Complex complex, double divisor) { return new Complex(complex.RealPart / divisor, complex.ImaginaryPart / divisor); }

        #endregion

        #region Operators

        /******************** Algebraic Field ********************/

        /// <summary>
        /// Computes the addition of two <see cref="Complex"/> numbers.
        /// </summary>
        /// <param name="complexA"> <see cref="Complex"/> number for the addition. </param>
        /// <param name="complexB"> <see cref="Complex"/> number for the addition. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the addition. </returns>
        public static Complex operator +(Complex complexA, Complex complexB)
        {
            return new Complex(complexA.RealPart + complexB.RealPart, complexA.ImaginaryPart + complexB.ImaginaryPart);
        }

        /// <summary>
        /// Computes the subtraction of two <see cref="Complex"/> numbers.
        /// </summary>
        /// <param name="complexA"> <see cref="Complex"/> number to subtract. </param>
        /// <param name="complexB"> <see cref="Complex"/> number to subtract with. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the subtraction. </returns>
        public static Complex operator -(Complex complexA, Complex complexB)
        {
            return new Complex(complexA.RealPart - complexB.RealPart, complexA.ImaginaryPart - complexB.ImaginaryPart);
        }

        /// <summary>
        /// Computes the multiplication of two <see cref="Complex"/> numbers.
        /// </summary>
        /// <param name="complexA"> <see cref="Complex"/> number for the multiplication. </param>
        /// <param name="complexB"> <see cref="Complex"/> number for the multiplication. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the multiplication. </returns>
        public static Complex operator *(Complex complexA, Complex complexB)
        {
            return new Complex((complexA.RealPart * complexB.RealPart) - (complexA.ImaginaryPart * complexB.ImaginaryPart),
                (complexA.RealPart * complexB.ImaginaryPart) + (complexA.ImaginaryPart * complexB.RealPart));
        }

        /// <summary>
        /// Computes the division of two <see cref="Complex"/> numbers.
        /// </summary>
        /// <param name="complexA"> <see cref="Complex"/> number to divide. </param>
        /// <param name="complexB"> <see cref="Complex"/> number to divide with. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the division. </returns>
        public static Complex operator /(Complex complexA, Complex complexB)
        {
            double normB = ((complexB.RealPart * complexB.RealPart) + (complexB.ImaginaryPart * complexB.ImaginaryPart));

            return new Complex(((complexA.RealPart * complexB.RealPart) + (complexA.ImaginaryPart * complexB.ImaginaryPart)) / normB,
                ((complexA.ImaginaryPart * complexB.RealPart) - (complexA.RealPart * complexB.ImaginaryPart)) / normB);
        }


        /******************** Real Embedding ********************/

        /// <summary>
        /// Computes the addition of a <see cref="Complex"/> number with a <see cref="Real"/> number.
        /// </summary>
        /// <param name="complex"> <see cref="Complex"/> number for the addition. </param>
        /// <param name="real"> <see cref="Real"/> number for the addition. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the addition. </returns>
        public static Complex operator +(Complex complex, Real real) { return new Complex(complex.RealPart + real.Value, complex.ImaginaryPart); }

        /// <summary>
        /// Computes the addition of a <see cref="Real"/> number with a <see cref="Complex"/> number.
        /// </summary>
        /// <param name="real"> <see cref="Real"/> number for the addition. </param>
        /// <param name="complex"> <see cref="Complex"/> number for the addition. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the addition. </returns>
        public static Complex operator +(Real real, Complex complex) { return new Complex(real.Value + complex.RealPart, complex.ImaginaryPart); }


        /// <summary>
        /// Computes the subtraction of a <see cref="Complex"/> number with a <see cref="Real"/> number.
        /// </summary>
        /// <param name="complex"> <see cref="Complex"/> number to subtract. </param>
        /// <param name="real"> <see cref="Real"/> number to subtract with. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the subtraction. </returns>
        public static Complex operator -(Complex complex, Real real) { return new Complex(complex.RealPart - real.Value, complex.ImaginaryPart); }

        /// <summary>
        /// Computes the subtraction of a <see cref="Real"/> number with a <see cref="Complex"/> number.
        /// </summary>
        /// <param name="real"> <see cref="Real"/> number to subtract. </param>
        /// <param name="complex"> <see cref="Complex"/> number to subtract with. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the subtraction. </returns>
        public static Complex operator -(Real real, Complex complex) { return new Complex(real.Value - complex.RealPart, -complex.ImaginaryPart); }


        /// <summary>
        /// Computes the multiplication of a <see cref="Complex"/> number with a <see cref="Real"/> number.
        /// </summary>
        /// <param name="complex"> <see cref="Complex"/> number for the multiplicaion. </param>
        /// <param name="real"> <see cref="Real"/> number for the multiplicaion. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the multiplication. </returns>
        public static Complex operator *(Complex complex, Real real) { return new Complex(complex.RealPart * real.Value, complex.ImaginaryPart * real.Value); }

        /// <summary>
        /// Computes the multiplication of a <see cref="Real"/> number with a <see cref="Complex"/> number.
        /// </summary>
        /// <param name="real"> <see cref="Real"/> number for the multiplicaion. </param>
        /// <param name="complex"> <see cref="Complex"/> number for the multiplicaion. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the multiplication. </returns>
        public static Complex operator *(Real real, Complex complex) { return new Complex(real.Value * complex.RealPart, real.Value * complex.ImaginaryPart); }


        /// <summary>
        /// Computes the division of a <see cref="Complex"/> number with a <see cref="Real"/> number.
        /// </summary>
        /// <param name="complex"> <see cref="Complex"/> number to divide. </param>
        /// <param name="real"> <see cref="Real"/> number to divide with. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the division. </returns>
        public static Complex operator /(Complex complex, Real real) { return new Complex(complex.RealPart / real.Value, complex.ImaginaryPart / real.Value); }

        /// <summary>
        /// Computes the division of a <see cref="Real"/> number with a <see cref="Complex"/> number.
        /// </summary>
        /// <param name="real"> <see cref="Real"/> number to divide. </param>
        /// <param name="complex"> <see cref="Complex"/> number to divide with. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the division. </returns>
        public static Complex operator /(Real real, Complex complex)
        {
            double norm = ((complex.RealPart * complex.RealPart) + (complex.ImaginaryPart * complex.ImaginaryPart));

            return new Complex(real.Value * (complex.RealPart / norm), real.Value * (-complex.ImaginaryPart / norm));
        }


        /******************** double Embedding ********************/

        /// <summary>
        /// Computes the addition of a <see cref="Complex"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="complex"> <see cref="Complex"/> number for the addition. </param>
        /// <param name="number"> <see cref="double"/>-precision real number for the addition. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the addition. </returns>
        public static Complex operator +(Complex complex, double number) { return new Complex(complex.RealPart + number, complex.ImaginaryPart); }

        /// <summary>
        /// Computes the addition of a <see cref="double"/>-precision real number with a <see cref="Complex"/> number.
        /// </summary>
        /// <param name="number"> <see cref="double"/>-precision real number for the addition. </param>
        /// <param name="complex"> <see cref="Complex"/> number for the addition. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the addition. </returns>
        public static Complex operator +(double number, Complex complex) { return new Complex(number + complex.RealPart, complex.ImaginaryPart); }


        /// <summary>
        /// Computes the subtraction of a <see cref="Complex"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="complex"> <see cref="Complex"/> number to subtract. </param>
        /// <param name="number"> <see cref="double"/>-precision real number to subtract with. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the subtraction. </returns>
        public static Complex operator -(Complex complex, double number) { return new Complex(complex.RealPart - number, complex.ImaginaryPart); }

        /// <summary>
        /// Computes the subtraction of a <see cref="double"/>-precision real number with a <see cref="Complex"/> number.
        /// </summary>
        /// <param name="number"> <see cref="double"/>-precision real number to subtract. </param>
        /// <param name="complex"> <see cref="Complex"/> number to subtract with. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the subtraction. </returns>
        public static Complex operator -(double number, Complex complex) { return new Complex(number- complex.RealPart, -complex.ImaginaryPart); }


        /// <summary>
        /// Computes the multiplication of a <see cref="Complex"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="complex"> <see cref="Complex"/> number for the multiplicaion. </param>
        /// <param name="number"> <see cref="double"/>-precision real number for the multiplicaion. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the multiplication. </returns>
        public static Complex operator *(Complex complex, double number) { return new Complex(complex.RealPart * number, complex.ImaginaryPart * number); }

        /// <summary>
        /// Computes the multiplication of a <see cref="double"/>-precision real number with a <see cref="Complex"/> number.
        /// </summary>
        /// <param name="number"> <see cref="double"/>-precision real number for the multiplicaion. </param>
        /// <param name="complex"> <see cref="Complex"/> number for the multiplicaion. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the multiplication. </returns>
        public static Complex operator *(double number, Complex complex) { return new Complex(number * complex.RealPart, number * complex.ImaginaryPart); }


        /// <summary>
        /// Computes the division of a <see cref="Complex"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="complex"> <see cref="Complex"/> number to divide. </param>
        /// <param name="number"> <see cref="double"/>-precision real number to divide with. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the division. </returns>
        public static Complex operator /(Complex complex, double number) { return new Complex(complex.RealPart / number, complex.ImaginaryPart / number); }

        /// <summary>
        /// Computes the division of a <see cref="double"/>-precision real number with a <see cref="Complex"/> number.
        /// </summary>
        /// <param name="number"> <see cref="double"/>-precision real number to divide. </param>
        /// <param name="complex"> <see cref="Complex"/> number to divide with. </param>
        /// <returns> The new <see cref="Complex"/> number resulting from the division. </returns>
        public static Complex operator /(double number, Complex complex)
        {
            double norm = ((complex.RealPart * complex.RealPart) + (complex.ImaginaryPart * complex.ImaginaryPart));

            return new Complex(number * (complex.RealPart / norm), number * (-complex.ImaginaryPart / norm));
        }

        #endregion

        #region Casts
/*
        /// <summary>
        /// Casts a <see cref="Real"/> number into a <see cref="Complex"/> number.
        /// </summary>
        /// <param name="real"> <see cref="Real"/> number to cast. </param>
        /// <returns> The <see cref="Complex"/> number resulting from the cast. </returns>
        public static implicit operator Complex(Real real) { return new Complex(real.Value, 0.0); }

        /// <summary>
        /// Casts a <see cref="double"/>-precision real number into a <see cref="Complex"/> number.
        /// </summary>
        /// <param name="number"> <see cref="double"/>-precision real number to cast. </param>
        /// <returns> The <see cref="Complex"/> number resulting from the cast. </returns>
        public static implicit operator Complex(double number) { return new Complex(number, 0.0); }

        /// <summary>
        /// Casts a <see cref="ValueTuple{T1, T2}"/> into a <see cref="Complex"/> number.
        /// </summary>
        /// <param name="pair"> <see cref="ValueTuple{T1, T2}"/> to cast. </param>
        /// <returns> The <see cref="Complex"/> number resulting from the cast. </returns>
        public static implicit operator Complex(ValueTuple<double, double> pair) { return new Complex(pair.Item1, pair.Item2); }
*/
        #endregion

        #region Methods

        /// <summary>
        /// Computes the conjugate of the current <see cref="Complex"/> number.
        /// </summary>
        /// <returns> <see langword="true"/> if the current <see cref="Complex"/> number was conjugated, <see langword="false"/> otherwise. </returns>
        public bool Conjugate()
        {
            ImaginaryPart = -ImaginaryPart;
            return true;
        }

        /// <summary>
        /// Computes the opposite of the current <see cref="Complex"/> number.
        /// </summary>
        /// <returns> <see langword="true"/> if the current <see cref="Complex"/> number was opposed, <see langword="false"/> otherwise. </returns>
        public bool Opposite() 
        {
            RealPart = -RealPart;
            ImaginaryPart = -ImaginaryPart;
            return true;
        }

        /// <summary>
        /// Computes the inverse of the current <see cref="Complex"/> number.
        /// </summary>
        /// <returns> <see langword="true"/> if the current <see cref="Complex"/> number was inversed, <see langword="false"/> otherwise. </returns>
        public bool Inverse() 
        {
            double norm = (RealPart * RealPart) + (ImaginaryPart * ImaginaryPart);
            if (norm == 0.0) { return false; }
            else
            {
                RealPart /= norm;
                ImaginaryPart = -ImaginaryPart / norm;
                return true;
            }
        }


        /// <summary>
        /// Computes the norm of the current <see cref="Complex"/> number.
        /// </summary>
        /// <returns> The value of the norm. </returns>
        public double Norm() { return (RealPart * RealPart) + (ImaginaryPart * ImaginaryPart); }


        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public bool Equals(Complex other) 
        { 
            return (Math.Abs(RealPart - other.RealPart) < Settings.AbsolutePrecision && Math.Abs(ImaginaryPart - other.ImaginaryPart) < Settings.AbsolutePrecision); 
        }

        #endregion

        
        #region Override Object

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is Complex complex && Equals(complex);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"({RealPart},{ImaginaryPart})";
        }

        #endregion


        #region Explicit Additive.IAbelianGroup<Complex>

        /******************** Properties ********************/

        /// <inheritdoc/>
        bool Alg_Fund.IAddable<Complex>.IsAssociative => true;

        /// <inheritdoc/>
        bool Alg_Fund.IAddable<Complex>.IsCommutative => true;


        /******************** Methods ********************/

        /// <inheritdoc/>
        Complex Alg_Fund.IAddable<Complex>.Add(Complex other) { return new Complex(RealPart + other.RealPart, ImaginaryPart + other.ImaginaryPart); }

        /// <inheritdoc/>
        Complex Alg_Fund.ISubtractable<Complex>.Subtract(Complex other) { return new Complex(RealPart - other.RealPart, ImaginaryPart - other.ImaginaryPart); }

        /// <inheritdoc/>
        Complex Alg_Fund.IZeroable<Complex>.Zero() { return Complex.Zero(); }

        #endregion

        #region Explicit Multiplicative.IAbelianGroup<Complex>

        /******************** Properties ********************/

        /// <inheritdoc/>
        bool Alg_Fund.IMultiplicable<Complex>.IsAssociative => true;

        /// <inheritdoc/>
        bool Alg_Fund.IMultiplicable<Complex>.IsCommutative => true;


        /******************** Methods ********************/

        /// <inheritdoc/>
        Complex Alg_Fund.IMultiplicable<Complex>.Multiply(Complex other) { return Multiply(this, other); }

        /// <inheritdoc/>
        Complex Alg_Fund.IDivisible<Complex>.Divide(Complex other) { return Divide(this, other); }

        /// <inheritdoc/>
        Complex Alg_Fund.IOneable<Complex>.One() { return Complex.One(); }

        #endregion

        #region Explicit IGroupAction<Complex,double>

        /// <inheritdoc/>
        Complex Alg_Fund.IGroupAction<Complex, double>.Multiply(double factor) { return Multiply(factor, this); }

        /// <inheritdoc/>
        Complex Alg_Fund.IGroupAction<Complex, double>.Divide(double divisor) { return Divide(this, divisor); }

        #endregion

    }

}
