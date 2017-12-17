using UnityEngine;

namespace CI.QuickSave.Core.Models
{
    public class Texture2D
    {
        public byte[] pixels;

        public UnityEngine.Texture2D ToUnityType()
        {
            UnityEngine.Texture2D texture2D = new UnityEngine.Texture2D(1, 1);
            texture2D.LoadImage(pixels);
            texture2D.Apply();
            return texture2D;
        }

        public static Texture2D FromUnityType(UnityEngine.Texture2D texture2D)
        {
            return new Texture2D()
            {
                pixels = texture2D.EncodeToPNG(),
            };
        }
    }
}