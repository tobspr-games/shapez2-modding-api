using Core.Logging;
using JetBrains.Annotations;
using ShapezShifter;

[UsedImplicitly]
public class Main : IMod
{
    private readonly ILogger Logger;
    private readonly GameCoreCallbackExtender CallbackExtender;

    public Main(ILogger logger)
    {
        logger.Info?.Log("Shapez Shifter Initialized");
        Logger = logger;
        CallbackExtender = new GameCoreCallbackExtender();
        ShapezCallbackExt.OnPreGameStart = CallbackExtender.OnPreGameStart;
        ShapezCallbackExt.OnPostGameStart = CallbackExtender.OnPostGameStart;
        TestPatchingIsWorking(logger);
    }

    public void Dispose()
    {
        Logger.Info?.Log("Shapez Shifter shut down requested");
        CallbackExtender.Dispose();
        Logger.Info?.Log("Shapez Shifter shut down completed");
    }

    private void TestPatchingIsWorking(ILogger logger)
    {
        var testClass = new TestClass();
        testClass.Bump();
        testClass.Bump();
        testClass.Bump();
        if (testClass.Count != 3)
        {
            logger.Error?.Log("Hook is applied way too early");
        }

        var hook = HookHelper.ReplaceHook<TestClass>(g => g.Bump(), NoBump);

        testClass.Bump();
        testClass.Bump();
        testClass.Bump();

        if (testClass.Count != 3)
        {
            logger.Error?.Log($"Hook is not being applied. Count: {testClass.Count}");
        }

        hook.Dispose();

        testClass.Bump();
        testClass.Bump();

        if (testClass.Count != 5)
        {
            logger.Error?.Log($"Hook cannot be unhooked. Count: {testClass.Count}");
        }

        return;

        void NoBump(TestClass t)
        {
        }
    }


    private class TestClass
    {
        public int Count;

        public void Bump()
        {
            Count++;
        }
    }
}