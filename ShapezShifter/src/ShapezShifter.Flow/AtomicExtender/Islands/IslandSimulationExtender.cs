using System.Collections.Generic;
using Core.Events;
using Core.Factory;
using Game.Content.AtomicIslands;
using Game.Core.Simulation;
using ShapezShifter.Hijack;

namespace ShapezShifter.Flow.Atomic
{
    public class IslandSimulationExtender<TSimulation> : ISimulationSystemsRewirer,
        IChainableRewirer
        where TSimulation : ISimulation
    {
        private readonly IslandDefinitionId DefinitionId;
        private readonly IFactoryBuilder<TSimulation> FactoryBuilder;

        public IslandSimulationExtender(IslandDefinitionId definitionId,
            IFactoryBuilder<TSimulation> factoryBuilder)
        {
            DefinitionId = definitionId;
            FactoryBuilder = factoryBuilder;
        }


        public void ModifySimulationSystems(ICollection<ISimulationSystem> simulationSystems,
            SimulationSystemsDependencies dependencies)
        {
            IFactory<TSimulation> factory = FactoryBuilder.BuildFactory(dependencies);
            simulationSystems.Add(CreateAtomicSystem(factory));

            _AfterExtensionApplied.Invoke();
        }

        private ISimulationSystem CreateAtomicSystem(IFactory<TSimulation> factory)
        {
            return new AtomicStatelessIslandSimulationSystem<TSimulation>(factory,
                DefinitionId);
        }

        public IEvent AfterHijack => _AfterExtensionApplied;

        private readonly MultiRegisterEvent _AfterExtensionApplied =
            new();
    }

    public class IslandSimulationExtender<TSimulation, TState, TConfig> : ISimulationSystemsRewirer,
        IChainableRewirer<TConfig>
        where TSimulation : Simulation<TState>
        where TState : class, ISimulationState, new()
    {
        private readonly IslandDefinitionId DefinitionId;
        private readonly IFactoryBuilder<TSimulation, TState, TConfig> FactoryBuilder;

        public IslandSimulationExtender(IslandDefinitionId definitionId,
            IFactoryBuilder<TSimulation, TState, TConfig> factoryBuilder)
        {
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
            return new AtomicStatefulIslandSimulationSystem<TSimulation, TState>(factory,
                DefinitionId);
        }

        public IEvent<TConfig> AfterHijack => _AfterExtensionApplied;

        private readonly MultiRegisterEvent<TConfig> _AfterExtensionApplied =
            new();
    }
}