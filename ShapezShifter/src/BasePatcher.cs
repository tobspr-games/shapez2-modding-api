using System;
using MonoMod.RuntimeDetour;

public class BasePatcher : IPatcher
{
    // Todo: Dispose
    private static Hook BeforeGameStartHook;
    private static Hook AfterGameStartHook;

    public void Patch()
    {
        TestPatchingIsWorking();
        BeforeGameStartHook = HookHelper.CreatePrefixHook<GameCore>(g => g.Init(), ShapezCallbackExt.BeforeGameStart);
        AfterGameStartHook = HookHelper.CreatePostfixHook<GameCore>(g => g.Init(), ShapezCallbackExt.AfterGameStart);
    }


    private void TestPatchingIsWorking()
    {
        var testClass = new TestClass();
        testClass.Bump();
        testClass.Bump();
        testClass.Bump();
        if (testClass.Count != 3)
        {
            throw new Exception("Hook is applied way too early");
        }

        var hook = HookHelper.ReplaceHook<TestClass>(g => g.Bump(), NoBump);

        testClass.Bump();
        testClass.Bump();
        testClass.Bump();

        if (testClass.Count != 3)
        {
            throw new Exception($"Hook is not being applied. Count: {testClass.Count}");
        }

        hook.Dispose();

        testClass.Bump();
        testClass.Bump();

        if (testClass.Count != 5)
        {
            throw new Exception($"Hook cannot be unhooked. Count: {testClass.Count}");
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