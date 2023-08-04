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

            // Todo: 1. find methods that have Prefix, Postfix and Replace attributes in all assemblies
            // Todo: 2. Solve deps & conflicts between them
            // Todo: 3. Patch in compatible order
            Harmony harmony = new Harmony($"{COMPANY}.{GAME}.{PRODUCT}");
            harmony.PatchAll(typeof(ShapezCallbackExt));

            return mods;
        }
    }
}
