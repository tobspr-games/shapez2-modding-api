using System.Linq;
using Core.Events;

namespace ShapezShifter.Fluent.Atomic
{
    internal class GameScenarioExtender : IGameScenarioExtender, IChainableExtender
    {
        private readonly ScenarioSelector ScenarioFilter;
        private readonly BuildingDefinitionGroupId GroupId;

        public GameScenarioExtender(ScenarioSelector scenarioFilter, BuildingDefinitionGroupId groupId)
        {
            ScenarioFilter = scenarioFilter;
            GroupId = groupId;
        }

        public GameScenario ExtendGameScenario(GameScenario gameScenario)
        {
            if (!ScenarioFilter.Invoke(gameScenario))
            {
                return gameScenario;
            }

            gameScenario.Progression.Levels[^1].Rewards = gameScenario.Progression.Levels[^1]
               .Rewards.Append(new ResearchRewardBuildingGroup(GroupId))
               .ToList();
            _AfterExtensionApplied.Invoke();
            return gameScenario;
        }

        public IEvent AfterExtensionApplied => _AfterExtensionApplied;
        private readonly MultiRegisterEvent _AfterExtensionApplied = new();
    }
}