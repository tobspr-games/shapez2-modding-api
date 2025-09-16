using System;
using Game.Core.Simulation;
using ShapezShifter.Flow.Toolbar;
using ShapezShifter.Hijack;

namespace ShapezShifter.Flow.Atomic
{
    public class AtomicIslandExtender :
        IBaseIslandExtender,
        IScenarioSelectiveIslandExtender,
        IDefinedIslandExtender,
        IDefinedPlaceableIslandExtender,
        IDefinedPlaceableAccessibleIslandExtender,
        IDefinedSimulatableIslandExtender,
        IDefinedSimulatablePlaceableIslandExtender,
        IAtomicIslandExtender,
        IIslandExtender
    {
        // If each interface would have a specialized implementor, these fields could become
        // read-only. However that would create a lot of extra boiler-plate code

        private ScenarioSelector ScenarioFilter;

        private IToolbarEntryInsertLocation ToolbarEntryInsertLocation;
        private IIslandModulesData ModulesData;
        private IIslandBuilder IslandBuilder;
        private IIslandGroupBuilder IslandGroupBuilder;
        private ISimulationExtender LazySimulationExtender;

        public IScenarioSelectiveIslandExtender SpecificScenarios(ScenarioSelector scenarioFilter)
        {
            ScenarioFilter = scenarioFilter;
            return this;
        }

        public IScenarioSelectiveIslandExtender AllScenarios()
        {
            ScenarioFilter = AllScenariosFunc;
            return this;

            bool AllScenariosFunc(GameScenario scenario)
            {
                return true;
            }
        }

        public IDefinedIslandExtender WithIsland(IIslandBuilder island,
            IIslandGroupBuilder islandGroup)
        {
            IslandBuilder = island;
            IslandGroupBuilder = islandGroup;
            return this;
        }

        IDefinedPlaceableIslandExtender IDefinedIslandExtender.WithDefaultPlacement()
        {
            return this;
        }

        IDefinedPlaceableAccessibleIslandExtender IDefinedPlaceableIslandExtender.InToolbar(
            IToolbarEntryInsertLocation toolbarEntryInsertLocation)
        {
            ToolbarEntryInsertLocation = toolbarEntryInsertLocation;
            return this;
        }

        public IAtomicIslandExtender WithSimulation
            <TSimulation, TState, TConfig>(IFactoryBuilder<TSimulation, TState, TConfig> factoryBuilder)
            where TSimulation : Simulation<TState> where TState : class, ISimulationState, new()
        {
            LazySimulationExtender =
                new TypedSimulationExtender<TSimulation, TState, TConfig>(ScenarioFilter, factoryBuilder);
            return this;
        }

        IAtomicIslandExtender IDefinedPlaceableAccessibleIslandExtender.WithoutSimulation()
        {
            return this;
        }

        IAtomicIslandExtender IDefinedIslandExtender.WithoutSimulation()
        {
            return this;
        }

        public IAtomicIslandExtender WithSimulation
            <TSimulation, TConfig, TBaseConfiguration, TSimulationConfiguration>(
                IFactoryBuilder<TSimulation, TConfig, TBaseConfiguration> factoryBuilder)
        {
            throw new NotImplementedException();
        }


        IDefinedSimulatablePlaceableIslandExtender IDefinedSimulatableIslandExtender.
            WithDefaultPlacement()
        {
            return this;
        }

        IAtomicIslandExtender IDefinedSimulatablePlaceableIslandExtender.InToolbar(
            IToolbarEntryInsertLocation toolbarEntryInsertLocation)
        {
            ToolbarEntryInsertLocation = toolbarEntryInsertLocation;
            return this;
        }

        public IIslandExtender WithCustomModules(IIslandModuleDataProvider islandModules)
        {
            ModulesData = new CustomIslandsModulesData(islandModules);
            return this;
        }

        public IIslandExtender WithoutModules()
        {
            ModulesData = new NoModulesData();
            return this;
        }


        public void Build()
        {
            BuildExtenders();

            // This method creates a tree of links for extending the game to add a island completely (base data,
            // simulation, placement, toolbar, modules).
            // Some of these extenders depend on data created on a previous step, but more importantly, they require
            // that the previous extension was applied to the game. Thus, this method create an extension logic tree
            // where the extenders are activated in the order they should. When all extenders are applied, the process
            // starts again (this happens, for example, when loading another savegame) 
            void BuildExtenders()
            {
                // Start the chain of extenders with the scenario extender. This also serves as a filter for only
                // applying the other extenders if the scenario is part of the filter 
                RewirerChainLink scenarioRewirer = RewirerChain
                   .BeginRewiringWith(new GameScenarioIslandRewirer(ScenarioFilter,
                        IslandGroupBuilder.GroupId));

                // Then add the island group and island to the game islands object
                RewirerChainLink<IslandDefinition> islandRewirer =
                    scenarioRewirer.ThenContinueRewiringWith(BuildIslandExtender);

                RewirerChainLink simulationsRewirer = null;
                if (LazySimulationExtender != null)
                {
                    // With the island added, create the simulation
                    simulationsRewirer = LazySimulationExtender.ContinueAfter(islandRewirer);
                }

                // With the island, create the placement
                RewirerChainLink<IslandPlacementResult> placementRewirer =
                    islandRewirer.ThenContinueRewiringWith(BuildDefaultPlacementExtender);

                // And with the placement, create a toolbar entry
                RewirerChainLink<PlacementToolbarElementData> toolbarRewirer =
                    placementRewirer.ThenContinueRewiringWith(BuildToolbarExtender(ToolbarEntryInsertLocation));

                // And finally add the modules
                RewirerChainLink modulesRewirer =
                    islandRewirer.ThenContinueRewiringWith(BuildModulesExtender);

                // When all extenders are called (noticed that the specific order does not matter), the process is
                // restarted
                IWaitAllRewirers modulesAndToolbar = AggregatedChain
                   .WaitFor(modulesRewirer)
                   .And(toolbarRewirer);

                IWaitAllRewirers allRewirersToWait =
                    simulationsRewirer == null ? modulesAndToolbar : modulesAndToolbar.And(simulationsRewirer);
                allRewirersToWait.AfterHijack.Register(OnApplyAllExtenders);
                return;

                void OnApplyAllExtenders()
                {
                    allRewirersToWait.AfterHijack.Unregister(OnApplyAllExtenders);
                    BuildExtenders();
                }
            }
        }

        private IslandsExtender BuildIslandExtender()
        {
            return new IslandsExtender(IslandBuilder, IslandGroupBuilder);
        }

        private DefaultIslandPlacementExtender BuildDefaultPlacementExtender(
            IslandDefinition def)
        {
            return new DefaultIslandPlacementExtender(def);
        }

        private Func<IslandPlacementResult, ToolbarRewirer> BuildToolbarExtender(
            IToolbarEntryInsertLocation entryInsertLocation)
        {
            return BuildToolbarExtenderFunc;

            ToolbarRewirer BuildToolbarExtenderFunc(
                IslandPlacementResult placementResult)
            {
                PlacementInitiatorId placement = placementResult.InitiatorId;
                IIslandDefinitionGroup group = placementResult.Island.CustomData.Get<IIslandDefinitionGroup>();

                GroupPresentationData presentation = group.CustomData.Get<GroupPresentationData>();

                return new ToolbarRewirer(placement,
                    presentation.Title,
                    presentation.Description,
                    presentation.Icon,
                    entryInsertLocation);
            }
        }

        private Func<IslandDefinition, IslandSimulationExtender<TSimulation, TState, TConfig>>
            BuildSimulationExtender<TSimulation, TState, TConfig>(
                IFactoryBuilder<TSimulation, TState, TConfig> factoryBuilder)
            where TSimulation : Simulation<TState> where TState : class, ISimulationState, new()
        {
            return BuildToolbarExtenderFunc;

            IslandSimulationExtender<TSimulation, TState, TConfig> BuildToolbarExtenderFunc(
                IslandDefinition islandDefinition)
            {
                return new IslandSimulationExtender<TSimulation, TState, TConfig>(ScenarioFilter,
                    islandDefinition.Id,
                    factoryBuilder);
            }
        }


        private IChainableRewirer BuildModulesExtender(IslandDefinition islandDefinition)
        {
            return new IslandModulesExtender(islandDefinition, ModulesData);
        }

        private class TypedSimulationExtender<TSimulation, TState, TConfig> : ISimulationExtender
            where TSimulation : Simulation<TState> where TState : class, ISimulationState, new()

        {
            private readonly ScenarioSelector ScenarioSelector;
            private readonly IFactoryBuilder<TSimulation, TState, TConfig> FactoryBuilder;

            public TypedSimulationExtender(ScenarioSelector scenarioSelector,
                IFactoryBuilder<TSimulation, TState, TConfig> factoryBuilder)
            {
                ScenarioSelector = scenarioSelector;
                FactoryBuilder = factoryBuilder;
            }

            public RewirerChainLink ContinueAfter(RewirerChainLink<IslandDefinition> rewirerChainLink)
            {
                return rewirerChainLink.ThenContinueRewiringWith(BuildToolbarExtenderFunc)
                   .ThenContinueRewiringWith(BuildBuffablesExtender);

                IslandSimulationExtender<TSimulation, TState, TConfig> BuildToolbarExtenderFunc(
                    IslandDefinition islandDefinition)
                {
                    return new IslandSimulationExtender<TSimulation, TState, TConfig>(ScenarioSelector,
                        islandDefinition.Id,
                        FactoryBuilder);
                }

                IChainableRewirer BuildBuffablesExtender(TConfig config)
                {
                    return new BuffablesExtender<TConfig>(config);
                }
            }
        }

        private interface ISimulationExtender
        {
            RewirerChainLink ContinueAfter(RewirerChainLink<IslandDefinition> rewirerChainLink);
        }
    }


