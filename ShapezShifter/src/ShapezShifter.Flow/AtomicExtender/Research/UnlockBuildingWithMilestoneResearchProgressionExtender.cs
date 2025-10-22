using System.Linq;

namespace ShapezShifter.Flow.Research
{
    public class UnlockBuildingWithMilestoneResearchProgressionExtender : IBuildingResearchProgressionExtender
    {
        private readonly IMilestoneSelector MilestoneSelector;

        public UnlockBuildingWithMilestoneResearchProgressionExtender(IMilestoneSelector milestoneSelector)
        {
            MilestoneSelector = milestoneSelector;
        }

        public void ExtendResearch(ResearchProgression researchProgression, BuildingDefinitionGroupId groupId)
        {
            ResearchLevel level = MilestoneSelector.Select(researchProgression.Levels);
            level.Rewards = level.Rewards.Append(new ResearchRewardBuildingGroup(groupId)).ToList();
        }
    }
}