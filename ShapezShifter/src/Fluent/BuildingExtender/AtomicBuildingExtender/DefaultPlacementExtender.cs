using Core.Events;

namespace ShapezShifter.Fluent.Atomic
{
    public class DefaultPlacementExtender : IShapeBuildingsPlacementInitiatorsExtender,
        IChainableExtender<PlacementResult>
    {
        private readonly IBuildingDefinition BuildingDefinition;

        public DefaultPlacementExtender(IBuildingDefinition buildingDefinition)
        {
            BuildingDefinition = buildingDefinition;
        }

        public void ExtendBuildingInitiators(BuildingInitiatorsParams @params,
            IPlacementInitiatorIdRegistry placementRegistry)
        {
            CreatePlacementInitiator(@params, placementRegistry);
        }

        private void CreatePlacementInitiator(
            BuildingInitiatorsParams buildingInitiatorsParams,
            IPlacementInitiatorIdRegistry placementRegistry)
        {
            var buildingsCreator = new ShapeBuildingsPlacersCreator(
                buildingInitiatorsParams.Buildings,
                buildingInitiatorsParams.ProgressManager,
                buildingInitiatorsParams.EntityPlacementRunner,
                buildingInitiatorsParams.BuildingsModules,
                buildingInitiatorsParams.PipetteMap,
                (ITutorialState)buildingInitiatorsParams.TutorialState,
                buildingInitiatorsParams.ViewportLayerController);

            var diagonalCutter = buildingsCreator.CreateDefaultPlacer(BuildingDefinition);

            var placementInitiatorId = placementRegistry.RegisterInitiator(
                $"{BuildingDefinition.Id.Name}Initiator",
                diagonalCutter);
            var result = new PlacementResult(placementInitiatorId, BuildingDefinition);
            _AfterExtensionApplied.Invoke(result);
        }

        public IEvent<PlacementResult> AfterExtensionApplied => _AfterExtensionApplied;
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