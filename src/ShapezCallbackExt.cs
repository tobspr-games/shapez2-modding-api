using System;
using System.Diagnostics;
using HarmonyLib;
using JetBrains.Annotations;

public static class ShapezCallbackExt
{
    public static Action OnPostGameStart;
    public static Action OnPreGameStart;

    [HarmonyPatch(typeof(GameCore), "Start")]
    [HarmonyPrefix]
    static void BeforeGameStart()
    {
        OnPreGameStart?.Invoke();
    }

    [HarmonyPatch(typeof(GameCore), "Start")]
    [HarmonyPostfix]
    private static void AfterGameStart()
    {
        OnPostGameStart?.Invoke();
    }
}
