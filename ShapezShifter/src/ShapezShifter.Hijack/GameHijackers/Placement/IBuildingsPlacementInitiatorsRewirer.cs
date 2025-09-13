using JetBrains.Annotations;

namespace ShapezShifter.Hijack
{
    [PublicAPI]
    public interface
        IShapeBuildingsPlacementInitiatorsRewirer : IBuildingsPlacementInitiatorsRewirer
    {
    }

    [PublicAPI]
    public interface
        IFluidBuildingsPlacementInitiatorsRewirer : IBuildingsPlacementInitiatorsRewirer
    {
    }

    [PublicAPI]
    public interface
        ISignalBuildingsPlacementInitiatorsRewirer : IBuildingsPlacementInitiatorsRewirer
    {
    }

    [PublicAPI]
    public interface
        IDecorationBuildingsPlacementInitiatorsRewirer : IBuildingsPlacementInitiatorsRewirer
    {
    }

    [PublicAPI]
    public interface
        ISandboxBuildingsPlacementInitiatorsRewirer : IBuildingsPlacementInitiatorsRewirer
    {
    }

    [PublicAPI]
    public interface IBuildingsPlacementInitiatorsRewirer : IRewirer
    {
        void ModifyBuildingPlacers(BuildingInitiatorsParams @params,
            IPlacementInitiatorIdRegistry placementRegistry);
    }
}