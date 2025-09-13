namespace ShapezShifter.Flow
{
    public interface IIdentifiablePresentableAndCategorizedBuildingGroupBuilder
    {
        IBuildingGroupBuilder WithPreferredPlacement(
            DefaultPreferredPlacementMode defaultPreferredPlacementMode);
    }
}