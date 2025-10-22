using Core.Events;
using Game.Core.Rendering.MeshGeneration;
using ShapezShifter.Hijack;

namespace ShapezShifter.Flow.Atomic
{
    public class BuildingsExtender : IBuildingsRewirer, IChainableRewirer<BuildingDefinition>
    {
        private readonly IBuildingBuilder BuildingBuilder;
        private readonly IBuildingGroupBuilder BuildingGroupBuilder;
        public IEvent<BuildingDefinition> AfterHijack => _AfterExtensionApplied;
        private readonly MultiRegisterEvent<BuildingDefinition> _AfterExtensionApplied = new();

        public BuildingsExtender(IBuildingBuilder buildingBuilder,
            IBuildingGroupBuilder buildingGroupBuilder)
        {
            BuildingBuilder = buildingBuilder;
            BuildingGroupBuilder = buildingGroupBuilder;
        }

        public GameBuildings ModifyGameBuildings(MetaGameModeBuildings metaBuildings,
            GameBuildings gameBuildings,
            IMeshCache meshCache, VisualThemeBaseResources theme)
        {
            BuildingDefinitionGroup buildingGroup = BuildingGroupBuilder.BuildAndRegister(gameBuildings);
            BuildingDefinition building = BuildingBuilder.BuildAndRegister(buildingGroup, gameBuildings);

            _AfterExtensionApplied.Invoke(building);
            return gameBuildings;
        }
    }
}