using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.Geometry.Euclidean3D;
using Alg_Fund = BRIDGES.Algebra.Fundamentals;
using Alg_Str = BRIDGES.Algebra.Sets;
using Alg_Mes = BRIDGES.Algebra.Measure;


namespace BRIDGES.Test.Geometry.Euclidean3D
{
    /// <summary>
    /// Class testing the members of the <see cref="Line"/> structure.
    /// </summary>
    [TestClass]
    public class LineTest
    {
        #region Behavior

        /// <summary>
        /// Tests that <see cref="Line"/> is not reference type.
        /// </summary>
        [TestMethod("Behavior IsNotReference")]
        public void IsNotReference()
        {
            // Arrange
            Line lineA = new Line(new Point(1.0, 2.0 ,3.0), new Vector(1.5, 2.5, 3.5));
            Line lineB = new Line(new Point(4.0, 5.0, 6.0), new Vector(4.5, 5.5, 6.5));

            //Act
            lineA = lineB;

            // Assert
            Assert.IsTrue(lineA.Equals(lineB));
            Assert.AreNotSame(lineA, lineB);
        }

        #endregion


        #region Properties

        /// <summary>
        /// Tests the initialisation of the <see cref="Line"/> from its three cooridnates,
        /// and the <see cref="Line.Origin"/>, <see cref="Line.Axis"/> properties.
        /// </summary>
        [TestMethod("Property Origin & Axis")]
        public void OriginAndAxis()
        {
            // Arrange
            Line line = new Line(new Point(1.0, 2.0, 3.0), new Vector(1.5, 2.5, 3.5));
            Point expOrigin = new Point(1.0, 2.0, 3.0);
            Vector expAxis = new Vector(1.5, 2.5, 3.5);

            //Act
            Point origin = line.Origin;
            Vector axis = line.Axis;

            // Assert
            Assert.IsTrue(origin.Equals(expOrigin));
            Assert.IsTrue(axis.Equals(expAxis));
        }


        #endregion

        #region Constructors

        /// <summary>
        /// Tests the initialisation of the <see cref="Line"/> from another <see cref="Line"/>.
        /// </summary>
        [TestMethod("Constructor(Line)")]
        public void Constructor_Line()
        {
            // Arrange
            Line expected = new Line(new Point(1.0, 2.0, 3.0), new Vector(1.5, 2.5, 3.5));

            // Act
            Line line = new Line(expected);

            // Assert
            Assert.IsTrue(line.Equals(expected));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Tests the method <see cref="Line.Flip()"/>.
        /// </summary>
        [TestMethod("Method Flip()")]
        public void Flip()
        {
            // Arrange
            Line line = new Line(new Point(1.0, 2.0, 3.0), new Vector(1.5, 2.5, 3.5));
            Line expected = new Line(new Point(1.0, 2.0, 3.0), new Vector(-1.5, -2.5, -3.5));
            // Act
            line.Flip();
            //Assert
            Assert.IsTrue(line.Origin.Equals(expected.Origin));
            Assert.IsTrue(line.Axis.Equals(expected.Axis));
        }

        /// <summary>
        /// Tests the method <see cref="Line.PointAt(double)"/>.
        /// </summary>
        [TestMethod("Method PointAt(Double)")]
        public void PointAt_Double()
        {
            // Arrange
            Line line = new Line(new Point(1.0, 2.0, 3.0), new Vector(1.5, 2.5, 3.5));
            double paramA = 0.0;
            Point expectedA = new Point(1.0, 2.0, 3.0);
            double paramB = Math.Sqrt(20.75);
            Point expectedB = new Point(2.5, 4.5, 6.5);
            double paramC = 3.0 * Math.Sqrt(20.75);
            Point expectedC = new Point(5.5, 9.5, 13.5);

            // Act
            Point a = line.PointAt(paramA);
            Point b = line.PointAt(paramB);
            Point c = line.PointAt(paramC);

            //Assert
            Assert.IsTrue(a.Equals(expectedA));
            Assert.IsTrue(b.Equals(expectedB));
            Assert.IsTrue(c.Equals(expectedC));
        }

        /// <summary>
        /// Tests the method <see cref="Line.Equals(Line)"/>.
        /// </summary>
        [TestMethod("Method Equals(Line)")]
        public void Equals_Line()
        {
            // Arrange
            Line lineA = new Line(new Point(1.0, 2.0, 3.0), new Vector(1.5, 2.5, 3.5));
            Line lineB = new Line(new Point(1.0, 2.0, 3.0), new Vector(1.5, 2.5, 3.5));
            // Act
            bool areEqual = lineA.Equals(lineB);
            // Assert
            Assert.IsTrue(areEqual);
        }

        #endregion
    }
}
