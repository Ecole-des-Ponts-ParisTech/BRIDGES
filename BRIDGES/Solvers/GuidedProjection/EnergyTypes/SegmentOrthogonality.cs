using System;
using System.Collections.Generic;

using BRIDGES.LinearAlgebra.Vectors;
using BRIDGES.Solvers.GuidedProjection.Interfaces;


namespace BRIDGES.Solvers.GuidedProjection.EnergyTypes
{
    /// <summary>
    /// Energy enforcing a segment defined from two point variables, <em>pi</em> and <em>pj</em>, to be orthogonal to a constant direction <em>v</em>.
    /// </summary>
    /// <remarks> The vector xReduced = [pi, pj].</remarks>
    public class SegmentOrthogonality : IEnergyType
    {
        #region Properties

        /// <inheritdoc cref="IEnergyType.LocalKi"/>
        public SparseVector LocalKi { get; }

        /// <inheritdoc cref="IEnergyType.Si"/>
        public double Si { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="SegmentOrthogonality"/> class.
        /// </summary>
        /// <param name="coordinates"> Coordinates of the target direction vector. </param>
        public SegmentOrthogonality(double[] coordinates)
        {
            /******************** Define LocalKi ********************/

            Dictionary<int, double> components = new Dictionary<int, double>((2 * coordinates.Length));
            for (int i = 0; i < coordinates.Length; i++)
            {
                components.Add(i, -coordinates[i]);
                components.Add(coordinates.Length + i, coordinates[i]);
            }

            LocalKi = new SparseVector(2 * coordinates.Length, ref components);


            /******************** Define Si ********************/

            Si = 0.0;
        }

        #endregion
    }
}
