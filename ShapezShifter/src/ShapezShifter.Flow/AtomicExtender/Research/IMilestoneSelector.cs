using System.Collections.Generic;

namespace ShapezShifter.Flow.Research
{
    public interface IMilestoneSelector
    {
        public ResearchLevel Select(IReadOnlyList<ResearchLevel> milestones);
    }
}