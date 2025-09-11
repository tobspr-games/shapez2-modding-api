namespace ShapezShifter.Fluent
{
    public static class Building
    {
        public static IIdentifiableBuildingBuilder Create(BuildingDefinitionId id)
        {
            return new BuildingBuilder(id);
        }
    }
}