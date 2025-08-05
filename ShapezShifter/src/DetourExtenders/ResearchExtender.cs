using System;
using Core.Logging;
using MonoMod.RuntimeDetour;

namespace ShapezShifter
{
    internal class ResearchExtender : IDisposable
    {
        private readonly IExtendersProvider ExtendersProvider;
        private readonly ILogger Logger;
        private readonly Hook ScenarioDeserializationHook;

        public ResearchExtender(IExtendersProvider extendersProvider, ILogger logger)
        {
            ExtendersProvider = extendersProvider;
            Logger = logger;
            ScenarioDeserializationHook =
                DetourHelper.CreatePostfixHook<GameCore, ResearchManager.SerializedData, bool>(
                    (ruleManager, data, isNewGame) => ruleManager.Init_3_2_EssentialManagers(data, isNewGame),
                    Postfix);
        }

        private void Postfix(GameCore gameCore, ResearchManager.SerializedData serializedData, bool isNewGame)
        {
            Logger.Info?.Log("Extending research");
            var researchExtenders = ExtendersProvider.ExtendersOfType<IResearchExtender>();
            foreach (var researchExtender in researchExtenders)
            {
                Logger.Info?.Log("Applying extender");

                researchExtender.ModifyResearch(gameCore.Research);
            }
        }


        public void Dispose()
        {
            ScenarioDeserializationHook.Dispose();
        }
    }

    public interface IResearchExtender : IExtender
    {
        void ModifyResearch(ResearchManager gameCoreResearch);
    }
}