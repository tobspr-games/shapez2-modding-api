using System.Collections.Generic;

namespace ShapezShifter.Flow.Atomic
{
    public interface ISideUpgradeSelector
    {
        public ResearchSideUpgrade Select(IReadOnlyList<ResearchSideUpgrade> milestones);
    }
}