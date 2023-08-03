using HarmonyLib;
using System;
using System.Linq;

namespace ShapezShifter
{
    public class ModRuntimePatcher : IRuntimePatcher
    {
        private const string COMPANY = "tobspr";
        private const string GAME = "shapez2";
        private const string PRODUCT = "shapez_shifted";

        public void Load()
        {
            ModLoader.LoadMods();

            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(asm => asm.GetLoadableTypes())
                .Where(t => t.GetMethods().Any(m => m.GetCustomAttributes(typeof(HarmonyPatch), true).Length > 0)).Distinct();

            var typesDebug = AppDomain.CurrentDomain.GetAssemblies()
                                      .SelectMany(asm => asm.GetLoadableTypes());

            Harmony harmony = new Harmony($"{COMPANY}.{GAME}.{PRODUCT}");
            harmony.PatchAll(typeof(ShapezCallbackExt));
        }
    }
}
