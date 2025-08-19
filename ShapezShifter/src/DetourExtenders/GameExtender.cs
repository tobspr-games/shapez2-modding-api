using System;
using Core.Logging;

namespace ShapezShifter
{
    internal class GameExtender : IDisposable
    {
        private readonly ToolbarExtender ToolbarExtender;
        private readonly BuildingsPlacementInitiatorsExtender BuildingsPlacementInitiatorsExtender;
        private readonly BuildingsExtender BuildingsExtender;
        private readonly BuildingModulesExtender BuildingModulesExtender;
        private readonly GameScenarioExtender GameScenarioExtender;
        private readonly SimulationSystemsExtender SimulationSystemsExtender;
        private readonly BuffablesExtender BuffablesExtender;

        public GameExtender(IExtendersProvider extendersProvider, ILogger logger)
        {
            ToolbarExtender = new ToolbarExtender(extendersProvider);
            BuildingsPlacementInitiatorsExtender =
                new BuildingsPlacementInitiatorsExtender(extendersProvider);
            BuildingsExtender = new BuildingsExtender(extendersProvider, logger);
            BuildingModulesExtender = new BuildingModulesExtender(extendersProvider);
            GameScenarioExtender = new GameScenarioExtender(extendersProvider, logger);
            SimulationSystemsExtender = new SimulationSystemsExtender(extendersProvider);
            BuffablesExtender = new BuffablesExtender(extendersProvider);
        }

        public void Dispose()
        {
            GameScenarioExtender.Dispose();
            BuildingModulesExtender.Dispose();
            BuildingsExtender.Dispose();
            BuildingsPlacementInitiatorsExtender.Dispose();
            ToolbarExtender.Dispose();
            SimulationSystemsExtender.Dispose();
            BuffablesExtender.Dispose();
        }
    }
}