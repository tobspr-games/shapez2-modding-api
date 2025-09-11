using System;
using Game.Core.Simulation;
using ShapezShifter.Fluent.Toolbar;

namespace ShapezShifter.Fluent.Atomic
{
    public class AtomicBuildingExtender :
        IBaseBuildingExtender,
        IScenarioSelectiveBuildingExtender,
        IDefinedBuildingExtender,
        IDefinedPlaceableBuildingExtender,
        IDefinedPlaceableAccessibleBuildingExtender,
        IDefinedSimulatableBuildingExtender,
        IDefinedSimulatablePlaceableBuildingExtender,
        IAtomicBuildingExtender,
        IBuildingExtender
    {
        // If each interface would have a specialized implementor, these fields could become
        // read-only. However that would create a lot of extra boiler-plate code

        private ScenarioSelector ScenarioFilter;

        private IToolbarEntryInsertLocation ToolbarEntryInsertLocation;
        private IModulesData ModulesData;
        private IBuildingBuilder BuildingBuilder;
        private IBuildingGroupBuilder BuildingGroupBuilder;
        private ISimulationExtender LazySimulationExtender;

        public IScenarioSelectiveBuildingExtender SpecificScenarios(ScenarioSelector scenarioFilter)
        {
            ScenarioFilter = scenarioFilter;
            return this;
        }

        public IScenarioSelectiveBuildingExtender AllScenarios()
        {
            ScenarioFilter = AllScenariosFunc;
            return this;

            bool AllScenariosFunc(GameScenario scenario)
            {
                return true;
            }
        }

        public IDefinedBuildingExtender WithBuilding(IBuildingBuilder building,
            IBuildingGroupBuilder buildingGroup)
        {
            BuildingBuilder = building;
            BuildingGroupBuilder = buildingGroup;
            return this;
        }

        IDefinedPlaceableBuildingExtender IDefinedBuildingExtender.WithDefaultPlacement()
        {
            return this;
        }

        IDefinedPlaceableAccessibleBuildingExtender IDefinedPlaceableBuildingExtender.InToolbar(
            IToolbarEntryInsertLocation toolbarEntryInsertLocation)
        {
            ToolbarEntryInsertLocation = toolbarEntryInsertLocation;
            return this;
        }

        public IAtomicBuildingExtender WithSimulation
            <TSimulation, TState, TConfig>(
                IFactoryBuilder<TSimulation, TState, TConfig> factoryBuilder)
            where TSimulation : ISimulation where TState : class, ISimulationState, new()
        {
            LazySimulationExtender =
                new TypedSimulationExtender<TSimulation, TState, TConfig>(ScenarioFilter, factoryBuilder);
            return this;
        }

        public IAtomicBuildingExtender WithSimulation
            <TSimulation, TConfig, TBaseConfiguration, TSimulationConfiguration>(
                IFactoryBuilder<TSimulation, TConfig, TBaseConfiguration> factoryBuilder)
        {
            throw new NotImplementedException();
        }


        IDefinedSimulatablePlaceableBuildingExtender IDefinedSimulatableBuildingExtender.
            WithDefaultPlacement()
        {
            return this;
        }

        IAtomicBuildingExtender IDefinedSimulatablePlaceableBuildingExtender.InToolbar(
            IToolbarEntryInsertLocation toolbarEntryInsertLocation)
        {
            ToolbarEntryInsertLocation = toolbarEntryInsertLocation;
            return this;
        }

        public IBuildingExtender WithAtomicShapeProcessingModules(ResearchSpeedId speedId, float processingDuration)
        {
            ModulesData = new AtomicShapeProcessingModulesData(speedId, processingDuration);
            return this;
        }

        public IBuildingExtender WithCustomModules(IBuildingModules buildingModules)
        {
            ModulesData = new CustomModulesData(buildingModules);
            return this;
        }


        public void Build()
        {
            BuildExtenders();

            // This method creates a tree of links for extending the game to add a building completely (base data,
            // simulation, placement, toolbar, modules).
            // Some of these extenders depend on data created on a previous step, but more importantly, they require
            // that the previous extension was applied to the game. Thus, this method create an extension logic tree
            // where the extenders are activated in the order they should. When all extenders are applied, the process
            // starts again (this happens, for example, when loading another savegame) 
            void BuildExtenders()
            {
                Debugging.Logger.Info?.Log("Building extenders");

                // Start the chain of extenders with the scenario extender. This also serves as a filter for only
                // applying the other extenders if the scenario is part of the filter 
                var scenarioExtender = ExtenderChain
                   .BeginExtendingWith(new GameScenarioExtender(ScenarioFilter,
                        BuildingGroupBuilder.GroupId));

                // Then add the building group and building to the game buildings object
                var buildingExtender = scenarioExtender.ThenContinueExtendingWith(BuildBuildingPlacementExtender);

                // With the building added, create the simulation
                var simulationsExtender = LazySimulationExtender.ContinueAfter(buildingExtender);

                // With the building, create the placement
                var placementExtender =
                    buildingExtender.ThenContinueExtendingWith(BuildDefaultPlacementExtender);

                // And with the placement, create a toolbar entry
                var toolbarExtender =
                    placementExtender.ThenContinueExtendingWith(BuildToolbarExtender(ToolbarEntryInsertLocation));

                // And finally add the modules
                var modulesExtender = buildingExtender.ThenContinueExtendingWith(BuildModulesExtender);

                // When all extenders are called (noticed that the specific order does not matter), the process is
                // restarted
                var allExtenders = AggregatedChain
                   .WaitFor(simulationsExtender)
                   .And(toolbarExtender)
                   .And(modulesExtender);
                allExtenders.AfterExtensionApplied.Register(OnApplyAllExtenders);
                return;

                void OnApplyAllExtenders()
                {
                    allExtenders.AfterExtensionApplied.Unregister(OnApplyAllExtenders);
                    BuildExtenders();
                }
            }
        }

        private BuildingsExtender BuildBuildingPlacementExtender()
        {
            return new BuildingsExtender(BuildingBuilder, BuildingGroupBuilder);
        }

        private DefaultPlacementExtender BuildDefaultPlacementExtender(
            BuildingDefinition def)
        {
            return new DefaultPlacementExtender(def);
        }

        private Func<PlacementResult, ToolbarExtender> BuildToolbarExtender(
            IToolbarEntryInsertLocation entryInsertLocation)
        {
            return BuildToolbarExtenderFunc;

            ToolbarExtender BuildToolbarExtenderFunc(
                PlacementResult placementResult)
            {
                var placement = placementResult.InitiatorId;
                var group = placementResult.Building.CustomData.Get<IBuildingDefinitionGroup>();

                return new ToolbarExtender(placement,
                    group.Title,
                    group.Description,
                    group.Icon,
                    entryInsertLocation);
            }
        }

        private Func<BuildingDefinition, SimulationExtender<TSimulation, TState, TConfig>>
            BuildSimulationExtender<TSimulation, TState, TConfig>(
                IFactoryBuilder<TSimulation, TState, TConfig> factoryBuilder)
            where TSimulation : ISimulation where TState : class, ISimulationState, new()
        {
            return BuildToolbarExtenderFunc;

            SimulationExtender<TSimulation, TState, TConfig> BuildToolbarExtenderFunc(
                BuildingDefinition buildingDefinition)
            {
                return new SimulationExtender<TSimulation, TState, TConfig>(ScenarioFilter,
                    buildingDefinition.Id,
                    factoryBuilder);
            }
        }


        private IChainableExtender BuildModulesExtender(BuildingDefinition buildingDefinition)
        {
            return new ModulesExtender(buildingDefinition, ModulesData);
        }

        private class TypedSimulationExtender<TSimulation, TState, TConfig> : ISimulationExtender
            where TSimulation : ISimulation where TState : class, ISimulationState, new()

        {
            private readonly ScenarioSelector ScenarioSelector;
            private readonly IFactoryBuilder<TSimulation, TState, TConfig> FactoryBuilder;

            public TypedSimulationExtender(ScenarioSelector scenarioSelector,
                IFactoryBuilder<TSimulation, TState, TConfig> factoryBuilder)
            {
                ScenarioSelector = scenarioSelector;
                FactoryBuilder = factoryBuilder;
            }

            public ExtenderChainLink ContinueAfter(ExtenderChainLink<BuildingDefinition> extenderChainLink)
            {
                return extenderChainLink.ThenContinueExtendingWith(BuildToolbarExtenderFunc)
                   .ThenContinueExtendingWith(BuildBuffablesExtender);

                SimulationExtender<TSimulation, TState, TConfig> BuildToolbarExtenderFunc(
                    BuildingDefinition buildingDefinition)
                {
                    return new SimulationExtender<TSimulation, TState, TConfig>(ScenarioSelector,
                        buildingDefinition.Id,
                        FactoryBuilder);
                }

                IChainableExtender BuildBuffablesExtender(TConfig config)
                {
                    return new BuffablesExtender<TConfig>(config);
                }
            }
        }

        private interface ISimulationExtender
        {
            ExtenderChainLink ContinueAfter(ExtenderChainLink<BuildingDefinition> extenderChainLink);
        }
    }


