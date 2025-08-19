using System.Collections.Generic;

namespace ShapezShifter
{
    public interface IBuffablesExtender : IExtender
    {
        ICollection<object> ExtendBuffables(ICollection<object> buffables);
    }
}