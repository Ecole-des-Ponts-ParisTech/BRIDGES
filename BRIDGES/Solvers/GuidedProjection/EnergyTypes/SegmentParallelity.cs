using System;
using System.Collections.Generic;

using BRIDGES.LinearAlgebra.Vectors;
using BRIDGES.Solvers.GuidedProjection.Interfaces;


namespace BRIDGES.Solvers.GuidedProjection.EnergyTypes
{
    /// <summary>
    /// Energy enforcing a segment defined from two point variables, <em>pi</em> and <em>pj</em>, to be parallel to a constant direction <em>v</em>.
    /// </summary>
    /// <remarks> 
    /// A scalar variable <em>l</em> identified as the segment length must be defined.<br/>
    /// The vector xReduced = [pi, pj, l].
    /// </remarks>
    public class SegmentParallelity : IEnergyType
    {
        #region Properties

        /// <inheritdoc cref="IEnergyType.LocalKi"/>
        public SparseVector LocalKi { get; }

        /// <inheritdoc cref="IEnergyType.Si"/>
        public double Si { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="SegmentParallelity"/> class by defining the coordinates of the target direction vector.
        /// </summary>
        /// <param name="coordinates"> Coordinates of the target direction vector. </param>
        public SegmentParallelity(double[] coordinates)
        {
            // Unitise the direction vector
            double length = 0.0;
            for (int i = 0; i < coordinates.Length; i++)
            {
                length += coordinates[i] * coordinates[i];
            }
            length = Math.Sqrt(length);

            if (length == 0.0)
            {
                throw new DivideByZeroException("The length of the target direction vector must be different than zero.");
            }

            for (int i = 0; i < coordinates.Length; i++)
            {
                coordinates[i] = coordinates[i] / length;
            }

            /******************** Define LocalKi ********************/

            Dictionary<int, double> component = new Dictionary<int, double>((2 * coordinates.Length) + 1);
            for (int i = 0; i < coordinates.Length; i++)
            {
                component.Add(i, -coordinates[i]);
                component.Add(coordinates.Length + i, coordinates[i]);
            }
            component.Add(2 * coordinates.Length, -1);

            LocalKi = new SparseVector((2 * coordinates.Length) + 1, ref component);

            /******************** Define Si ********************/
            Si = 0.0;
        }

        #endregion
    }
}
