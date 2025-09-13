using Core.Events;
using ShapezShifter.Hijack;

namespace ShapezShifter.Flow.Atomic
{
    public class DefaultPlacementRewirer : IShapeBuildingsPlacementInitiatorsRewirer,
        IChainableRewirer<PlacementResult>
    {
        private readonly IBuildingDefinition BuildingDefinition;

        public DefaultPlacementRewirer(IBuildingDefinition buildingDefinition)
        {
            BuildingDefinition = buildingDefinition;
        }

        public void ModifyBuildingPlacers(BuildingInitiatorsParams @params,
            IPlacementInitiatorIdRegistry placementRegistry)
        {
            CreatePlacementInitiator(@params, placementRegistry);
        }

        private void CreatePlacementInitiator(
            BuildingInitiatorsParams buildingInitiatorsParams,
            IPlacementInitiatorIdRegistry placementRegistry)
        {
            ShapeBuildingsPlacersCreator buildingsCreator = new(
                buildingInitiatorsParams.Buildings,
                buildingInitiatorsParams.ProgressManager,
                buildingInitiatorsParams.EntityPlacementRunner,
                buildingInitiatorsParams.BuildingsModules,
                buildingInitiatorsParams.PipetteMap,
                (ITutorialState)buildingInitiatorsParams.TutorialState,
                buildingInitiatorsParams.ViewportLayerController);

            IPlacementInitiator diagonalCutter = buildingsCreator.CreateDefaultPlacer(BuildingDefinition);

            PlacementInitiatorId placementInitiatorId = placementRegistry.RegisterInitiator(
                $"{BuildingDefinition.Id.Name}Initiator",
                diagonalCutter);
            PlacementResult result = new(placementInitiatorId, BuildingDefinition);
            _AfterExtensionApplied.Invoke(result);
        }

        public IEvent<PlacementResult> AfterHijack => _AfterExtensionApplied;
        private readonly MultiRegisterEvent<PlacementResult> _AfterExtensionApplied = new();
    }

    public readonly struct PlacementResult
    {
        public readonly PlacementInitiatorId InitiatorId;
        public readonly IBuildingDefinition Building;

        public PlacementResult(PlacementInitiatorId initiatorId, IBuildingDefinition building)
        {
            InitiatorId = initiatorId;
            Building = building;
        }
    }
}