    public interface IBaseBuildingExtender
    {
        IScenarioSelectiveBuildingExtender SpecificScenarios(ScenarioSelector scenarioFilter);

        IScenarioSelectiveBuildingExtender AllScenarios();
    }

    // Scenario
    public interface IScenarioSelectiveBuildingExtender
    {
        IDefinedBuildingExtender WithBuilding(IBuildingBuilder building,
            IBuildingGroupBuilder buildingGroup);
    }

    // Scenario -> Building
    public interface IDefinedBuildingExtender
    {
        IDefinedPlaceableBuildingExtender WithDefaultPlacement();

        IAtomicBuildingExtender WithSimulation<TSimulation,
            TState, TConfig>(
            IFactoryBuilder<TSimulation, TState, TConfig> factoryBuilder)
            where TSimulation : ISimulation where TState : class, ISimulationState, new();

        IAtomicBuildingExtender WithSimulation<
            TSimulation,
            TState, TBaseConfiguration, TSimulationConfiguration>(
            IFactoryBuilder<TSimulation, TState, TBaseConfiguration> factoryBuilder);
    }

    // Scenario -> Building -> Placement
    public interface IDefinedPlaceableBuildingExtender
    {
        IDefinedPlaceableAccessibleBuildingExtender InToolbar(
            IToolbarEntryInsertLocation entryInsertLocation);
    }

