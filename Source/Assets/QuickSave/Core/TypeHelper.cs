using System;
using System.Collections.Generic;
using CI.QuickSave.Core.Models;

namespace CI.QuickSave.Core
{
    public static class TypeHelper
    {
        private static readonly IDictionary<string, Func<object, object>> _unityTypes = new Dictionary<string, Func<object, object>>()
        {
            { "UnityEngine.Vector2", new Func<object, object>((value) => { return Vector2.FromUnityType((UnityEngine.Vector2)value); }) },
            { "UnityEngine.Vector3", new Func<object, object>((value) => { return Vector3.FromUnityType((UnityEngine.Vector3)value); }) },
            { "UnityEngine.Vector4", new Func<object, object>((value) => { return Vector4.FromUnityType((UnityEngine.Vector4)value); }) },
            { "UnityEngine.Quaternion", new Func<object, object>((value) => { return Quaternion.FromUnityType((UnityEngine.Quaternion)value); }) },
            { "UnityEngine.Color", new Func<object, object>((value) => { return Color.FromUnityType((UnityEngine.Color)value); }) },
            { "UnityEngine.Color32", new Func<object, object>((value) => { return Color32.FromUnityType((UnityEngine.Color32)value); }) },
            { "UnityEngine.Rect", new Func<object, object>((value) => { return Rect.FromUnityType((UnityEngine.Rect)value); }) },
            { "UnityEngine.Bounds", new Func<object, object>((value) => { return Bounds.FromUnityType((UnityEngine.Bounds)value); }) },
            { "UnityEngine.Matrix4x4", new Func<object, object>((value) => { return Matrix4x4.FromUnityType((UnityEngine.Matrix4x4)value); }) }
        };

        private static readonly IDictionary<string, Func<string, object>> _quickSaveTypes = new Dictionary<string, Func<string, object>>()
        {
            { "UnityEngine.Vector2", new Func<string, object>((value) => { return JsonSerialiser.Deserialise<Vector2>(value).ToUnityType(); }) }
        };

        public static object ReplaceIfUnityType<T>(T value)
        {
            string typename = typeof(T).FullName;

            if (_unityTypes.ContainsKey(typename))
            {
                return _unityTypes[typename](value);
            }

            return value;
        }

        public static bool IsUnityType<T>()
        {
            string typename = typeof(T).FullName;

            return _quickSaveTypes.ContainsKey(typename);
        }

        public static T ReplaceIfQuickSaveType<T>(string value)
        {
            string typename = typeof(T).FullName;

            return (T)_quickSaveTypes[typename](value);
        }
    }
}