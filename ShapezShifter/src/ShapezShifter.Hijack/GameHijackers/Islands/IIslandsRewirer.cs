namespace ShapezShifter.Hijack
{
    public interface IIslandsRewirer : IRewirer
    {
        GameIslands ModifyGameIslands(IslandDefinitionFactory factory, MetaGameModeIslands metaIslands,
            GameIslands gameIslands);
    }
}