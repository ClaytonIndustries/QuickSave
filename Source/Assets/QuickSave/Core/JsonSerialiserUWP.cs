#if NETFX_CORE
using Newtonsoft.Json;

namespace CI.QuickSave.Core
{
    public class JsonSerialiserUWP : IJsonSerialiser
    {
        public string Serialise<T>(T value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public T Deserialise<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
#endif