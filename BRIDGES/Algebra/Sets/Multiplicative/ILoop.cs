using System;

using BRIDGES.Algebra.Fundamentals;


namespace BRIDGES.Algebra.Sets.Multiplicative
{
    /// <summary>
    /// Interface defining a multiplicative loop.
    /// </summary>
    /// <typeparam name="T"> Type of the elements in the multiplicative set. </typeparam>
    public interface ILoop<T> : IQuasiGroup<T>, IOneable<T>
        where T : ILoop<T>
    {
        /* Nothing to do */
    }
}
