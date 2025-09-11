namespace ShapezShifter.Fluent
{
    public static class BuildingGroup
    {
        public static IIdentifiableBuildingGroupBuilder Create(
            BuildingDefinitionGroupId definitionGroupId)
        {
            return new BuildingGroupBuilder(definitionGroupId);
        }
    }
}