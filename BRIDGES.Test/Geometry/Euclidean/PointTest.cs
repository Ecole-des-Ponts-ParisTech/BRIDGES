using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES;
using BRIDGES.Geometry.Euclidean;


namespace BRIDGES.Test
{
    /// <summary>
    /// Class defining the test for the <see cref="Point"/>.
    /// </summary>
    [TestClass]
    public class PointTest
    {
        /// <summary>
        /// Ensures that a <see cref="Point"/> is not a reference type.
        /// </summary>
        public void Point_IsNotReference()
        {
            // Arrange
            Point point = new Point(0.0, 0.0, 0.0);
            Point otherPoint = new Point(1.0, 2.0, 3.0);
            //Act
            point = otherPoint;
            // Assert
            Assert.IsTrue(point.Equals(otherPoint));
            Assert.IsFalse(object.ReferenceEquals(point, otherPoint));
        }

        #region Constructors

        /// <summary>
        /// Ensures that the parameterless constructor initialises a <see cref="Point"/> at the origin.
        /// </summary>
        [TestMethod]
        public void Point_DefaultConstructor()
        {
            // Arrange
            Point point = new Point();
            //Act
            Point otherPoint = new Point(0.0, 0.0, 0.0);
            // Assert
            Assert.IsTrue(point.Equals(otherPoint));
        }

        #endregion


        #region Static Methods

        /// <summary>
        /// Ensures the validity of the method adding two points.
        /// </summary>
        [TestMethod]
        public void Point_Add_PointPoint()
        {
            // Arrange
            Point point1 = new Point(1.0, 2.0, 3.0);
            Point point2 = new Point(1.5, 3.5, 5.0);
            //Act
            Point otherPoint = Point.Add(point1, point2);
            // Assert
            Assert.IsTrue(otherPoint.Equals(new Point(2.5, 5.5, 8.0)));
        }

        /// <summary>
        /// Ensures the validity of the method subtracting two <see cref="Point"/>.
        /// </summary>
        [TestMethod]
        public void Point_Substract_PointPoint()
        {
            // Arrange
            Point point1 = new Point(1.0, 2.0, 3.0);
            Point point2 = new Point(1.5, 3.5, 5.0);
            //Act
            Point otherPoint = Point.Subtract(point2, point1);
            // Assert
            Assert.IsTrue(otherPoint.Equals(new Point(0.5, 1.5, 2.0)));
        }

        /// <summary>
        /// Ensures the validity of the method multiplying a <see cref="Point"/> with a <see cref="double"/> value.
        /// </summary>
        [TestMethod]
        public void Point_Multiply_DoublePoint()
        {
            // Arrange
            Point point = new Point(1.3, 2.4, 3.3);
            double factor = 2.2;
            //Act
            Point otherPoint = Point.Multiply(factor, point);
            // Assert
            Assert.IsTrue(otherPoint.Equals(new Point(2.86, 5.28, 7.26)));
        }

        /// <summary>
        /// Ensures the validity of the method dividing a <see cref="Point"/> with a <see cref="double"/> value.
        /// </summary>
        [TestMethod]
        public void Point_Divide_PointDouble()
        {
            // Arrange
            Point point = new Point(1.5, 2.5, 3.2);
            double divisor = 2.0;
            //Act
            Point otherPoint = Point.Divide(point, divisor);
            // Assert
            Assert.IsTrue(otherPoint.Equals(new Point(0.75, 1.25, 1.6)));
        }

