using System;
using HarmonyLib;
using JetBrains.Annotations;

public static class ShapezCallbackExt
{
    public static Action OnGameStart;

    [HarmonyPatch(typeof(GameCore), "Start")]
    [HarmonyPostfix]
    private static void CreateOnGameStartCallback()
    {
        OnGameStart?.Invoke();
    }
}
