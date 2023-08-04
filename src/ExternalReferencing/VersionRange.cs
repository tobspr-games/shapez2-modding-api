public readonly struct VersionRange
{
    public readonly Version Min;
    public readonly Version Max;

    public VersionRange(Version min, Version max)
    {
        Min = min;
        Max = max;
    }


    public bool InsideRange(Version version)
    {
        return version.VersionId >= Min.VersionId && version.VersionId <= Max.VersionId;
    }
}
