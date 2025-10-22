namespace ShapezShifter.Flow.Research
{
    public class SideUpgradeData
    {
        public readonly SideUpgradePresentationData PresentationData;
        public readonly RewardlessSideUpgradeLogicalData LogicalData;

        public SideUpgradeData(SideUpgradePresentationData presentationData,
            RewardlessSideUpgradeLogicalData logicalData)
        {
            PresentationData = presentationData;
            LogicalData = logicalData;
        }
    }
}