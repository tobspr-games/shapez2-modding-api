using System;
using System.Collections.Generic;
using Core.Collections.Scoped;
using ILogger = Core.Logging.ILogger;

internal class SteamModLoadingOrchestrator : IDisposable
{
    private readonly SteamModLocationsProvider LocationsProvider = new();
    private readonly SteamModMetadataParser MetadataParser = new();
    private readonly Dictionary<string, SteamModMetadata> InstalledPackages = new();
    private readonly SteamModLoader ModLoader;

    public SteamModLoadingOrchestrator(ILogger logger)
    {
        foreach (var workshopModLocation in LocationsProvider.GetWorkshopModItems())
        {
            if (!MetadataParser.TryParse(workshopModLocation, out var metadata))
            {
                logger.Error?.Log($"Could not load metadata for workshop item at {workshopModLocation}");
            }

            logger.Info?.Log($"Found a package {metadata.Title}");
            InstalledPackages.Add(metadata.Name, metadata);
        }

        using var dependencyRelations = ScopedList<DependencyRelation<string, string>>.Get();
        foreach (var installedPackage in InstalledPackages.Values)
        {
            foreach (var dependency in installedPackage.Dependencies)
            {
                dependencyRelations.Add(new DependencyRelation<string, string>(installedPackage.Name,
                    dependency.FullModName));
            }
        }

        using var dependencyGraphResolver = new GraphResolver<string>(dependencyRelations);

        var namesInOrder = dependencyGraphResolver.ResolveTopologicalOrder();

        using var packagesInOrder = ScopedList<SteamModMetadata>.Get();
        using var missingPackages = ScopedList<string>.Get();

        if (!TrySolvingNamesToPackages(namesInOrder, InstalledPackages, packagesInOrder, missingPackages))
        {
            foreach (var missingPackage in missingPackages)
            {
                logger.Error?.Log(
                    $"{missingPackage} is missing. It was not downloaded or it is incorrectly configured");
            }

            logger.Error?.Log("Could not solve all names to packages");
            return;
        }

        logger.Info?.Log("Dependencies resolved");

        ModLoader = new SteamModLoader(packagesInOrder, logger);
    }

    private bool TrySolvingNamesToPackages<TName, TPackage>(IEnumerable<TName> namesInOrder,
        Dictionary<TName, TPackage> installedPackages,
        ICollection<TPackage> packagesInOrder,
        ICollection<TName> missingPackages)
    {
        foreach (var name in namesInOrder)
        {
            if (installedPackages.TryGetValue(name, out var package))
            {
                packagesInOrder.Add(package);
            }
            else
            {
                missingPackages.Add(name);
            }
        }

        return missingPackages.Count == 0;
    }

    public void Dispose()
    {
        ModLoader?.Dispose();
    }
}