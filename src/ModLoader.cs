using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ShapezShifter
{
    /// <summary>
    /// Responsible for loading mods and providing a more extensible API
    /// </summary>
    public static class ModLoader
    {
        public static IEnumerable<IMod> LoadMods(string path)
        {
            if (!Directory.Exists(path))
            {
                Debug.LogWarning("No mods to load");
                yield break;
            }
            var assemblies = Directory.GetFiles(path, "*.dll");

            // Todo: load in-game after download
            foreach (var assemblyPath in assemblies)
            {
                var assembly = Assembly.LoadFrom(assemblyPath);
                var patchInterfaceType = typeof(IMod);
                var modType = assembly.GetLoadableTypes()
                    .SingleOrDefault(type => patchInterfaceType.IsAssignableFrom(type));
                var mod = (IMod)Activator.CreateInstance(modType);
                mod.Init();
                yield return mod;
            }
        }

    }
}