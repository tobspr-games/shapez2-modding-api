using Core.Events;
using ShapezShifter.Hijack;

namespace ShapezShifter.Flow.Atomic
{
    public class DefaultIslandPlacementExtender : IPlatformIslandPlacementRewirers,
        IChainableRewirer<IslandPlacementResult>
    {
        private readonly IIslandDefinition IslandDefinition;

        public DefaultIslandPlacementExtender(IIslandDefinition islandDefinition)
        {
            IslandDefinition = islandDefinition;
        }

        public IEvent<IslandPlacementResult> AfterHijack => _AfterExtensionApplied;
        private readonly MultiRegisterEvent<IslandPlacementResult> _AfterExtensionApplied = new();

        public void ModifyIslandPlacers(IslandInitiatorsParams islandInitiatorsParams,
            IPlacementInitiatorIdRegistry placementRegistry)
        {
            PlatformIslandsPlacersCreators islandsPlacers = new(
                islandInitiatorsParams.Buildings,
                islandInitiatorsParams.Islands,
                islandInitiatorsParams.MaxBuildingLayer,
                islandInitiatorsParams.ProgressManager,
                islandInitiatorsParams.EntityPlacementRunner,
                islandInitiatorsParams.IslandsModulesLookup,
                islandInitiatorsParams.PipetteMap,
                (ITutorialState)islandInitiatorsParams.TutorialState,
                islandInitiatorsParams.ChunkLimitManager,
                islandInitiatorsParams.ViewportLayersController,
                islandInitiatorsParams.RailColorRegistry);

            IPlacementInitiator placer = islandsPlacers.CreateDefaultPlacer(IslandDefinition);

            PlacementInitiatorId placementInitiatorId = placementRegistry.RegisterInitiator(
                $"{IslandDefinition.Id.Name}Initiator",
                placer);
            IslandPlacementResult result = new(placementInitiatorId, IslandDefinition);
            _AfterExtensionApplied.Invoke(result);
        }
    }
}