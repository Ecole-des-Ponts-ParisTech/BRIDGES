﻿using System;


namespace BRIDGES.Algebra.Structures.Multiplicative
{
    /// <summary>
    /// Interface defining a multiplicative abelian group.
    /// </summary>
    /// <typeparam name="T"> Type of the elements in the multiplicative set. </typeparam>
    internal interface IAbelianGroup<T> : IGroup<T>
        where T : IAbelianGroup<T>
    {
        // In the C#8.0 language version, interfaces can have explicit implementations.
        // But being based netstandard2.0 (because of Rhino7 dependencies) capes the language version to C#7.3.

        /*
        bool IAddable<T>.IsCommutative
        { 
            get { return true; } 
        }
        */
    }
}
