public class BasePatcher : IPatcher
{
    public void Patch()
    {
        HookHelper.CreatePrefixHook<GameCore>((g) => g.Start(), ShapezCallbackExt.BeforeGameStart);
        HookHelper.CreatePostfixHook<GameCore>((g) => g.Start(), ShapezCallbackExt.AfterGameStart);
    }
}