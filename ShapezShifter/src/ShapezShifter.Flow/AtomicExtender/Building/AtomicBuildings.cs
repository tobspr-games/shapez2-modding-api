namespace ShapezShifter.Flow.Atomic
{
    public static class AtomicBuildings
    {
        public static IBaseBuildingExtender Extend()
        {
            return new AtomicBuildingExtender();
        }
    }
}