namespace ShapezShifter.Fluent
{
    public interface IIdentifiablePresentableAndCategorizedBuildingGroupBuilder
    {
        IBuildingGroupBuilder WithPreferredPlacement(
            DefaultPreferredPlacementMode defaultPreferredPlacementMode);
    }
}