using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.Geometry.Euclidean3D;
using Alg_Fund = BRIDGES.Algebra.Fundamentals;
using Alg_Str = BRIDGES.Algebra.Structures;
using Alg_Mes = BRIDGES.Algebra.Measure;


namespace BRIDGES.Test.Geometry.Euclidean3D
{
    /// <summary>
    /// Class testing the members of the <see cref="Point"/> structure.
    /// </summary>
    [TestClass]
    public class PointTest
    {
        #region Behavior

        /// <summary>
        /// Tests that <see cref="Point"/> is not reference type.
        /// </summary>
        [TestMethod("Behavior IsNotReference")]
        public void IsNotReference()
        {
            // Arrange
            Point pointA = new Point(1.0, 2.0, 3.0);
            Point pointB = new Point(4.0, 5.0, 6.0);

            //Act
            pointA = pointB;

            // Assert
            Assert.IsTrue(pointA.Equals(pointB));
            Assert.AreNotSame(pointA, pointB);
        }

        #endregion


        #region Properties

        /// <summary>
        /// Tests the <see cref="Point.Dimension"/> property.
        /// </summary>
        [TestMethod("Property Dimension")]
        public void Dimension()
        {
            // Arrange
            Point point = new Point(1.5, 1.7, 2.1);

            //Act
            int dimension = point.Dimension;

            // Assert
            Assert.AreEqual(3, dimension);
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="Point"/> from its three cooridnates,
        /// and the <see cref="Point.X"/>, <see cref="Point.Y"/>, <see cref="Point.Z"/> properties.
        /// </summary>
        [TestMethod("Property X & Y & Z")]
        public void XAndYAndZ()
        {
            // Arrange
            Point point = new Point(1.5, 1.7, 2.1);

            //Act
            double x = point.X;
            double y = point.Y;
            double z = point.Z;

            // Assert
            Assert.AreEqual(x, 1.5, Settings.AbsolutePrecision);
            Assert.AreEqual(y, 1.7, Settings.AbsolutePrecision);
            Assert.AreEqual(z, 2.1, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the <see cref="Point"/> indexer property.
        /// </summary>
        [TestMethod("Property this[int]")]
        public void Indexer()
        {
            // Arrange
            Point point = new Point(1.5, 1.7, 2.1);
            //Act
            double x = point[0];
            double y = point[1];
            double z = point[2];
            // Assert
            Assert.AreEqual(x, 1.5, Settings.AbsolutePrecision);
            Assert.AreEqual(y, 1.7, Settings.AbsolutePrecision);
            Assert.AreEqual(z, 2.1, Settings.AbsolutePrecision);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Tests the initialisation of the <see cref="Point"/> from its coordinates.
        /// </summary>
        [TestMethod("Constructor(Double[])")]
        public void Constructor_DoubleArray()
        {
            // Arrange
            Point result = new Point(2.0, 5.0, 8.0);
            bool throwsException = false;

            // Act
            Point point = new Point(new double[3] { 2.0, 5.0, 8.0 });

            try { Point otherPoint = new Point(new double[] { 2.0, 5.0, 8.0, 11.0 }); } 
            catch (ArgumentOutOfRangeException e) { throwsException = true; }

            // Assert
            Assert.IsTrue(point.Equals(result));
            Assert.IsTrue(throwsException);
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="Point"/> from another <see cref="Point"/>.
        /// </summary>
        [TestMethod("Constructor(Point)")]
        public void Constructor_Point()
        {
            // Arrange
            Point result = new Point(2.0, 5.0, 8.0);

            // Act
            Point point = new Point(result);

            // Assert
            Assert.IsTrue(point.Equals(result));
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Tests the static property <see cref="Point.Zero"/>.
        /// </summary>
        [TestMethod("Static Zero")]
        public void Static_Zero()
        {
            // Arrange
            Point result = new Point(0.0, 0.0, 0.0);
            // Act
            Point point = Point.Zero;
            // Assert
            Assert.IsTrue(point.Equals(result));
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Tests the static method <see cref="Point.CrossProduct(Point, Point)"/>.
        /// </summary>
        [TestMethod("Static CrossProduct(Point,Point)")]
        public void Static_CrossProduct_Point_Point()
        {
            // Arrange
            Point pointA = new Point(1.0, 2.0, 3.0);
            Point pointB = new Point(1.5, 3.5, 5.0);
            Point result = new Point(-0.5, -0.5, 0.5);
            //Act
            Point otherPoint = Point.CrossProduct(pointA, pointB);
            // Assert
            Assert.IsTrue(otherPoint.Equals(result));
        }


        /******************** Algebraic Additive Group ********************/

        /// <summary>
        /// Tests the static method <see cref="Point.Add(Point, Point)"/>.
        /// </summary>
        [TestMethod("Static Add(Point,Point)")]
        public void Static_Add_Point_Point()
        {
            // Arrange
            Point pointA = new Point(1.5, 2.5, 3.5);
            Point pointB = new Point(1.2, 2.4, 3.6);
            Point result = new Point(2.7, 4.9, 7.1);
            //Act
            Point otherPoint = Point.Add(pointA, pointB);
            // Assert
            Assert.IsTrue(otherPoint.Equals(result));
        }

        /// <summary>
        /// Tests the static method <see cref="Point.Subtract(Point, Point)"/>.
        /// </summary>
        [TestMethod("Static Subtract(Point,Point)")]
        public void Static_Substract_Point_Point()
        {
            // Arrange
            Point pointA = new Point(1.5, 2.5, 3.5);
            Point pointB = new Point(1.2, 2.4, 3.6);
            Point result = new Point(0.3, 0.1, -0.1);
            //Act
            Point otherPoint = Point.Subtract(pointA, pointB);
            // Assert
            Assert.IsTrue(otherPoint.Equals(result));
        }


        /******************** Vector Embedding ********************/

        /// <summary>
        /// Tests the static method <see cref="Point.Add(Point, Vector)"/>
        /// </summary>
        [TestMethod("Static Add(Point,Vector)")]
        public void Static_Add_Point_Vector()
        {
            // Arrange
            Point point = new Point(2.2, 4.6, 9.2);
            Vector vector = new Vector(3.3, 4.4, 5.5);
            Point result = new Point(5.5, 9.0, 14.7);
            //Act
            Point otherPoint = Point.Add(point, vector);
            // Assert
            Assert.IsTrue(otherPoint.Equals(result));
        }

        /// <summary>
        /// Tests the static method <see cref="Point.Subtract(Point, Vector)"/>
        /// </summary>
        [TestMethod("Static Subtract(Point,Vector)")]
        public void Static_Subtract_Point_Vector()
        {
            // Arrange
            Point point = new Point(2.2, 4.6, 9.2);
            Vector vector = new Vector(3.3, -4.4, 5.5);
            Point result = new Point(-1.1, 9.0, 3.7);
            //Act
            Point otherPoint = Point.Subtract(point, vector);
            // Assert
            Assert.IsTrue(otherPoint.Equals(result));
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Tests the static method <see cref="Point.Multiply(double, Point)"/>
        /// </summary>
        [TestMethod("Static Multiply(Double,Point)")]
        public void Static_Multiply_Double_Point()
        {
            // Arrange
            double number = 1.5;
            Point point = new Point(2.2, -4.6, 9.2);
            Point result = new Point(3.3, -6.9, 13.8);
            //Act
            Point otherPoint = Point.Multiply(number, point);
            // Assert
            Assert.IsTrue(otherPoint.Equals(result));
        }

        /// <summary>
        /// Tests the static method <see cref="Point.Divide(Point, double)"/>
        /// </summary>
        [TestMethod("Static Divide(Point,Double)")]
        public void Static_Divide_Point_Double()
        {
            // Arrange
            Point point = new Point(2.4, -4.6, 9.2);
            double number = 2.0;
            Point result = new Point(1.2, -2.3, 4.6);
            //Act
            Point otherPoint = Point.Divide(point, number);
            // Assert
            Assert.IsTrue(otherPoint.Equals(result));
        }


        /******************** Hilbert Space ********************/

        /// <summary>
        /// Tests the static method <see cref="Point.DotProduct(Point, Point)"/>.
        /// </summary>
        [TestMethod("Static DotProduct(Point,Point)")]
        public void Static_DotProduct_Point_Point()
        {
            // Arrange
            Point pointA = new Point(2.2, -4.6, 9.2);
            Point pointB= new Point(1.4, -2.6, -5.2);
            double result = -32.8; 
            //Act
            double dotProduct = Point.DotProduct(pointA, pointB);
            // Assert
            Assert.AreEqual(dotProduct, result, Settings.AbsolutePrecision);
        }

        #endregion

        #region Static Operator

        /******************** Algebraic Field ********************/

        /// <summary>
        /// Tests the static operator <see cref="Point.operator +(Point, Point)"/>.
        /// </summary>
        [TestMethod("Operator Add(Point,Point)")]
        public void Operator_Add_Point_Point()
        {
            // Arrange
            Point pointA = new Point(1.5, 2.5, 3.5);
            Point pointB = new Point(1.2, 2.4, 3.6);
            Point result = new Point(2.7, 4.9, 7.1);
            //Act
            Point otherPoint = pointA + pointB;
            // Assert
            Assert.IsTrue(otherPoint.Equals(result));
        }

        /// <summary>
        /// Tests the static operator <see cref="Point.operator -(Point, Point)"/>.
        /// </summary>
        [TestMethod("Operator Subtract(Point,Point)")]
        public void Operator_Subtract_Point_Point()
        {
            // Arrange
            Point pointA = new Point(1.5, 2.5, 3.5);
            Point pointB = new Point(1.2, 2.4, 3.6);
            Point result = new Point(0.3, 0.1, -0.1);
            //Act
            Point otherPoint = pointA - pointB;
            // Assert
            Assert.IsTrue(otherPoint.Equals(result));
        }

        /// <summary>
        /// Tests the static operator <see cref="Point.operator -(Point)"/>.
        /// </summary>
        [TestMethod("Operator Subtract(Point)")]
        public void Operator_Subtract_Point()
        {
            // Arrange
            Point point = new Point(1.0, -2.0, 3.0);
            Point result = new Point(-1.0, 2.0, -3.0);
            //Act
            Point otherPoint = - point;
            // Assert
            Assert.IsTrue(otherPoint.Equals(result));
        }


        /******************** Vector Embedding ********************/

        /// <summary>
        /// Tests the static operator <see cref="Point.operator +(Point, Vector)"/>.
        /// </summary>
        [TestMethod("Operator Add(Point,Vector)")]
        public void Operator_Add_Point_Vector()
        {
            // Arrange
            Point point = new Point(1.5, 2.5, 3.5);
            Vector vector = new Vector(1.2, 2.4, 3.6);
            Point result = new Point(2.7, 4.9, 7.1);
            //Act
            Point otherPoint = point + vector;
            // Assert
            Assert.IsTrue(otherPoint.Equals(result));
        }

        /// <summary>
        /// Tests the static operator <see cref="Point.operator -(Point, Vector)"/>.
        /// </summary>
        [TestMethod("Operator Subtract(Point,Vector)")]
        public void Operator_Subtract_Point_Vector()
        {
            // Arrange
            Point point = new Point(1.5, 2.5, 3.5);
            Vector vector = new Vector(1.2, 2.4, 3.6);
            Point result = new Point(0.3, 0.1, -0.1);
            //Act
            Point otherPoint = point - vector;
            // Assert
            Assert.IsTrue(otherPoint.Equals(result));
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Tests the static operator <see cref="Point.operator *(double, Point)"/>.
        /// </summary>
        [TestMethod("Operator Multiply(Double,Point)")]
        public void Operator_Multiply_Double_Point()
        {
            // Arrange
            double number = 1.5;
            Point point = new Point(2.2, -4.6, 9.2);
            Point result = new Point(3.3, -6.9, 13.8);
            //Act
            Point otherPoint = number * point;
            // Assert
            Assert.IsTrue(otherPoint.Equals(result));
        }

        /// <summary>
        /// Tests the static operator <see cref="Point.operator *(Point, double)"/>.
        /// </summary>
        [TestMethod("Operator Multiply(Point,Double)")]
        public void Operator_Multiply_Point_Double()
        {
            // Arrange
            Point point = new Point(2.2, -4.6, 9.2);
            double number = 1.5;
            Point result = new Point(3.3, -6.9, 13.8);
            //Act
            Point otherPoint = point * number;
            // Assert
            Assert.IsTrue(otherPoint.Equals(result));
        }

        /// <summary>
        /// Tests the static operator <see cref="Point.operator /(Point, double)"/>.
        /// </summary>
        [TestMethod("Operator Divide(Point,Double)")]
        public void Operator_Divide_Point_Double()
        {
            // Arrange
            Point point = new Point(2.4, -4.6, 9.2);
            double number = 2.0;
            Point result = new Point(1.2, -2.3, 4.6);
            //Act
            Point otherPoint = point / number;
            // Assert
            Assert.IsTrue(otherPoint.Equals(result));
        }


        /******************** Hilbert Space ********************/

        /// <summary>
        /// Tests the static operator <see cref="Point.operator *(Point, Point)"/>.
        /// </summary>
        [TestMethod("Operator Multiply(Point,Point)")]
        public void Operator_Multiply_Point_Point()
        {
            // Arrange
            Point pointA = new Point(2.2, -4.6, 9.2);
            Point pointB = new Point(1.4, -2.6, -5.2);
            double result = -32.8;
            //Act
            double dotProduct = pointA * pointB;
            // Assert
            Assert.AreEqual(dotProduct, result, Settings.AbsolutePrecision);
        }

        #endregion

        #region Casts

        /// <summary>
        /// Tests the implicit cast of a <see cref="Vector"/> into a <see cref="Point"/>.
        /// </summary>
        [TestMethod("Cast FromVector")]
        public void Cast_FromVector()
        {
            // Arrange
            Vector vector = new Vector(1.0, 2.0, -3.0);
            Point result = new Point(1.0, 2.0, -3.0);
            // Act
            Point point = vector;
            // Assert
            Assert.IsTrue(point.Equals(result));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Tests the method <see cref="Point.DistanceTo(Point)"/>.
        /// </summary>
        [TestMethod("Method Distance(Point)")]
        public void Distance_Point()
        {
            // Arrange
            Point pointA = new Point(1.0, 3.0, 5.0);
            Point pointB = new Point(2.0, 2.0, 2.0);
            double result = Math.Sqrt(11.0);
            // Act
            double distance = pointA.DistanceTo(pointB);
            //Assert
            Assert.AreEqual(distance, result, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the method <see cref="Point.Norm"/>.
        /// </summary>
        [TestMethod("Method Norm()")]
        public void Norm()
        {
            // Arrange
            Point point = new Point(1.0, 3.0, 5.0);
            double result = Math.Sqrt(35.0);
            // Act
            double norm = point.Norm();
            //Assert
            Assert.AreEqual(norm, result, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the method <see cref="Point.GetCoordinates"/>.
        /// </summary>
        [TestMethod("Method GetCoordinates()")]
        public void GetCoordinates()
        {
            // Arrange
            Point point = new Point(1.0, 3.0, 5.0);
            double[] result = new double[3] { 1.0, 3.0, 5.0 };
            // Act
            double[] coordinates = point.GetCoordinates();
            //Assert
            Assert.AreEqual(coordinates.Length, 3);
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(coordinates[i], result[i], Settings.AbsolutePrecision);
            }
        }


        /// <summary>
        /// Tests the method <see cref="Point.Equals(Point)"/>.
        /// </summary>
        [TestMethod("Method Equals(Point)")]
        public void Equals_Point()
        {
            // Arrange
            Point pointA = new Point(1.0, 3.0, 5.0);
            Point pointB = new Point(1.0, 3.0, 5.0);
            // Act
            bool areEqual = pointA.Equals(pointB);
            // Assert
            Assert.IsTrue(areEqual);
        }

        #endregion


        #region Explicit : Additive.IAbelianGroup<Point>

        /******************** Properties ********************/

        /// <summary>
        /// Tests the <see cref="Alg_Fund.IAddable{T}.IsAssociative"/> property of <see cref="Point"/>.
        /// </summary>
        [TestMethod("AsIAddable<Point> Property IsAssociative")]
        public void AsIAddable_IsAssociative()
        {
            // Arrange
            Point point = new Point(1.0, -5.0, 2.5);
            // Act
            Alg_Fund.IAddable<Point> addable = (Alg_Fund.IAddable<Point>)point;
            // Assert
            Assert.IsTrue(addable.IsAssociative);
        }

        /// <summary>
        /// Tests the <see cref="Alg_Fund.IAddable{T}.IsCommutative"/> property of <see cref="Point"/>.
        /// </summary>
        [TestMethod("AsIAddable<Point> Property IsCommutative")]
        public void AsIAddable_IsCommutative()
        {
            // Arrange
            Point point = new Point(1.0, -5.0, 2.5);
            // Act
            Alg_Fund.IAddable<Point> addable = (Alg_Fund.IAddable<Point>)point;
            // Assert
            Assert.IsTrue(addable.IsCommutative);
        }


        /******************** Methods ********************/

        /// <summary>
        /// Tests the <see cref="Alg_Fund.IAddable{T}.Add(T)"/> method of <see cref="Point"/>.
        /// </summary>
        [TestMethod("AsIAddable<Point> Add(Point)")]
        public void AsIAddable_Add_Point()
        {
            // Arrange
            Point pointA = new Point(1.5, 6.0, 3.2);
            Point pointB = new Point(5.5, 2.0, 4.5);
            Point result = new Point(7.0, 8.0, 7.7);
            //Act
            Alg_Fund.IAddable<Point> addable = (Alg_Fund.IAddable<Point>)pointA;
            Point otherPoint = addable.Add(pointB);
            // Assert
            Assert.IsTrue(otherPoint.Equals(result));
        }

        /// <summary>
        /// Tests the <see cref="Alg_Fund.ISubtractable{T}.Subtract(T)"/> method of <see cref="Point"/>.
        /// </summary>
        [TestMethod("AsISubtractable<Point> Subtract(Point)")]
        public void AsISubtractable_Substract_Point()
        {
            // Arrange
            Point pointA = new Point(1.5, 6.0, 3.2);
            Point pointB = new Point(5.5, 2.0, 4.5);
            Point result = new Point(-4.0, 4.0, -1.3);
            //Act
            Alg_Fund.ISubtractable<Point> subtractable = (Alg_Fund.ISubtractable<Point>)pointA;
            Point otherPoint = subtractable.Subtract(pointB);
            // Assert
            Assert.IsTrue(otherPoint.Equals(result));
        }

        /// <summary>
        /// Tests the <see cref="Alg_Str.Additive.IGroup{T}.Opposite"/> method of <see cref="Point"/>.
        /// </summary>
        [TestMethod("AsIGroup<Point> Opposite()")]
        public void AsIGroup_Opposite()
        {
            // Arrange
            Point point = new Point(1.5, -6.0, 3.2);
            Point result = new Point(-1.5, 6.0, -3.2);
            //Act
            Alg_Str.Additive.IGroup<Point> opposable = (Alg_Str.Additive.IGroup<Point>)point;
            opposable.Opposite();
            // Assert
            Assert.IsTrue(opposable.Equals(result));
        }

        /// <summary>
        /// Tests the <see cref="Alg_Fund.IZeroable{T}.Zero"/> method of <see cref="Point"/>.
        /// </summary>
        [TestMethod("AsIZeroable<Point> Zero()")]
        public void AsIZeroable_Zero()
        {
            // Arrange
            Point point = new Point(1.5, 6.0, 3.2);
            Point result = new Point(0.0, 0.0, 0.0);
            //Act
            Alg_Fund.IZeroable<Point> zeroable = (Alg_Fund.IZeroable<Point>)point;
            Point otherPoint = zeroable.Zero();
            // Assert
            Assert.IsTrue(otherPoint.Equals(result));
        }

        #endregion

        #region Explicit : IGroupAction<Double,Point>

        /******************** Methods ********************/

        /// <summary>
        /// Tests the <see cref="Alg_Str.IGroupAction{TValue, T}.Multiply(TValue)"/> method of <see cref="Point"/>.
        /// </summary>
        [TestMethod("AsIGroupAction<Double,Point> Multiply(Double)")]
        public void AsIGroupAction_Multiply_Double()
        {
            // Arrange
            Point point = new Point(1.0, 2.5, 5.2);
            double number = 4.0;
            Point result = new Point(4.0, 10.0, 20.8);
            //Act
            Alg_Str.IGroupAction<double, Point> groupActionable = (Alg_Str.IGroupAction<double, Point>)point;
            Point otherPoint = groupActionable.Multiply(number);
            // Assert
            Assert.IsTrue(otherPoint.Equals(result));
        }

        /// <summary>
        /// Tests the <see cref="Alg_Str.IGroupAction{TValue,T}.Divide(TValue)"/> method of <see cref="Point"/>.
        /// </summary>
        [TestMethod("AsIGroupAction<Double,Point> Divide(Double)")]
        public void AsIGroupAction_Divide_Double()
        {
            // Arrange
            Point point = new Point(1.0, 6.0, 3.4);
            double number = 4.0;
            Point result = new Point(0.25, 1.5, 0.85);
            //Act
            Alg_Str.IGroupAction<double, Point> groupActionable = (Alg_Str.IGroupAction<double, Point>)point;
            Point otherPoint = groupActionable.Divide(number);
            // Assert
            Assert.IsTrue(otherPoint.Equals(result));
        }

        #endregion

        #region Explicit : IDotProduct<Double,Point>

        /******************** Methods ********************/

        /// <summary>
        /// Tests the <see cref="Alg_Mes.INorm{T}.Unitise"/> method of <see cref="Point"/>.
        /// </summary>
        [TestMethod("AsINorm<Point> Unitise()")]
        public void AsINorm_Unitise()
        {
            // Arrange
            Point point = new Point(1.0, -5.0, 2.5);
            // Act
            Alg_Mes.INorm<Point> normable = (Alg_Mes.INorm<Point>)point;
            normable.Unitise();
            double norm = normable.Norm();
            // Assert
            Assert.AreEqual(norm, 1.0, Settings.AbsolutePrecision);
        }


        /// <summary>
        /// Tests the <see cref="Alg_Mes.IDotProduct{TValue,T}.DotProduct(T)"/> method of <see cref="Point"/>.
        /// </summary>
        [TestMethod("AsIDotProduct<Double,Point> DotProduct(Point)")]
        public void AsIDotProduct_DotProduct_Point()
        {
            // Arrange
            Point pointA = new Point(2.2, -4.6, 9.2);
            Point pointB = new Point(1.4, -2.6, -5.2);
            double result = -32.8;
            //Act
            Alg_Mes.IDotProduct<double, Point> dotProductable = (Alg_Mes.IDotProduct<double, Point>)pointA;
            double dotProduct = dotProductable.DotProduct(pointB);
            // Assert
            Assert.AreEqual(dotProduct, result, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the <see cref="Alg_Mes.IDotProduct{TValue,T}.AngleWith(T)"/> method of <see cref="Point"/>.
        /// </summary>
        [TestMethod("AsIDotProduct<Double,Point> AngleWith(Point)")]
        public void AsIDotProduct_AngleWith_Point()
        {
            // Arrange
            Point pointA = new Point(1.5, 3.0, 7.5);
            Point pointB = new Point(4.0, 5.0, 6.0); 
            double result = Math.Acos( 66 / (Math.Sqrt(67.5) * Math.Sqrt(77))); 
            //Act
            Alg_Mes.IDotProduct<double, Point> dotProductable = (Alg_Mes.IDotProduct<double, Point>)pointA;
            double angle = dotProductable.AngleWith(pointB);
            // Assert
            Assert.AreEqual(angle, result, Settings.AbsolutePrecision);
        }

        #endregion

        // TODO:
        // public void Static_MobiusInversion_Point_Point_Double()
    }
}
