using System;
using Newtonsoft.Json;
using Semver;

namespace ShapezSteamModPackageManager
{
    public class SemVersionConverter : JsonConverter<SemVersion>
    {
        public override void WriteJson(JsonWriter writer, SemVersion value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override SemVersion ReadJson(JsonReader reader, Type objectType, SemVersion existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                throw new JsonReaderException("SemVersion value is null");
            }

            var version = (string)reader.Value;
            return SemVersion.Parse(version, SemVersionStyles.Any);
        }
    }

    public class SemVersionRangeConverter : JsonConverter<SemVersionRange>
    {
        public override void WriteJson(JsonWriter writer, SemVersionRange value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override SemVersionRange ReadJson(JsonReader reader, Type objectType, SemVersionRange existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                throw new JsonReaderException("SemVersionRange value is null");
            }

            var version = (string)reader.Value;
            return SemVersionRange.Parse(version, SemVersionRangeOptions.Loose);
        }
    }
}