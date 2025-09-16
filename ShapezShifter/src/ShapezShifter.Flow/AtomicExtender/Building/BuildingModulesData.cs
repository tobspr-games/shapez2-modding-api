namespace ShapezShifter.Flow.Atomic
{
    public class BuildingModulesData : IBuildingModulesData
    {
        public readonly ResearchSpeedId SpeedId;
        public readonly float InitialProcessingDuration;

        public BuildingModulesData(ResearchSpeedId speedId, float initialProcessingDuration)
        {
            SpeedId = speedId;
            InitialProcessingDuration = initialProcessingDuration;
        }
    }

    public class CustomBuildingsModulesData : IBuildingModulesData
    {
        public readonly IBuildingModules Modules;

        public CustomBuildingsModulesData(IBuildingModules modules)
        {
            Modules = modules;
        }
    }


    public interface IBuildingModulesData
    {
    }
}