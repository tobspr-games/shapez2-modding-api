using System;
using Core.Events;
using ShapezShifter.Hijack;

namespace ShapezShifter.Flow.Atomic
{
    public class ModulesRewirer : IBuildingModulesRewirer, IChainableRewirer
    {
        private readonly IBuildingDefinition BuildingDefinition;
        private readonly IModulesData Data;

        public ModulesRewirer(IBuildingDefinition buildingDefinition,
            IModulesData data)
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