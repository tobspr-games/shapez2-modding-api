using Core.Factory;
using ShapezShifter.Hijack;

namespace ShapezShifter.Flow.Atomic
{
    public interface IFactoryBuilder<out TSimulation, in TState, TConfig>
    {
        IFactory<TState, TSimulation> BuildFactory(SimulationSystemsDependencies dependencies,
            out TConfig config);
    }
}