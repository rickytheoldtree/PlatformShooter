using UnityEngine;

namespace RicKit.Utils
{
    public static class TextureUtils
    {
        public static Texture2D ToTexture2D(this RenderTexture renderTexture)
        {
            var texture2D = new Texture2D(renderTexture.width, renderTexture.height);
            RenderTexture.active = renderTexture;
            texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture2D.Apply();
            return texture2D;
        }
    }
}