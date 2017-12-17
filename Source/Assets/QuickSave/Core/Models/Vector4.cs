
namespace CI.QuickSave.Core.Models
{
    public class Vector4
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public UnityEngine.Vector4 ToUnityType()
        {
            return new UnityEngine.Vector4(x, y, z, w);
        }

        public static Vector4 FromUnityType(UnityEngine.Vector4 vector4)
        {
            return new Vector4()
            {
                x = vector4.x,
                y = vector4.y,
                z = vector4.z,
                w = vector4.w
            };
        }
    }
}