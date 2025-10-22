using Game.Content.Features.Fluids;
using Game.Content.Features.Signals.Channels;
using Game.Core.Simulation;
using Game.Orchestration;

namespace ShapezShifter.Hijack
{
    public class SimulationSystemsDependencies
    {
        public readonly Ticks InitialSimulationTime;
        public readonly GameMode Mode;
        public readonly IShapeRegistry ShapeRegistry;
        public readonly IGameResourcesMap ResourcesMap;
        public readonly IFluidRegistry FluidRegistry;
        public readonly FluidPackageItemSolver FluidPackagesItem;
        public readonly ISignalChannelRegistry SignalChannelRegistry;
        public readonly IResearchUnlockManager ResearchUnlockManager;

        public SimulationSystemsDependencies(BuiltinSimulationSystems builtinSimulationSystems)
        {
            InitialSimulationTime = builtinSimulationSystems.InitialSimulationTime;
            Mode = builtinSimulationSystems.Mode;
            ShapeRegistry = builtinSimulationSystems.ShapeRegistry;
            ResourcesMap = builtinSimulationSystems.ResourcesMap;
            FluidRegistry = builtinSimulationSystems.FluidRegistry;
            FluidPackagesItem = builtinSimulationSystems.FluidPackagesItem;
            SignalChannelRegistry = builtinSimulationSystems.SignalChannelRegistry;
            ResearchUnlockManager = builtinSimulationSystems.ResearchUnlockManager;
        }

        public SimulationSystemsDependencies(Ticks initialSimulationTime, GameMode mode, IShapeRegistry shapeRegistry,
            IGameResourcesMap resourcesMap, IFluidRegistry fluidRegistry, FluidPackageItemSolver fluidPackagesItem,
            ISignalChannelRegistry signalChannelRegistry, IResearchUnlockManager researchUnlockManager)
        {
            InitialSimulationTime = initialSimulationTime;
            Mode = mode;
            ShapeRegistry = shapeRegistry;
            ResourcesMap = resourcesMap;
            FluidRegistry = fluidRegistry;
            FluidPackagesItem = fluidPackagesItem;
            SignalChannelRegistry = signalChannelRegistry;
            ResearchUnlockManager = researchUnlockManager;
        }
    }
}