using System;
using Core.Events;

namespace ShapezShifter
{
    public static class ExtenderChain
    {
        public static ExtenderChainLink BeginExtendingWith(IChainableExtender extender)
        {
            var handle = ShapezExtensions.AddExtender(extender);
            extender.AfterExtensionApplied.Register(RemoveExtender);
            return new ExtenderChainLink(extender.AfterExtensionApplied);

            void RemoveExtender()
            {
                ShapezExtensions.RemoveExtender(handle);
            }
        }


        public static ExtenderChainLink<TPropagatedData> BeginExtendingWith<TPropagatedData>(
            IChainableExtender<TPropagatedData> extender)
        {
            var handle = ShapezExtensions.AddExtender(extender);
            extender.AfterExtensionApplied.Register(RemoveExtender);

            return new ExtenderChainLink<TPropagatedData>(extender.AfterExtensionApplied);

            void RemoveExtender(TPropagatedData obj)
            {
                ShapezExtensions.RemoveExtender(handle);
            }
        }
    }

    public class ExtenderChainLink
    {
        public readonly IEvent OnPropagate;

        public ExtenderChainLink(IEvent onPropagate)
        {
            OnPropagate = onPropagate;
        }

        public ExtenderChainLink ThenContinueWith(Func<IChainable> chainableFactory)
        {
            var chainEvt = new MultiRegisterEvent();
            OnPropagate.Register(AfterExtended);
            return new ExtenderChainLink(chainEvt);

            void AfterExtended()
            {
                OnPropagate.Unregister(AfterExtended);
                var extender = chainableFactory.Invoke();
                extender.AfterExtensionApplied.Register(chainEvt.Invoke);
            }
        }

        public ExtenderChainLink<TPropagatedData> ThenContinueWith<TChainable, TPropagatedData>(
            Func<TChainable> extenderFactory)
            where TChainable : IChainable<TPropagatedData>
        {
            var chainEvt = new MultiRegisterEvent<TPropagatedData>();
            OnPropagate.Register(AfterExtended);
            return new ExtenderChainLink<TPropagatedData>(chainEvt);

            void AfterExtended()
            {
                OnPropagate.Unregister(AfterExtended);
                var extender = extenderFactory.Invoke();

                extender.AfterExtensionApplied.Register(chainEvt.Invoke);
            }
        }

        public ExtenderChainLink ThenContinueExtendingWith(Func<IChainableExtender> extenderFactory)
        {
            var chainEvt = new MultiRegisterEvent();
            OnPropagate.Register(AfterExtended);
            return new ExtenderChainLink(chainEvt);

            void AfterExtended()
            {
                OnPropagate.Unregister(AfterExtended);
                var extender = extenderFactory.Invoke();
                var handle = ShapezExtensions.AddExtender(extender);
                extender.AfterExtensionApplied.Register(chainEvt.Invoke);
                extender.AfterExtensionApplied.Register(RemoveExtender);
                return;

                void RemoveExtender()
                {
                    ShapezExtensions.RemoveExtender(handle);
                }
            }
        }

        public ExtenderChainLink<TPropagatedData> ThenContinueExtendingWith<TPropagatedData>(
            Func<IChainableExtender<TPropagatedData>> extenderFactory)
        {
            var chainEvt = new MultiRegisterEvent<TPropagatedData>();
            OnPropagate.Register(AfterExtended);
            return new ExtenderChainLink<TPropagatedData>(chainEvt);

            void AfterExtended()
            {
                OnPropagate.Unregister(AfterExtended);
                var extender = extenderFactory.Invoke();
                var handle = ShapezExtensions.AddExtender(extender);

                extender.AfterExtensionApplied.Register(chainEvt.Invoke);
                extender.AfterExtensionApplied.Register(RemoveExtender);
                return;

                void RemoveExtender(TPropagatedData _)
                {
                    ShapezExtensions.RemoveExtender(handle);
                }
            }
        }
    }

    public class ExtenderChainLink<TPropagatedData>
    {
        public readonly IEvent<TPropagatedData> OnPropagate;

        public ExtenderChainLink(IEvent<TPropagatedData> onPropagate)
        {
            OnPropagate = onPropagate;
        }

        public ExtenderChainLink ThenContinueWith(Func<TPropagatedData, IChainable> extenderFactory)
        {
            var chainEvt = new MultiRegisterEvent();
            OnPropagate.Register(AfterExtended);
            return new ExtenderChainLink(chainEvt);

            void AfterExtended(TPropagatedData propagatedData)
            {
                OnPropagate.Unregister(AfterExtended);
                var extender = extenderFactory.Invoke(propagatedData);
                extender.AfterExtensionApplied.Register(chainEvt.Invoke);
            }
        }

        public ExtenderChainLink<TNextPropagatedData> ThenContinueWith<TNextPropagatedData>(
            Func<TPropagatedData, IChainable<TNextPropagatedData>> extenderFactory)
        {
            var chainEvt = new MultiRegisterEvent<TNextPropagatedData>();
            OnPropagate.Register(AfterExtended);
            return new ExtenderChainLink<TNextPropagatedData>(chainEvt);

            void AfterExtended(TPropagatedData propagatedData)
            {
                OnPropagate.Unregister(AfterExtended);
                var extender = extenderFactory.Invoke(propagatedData);
                extender.AfterExtensionApplied.Register(chainEvt.Invoke);
            }
        }

        public ExtenderChainLink ThenContinueExtendingWith(
            Func<TPropagatedData, IChainableExtender> extenderFactory)
        {
            var chainEvt = new MultiRegisterEvent();
            OnPropagate.Register(AfterExtended);

            return new ExtenderChainLink(chainEvt);

            void AfterExtended(TPropagatedData propagatedData)
            {
                OnPropagate.Unregister(AfterExtended);
                var extender = extenderFactory.Invoke(propagatedData);
                var handle = ShapezExtensions.AddExtender(extender);

                extender.AfterExtensionApplied.Register(chainEvt.Invoke);
                extender.AfterExtensionApplied.Register(RemoveExtender);
                return;

                void RemoveExtender()
                {
                    ShapezExtensions.RemoveExtender(handle);
                }
            }
        }

        public ExtenderChainLink<TNextPropagatedData> ThenContinueExtendingWith<TNextPropagatedData>(
            Func<TPropagatedData, IChainableExtender<TNextPropagatedData>> extenderFactory)
        {
            var chainEvt = new MultiRegisterEvent<TNextPropagatedData>();
            OnPropagate.Register(AfterExtended);
            return new ExtenderChainLink<TNextPropagatedData>(chainEvt);

            void AfterExtended(TPropagatedData propagatedData)
            {
                OnPropagate.Unregister(AfterExtended);
                var extender = extenderFactory.Invoke(propagatedData);
                var handle = ShapezExtensions.AddExtender(extender);
                extender.AfterExtensionApplied.Register(chainEvt.Invoke);
                extender.AfterExtensionApplied.Register(RemoveExtender);
                return;

                void RemoveExtender(TNextPropagatedData _)
                {
                    ShapezExtensions.RemoveExtender(handle);
                }
            }
        }
    }
}