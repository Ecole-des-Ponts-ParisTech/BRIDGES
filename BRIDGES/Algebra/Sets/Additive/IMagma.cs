using System;

using BRIDGES.Algebra.Fundamentals;


namespace BRIDGES.Algebra.Sets.Additive
{
    /// <summary>
    /// Interface defining methods to manipulate elements in an additive magma.
    /// </summary>
    /// <typeparam name="T"> Type of the elements in the additive magma. </typeparam>
    public interface IMagma<T> : IAddable<T>
        where T : IMagma<T>
    {
        /* Do Nothing */
    }
}
