using System;


namespace BRIDGES.Algebra.Sets.Multiplicative
{
    /// <summary>
    /// Interface defining methods to manipulate elements in a multiplicative group.
    /// </summary>
    /// <typeparam name="T"> Type of the elements in the multiplicative group. </typeparam>
    public interface IGroup<T> : IMonoid<T>, ILoop<T>
        where T : IGroup<T>
    {
        // In the C#8.0 language version, interfaces can have explicit implementations.
        // But being based netstandard2.0 (because of Rhino7 dependencies) capes the language version to C#7.3.

        // It is repeated to ensure that there is a most specific implementation of the in the interfaces.

        /*
        bool IAddable<T>.IsAssociative
        { 
            get { return true; } 
        }
        */

        #region Methods

        /// <summary>
        /// Replaces the current element with its inverse.
        /// </summary>
        /// <returns> <see langword="true"/> if the current element was inversed, <see langword="false"/> otherwise. </returns>
        bool Inverse();

        #endregion
    }
}
