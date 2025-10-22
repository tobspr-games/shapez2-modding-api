namespace ShapezShifter.Hijack
{
    public static class RewirerChain
    {
        public static RewirerChainLink BeginRewiringWith(IChainableRewirer rewirer)
        {
            RewirerHandle handle = GameRewirers.AddRewirer(rewirer);
            rewirer.AfterHijack.Register(RemoveInterceptor);
            return new RewirerChainLink(rewirer.AfterHijack);

            void RemoveInterceptor()
            {
                GameRewirers.RemoveRewirer(handle);
            }
        }


        public static RewirerChainLink<TPropagatedData> BeginRewiringWith<TPropagatedData>(
            IChainableRewirer<TPropagatedData> rewirer)
        {
            RewirerHandle handle = GameRewirers.AddRewirer(rewirer);
            rewirer.AfterHijack.Register(RemoveInterceptor);

            return new RewirerChainLink<TPropagatedData>(rewirer.AfterHijack);

            void RemoveInterceptor(TPropagatedData obj)
            {
                GameRewirers.RemoveRewirer(handle);
            }
        }
    }
}