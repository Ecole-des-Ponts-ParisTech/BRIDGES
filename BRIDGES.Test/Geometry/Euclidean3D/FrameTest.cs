using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.Geometry.Euclidean3D;


namespace BRIDGES.Test.Geometry.Euclidean3D
{
    /// <summary>
    /// Class testing the members of the <see cref="Frame"/> class.
    /// </summary>
    [TestClass]
    public class FrameTest
    {
        #region Behavior

        /// <summary>
        /// Tests that <see cref="Frame"/> is reference type.
        /// </summary>
        [TestMethod("Behavior IsReference")]
        public void IsReference()
        {
            // Arrange
            Frame frameA = new Frame(new Point(2.0, 4.0, 6.0), new Vector(1.0, 2.0, 0.5), new Vector(-1.5, 2.5, 1.0), new Vector(-0.6, -0.4, 2.0));
            Frame frameB = new Frame(new Point(1.2, 2.8, 5.0), new Vector(0.5,0.4, 2.0), new Vector(0.0, -2.0, 0.8), new Vector(3.2, 0.2, -0.4));
            //Act
            frameA = frameB;
            // Assert
            Assert.IsTrue(frameA.Equals(frameB));
            Assert.AreSame(frameA, frameB);
        }

        #endregion


        #region Properties

