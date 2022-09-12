﻿using System;


namespace BRIDGES.Algebra.Sets.Multiplicative
{
    /// <summary>
    /// Interface defining methods to manipulate elements in a multiplicative semi-group.
    /// </summary>
    /// <typeparam name="T"> Type of the elements in the multiplicative semi-group. </typeparam>
    public interface ISemiGroup<T> : IMagma<T>
        where T : ISemiGroup<T>
    {
        // In the C#8.0 language version, interfaces can have explicit implementations.
        // But being based netstandard2.0 (because of Rhino7 dependencies) capes the language version to C#7.3.

        /*
        bool IAddable<T>.IsAssociative
        { 
            get { return true; } 
        }
        */
    }
}
