using JetBrains.Annotations;

namespace ShapezShifter.Hijack
{
    [PublicAPI]
    public interface
        IShapeBuildingPlacementRewirers : IBuildingPlacementRewirers
    {
    }

    [PublicAPI]
    public interface
        IFluidBuildingPlacementRewirers : IBuildingPlacementRewirers
    {
    }

    [PublicAPI]
    public interface
        ISignalBuildingPlacementRewirers : IBuildingPlacementRewirers
    {
    }

    [PublicAPI]
    public interface
        IDecorationBuildingPlacementRewirers : IBuildingPlacementRewirers
    {
    }

    [PublicAPI]
    public interface
        ISandboxBuildingPlacementRewirers : IBuildingPlacementRewirers
    {
    }

    [PublicAPI]
    public interface IBuildingPlacementRewirers : IRewirer
    {
        void ModifyBuildingPlacers(BuildingInitiatorsParams @params,
            IPlacementInitiatorIdRegistry placementRegistry);
    }
}