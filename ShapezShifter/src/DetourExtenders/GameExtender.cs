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
        private readonly ResearchExtender ResearchExtender;

        public GameExtender(IExtendersProvider extendersProvider, ILogger logger)
        {
            ToolbarExtender = new ToolbarExtender(extendersProvider);
            PlacementInitiatorsExtender = new PlacementInitiatorsExtender(extendersProvider);
            BuildingsExtender = new BuildingsExtender(extendersProvider, logger);
            BuildingModulesExtender = new BuildingModulesExtender(extendersProvider);
            ResearchExtender = new ResearchExtender(extendersProvider, logger);
        }

        public void Dispose()
        {
            ResearchExtender.Dispose();
            BuildingModulesExtender.Dispose();
            BuildingsExtender.Dispose();
            PlacementInitiatorsExtender.Dispose();
            ToolbarExtender.Dispose();
        }
    }
}