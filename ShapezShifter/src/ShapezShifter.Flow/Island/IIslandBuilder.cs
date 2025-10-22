namespace ShapezShifter.Flow
{
    public interface IIslandBuilder
    {
        IslandDefinition BuildAndRegister(IslandDefinitionGroup group,
            GameIslands gameIslands);
    }
}