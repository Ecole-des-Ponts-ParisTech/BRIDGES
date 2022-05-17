using System;

using Alg_Fund = BRIDGES.Algebra.Fundamentals;
using Alg_Str = BRIDGES.Algebra.Structures;


namespace BRIDGES.Arithmetic.Numbers
{
    /// <summary>
    /// Structure defining quaternion number.
    /// </summary>
    public struct Quaternion
        : Alg_Str.Additive.IAbelianGroup<Quaternion>, Alg_Str.Multiplicative.IAbelianGroup<Quaternion>, Alg_Fund.IGroupAction<Quaternion, double>,
          IEquatable<Quaternion>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the scalar part of the current <see cref="Quaternion"/> number.
        /// </summary>
        public double ScalarPart { get; set; }

        /// <summary>
        /// Gets or sets the first component "i" of the vector part of the current <see cref="Quaternion"/> number.
        /// </summary>
        public double I { get; set; }

        /// <summary>
        /// Gets or sets the second component "j" of the vector part of the current <see cref="Quaternion"/> number.
        /// </summary>
        public double J { get; set; }

        /// <summary>
        /// Gets or sets the third component "k" of the vector part of the current <see cref="Quaternion"/> number.
        /// </summary>
        public double K { get; set; }

        /// <summary>
        /// Gets the vector part of the current <see cref="Quaternion"/> number.
        /// </summary>
        public double[] VectorPart
        {
            get { return new double[3] { I, J, K }; }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of <see cref="Quaternion"/> structure by defining its real and imaginary components.
        /// </summary>
        /// <param name="r"> Value of the first component. </param>
        /// <param name="i"> Value of the second component. </param>
        /// <param name="j"> Value of the third component. </param>
        /// <param name="k"> Value of the fourth component. </param>
        public Quaternion(double r, double i, double j, double k)
        {
            ScalarPart = r; I = i; J = j; K = k;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="Quaternion"/> structure by defining its components.
        /// </summary>
        /// <param name="components"> Value of the components. </param>
        public Quaternion(double[] components)
        {
            if(components.Length != 4) { throw new ArgumentException("The length of the components array is different from four, the dimension of Quaternions."); }
            
            ScalarPart = components[0]; 
            I = components[1]; 
            J = components[2]; 
            K = components[3];
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Quaternion"/> structure from another <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="quaternion"> <see cref="Quaternion"/> number to copy. </param>
        public Quaternion(Quaternion quaternion)
        {
            ScalarPart = quaternion.ScalarPart;
            I = quaternion.I;
            J = quaternion.J;
            K = quaternion.K;
        }

        #endregion

        #region Static Members

        /// <summary>
        /// Gets a new instance of the <see cref="Quaternion"/> structure equal to the additive neutral element.<br/>
        /// It corresponds to the <see cref="Quaternion"/> with a real number equal to zero and an imaginary part equal to zero : (0.0, 0.0, 0.0, 0.0).
        /// </summary>
        /// <returns> The new <see cref="Quaternion"/> number equal to zero. </returns>
        public static Quaternion Zero() { return new Quaternion(0.0, 0.0, 0.0, 0.0); }

        /// <summary>
        /// Gets a new instance of the <see cref="Quaternion"/> structure equal to the multiplicative neutral element.<br/>
        /// It corresponds to the <see cref="Quaternion"/> with a real number equal to one and an imaginary number equal to zero : (1.0, 0.0, 0.0, 0.0).
        /// </summary>
        /// <returns> The new <see cref="Quaternion"/> number equal to one. </returns>
        public static Quaternion One() { return new Quaternion(1.0, 0.0, 0.0, 0.0); }

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets the conjugate value of a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="quaternion"> <see cref="Quaternion"/> number to conjugate. </param>
        /// <returns> The new <see cref="Quaternion"/> number, conjugate of the initial one. </returns>
        public static Quaternion Conjugate(Quaternion quaternion) 
        { 
            return new Quaternion(quaternion.ScalarPart, -quaternion.I, -quaternion.J, -quaternion.K); 
        }


        /******************** Algebraic Field ********************/

        /// <inheritdoc cref="operator +(Quaternion, Quaternion)"/>
        public static Quaternion Add(Quaternion quaternionA, Quaternion quaternionB)
        { 
            return new Quaternion(quaternionA.ScalarPart + quaternionB.ScalarPart, quaternionA.I + quaternionB.I, quaternionA.J + quaternionB.J, quaternionA.K + quaternionB.K); 
        }

        /// <inheritdoc cref="operator -(Quaternion, Quaternion)"/>
        public static Quaternion Subtract(Quaternion quaternionA, Quaternion quaternionB)
        {
            return new Quaternion(quaternionA.ScalarPart - quaternionB.ScalarPart, quaternionA.I - quaternionB.I, quaternionA.J - quaternionB.J, quaternionA.K - quaternionB.K);
        }

        /// <summary>
        /// Computes the opposite of the given <see cref="Quaternion"/> number.
        /// </summary>
        /// <returns> The new <see cref="Quaternion"/> number, opposite of the initial one. </returns>
        public static Quaternion Opposite(Quaternion quaternion) 
        { 
            return new Quaternion(-quaternion.ScalarPart, -quaternion.I, -quaternion.J, -quaternion.K); 
        }


        /// <inheritdoc cref="operator *(Quaternion, Quaternion)"/>
        public static Quaternion Multiply(Quaternion quaternionA, Quaternion quaternionB) 
        {
            return new Quaternion((quaternionA.ScalarPart * quaternionB.ScalarPart) - (quaternionA.I * quaternionB.I) - (quaternionA.J * quaternionB.J) - (quaternionA.K * quaternionB.K),
                (quaternionA.ScalarPart * quaternionB.I) + (quaternionA.I * quaternionB.ScalarPart) + (quaternionA.J * quaternionB.K) - (quaternionA.K * quaternionB.J),
                (quaternionA.ScalarPart * quaternionB.J) - (quaternionA.I * quaternionB.K) + (quaternionA.J * quaternionB.ScalarPart) + (quaternionA.K * quaternionB.I),
                (quaternionA.ScalarPart * quaternionB.K) + (quaternionA.I * quaternionB.J) - (quaternionA.J * quaternionB.I) + (quaternionA.K * quaternionB.ScalarPart));
        }

        /// <inheritdoc cref="operator /(Quaternion, Quaternion)"/>
        public static Quaternion Divide(Quaternion quaternionA, Quaternion quaternionB)
        {
            double norm = (quaternionB.ScalarPart * quaternionB.ScalarPart) + (quaternionB.I * quaternionB.I) + (quaternionB.J * quaternionB.J) + (quaternionB.K * quaternionB.K);

            return new Quaternion(((quaternionA.ScalarPart * quaternionB.ScalarPart) + (quaternionA.I * quaternionB.I) + (quaternionA.J * quaternionB.J) + (quaternionA.K * quaternionB.K)) / norm,
                (-(quaternionA.ScalarPart * quaternionB.I) + (quaternionA.I * quaternionB.ScalarPart) - (quaternionA.J * quaternionB.K) + (quaternionA.K * quaternionB.J)) / norm,
                (-(quaternionA.ScalarPart * quaternionB.J) + (quaternionA.I * quaternionB.K) + (quaternionA.J * quaternionB.ScalarPart) - (quaternionA.K * quaternionB.I)) / norm,
                (-(quaternionA.ScalarPart * quaternionB.K) - (quaternionA.I * quaternionB.J) + (quaternionA.J * quaternionB.I) + (quaternionA.K * quaternionB.ScalarPart)) / norm);
        }

        /// <summary>
        /// Computes the inverse of the current <see cref="Quaternion"/> number.
        /// </summary>
        /// <returns> The new <see cref="Quaternion"/> number, inverse of the initial one. </returns>
        public static Quaternion Inverse(Quaternion quaternion)
        {
            double norm = (quaternion.ScalarPart * quaternion.ScalarPart) + (quaternion.I * quaternion.I) + (quaternion.J * quaternion.J) + (quaternion.K * quaternion.K);

            return new Quaternion(quaternion.ScalarPart / norm, -quaternion.I / norm, -quaternion.J / norm, -quaternion.K / norm);
        }


        /******************** Complex Embedding ********************/

        /// <inheritdoc cref="operator +(Quaternion, Complex)"/>
        public static Quaternion Add(Quaternion quaternion, Complex complex) 
        { 
            return new Quaternion(quaternion.ScalarPart + complex.RealPart, quaternion.I + complex.ImaginaryPart, quaternion.J, quaternion.K); 
        }

        /// <inheritdoc cref="operator +(Complex, Quaternion)"/>
        public static Quaternion Add(Complex complex, Quaternion quaternion)
        {
            return new Quaternion(complex.RealPart + quaternion.ScalarPart, complex.ImaginaryPart + quaternion.I, quaternion.J, quaternion.K);
        }


        /// <inheritdoc cref="operator -(Quaternion, Complex)"/>
        public static Quaternion Subtract(Quaternion quaternion, Complex complex)
        {
            return new Quaternion(quaternion.ScalarPart - complex.RealPart, quaternion.I - complex.ImaginaryPart, quaternion.J, quaternion.K);
        }

        /// <inheritdoc cref="operator -(Complex, Quaternion)"/>
        public static Quaternion Subtract(Complex complex, Quaternion quaternion)
        {
            return new Quaternion(complex.RealPart - quaternion.ScalarPart, complex.ImaginaryPart - quaternion.I, -quaternion.J, -quaternion.K);
        }


        /// <inheritdoc cref="operator *(Quaternion, Complex)"/>
        public static Quaternion Multiply(Quaternion quaternion, Complex complex)
        {
            return new Quaternion((quaternion.ScalarPart * complex.RealPart) - (quaternion.I * complex.ImaginaryPart),
                (quaternion.ScalarPart * complex.ImaginaryPart) + (quaternion.I * complex.RealPart),
                (quaternion.J * complex.RealPart) + (quaternion.K * complex.ImaginaryPart),
                - (quaternion.J * complex.ImaginaryPart) + (quaternion.K * complex.RealPart));
        }

        /// <inheritdoc cref="operator *(Complex, Quaternion)"/>
        public static Quaternion Multiply(Complex complex, Quaternion quaternion)
        {
            return new Quaternion((complex.RealPart * quaternion.ScalarPart) - (complex.ImaginaryPart * quaternion.I),
                (complex.ImaginaryPart * quaternion.ScalarPart) + (complex.RealPart * quaternion.I),
                (complex.RealPart * quaternion.J) + (complex.ImaginaryPart * quaternion.K),
                -(complex.ImaginaryPart * quaternion.J) + (complex.RealPart * quaternion.K));
        }


        /// <inheritdoc cref="operator /(Quaternion, Complex)"/>
        public static Quaternion Divide(Quaternion quaternion, Complex complex)
        {
            double norm = (complex.RealPart * complex.RealPart) + (complex.ImaginaryPart * complex.ImaginaryPart);

            return new Quaternion(((quaternion.ScalarPart * complex.RealPart) + (quaternion.I * complex.ImaginaryPart)) / norm,
                (-(quaternion.ScalarPart * complex.ImaginaryPart) + (quaternion.I * complex.RealPart)) / norm,
                ((quaternion.J * complex.RealPart) - (quaternion.K * complex.ImaginaryPart)) / norm,
                ((quaternion.J * complex.ImaginaryPart) + (quaternion.K * complex.RealPart)) / norm);
        }

        /// <inheritdoc cref="operator /(Complex, Quaternion)"/>
        public static Quaternion Divide(Complex complex, Quaternion quaternion)
        {
            double norm = (quaternion.ScalarPart * quaternion.ScalarPart) + (quaternion.I * quaternion.I) + (quaternion.J * quaternion.J) + (quaternion.K * quaternion.K);

            return new Quaternion(
                ((complex.RealPart * quaternion.ScalarPart) + (complex.ImaginaryPart * quaternion.I)) / norm,
                (-(complex.RealPart * quaternion.I) + (complex.ImaginaryPart * quaternion.ScalarPart)) / norm,
                (-(complex.RealPart * quaternion.J) + (complex.ImaginaryPart * quaternion.K)) / norm,
                (-(complex.RealPart * quaternion.K) - (complex.ImaginaryPart * quaternion.J)) / norm);
        }


        /******************** Real Embedding ********************/

        /// <inheritdoc cref="operator +(Quaternion, Real)"/>
        public static Quaternion Add(Quaternion quaternion, Real real)
        { 
            return new Quaternion(quaternion.ScalarPart + real.Value, quaternion.I, quaternion.J, quaternion.K); 
        }

        /// <inheritdoc cref="operator +(Real, Quaternion)"/>
        public static Quaternion Add(Real real, Quaternion quaternion)
        {
            return new Quaternion(real.Value + quaternion.ScalarPart, quaternion.I, quaternion.J, quaternion.K);
        }


        /// <inheritdoc cref="operator -(Quaternion, Real)"/>
        public static Quaternion Subtract(Quaternion quaternion, Real real)
        {
            return new Quaternion(quaternion.ScalarPart - real.Value, quaternion.I, quaternion.J, quaternion.K);
        }

        /// <inheritdoc cref="operator -(Real, Quaternion)"/>
        public static Quaternion Subtract(Real real, Quaternion quaternion)
        {
            return new Quaternion(real.Value - quaternion.ScalarPart, -quaternion.I, -quaternion.J, -quaternion.K);
        }


        /// <inheritdoc cref="operator *(Quaternion, Real)"/>
        public static Quaternion Multiply(Quaternion quaternion, Real real)
        {
            return new Quaternion(quaternion.ScalarPart * real.Value, quaternion.I * real.Value, quaternion.J * real.Value, quaternion.K * real.Value);
        }

        /// <inheritdoc cref="operator *(Real, Quaternion)"/>
        public static Quaternion Multiply(Real real, Quaternion quaternion)
        {
            return new Quaternion(real.Value * quaternion.ScalarPart, real.Value * quaternion.I, real.Value * quaternion.J, real.Value * quaternion.K);
        }


        /// <inheritdoc cref="operator /(Quaternion, Real)"/>
        public static Quaternion Divide(Quaternion quaternion, Real real)
        {
            return new Quaternion(quaternion.ScalarPart / real.Value, quaternion.I / real.Value, quaternion.J / real.Value, quaternion.K / real.Value);
        }

        /// <inheritdoc cref="operator /(Real, Quaternion)"/>
        public static Quaternion Divide(Real real, Quaternion quaternion)
        {
            double norm = (quaternion.ScalarPart * quaternion.ScalarPart) + (quaternion.I * quaternion.I) + (quaternion.J * quaternion.J) + (quaternion.K * quaternion.K);

            return new Quaternion(real.Value * quaternion.ScalarPart / norm, real.Value * (-quaternion.I) / norm, real.Value * (-quaternion.J) / norm, real.Value * (-quaternion.K) / norm);
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Computes the scalar multiplication of a <see cref="Quaternion"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="factor"> <see cref="double"/>-precision real number. </param>
        /// <param name="quaternion"> <see cref="Quaternion"/> number to multiply. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the scalar multiplication. </returns>
        public static Quaternion Multiply(double factor, Quaternion quaternion)
        {
            return new Quaternion(factor * quaternion.ScalarPart, factor * quaternion.I, factor * quaternion.J, factor * quaternion.K);
        }

        /// <summary>
        /// Computes the scalar division of a <see cref="Quaternion"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="quaternion"> <see cref="Quaternion"/> number to divide. </param>
        /// <param name="divisor"> <see cref="double"/>-precision real number to divide with. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the scalar division. </returns>
        public static Quaternion Divide(Quaternion quaternion, double divisor)
        {
            return new Quaternion(quaternion.ScalarPart / divisor, quaternion.I / divisor, quaternion.J / divisor, quaternion.K / divisor);
        }

        #endregion

        #region Operators

        /******************** Algebraic Field ********************/

        /// <summary>
        /// Computes the addition of two <see cref="Quaternion"/> numbers.
        /// </summary>
        /// <param name="quaternionA"> <see cref="Quaternion"/> number for the addition. </param>
        /// <param name="quaternionB"> <see cref="Quaternion"/> number for the addition. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the addition. </returns>
        public static Quaternion operator +(Quaternion quaternionA, Quaternion quaternionB)
        {
            return new Quaternion(quaternionA.ScalarPart + quaternionB.ScalarPart, quaternionA.I + quaternionB.I, quaternionA.J + quaternionB.J, quaternionA.K + quaternionB.K);
        }

        /// <summary>
        /// Computes the subtraction of two <see cref="Quaternion"/> numbers.
        /// </summary>
        /// <param name="quaternionA"> <see cref="Quaternion"/> number to subtract. </param>
        /// <param name="quaternionB"> <see cref="Quaternion"/> number to subtract with. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the subtraction. </returns>
        public static Quaternion operator -(Quaternion quaternionA, Quaternion quaternionB)
        {
            return new Quaternion(quaternionA.ScalarPart - quaternionB.ScalarPart, quaternionA.I - quaternionB.I, quaternionA.J - quaternionB.J, quaternionA.K - quaternionB.K);
        }


        /// <summary>
        /// Computes the multiplication of two <see cref="Quaternion"/> numbers.
        /// </summary>
        /// <param name="quaternionA"> <see cref="Quaternion"/> number for the multiplication. </param>
        /// <param name="quaternionB"> <see cref="Quaternion"/> number for the multiplication. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the multiplication. </returns>
        public static Quaternion operator *(Quaternion quaternionA, Quaternion quaternionB)
        {
            return new Quaternion((quaternionA.ScalarPart * quaternionB.ScalarPart) - (quaternionA.I * quaternionB.I) - (quaternionA.J * quaternionB.J) - (quaternionA.K * quaternionB.K),
                (quaternionA.ScalarPart * quaternionB.I) + (quaternionA.I * quaternionB.ScalarPart) + (quaternionA.J * quaternionB.K) - (quaternionA.K * quaternionB.J),
                (quaternionA.ScalarPart * quaternionB.J) - (quaternionA.I * quaternionB.K) + (quaternionA.J * quaternionB.ScalarPart) + (quaternionA.K * quaternionB.I),
                (quaternionA.ScalarPart * quaternionB.K) + (quaternionA.I * quaternionB.J) - (quaternionA.J * quaternionB.I) + (quaternionA.K * quaternionB.ScalarPart));
        }

        /// <summary>
        /// Computes the division of two <see cref="Quaternion"/> numbers.
        /// </summary>
        /// <param name="quaternionA"> <see cref="Quaternion"/> number to divide. </param>
        /// <param name="quaternionB"> <see cref="Quaternion"/> number to divide with. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the division. </returns>
        public static Quaternion operator /(Quaternion quaternionA, Quaternion quaternionB)
        {
            double norm = (quaternionB.ScalarPart * quaternionB.ScalarPart) + (quaternionB.I * quaternionB.I) + (quaternionB.J * quaternionB.J) + (quaternionB.K * quaternionB.K);

            return new Quaternion(((quaternionA.ScalarPart * quaternionB.ScalarPart) + (quaternionA.I * quaternionB.I) + (quaternionA.J * quaternionB.J) + (quaternionA.K * quaternionB.K)) / norm,
                (-(quaternionA.ScalarPart * quaternionB.I) + (quaternionA.I * quaternionB.ScalarPart) - (quaternionA.J * quaternionB.K) + (quaternionA.K * quaternionB.J)) / norm,
                (-(quaternionA.ScalarPart * quaternionB.J) + (quaternionA.I * quaternionB.K) + (quaternionA.J * quaternionB.ScalarPart) - (quaternionA.K * quaternionB.I)) / norm,
                (-(quaternionA.ScalarPart * quaternionB.K) - (quaternionA.I * quaternionB.J) + (quaternionA.J * quaternionB.I) + (quaternionA.K * quaternionB.ScalarPart)) / norm);
        }

        /******************** Complex Embedding ********************/

        /// <summary>
        /// Computes the addition of a <see cref="Quaternion"/> number with a <see cref="Complex"/> number.
        /// </summary>
        /// <param name="quaternion"> <see cref="Quaternion"/> number for the addition. </param>
        /// <param name="complex"> <see cref="Complex"/> number for the addition. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the addition. </returns>
        public static Quaternion operator +(Quaternion quaternion, Complex complex)
        {
            return new Quaternion(quaternion.ScalarPart + complex.RealPart, quaternion.I + complex.ImaginaryPart, quaternion.J, quaternion.K);
        }

        /// <summary>
        /// Computes the addition of a <see cref="Complex"/> number with a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="complex"> <see cref="Complex"/> number for the addition. </param>
        /// <param name="quaternion"> <see cref="Quaternion"/> number for the addition. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the addition. </returns>
        public static Quaternion operator +(Complex complex, Quaternion quaternion)
        {
            return new Quaternion(complex.RealPart + quaternion.ScalarPart, complex.ImaginaryPart + quaternion.I, quaternion.J, quaternion.K);
        }


        /// <summary>
        /// Computes the subtraction of a <see cref="Quaternion"/> number with a <see cref="Complex"/> number.
        /// </summary>
        /// <param name="quaternion"> <see cref="Quaternion"/> number to subtract. </param>
        /// <param name="complex"> <see cref="Complex"/> number to subtract with. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the subtraction. </returns>
        public static Quaternion operator -(Quaternion quaternion, Complex complex)
        {
            return new Quaternion(quaternion.ScalarPart - complex.RealPart, quaternion.I - complex.ImaginaryPart, quaternion.J, quaternion.K);
        }

        /// <summary>
        /// Computes the subtraction of a <see cref="Complex"/> number with a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="complex"> <see cref="Complex"/> number to subtract. </param>
        /// <param name="quaternion"> <see cref="Quaternion"/> number to subtract with. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the subtraction. </returns>
        public static Quaternion operator -(Complex complex, Quaternion quaternion)
        {
            return new Quaternion(complex.RealPart - quaternion.ScalarPart, complex.ImaginaryPart - quaternion.I, -quaternion.J, -quaternion.K);
        }


        /// <summary>
        /// Computes the multiplication of a <see cref="Quaternion"/> number with a <see cref="Complex"/> number.
        /// </summary>
        /// <param name="quaternion"> <see cref="Quaternion"/> number for the multiplicaion. </param>
        /// <param name="complex"> <see cref="Complex"/> number for the multiplicaion. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the multiplication. </returns>
        public static Quaternion operator *(Quaternion quaternion, Complex complex)
        {
            return new Quaternion((quaternion.ScalarPart * complex.RealPart) - (quaternion.I * complex.ImaginaryPart),
                (quaternion.ScalarPart * complex.ImaginaryPart) + (quaternion.I * complex.RealPart),
                (quaternion.J * complex.RealPart) + (quaternion.K * complex.ImaginaryPart),
                -(quaternion.J * complex.ImaginaryPart) + (quaternion.K * complex.RealPart));
        }

        /// <summary>
        /// Computes the multiplication of a <see cref="Complex"/> number with a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="complex"> <see cref="Complex"/> number for the multiplicaion. </param>
        /// <param name="quaternion"> <see cref="Quaternion"/> number for the multiplicaion. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the multiplication. </returns>
        public static Quaternion operator *(Complex complex, Quaternion quaternion)
        {
            return new Quaternion((complex.RealPart * quaternion.ScalarPart) - (complex.ImaginaryPart * quaternion.I),
                (complex.ImaginaryPart * quaternion.ScalarPart) + (complex.RealPart * quaternion.I),
                (complex.RealPart * quaternion.J) + (complex.ImaginaryPart * quaternion.K),
                -(complex.ImaginaryPart * quaternion.J) + (complex.RealPart * quaternion.K));
        }


        /// <summary>
        /// Computes the division of a <see cref="Quaternion"/> number with a <see cref="Complex"/> number.
        /// </summary>
        /// <param name="quaternion"> <see cref="Quaternion"/> number to divide. </param>
        /// <param name="complex"> <see cref="Complex"/> number to divide with. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the division. </returns>
        public static Quaternion operator /(Quaternion quaternion, Complex complex)
        {
            double norm = (complex.RealPart * complex.RealPart) + (complex.ImaginaryPart * complex.ImaginaryPart);

            return new Quaternion(((quaternion.ScalarPart * complex.RealPart) + (quaternion.I * complex.ImaginaryPart)) / norm,
                (-(quaternion.ScalarPart * complex.ImaginaryPart) + (quaternion.I * complex.RealPart)) / norm,
                ((quaternion.J * complex.RealPart) - (quaternion.K * complex.ImaginaryPart)) / norm,
                ((quaternion.J * complex.ImaginaryPart) + (quaternion.K * complex.RealPart)) / norm);
        }

        /// <summary>
        /// Computes the division of a <see cref="Complex"/> number with a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="complex"> <see cref="Complex"/> number to divide. </param>
        /// <param name="quaternion"> <see cref="Quaternion"/> number to divide with. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the division. </returns>
        public static Quaternion operator /(Complex complex, Quaternion quaternion)
        {
            double norm = (quaternion.ScalarPart * quaternion.ScalarPart) + (quaternion.I * quaternion.I) + (quaternion.J * quaternion.J) + (quaternion.K * quaternion.K);

            return new Quaternion(
                ((complex.RealPart * quaternion.ScalarPart) + (complex.ImaginaryPart * quaternion.I)) / norm,
                (-(complex.RealPart * quaternion.I) + (complex.ImaginaryPart * quaternion.ScalarPart)) / norm,
                (-(complex.RealPart * quaternion.J) + (complex.ImaginaryPart * quaternion.K)) / norm,
                (-(complex.RealPart * quaternion.K) - (complex.ImaginaryPart * quaternion.J)) / norm);
        }

        /******************** Real Embedding ********************/

        /// <summary>
        /// Computes the addition of a <see cref="Quaternion"/> number with a <see cref="Real"/> number.
        /// </summary>
        /// <param name="quaternion"> <see cref="Quaternion"/> number for the addition. </param>
        /// <param name="real"> <see cref="Real"/> number for the addition. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the addition. </returns>
        public static Quaternion operator +(Quaternion quaternion, Real real)
        {
            return new Quaternion(quaternion.ScalarPart + real.Value, quaternion.I, quaternion.J, quaternion.K);
        }

        /// <summary>
        /// Computes the addition of a <see cref="Real"/> number with a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="real"> <see cref="Real"/> number for the addition. </param>
        /// <param name="quaternion"> <see cref="Quaternion"/> number for the addition. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the addition. </returns>
        public static Quaternion operator +(Real real, Quaternion quaternion)
        {
            return new Quaternion(real.Value + quaternion.ScalarPart, quaternion.I, quaternion.J, quaternion.K);
        }


        /// <summary>
        /// Computes the subtraction of a <see cref="Quaternion"/> number with a <see cref="Real"/> number.
        /// </summary>
        /// <param name="quaternion"> <see cref="Quaternion"/> number to subtract. </param>
        /// <param name="real"> <see cref="Real"/> number to subtract with. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the subtraction. </returns>
        public static Quaternion operator -(Quaternion quaternion, Real real)
        {
            return new Quaternion(quaternion.ScalarPart - real.Value, quaternion.I, quaternion.J, quaternion.K);
        }

        /// <summary>
        /// Computes the subtraction of a <see cref="Real"/> number with a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="real"> <see cref="Real"/> number to subtract. </param>
        /// <param name="quaternion"> <see cref="Quaternion"/> number to subtract with. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the subtraction. </returns>
        public static Quaternion operator -(Real real, Quaternion quaternion)
        {
            return new Quaternion(real.Value - quaternion.ScalarPart, -quaternion.I, -quaternion.J, -quaternion.K);
        }


        /// <summary>
        /// Computes the multiplication of a <see cref="Quaternion"/> number with a <see cref="Real"/> number.
        /// </summary>
        /// <param name="quaternion"> <see cref="Quaternion"/> number for the multiplicaion. </param>
        /// <param name="real"> <see cref="Real"/> number for the multiplicaion. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the multiplication. </returns>
        public static Quaternion operator *(Quaternion quaternion, Real real)
        {
            return new Quaternion(quaternion.ScalarPart * real.Value, quaternion.I * real.Value, quaternion.J * real.Value, quaternion.K * real.Value);
        }

        /// <summary>
        /// Computes the multiplication of a <see cref="Real"/> number with a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="real"> <see cref="Real"/> number for the multiplicaion. </param>
        /// <param name="quaternion"> <see cref="Quaternion"/> number for the multiplicaion. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the multiplication. </returns>
        public static Quaternion operator *(Real real, Quaternion quaternion)
        {
            return new Quaternion(real.Value * quaternion.ScalarPart, real.Value * quaternion.I, real.Value * quaternion.J, real.Value * quaternion.K);
        }


        /// <summary>
        /// Computes the division of a <see cref="Quaternion"/> number with a <see cref="Real"/> number.
        /// </summary>
        /// <param name="quaternion"> <see cref="Quaternion"/> number to divide. </param>
        /// <param name="real"> <see cref="Real"/> number to divide with. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the division. </returns>
        public static Quaternion operator /(Quaternion quaternion, Real real)
        {
            return new Quaternion(quaternion.ScalarPart / real.Value, quaternion.I / real.Value, quaternion.J / real.Value, quaternion.K / real.Value);
        }

        /// <summary>
        /// Computes the division of a <see cref="Real"/> number with a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="real"> <see cref="Real"/> number to divide. </param>
        /// <param name="quaternion"> <see cref="Quaternion"/> number to divide with. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the division. </returns>
        public static Quaternion operator /(Real real, Quaternion quaternion)
        {
            double norm = (quaternion.ScalarPart * quaternion.ScalarPart) + (quaternion.I * quaternion.I) + (quaternion.J * quaternion.J) + (quaternion.K * quaternion.K);

            return new Quaternion(real.Value * quaternion.ScalarPart / norm, real.Value * (-quaternion.I) / norm, real.Value * (-quaternion.J) / norm, real.Value * (-quaternion.K) / norm);
        }

        /******************** Real Embedding ********************/

        /// <summary>
        /// Computes the addition of a <see cref="Quaternion"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="quaternion"> <see cref="Quaternion"/> number for the addition. </param>
        /// <param name="number"> <see cref="double"/>-precision real number for the addition. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the addition. </returns>
        public static Quaternion operator +(Quaternion quaternion, double number)
        {
            return new Quaternion(quaternion.ScalarPart + number, quaternion.I, quaternion.J, quaternion.K);
        }

        /// <summary>
        /// Computes the addition of a <see cref="double"/>-precision real number with a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="number"> <see cref="double"/>-precision real number for the addition. </param>
        /// <param name="quaternion"> <see cref="Quaternion"/> number for the addition. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the addition. </returns>
        public static Quaternion operator +(double number, Quaternion quaternion)
        {
            return new Quaternion(number + quaternion.ScalarPart, quaternion.I, quaternion.J, quaternion.K);
        }
        

        /// <summary>
        /// Computes the subtraction of a <see cref="Quaternion"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="quaternion"> <see cref="Quaternion"/> number to subtract. </param>
        /// <param name="number"> <see cref="double"/>-precision real number to subtract with. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the subtraction. </returns>
        public static Quaternion operator -(Quaternion quaternion, double number)
        {
            return new Quaternion(quaternion.ScalarPart - number, quaternion.I, quaternion.J, quaternion.K);
        }

        /// <summary>
        /// Computes the subtraction of a <see cref="Real"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="number"> <see cref="double"/>-precision real number to subtract. </param>
        /// <param name="quaternion"> <see cref="Quaternion"/> number to subtract with. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the subtraction. </returns>
        public static Quaternion operator -(double number, Quaternion quaternion)
        {
            return new Quaternion(number - quaternion.ScalarPart, -quaternion.I, -quaternion.J, -quaternion.K);
        }


        /// <summary>
        /// Computes the multiplication of a <see cref="Quaternion"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="quaternion"> <see cref="Quaternion"/> number for the multiplicaion. </param>
        /// <param name="number"> <see cref="double"/>-precision real number for the multiplicaion. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the multiplication. </returns>
        public static Quaternion operator *(Quaternion quaternion, double number)
        {
            return new Quaternion(quaternion.ScalarPart * number, quaternion.I * number, quaternion.J * number, quaternion.K * number);
        }

        /// <summary>
        /// Computes the multiplication of a <see cref="double"/>-precision real number with a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="number"> <see cref="double"/>-precision real number for the multiplicaion. </param>
        /// <param name="quaternion"> <see cref="Quaternion"/> number for the multiplicaion. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the multiplication. </returns>
        public static Quaternion operator *(double number, Quaternion quaternion)
        {
            return new Quaternion(number * quaternion.ScalarPart, number * quaternion.I, number * quaternion.J, number * quaternion.K);
        }


        /// <summary>
        /// Computes the division of a <see cref="Quaternion"/> number with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="quaternion"> <see cref="Quaternion"/> number to divide. </param>
        /// <param name="number"> <see cref="double"/>-precision real number to divide with. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the division. </returns>
        public static Quaternion operator /(Quaternion quaternion, double number)
        {
            return new Quaternion(quaternion.ScalarPart / number, quaternion.I / number, quaternion.J / number, quaternion.K / number);
        }

        /// <summary>
        /// Computes the division of a <see cref="double"/>-precision real number with a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="number"> <see cref="double"/>-precision real number to divide. </param>
        /// <param name="quaternion"> <see cref="Quaternion"/> number to divide with. </param>
        /// <returns> The new <see cref="Quaternion"/> number resulting from the division. </returns>
        public static Quaternion operator /(double number, Quaternion quaternion)
        {
            double norm = (quaternion.ScalarPart * quaternion.ScalarPart) + (quaternion.I * quaternion.I) + (quaternion.J * quaternion.J) + (quaternion.K * quaternion.K);

            return new Quaternion(number * quaternion.ScalarPart / norm, number * (-quaternion.I) / norm, number * (-quaternion.J) / norm, number * (-quaternion.K) / norm);
        }

        #endregion

        #region Casts
/*
        /// <summary>
        /// Casts a <see cref="Complex"/> number into a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="complex"> <see cref="Complex"/> number to cast. </param>
        /// <returns> The <see cref="Quaternion"/> number resulting from the cast. </returns>
        public static implicit operator Quaternion(Complex complex) { return new Quaternion(complex.RealPart, complex.ImaginaryPart, 0.0, 0.0); }

        /// <summary>
        /// Casts a <see cref="Real"/> number into a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="real"> <see cref="Real"/> number to cast. </param>
        /// <returns> The <see cref="Quaternion"/> number resulting from the cast. </returns>
        public static implicit operator Quaternion(Real real) { return new Quaternion(real.Value, 0.0, 0.0, 0.0); }

        /// <summary>
        /// Casts a <see cref="double"/>-precision real number into a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="number"> <see cref="double"/>-precision real number to cast. </param>
        /// <returns> The <see cref="Quaternion"/> number resulting from the cast. </returns>
        public static implicit operator Quaternion(double number) { return new Quaternion(number, 0.0, 0.0, 0.0); }

        /// <summary>
        /// Casts a <see cref="ValueTuple{T1, T2, T3, T4}"/> into a <see cref="Quaternion"/> number.
        /// </summary>
        /// <param name="quadruple"> <see cref="ValueTuple{T1, T2, T3, T4}"/> to cast. </param>
        /// <returns> The <see cref="Quaternion"/> number resulting from the cast. </returns>
        public static implicit operator Quaternion(ValueTuple<double, double, double, double> quadruple) { return new Quaternion(quadruple.Item1, quadruple.Item2, quadruple.Item3, quadruple.Item4); }
*/
        #endregion

        #region Methods

        /// <summary>
        /// Computes the conjugate of the current <see cref="Quaternion"/> number.
        /// </summary>
        /// <returns> <see langword="true"/> if the current <see cref="Quaternion"/> number was conjugated, <see langword="false"/> otherwise. </returns>
        public bool Conjugate()
        {
            I = -I; J = -J; K = -K;

            return true;
        }

        /// <summary>
        /// Computes the opposite of the current <see cref="Quaternion"/> number.
        /// </summary>
        /// <returns> <see langword="true"/> if the current <see cref="Quaternion"/> number was opposed, <see langword="false"/> otherwise. </returns>
        public bool Opposite()
        {
            ScalarPart = -ScalarPart;
            I = -I; J = -J; K = -K;

            return true;
        }

        /// <summary>
        /// Computes the inverse of the current <see cref="Quaternion"/> number.
        /// </summary>
        /// <returns> <see langword="true"/> if the current <see cref="Quaternion"/> number was inversed, <see langword="false"/> otherwise. </returns>
        public bool Inverse()
        {
            double norm = (ScalarPart * ScalarPart) + (I * I) + (J * J) + (K * K);

            if (norm == 0.0) { return false; }
            else
            {
                ScalarPart /= norm;
                I = -I / norm;
                J = -J / norm;
                K = -K / norm;

                return true;
            }
        }


        /// <summary>
        /// Computes the norm of the current <see cref="Quaternion"/> number.
        /// </summary>
        /// <returns> The value of the norm. </returns>
        public double Norm() 
        { 
            return (ScalarPart * ScalarPart) + (I * I) + (J * J) + (K * K); 
        }


        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public bool Equals(Quaternion other)
        {
            return (Math.Abs(ScalarPart - other.ScalarPart) < Settings.AbsolutePrecision 
                && Math.Abs(I - other.I) < Settings.AbsolutePrecision
                && Math.Abs(J - other.J) < Settings.AbsolutePrecision 
                && Math.Abs(K - other.K) < Settings.AbsolutePrecision);
        }

        #endregion


        #region Override Object

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is Quaternion quaternion && Equals(quaternion);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"({ScalarPart},{I},{J},{K})";
        }

        #endregion


        #region Explicit Additive.IAbelianGroup<Quaternion>

        /******************** Properties ********************/

        /// <inheritdoc/>
        bool Alg_Fund.IAddable<Quaternion>.IsAssociative => true;

        /// <inheritdoc/>
        bool Alg_Fund.IAddable<Quaternion>.IsCommutative => true;


        /******************** Methods ********************/

        /// <inheritdoc/>
        Quaternion Alg_Fund.IAddable<Quaternion>.Add(Quaternion other)
        {
            return new Quaternion(ScalarPart + other.ScalarPart, I + other.I, J + other.J, K + other.K);
        }

        /// <inheritdoc/>
        Quaternion Alg_Fund.ISubtractable<Quaternion>.Subtract(Quaternion other)
        {
            return new Quaternion(ScalarPart - other.ScalarPart, I - other.I, J - other.J, K - other.K);
        }

        /// <inheritdoc/>
        Quaternion Alg_Fund.IZeroable<Quaternion>.Zero() { return Quaternion.Zero(); }

        #endregion

        #region Explicit Multiplicative.IAbelianGroup<Quaternion>

        /******************** Properties ********************/

        /// <inheritdoc/>
        bool Alg_Fund.IMultiplicable<Quaternion>.IsAssociative => true;

        /// <inheritdoc/>
        bool Alg_Fund.IMultiplicable<Quaternion>.IsCommutative => false;


        /******************** Methods ********************/

        /// <inheritdoc/>
        Quaternion Alg_Fund.IMultiplicable<Quaternion>.Multiply(Quaternion other) { return Multiply(this, other); }

        /// <inheritdoc/>
        Quaternion Alg_Fund.IDivisible<Quaternion>.Divide(Quaternion other) { return Divide(this, other); }

        /// <inheritdoc/>
        Quaternion Alg_Fund.IOneable<Quaternion>.One() { return Quaternion.One(); }

        #endregion

        #region Explicit IGroupAction<Quaternion,double>

        /// <inheritdoc/>
        Quaternion Alg_Fund.IGroupAction<Quaternion, double>.Multiply(double factor) { return Multiply(factor, this); }

        /// <inheritdoc/>
        Quaternion Alg_Fund.IGroupAction<Quaternion, double>.Divide(double divisor) { return Divide(this, divisor); }

        #endregion
    }
}
