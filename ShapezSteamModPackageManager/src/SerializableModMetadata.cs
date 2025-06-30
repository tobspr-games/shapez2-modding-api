using System;
using System.Collections.Generic;
using System.Linq;
using Semver;

[Serializable]
public class SerializableModMetadata
{
    public string Name;
    public SemVersion Version;

    public string Title;
    public string Description;
    public string IconPath;

    public string Author;

    public string[] Assemblies;

    public SerializableModDependency[] Dependencies;
}

public class SteamModMetadata
{
    public readonly string Name;
    public readonly SemVersion Version;

    public readonly string Title;
    public readonly string Description;
    public readonly string IconPath;

    public readonly string Author;

    public readonly string[] Assemblies;

    public readonly SerializableModDependency[] Dependencies;

    public SteamModMetadata(string name, SemVersion version, string title, string description, string iconPath,
        string author, IEnumerable<string> assemblies, IEnumerable<SerializableModDependency> dependencies)
    {
        Name = name;
        Version = version;
        Title = title;
        Description = description;
        IconPath = iconPath;
        Author = author;
        Assemblies = assemblies.ToArray();
        Dependencies = dependencies.ToArray();
    }
}

public class ModDependency
{
    public readonly string FullModName;
    public readonly SemVersion Version;
    public readonly SemVersionRange SupportedVersionRange;

    public ModDependency(string fullModName, SemVersion version, SemVersionRange supportedVersionRange)
    {
        FullModName = fullModName;
        Version = version;
        SupportedVersionRange = supportedVersionRange;
    }
}

[Serializable]
public class SerializableModDependency
{
    public string FullModName;
    public SemVersion Version;
    public SemVersionRange SupportedVersionRange;
}