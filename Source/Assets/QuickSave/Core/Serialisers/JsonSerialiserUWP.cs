////////////////////////////////////////////////////////////////////////////////
//  
// @module Quick Save for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if NETFX_CORE
using Newtonsoft.Json;

namespace CI.QuickSave.Core.Serialisers
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