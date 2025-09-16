using System.Collections.Generic;
using Game.Core.Rails;

namespace ShapezShifter
{
    public class IslandInitiatorsParams
    {
        public readonly GameBuildings Buildings;
        public readonly GameIslands Islands;
        public readonly IIslandsModulesLookup IslandsModulesLookup;
        public readonly IEntityPlacementRunner EntityPlacementRunner;
        public readonly ITutorialStateWriteAccess TutorialState;
        public readonly IDictionary<IEntityDefinition, PipettePlacementRequest> PipetteMap;
        public readonly ResearchUnlockProgressManager ProgressManager;
        public readonly IReadonlyRailColorRegistry RailColorRegistry;
        public readonly ResearchChunkLimitManager ChunkLimitManager;
        public readonly IViewportLayersController ViewportLayersController;
        public readonly short MaxBuildingLayer;

        public IslandInitiatorsParams(IslandPlacersCreator placersCreator, short maxBuildingLayer) : this(
            placersCreator.Buildings,
            placersCreator.Islands,
            maxBuildingLayer,
            placersCreator.ProgressManager,
            placersCreator.EntityPlacementRunner,
            placersCreator.IslandsModulesLookup,
            placersCreator.PipetteMap,
            placersCreator.TutorialState,
            placersCreator.ChunkLimitManager,
            placersCreator.ViewportLayersController,
            placersCreator.RailColorRegistry)
        {
        }

        private IslandInitiatorsParams(GameBuildings buildings,
            GameIslands islands,
            short maxBuildingLayer,
            ResearchUnlockProgressManager progressManager,
            IEntityPlacementRunner entityPlacementRunner,
            IIslandsModulesLookup islandsModulesLookup,
            IDictionary<IEntityDefinition, PipettePlacementRequest> pipetteMap,
            ITutorialStateWriteAccess tutorialState,
            ResearchChunkLimitManager chunkLimitManager,
            IViewportLayersController viewportLayersController,
            IReadonlyRailColorRegistry railColorRegistry)
        {
            Buildings = buildings;
            Islands = islands;
            MaxBuildingLayer = maxBuildingLayer;
            IslandsModulesLookup = islandsModulesLookup;
            EntityPlacementRunner = entityPlacementRunner;
            TutorialState = tutorialState;
            PipetteMap = pipetteMap;
            ProgressManager = progressManager;
            RailColorRegistry = railColorRegistry;
            ChunkLimitManager = chunkLimitManager;
            ViewportLayersController = viewportLayersController;
        }
    }
}