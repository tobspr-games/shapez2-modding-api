using System.Collections.Generic;

namespace ShapezShifter
{
    public interface ISimulationSystemsExtender : IExtender
    {
        void ExtendSimulationSystems(ICollection<ISimulationSystem> simulationSystems,
            SimulationSystemsDependencies dependencies);
    }
}