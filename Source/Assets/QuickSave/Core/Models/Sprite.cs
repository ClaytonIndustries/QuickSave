
namespace CI.QuickSave.Core.Models
{
    public class Sprite
    {
        public Texture2D texture;
        public Rect rect;
        public Vector2 pivot;
        public float pixelsPerUnit;
        public Vector4 border;

        public UnityEngine.Sprite ToUnityType()
        {
            return UnityEngine.Sprite.Create(texture.ToUnityType(), rect.ToUnityType(), pivot.ToUnityType(), pixelsPerUnit, 0, UnityEngine.SpriteMeshType.Tight, border.ToUnityType());
        }

        public static Sprite FromUnityType(UnityEngine.Sprite sprite)
        {
            return new Sprite()
            {
                texture = Texture2D.FromUnityType(sprite.texture),
                rect = Rect.FromUnityType(sprite.rect),
                pivot = Vector2.FromUnityType(sprite.pivot),
                pixelsPerUnit = sprite.pixelsPerUnit,
                border = Vector4.FromUnityType(sprite.border)
            };
        }
    }
}