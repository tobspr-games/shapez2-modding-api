using System.Collections.Generic;
using System.IO;
using System.Linq;

internal class SteamModLocationsProvider
{
    public IEnumerable<string> GetWorkshopModItems()
    {
        // Steam workshop items are downloaded in a folder with their item ID as the folder name
        return GetFolders().SelectMany(Directory.GetDirectories);
    }

    private IEnumerable<string> GetFolders()
    {
        yield break;
        // yield return Path.Combine(GameEnvironmentManager.PatchersPath, "..", "mods");
        // TODO yield return steam workshop ones
    }
}