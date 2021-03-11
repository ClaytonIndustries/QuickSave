using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CI.QuickSave.Core.Converters
{
    public class Texture2DConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Texture2D);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var val = JObject.Load(reader);
            var texture2D = new Texture2D(1, 1);
            texture2D.LoadImage((byte[])val["pixels"]);
            texture2D.Apply();
            return texture2D;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var val = (Texture2D)value;
            serializer.Serialize(writer, new { pixels = val.EncodeToPNG() });
        }
    }
}