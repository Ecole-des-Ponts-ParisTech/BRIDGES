using System;

using BRIDGES.Algebra.Fundamentals;


namespace BRIDGES.Algebra.Sets.Multiplicative
{
    /// <summary>
    /// Interface defining methods to manipulate elements in a multiplicative monoid.
    /// </summary>
    /// <typeparam name="T"> Type of the elements in the multiplicative monoid. </typeparam>
    public interface IMonoid<T> : ISemiGroup<T>, IOneable<T>
        where T : IMonoid<T>
    {
        /* Do Nothing */
    }
}
