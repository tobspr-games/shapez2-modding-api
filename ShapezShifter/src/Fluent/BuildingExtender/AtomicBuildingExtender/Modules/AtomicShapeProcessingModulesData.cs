namespace ShapezShifter.Fluent.Atomic
{
    public class AtomicShapeProcessingModulesData : IModulesData
    {
        public readonly ResearchSpeedId SpeedId;
        public readonly float InitialProcessingDuration;

        public AtomicShapeProcessingModulesData(ResearchSpeedId speedId, float initialProcessingDuration)
        {
            SpeedId = speedId;
            InitialProcessingDuration = initialProcessingDuration;
        }
    }

    public class CustomModulesData : IModulesData
    {
        public readonly IBuildingModules Modules;

        public CustomModulesData(IBuildingModules modules)
        {
            Modules = modules;
        }
    }


    public interface IModulesData
    {
    }
}