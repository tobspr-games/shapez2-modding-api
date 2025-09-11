using System.Collections.Generic;
using Core.Events;

namespace ShapezShifter
{
    public static class AggregatedChain
    {
        public static IWaitAllExtenders WaitFor(ExtenderChainLink extender)
        {
            return new WaitAllExtenders().And(extender);
        }
    }

    public class WaitAllExtenders : IWaitAllExtenders
    {
        private readonly HashSet<int> Links = new();

        private int LastIndex;

        public IWaitAllExtenders And(ExtenderChainLink chainableExtender)
        {
            int index = LastIndex;
            chainableExtender.OnPropagate.Register(ClearLink);
            Links.Add(index);
            LastIndex++;
            return this;

            void ClearLink()
            {
                Links.Remove(index);
                InvokeCallbackIfAllExtendersHaveBeenApplied();
            }
        }

        public IWaitAllExtenders And<T>(ExtenderChainLink<T> chainableExtender)
        {
            int index = LastIndex;
            chainableExtender.OnPropagate.Register(ClearLink);
            Links.Add(index);
            LastIndex++;

            return this;

            void ClearLink(T t)
            {
                Links.Remove(index);
                InvokeCallbackIfAllExtendersHaveBeenApplied();
            }
        }

        private void InvokeCallbackIfAllExtendersHaveBeenApplied()
        {
            if (Links.Count == 0)
            {
                _AfterExtensionApplied.Invoke();
            }
        }

        public IEvent AfterExtensionApplied => _AfterExtensionApplied;
        private readonly MultiRegisterEvent _AfterExtensionApplied = new();
    }

    public interface IWaitAllExtenders : IChainable
    {
        IWaitAllExtenders And(ExtenderChainLink chainableExtender);
        IWaitAllExtenders And<T>(ExtenderChainLink<T> chainableExtender);
    }
}