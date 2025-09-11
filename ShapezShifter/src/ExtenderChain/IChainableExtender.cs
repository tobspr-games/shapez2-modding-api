using Core.Events;

namespace ShapezShifter
{
    public interface IChainableExtender : IChainable, IExtender
    {
    }

    public interface IChainableExtender<out T> : IChainable<T>, IExtender
    {
    }

    public interface IChainable
    {
        public IEvent AfterExtensionApplied { get; }
    }

    public interface IChainable<out T>
    {
        public IEvent<T> AfterExtensionApplied { get; }
    }
}