using System;
using System.Collections.Generic;

using BRIDGES.LinearAlgebra.Matrices.Storage;

using BRIDGES.Solvers.GuidedProjection.Interfaces;


namespace BRIDGES.Solvers.GuidedProjection.QuadraticConstraints
{
    /// <summary>
    /// Constraint enforcing a segment defined from two point variables, <em>pi</em> and <em>pj</em>, to be orthogonal to a given direction <em>v</em>.
    /// </summary>
    /// <remarks> The vector xReduced = [pi, pj].</remarks>
    public class SegmentOrthogonality : IEnergyType
    {
        #region Properties

        /// <inheritdoc cref="IEnergyType.LocalKi"/>
        public Dictionary<int, double> LocalKi { get; }

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
            // Initialisation of Ki
            LocalKi = new Dictionary<int, double>((2 * coordinates.Length));
            for (int i = 0; i < coordinates.Length; i++)
            {
                LocalKi.Add(i, -coordinates[i]);
                LocalKi.Add(coordinates.Length + i, coordinates[i]);
            }

            // Initialisation of Si
            Si = 0.0;
        }

        #endregion
    }
}
