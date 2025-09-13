using System;
using System.Collections.Generic;
using MonoMod.RuntimeDetour;
using ShapezShifter.SharpDetour;

namespace ShapezShifter.Hijack
{
    internal class BuildingsPlacementInitiatorsInterceptor : IDisposable
    {
        private readonly IRewirerProvider RewirerProvider;
        private readonly Hook ShapeBuildingsRegisterPlacersHook;
        private readonly Hook FluidBuildingsRegisterPlacersHook;
        private readonly Hook SignalBuildingsRegisterPlacersHook;
        private readonly Hook DecorationsBuildingsRegisterPlacersHook;
        private readonly Hook SandboxBuildingsRegisterPlacersHook;

        internal BuildingsPlacementInitiatorsInterceptor(IRewirerProvider rewirerProvider)
        {
            RewirerProvider = rewirerProvider;

            ShapeBuildingsRegisterPlacersHook =
                CreateDetour<ShapeBuildingsPlacersCreator,
                    IShapeBuildingsPlacementInitiatorsRewirer>();

            FluidBuildingsRegisterPlacersHook =
                CreateDetour<FluidBuildingsPlacersCreator,
                    IFluidBuildingsPlacementInitiatorsRewirer>();

            SignalBuildingsRegisterPlacersHook =
                CreateDetour<SignalBuildingsPlacersCreator,
                    ISignalBuildingsPlacementInitiatorsRewirer>();

            DecorationsBuildingsRegisterPlacersHook =
                CreateDetour<DecorationBuildingsPlacersCreator,
                    IDecorationBuildingsPlacementInitiatorsRewirer>();

            SandboxBuildingsRegisterPlacersHook =
                CreateDetour<SandboxBuildingsPlacersCreator,
                    ISandboxBuildingsPlacementInitiatorsRewirer>();

            return;

            Hook CreateDetour<TCreator, TRewirer>()
                where TCreator : BuildingPlacersCreator
                where TRewirer : IBuildingsPlacementInitiatorsRewirer
            {
                return DetourHelper
                   .CreatePostfixHook<TCreator, IPlacementInitiatorIdRegistry,
                        ICollection<IDisposable>>((creator, registry, disposables) =>
                            creator.RegisterPlacers(registry, disposables),
                        RegisterPlacers<TCreator, TRewirer>);
            }
        }

        private void RegisterPlacers<TCreator, TRewirer>(TCreator creator,
            IPlacementInitiatorIdRegistry initiatorIdRegistry,
            ICollection<IDisposable> disposables)
            where TCreator : BuildingPlacersCreator
            where TRewirer : IBuildingsPlacementInitiatorsRewirer
        {
            IEnumerable<TRewirer> initiatorsRewirers = RewirerProvider.RewirersOfType<TRewirer>();
            BuildingInitiatorsParams @params = new(creator);
            foreach (TRewirer initiatorsRewirer in initiatorsRewirers)
            {
                initiatorsRewirer.ModifyBuildingPlacers(@params, initiatorIdRegistry);
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