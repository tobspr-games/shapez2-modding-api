using System.Linq;
using Core.Events;
using ShapezShifter.Hijack;

namespace ShapezShifter.Flow.Atomic
{
    internal class GameScenarioIslandRewirer : IGameScenarioRewirer, IChainableRewirer
    {
        private readonly ScenarioSelector ScenarioFilter;
        private readonly IslandDefinitionGroupId GroupId;

        public GameScenarioIslandRewirer(ScenarioSelector scenarioFilter, IslandDefinitionGroupId groupId)
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
               .Rewards.Append(new ResearchRewardIslandGroup(new SerializedResearchRewardIslandGroup(GroupId.Name)))
               .ToList();
            _AfterExtensionApplied.Invoke();
            return gameScenario;
        }

        public IEvent AfterHijack => _AfterExtensionApplied;
        private readonly MultiRegisterEvent _AfterExtensionApplied = new();
    }
}