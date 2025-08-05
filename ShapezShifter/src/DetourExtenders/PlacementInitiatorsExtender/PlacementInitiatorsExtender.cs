using System;
using System.Collections.Generic;
using MonoMod.RuntimeDetour;

namespace ShapezShifter
{
    internal class PlacementInitiatorsExtender : IDisposable
    {
        private readonly IExtendersProvider ExtendersProvider;
        private readonly Hook RegisterPlacersHook;

        internal PlacementInitiatorsExtender(IExtendersProvider extendersProvider)
        {
            ExtendersProvider = extendersProvider;

            RegisterPlacersHook = DetourHelper
                .CreatePostfixHook<ShapeBuildingsPlacersCreator, IPlacementInitiatorIdRegistry,
                    ICollection<IDisposable>>((creator, registry, disposables) =>
                    creator.RegisterPlacers(registry, disposables), RegisterPlacers);
        }

        private void RegisterPlacers(ShapeBuildingsPlacersCreator creator,
            IPlacementInitiatorIdRegistry initiatorIdRegistry,
            ICollection<IDisposable> disposables)
        {
            var initiatorsExtenders = ExtendersProvider.ExtendersOfType<IPlacementInitiatorsExtender>();
            foreach (var initiatorsExtender in initiatorsExtenders)
            {
                initiatorsExtender.ExtendInitiators(creator.EntityPlacementRunner, initiatorIdRegistry);
            }
        }

        public void Dispose()
        {
            RegisterPlacersHook.Dispose();
        }
    }
}