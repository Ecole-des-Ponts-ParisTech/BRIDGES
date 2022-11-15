using System;

using BRIDGES.Algebra.Fundamentals;


namespace BRIDGES.Algebra.Sets.Multiplicative
{
    /// <summary>
    /// Interface defining methods to manipulate elements in a multiplicative quasi-group.
    /// </summary>
    /// <typeparam name="T"> Type of the elements in the multiplicative quasi-group. </typeparam>
    public interface IQuasiGroup<T> : IMagma<T>, IDivisible<T>
        where T : IQuasiGroup<T>
    {
        /* Do Nothing */
    }
}