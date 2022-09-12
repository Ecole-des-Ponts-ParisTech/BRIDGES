using System;

using BRIDGES.Algebra.Fundamentals;


namespace BRIDGES.Algebra.Sets.Additive
{
    /// <summary>
    /// Interface defining methods to manipulate elements in an additive quasi-group.
    /// </summary>
    /// <typeparam name="T"> Type of the elements in the additive quasi-group. </typeparam>
    public interface IQuasiGroup<T> : IMagma<T>, ISubtractable<T>
        where T : IQuasiGroup<T>
    {
        /* Do Nothing */
    }
}