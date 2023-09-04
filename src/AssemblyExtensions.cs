using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

internal static class AssemblyExtensions
{
    internal static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException e)
        {
            return e.Types.Where(t => t != null);
        }
    }
}