    public interface IBaseIslandExtender
    {
        IScenarioSelectiveIslandExtender SpecificScenarios(ScenarioSelector scenarioFilter);

        IScenarioSelectiveIslandExtender AllScenarios();
    }

    // Scenario
    public interface IScenarioSelectiveIslandExtender
    {
        IDefinedIslandExtender WithIsland(IIslandBuilder island,
            IIslandGroupBuilder islandGroup);
    }

    // Scenario -> Island
    public interface IDefinedIslandExtender
    {
        IDefinedPlaceableIslandExtender WithDefaultPlacement();

        IAtomicIslandExtender WithSimulation<TSimulation,
            TState, TConfig>(
            IFactoryBuilder<TSimulation, TState, TConfig> factoryBuilder)
            where TSimulation : Simulation<TState> where TState : class, ISimulationState, new();

        IAtomicIslandExtender WithoutSimulation();

        IAtomicIslandExtender WithSimulation<
            TSimulation,
            TState, TBaseConfiguration, TSimulationConfiguration>(
            IFactoryBuilder<TSimulation, TState, TBaseConfiguration> factoryBuilder);
    }

    // Scenario -> Island -> Placement
    public interface IDefinedPlaceableIslandExtender
    {
        IDefinedPlaceableAccessibleIslandExtender InToolbar(
            IToolbarEntryInsertLocation entryInsertLocation);
    }