        /// <summary>
        /// Tests the <see cref="Frame.Dimension"/> property.
        /// </summary>
        [TestMethod("Property Dimension")]
        public void Dimension()
        {
            // Arrange
            Frame frame = new Frame(new Point(2.0, 4.0, 6.0), new Vector(1.0, 2.0, 0.5), new Vector(-1.5, 2.5, 1.0), new Vector(-0.6, -0.4, 2.0));

            //Act
            int dimension = frame.Dimension;

            // Assert
            Assert.AreEqual(3, dimension);
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="Frame"/> from its three coordinates,
        /// and the <see cref="Frame.Origin"/>, <see cref="Frame.XAxis"/>, <see cref="Frame.YAxis"/>, <see cref="Frame.ZAxis"/> properties.
        /// </summary>
        [TestMethod("Property Origin & XAxis & YAxis & ZAxis")]
        public void OriginAndXAxisAndYAxisAndZAxis()
        {
            // Arrange
            Frame frame = new Frame(new Point(2.0, 4.0, 6.0), new Vector(1.0, 2.0, 0.5), new Vector(-1.5, 2.5, 1.0), new Vector(-0.6, -0.4, 2.0));

            Point oResult = new Point(2.0, 4.0, 6.0);
            Vector xResult = new Vector(1.0, 2.0, 0.5);
            Vector yResult = new Vector(-1.5, 2.5, 1.0);
            Vector zResult = new Vector(-0.6, -0.4, 2.0);

            //Act

            Point origin = frame.Origin;
            Vector xAxis = frame.XAxis;
            Vector yAxis = frame.YAxis;
            Vector zAxis = frame.ZAxis;

            // Assert
            Assert.IsTrue(origin.Equals(oResult));
            Assert.IsTrue(xAxis.Equals(xResult));
            Assert.IsTrue(yAxis.Equals(yResult));
            Assert.IsTrue(zAxis.Equals(zResult));
        }

        /// <summary>
        /// Tests the <see cref="Frame"/> indexer property.
        /// </summary>
        [TestMethod("Property this[int]")]
        public void Indexer()
        {
            // Arrange
            Frame frame = new Frame(new Point(2.0, 4.0, 6.0), new Vector(1.0, 2.0, 0.5), new Vector(-1.5, 2.5, 1.0), new Vector(-0.6, -0.4, 2.0));

            Vector xResult = new Vector(1.0, 2.0, 0.5);
            Vector yResult = new Vector(-1.5, 2.5, 1.0);
            Vector zResult = new Vector(-0.6, -0.4, 2.0);

            //Act
            Vector xAxis = frame[0];
            Vector yAxis = frame[1];
            Vector zAxis = frame[2];

            // Assert
            Assert.IsTrue(xAxis.Equals(xResult));
            Assert.IsTrue(yAxis.Equals(yResult));
            Assert.IsTrue(zAxis.Equals(zResult));
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Tests the initialisation of the <see cref="Frame"/> from its origin and three axes.
        /// </summary>
        [TestMethod("Constructor(Point,Vector,Vector,Vector)")]
        public void Constructor_Point_Vector_Vector_Vector()
        {
            // Arrange
            bool xyThrowsException = false;
            bool xzThrowsException = false;
            bool yzThrowsException = false;

            // Act
            Frame frame = new Frame(new Point(2.0, 4.0, 6.0), new Vector(1.0, 2.0, -0.5), new Vector(-1.5, 2.5, 1.0), new Vector(-0.6, -0.4, 2.0));

            try { Frame otherFrame = new Frame(new Point(2.0, 4.0, 6.0), new Vector(1.0, 2.0, -0.5), new Vector(-1.5, -3.0, 0.75), new Vector(-0.6, -0.4, 2.0)); }
            catch (ArgumentException) { xyThrowsException = true; }

            try { Frame otherFrame = new Frame(new Point(2.0, 4.0, 6.0), new Vector(1.0, 2.0, -0.5), new Vector(-1.5, 2.5, 1.0), new Vector(2.5, 5.0, -1.25)); }
            catch (ArgumentException) { xzThrowsException = true; }

            try { Frame otherFrame = new Frame(new Point(2.0, 4.0, 6.0), new Vector(1.0, 2.0, -0.5), new Vector(-1.5, 2.5, 1.0), new Vector(-0.75, 1.25, 0.5)); }
            catch (ArgumentException) { yzThrowsException = true; }

            // Assert
            Assert.IsTrue(xyThrowsException);
            Assert.IsTrue(xzThrowsException);
            Assert.IsTrue(yzThrowsException);
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="Frame"/> from its origin and axes.
        /// </summary>
        [TestMethod("Constructor(Point,Vector[])")]
        public void Constructor_Point_VectorArray()
        {
            // Arrange
            Frame result = new Frame(new Point(2.0, 4.0, 6.0), new Vector(1.0, 2.0, -0.5), new Vector(-1.5, 2.5, 1.0), new Vector(-0.6, -0.4, 2.0));
            bool throwsException = false;
            bool xyThrowsException = false;
            bool xzThrowsException = false;
            bool yzThrowsException = false;

            // Act
            Frame frame = new Frame(new Point(2.0, 4.0, 6.0), new Vector[3] { new Vector(1.0, 2.0, -0.5), new Vector(-1.5, 2.5, 1.0), new Vector(-0.6, -0.4, 2.0) });

            try { Frame otherFrame = new Frame(new Point(2.0, 4.0, 6.0), new Vector[2] { new Vector(1.0, 2.0, -0.5), new Vector(-1.5, 2.5, 1.0)}); }
            catch (RankException) { throwsException = true; }

            try { Frame otherFrame = new Frame(new Point(2.0, 4.0, 6.0), new Vector[3] { new Vector(1.0, 2.0, -0.5), new Vector(-1.5, -3.0, 0.75), new Vector(-0.6, -0.4, 2.0) }); }
            catch (ArgumentException) { xyThrowsException = true; }

            try { Frame otherFrame = new Frame(new Point(2.0, 4.0, 6.0), new Vector[3] { new Vector(1.0, 2.0, -0.5), new Vector(-1.5, 2.5, 1.0), new Vector(2.5, 5.0, -1.25) }); }
            catch (ArgumentException) { xzThrowsException = true; }

            try { Frame otherFrame = new Frame(new Point(2.0, 4.0, 6.0), new Vector[3] { new Vector(1.0, 2.0, -0.5), new Vector(-1.5, 2.5, 1.0), new Vector(-0.75, 1.25, 0.5) }); }
            catch (ArgumentException) { yzThrowsException = true; }

            // Assert
            Assert.IsTrue(frame.Equals(result));
            Assert.IsTrue(throwsException);
            Assert.IsTrue(xyThrowsException);
            Assert.IsTrue(xzThrowsException);
            Assert.IsTrue(yzThrowsException);
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="Frame"/> from another <see cref="Frame"/>.
        /// </summary>
        [TestMethod("Constructor(Frame)")]
        public void Constructor_Frame()
        {
            // Arrange
            Frame result = new Frame(new Point(2.0, 4.0, 6.0), new Vector(1.0, 2.0, -0.5), new Vector(-1.5, 2.5, 1.0), new Vector(-0.6, -0.4, 2.0));
            // Act
            Frame frame = new Frame(result);
            // Assert
            Assert.IsTrue(frame.Equals(result));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Tests the equality comparison of two <see cref="Frame"/>.
        /// </summary>
        [TestMethod("Method Equals(Frame)")]
        public void Equals_Frame()
        {
            // Arrange
            Frame frameA = new Frame(new Point(2.0, 4.0, 6.0), new Vector(1.0, 2.0, 0.5), new Vector(-1.5, 2.5, 1.0), new Vector(-0.6, -0.4, 2.0));
            Frame frameB = new Frame(new Point(2.0, 4.0, 6.0), new Vector(1.0, 2.0, 0.5), new Vector(-1.5, 2.5, 1.0), new Vector(-0.6, -0.4, 2.0));

            // Act
            bool areEqual = frameA.Equals(frameB);

            // Assert
            Assert.IsTrue(areEqual);
        }

        #endregion
    }
}
