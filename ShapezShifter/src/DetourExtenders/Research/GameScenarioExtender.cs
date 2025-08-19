using System;
using Core.Logging;
using MonoMod.RuntimeDetour;

namespace ShapezShifter
{
    internal class GameScenarioExtender : IDisposable
    {
        private readonly IExtendersProvider ExtendersProvider;
        private readonly ILogger Logger;
        private readonly Hook ScenarioDeserializationHook;

        public GameScenarioExtender(IExtendersProvider extendersProvider, ILogger logger)
        {
            ExtendersProvider = extendersProvider;
            Logger = logger;
            ScenarioDeserializationHook =
                DetourHelper.CreatePostfixHook<GameData, string, GameScenario>(
                    (gameData, uniqueId) => gameData.GetScenarioCloned(uniqueId),
                    Postfix);
        }

        private GameScenario Postfix(GameData data, string uniqueId,
            GameScenario gameScenario)
        {
            Logger.Info?.Log("Extending research");
            var researchExtenders = ExtendersProvider.ExtendersOfType<IGameScenarioExtender>();
            foreach (IGameScenarioExtender researchExtender in researchExtenders)
            {
                gameScenario = researchExtender.ExtendGameScenario(gameScenario);
            }


            return gameScenario;
        }

        public void Dispose()
        {
            ScenarioDeserializationHook.Dispose();
        }
    }
}