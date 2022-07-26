using System;

using BRIDGES.Algebra.Fundamentals;


namespace BRIDGES.Algebra.Sets.Additive
{
    /// <summary>
    /// Interface defining an additive loop.
    /// </summary>
    /// <typeparam name="T"> Type of the elements in the additive set. </typeparam>
    public interface ILoop<T> : IQuasiGroup<T>, IZeroable<T>
        where T : ILoop<T>
    {
        /* Nothing to do */
    }
}
