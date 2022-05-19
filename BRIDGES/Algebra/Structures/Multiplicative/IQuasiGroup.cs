using System;

using BRIDGES.Algebra.Fundamentals;


namespace BRIDGES.Algebra.Structures.Multiplicative
{
    /// <summary>
    /// Interface defining a multiplicative quasi-group.
    /// </summary>
    /// <typeparam name="T"> Type of the elements in the multiplicative set. </typeparam>
    internal interface IQuasiGroup<T> : IMagma<T>, IDivisible<T>
        where T : IQuasiGroup<T>
    {
        /* Nothing to do */
    }
}