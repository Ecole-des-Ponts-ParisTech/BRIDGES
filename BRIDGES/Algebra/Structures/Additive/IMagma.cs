using System;

using BRIDGES.Algebra.Fundamentals;


namespace BRIDGES.Algebra.Structures.Additive
{
    /// <summary>
    /// Interface defining an additive magma.
    /// </summary>
    /// <typeparam name="T"> Type of the elements in the additive set. </typeparam>
    internal interface IMagma<T> : IAddable<T>
        where T : IMagma<T>
    {
        /* Nothing to do */
    }
}
