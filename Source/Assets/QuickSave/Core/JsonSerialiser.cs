using JsonFx.Json;
using JsonFx.Json.Resolvers;
using JsonFx.Serialization;
using JsonFx.Serialization.Resolvers;

namespace CI.QuickSave.Core
{
    public static class JsonSerialiser
    {
        public static string Serialise<T>(T value)
        {
            CombinedResolverStrategy resolver = new CombinedResolverStrategy(new JsonResolverStrategy());

            JsonWriter writer = new JsonWriter(new DataWriterSettings(resolver));

            return writer.Write(value);
        }

        public static T Deserialise<T>(string json)
        {
            CombinedResolverStrategy resolver = new CombinedResolverStrategy(new JsonResolverStrategy());

            JsonReader reader = new JsonReader(new DataReaderSettings(resolver));

            return reader.Read<T>(json);
        }
    }
}
