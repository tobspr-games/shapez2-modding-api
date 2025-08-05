using System;
using System.Runtime.CompilerServices;
using Core.Logging;
using JetBrains.Annotations;

[assembly: InternalsVisibleTo("ShapezShifterTests")]

namespace ShapezShifter
{
    [UsedImplicitly]
    public class Main : IMod
    {
        private readonly ILogger Logger;
        private readonly GameCoreCallbackExtender CallbackExtender;

        private readonly GameExtender GameExtender;
        private bool Disposed;

        public Main(ILogger logger)
        {
            logger.Info?.Log("Shapez Shifter Initialized");
            Logger = logger;

            SetupPathEnvironmentVariable();

            // CallbackExtender = new GameCoreCallbackExtender();
            // ShapezCallbackExt.OnPreGameStart = CallbackExtender.OnPreGameStart;
            // ShapezCallbackExt.OnPostGameStart = CallbackExtender.OnPostGameStart;

            var staticallyAccessibleExtendersProvider = new CachedStaticallyAccessibleExtendersProvider(logger);
            GameExtender = new GameExtender(staticallyAccessibleExtendersProvider, logger);
        }

        private static void SetupPathEnvironmentVariable()
        {
            Environment.SetEnvironmentVariable("SPZ2_SHIFTER", typeof(Main).Assembly.Location);
        }

        public void Dispose()
        {
            if (Disposed)
            {
                Logger.Warning?.Log("Mod was already disposed");
                return;
            }

            Disposed = true;
            GameExtender.Dispose();
            Logger.Info?.Log("Shapez Shifter shut down requested");
            CallbackExtender.Dispose();
            Logger.Info?.Log("Shapez Shifter shut down completed");
        }
    }
}