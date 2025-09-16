namespace ShapezShifter.Flow.Atomic
{
    public readonly struct IslandPlacementResult
    {
        public readonly PlacementInitiatorId InitiatorId;
        public readonly IIslandDefinition Island;

        public IslandPlacementResult(PlacementInitiatorId initiatorId, IIslandDefinition island)
        {
            InitiatorId = initiatorId;
            Island = island;
        }
    }
}