    // Scenario -> Island -> Placement -> Toolbar
    public interface IDefinedPlaceableAccessibleIslandExtender
    {
        IAtomicIslandExtender WithSimulation<TSimulation,
            TState, TConfig>(
            IFactoryBuilder<TSimulation, TState, TConfig> factoryBuilder)
            where TSimulation : Simulation<TState> where TState : class, ISimulationState, new();

        IAtomicIslandExtender WithoutSimulation();

        IAtomicIslandExtender WithSimulation<
            TSimulation,
            TState, TBaseConfiguration, TSimulationConfiguration>(
            IFactoryBuilder<TSimulation, TState, TBaseConfiguration> factoryBuilder);
    }


    // Scenario -> Island -> Simulation -> Buff
    public interface IDefinedSimulatableIslandExtender
    {
        IDefinedSimulatablePlaceableIslandExtender WithDefaultPlacement();
    }

    // Scenario -> Island -> Simulation -> Buff -> Placement
    public interface IDefinedSimulatablePlaceableIslandExtender
    {
        IAtomicIslandExtender InToolbar(IToolbarEntryInsertLocation entryInsertLocation);
    }

    public interface IAtomicIslandExtender
    {
        IIslandExtender WithCustomModules(IIslandModuleDataProvider islandModules);
        IIslandExtender WithoutModules();
    }

    public interface IIslandExtender
    {
        void Build();
    }
}