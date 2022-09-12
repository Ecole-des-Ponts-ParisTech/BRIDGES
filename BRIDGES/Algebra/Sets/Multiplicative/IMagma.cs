using System;

using BRIDGES.Algebra.Fundamentals;


namespace BRIDGES.Algebra.Sets.Multiplicative
{
    /// <summary>
    /// Interface defining methods to manipulate elements in a multiplicative magma.
    /// </summary>
    /// <typeparam name="T"> Type of the elements in the multiplicative magma. </typeparam>
    public interface IMagma<T> : IMultiplicable<T>
        where T : IMagma<T>
    {
        /* Do Nothing */
    }
}
