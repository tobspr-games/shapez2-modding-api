using System.Collections.Generic;
using Core.Events;
using Core.Factory;
using Game.Core.Simulation;

namespace ShapezShifter.Fluent.Atomic
{
    public class SimulationExtender<TSimulation, TState, TConfig> : ISimulationSystemsExtender,
        IChainableExtender<TConfig> where TSimulation : ISimulation
        where TState : class, ISimulationState, new()
    {
        private readonly ScenarioSelector ScenarioFilter;
        private readonly BuildingDefinitionId DefinitionId;
        private readonly IFactoryBuilder<TSimulation, TState, TConfig> FactoryBuilder;

        public SimulationExtender(ScenarioSelector scenarioFilter,
            BuildingDefinitionId definitionId,
            IFactoryBuilder<TSimulation, TState, TConfig> factoryBuilder)
        {
            ScenarioFilter = scenarioFilter;
            DefinitionId = definitionId;
            FactoryBuilder = factoryBuilder;
        }


        public void ExtendSimulationSystems(ICollection<ISimulationSystem> simulationSystems,
            SimulationSystemsDependencies dependencies)
        {
            var factory = FactoryBuilder.BuildFactory(dependencies, out var config);
            simulationSystems.Add(CreateAtomicSystem(factory));

            _AfterExtensionApplied.Invoke(config);
        }

        private ISimulationSystem CreateAtomicSystem(IFactory<TState, TSimulation> factory)
        {
            return new AtomicStatefulBuildingSimulationSystem<TSimulation, TState>(factory,
                DefinitionId);
        }

        public IEvent<TConfig> AfterExtensionApplied => _AfterExtensionApplied;

        private readonly MultiRegisterEvent<TConfig> _AfterExtensionApplied =
            new();
    }
}