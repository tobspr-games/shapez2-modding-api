using System;
using Core.Logging;

namespace ShapezShifter.Hijack
{
    internal class GameInterceptors : IDisposable
    {
        private readonly ToolbarInterceptor ToolbarInterceptor;
        private readonly BuildingsPlacementInitiatorsInterceptor BuildingsPlacementInterceptor;
        private readonly BuildingsInterceptor BuildingsInterceptor;
        private readonly BuildingModulesInterceptor BuildingModulesInterceptor;
        private readonly GameScenarioInterceptor GameScenarioInterceptor;
        private readonly SimulationSystemsInterceptor SimulationSystemsInterceptor;
        private readonly BuffablesInterceptor BuffablesInterceptor;

        public GameInterceptors(IRewirerProvider rewirerProvider, ILogger logger)
        {
            ToolbarInterceptor = new ToolbarInterceptor(rewirerProvider);
            BuildingsPlacementInterceptor = new BuildingsPlacementInitiatorsInterceptor(rewirerProvider);
            BuildingsInterceptor = new BuildingsInterceptor(rewirerProvider, logger);
            BuildingModulesInterceptor = new BuildingModulesInterceptor(rewirerProvider);
            GameScenarioInterceptor = new GameScenarioInterceptor(rewirerProvider, logger);
            SimulationSystemsInterceptor = new SimulationSystemsInterceptor(rewirerProvider);
            BuffablesInterceptor = new BuffablesInterceptor(rewirerProvider);
        }

        public void Dispose()
        {
            GameScenarioInterceptor.Dispose();
            BuildingModulesInterceptor.Dispose();
            BuildingsInterceptor.Dispose();
            BuildingsPlacementInterceptor.Dispose();
            ToolbarInterceptor.Dispose();
            SimulationSystemsInterceptor.Dispose();
            BuffablesInterceptor.Dispose();
        }
    }
}