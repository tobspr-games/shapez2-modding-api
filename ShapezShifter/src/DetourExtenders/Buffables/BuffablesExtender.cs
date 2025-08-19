using System;
using System.Collections.Generic;
using System.Linq;
using Game.Orchestration;
using MonoMod.RuntimeDetour;

namespace ShapezShifter
{
    internal class BuffablesExtender : IDisposable
    {
        private readonly IExtendersProvider ExtendersProvider;
        private readonly Hook Hook;

        internal BuffablesExtender(IExtendersProvider extendersProvider)
        {
            ExtendersProvider = extendersProvider;

            Hook = DetourHelper.CreateStaticPostfixHook<GameMode, IEnumerable<object>>(
                typeof(GameModeBuffQueries),
                mode => GameModeBuffQueries.AllBuffables(mode),
                Postfix);
        }

        private IEnumerable<object> Postfix(GameMode mode, IEnumerable<object> buffables)
        {
            var buffablesExtenders = ExtendersProvider.ExtendersOfType<IBuffablesExtender>();

            ICollection<object> buffablesList = buffables.ToList();
            foreach (IBuffablesExtender extender in buffablesExtenders)
            {
                buffablesList = extender.ExtendBuffables(buffablesList);
            }

            return buffablesList;
        }

        public void Dispose()
        {
            Hook.Dispose();
        }
    }
}