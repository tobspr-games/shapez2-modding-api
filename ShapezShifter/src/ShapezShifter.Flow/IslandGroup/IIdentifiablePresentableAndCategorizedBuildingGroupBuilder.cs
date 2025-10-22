namespace ShapezShifter.Flow
{
    public interface IIdentifiablePresentableAndCategorizedIslandGroupBuilder
    {
        IIslandGroupBuilder WithPreferredPlacement(
            DefaultPreferredPlacementMode defaultPreferredPlacementMode);
    }
}