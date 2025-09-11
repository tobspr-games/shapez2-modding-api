using System;
using Core.Events;

namespace ShapezShifter.Fluent.Atomic
{
    public class ModulesExtender : IBuildingModulesExtender, IChainableExtender
    {
        private readonly IBuildingDefinition BuildingDefinition;
        private readonly IModulesData Data;

        public ModulesExtender(IBuildingDefinition buildingDefinition,
            IModulesData data)
        {
            BuildingDefinition = buildingDefinition;
            Data = data;
        }

        public IEvent AfterExtensionApplied => _AfterExtensionApplied;

        private readonly MultiRegisterEvent _AfterExtensionApplied =
            new();

        public void AddModules(BuildingsModulesLookup modulesLookup)
        {
            var buildingModules = Data switch
            {
                AtomicShapeProcessingModulesData processingModulesData => new ItemSimulationBuildingModuleDataProvider(
                    processingModulesData.SpeedId,
                    processingModulesData.InitialProcessingDuration),
                CustomModulesData customModulesData => customModulesData.Modules,
                _ => throw new Exception()
            };

            modulesLookup.AddModule(BuildingDefinition.Id, BuildingDefinition, buildingModules);
            _AfterExtensionApplied.Invoke();
        }
    }
}