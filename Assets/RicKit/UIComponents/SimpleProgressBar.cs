using DG.Tweening;
using UnityEngine;

namespace RicKit.UIComponents
{
    public class SimpleProgressBar : MonoBehaviour
    {
        [SerializeField] protected RectTransform bar;
        protected Sequence changeSeq;
        private float maxWidth;
        private float height;
        protected virtual void Awake()
        {
            maxWidth = bar.sizeDelta.x;
            height = bar.sizeDelta.y;
        }
        public void SetProgress(float progress)
        {
            bar.sizeDelta = new Vector2(maxWidth * progress, height);
        }
        public void ChangeProgress(float progress, float duration)
        {
            changeSeq?.Kill();
            changeSeq = DOTween.Sequence().Append(bar.DOSizeDelta(new Vector2(maxWidth * progress, height), duration));
        }
    }
}