namespace ShapezShifter.Flow
{
    public interface IBuildingBuilder
    {
        BuildingDefinition BuildAndRegister(BuildingDefinitionGroup group,
            GameBuildings gameBuildings);
    }
}