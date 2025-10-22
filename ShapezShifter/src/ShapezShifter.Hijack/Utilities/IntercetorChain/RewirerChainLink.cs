using System;
using Core.Events;

namespace ShapezShifter.Hijack
{
    public class RewirerChainLink<TPropagatedData>
    {
        public readonly IEvent<TPropagatedData> OnPropagate;

        public RewirerChainLink(IEvent<TPropagatedData> onPropagate)
        {
            OnPropagate = onPropagate;
        }

        public RewirerChainLink ThenContinueWith(Func<TPropagatedData, IChainable> interceptorFactory)
        {
            MultiRegisterEvent chainEvt = new();
            OnPropagate.Register(AfterIntercepted);
            return new RewirerChainLink(chainEvt);

            void AfterIntercepted(TPropagatedData propagatedData)
            {
                OnPropagate.Unregister(AfterIntercepted);
                IChainable extender = interceptorFactory.Invoke(propagatedData);
                extender.AfterHijack.Register(chainEvt.Invoke);
            }
        }

        public RewirerChainLink<TNextPropagatedData> ThenContinueWith<TNextPropagatedData>(
            Func<TPropagatedData, IChainable<TNextPropagatedData>> interceptorFactory)
        {
            var chainEvt = new MultiRegisterEvent<TNextPropagatedData>();
            OnPropagate.Register(AfterIntercepted);
            return new RewirerChainLink<TNextPropagatedData>(chainEvt);

            void AfterIntercepted(TPropagatedData propagatedData)
            {
                OnPropagate.Unregister(AfterIntercepted);
                IChainable<TNextPropagatedData> extender = interceptorFactory.Invoke(propagatedData);
                extender.AfterHijack.Register(chainEvt.Invoke);
            }
        }

        public RewirerChainLink ThenContinueRewiringWith(
            Func<TPropagatedData, IChainableRewirer> interceptorFactory)
        {
            MultiRegisterEvent chainEvt = new();
            OnPropagate.Register(AfterIntercepted);

            return new RewirerChainLink(chainEvt);

            void AfterIntercepted(TPropagatedData propagatedData)
            {
                OnPropagate.Unregister(AfterIntercepted);
                IChainableRewirer rewirer = interceptorFactory.Invoke(propagatedData);
                RewirerHandle handle = GameRewirers.AddRewirer(rewirer);

                rewirer.AfterHijack.Register(chainEvt.Invoke);
                rewirer.AfterHijack.Register(RemoveExtender);
                return;

                void RemoveExtender()
                {
                    GameRewirers.RemoveRewirer(handle);
                }
            }
        }

        public RewirerChainLink<TNextPropagatedData> ThenContinueRewiringWith<TNextPropagatedData>(
            Func<TPropagatedData, IChainableRewirer<TNextPropagatedData>> interceptorFactory)
        {
            var chainEvt = new MultiRegisterEvent<TNextPropagatedData>();
            OnPropagate.Register(AfterIntercepted);
            return new RewirerChainLink<TNextPropagatedData>(chainEvt);

            void AfterIntercepted(TPropagatedData propagatedData)
            {
                OnPropagate.Unregister(AfterIntercepted);
                IChainableRewirer<TNextPropagatedData> rewirer = interceptorFactory.Invoke(propagatedData);
                RewirerHandle handle = GameRewirers.AddRewirer(rewirer);
                rewirer.AfterHijack.Register(chainEvt.Invoke);
                rewirer.AfterHijack.Register(RemoveExtender);
                return;

                void RemoveExtender(TNextPropagatedData _)
                {
                    GameRewirers.RemoveRewirer(handle);
                }
            }
        }
    }

    public class RewirerChainLink
    {
        public readonly IEvent OnPropagate;

        public RewirerChainLink(IEvent onPropagate)
        {
            OnPropagate = onPropagate;
        }

        public RewirerChainLink ThenContinueWith(Func<IChainable> chainableFactory)
        {
            MultiRegisterEvent chainEvt = new();
            OnPropagate.Register(AfterIntercepted);
            return new RewirerChainLink(chainEvt);

            void AfterIntercepted()
            {
                OnPropagate.Unregister(AfterIntercepted);
                IChainable extender = chainableFactory.Invoke();
                extender.AfterHijack.Register(chainEvt.Invoke);
            }
        }

        public RewirerChainLink<TPropagatedData> ThenContinueWith<TChainable, TPropagatedData>(
            Func<TChainable> interceptorFactory)
            where TChainable : IChainable<TPropagatedData>
        {
            var chainEvt = new MultiRegisterEvent<TPropagatedData>();
            OnPropagate.Register(AfterIntercepted);
            return new RewirerChainLink<TPropagatedData>(chainEvt);

            void AfterIntercepted()
            {
                OnPropagate.Unregister(AfterIntercepted);
                TChainable chainable = interceptorFactory.Invoke();

                chainable.AfterHijack.Register(chainEvt.Invoke);
            }
        }

        public RewirerChainLink ThenContinueRewiringWith(Func<IChainableRewirer> interceptorFactory)
        {
            MultiRegisterEvent chainEvt = new();
            OnPropagate.Register(AfterIntercepted);
            return new RewirerChainLink(chainEvt);

            void AfterIntercepted()
            {
                OnPropagate.Unregister(AfterIntercepted);
                IChainableRewirer rewirer = interceptorFactory.Invoke();
                RewirerHandle handle = GameRewirers.AddRewirer(rewirer);
                rewirer.AfterHijack.Register(chainEvt.Invoke);
                rewirer.AfterHijack.Register(RemoveExtender);
                return;

                void RemoveExtender()
                {
                    GameRewirers.RemoveRewirer(handle);
                }
            }
        }

        public RewirerChainLink<TPropagatedData> ThenContinueRewiringWith<TPropagatedData>(
            Func<IChainableRewirer<TPropagatedData>> interceptorFactory)
        {
            var chainEvt = new MultiRegisterEvent<TPropagatedData>();
            OnPropagate.Register(AfterIntercepted);
            return new RewirerChainLink<TPropagatedData>(chainEvt);

            void AfterIntercepted()
            {
                OnPropagate.Unregister(AfterIntercepted);
                IChainableRewirer<TPropagatedData> rewirer = interceptorFactory.Invoke();
                RewirerHandle handle = GameRewirers.AddRewirer(rewirer);

                rewirer.AfterHijack.Register(chainEvt.Invoke);
                rewirer.AfterHijack.Register(RemoveExtender);
                return;

                void RemoveExtender(TPropagatedData _)
                {
                    GameRewirers.RemoveRewirer(handle);
                }
            }
        }
    }
}