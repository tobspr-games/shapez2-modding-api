namespace ShapezShifter.Flow.Research
{
    public interface IBuildingResearchProgressionExtender
    {
        void ExtendResearch(ResearchProgression researchProgression, BuildingDefinitionGroupId groupId);
    }
}