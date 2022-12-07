using System;
using System.Collections.Generic;
using System.Text;

namespace BRIDGES.Geometry.Kernel
{
    /// <summary>
    /// Interface defining a generalized method for determining geometric equality between two instances.
    /// </summary>
    /// <typeparam name="T"> The type of objects to compare with. </typeparam>
    public interface IGeometricallyEquatable<T>
    {
        /// <summary>
        /// Evaluates whether the current object is geometrically equal to the given <typeparamref name="T"/>.
        /// </summary>
        /// <param name="other"> The object to compare with. </param>
        /// <returns> <see langword="true"/> if the two objects are geometrically equal, <see langword="false"/> otherwise.</returns>
        bool GeometricallyEquals(T other);
    }
}
