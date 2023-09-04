using System;

public static class ShapezCallbackExt
{
    public static Action OnPreGameStart;
    public static Action OnPostGameStart;

    public static void BeforeGameStart()
    {
        UnityEngine.Debug.Log("Before Game Start");
        OnPreGameStart?.Invoke();
    }

    public static void AfterGameStart()
    {
        UnityEngine.Debug.Log("After Game Start");
        OnPostGameStart?.Invoke();
    }

}
