using System;
using System.Collections.Generic;
using Core.Logging;
using MonoMod.RuntimeDetour;
using ShapezShifter.SharpDetour;

namespace ShapezShifter.Hijack
{
    internal class GameScenarioInterceptor : IDisposable
    {
        private readonly IRewirerProvider RewirerProvider;
        private readonly ILogger Logger;
        private readonly Hook ScenarioDeserializationHook;

        public GameScenarioInterceptor(IRewirerProvider rewirerProvider, ILogger logger)
        {
            RewirerProvider = rewirerProvider;
            Logger = logger;
            ScenarioDeserializationHook =
                DetourHelper.CreatePostfixHook<GameData, string, GameScenario>(
                    (gameData, uniqueId) => gameData.GetScenarioCloned(uniqueId),
                    Postfix);
        }

        private GameScenario Postfix(GameData data, string uniqueId,
            GameScenario gameScenario)
        {
            Logger.Info?.Log("Modifying research");
            IEnumerable<IGameScenarioRewirer> scenarioRewirers = RewirerProvider.RewirersOfType<IGameScenarioRewirer>();
            foreach (IGameScenarioRewirer scenarioRewirer in scenarioRewirers)
            {
                gameScenario = scenarioRewirer.ModifyGameScenario(gameScenario);
            }


            return gameScenario;
        }

        public void Dispose()
        {
            ScenarioDeserializationHook.Dispose();
        }
    }
}