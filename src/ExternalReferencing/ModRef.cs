using System;

public struct ModRef
{
    private readonly string _name;
    private readonly Guid _uniqueGuid;
    private readonly int _steamId;
    private readonly VersionRange _versionRange;


    public ModRef(int steamId, VersionRange versionRange)
    {
        _steamId = steamId;
        _versionRange = versionRange;
        _name = null;
        _uniqueGuid = default;
    }

    public ModRef(Guid uniqueGuid, VersionRange versionRange)
    {
        _uniqueGuid = uniqueGuid;
        _name = null;
        _steamId = 0;
        _versionRange = versionRange;
    }

    public ModRef(string name, VersionRange versionRange)
    {
        _name = name;
        _uniqueGuid = default;
        _steamId = 0;
        _versionRange = versionRange;
    }
}
