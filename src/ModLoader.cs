using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ShapezShifter
{

    /// <summary>
    /// Responsible for loading mods and providing a more extensible API
    /// </summary>
    public static class ModLoader
    {
        private const string MODS_PATH = "Mods";

        public static void LoadMods()
        {
            var patchesPath = Path.GetFullPath(".");
            var patchesFolder = Path.Combine(patchesPath, MODS_PATH);
            var assemblies = Directory.GetFiles(patchesFolder, "*.dll");

            // Todo: load in-game after download
            foreach (var assemblyPath in assemblies)
            {
                var assembly = Assembly.LoadFrom(assemblyPath);
                LoadMod(assembly);
            }
        }


        private static void LoadMod(Assembly assembly)
        {
            var patchInterfaceType = typeof(IMod);
            var modType = assembly.GetLoadableTypes()
                .SingleOrDefault(type => patchInterfaceType.IsAssignableFrom(type));

            if (modType == null)
            {
                return;
            }

            var mod = (IMod)Activator.CreateInstance(modType);

            mod.Init();
        }

    }
}