        /// <summary>
        /// Ensures the validity of the method computing the dot product of two <see cref="Point"/>.
        /// </summary>
        [TestMethod]
        public void Point_DotProduct_PointPoint()
        {
            // Arrange
            Point point1 = new Point(1.0, 2.0, 3.0);
            Point point2 = new Point(1.5, 3.5, 5.0);
            //Act
            double product = Point.DotProduct(point1, point2);
            // Assert
            Assert.AreEqual(23.5, product, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Ensures the validity of the method computing the cross product of two <see cref="Point"/>.
        /// </summary>
        [TestMethod]
        public void Point_CrossProduct_PointPoint()
        {
            // Arrange
            Point point1 = new Point(1.0, 2.0, 3.0);
            Point point2 = new Point(1.5, 3.5, 5.0);
            //Act
            Point product = Point.CrossProduct(point1, point2);
            // Assert
            Assert.IsTrue(product.Equals(new Point(-0.5, -0.5, 0.5)));
        }

        #endregion

        #region Operators

        /// <summary>
        /// Ensures the validity of the operator adding two points.
        /// </summary>
        [TestMethod]
        public void Point_OperatorAdd_PointPoint()
        {
            // Arrange
            Point point1 = new Point(1.0, 2.0, 3.0);
            Point point2 = new Point(1.5, 3.5, 5.0);
            //Act
            Point otherPoint = point1 + point2;
            // Assert
            Assert.IsTrue(otherPoint.Equals(new Point(2.5, 5.5, 8.0)));
        }

        /// <summary>
        /// Ensures the validity of the operator subtracting two <see cref="Point"/>.
        /// </summary>
        [TestMethod]
        public void Point_OperatorSubstract_PointPoint()
        {
            // Arrange
            Point point1 = new Point(1.0, 2.0, 3.0);
            Point point2 = new Point(1.5, 3.5, 5.0);
            //Act
            Point otherPoint = point2 - point1;
            // Assert
            Assert.IsTrue(otherPoint.Equals(new Point(0.5, 1.5, 2.0)));
        }


        /// <summary>
        /// Ensures the validity of the operator multiplying a <see cref="Point"/> with a <see cref="double"/> value.
        /// </summary>
        [TestMethod]
        public void Point_OperatorMultiply_PointDouble()
        {
            Point point = new Point(1.5, 3.5, 5.0);
            double factor = 2.5;
            //Act
            Point otherPoint = point * factor;
            // Assert
            Assert.IsTrue(otherPoint.Equals(new Point(3.75, 8.75, 12.5)));
        }

        /// <summary>
        /// Ensures the validity of the operator multiplying a <see cref="Point"/> with a <see cref="double"/> value.
        /// </summary>
        [TestMethod]
        public void Point_OperatorMultiply_DoublePoint()
        {
            Point point = new Point(1.5, 3.5, 5.0);
            double factor = 2.5;
            //Act
            Point otherPoint = factor * point;
            // Assert
            Assert.IsTrue(otherPoint.Equals(new Point(3.75, 8.75, 12.5)));
        }

        /// <summary>
        /// Ensures the validity of the operator dividing a <see cref="Point"/> with a <see cref="double"/> value.
        /// </summary>
        [TestMethod]
        public void Point_OperatorDivide_PointDouble()
        {
            Point point = new Point(1.5, 3.5, 5.0);
            double factor = 2.0;
            //Act
            Point otherPoint = point / factor;
            // Assert
            Assert.IsTrue(otherPoint.Equals(new Point(0.75, 1.75, 2.5)));
        }

        /// <summary>
        /// Ensures the validity of the operator computing the dot product of two <see cref="Point"/>.
        /// </summary>
        [TestMethod]
        public void Point_OperatorMultiply_PointPoint()
        {
            // Arrange
            Point point1 = new Point(1.0, 2.0, 3.0);
            Point point2 = new Point(1.5, 3.5, 5.0);
            //Act
            double product = point1 * point2;
            // Assert
            Assert.AreEqual(23.5, product, Settings.AbsolutePrecision);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Ensures the validity of the method computing the distance between two <see cref="Point"/>.
        /// </summary>
        [TestMethod]
        public void Point_Distance_Point()
        {
            // Arrange
            Point pointA = new Point(1.0, 3.0, 5.0);
            Point pointB = new Point(2.0, 2.0, 2.0);
            // Act
            double distance = pointA.DistanceTo(pointB);
            //Assert
            Assert.AreEqual(Math.Sqrt(11.0), distance);
        }

        #endregion
    }
}
