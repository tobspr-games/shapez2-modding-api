using System.Collections.Generic;
using System.Linq;

namespace ShapezShifter.Flow.Research
{
    public class RewardlessSideUpgradeLogicalData
    {
        public readonly string[] RequiredUpgradeIds;
        public readonly string[] RequiredMechanicIds;

        public readonly ISerializedResearchReward[] Rewards;
        public readonly ISerializedResearchCost[] Costs;

        private RewardlessSideUpgradeLogicalData(IEnumerable<string> requiredUpgradeIds,
            IEnumerable<string> requiredMechanicIds,
            IEnumerable<ISerializedResearchReward> rewards, IEnumerable<ISerializedResearchCost> costs)
        {
            RequiredUpgradeIds = requiredUpgradeIds.ToArray();
            RequiredMechanicIds = requiredMechanicIds.ToArray();
            Rewards = rewards.ToArray();
            Costs = costs.ToArray();
        }
    }
}