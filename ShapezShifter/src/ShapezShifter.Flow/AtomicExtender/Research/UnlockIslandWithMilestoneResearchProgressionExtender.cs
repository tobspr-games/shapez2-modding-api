using System.Linq;

namespace ShapezShifter.Flow.Research
{
    public class UnlockIslandWithMilestoneResearchProgressionExtender : IIslandResearchProgressionExtender
    {
        private readonly IMilestoneSelector MilestoneSelector;

        public UnlockIslandWithMilestoneResearchProgressionExtender(IMilestoneSelector milestoneSelector)
        {
            MilestoneSelector = milestoneSelector;
        }

        public void ExtendResearch(ResearchProgression researchProgression, IslandDefinitionGroupId groupId)
        {
            ResearchLevel level = MilestoneSelector.Select(researchProgression.Levels);
            level.Rewards = level.Rewards
               .Append(new ResearchRewardIslandGroup(new SerializedResearchRewardIslandGroup(groupId.Name)))
               .ToList();
        }
    }
}