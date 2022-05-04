using System;

using BRIDGES.Algebra.Fundamentals;


namespace BRIDGES.Algebra.Structures.Multiplicative
{
    /// <summary>
    /// Interface defining a multiplicative monoid.
    /// </summary>
    /// <typeparam name="T"> Type of the elements in the multiplicative set. </typeparam>
    internal interface IMonoid<T> : ISemiGroup<T>, IOneable<T>
        where T : IMonoid<T>
    {
        /* Nothing to do */
    }
}
