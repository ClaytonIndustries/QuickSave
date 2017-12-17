
namespace CI.QuickSave.Core.Models
{
    public class Color
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public UnityEngine.Color ToUnityType()
        {
            return new UnityEngine.Color(r, g, b, a);
        }

        public static Color FromUnityType(UnityEngine.Color color)
        {
            return new Color()
            {
                r = color.r,
                g = color.g,
                b = color.b,
                a = color.a
            };
        }
    }
}