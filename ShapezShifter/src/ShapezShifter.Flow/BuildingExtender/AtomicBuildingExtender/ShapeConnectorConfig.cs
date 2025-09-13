namespace ShapezShifter.Flow.Atomic
{
    public struct ShapeConnectorConfig
    {
        public readonly TileDirection Direction;
        public readonly BuildingBeltStandType StandType;
        public readonly BuildingItemIOType CapsType;
        public readonly bool Separators;

        private ShapeConnectorConfig(TileDirection direction,
            BuildingItemIOType capsType = BuildingItemIOType.ElevatedBorder,
            BuildingBeltStandType standType = BuildingBeltStandType.Normal, bool separators = false)
        {
            Direction = direction;
            StandType = standType;
            CapsType = capsType;
            Separators = separators;
        }

        public static ShapeConnectorConfig DefaultInput(
            BuildingItemIOType capsType = BuildingItemIOType.ElevatedBorder,
            BuildingBeltStandType standType = BuildingBeltStandType.Normal, bool separators = false)
        {
            return new ShapeConnectorConfig(TileDirection.West, capsType, standType, separators);
        }

        public static ShapeConnectorConfig DefaultOutput(
            BuildingItemIOType capsType = BuildingItemIOType.ElevatedBorder,
            BuildingBeltStandType standType = BuildingBeltStandType.Normal, bool separators = false)
        {
            return new ShapeConnectorConfig(TileDirection.East, capsType, standType, separators);
        }
    }
}