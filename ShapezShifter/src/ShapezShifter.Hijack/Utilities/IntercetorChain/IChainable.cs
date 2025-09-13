using Core.Events;

namespace ShapezShifter.Hijack
{
    public interface IChainable<out T>
    {
        public IEvent<T> AfterHijack { get; }
    }

    public interface IChainable
    {
        public IEvent AfterHijack { get; }
    }
}