namespace ShapezShifter.Flow
{
    public static class Island
    {
        public static IIdentifiableIslandBuilder Create(IslandDefinitionId id)
        {
            return new IslandBuilder(id);
        }
    }
}