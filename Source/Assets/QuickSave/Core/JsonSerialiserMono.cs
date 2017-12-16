#if !NETFX_CORE
using JsonFx.Json;
using JsonFx.Json.Resolvers;
using JsonFx.Serialization;
using JsonFx.Serialization.Resolvers;
using JsonFx.Xml.Resolvers;

namespace CI.QuickSave.Core
{
    public class JsonSerialiserMono : IJsonSerialiser
    {
        public string Serialise<T>(T value)
        {
            CombinedResolverStrategy resolver = new CombinedResolverStrategy(new JsonResolverStrategy());

            JsonWriter writer = new JsonWriter(new DataWriterSettings(resolver));

            return writer.Write(value);
        }

        public T Deserialise<T>(string json)
        {
            //CombinedResolverStrategy resolver = new CombinedResolverStrategy(new JsonResolverStrategy());

            // Some way to prevent circular objects

            var resolver = new CombinedResolverStrategy(
                new JsonResolverStrategy(),                                                             // simple JSON attributes
                new DataContractResolverStrategy(),                                                     // DataContract attributes
                new XmlResolverStrategy(),                                                              // XmlSerializer attributes
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.PascalCase),       // DotNetStyle
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.CamelCase),        // jsonStyle
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.Lowercase, "-"),   // xml-style
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.Uppercase, "_"));	// CONST_STYLE


            JsonReader reader = new JsonReader(new DataReaderSettings(resolver));

            return reader.Read<T>(json);
        }
    }
}
#endif