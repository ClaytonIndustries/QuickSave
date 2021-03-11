////////////////////////////////////////////////////////////////////////////////
//  
// @module Quick Save for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using CI.QuickSave.Core.Converters;
using Newtonsoft.Json;

namespace CI.QuickSave.Core.Serialisers
{
    public static class JsonSerialiser
    {
        private static readonly JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            Converters = new List<JsonConverter>()
            {
                new ColorConverter(),
                new QuaternionConverter(),
                new Matrix4x4Converter(),
                new Texture2DConverter(),
                new SpriteConverter()
            }
        };

        public static string Serialise<T>(T value)
        {
            return JsonConvert.SerializeObject(value, settings);
        }

        public static T Deserialise<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, settings);
        }
    }
}