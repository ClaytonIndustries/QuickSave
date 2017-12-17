
namespace CI.QuickSave.Core.Models
{
    public class Vector3
    {
        public float x;
        public float y;
        public float z;

        public UnityEngine.Vector3 ToUnityType()
        {
            return new UnityEngine.Vector3(x, y, z);
        }

        public static Vector3 FromUnityType(UnityEngine.Vector3 vector3)
        {
            return new Vector3()
            {
                x = vector3.x,
                y = vector3.y,
                z = vector3.z
            };
        }
    }
}