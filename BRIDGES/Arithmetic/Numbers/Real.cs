using System;

using Alg_Fund = BRIDGES.Algebra.Fundamentals;
using Alg_Str = BRIDGES.Algebra.Structures;


namespace BRIDGES.Arithmetic.Numbers
{
    /// <summary>
    /// Structure defining a real number.
    /// </summary>
    public struct Real
        : Alg_Str.Additive.IAbelianGroup<Real>, Alg_Str.Multiplicative.IAbelianGroup<Real>,
          IEquatable<Real>
    {
        #region Properties

        /// <summary>
        /// Value of the real numbers.
        /// </summary>
        public double Value { get; set; }

        #endregion

        #region Constructors
        
        /// <summary>
        /// Initialises a new instance of the <see cref="Real"/> structure by defining its value.
        /// </summary>
        /// <param name="number"> Value of the real number. </param>
        public Real(double number = 0.0)
        {
            Value = number;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Real"/> structure from another <see cref="Real"/> number.
        /// </summary>
        /// <param name="real"> <see cref="Real"/> number to copy. </param>
        public Real(Real real)
        {
            Value = real.Value;
        }

        #endregion

        #region Static Members

        /// <summary>
        /// Gets a new instance of the <see cref="Real"/> structure equal to zero.
        /// </summary>
        /// <returns> The new instance <see cref="Real"/> number equal to zero. </returns>
        public static Real Zero() { return new Real(0.0); }

        /// <summary>
        /// Gets a new instance of the <see cref="Real"/> structure equal to one.
        /// </summary>
        /// <returns> The new <see cref="Real"/> number equal to one. </returns>
        public static Real One() { return new Real(1.0); }

        #endregion

        #region Static Methods

        /******************** Algebraic Field ********************/

        /// <inheritdoc cref="operator +(Real, Real)"/>
        public static Real Add(Real realA, Real realB) { return new Real(realA.Value + realB.Value); }

        /// <inheritdoc cref="operator -(Real, Real)"/>
        public static Real Subtract(Real realA, Real realB) { return new Real(realA.Value - realB.Value); }

        /// <summary>
        /// Computes the opposite of the given <see cref="Real"/> number.
        /// </summary>
        /// <returns> The new <see cref="Real"/> number, opposite of the initial one. </returns>
        public static Real Opposite(Real real) { return new Real(-real.Value); }


        /// <inheritdoc cref="operator *(Real, Real)"/>
        public static Real Multiply(Real realA, Real realB) { return new Real(realA.Value * realB.Value); }

        /// <inheritdoc cref="operator /(Real, Real)"/>
        public static Real Divide(Real realA, Real realB) { return new Real(realA.Value / realB.Value); }

        /// <summary>
        /// Computes the inverse of the given <see cref="Real"/> number.
        /// </summary>
        /// <returns> The new <see cref="Real"/> number, inverse of the initial one. </returns>
        public static Real Inverse(Real real) { return new Real( 1/real.Value); }

        #endregion

        #region Operators

        /******************** Algebraic Field ********************/

        /// <summary>
        /// Computes the addition of two <see cref="Real"/> numbers.
        /// </summary>
        /// <param name="realA"> <see cref="Real"/> number for the addition. </param>
        /// <param name="realB"> <see cref="Real"/> number for the addition. </param>
        /// <returns> The new <see cref="Real"/> number resulting from the addition. </returns>
        public static Real operator +(Real realA, Real realB) { return new Real(realA.Value + realB.Value); }

        /// <summary>
        /// Computes the subtraction of two <see cref="Real"/> numbers.
        /// </summary>
        /// <param name="realA"> <see cref="Real"/> number to subtract. </param>
        /// <param name="realB"> <see cref="Real"/> number to subtract with. </param>
        /// <returns> The new <see cref="Real"/> number resulting from the subtraction. </returns>
        public static Real operator -(Real realA, Real realB) { return new Real(realA.Value - realB.Value); }

        /// <summary>
        /// Computes the multiplication of two <see cref="Real"/> numbers.
        /// </summary>
        /// <param name="realA"> <see cref="Real"/> number for the multiplication. </param>
        /// <param name="realB"> <see cref="Real"/> number for the multiplication. </param>
        /// <returns> The new <see cref="Real"/> number resulting from the multiplication. </returns>
        public static Real operator *(Real realA, Real realB) { return new Real(realA.Value * realB.Value); }

        /// <summary>
        /// Computes the division of two <see cref="Real"/> numbers.
        /// </summary>
        /// <param name="realA"> <see cref="Real"/> number to divide. </param>
        /// <param name="realB"> <see cref="Real"/> number to divide with. </param>
        /// <returns> The new <see cref="Real"/> number resulting from the division. </returns>
        public static Real operator /(Real realA, Real realB) { return new Real(realA.Value / realB.Value); }


        /******************** double Embedding ********************/

        /// <summary>
        /// Computes the addition of a <see cref="Real"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="real"> <see cref="Real"/> number for the addition. </param>
        /// <param name="number"> <see cref="double"/>-precision real number for the addition. </param>
        /// <returns> The new <see cref="Real"/> number resulting from the addition. </returns>
        public static Real operator +(Real real, double number) { return new Real(real.Value + number); }

        /// <summary>
        /// Computes the addition of a <see cref="double"/>-precision real number with a <see cref="Real"/> number.
        /// </summary>
        /// <param name="number"> <see cref="double"/>-precision real number for the addition. </param>
        /// <param name="real"> <see cref="Real"/> number for the addition. </param>
        /// <returns> The new <see cref="Real"/> number resulting from the addition. </returns>
        public static Real operator +(double number, Real real) { return new Real(number + real.Value); }


        /// <summary>
        /// Computes the subtraction of a <see cref="Real"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="real"> <see cref="Real"/> number to subtract. </param>
        /// <param name="number"> <see cref="double"/>-precision real number to subtract with. </param>
        /// <returns> The new <see cref="Real"/> number resulting from the subtraction. </returns>
        public static Real operator -(Real real, double number) { return new Real(real.Value - number); }

        /// <summary>
        /// Computes the subtraction of a <see cref="double"/>-precision real number with a <see cref="Real"/> number.
        /// </summary>
        /// <param name="number"> <see cref="double"/>-precision real number to subtract. </param>
        /// <param name="real"> <see cref="Real"/> number to subtract with. </param>
        /// <returns> The new <see cref="Real"/> number resulting from the subtraction. </returns>
        public static Real operator -(double number, Real real) { return new Real(number - real.Value); }


        /// <summary>
        /// Computes the multiplication of a <see cref="Real"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="real"> <see cref="Real"/> number for the multiplicaion. </param>
        /// <param name="number"> <see cref="double"/>-precision real number for the multiplicaion. </param>
        /// <returns> The new <see cref="Real"/> number resulting from the multiplication. </returns>
        public static Real operator *(Real real, double number) { return new Real(real.Value * number); }

        /// <summary>
        /// Computes the multiplication of a <see cref="double"/>-precision real number with a <see cref="Real"/> number.
        /// </summary>
        /// <param name="number"> <see cref="double"/>-precision real number for the multiplicaion. </param>
        /// <param name="real"> <see cref="Real"/> number for the multiplicaion. </param>
        /// <returns> The new <see cref="Real"/> number resulting from the multiplication. </returns>
        public static Real operator *(double number, Real real) { return new Real(number * real.Value); }


        /// <summary>
        /// Computes the division of a <see cref="Real"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="real"> <see cref="Real"/> number to divide. </param>
        /// <param name="number"> <see cref="double"/>-precision real number to divide with. </param>
        /// <returns> The new <see cref="Real"/> number resulting from the division. </returns>
        public static Real operator /(Real real, double number) { return new Real(real.Value / number); }

        /// <summary>
        /// Computes the division of a <see cref="double"/>-precision real number with a <see cref="Real"/> number.
        /// </summary>
        /// <param name="number"> <see cref="double"/>-precision real number to divide. </param>
        /// <param name="real"> <see cref="Real"/> number to divide with. </param>
        /// <returns> The new <see cref="Real"/> number resulting from the division. </returns>
        public static Real operator /(double number, Real real) { return new Real(number / real.Value); }

        #endregion

        #region Cast
/*
        /// <summary>
        /// Casts a <see cref="Real"/> number into a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="real"> <see cref="Real"/> number to cast. </param>
        /// <returns> The <see cref="double"/>-precision real number resulting from the cast. </returns>
        public static implicit operator double(Real real) { return real.Value; }

        /// <summary>
        /// Casts a <see cref="double"/>-precision real number into a <see cref="Real"/> number.
        /// </summary>
        /// <param name="number"> <see cref="double"/>-precision real number to cast. </param>
        /// <returns> The <see cref="Real"/> number resulting from the cast. </returns>
        public static implicit operator Real(double number) { return new Real(number); }
*/
        #endregion

        #region Methods

        /// <summary>
        /// Computes the opposite of the current <see cref="Real"/> number.
        /// </summary>
        /// <returns> <see langword="true"/> if the current <see cref="Real"/> number was opposed, <see langword="false"/> otherwise. </returns>
        public bool Opposite()
        {
            Value = -Value;
            return true;
        }

        /// <summary>
        /// Computes the inverse of the current <see cref="Real"/> number.
        /// </summary>
        /// <returns> <see langword="true"/> if the current <see cref="Real"/> number was inversed, <see langword="false"/> otherwise. </returns>
        public bool Inverse()
        {
            if (Value == 0.0) { return false; }
            else
            {
                Value = 1 / Value;
                return true;
            }
        }


        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public bool Equals(Real other) { return Math.Abs(Value - other.Value) < Settings.AbsolutePrecision; }

        #endregion


        #region Override Object

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is Real real && Equals(real);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{Value}";
        }

        #endregion


        #region Explicit Additive.IAbelianGroup<Real>

        /******************** Properties ********************/

        /// <inheritdoc/>
        bool Alg_Fund.IAddable<Real>.IsAssociative => true;

        /// <inheritdoc/>
        bool Alg_Fund.IAddable<Real>.IsCommutative => true;


        /******************** Methods ********************/

        /// <inheritdoc/>
        Real Alg_Fund.IAddable<Real>.Add(Real other) { return new Real(Value + other.Value); }

        /// <inheritdoc/>
        Real Alg_Fund.ISubtractable<Real>.Subtract(Real other) { return new Real(Value - other.Value); }

        /// <inheritdoc/>
        Real Alg_Fund.IZeroable<Real>.Zero() { return Real.Zero(); }

        #endregion

        #region Explicit Multiplicative.IAbelianGroup<Real>

        /******************** Properties ********************/

        /// <inheritdoc/>
        bool Alg_Fund.IMultiplicable<Real>.IsAssociative => true;

        /// <inheritdoc/>
        bool Alg_Fund.IMultiplicable<Real>.IsCommutative => true;


        /******************** Methods ********************/

        /// <inheritdoc/>
        Real Alg_Fund.IMultiplicable<Real>.Multiply(Real other) { return new Real(Value * other.Value); }

        /// <inheritdoc/>
        Real Alg_Fund.IDivisible<Real>.Divide(Real other) { return new Real(Value / other.Value); }

        /// <inheritdoc/>
        Real Alg_Fund.IOneable<Real>.One() { return Real.One(); }

        #endregion
    }
}
