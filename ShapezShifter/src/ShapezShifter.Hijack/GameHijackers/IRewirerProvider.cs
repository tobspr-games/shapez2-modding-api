using System.Collections.Generic;

namespace ShapezShifter.Hijack
{
    internal interface IRewirerProvider
    {
        IEnumerable<TRewirers> RewirersOfType<TRewirers>()
            where TRewirers : IRewirer;
    }
}