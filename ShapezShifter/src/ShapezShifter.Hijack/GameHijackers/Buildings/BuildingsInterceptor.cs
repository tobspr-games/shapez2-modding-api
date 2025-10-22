using System;
using System.Collections.Generic;
using Core.Logging;
using Game.Core.Rendering.MeshGeneration;
using Global.Core;
using MonoMod.RuntimeDetour;
using ShapezShifter.SharpDetour;

namespace ShapezShifter.Hijack
{
    internal class BuildingsInterceptor : IDisposable
    {
        private readonly IRewirerProvider RewirerProvider;
        private readonly ILogger Logger;
        private readonly Hook BuildingsFactoryFromMetadataHook;

        public BuildingsInterceptor(IRewirerProvider rewirerProvider, ILogger logger)
        {
            RewirerProvider = rewirerProvider;
            Logger = logger;
            BuildingsFactoryFromMetadataHook = DetourHelper
               .CreateStaticPostfixHook<GameModeBuildingsFactory, MetaGameModeBuildings, IMeshCache
                    ,
                    VisualThemeBaseResources
                    , GameBuildings>((factory, meta, meshCache, resources) =>
                        GameModeBuildingsFactory.FromMetadata(meta, meshCache, resources),
                    Postfix);
        }

        private GameBuildings Postfix(MetaGameModeBuildings metaBuildings,
            IMeshCache meshCache, VisualThemeBaseResources theme, GameBuildings gameBuildings)
        {
            IEnumerable<IBuildingsRewirer> buildingsRewirers =
                RewirerProvider.RewirersOfType<IBuildingsRewirer>();

            Logger.Info?.Log("Intercepting buildings creation");

            int buildingsCount = gameBuildings.All.Count;

            foreach (IBuildingsRewirer buildingsRewirer in buildingsRewirers)
            {
                gameBuildings = buildingsRewirer.ModifyGameBuildings(metaBuildings, gameBuildings, meshCache, theme);
            }

            Logger.Info?.Log($"New buildings: {gameBuildings._All.Count} + {buildingsCount}");

            return gameBuildings;
        }

        public void Dispose()
        {
            BuildingsFactoryFromMetadataHook.Dispose();
        }
    }
}