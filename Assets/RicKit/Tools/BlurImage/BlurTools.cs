using RicKit.Utils;
using UnityEngine;

namespace RicKit.Tools.BlurTool
{
    public class BlurTools
    {
        private static BlurTools instance;
        public  static BlurTools I => instance ??= new BlurTools();
        private const int BlurHorPass = 0;
        private const int BlurVerPass = 1;

        private const float BlurSize = 2.0f; // 模糊额外散步大小
        private const int BlitTimes = 4; // 模糊采样迭代次数
        private const float BlurSpread = 2; // 模糊散值
        private static readonly int BlurSizeID = Shader.PropertyToID("_BlurSize");

        private readonly Material mat;
        private static RenderTexture finalBlurRT;
        private static RenderTexture tempRT;

        private BlurTools()
        {
            mat = new Material(Shader.Find("Hidden/Blur"));
        }

        private Sprite Blur(Sprite sprite)
        {
            var texture = sprite.texture;
            var width = texture.width;
            var height = texture.height;
            finalBlurRT = RenderTexture.GetTemporary(width, height, 0);
            Graphics.Blit(texture, finalBlurRT);
            return Blur(finalBlurRT);
        }

        public Sprite BlurCameraSprite(Camera cam)
        {
            var renderTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 0);
            if (cam.targetTexture)
            {
                var temp = cam.targetTexture;
                cam.targetTexture = renderTexture;
                cam.Render();
                cam.targetTexture = temp;
                return Blur(renderTexture);
            }
            else
            {
                cam.targetTexture = renderTexture;
                cam.Render();
                cam.targetTexture = null;
                return Blur(renderTexture);
            }
        }

        public Texture BlurCameraTex(Camera cam)
        {
            var renderTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 0);
            if (cam.targetTexture)
            {
                var temp = cam.targetTexture;
                cam.targetTexture = renderTexture;
                cam.Render();
                cam.targetTexture = temp;
                return Blur(renderTexture).texture;
            }
            else
            {
                cam.targetTexture = renderTexture;
                cam.Render();
                cam.targetTexture = null;
                return Blur(renderTexture).texture;
            }
        }
        private Sprite Blur(RenderTexture renderTexture)
        {
            for (var i = 0; i < BlitTimes; i++)
            {
                mat.SetFloat(BlurSizeID, (1.0f + i * BlurSpread) * BlurSize);
                tempRT = RenderTexture.GetTemporary(renderTexture.width, renderTexture.height, 0);
                Graphics.Blit(renderTexture, tempRT, mat, BlurHorPass);
                Graphics.Blit(tempRT, renderTexture, mat, BlurVerPass);
                RenderTexture.ReleaseTemporary(tempRT);
            }

            var texture2D = renderTexture.ToTexture2D();
            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(renderTexture);
            var blurredSprite =
                Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
            return blurredSprite;
        }
    }
}