using System;
using Core.Logging;
using Game.Core.Rendering.MeshGeneration;
using Global.Core;
using MonoMod.RuntimeDetour;

namespace ShapezShifter
{
    internal class BuildingsExtender : IDisposable
    {
        private readonly IExtendersProvider ExtendersProvider;
        private readonly ILogger Logger;
        private readonly Hook BuildingsFactoryFromMetadataHook;

        public BuildingsExtender(IExtendersProvider extendersProvider, ILogger logger)
        {
            ExtendersProvider = extendersProvider;
            Logger = logger;
            BuildingsFactoryFromMetadataHook = DetourHelper
                .CreateStaticPostfixHook<GameModeBuildingsFactory, MetaGameModeBuildings, IMeshCache
                    ,
                    VisualThemeBaseResources
                    , GameBuildings>((factory, meta, meshCache, resources) =>
                    GameModeBuildingsFactory.FromMetadata(meta, meshCache, resources), Postfix);
        }

        private GameBuildings Postfix(MetaGameModeBuildings metaBuildings,
            IMeshCache meshCache, VisualThemeBaseResources theme, GameBuildings gameBuildings)
        {
            var buildingsExtenders = ExtendersProvider.ExtendersOfType<IBuildingsExtender>();

            foreach (IBuildingsExtender buildingsExtender in buildingsExtenders)
            {
                gameBuildings =
                    buildingsExtender.ModifyGameBuildings(metaBuildings, gameBuildings, meshCache,
                        theme);
            }

            return gameBuildings;
        }

        public void Dispose()
        {
            BuildingsFactoryFromMetadataHook.Dispose();
        }
    }
}