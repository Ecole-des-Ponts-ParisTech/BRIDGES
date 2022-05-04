using System;


namespace BRIDGES.Algebra.Structures
{
    internal interface IField<T> : Additive.IAbelianGroup<T>, Multiplicative.IAbelianGroup<T>
        where T : IField<T>
    {
        /* Nothing to do */
    }
}
