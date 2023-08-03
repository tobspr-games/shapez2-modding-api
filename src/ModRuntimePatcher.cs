using HarmonyLib;
using System.Collections.Generic;

namespace ShapezShifter
{
    public class ModRuntimePatcher : IModManager
    {
        private const string COMPANY = "tobspr";
        private const string GAME = "shapez2";
        private const string PRODUCT = "shapez_shifted";

        public IEnumerable<IMod> Load(string modsPath)
        {
            var mods = ModLoader.LoadMods(modsPath);


            Harmony harmony = new Harmony($"{COMPANY}.{GAME}.{PRODUCT}");
            harmony.PatchAll(typeof(ShapezCallbackExt));

            return mods;
        }
    }
}