    // Scenario -> Building -> Placement -> Toolbar
    public interface IDefinedPlaceableAccessibleBuildingExtender
    {
        IAtomicBuildingExtender WithSimulation<TSimulation,
            TState, TConfig>(
            IFactoryBuilder<TSimulation, TState, TConfig> factoryBuilder)
            where TSimulation : ISimulation where TState : class, ISimulationState, new();

        IAtomicBuildingExtender WithSimulation<
            TSimulation,
            TState, TBaseConfiguration, TSimulationConfiguration>(
            IFactoryBuilder<TSimulation, TState, TBaseConfiguration> factoryBuilder);
    }


    // Scenario -> Building -> Simulation -> Buff
    public interface IDefinedSimulatableBuildingExtender
    {
        IDefinedSimulatablePlaceableBuildingExtender WithDefaultPlacement();
    }

    // Scenario -> Building -> Simulation -> Buff -> Placement
    public interface IDefinedSimulatablePlaceableBuildingExtender
    {
        IAtomicBuildingExtender InToolbar(IToolbarEntryInsertLocation entryInsertLocation);
    }

    public interface IAtomicBuildingExtender
    {
        IBuildingExtender WithAtomicShapeProcessingModules(ResearchSpeedId speedId, float processingDuration);
        IBuildingExtender WithCustomModules(IBuildingModules buildingModules);
    }

    public interface IBuildingExtender
    {
        void Build();
    }
}