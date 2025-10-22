namespace ShapezShifter.Hijack
{
    public interface IChainableRewirer : IChainable, IRewirer
    {
    }

    public interface IChainableRewirer<out T> : IChainable<T>, IRewirer
    {
    }
}