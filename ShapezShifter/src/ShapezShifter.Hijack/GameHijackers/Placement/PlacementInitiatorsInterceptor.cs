using System;
using System.Collections.Generic;
using MonoMod.RuntimeDetour;
using ShapezShifter.SharpDetour;

namespace ShapezShifter.Hijack
{
    internal class PlacementInitiatorsInterceptor : IDisposable
    {
        private readonly IRewirerProvider RewirerProvider;
        private readonly Hook ShapeBuildingsRegisterPlacersHook;
        private readonly Hook FluidBuildingsRegisterPlacersHook;
        private readonly Hook SignalBuildingsRegisterPlacersHook;
        private readonly Hook DecorationsBuildingsRegisterPlacersHook;
        private readonly Hook SandboxBuildingsRegisterPlacersHook;
        private readonly Hook PlatformIslandsRegisterPlacersHook;
        private readonly Hook TrainIslandsRegisterPlacersHook;
        private readonly Hook ConverterIslandsRegisterPlacersHook;

        internal PlacementInitiatorsInterceptor(IRewirerProvider rewirerProvider)
        {
            RewirerProvider = rewirerProvider;

            ShapeBuildingsRegisterPlacersHook =
                CreateBuildingDetour<ShapeBuildingsPlacersCreator,
                    IShapeBuildingPlacementRewirers>();

            FluidBuildingsRegisterPlacersHook =
                CreateBuildingDetour<FluidBuildingsPlacersCreator,
                    IFluidBuildingPlacementRewirers>();

            SignalBuildingsRegisterPlacersHook =
                CreateBuildingDetour<SignalBuildingsPlacersCreator,
                    ISignalBuildingPlacementRewirers>();

            DecorationsBuildingsRegisterPlacersHook =
                CreateBuildingDetour<DecorationBuildingsPlacersCreator,
                    IDecorationBuildingPlacementRewirers>();

            SandboxBuildingsRegisterPlacersHook =
                CreateBuildingDetour<SandboxBuildingsPlacersCreator,
                    ISandboxBuildingPlacementRewirers>();

            SandboxBuildingsRegisterPlacersHook =
                CreateBuildingDetour<SandboxBuildingsPlacersCreator,
                    ISandboxBuildingPlacementRewirers>();

            PlatformIslandsRegisterPlacersHook =
                CreateIslandDetour<PlatformIslandsPlacersCreators,
                    IPlatformIslandPlacementRewirers>();

            TrainIslandsRegisterPlacersHook =
                CreateIslandDetour<TrainIslandsPlacersCreators,
                    ITrainIslandPlacementRewirers>();

            ConverterIslandsRegisterPlacersHook =
                CreateIslandDetour<ConverterIslandsPlacersCreators,
                    IConverterIslandPlacementRewirers>();

            return;

            Hook CreateBuildingDetour<TCreator, TRewirer>()
                where TCreator : BuildingPlacersCreator
                where TRewirer : IBuildingPlacementRewirers
            {
                return DetourHelper
                   .CreatePostfixHook<TCreator, IPlacementInitiatorIdRegistry,
                        ICollection<IDisposable>>((creator, registry, disposables) =>
                            creator.RegisterPlacers(registry, disposables),
                        RegisterBuildingPlacers<TCreator, TRewirer>);
            }

            Hook CreateIslandDetour<TCreator, TRewirer>()
                where TCreator : IslandPlacersCreator
                where TRewirer : IIslandPlacementRewirers
            {
                return DetourHelper
                   .CreatePostfixHook<TCreator, IPlacementInitiatorIdRegistry,
                        ICollection<IDisposable>>((creator, registry, disposables) =>
                            creator.RegisterPlacers(registry, disposables),
                        RegisterIslandPlacers<TCreator, TRewirer>);
            }
        }

        private void RegisterBuildingPlacers<TCreator, TRewirer>(TCreator creator,
            IPlacementInitiatorIdRegistry initiatorIdRegistry,
            ICollection<IDisposable> disposables)
            where TCreator : BuildingPlacersCreator
            where TRewirer : IBuildingPlacementRewirers
        {
            IEnumerable<TRewirer> initiatorsRewirers = RewirerProvider.RewirersOfType<TRewirer>();
            BuildingInitiatorsParams @params = new(creator);
            foreach (TRewirer initiatorsRewirer in initiatorsRewirers)
            {
                initiatorsRewirer.ModifyBuildingPlacers(@params, initiatorIdRegistry);
            }
        }

        private void RegisterIslandPlacers<TCreator, TRewirer>(TCreator creator,
            IPlacementInitiatorIdRegistry initiatorIdRegistry,
            ICollection<IDisposable> disposables)
            where TCreator : IslandPlacersCreator
            where TRewirer : IIslandPlacementRewirers
        {
            IEnumerable<TRewirer> initiatorsRewirers = RewirerProvider.RewirersOfType<TRewirer>();
            IslandInitiatorsParams @params = new(creator, 3);
            foreach (TRewirer initiatorsRewirer in initiatorsRewirers)
            {
                initiatorsRewirer.ModifyIslandPlacers(@params, initiatorIdRegistry);
            }
        }

        public void Dispose()
        {
            ShapeBuildingsRegisterPlacersHook.Dispose();
            FluidBuildingsRegisterPlacersHook.Dispose();
            SignalBuildingsRegisterPlacersHook.Dispose();
            DecorationsBuildingsRegisterPlacersHook.Dispose();
            SandboxBuildingsRegisterPlacersHook.Dispose();
            PlatformIslandsRegisterPlacersHook.Dispose();
            TrainIslandsRegisterPlacersHook.Dispose();
            ConverterIslandsRegisterPlacersHook.Dispose();
        }
    }
}