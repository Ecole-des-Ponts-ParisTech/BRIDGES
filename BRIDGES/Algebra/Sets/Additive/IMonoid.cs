using System;

using BRIDGES.Algebra.Fundamentals;


namespace BRIDGES.Algebra.Sets.Additive
{
    /// <summary>
    /// Interface defining an additive monoid.
    /// </summary>
    /// <typeparam name="T"> Type of the elements in the additive set. </typeparam>
    public interface IMonoid<T> : ISemiGroup<T>, IZeroable<T>
        where T : IMonoid<T>
    {
        /* Nothing to do */
    }
}
