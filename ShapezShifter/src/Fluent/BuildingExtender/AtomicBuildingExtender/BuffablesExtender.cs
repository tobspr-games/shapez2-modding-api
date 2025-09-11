using System.Collections.Generic;
using Core.Events;

namespace ShapezShifter.Fluent.Atomic
{
    public class BuffablesExtender<TBuffableConfig> : IBuffablesExtender,
        IChainableExtender
    {
        private readonly TBuffableConfig BuffableConfig;

        public BuffablesExtender(TBuffableConfig buffableConfig)
        {
            BuffableConfig = buffableConfig;
        }


        public IEvent AfterExtensionApplied => _AfterExtensionApplied;

        private readonly MultiRegisterEvent _AfterExtensionApplied =
            new();

        public ICollection<object> ExtendBuffables(ICollection<object> buffables)
        {
            buffables.Add(BuffableConfig);
            _AfterExtensionApplied.Invoke();
            return buffables;
        }
    }
}