using System;

using BRIDGES.Algebra.Fundamentals;


namespace BRIDGES.Algebra.Structures.Additive
{
    /// <summary>
    /// Interface defining an additive monoid.
    /// </summary>
    /// <typeparam name="T"> Type of the elements in the additive set. </typeparam>
    internal interface IMonoid<T> : ISemiGroup<T>, IZeroable<T>
        where T : IMonoid<T>
    {
        /* Nothing to do */
    }
}
