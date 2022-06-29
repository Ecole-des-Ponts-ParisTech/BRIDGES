using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.Geometry.Euclidean3D;


namespace BRIDGES.Test.Geometry.Euclidean3D
{
    /// <summary>
    /// Class testing the members of the <see cref="Basis"/> class.
    /// </summary>
    [TestClass]
    public class BasisTest
    {
        #region Behavior

        /// <summary>
        /// Tests that <see cref="Basis"/> is reference type.
        /// </summary>
        [TestMethod("Behavior IsReference")]
        public void IsReference()
        {
            // Arrange
            Basis basisA = new Basis(new Vector(1.0, 2.0, 0.5), new Vector(-1.5, 2.5, 1.0), new Vector(-0.6, -0.4, 2.0));
            Basis basisB = new Basis(new Vector(0.5,0.4, 2.0), new Vector(0.0, -2.0, 0.8), new Vector(3.2, 0.2, -0.4));
            //Act
            basisA = basisB;
            // Assert
            Assert.IsTrue(basisA.Equals(basisB));
            Assert.AreSame(basisA, basisB);
        }

        #endregion


        #region Properties

        /// <summary>
        /// Tests the <see cref="Basis.Dimension"/> property.
        /// </summary>
        [TestMethod("Property Dimension")]
        public void Dimension()
        {
            // Arrange
            Basis basis = new Basis(new Vector(1.0, 2.0, 0.5), new Vector(-1.5, 2.5, 1.0), new Vector(-0.6, -0.4, 2.0));

            //Act
            int dimension = basis.Dimension;

            // Assert
            Assert.AreEqual(3, dimension);
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="Basis"/> from its three coordinates,
        /// and the <see cref="Basis.XAxis"/>, <see cref="Basis.YAxis"/>, <see cref="Basis.ZAxis"/> properties.
        /// </summary>
        [TestMethod("Property Origin & XAxis & YAxis & ZAxis")]
        public void OriginAndXAxisAndYAxisAndZAxis()
        {
            // Basis
            Basis basis = new Basis(new Vector(1.0, 2.0, 0.5), new Vector(-1.5, 2.5, 1.0), new Vector(-0.6, -0.4, 2.0));

            Vector xResult = new Vector(1.0, 2.0, 0.5);
            Vector yResult = new Vector(-1.5, 2.5, 1.0);
            Vector zResult = new Vector(-0.6, -0.4, 2.0);

            //Act
            Vector xAxis = basis.XAxis;
            Vector yAxis = basis.YAxis;
            Vector zAxis = basis.ZAxis;

            // Assert
            Assert.IsTrue(xAxis.Equals(xResult));
            Assert.IsTrue(yAxis.Equals(yResult));
            Assert.IsTrue(zAxis.Equals(zResult));
        }

        /// <summary>
        /// Tests the <see cref="Basis"/> indexer property.
        /// </summary>
        [TestMethod("Property this[int]")]
        public void Indexer()
        {
            // Arrange
            Basis basis = new Basis(new Vector(1.0, 2.0, 0.5), new Vector(-1.5, 2.5, 1.0), new Vector(-0.6, -0.4, 2.0));

            Vector xResult = new Vector(1.0, 2.0, 0.5);
            Vector yResult = new Vector(-1.5, 2.5, 1.0);
            Vector zResult = new Vector(-0.6, -0.4, 2.0);

            //Act
            Vector xAxis = basis[0];
            Vector yAxis = basis[1];
            Vector zAxis = basis[2];

            // Assert
            Assert.IsTrue(xAxis.Equals(xResult));
            Assert.IsTrue(yAxis.Equals(yResult));
            Assert.IsTrue(zAxis.Equals(zResult));
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Tests the initialisation of the <see cref="Basis"/> from three axes.
        /// </summary>
        [TestMethod("Constructor(Vector,Vector,Vector)")]
        public void Constructor_Vector_Vector_Vector()
        {
            // Arrange
            bool xyThrowsException = false;
            bool xzThrowsException = false;
            bool yzThrowsException = false;

            // Act
            Basis basis = new Basis(new Vector(1.0, 2.0, -0.5), new Vector(-1.5, 2.5, 1.0), new Vector(-0.6, -0.4, 2.0));

            try { Basis otherBasis = new Basis(new Vector(1.0, 2.0, -0.5), new Vector(-1.5, -3.0, 0.75), new Vector(-0.6, -0.4, 2.0)); }
            catch (ArgumentException e) { xyThrowsException = true; }

            try { Basis otherBasis = new Basis(new Vector(1.0, 2.0, -0.5), new Vector(-1.5, 2.5, 1.0), new Vector(2.5, 5.0, -1.25)); }
            catch (ArgumentException e) { xzThrowsException = true; }

            try { Basis otherBasis = new Basis(new Vector(1.0, 2.0, -0.5), new Vector(-1.5, 2.5, 1.0), new Vector(-0.75, 1.25, 0.5)); }
            catch (ArgumentException e) { yzThrowsException = true; }

            // Assert
            Assert.IsTrue(xyThrowsException);
            Assert.IsTrue(xzThrowsException);
            Assert.IsTrue(yzThrowsException);
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="Basis"/> from its axes.
        /// </summary>
        [TestMethod("Constructor(Vector[])")]
        public void Constructor_VectorArray()
        {
            // Arrange
            Basis result = new Basis(new Vector(1.0, 2.0, -0.5), new Vector(-1.5, 2.5, 1.0), new Vector(-0.6, -0.4, 2.0));
            bool throwsException = false;
            bool xyThrowsException = false;
            bool xzThrowsException = false;
            bool yzThrowsException = false;

            // Act
            Basis basis = new Basis(new Vector[3] { new Vector(1.0, 2.0, -0.5), new Vector(-1.5, 2.5, 1.0), new Vector(-0.6, -0.4, 2.0) });

            try { Basis otherBasis = new Basis(new Vector[2] { new Vector(1.0, 2.0, -0.5), new Vector(-1.5, 2.5, 1.0) }); }
            catch (RankException e) { throwsException = true; }

            try { Basis otherBasis = new Basis(new Vector[3] { new Vector(1.0, 2.0, -0.5), new Vector(-1.5, -3.0, 0.75), new Vector(-0.6, -0.4, 2.0) }); }
            catch (ArgumentException e) { xyThrowsException = true; }

            try { Basis otherBasis = new Basis(new Vector[3] { new Vector(1.0, 2.0, -0.5), new Vector(-1.5, 2.5, 1.0), new Vector(2.5, 5.0, -1.25) }); }
            catch (ArgumentException e) { xzThrowsException = true; }

            try { Basis otherBasis = new Basis(new Vector[3] { new Vector(1.0, 2.0, -0.5), new Vector(-1.5, 2.5, 1.0), new Vector(-0.75, 1.25, 0.5) }); }
            catch (ArgumentException e) { yzThrowsException = true; }

            // Assert
            Assert.IsTrue(basis.Equals(result));
            Assert.IsTrue(throwsException);
            Assert.IsTrue(xyThrowsException);
            Assert.IsTrue(xzThrowsException);
            Assert.IsTrue(yzThrowsException);
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="Basis"/> from another <see cref="Basis"/>.
        /// </summary>
        [TestMethod("Constructor(Basis)")]
        public void Constructor_Basis()
        {
            // Arrange
            Basis result = new Basis(new Vector(1.0, 2.0, -0.5), new Vector(-1.5, 2.5, 1.0), new Vector(-0.6, -0.4, 2.0));

            // Act
            Basis basis = new Basis(result);

            // Assert
            Assert.IsTrue(basis.Equals(result));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Tests the equality comparison of two <see cref="Basis"/>.
        /// </summary>
        [TestMethod("Method Equals(Basis)")]
        public void Equals_Basis()
        {
            // Arrange
            Basis basisA = new Basis(new Vector(1.0, 2.0, 0.5), new Vector(-1.5, 2.5, 1.0), new Vector(-0.6, -0.4, 2.0));
            Basis BasisB = new Basis(new Vector(1.0, 2.0, 0.5), new Vector(-1.5, 2.5, 1.0), new Vector(-0.6, -0.4, 2.0));

            // Act
            bool areEqual = basisA.Equals(BasisB);

            // Assert
            Assert.IsTrue(areEqual);
        }

        #endregion
    }
}
