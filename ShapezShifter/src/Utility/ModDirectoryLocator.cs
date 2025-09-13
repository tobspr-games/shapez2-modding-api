using System;
using System.IO;

namespace ShapezShifter
{
    public static class ModDirectoryLocator
    {
        public static string GetDirectoryLocation<TMod>()
            where TMod : IMod
        {
            return GetDirectoryLocation(typeof(TMod));
        }

        public static ModFolderLocator CreateLocator<TMod>()
            where TMod : IMod
        {
            return new ModFolderLocator(GetDirectoryLocation(typeof(TMod)));
        }

        public static string GetDirectoryLocation(IMod mod)
        {
            return mod.GetType().Assembly.Location;
        }

        private static string GetDirectoryLocation(Type t)
        {
            DirectoryInfo parent = Directory.GetParent(t.Assembly.Location);
            return parent == null ? throw new Exception("Invalid assembly location") : parent.Name;
        }
    }

    public class ModFolderLocator
    {
        private readonly string AssemblyLocation;

        public ModFolderLocator(string assemblyLocation)
        {
            AssemblyLocation = assemblyLocation;
        }

        public string SubPath(string subPath)
        {
            return Path.Combine(AssemblyLocation, subPath);
        }

        public ModFolderLocator SubLocator(string subPath)
        {
            return new ModFolderLocator(SubPath(subPath));
        }
    }
}