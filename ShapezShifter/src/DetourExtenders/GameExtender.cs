using System;
using Core.Logging;

namespace ShapezShifter
{
    internal class GameExtender : IDisposable
    {
        private readonly ToolbarExtender ToolbarExtender;
        private readonly PlacementInitiatorsExtender PlacementInitiatorsExtender;
        private readonly BuildingsExtender BuildingsExtender;
        private readonly BuildingModulesExtender BuildingModulesExtender;
        private readonly GameScenarioExtender GameScenarioExtender;
        private readonly SimulationSystemsExtender SimulationSystemsExtender;
        private readonly BuffablesExtender BuffablesExtender;

        public GameExtender(IExtendersProvider extendersProvider, ILogger logger)
        {
            ToolbarExtender = new ToolbarExtender(extendersProvider);
            PlacementInitiatorsExtender = new PlacementInitiatorsExtender(extendersProvider);
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
            PlacementInitiatorsExtender.Dispose();
            ToolbarExtender.Dispose();
            SimulationSystemsExtender.Dispose();
            BuffablesExtender.Dispose();
        }
    }
}