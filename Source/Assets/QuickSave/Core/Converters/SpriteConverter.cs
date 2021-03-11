using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CI.QuickSave.Core.Converters
{
    public class SpriteConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Sprite);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var val = JObject.Load(reader);

            return Sprite.Create(val["texture"].ToObject<Texture2D>(serializer),
                val["rect"].ToObject<Rect>(serializer),
                val["pivot"].ToObject<Vector2>(serializer),
                ((float?)val["pixelsPerUnit"]).GetValueOrDefault(),
                0, SpriteMeshType.Tight,
                val["border"].ToObject<Vector4>(serializer)
            );
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var val = (Sprite)value;
            serializer.Serialize(writer, new 
            { 
                val.texture,
                val.rect,
                val.pivot,
                val.pixelsPerUnit,
                val.border
            });
        }
    }
}