namespace ShapezShifter.Flow
{
    public interface IIdentifiableAndPresentableIslandGroupBuilder
    {
        IIdentifiablePresentableAndCategorizedIslandGroupBuilder AsTransportableIsland();

        IIdentifiablePresentableAndCategorizedIslandGroupBuilder AsNonTransportableIsland();
    }
}