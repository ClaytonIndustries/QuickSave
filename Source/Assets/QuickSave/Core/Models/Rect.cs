
namespace CI.QuickSave.Core.Models
{
    public class Rect
    {
        public float yMax;
        public float xMax;
        public float yMin;
        public float xMin;
        public float x;
        public float y;
        public float height;
        public float width;
        public Vector2 max;
        public Vector2 min;
        public Vector2 center;
        public Vector2 position;
        public Vector2 size;

        public static Rect FromUnityType(UnityEngine.Rect rect)
        {
            return new Rect()
            {
                yMax = rect.yMax,
                xMax = rect.xMax,
                yMin = rect.yMin,
                xMin = rect.xMin,
                x = rect.x,
                y = rect.y,
                height = rect.height,
                width = rect.width,
                max = Vector2.FromUnityType(rect.max),
                min = Vector2.FromUnityType(rect.min),
                center = Vector2.FromUnityType(rect.center),
                position = Vector2.FromUnityType(rect.position),
                size = Vector2.FromUnityType(rect.size),
            };
        }
    }
}