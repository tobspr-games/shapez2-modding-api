namespace ShapezShifter.Flow.Atomic
{
    public static class AtomicIslands
    {
        public static IBaseIslandExtender Extend()
        {
            return new AtomicIslandExtender();
        }
    }
}