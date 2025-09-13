using System.Collections.Generic;
using Core.Events;
using Core.Factory;
using Game.Core.Simulation;
using ShapezShifter.Hijack;

namespace ShapezShifter.Flow.Atomic
{
    public class SimulationRewirer<TSimulation, TState, TConfig> : ISimulationSystemsRewirer,
        IChainableRewirer<TConfig>
        where TSimulation : ISimulation
        where TState : class, ISimulationState, new()
    {
        private readonly ScenarioSelector ScenarioFilter;
        private readonly BuildingDefinitionId DefinitionId;
        private readonly IFactoryBuilder<TSimulation, TState, TConfig> FactoryBuilder;

        public SimulationRewirer(ScenarioSelector scenarioFilter,
            BuildingDefinitionId definitionId,
            IFactoryBuilder<TSimulation, TState, TConfig> factoryBuilder)
        {
            ScenarioFilter = scenarioFilter;
            DefinitionId = definitionId;
            FactoryBuilder = factoryBuilder;
        }


        public void ModifySimulationSystems(ICollection<ISimulationSystem> simulationSystems,
            SimulationSystemsDependencies dependencies)
        {
            IFactory<TState, TSimulation> factory = FactoryBuilder.BuildFactory(dependencies, out TConfig config);
            simulationSystems.Add(CreateAtomicSystem(factory));

            _AfterExtensionApplied.Invoke(config);
        }

        private ISimulationSystem CreateAtomicSystem(IFactory<TState, TSimulation> factory)
        {
            return new AtomicStatefulBuildingSimulationSystem<TSimulation, TState>(factory,
                DefinitionId);
        }

        public IEvent<TConfig> AfterHijack => _AfterExtensionApplied;

        private readonly MultiRegisterEvent<TConfig> _AfterExtensionApplied =
            new();
    }
}