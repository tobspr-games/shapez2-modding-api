namespace ShapezShifter.Fluent
{
    public interface IInitialBuildingExtender
    {
        IScenarioTargetedBuildingExtender TargetingScenariosThat(ScenarioSelector filter);
    }

    public interface IScenarioTargetedBuildingExtender
    {
        IBuildingExtensionPromise CreateBuilding(IBuildingBuilder buildingBuilder);
    }

    public interface IBuildingExtensionPromise
    {
        IUnlockableBuildingExtensionPromise UnlockAt();
    }

    public interface IUnlockableBuildingExtensionPromise
    {
        IBuildingExtensionPromiseWithPlacement PlaceWith();
    }

    public interface IBuildingExtensionPromiseWithPlacement
    {
        IBuildingExtensionPromiseWithPlacement AccessibleFrom();
    }

    public interface IBuildingExtensionPromiseWithPlacementAndToolbar
    {
        ISimulatedBuildingExtensionPromise WithSimulation();
        ISimulatedBuildingExtensionPromise WithoutSimulation();
    }

    public interface ISimulatedBuildingExtensionPromise
    {
        public ICompleteBuildingExtensionBuilder WithEssentialModules();
        public ICompleteBuildingExtensionBuilder WithModules(IBuildingModules buildingModules);
    }

    public interface ICompleteBuildingExtensionBuilder
    {
        public IExtender Register();
    }
}