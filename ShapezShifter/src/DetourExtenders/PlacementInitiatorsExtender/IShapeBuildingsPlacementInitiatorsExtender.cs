using JetBrains.Annotations;

namespace ShapezShifter
{
    [PublicAPI]
    public interface
        IShapeBuildingsPlacementInitiatorsExtender : IBuildingsPlacementInitiatorsExtender
    {
    }

    [PublicAPI]
    public interface
        IFluidBuildingsPlacementInitiatorsExtender : IBuildingsPlacementInitiatorsExtender
    {
    }

    [PublicAPI]
    public interface
        ISignalBuildingsPlacementInitiatorsExtender : IBuildingsPlacementInitiatorsExtender
    {
    }

    [PublicAPI]
    public interface
        IDecorationBuildingsPlacementInitiatorsExtender : IBuildingsPlacementInitiatorsExtender
    {
    }

    [PublicAPI]
    public interface
        ISandboxBuildingsPlacementInitiatorsExtender : IBuildingsPlacementInitiatorsExtender
    {
    }

    [PublicAPI]
    public interface IBuildingsPlacementInitiatorsExtender : IExtender
    {
        void ExtendBuildingInitiators(BuildingInitiatorsParams @params,
            IPlacementInitiatorIdRegistry placementRegistry);
    }
}