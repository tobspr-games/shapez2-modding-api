using System.Collections.Generic;
using Core.Events;
using ShapezShifter.Hijack;

namespace ShapezShifter.Flow.Atomic
{
    public class BuffablesExtender<TBuffableConfig> : IBuffablesRewirer,
        IChainableRewirer
    {
        private readonly TBuffableConfig BuffableConfig;

        public BuffablesExtender(TBuffableConfig buffableConfig)
        {
            BuffableConfig = buffableConfig;
        }


        public IEvent AfterHijack => _AfterExtensionApplied;

        private readonly MultiRegisterEvent _AfterExtensionApplied =
            new();

        public ICollection<object> ModifyBuffables(ICollection<object> buffables)
        {
            buffables.Add(BuffableConfig);
            _AfterExtensionApplied.Invoke();
            return buffables;
        }
    }
}