using System.Collections.Generic;

namespace ShapezShifter
{
    public class BuildingInitiatorsParams
    {
        public readonly GameBuildings Buildings;
        public readonly IBuildingsModulesLookup BuildingsModules;
        public readonly IEntityPlacementRunner EntityPlacementRunner;
        public readonly ITutorialStateWriteAccess TutorialState;
        public readonly IDictionary<IEntityDefinition, PipettePlacementRequest> PipetteMap;
        public readonly ResearchUnlockProgressManager ProgressManager;
        public readonly IViewportLayersController ViewportLayerController;

        public BuildingInitiatorsParams(BuildingPlacersCreator placersCreator) : this(
            placersCreator.Buildings, placersCreator.BuildingsModules,
            placersCreator.EntityPlacementRunner, placersCreator.TutorialState,
            placersCreator.PipetteMap, placersCreator.ProgressManager,
            placersCreator.ViewportLayerController)
        {
        }

        private BuildingInitiatorsParams(GameBuildings buildings,
            IBuildingsModulesLookup buildingsModules,
            IEntityPlacementRunner entityPlacementRunner,
            ITutorialStateWriteAccess tutorialState,
            IDictionary<IEntityDefinition, PipettePlacementRequest> pipetteMap,
            ResearchUnlockProgressManager progressManager,
            IViewportLayersController viewportLayerController)
        {
            Buildings = buildings;
            BuildingsModules = buildingsModules;
            EntityPlacementRunner = entityPlacementRunner;
            TutorialState = tutorialState;
            PipetteMap = pipetteMap;
            ProgressManager = progressManager;
            ViewportLayerController = viewportLayerController;
        }
    }
}