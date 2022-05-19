using System;

using BRIDGES.Algebra.Fundamentals;


namespace BRIDGES.Algebra.Structures.Multiplicative
{
    /// <summary>
    /// Interface defining a multiplicative magma.
    /// </summary>
    /// <typeparam name="T"> Type of the elements in the multiplicative set. </typeparam>
    internal interface IMagma<T> : IMultiplicable<T>
        where T : IMagma<T>
    {
        /* Nothing to do */
    }
}
