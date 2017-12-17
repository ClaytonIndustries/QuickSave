
namespace CI.QuickSave.Core.Models
{
    public class Vector4
    {
        public float x;
        public float y;
        public float z;
        public float w;

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