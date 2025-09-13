namespace ShapezShifter.Flow
{
    public interface IIdentifiableAndPresentableBuildingGroupBuilder
    {
        IIdentifiablePresentableAndCategorizedBuildingGroupBuilder AsTransportableBuilding();

        IIdentifiablePresentableAndCategorizedBuildingGroupBuilder AsNonTransportableBuilding();
    }
}