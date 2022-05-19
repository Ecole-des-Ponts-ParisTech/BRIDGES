﻿using System;

using BRIDGES.Algebra.Fundamentals;


namespace BRIDGES.Algebra.Structures.Additive
{
    /// <summary>
    /// Interface defining an additive quasi-group.
    /// </summary>
    /// <typeparam name="T"> Type of the elements in the additive set. </typeparam>
    internal interface IQuasiGroup<T> : IMagma<T>, ISubtractable<T>
        where T : IQuasiGroup<T>
    {
        /* Nothing to do */
    }
}