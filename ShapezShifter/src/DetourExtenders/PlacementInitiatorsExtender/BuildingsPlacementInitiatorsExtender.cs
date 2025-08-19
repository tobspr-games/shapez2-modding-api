using System;
using System.Collections.Generic;
using MonoMod.RuntimeDetour;

namespace ShapezShifter
{
    internal class BuildingsPlacementInitiatorsExtender : IDisposable
    {
        private readonly IExtendersProvider ExtendersProvider;
        private readonly Hook ShapeBuildingsRegisterPlacersHook;
        private readonly Hook FluidBuildingsRegisterPlacersHook;
        private readonly Hook SignalBuildingsRegisterPlacersHook;
        private readonly Hook DecorationsBuildingsRegisterPlacersHook;
        private readonly Hook SandboxBuildingsRegisterPlacersHook;

        internal BuildingsPlacementInitiatorsExtender(IExtendersProvider extendersProvider)
        {
            ExtendersProvider = extendersProvider;

            ShapeBuildingsRegisterPlacersHook =
                CreateDetour<ShapeBuildingsPlacersCreator,
                    IShapeBuildingsPlacementInitiatorsExtender>();

            FluidBuildingsRegisterPlacersHook =
                CreateDetour<FluidBuildingsPlacersCreator,
                    IFluidBuildingsPlacementInitiatorsExtender>();

            SignalBuildingsRegisterPlacersHook =
                CreateDetour<SignalBuildingsPlacersCreator,
                    ISignalBuildingsPlacementInitiatorsExtender>();

            DecorationsBuildingsRegisterPlacersHook =
                CreateDetour<DecorationBuildingsPlacersCreator,
                    IDecorationBuildingsPlacementInitiatorsExtender>();

            SandboxBuildingsRegisterPlacersHook =
                CreateDetour<SandboxBuildingsPlacersCreator,
                    ISandboxBuildingsPlacementInitiatorsExtender>();

            return;

            Hook CreateDetour<TCreator, TExtender>() where TCreator : BuildingPlacersCreator
                where TExtender : IBuildingsPlacementInitiatorsExtender
            {
                return DetourHelper
                    .CreatePostfixHook<TCreator, IPlacementInitiatorIdRegistry,
                        ICollection<IDisposable>>((creator, registry, disposables) =>
                            creator.RegisterPlacers(registry, disposables),
                        RegisterPlacers<TCreator, TExtender>);
            }
        }

        private void RegisterPlacers<TCreator, TExtender>(TCreator creator,
            IPlacementInitiatorIdRegistry initiatorIdRegistry,
            ICollection<IDisposable> disposables) where TCreator : BuildingPlacersCreator
            where TExtender : IBuildingsPlacementInitiatorsExtender
        {
            var initiatorsExtenders =
                ExtendersProvider.ExtendersOfType<TExtender>();
            BuildingInitiatorsParams @params = new(creator);
            foreach (TExtender initiatorsExtender in initiatorsExtenders)
            {
                initiatorsExtender.ExtendBuildingInitiators(@params, initiatorIdRegistry);
            }
        }

        public void Dispose()
        {
            ShapeBuildingsRegisterPlacersHook.Dispose();
            FluidBuildingsRegisterPlacersHook.Dispose();
            SignalBuildingsRegisterPlacersHook.Dispose();
            DecorationsBuildingsRegisterPlacersHook.Dispose();
            SandboxBuildingsRegisterPlacersHook.Dispose();
        }
    }
}