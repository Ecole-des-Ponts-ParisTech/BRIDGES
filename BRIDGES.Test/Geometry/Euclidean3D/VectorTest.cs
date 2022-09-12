using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.Geometry.Euclidean3D;
using Alg_Fund = BRIDGES.Algebra.Fundamentals;
using Alg_Str = BRIDGES.Algebra.Sets;
using Alg_Mes = BRIDGES.Algebra.Measure;


namespace BRIDGES.Test.Geometry.Euclidean3D
{
    /// <summary>
    /// Class testing the members of the <see cref="Vector"/> structure.
    /// </summary>
    [TestClass]
    public class VectorTest
    {
        #region Behavior

        /// <summary>
        /// Tests that <see cref="Vector"/> is not reference type.
        /// </summary>
        [TestMethod("Behavior IsNotReference")]
        public void IsNotReference()
        {
            // Arrange
            Vector vectorA = new Vector(1.0, 2.0, 3.0);
            Vector vectorB = new Vector(4.0, 5.0, 6.0);
            //Act
            vectorA = vectorB;
            // Assert
            Assert.IsTrue(vectorA.Equals(vectorB));
            Assert.AreNotSame(vectorA, vectorB);
        }

        #endregion


        #region Properties

        /// <summary>
        /// Tests the <see cref="Vector.Dimension"/> property.
        /// </summary>
        [TestMethod("Property Dimension")]
        public void Dimension()
        {
            // Arrange
            Vector vector = new Vector(1.5, 1.7, 2.1);

            //Act
            int dimension = vector.Dimension;

            // Assert
            Assert.AreEqual(3, dimension);
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="Vector"/> from its three coordinates,
        /// and the <see cref="Vector.X"/>, <see cref="Vector.Y"/>, <see cref="Vector.Z"/> properties.
        /// </summary>
        [TestMethod("Property X & Y & Z")]
        public void XAndYAndZ()
        {
            // Arrange
            Vector vector = new Vector(1.5, 1.7, 2.1);

            //Act
            double x = vector.X;
            double y = vector.Y;
            double z = vector.Z;

            // Assert
            Assert.AreEqual(x, 1.5, Settings.AbsolutePrecision);
            Assert.AreEqual(y, 1.7, Settings.AbsolutePrecision);
            Assert.AreEqual(z, 2.1, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the <see cref="Vector"/> indexer property.
        /// </summary>
        [TestMethod("Property this[int]")]
        public void Indexer()
        {
            // Arrange
            Vector vector = new Vector(1.5, 1.7, 2.1);

            //Act
            double x = vector[0];
            double y = vector[1];
            double z = vector[2];

            // Assert
            Assert.AreEqual(x, 1.5, Settings.AbsolutePrecision);
            Assert.AreEqual(y, 1.7, Settings.AbsolutePrecision);
            Assert.AreEqual(z, 2.1, Settings.AbsolutePrecision);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Tests the initialisation of the <see cref="Vector"/> from its cooridnates.
        /// </summary>
        [TestMethod("Constructor(Double[])")]
        public void Constructor_DoubleArray()
        {
            // Arrange
            Vector result = new Vector(2.0, 5.0, 8.0);
            bool throwsException = false;
            // Act
            Vector vector = new Vector(new double[3] { 2.0, 5.0, 8.0 });
            try { Vector otherVector = new Vector(new double[] { 2.0, 5.0, 8.0, 11.0 }); }
            catch (ArgumentOutOfRangeException) { throwsException = true; }
            // Assert
            Assert.IsTrue(vector.Equals(result));
            Assert.IsTrue(throwsException);
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="Vector"/> from another <see cref="Vector"/>.
        /// </summary>
        [TestMethod("Constructor(Vector)")]
        public void Constructor_Vector()
        {
            // Arrange
            Vector result = new Vector(2.0, 5.0, 8.0);
            // Act
            Vector vector = new Vector(result);
            // Assert
            Assert.IsTrue(vector.Equals(result));
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="Vector"/> from a start and end <see cref="Point"/>.
        /// </summary>
        [TestMethod("Constructor(Point,Point)")]
        public void Constructor_Point_Point()
        {
            // Arrange
            Point start = new Point(2.0, 4.0, 6.0);
            Point end = new Point(4.0, 9.0, 14.0);
            Vector result = new Vector(2.0, 5.0, 8.0);
            // Act
            Vector vector = new Vector(start, end);
            // Assert
            Assert.IsTrue(vector.Equals(result));
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Tests the static property <see cref="Vector.Zero"/>.
        /// </summary>
        [TestMethod("Static Zero")]
        public void Static_Zero()
        {
            // Arrange
            Vector result = new Vector(0.0, 0.0, 0.0);
            // Act
            Vector vector = Vector.Zero;
            // Assert
            Assert.IsTrue(vector.Equals(result));
        }


        /// <summary>
        /// Tests the static property <see cref="Vector.WorldX"/>.
        /// </summary>
        [TestMethod("Static WorldX")]
        public void Static_WorldX()
        {
            // Arrange
            Vector result = new Vector(1.0, 0.0, 0.0);
            // Act
            Vector vector = Vector.WorldX;
            // Assert
            Assert.IsTrue(vector.Equals(result));
        }

        /// <summary>
        /// Tests the static property <see cref="Vector.WorldY"/>.
        /// </summary>
        [TestMethod("Static WorldY")]
        public void Static_WorldY()
        {
            // Arrange
            Vector result = new Vector(0.0, 1.0, 0.0);
            // Act
            Vector vector = Vector.WorldY;
            // Assert
            Assert.IsTrue(vector.Equals(result));
        }

        /// <summary>
        /// Tests the static property <see cref="Vector.WorldZ"/>.
        /// </summary>
        [TestMethod("Static WorldZ")]
        public void Static_WorldZ()
        {
            // Arrange
            Vector result = new Vector(0.0, 0.0, 1.0);
            // Act
            Vector vector = Vector.WorldZ;
            // Assert
            Assert.IsTrue(vector.Equals(result));
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Tests the static method <see cref="Vector.CrossProduct(Vector, Vector)"/>.
        /// </summary>
        [TestMethod("Static CrossProduct(Vector,Vector)")]
        public void Static_CrossProduct_Vector_Vector()
        {
            // Arrange
            Vector vectorA = new Vector(1.0, 2.0, 3.0);
            Vector vectorB = new Vector(1.5, 3.5, 5.0);
            Vector result = new Vector(-0.5, -0.5, 0.5);
            //Act
            Vector otherVector = Vector.CrossProduct(vectorA, vectorB);
            // Assert
            Assert.IsTrue(otherVector.Equals(result));
        }


        /// <summary>
        /// Tests the static method <see cref="Vector.AreParallel(Vector, Vector)"/>.
        /// </summary>
        [TestMethod("Static AreParallel(Vector,Vector)")]
        public void Static_AreParallel_Vector_Vector()
        {
            // Arrange
            Vector vectorA = new Vector(1.0, 2.0, 3.0);
            Vector vectorB = new Vector(1.5, 3.5, 5.0);
            Vector vectorC = new Vector(1.5, 3.0, 4.5);
            //Act
            bool parallelityAB = Vector.AreParallel(vectorA, vectorB);
            bool parallelityAC = Vector.AreParallel(vectorA, vectorC);
            // Assert
            Assert.IsFalse(parallelityAB);
            Assert.IsTrue(parallelityAC);
        }

        /// <summary>
        /// Tests the static method <see cref="Vector.AreOrthogonal(Vector, Vector)"/>.
        /// </summary>
        [TestMethod("Static AreOrthogonal(Vector,Vector)")]
        public void Static_AreOrthogonal_Vector_Vector()
        {
            // Arrange
            Vector vectorA = new Vector(1.0, 2.0, 3.0);
            Vector vectorB = new Vector(1.5, 3.5, 5.0);
            Vector vectorC = new Vector(4.0, -2.0, 0.0);
            //Act
            bool orthogonalityAB = Vector.AreOrthogonal(vectorA, vectorB);
            bool orthogonalityAC = Vector.AreOrthogonal(vectorA, vectorC);
            // Assert
            Assert.IsFalse(orthogonalityAB);
            Assert.IsTrue(orthogonalityAC);
        }


        /******************** Algebraic Additive Group ********************/

        /// <summary>
        /// Tests the static method <see cref="Vector.Add(Vector, Vector)"/>.
        /// </summary>
        [TestMethod("Static Add(Vector,Vector)")]
        public void Static_Add_Vector_Vector()
        {
            // Arrange
            Vector vectorA = new Vector(1.5, 2.5, 3.5);
            Vector vectorB = new Vector(1.2, 2.4, 3.6);
            Vector result = new Vector(2.7, 4.9, 7.1);
            //Act
            Vector otherVector = Vector.Add(vectorA, vectorB);
            // Assert
            Assert.IsTrue(otherVector.Equals(result));
        }

        /// <summary>
        /// Tests the static method <see cref="Vector.Subtract(Vector, Vector)"/>.
        /// </summary>
        [TestMethod("Static Subtract(Vector,Vector)")]
        public void Static_Substract_Vector_Vector()
        {
            // Arrange
            Vector vectorA = new Vector(1.5, 2.5, 3.5);
            Vector vectorB = new Vector(1.2, 2.4, 3.6);
            Vector result = new Vector(0.3, 0.1, -0.1);
            //Act
            Vector otherVector = Vector.Subtract(vectorA, vectorB);
            // Assert
            Assert.IsTrue(otherVector.Equals(result));
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Tests the static method <see cref="Vector.Multiply(double, Vector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(Double,Vector)")]
        public void Static_Multiply_Double_Vector()
        {
            // Arrange
            double number = 1.5;
            Vector vector = new Vector(2.2, -4.6, 9.2);
            Vector result = new Vector(3.3, -6.9, 13.8);
            //Act
            Vector otherVector = Vector.Multiply(number, vector);
            // Assert
            Assert.IsTrue(otherVector.Equals(result));
        }

        /// <summary>
        /// Tests the static method <see cref="Vector.Divide(Vector, double)"/>.
        /// </summary>
        [TestMethod("Static Divide(Vector,Double)")]
        public void Static_Divide_Vector_Double()
        {
            // Arrange
            Vector vector = new Vector(2.4, -4.6, 9.2);
            double number = 2.0;
            Vector result = new Vector(1.2, -2.3, 4.6);
            //Act
            Vector otherVector = Vector.Divide(vector, number);
            // Assert
            Assert.IsTrue(otherVector.Equals(result));
        }


        /******************** Hilbert Space ********************/

        /// <summary>
        /// Tests the static method <see cref="Vector.DotProduct(Vector, Vector)"/>.
        /// </summary>
        [TestMethod("Static DotProduct(Vector,Vector)")]
        public void Static_DotProduct_Vector_Vector()
        {
            // Arrange
            Vector vectorA = new Vector(2.2, -4.6, 9.2);
            Vector vectorB = new Vector(1.4, -2.6, -5.2);
            double result = -32.8;
            //Act
            double dotProduct = Vector.DotProduct(vectorA, vectorB);
            // Assert
            Assert.AreEqual(dotProduct, result, Settings.AbsolutePrecision);
        }

        #endregion

        #region Static Operator

        /******************** Algebraic Field ********************/

        /// <summary>
        /// Tests the static operator <see cref="Vector.operator +(Vector, Vector)"/>.
        /// </summary>
        [TestMethod("Operator Add(Vector,Vector)")]
        public void Operator_Add_Vector_Vector()
        {
            // Arrange
            Vector vectorA = new Vector(1.5, 2.5, 3.5);
            Vector vectorB = new Vector(1.2, 2.4, 3.6);
            Vector result = new Vector(2.7, 4.9, 7.1);
            //Act
            Vector otherVector = vectorA + vectorB;
            // Assert
            Assert.IsTrue(otherVector.Equals(result));
        }

        /// <summary>
        /// Tests the static operator <see cref="Vector.operator -(Vector, Vector)"/>.
        /// </summary>
        [TestMethod("Operator Subtract(Vector,Vector)")]
        public void Operator_Subtract_Vector_Vector()
        {
            // Arrange
            Vector vectorA = new Vector(1.5, 2.5, 3.5);
            Vector vectorB = new Vector(1.2, 2.4, 3.6);
            Vector result = new Vector(0.3, 0.1, -0.1);
            //Act
            Vector otherVector = vectorA - vectorB;
            // Assert
            Assert.IsTrue(otherVector.Equals(result));
        }

        /// <summary>
        /// Tests the static operator <see cref="Vector.operator -(Vector)"/>.
        /// </summary>
        [TestMethod("Operator Subtract(Vector)")]
        public void Operator_Subtract_Vector()
        {
            // Arrange
            Vector vector = new Vector(1.0, -2.0, 3.0);
            Vector result = new Vector(-1.0, 2.0, -3.0);
            //Act
            Vector otherVector = -vector;
            // Assert
            Assert.IsTrue(otherVector.Equals(result));
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Tests the static operator <see cref="Vector.operator *(double, Vector)"/>.
        /// </summary>
        [TestMethod("Operator Multiply(Double,Vector)")]
        public void Operator_Multiply_Double_Vector()
        {
            // Arrange
            double number = 1.5;
            Vector vector = new Vector(2.2, -4.6, 9.2);
            Vector result = new Vector(3.3, -6.9, 13.8);
            //Act
            Vector otherVector = number * vector;
            // Assert
            Assert.IsTrue(otherVector.Equals(result));
        }

        /// <summary>
        /// Tests the static operator <see cref="Vector.operator *(Vector, double)"/>.
        /// </summary>
        [TestMethod("Operator Multiply(Double,Vector)")]
        public void Operator_Multiply_Vector_Double()
        {
            // Arrange
            Vector vector = new Vector(2.2, -4.6, 9.2);
            double number = 1.5;
            Vector result = new Vector(3.3, -6.9, 13.8);
            //Act
            Vector otherVector = vector * number;
            // Assert
            Assert.IsTrue(otherVector.Equals(result));
        }

        /// <summary>
        /// Tests the static operator <see cref="Vector.operator /(Vector, double)"/>.
        /// </summary>
        [TestMethod("Operator Divide(Vector,Double)")]
        public void Operator_Divide_Vector_Double()
        {
            // Arrange
            Vector vector = new Vector(2.4, -4.6, 9.2);
            double number = 2.0;
            Vector result = new Vector(1.2, -2.3, 4.6);
            //Act
            Vector otherVector = vector / number;
            // Assert
            Assert.IsTrue(otherVector.Equals(result));
        }


        /******************** Hilbert Space ********************/

        /// <summary>
        /// Tests the static operator <see cref="Vector.operator *(Vector, Vector)"/>.
        /// </summary>
        [TestMethod("Operator Multiply(Vector,Vector)")]
        public void Operator_Multiply_Vector_Vector()
        {
            // Arrange
            Vector vectorA = new Vector(2.2, -4.6, 9.2);
            Vector vectorB = new Vector(1.4, -2.6, -5.2);
            double result = -32.8;
            //Act
            double dotProduct = vectorA * vectorB;
            // Assert
            Assert.AreEqual(dotProduct, result, Settings.AbsolutePrecision);
        }

        #endregion

        #region Casts

        /// <summary>
        /// Tests the implicit cast of a <see cref="Point"/> into a <see cref="Vector"/>.
        /// </summary>
        [TestMethod("Cast FromPoint")]
        public void Cast_FromPoint()
        {
            // Arrange
            Point point = new Point(1.0, 2.0, -3.0);
            Vector result = new Vector(1.0, 2.0, -3.0);
            // Act
            Vector vector = point;
            // Assert
            Assert.IsTrue(vector.Equals(result));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Tests the method <see cref="Vector.GetCoordinates"/>.
        /// </summary>
        [TestMethod("Method GetCoordinates()")]
        public void GetCoordinates()
        {
            // Arrange
            Vector vector = new Vector(1.0, 3.0, 5.0);
            double[] result = new double[3] { 1.0, 3.0, 5.0 };
            // Act
            double[] coordinates = vector.GetCoordinates();
            //Assert
            Assert.AreEqual(coordinates.Length, 3);
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(coordinates[i], result[i], Settings.AbsolutePrecision);
            }
        }


        /// <summary>
        /// Tests the method <see cref="Vector.SquaredLength"/>.
        /// </summary>
        [TestMethod("Method SquaredLength()")]
        public void SquaredLength()
        {
            // Arrange
            Vector vector = new Vector(1.0, 3.0, 5.0);
            double result = 35.0;
            // Act
            double squaredLength = vector.SquaredLength();
            //Assert
            Assert.AreEqual(squaredLength, result, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the method <see cref="Vector.Length"/>.
        /// </summary>
        [TestMethod("Method Length()")]
        public void Length()
        {
            // Arrange
            Vector vector = new Vector(1.0, -3.0, 5.0);
            double result = Math.Sqrt(35.0);
            // Act
            double length = vector.Length();
            //Assert
            Assert.AreEqual(length, result, Settings.AbsolutePrecision);
        }


        /// <summary>
        /// Tests the method <see cref="Vector.Unitise"/>.
        /// </summary>
        [TestMethod("Method Unitise()")]
        public void Unitise()
        {
            // Arrange
            Vector vector = new Vector(1.0, -5.0, 2.5);
            // Act
            vector.Unitise();
            double length = vector.Length();
            // Assert
            Assert.AreEqual(length, 1.0, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the method <see cref="Vector.IsUnit"/>.
        /// </summary>
        [TestMethod("Method IsUnit()")]
        public void IsUnit()
        {
            // Arrange
            Vector vectorA = new Vector(1.0, -5.0, 2.5);
            Vector vectorB = new Vector(3.0 / Math.Sqrt(50.0), 4.0 / Math.Sqrt(50.0), 5.0 / Math.Sqrt(50.0));
            // Act
            bool isAUnit  = vectorA.IsUnit();
            bool isBUnit = vectorB.IsUnit();
            // Assert
            Assert.IsFalse(isAUnit);
            Assert.IsTrue(isBUnit);
        }


        /// <summary>
        /// Tests the method <see cref="Vector.AngleWith(Vector)"/>.
        /// </summary>
        [TestMethod("Method AngleWith(Vector)")]
        public void AngleWith_Vector()
        {
            // Arrange
            Vector vectorA = new Vector(1.5, 3.0, 7.5);
            Vector vectorB = new Vector(4.0, 5.0, 6.0);
            double result = Math.Acos(66 / (Math.Sqrt(67.5) * Math.Sqrt(77)));
            //Act
            double angle = vectorA.AngleWith(vectorB);
            // Assert
            Assert.AreEqual(angle, result, Settings.AbsolutePrecision);
        }


        /// <summary>
        /// Tests the method <see cref="Vector.Equals(Vector)"/>.
        /// </summary>
        [TestMethod("Method Equals(Vector)")]
        public void Equals_Vector()
        {
            // Arrange
            Vector vectorA = new Vector(1.0, 3.0, 5.0);
            Vector vectorB = new Vector(1.0, 3.0, 5.0);
            // Act
            bool areEqual = vectorA.Equals(vectorB);
            // Assert
            Assert.IsTrue(areEqual);
        }

        #endregion


        #region Explicit : Additive.IAbelianGroup<Vector>

        /******************** Properties ********************/

        /// <summary>
        /// Tests the <see cref="Alg_Fund.IAddable{T}.IsAssociative"/> property of <see cref="Vector"/>.
        /// </summary>
        [TestMethod("AsIAddable<Vector> Property IsAssociative")]
        public void AsIAddable_IsAssociative()
        {
            // Arrange
            Vector vector = new Vector(1.0, -5.0, 2.5);
            // Act
            Alg_Fund.IAddable<Vector> addable = (Alg_Fund.IAddable<Vector>)vector;
            // Assert
            Assert.IsTrue(addable.IsAssociative);
        }

        /// <summary>
        /// Tests the <see cref="Alg_Fund.IAddable{T}.IsCommutative"/> property of <see cref="Vector"/>.
        /// </summary>
        [TestMethod("AsIAddable<Vector> Property IsCommutative")]
        public void AsIAddable_IsCommutative()
        {
            // Arrange
            Vector vector = new Vector(1.0, -5.0, 2.5);
            // Act
            Alg_Fund.IAddable<Vector> addable = (Alg_Fund.IAddable<Vector>)vector;
            // Assert
            Assert.IsTrue(addable.IsCommutative);
        }


        /******************** Methods ********************/

        /// <summary>
        /// Tests the <see cref="Alg_Fund.IAddable{T}.Add(T)"/> method 
        /// computing the addition of the current <see cref="Vector"/> with another <see cref="Vector"/>.
        /// </summary>
        [TestMethod("AsIAddable<Vector> Add(Vector)")]
        public void AsIAddable_Add_Vector()
        {
            // Arrange
            Vector vectorA = new Vector(1.5, 6.0, 3.2);
            Vector vectorB = new Vector(5.5, 2.0, 4.5);
            Vector result = new Vector(7.0, 8.0, 7.7);
            //Act
            Alg_Fund.IAddable<Vector> addable = (Alg_Fund.IAddable<Vector>)vectorA;
            Vector otherVector = addable.Add(vectorB);
            // Assert
            Assert.IsTrue(otherVector.Equals(result));
        }

        /// <summary>
        /// Tests the <see cref="Alg_Fund.ISubtractable{T}.Subtract(T)"/> method 
        /// computing the subtraction of the current <see cref="Vector"/> with another <see cref="Vector"/>.
        /// </summary>
        [TestMethod("AsISubtractable<Vector> Subtract(Vector)")]
        public void AsISubtractable_Substract_Vector()
        {
            // Arrange
            Vector vectorA = new Vector(1.5, 6.0, 3.2);
            Vector vectorB = new Vector(5.5, 2.0, 4.5);
            Vector result = new Vector(-4.0, 4.0, -1.3);
            //Act
            Alg_Fund.ISubtractable<Vector> subtractable = (Alg_Fund.ISubtractable<Vector>)vectorA;
            Vector otherVector = subtractable.Subtract(vectorB);
            // Assert
            Assert.IsTrue(otherVector.Equals(result));
        }

        /// <summary>
        /// Tests the <see cref="Alg_Str.Additive.IGroup{T}.Opposite"/> method returning the opposite of the current <see cref="Vector"/>.
        /// </summary>
        [TestMethod("AsIGroup<Vector> Opposite()")]
        public void AsIGroup_Opposite()
        {
            // Arrange
            Vector vector = new Vector(1.5, -6.0, 3.2);
            Vector result = new Vector(-1.5, 6.0, -3.2);
            //Act
            Alg_Str.Additive.IGroup<Vector> opposable = (Alg_Str.Additive.IGroup<Vector>)vector;
            opposable.Opposite();
            // Assert
            Assert.IsTrue(opposable.Equals(result));
        }

        /// <summary>
        /// Tests the <see cref="Alg_Fund.IZeroable{T}.Zero"/> method returning the additive neutral element of the <see cref="Vector"/>.
        /// </summary>
        [TestMethod("AsIZeroable<Vector> Zero()")]
        public void AsIZeroable_Zero()
        {
            // Arrange
            Vector vector = new Vector(1.5, 6.0, 3.2);
            Vector result = new Vector(0.0, 0.0, 0.0);
            //Act
            Alg_Fund.IZeroable<Vector> zeroable = (Alg_Fund.IZeroable<Vector>)vector;
            Vector otherVector = zeroable.Zero();
            // Assert
            Assert.IsTrue(otherVector.Equals(result));
        }

        #endregion

        #region Explicit : IGroupAction<Double,Vector>

        /******************** Methods ********************/

        /// <summary>
        /// Tests the <see cref="Alg_Str.IGroupAction{TValue, T}.Multiply(TValue)"/> method
        /// computing the scalar multiplication of the current <see cref="Vector"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        [TestMethod("AsIGroupAction<Double,Vector> Multiply(Double)")]
        public void AsIGroupAction_Multiply_Double()
        {
            // Arrange
            Vector vector = new Vector(1.0, 2.5, 5.2);
            double number = 4.0;
            Vector result = new Vector(4.0, 10.0, 20.8);
            //Act
            Alg_Str.IGroupAction<double, Vector> groupActionable = (Alg_Str.IGroupAction<double, Vector>)vector;
            Vector otherVector = groupActionable.Multiply(number);
            // Assert
            Assert.IsTrue(otherVector.Equals(result));
        }

        /// <summary>
        /// Tests the <see cref="Alg_Str.IGroupAction{TValue,T}.Divide(TValue)"/> method
        /// computing the scalar division of the current <see cref="Vector"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        [TestMethod("AsIGroupAction<Double,Vector> Divide(Double)")]
        public void AsIGroupAction_Divide_Double()
        {
            // Arrange
            Vector vector = new Vector(1.0, 6.0, 3.4);
            double number = 4.0;
            Vector result = new Vector(0.25, 1.5, 0.85);
            //Act
            Alg_Str.IGroupAction<double, Vector> groupActionable = (Alg_Str.IGroupAction<double, Vector>)vector;
            Vector otherVector = groupActionable.Divide(number);
            // Assert
            Assert.IsTrue(otherVector.Equals(result));
        }

        #endregion

        #region Explicit : IDotProduct<Double,Vector>

        /******************** Methods ********************/

        /// <summary>
        /// Tests the <see cref="Alg_Mes.IMetric{T}.DistanceTo(T)"/> method
        /// computing the distance between the current <see cref="Vector"/> with another <see cref="Vector"/>.
        /// </summary>
        [TestMethod("AsIMetric<Vector> Distance(Vector)")]
        public void AsIMetric_Distance_Vector()
        {
            // Arrange
            Vector vectorA = new Vector(1.0, 3.0, 5.0);
            Vector vectorB = new Vector(2.0, 2.0, 2.0);
            double result = Math.Sqrt(11.0);
            // Act
            Alg_Mes.IMetric<Vector> metricable = (Alg_Mes.IMetric<Vector>)vectorA;
            double distance = metricable.DistanceTo(vectorB);
            //Assert
            Assert.AreEqual(distance, result, Settings.AbsolutePrecision);
        }


        /// <summary>
        /// Tests the <see cref="Alg_Mes.INorm{T}.Norm"/> method computing the norm of the current <see cref="Vector"/>.
        /// </summary>
        [TestMethod("AsINorm<Vector> Norm()")]
        public void Norm()
        {
            // Arrange
            Vector vector = new Vector(1.0, 3.0, 5.0);
            double result = Math.Sqrt(35.0);
            // Act
            Alg_Mes.INorm<Vector> normable = (Alg_Mes.INorm<Vector>)vector;
            double norm = normable.Norm();
            //Assert
            Assert.AreEqual(norm, result, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the <see cref="Alg_Mes.IDotProduct{TValue,T}.DotProduct(T)"/> method 
        /// computing the dot product of the current <see cref="Vector"/> with another <see cref="Vector"/>.
        /// </summary>
        [TestMethod("AsIDotProduct<Double,Vector> DotProduct(Vector)")]
        public void AsIDotProduct_DotProduct_Vector()
        {
            // Arrange
            Vector vectorA = new Vector(2.2, -4.6, 9.2);
            Vector vectorB = new Vector(1.4, -2.6, -5.2);
            double result = -32.8;
            //Act
            Alg_Mes.IDotProduct<double, Vector> dotProductable = (Alg_Mes.IDotProduct<double, Vector>)vectorA;
            double dotProduct = dotProductable.DotProduct(vectorB);
            // Assert
            Assert.AreEqual(dotProduct, result, Settings.AbsolutePrecision);
        }

        #endregion
    }
}
