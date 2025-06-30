using Core.Logging;
using Global.Patching;
using JetBrains.Annotations;

public class EntryPoint : IMod
{
    private readonly ILogger Logger;

    private readonly SteamModLoadingOrchestrator ModLoadingOrchestrator;

    [UsedImplicitly]
    public EntryPoint(ILogger logger)
    {
        Logger = logger;
        Logger.Info?.Log("Greetings from the Shapez Steam Mod Package Manager");
        ModLoadingOrchestrator = new SteamModLoadingOrchestrator(logger);
    }

    public void Dispose()
    {
        ModLoadingOrchestrator.Dispose();
        Logger.Info?.Log("Farewell from the Shapez Steam Mod Package Manager");
    }

    public ModComposedSignature ComposedSignature => new(new[]
    {
        new ModSignature("Steamed Shapez", 1, 0, 0)
    });
}