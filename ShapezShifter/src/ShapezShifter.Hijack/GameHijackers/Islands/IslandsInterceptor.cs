using System;
using System.Collections.Generic;
using Core.Logging;
using MonoMod.RuntimeDetour;
using ShapezShifter.SharpDetour;

namespace ShapezShifter.Hijack
{
    internal class IslandsInterceptor : IDisposable
    {
        private readonly IRewirerProvider RewirerProvider;
        private readonly ILogger Logger;
        private readonly Hook IslandsFactoryFromMetadataHook;

        public IslandsInterceptor(IRewirerProvider rewirerProvider, ILogger logger)
        {
            RewirerProvider = rewirerProvider;
            Logger = logger;
            IslandsFactoryFromMetadataHook = DetourHelper
               .CreatePostfixHook<IslandDefinitionFactory, MetaGameModeIslands, GameIslands>((factory, meta) =>
                        factory.BakeMetadataIntoRuntime(meta),
                    Postfix);
        }

        private GameIslands Postfix(IslandDefinitionFactory islandDefinitionFactory, MetaGameModeIslands metaIslands,
            GameIslands gameIslands)
        {
            IEnumerable<IIslandsRewirer> islandsRewirers = RewirerProvider.RewirersOfType<IIslandsRewirer>();

            Logger.Info?.Log("Intercepting islands creation");

            int islandsCount = gameIslands.AllDefinitions.Count;

            foreach (IIslandsRewirer islandsRewirer in islandsRewirers)
            {
                gameIslands = islandsRewirer.ModifyGameIslands(islandDefinitionFactory, metaIslands, gameIslands);
            }

            Logger.Info?.Log($"New islands: {gameIslands.AllDefinitions.Count} + {islandsCount}");

            return gameIslands;
        }

        public void Dispose()
        {
            IslandsFactoryFromMetadataHook.Dispose();
        }
    }
}