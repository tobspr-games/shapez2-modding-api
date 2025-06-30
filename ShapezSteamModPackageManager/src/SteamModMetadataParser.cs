using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ShapezSteamModPackageManager;

internal class SteamModMetadataParser
{
    private readonly JsonSerializerSettings JsonSerializerSettings;

    public SteamModMetadataParser()
    {
        JsonSerializerSettings = new JsonSerializerSettings();
        JsonSerializerSettings.Converters.Add(new SemVersionConverter());
        JsonSerializerSettings.Converters.Add(new SemVersionRangeConverter());
    }

    public bool TryParse(string folderLocation, out SteamModMetadata steamModMetadata)
    {
        var completePath = Path.Combine(folderLocation, "meta.json");
        if (!File.Exists(completePath))
        {
            steamModMetadata = null;
            return false;
        }

        var serializableModMetadata =
            JsonConvert.DeserializeObject<SerializableModMetadata>(File.ReadAllText(completePath),
                JsonSerializerSettings);

        steamModMetadata = FromSerializable(serializableModMetadata);
        return true;

        SteamModMetadata FromSerializable(SerializableModMetadata s)
        {
            return new SteamModMetadata(s.Name, s.Version, s.Title, s.Description, s.IconPath, s.Author,
                s.Assemblies.Select(x => Path.Combine(folderLocation, x)), s.Dependencies);
        }
    }
}