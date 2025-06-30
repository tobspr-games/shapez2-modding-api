using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.Collections.Scoped;
using Core.Dependency;
using Core.Logging;
using Global.Patching;
using ShapezSteamModPackageManager;

internal class SteamModLoader : IDisposable
{
    private readonly ILogger Logger;
    private readonly ScopedList<SteamMod> LoadedMods = ScopedList<SteamMod>.Get();

    public SteamModLoader(IEnumerable<SteamModMetadata> packagesInOrder, ILogger logger)
    {
        Logger = logger;
        foreach (var metadata in packagesInOrder)
        {
            logger.Info?.Log($"Loading package {metadata.Name}");
            using var assemblies = ScopedList<Assembly>.Get();
            foreach (var assemblyPath in metadata.Assemblies)
            {
                logger.Info?.Log($"\tLoading assembly {assemblyPath}");
                var assembly = Assembly.LoadFrom(assemblyPath);
                assemblies.Add(assembly);
            }


            var types = GetLoadableTypes(assemblies, logger);
            var bootstrap = types.SingleOrDefault(type => typeof(ISteamMod).IsAssignableFrom(type));

            ISteamMod instance = null;
            if (bootstrap != null)
            {
                using var dependencyContainer = new DependencyContainer();
                var signature = ConvertToSignature(metadata);
                var modLogger = new PrefixedLogger(logger, signature.ToString());
                dependencyContainer.Bind<ILogger>().To(modLogger);
                instance = (ISteamMod)dependencyContainer.Create(bootstrap);
            }

            LoadedMods.Add(new SteamMod(metadata, instance));
        }
    }

    private static IEnumerable<Type> GetLoadableTypes(IEnumerable<Assembly> assembly, ILogger logger)
    {
        try
        {
            return assembly.SelectMany(x => x.GetTypes());
        }
        catch (ReflectionTypeLoadException e)
        {
            logger.Warning?.Log(e.Message);
            return e.Types.Where(t => t != null);
        }
    }

    public void Dispose()
    {
        foreach (var loadedMod in LoadedMods)
        {
            try
            {
                loadedMod.EntryPoint?.Dispose();
            }
            catch (Exception e)
            {
                var signature = ConvertToSignature(loadedMod.Metadata);
                var modLogger = new PrefixedLogger(Logger, signature.ToString());
                modLogger.Exception?.LogException(e);
            }
        }

        LoadedMods.Dispose();
    }

    private static ModSignature ConvertToSignature(SteamModMetadata modMetadata)
    {
        var v = modMetadata.Version;
        return new ModSignature(modMetadata.Name, (int)v.Major, (int)v.Minor, (int)v.Patch);
    }
}