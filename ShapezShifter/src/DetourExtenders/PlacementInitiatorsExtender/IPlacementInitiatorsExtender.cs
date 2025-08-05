namespace ShapezShifter
{
    public interface IPlacementInitiatorsExtender : IExtender
    {
        void ExtendInitiators(IEntityPlacementRunner creatorEntityPlacementRunner,
            IPlacementInitiatorIdRegistry placementRegistry);
    }
}