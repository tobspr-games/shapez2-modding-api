using System;
using System.Collections.Generic;
using System.Linq;
using Game.Orchestration;
using MonoMod.RuntimeDetour;
using ShapezShifter.SharpDetour;

namespace ShapezShifter.Hijack
{
    internal class SimulationSystemsInterceptor : IDisposable
    {
        private readonly IRewirerProvider RewirerProvider;
        private readonly Hook CreateSimulationSystemsHook;

        internal SimulationSystemsInterceptor(IRewirerProvider rewirerProvider)
        {
            RewirerProvider = rewirerProvider;

            CreateSimulationSystemsHook = DetourHelper
               .CreatePostfixHook<BuiltinSimulationSystems, IEnumerable<ISimulationSystem>>(
                    builtinSimulationSystems =>
                        builtinSimulationSystems.CreateSimulationSystems(),
                    CreateSimulationSystems);
        }

        private IEnumerable<ISimulationSystem> CreateSimulationSystems(
            BuiltinSimulationSystems builtinSimulationSystems,
            IEnumerable<ISimulationSystem> systems)
        {
            List<ISimulationSystem> systemsList = systems.ToList();
            IEnumerable<ISimulationSystemsRewirer> simulationSystemsRewirers =
                RewirerProvider.RewirersOfType<ISimulationSystemsRewirer>();
            SimulationSystemsDependencies dependencies = new(builtinSimulationSystems);
            foreach (ISimulationSystemsRewirer simulationSystemsRewirer in simulationSystemsRewirers)
            {
                simulationSystemsRewirer.ModifySimulationSystems(systemsList, dependencies);
            }

            return systemsList;
        }

        public void Dispose()
        {
            CreateSimulationSystemsHook.Dispose();
        }
    }
}