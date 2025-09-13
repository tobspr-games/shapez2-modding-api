namespace ShapezShifter.Hijack
{
    public static class AggregatedChain
    {
        public static IWaitAllRewirers WaitFor(RewirerChainLink rewirer)
        {
            return new WaitAllRewirers().And(rewirer);
        }
    }
}