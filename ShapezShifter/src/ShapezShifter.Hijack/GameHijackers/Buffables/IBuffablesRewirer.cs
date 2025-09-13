using System.Collections.Generic;

namespace ShapezShifter.Hijack
{
    public interface IBuffablesRewirer : IRewirer
    {
        ICollection<object> ModifyBuffables(ICollection<object> buffables);
    }
}