using System;

public readonly struct Version
{
    public readonly int Major;
    public readonly int Minor;
    public readonly int Patch;

    internal readonly int VersionId;

    public Version(int major, int minor, int patch)
    {
        if (major > 99 || minor > 99 || patch > 99)
        {
            throw new Exception("Version major/minor/patch cannot exceed 99");
        }
        Major = major;
        Minor = minor;
        Patch = patch;

        VersionId = Major * 10000 + Minor * 100 + patch;
    }

    public override string ToString()
    {
        return $"{Major}.{Minor}.{Patch}";
    }
}
