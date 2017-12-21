using System;
using System.Collections.Generic;
using CI.QuickSave.Core.Models;
using CI.QuickSave.Core.Serialisers;

namespace CI.QuickSave.Core.Helpers
{
    public static class TypeHelper
    {
        private static readonly IDictionary<string, Func<object, object>> _unityTypeToQuickSaveType = new Dictionary<string, Func<object, object>>()
        {
            { "UnityEngine.Vector2", new Func<object, object>((value) => { return Vector2.FromUnityType((UnityEngine.Vector2)value); }) },
            { "UnityEngine.Vector3", new Func<object, object>((value) => { return Vector3.FromUnityType((UnityEngine.Vector3)value); }) },
            { "UnityEngine.Vector4", new Func<object, object>((value) => { return Vector4.FromUnityType((UnityEngine.Vector4)value); }) },
            { "UnityEngine.Quaternion", new Func<object, object>((value) => { return Quaternion.FromUnityType((UnityEngine.Quaternion)value); }) },
            { "UnityEngine.Color", new Func<object, object>((value) => { return Color.FromUnityType((UnityEngine.Color)value); }) },
            { "UnityEngine.Color32", new Func<object, object>((value) => { return Color32.FromUnityType((UnityEngine.Color32)value); }) },
            { "UnityEngine.Rect", new Func<object, object>((value) => { return Rect.FromUnityType((UnityEngine.Rect)value); }) },
            { "UnityEngine.Bounds", new Func<object, object>((value) => { return Bounds.FromUnityType((UnityEngine.Bounds)value); }) },
            { "UnityEngine.Matrix4x4", new Func<object, object>((value) => { return Matrix4x4.FromUnityType((UnityEngine.Matrix4x4)value); }) },
            { "UnityEngine.Texture2D", new Func<object, object>((value) => { return Texture2D.FromUnityType((UnityEngine.Texture2D)value); }) },
            { "UnityEngine.Sprite", new Func<object, object>((value) => { return Sprite.FromUnityType((UnityEngine.Sprite)value); }) }
        };

        private static readonly IDictionary<string, Func<string, IJsonSerialiser, object>> _quickSaveTypeToUnityType = new Dictionary<string, Func<string, IJsonSerialiser, object>>()
        {
            { "UnityEngine.Vector2", new Func<string, IJsonSerialiser, object>((value, serialiser) => { return serialiser.Deserialise<Vector2>(value).ToUnityType(); }) },
            { "UnityEngine.Vector3", new Func<string, IJsonSerialiser, object>((value, serialiser) => { return serialiser.Deserialise<Vector3>(value).ToUnityType(); }) },
            { "UnityEngine.Vector4", new Func<string, IJsonSerialiser, object>((value, serialiser) => { return serialiser.Deserialise<Vector4>(value).ToUnityType(); }) },
            { "UnityEngine.Quaternion", new Func<string, IJsonSerialiser, object>((value, serialiser) => { return serialiser.Deserialise<Quaternion>(value).ToUnityType(); }) },
            { "UnityEngine.Color", new Func<string, IJsonSerialiser, object>((value, serialiser) => { return serialiser.Deserialise<Color>(value).ToUnityType(); }) },
            { "UnityEngine.Color32", new Func<string, IJsonSerialiser, object>((value, serialiser) => { return serialiser.Deserialise<Color32>(value).ToUnityType(); }) },
            { "UnityEngine.Rect", new Func<string, IJsonSerialiser, object>((value, serialiser) => { return serialiser.Deserialise<Rect>(value).ToUnityType(); }) },
            { "UnityEngine.Bounds", new Func<string, IJsonSerialiser, object>((value, serialiser) => { return serialiser.Deserialise<Bounds>(value).ToUnityType(); }) },
            { "UnityEngine.Matrix4x4", new Func<string, IJsonSerialiser, object>((value, serialiser) => { return serialiser.Deserialise<Matrix4x4>(value).ToUnityType(); }) },
            { "UnityEngine.Texture2D", new Func<string, IJsonSerialiser, object>((value, serialiser) => { return serialiser.Deserialise<Texture2D>(value).ToUnityType(); }) },
            { "UnityEngine.Sprite", new Func<string, IJsonSerialiser, object>((value, serialiser) => { return serialiser.Deserialise<Sprite>(value).ToUnityType(); }) }
        };

        public static object ReplaceIfUnityType<T>(T value)
        {
            string typename = typeof(T).FullName;

            if (_unityTypeToQuickSaveType.ContainsKey(typename))
            {
                return _unityTypeToQuickSaveType[typename](value);
            }

            return value;
        }

        public static bool IsUnityType<T>()
        {
            string typename = typeof(T).FullName;

            return _unityTypeToQuickSaveType.ContainsKey(typename);
        }

        public static T DeserialiseUnityType<T>(string value, IJsonSerialiser jsonSerialiser)
        {
            string typename = typeof(T).FullName;

            return (T)_quickSaveTypeToUnityType[typename](value, jsonSerialiser);
        }
    }
}