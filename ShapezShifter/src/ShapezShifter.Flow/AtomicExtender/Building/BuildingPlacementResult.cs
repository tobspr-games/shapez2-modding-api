namespace ShapezShifter.Flow.Atomic
{
    public readonly struct BuildingPlacementResult
    {
        public readonly PlacementInitiatorId InitiatorId;
        public readonly IBuildingDefinition Building;

        public BuildingPlacementResult(PlacementInitiatorId initiatorId, IBuildingDefinition building)
        {
            InitiatorId = initiatorId;
            Building = building;
        }
    }
}