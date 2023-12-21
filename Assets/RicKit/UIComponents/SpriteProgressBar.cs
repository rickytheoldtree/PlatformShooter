using DG.Tweening;
using UnityEngine;

namespace RicKit.UIComponents
{
    public class SpriteProgressBar : MonoBehaviour
    {
        [SerializeField] private Transform bar;
        protected Sequence changeSq, showSq;
        private float maxScale;

        private void Awake()
        {
            maxScale = bar.localScale.x;
        }

        public void SetProgress(float progress)
        {
            bar.localScale = new Vector3(progress * maxScale, 1, 1);
        }

        public Tween Show()
        {
            showSq?.Kill();
            showSq = DOTween.Sequence().Append(transform.DOScale(1, 0.2f));
            return showSq;
        }

        public void ShowImmediate()
        {
            showSq?.Kill();
            transform.localScale = Vector3.one;
        }

        public Tween Hide()
        {
            showSq?.Kill();
            showSq = DOTween.Sequence().Append(transform.DOScale(0, 0.2f));
            return showSq;
        }

        public void HideImmediate()
        {
            showSq?.Kill();
            transform.localScale = Vector3.zero;
        }

        public void ChangeProgress(float progress, float duration)
        {
            changeSq?.Kill();
            changeSq = DOTween.Sequence().Append(bar.DOScaleX(maxScale * progress, duration));
        }
    }
}