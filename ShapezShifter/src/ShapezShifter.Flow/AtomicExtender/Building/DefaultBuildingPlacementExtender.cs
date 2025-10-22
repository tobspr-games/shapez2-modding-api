using Core.Events;
using ShapezShifter.Hijack;

namespace ShapezShifter.Flow.Atomic
{
    public class DefaultBuildingPlacementExtender : IShapeBuildingPlacementRewirers,
        IChainableRewirer<BuildingPlacementResult>
    {
        private readonly IBuildingDefinition BuildingDefinition;

        public DefaultBuildingPlacementExtender(IBuildingDefinition buildingDefinition)
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

            IPlacementInitiator placer = buildingsCreator.CreateDefaultPlacer(BuildingDefinition);

            PlacementInitiatorId placementInitiatorId = placementRegistry.RegisterInitiator(
                $"{BuildingDefinition.Id.Name}Initiator",
                placer);
            BuildingPlacementResult result = new(placementInitiatorId, BuildingDefinition);
            _AfterExtensionApplied.Invoke(result);
        }

        public IEvent<BuildingPlacementResult> AfterHijack => _AfterExtensionApplied;
        private readonly MultiRegisterEvent<BuildingPlacementResult> _AfterExtensionApplied = new();
    }
}