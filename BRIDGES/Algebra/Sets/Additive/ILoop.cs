using System;

using BRIDGES.Algebra.Fundamentals;


namespace BRIDGES.Algebra.Sets.Additive
{
    /// <summary>
    /// Interface defining methods to manipulate elements in an additive loop.
    /// </summary>
    /// <typeparam name="T"> Type of the elements in the additive loop. </typeparam>
    public interface ILoop<T> : IQuasiGroup<T>, IZeroable<T>
        where T : ILoop<T>
    {
        /* Do Nothing */
    }
}
