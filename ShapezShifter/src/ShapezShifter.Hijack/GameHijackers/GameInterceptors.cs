using System;
using Core.Logging;

namespace ShapezShifter.Hijack
{
    internal class GameInterceptors : IDisposable
    {
        private readonly ToolbarInterceptor ToolbarInterceptor;
        private readonly PlacementInitiatorsInterceptor PlacementInterceptor;
        private readonly BuildingsInterceptor BuildingsInterceptor;
        private readonly BuildingModulesInterceptor BuildingModulesInterceptor;
        private readonly GameScenarioInterceptor GameScenarioInterceptor;
        private readonly SimulationSystemsInterceptor SimulationSystemsInterceptor;
        private readonly BuffablesInterceptor BuffablesInterceptor;
        private readonly IslandsInterceptor IslandsInterceptor;
        private readonly IslandModulesInterceptor IslandModulesInterceptor;

        public GameInterceptors(IRewirerProvider rewirerProvider, ILogger logger)
        {
            ToolbarInterceptor = new ToolbarInterceptor(rewirerProvider);
            PlacementInterceptor = new PlacementInitiatorsInterceptor(rewirerProvider);
            BuildingsInterceptor = new BuildingsInterceptor(rewirerProvider, logger);
            BuildingModulesInterceptor = new BuildingModulesInterceptor(rewirerProvider);
            IslandsInterceptor = new IslandsInterceptor(rewirerProvider, logger);
            IslandModulesInterceptor = new IslandModulesInterceptor(rewirerProvider);
            GameScenarioInterceptor = new GameScenarioInterceptor(rewirerProvider, logger);
            SimulationSystemsInterceptor = new SimulationSystemsInterceptor(rewirerProvider);
            BuffablesInterceptor = new BuffablesInterceptor(rewirerProvider);
        }

        public void Dispose()
        {
            GameScenarioInterceptor.Dispose();
            BuildingModulesInterceptor.Dispose();
            BuildingsInterceptor.Dispose();
            PlacementInterceptor.Dispose();
            ToolbarInterceptor.Dispose();
            SimulationSystemsInterceptor.Dispose();
            BuffablesInterceptor.Dispose();
            IslandsInterceptor.Dispose();
            IslandModulesInterceptor.Dispose();
        }
    }
}