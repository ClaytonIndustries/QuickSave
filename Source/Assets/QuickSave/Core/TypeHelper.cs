using System.Linq;
using System.Collections.Generic;

namespace CI.QuickSave.Core
{
    public static class TypeHelper
    {
        private static IEnumerable<string> _unityTypes = new List<string>()
        {
            "UnityEngine.Vector2",
            "UnityEngine.Vector3",
            "UnityEngine.Vector4",
            "UnityEngine.Quaternion",
            "UnityEngine.Color",
            "UnityEngine.Color32",
            "UnityEngine.Rect",
            "UnityEngine.Bounds",
            "UnityEngine.Matrix4x4"
        };

        public static bool IsUnityType<T>(T value)
        {
            string typename = value.GetType().FullName;

            return _unityTypes.Any(x => x == typename);
        }
    }
}