////////////////////////////////////////////////////////////////////////////////
//  
// @module Quick Save for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using CI.QuickSave.Core.Helpers;
using Newtonsoft.Json;

namespace CI.QuickSave.Core.Serialisers
{
    public static class JsonSerialiser
    {
        public static string Serialise<T>(T value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public static T Deserialise<T>(string json)
        {
            if (TypeHelper.IsUnityType<T>())
            {
                //return TypeHelper.DeserialiseUnityType<T>(json, _serialiser);

                return JsonConvert.DeserializeObject<T>(json);
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
        }
    }
}