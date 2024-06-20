using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CI.QuickSave.Core.Converters
{
    public class TransformConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Transform);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var val = JObject.Load(reader);

            return val.ToObject<Transform>();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var val = (Transform)value;
            serializer.Serialize(writer, new
            {
                val.localPosition,
                val.localRotation,
                val.localScale,
                val.position
            });
        }
    }
}