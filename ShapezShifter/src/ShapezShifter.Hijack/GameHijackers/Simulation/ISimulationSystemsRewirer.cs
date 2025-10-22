using System.Collections.Generic;

namespace ShapezShifter.Hijack
{
    public interface ISimulationSystemsRewirer : IRewirer
    {
        void ModifySimulationSystems(ICollection<ISimulationSystem> simulationSystems,
            SimulationSystemsDependencies dependencies);
    }
}