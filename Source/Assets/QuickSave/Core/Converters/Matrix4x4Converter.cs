using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CI.QuickSave.Core.Converters
{
    public class Matrix4x4Converter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Matrix4x4);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var val = JObject.Load(reader);
            return new Matrix4x4()
            {
                m00 = ((float?)val["m00"]).GetValueOrDefault(),
                m33 = ((float?)val["m33"]).GetValueOrDefault(),
                m23 = ((float?)val["m23"]).GetValueOrDefault(),
                m13 = ((float?)val["m13"]).GetValueOrDefault(),
                m03 = ((float?)val["m03"]).GetValueOrDefault(),
                m32 = ((float?)val["m32"]).GetValueOrDefault(),
                m22 = ((float?)val["m22"]).GetValueOrDefault(),
                m02 = ((float?)val["m02"]).GetValueOrDefault(),
                m12 = ((float?)val["m12"]).GetValueOrDefault(),
                m21 = ((float?)val["m21"]).GetValueOrDefault(),
                m11 = ((float?)val["m11"]).GetValueOrDefault(),
                m01 = ((float?)val["m01"]).GetValueOrDefault(),
                m30 = ((float?)val["m30"]).GetValueOrDefault(),
                m20 = ((float?)val["m20"]).GetValueOrDefault(),
                m10 = ((float?)val["m10"]).GetValueOrDefault(),
                m31 = ((float?)val["m31"]).GetValueOrDefault(),
            };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var val = (Matrix4x4)value;
            serializer.Serialize(writer, new 
            { 
                val.m00,
                val.m33,
                val.m23,
                val.m13,
                val.m03,
                val.m32,
                val.m22,
                val.m02,
                val.m12,
                val.m21,
                val.m11,
                val.m01,
                val.m30,
                val.m20,
                val.m10,
                val.m31
            });
        }
    }
}