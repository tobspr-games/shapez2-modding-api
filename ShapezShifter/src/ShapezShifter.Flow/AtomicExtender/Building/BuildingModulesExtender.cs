using System;
using Core.Events;
using ShapezShifter.Hijack;

namespace ShapezShifter.Flow.Atomic
{
    public class BuildingModulesExtender : IBuildingModulesRewirer, IChainableRewirer
    {
        private readonly IBuildingDefinition BuildingDefinition;
        private readonly IBuildingModulesData Data;

        public BuildingModulesExtender(IBuildingDefinition buildingDefinition,
            IBuildingModulesData data)
        {
            BuildingDefinition = buildingDefinition;
            Data = data;
        }

        public IEvent AfterHijack => _AfterExtensionApplied;

        private readonly MultiRegisterEvent _AfterExtensionApplied =
            new();

        public void AddModules(BuildingsModulesLookup modulesLookup)
        {
            IBuildingModules buildingModules = Data switch
            {
                BuildingModulesData processingModulesData => new
                    ItemSimulationBuildingModuleDataProvider(
                        processingModulesData.SpeedId,
                        processingModulesData.InitialProcessingDuration),
                CustomBuildingsModulesData customModulesData => customModulesData.Modules,
                _ => throw new Exception()
            };

            modulesLookup.AddModule(BuildingDefinition.Id, BuildingDefinition, buildingModules);
            _AfterExtensionApplied.Invoke();
        }
    }
}