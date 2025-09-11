namespace ShapezShifter.Fluent
{
    public interface IBuildingBuilder
    {
        BuildingDefinition BuildAndRegister(BuildingDefinitionGroup group,
            GameBuildings gameBuildings);
    }
}