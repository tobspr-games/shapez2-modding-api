namespace ShapezShifter.Fluent
{
    public interface IIdentifiableAndPresentableBuildingGroupBuilder
    {
        IIdentifiablePresentableAndCategorizedBuildingGroupBuilder AsTransportableBuilding();

        IIdentifiablePresentableAndCategorizedBuildingGroupBuilder AsNonTransportableBuilding();
    }
}