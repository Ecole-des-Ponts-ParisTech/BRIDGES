using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.Arithmetic.Polynomials.Specials;


namespace BRIDGES.Test.Arithmetic.Polynomials.Specials
{
    /// <summary>
    /// Class testing the members of the <see cref="BSpline"/> structure.
    /// </summary>
    [TestClass]
    public class BSplineTest
    {
        #region Behavior

        /// <summary>
        /// Tests that <see cref="BSpline"/> is not reference type.
        /// </summary>
        [TestMethod("Behavior IsReference")]
        public void IsReference()
        {
            // Arrange
            BSpline bSplineA = new BSpline(2, 0, 2, new double[6] { 0, 0, 0, 1, 1, 1 });
            BSpline bSplineB = new BSpline(2, 1, 2, new double[6] { 0, 0, 0, 1, 1, 1 });

            //Act
            bSplineA = bSplineB;

            // Assert
            Assert.IsTrue(bSplineA.Equals(bSplineB));
            Assert.AreSame(bSplineA, bSplineB);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Tests the initialisation of the <see cref="BSpline"/> from its index, degree and associated knot vector.
        /// </summary>
        [DataTestMethod()]
        [DataRow(2, 0, new double[3] { 1.0, -2.0, 1.0 }, DisplayName = "N_{0,2} on 0 <= u < 1")]
        [DataRow(2, 1, new double[3] { 0.0, 2.0, -3.0 / 2.0 }, DisplayName = "N_{1,2} on 0 <= u < 1")]
        [DataRow(3, 1, new double[3] { 2.0, -2.0, 1.0 / 2.0 }, DisplayName = "N_{1,2} on 1 <= u < 2")]
        [DataRow(2, 2, new double[3] { 0.0, 0.0, 1.0 / 2.0 }, DisplayName = "N_{2,2} on 0 <= u < 1")]
        [DataRow(3, 2, new double[3] { -3.0 / 2.0, 3.0, -1.0 }, DisplayName = "N_{2,2} on 1 <= u < 2")]
        [DataRow(4, 2, new double[3] { 9.0 / 2.0, -3.0, 1.0 / 2.0 }, DisplayName = "N_{2,2} on 2 <= u < 3")]
        [DataRow(3, 3, new double[3] { 1.0 / 2.0, -1.0, 1.0 / 2.0 }, DisplayName = "N_{3,2} on 1 <= u < 2")]
        [DataRow(4, 3, new double[3] { -11.0 / 2.0, 5.0, -1.0 }, DisplayName = "N_{3,2} on 2 <= u < 3")]
        [DataRow(5, 3, new double[3] { 8.0, -4.0, 1.0 / 2.0 }, DisplayName = "N_{3,2} on 3 <= u < 4")]
        [DataRow(4, 4, new double[3] { 2.0, -2.0, 1.0 / 2.0 }, DisplayName = "N_{4,2} on 2 <= u < 3")]
        [DataRow(5, 4, new double[3] { -16.0, 10.0, -3.0 / 2.0 }, DisplayName = "N_{4,2} on 3 <= u < 4")]
        [DataRow(5, 5, new double[3] { 9.0, -6.0, 1.0 }, DisplayName = "N_{5,2} on 3 <= u < 4")]
        [DataRow(7, 5, new double[3] { 25.0, -10.0, 1.0 }, DisplayName = "N_{5,2} on 4 <= u < 5")]
        [DataRow(7, 6, new double[3] { -40.0, 18.0, -2.0 }, DisplayName = "N_{6,2} on 4 <= u < 5")]
        [DataRow(7, 7, new double[3] { 16.0, -8.0, 1.0 }, DisplayName = "N_{7,2} on 4 <= u < 5")]
        public void Constructor_Int_Int_DoubleArray(int spanIndex, int index, double[] expectedCoef)
        {
            // Act
            BSpline bSpline = new BSpline(spanIndex, index, 2, new double[11] { 0, 0, 0, 1, 2, 3, 4, 4, 5, 5, 5 });

            // Assert
            int coefCount = expectedCoef.Length;
            Assert.AreEqual(coefCount, bSpline.Degree + 1);

            for (int i_C = 0; i_C < coefCount; i_C++)
            {
                Assert.AreEqual(bSpline[i_C], expectedCoef[i_C], Settings.AbsolutePrecision);
            }
        }

        #endregion
    }
}
