using Core.Factory;

namespace ShapezShifter.Fluent.Atomic
{
    public interface IFactoryBuilder<out TSimulation, in TState, TConfig>
    {
        IFactory<TState, TSimulation> BuildFactory(SimulationSystemsDependencies dependencies,
            out TConfig config);
    }
}