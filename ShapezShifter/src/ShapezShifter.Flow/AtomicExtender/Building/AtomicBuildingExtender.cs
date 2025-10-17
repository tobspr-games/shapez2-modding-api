using System;
using System.Diagnostics.CodeAnalysis;
using Game.Core.Simulation;
using JetBrains.Annotations;
using ShapezShifter.Flow.Research;
using ShapezShifter.Flow.Toolbar;
using ShapezShifter.Hijack;

namespace ShapezShifter.Flow.Atomic
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
        IBuildingExtender, IDefinedUnlockableBuildingExtender
    {
        // If each interface would have a specialized implementor, these fields could become
        // read-only. However that would create a lot of extra boiler-plate code

        private ScenarioSelector ScenarioFilter;

        private IToolbarEntryInsertLocation ToolbarEntryInsertLocation;
        private IBuildingModulesData ModulesData;
        private IBuildingBuilder BuildingBuilder;
        private IBuildingGroupBuilder BuildingGroupBuilder;
        private ISimulationExtender LazySimulationExtender;

        private IBuildingResearchProgressionExtender ProgressionExtender;

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


        IDefinedPlaceableAccessibleBuildingExtender IDefinedPlaceableBuildingExtender.InToolbar(
            IToolbarEntryInsertLocation toolbarEntryInsertLocation)
        {
            ToolbarEntryInsertLocation = toolbarEntryInsertLocation;
            return this;
        }


        public IAtomicBuildingExtender WithSimulation
            <TSimulation>(IFactoryBuilder<TSimulation> factoryBuilder)
            where TSimulation : ISimulation
        {
            LazySimulationExtender =
                new TypedSimulationExtender<TSimulation>(factoryBuilder);
            return this;
        }

        IDefinedPlaceableBuildingExtender IDefinedUnlockableBuildingExtender.WithDefaultPlacement()
        {
            return this;
        }

        public IAtomicBuildingExtender WithSimulation
            <TSimulation, TState, TConfig>(
                IFactoryBuilder<TSimulation, TState, TConfig> factoryBuilder)
            where TSimulation : ISimulation where TState : class, ISimulationState, new()
        {
            LazySimulationExtender =
                new TypedSimulationExtender<TSimulation, TState, TConfig>(factoryBuilder);
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
            ModulesData = new BuildingModulesData(speedId, processingDuration);
            return this;
        }

        public IBuildingExtender WithCustomModules(IBuildingModules buildingModules)
        {
            ModulesData = new CustomBuildingsModulesData(buildingModules);
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
                // Start the chain of extenders with the scenario extender. This also serves as a filter for only
                // applying the other extenders if the scenario is part of the filter 
                RewirerChainLink scenarioRewirer = RewirerChain
                   .BeginRewiringWith(new GameScenarioBuildingExtender(ScenarioFilter,
                        ProgressionExtender,
                        BuildingGroupBuilder.GroupId));

                // Then add the building group and building to the game buildings object
                RewirerChainLink<BuildingDefinition> buildingRewirer =
                    scenarioRewirer.ThenContinueRewiringWith(BuildBuildingExtender);

                // With the building added, create the simulation
                RewirerChainLink simulationsRewirer = LazySimulationExtender.ContinueAfter(buildingRewirer);

                // With the building, create the placement
                RewirerChainLink<BuildingPlacementResult> placementRewirer =
                    buildingRewirer.ThenContinueRewiringWith(BuildDefaultPlacementExtender);

                // And with the placement, create a toolbar entry
                RewirerChainLink<PlacementToolbarElementData> toolbarRewirer =
                    placementRewirer.ThenContinueRewiringWith(BuildToolbarExtender(ToolbarEntryInsertLocation));

                // And finally add the modules
                RewirerChainLink modulesRewirer =
                    buildingRewirer.ThenContinueRewiringWith(BuildModulesExtender);

                // When all extenders are called (noticed that the specific order does not matter), the process is
                // restarted
                IWaitAllRewirers allRewirers = AggregatedChain
                   .WaitFor(simulationsRewirer)
                   .And(toolbarRewirer)
                   .And(modulesRewirer);
                allRewirers.AfterHijack.Register(OnApplyAllExtenders);
                return;

                void OnApplyAllExtenders()
                {
                    allRewirers.AfterHijack.Unregister(OnApplyAllExtenders);
                    BuildExtenders();
                }
            }
        }

        private BuildingsExtender BuildBuildingExtender()
        {
            return new BuildingsExtender(BuildingBuilder, BuildingGroupBuilder);
        }

        private DefaultBuildingPlacementExtender BuildDefaultPlacementExtender(
            BuildingDefinition def)
        {
            return new DefaultBuildingPlacementExtender(def);
        }

        private Func<BuildingPlacementResult, ToolbarRewirer> BuildToolbarExtender(
            IToolbarEntryInsertLocation entryInsertLocation)
        {
            return BuildToolbarExtenderFunc;

            ToolbarRewirer BuildToolbarExtenderFunc(
                BuildingPlacementResult placementResult)
            {
                PlacementInitiatorId placement = placementResult.InitiatorId;
                IBuildingDefinitionGroup group = placementResult.Building.CustomData.Get<IBuildingDefinitionGroup>();

                return new ToolbarRewirer(placement,
                    group.Title,
                    group.Description,
                    group.Icon,
                    entryInsertLocation);
            }
        }

        private Func<BuildingDefinition, BuildingSimulationExtender<TSimulation, TState, TConfig>>
            BuildSimulationExtender<TSimulation, TState, TConfig>(
                IFactoryBuilder<TSimulation, TState, TConfig> factoryBuilder)
            where TSimulation : ISimulation where TState : class, ISimulationState, new()
        {
            return BuildToolbarExtenderFunc;

            BuildingSimulationExtender<TSimulation, TState, TConfig> BuildToolbarExtenderFunc(
                BuildingDefinition buildingDefinition)
            {
                return new BuildingSimulationExtender<TSimulation, TState, TConfig>(buildingDefinition.Id,
                    factoryBuilder);
            }
        }


        private IChainableRewirer BuildModulesExtender(BuildingDefinition buildingDefinition)
        {
            return new BuildingModulesExtender(buildingDefinition, ModulesData);
        }

        private class TypedSimulationExtender<TSimulation> : ISimulationExtender
            where TSimulation : ISimulation
        {
            private readonly IFactoryBuilder<TSimulation> FactoryBuilder;

            public TypedSimulationExtender(
                IFactoryBuilder<TSimulation> factoryBuilder)
            {
                FactoryBuilder = factoryBuilder;
            }

            public RewirerChainLink ContinueAfter(RewirerChainLink<BuildingDefinition> rewirerChainLink)
            {
                return rewirerChainLink.ThenContinueRewiringWith(BuildToolbarExtenderFunc);

                BuildingSimulationExtender<TSimulation> BuildToolbarExtenderFunc(
                    BuildingDefinition buildingDefinition)
                {
                    return new BuildingSimulationExtender<TSimulation>(buildingDefinition.Id,
                        FactoryBuilder);
                }
            }
        }

        private class TypedSimulationExtender<TSimulation, TState, TConfig> : ISimulationExtender
            where TSimulation : ISimulation where TState : class, ISimulationState, new()

        {
            private readonly IFactoryBuilder<TSimulation, TState, TConfig> FactoryBuilder;

            public TypedSimulationExtender(IFactoryBuilder<TSimulation, TState, TConfig> factoryBuilder)
            {
                FactoryBuilder = factoryBuilder;
            }

            public RewirerChainLink ContinueAfter(RewirerChainLink<BuildingDefinition> rewirerChainLink)
            {
                return rewirerChainLink.ThenContinueRewiringWith(BuildToolbarExtenderFunc)
                   .ThenContinueRewiringWith(BuildBuffablesExtender);

                BuildingSimulationExtender<TSimulation, TState, TConfig> BuildToolbarExtenderFunc(
                    BuildingDefinition buildingDefinition)
                {
                    return new BuildingSimulationExtender<TSimulation, TState, TConfig>(buildingDefinition.Id,
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
            RewirerChainLink ContinueAfter(RewirerChainLink<BuildingDefinition> rewirerChainLink);
        }

        public IDefinedUnlockableBuildingExtender UnlockedAtMilestone(IMilestoneSelector milestoneSelector)
        {
            ProgressionExtender = new UnlockBuildingWithMilestoneResearchProgressionExtender(milestoneSelector);
            return this;
        }

        public IDefinedUnlockableBuildingExtender UnlockedWithNewSideUpgrade(
            IPresentableUnlockableSideUpgradeBuilder sideUpgradeBuilder)
        {
            ProgressionExtender = new UnlockBuildingWithNewSideUpgradeResearchProgressionExtender(sideUpgradeBuilder);
            return this;
        }

        public IDefinedUnlockableBuildingExtender UnlockedWithExistingSideUpgrade(
            ISideUpgradeSelector sideUpgradeSelector)
        {
            ProgressionExtender =
                new UnlockBuildingWithExistingSideUpgradeResearchProgressionExtender(sideUpgradeSelector);
            return this;
        }

        private enum ResearchProgression
        {
            Milestone,
            NewSideUpgrade,
            ExistingSideUpgrade
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
        IDefinedUnlockableBuildingExtender UnlockedAtMilestone(IMilestoneSelector milestoneSelector);

        IDefinedUnlockableBuildingExtender UnlockedWithNewSideUpgrade(
            IPresentableUnlockableSideUpgradeBuilder sideUpgradeBuilder);

        IDefinedUnlockableBuildingExtender UnlockedWithExistingSideUpgrade(ISideUpgradeSelector sideUpgradeSelector);
    }

    // Scenario -> Building -> Research

    public interface IDefinedUnlockableBuildingExtender
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