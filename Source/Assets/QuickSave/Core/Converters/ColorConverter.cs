using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CI.QuickSave.Core.Converters
{
    public class ColorConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Color);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var val = JObject.Load(reader);
            return new Color(((float?)val["r"]).GetValueOrDefault(), ((float?)val["g"]).GetValueOrDefault(), ((float?)val["b"]).GetValueOrDefault(), ((float?)val["a"]).GetValueOrDefault());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var val = (Color)value;
            serializer.Serialize(writer, new { val.r, val.g, val.b, val.a });
        }
    }
}