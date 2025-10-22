using System;
using System.Collections.Generic;
using System.Linq;
using Game.Orchestration;
using MonoMod.RuntimeDetour;
using ShapezShifter.SharpDetour;

namespace ShapezShifter.Hijack
{
    internal class BuffablesInterceptor : IDisposable
    {
        private readonly IRewirerProvider RewirerProvider;
        private readonly Hook Hook;

        internal BuffablesInterceptor(IRewirerProvider rewirerProvider)
        {
            RewirerProvider = rewirerProvider;

            Hook = DetourHelper.CreateStaticPostfixHook<GameMode, IEnumerable<object>>(
                typeof(GameModeBuffQueries),
                mode => GameModeBuffQueries.AllBuffables(mode),
                Postfix);
        }

        private IEnumerable<object> Postfix(GameMode mode, IEnumerable<object> buffables)
        {
            IEnumerable<IBuffablesRewirer> buffablesRewirers = RewirerProvider.RewirersOfType<IBuffablesRewirer>();

            ICollection<object> buffablesList = buffables.ToList();
            foreach (IBuffablesRewirer rewirer in buffablesRewirers)
            {
                buffablesList = rewirer.ModifyBuffables(buffablesList);
            }

            return buffablesList;
        }

        public void Dispose()
        {
            Hook.Dispose();
        }
    }
}