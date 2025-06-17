using System;

public static class ShapezCallbackExt
{
    public static Action OnPreGameStart;
    public static Action OnPostGameStart;

    public static void BeforeGameStart()
    {
        OnPreGameStart?.Invoke();
    }

    public static void AfterGameStart()
    {
        OnPostGameStart?.Invoke();
    }
}