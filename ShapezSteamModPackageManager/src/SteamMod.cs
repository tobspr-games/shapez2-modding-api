internal readonly struct SteamMod
{
    public readonly SteamModMetadata Metadata;
    public readonly ISteamMod EntryPoint;

    public SteamMod(SteamModMetadata metadata, ISteamMod entryPoint)
    {
        Metadata = metadata;
        EntryPoint = entryPoint;
    }
}