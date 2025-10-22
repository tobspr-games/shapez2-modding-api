namespace ShapezShifter.Hijack
{
    public interface
        IPlatformIslandPlacementRewirers : IIslandPlacementRewirers
    {
    }

    public interface
        ITrainIslandPlacementRewirers : IIslandPlacementRewirers
    {
    }

    public interface
        IConverterIslandPlacementRewirers : IIslandPlacementRewirers
    {
    }

    public interface IIslandPlacementRewirers : IRewirer
    {
        void ModifyIslandPlacers(IslandInitiatorsParams islandInitiatorsParams,
            IPlacementInitiatorIdRegistry placementRegistry);
    }
}