using System.Linq;
using Core.Events;
using ShapezShifter.Hijack;

namespace ShapezShifter.Flow.Atomic
{
    internal class GameScenarioBuildingExtender : IGameScenarioRewirer, IChainableRewirer
    {
        private readonly ScenarioSelector ScenarioFilter;
        private readonly BuildingDefinitionGroupId GroupId;

        public GameScenarioBuildingExtender(ScenarioSelector scenarioFilter, BuildingDefinitionGroupId groupId)
        {
            ScenarioFilter = scenarioFilter;
            GroupId = groupId;
        }

        public GameScenario ModifyGameScenario(GameScenario gameScenario)
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

        public IEvent AfterHijack => _AfterExtensionApplied;
        private readonly MultiRegisterEvent _AfterExtensionApplied = new();
    }
}