using System;

using BRIDGES.Algebra.Fundamentals;


namespace BRIDGES.Algebra.Sets.Additive
{
    /// <summary>
    /// Interface defining an additive magma.
    /// </summary>
    /// <typeparam name="T"> Type of the elements in the additive set. </typeparam>
    public interface IMagma<T> : IAddable<T>
        where T : IMagma<T>
    {
        /* Nothing to do */
    }
}
