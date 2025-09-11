using Core.Events;
using Game.Core.Rendering.MeshGeneration;

namespace ShapezShifter.Fluent.Atomic
{
    public class BuildingsExtender : IBuildingsExtender, IChainableExtender<BuildingDefinition>
    {
        private readonly IBuildingBuilder BuildingBuilder;
        private readonly IBuildingGroupBuilder BuildingGroupBuilder;
        public IEvent<BuildingDefinition> AfterExtensionApplied => _AfterExtensionApplied;
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
            var buildingGroup = BuildingGroupBuilder.BuildAndRegister(gameBuildings);
            var building = BuildingBuilder.BuildAndRegister(buildingGroup, gameBuildings);

            _AfterExtensionApplied.Invoke(building);
            return gameBuildings;
        }
    }
}