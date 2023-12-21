using DG.Tweening;
using RicKit.Common;
using UnityEngine;
using UnityEngine.UI;

namespace RicKit.MakeEditorTools
{
    public class PopMsgSystem : MonoSingleton<PopMsgSystem>
    {
        private CanvasGroup cg;
        private Text text;
        protected override void GetAwake()
        {
            var canvas = gameObject.AddComponent<Canvas>();
            cg = gameObject.AddComponent<CanvasGroup>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var img = new GameObject("Image", typeof(Image));
            img.transform.SetParent(canvas.transform);
            //设置锚点
            var rectTransform = img.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 0.5f);
            rectTransform.anchorMax = new Vector2(1, 0.5f);
            rectTransform.offsetMin = new Vector2(0, -100);
            rectTransform.offsetMax = new Vector2(0, 100);
            //设置图片
            img.GetComponent<Image>().color = new Color(0, 0, 0, 0.6f);
            //设置文本
            var txtGo = new GameObject("Text", typeof(Text));
            txtGo.transform.SetParent(img.transform);
            var textRectTransform = txtGo.GetComponent<RectTransform>();
            textRectTransform.anchorMin = Vector2.zero;
            textRectTransform.anchorMax = Vector2.one;
            textRectTransform.offsetMin = Vector2.zero;
            textRectTransform.offsetMax = Vector2.zero;
            text = txtGo.GetComponent<Text>();
            text.alignment = TextAnchor.MiddleCenter;
            text.fontSize = 30;
            text.color = Color.white;
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            cg.alpha = 0;
        }

        private Tween tween;
        public void Show(string msg)
        {
            tween?.Kill();
            text.text = msg;
            tween = DOTween.Sequence()
                .Append(cg.DOFade(1, 0.5f))
                .AppendInterval(1)
                .Append(cg.DOFade(0, 0.5f));
        }
    }
}