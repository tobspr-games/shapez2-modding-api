using Core.Events;
using ShapezShifter.Hijack;

namespace ShapezShifter.Flow.Atomic
{
    public class IslandsExtender : IIslandsRewirer, IChainableRewirer<IslandDefinition>
    {
        private readonly IIslandBuilder IslandBuilder;
        private readonly IIslandGroupBuilder IslandGroupBuilder;
        public IEvent<IslandDefinition> AfterHijack => _AfterExtensionApplied;
        private readonly MultiRegisterEvent<IslandDefinition> _AfterExtensionApplied = new();

        public IslandsExtender(IIslandBuilder islandBuilder,
            IIslandGroupBuilder islandGroupBuilder)
        {
            IslandBuilder = islandBuilder;
            IslandGroupBuilder = islandGroupBuilder;
        }

        public GameIslands ModifyGameIslands(IslandDefinitionFactory factory,
            MetaGameModeIslands metaIslands, GameIslands gameIslands)
        {
            IslandDefinitionGroup islandGroup = IslandGroupBuilder.BuildAndRegister(gameIslands);
            IslandDefinition island = IslandBuilder.BuildAndRegister(islandGroup, gameIslands);

            IslandDefinitionGroupId groupId = islandGroup.Id;
            string groupName = islandGroup.Id.Name;

            factory.IslandGroupDefinitionFactory.GroupIdToSerialMap.Add(groupId, groupName);
            factory.IslandGroupDefinitionFactory.GroupSerialToIdMap.Add(groupName, groupId);
            factory.IslandGroupDefinitionFactory.GroupImplementationMap.Add(groupId, islandGroup);

            _AfterExtensionApplied.Invoke(island);
            return gameIslands;
        }
    }
}