using System.Linq;
using ShapezShifter.Flow.Atomic;

namespace ShapezShifter.Flow.Research
{
    public class UnlockBuildingWithExistingSideUpgradeResearchProgressionExtender : IBuildingResearchProgressionExtender
    {
        private readonly ISideUpgradeSelector SideUpgradeSelector;

        public UnlockBuildingWithExistingSideUpgradeResearchProgressionExtender(
            ISideUpgradeSelector sideUpgradeSelector)
        {
            SideUpgradeSelector = sideUpgradeSelector;
        }

        public void ExtendResearch(ResearchProgression researchProgression, BuildingDefinitionGroupId groupId)
        {
            ResearchSideUpgrade sideUpgrade = SideUpgradeSelector.Select(researchProgression.SideUpgrades);
            sideUpgrade.Rewards = sideUpgrade.Rewards.Append(new ResearchRewardBuildingGroup(groupId)).ToList();
        }
    }
}