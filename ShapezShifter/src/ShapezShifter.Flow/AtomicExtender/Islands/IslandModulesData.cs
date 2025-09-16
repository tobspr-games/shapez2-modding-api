namespace ShapezShifter.Flow.Atomic
{
    public class CustomIslandsModulesData : IIslandModulesData
    {
        public readonly IIslandModuleDataProvider Modules;

        public CustomIslandsModulesData(IIslandModuleDataProvider modules)
        {
            Modules = modules;
        }
    }

    public class NoModulesData : IIslandModulesData
    {
    }


    public interface IIslandModulesData
    {
    }
}