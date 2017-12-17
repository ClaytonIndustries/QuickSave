
namespace CI.QuickSave.Core.Models
{
    public class Vector2
    {
        public float x;
        public float y;

        public UnityEngine.Vector2 ToUnityType()
        {
            return new UnityEngine.Vector2(x, y);
        }

        public static Vector2 FromUnityType(UnityEngine.Vector2 vector2)
        {
            return new Vector2()
            {
                x = vector2.x,
                y = vector2.y
            };
        }
    }
}