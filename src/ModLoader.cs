using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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
            var assemblies = Directory.GetFiles(path, SearchPatternForDynamicLibrary());
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

        private static string SearchPatternForDynamicLibrary()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return "*.dll";
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return "*.so";
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return "*.dylib";
            }

            Debug.LogWarning("Trying to load a dynamic library from an unsupported platform!?");
            return "*";
        }

    }
}