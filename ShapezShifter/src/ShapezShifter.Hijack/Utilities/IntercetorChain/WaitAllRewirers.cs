using System.Collections.Generic;
using Core.Events;

namespace ShapezShifter.Hijack
{
    public class WaitAllRewirers : IWaitAllRewirers
    {
        private readonly HashSet<int> Links = new();

        private int LastIndex;

        public IWaitAllRewirers And(RewirerChainLink chainableRewirer)
        {
            int index = LastIndex;
            chainableRewirer.OnPropagate.Register(ClearLink);
            Links.Add(index);
            LastIndex++;
            return this;

            void ClearLink()
            {
                Links.Remove(index);
                InvokeCallbackIfAllExtendersHaveBeenApplied();
            }
        }

        public IWaitAllRewirers And<T>(RewirerChainLink<T> chainableRewirer)
        {
            int index = LastIndex;
            chainableRewirer.OnPropagate.Register(ClearLink);
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

        public IEvent AfterHijack => _AfterExtensionApplied;
        private readonly MultiRegisterEvent _AfterExtensionApplied = new();
    }

    public interface IWaitAllRewirers : IChainable
    {
        IWaitAllRewirers And(RewirerChainLink chainableRewirer);
        IWaitAllRewirers And<T>(RewirerChainLink<T> chainableRewirer);
    }
}