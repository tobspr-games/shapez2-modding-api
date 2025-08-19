using System;
using System.Collections.Generic;
using System.Linq;
using Game.Orchestration;
using MonoMod.RuntimeDetour;

namespace ShapezShifter
{
    internal class SimulationSystemsExtender : IDisposable
    {
        private readonly IExtendersProvider ExtendersProvider;
        private readonly Hook CreateSimulationSystemsHook;

        internal SimulationSystemsExtender(IExtendersProvider extendersProvider)
        {
            ExtendersProvider = extendersProvider;

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
            var systemsList = systems.ToList();
            var initiatorsExtenders =
                ExtendersProvider.ExtendersOfType<ISimulationSystemsExtender>();
            SimulationSystemsDependencies dependencies = new(builtinSimulationSystems);
            foreach (ISimulationSystemsExtender initiatorsExtender in initiatorsExtenders)
            {
                initiatorsExtender.ExtendSimulationSystems(systemsList, dependencies);
            }

            return systemsList;
        }

        public void Dispose()
        {
            CreateSimulationSystemsHook.Dispose();
        }
    }
}