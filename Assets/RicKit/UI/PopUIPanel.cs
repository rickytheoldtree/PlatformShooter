using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace RicKit.UI
{
    public abstract class PopUIPanel : DoTweenUIPanel
    {
        protected const float Duration = 0.3f;

        [SerializeField] private CanvasGroup cgBlocker;

        [SerializeField] protected Transform panel;

        [SerializeField] protected Button btnBack;

        protected override void Awake()
        {
            base.Awake();
            if (btnBack)
                btnBack.onClick.AddListener(OnBackClick);
            cgBlocker.alpha = 0;
            cgBlocker.blocksRaycasts = true;
            CanvasGroup.alpha = 0;
        }

        public override void OnESCClick()
        {
            if (!btnBack || !btnBack.gameObject.activeSelf) return;
            OnBackClick();
        }

        protected override void OnAnimationIn()
        {
            sq.Kill(true);
            panel.localScale = 0.1f * Vector3.one;
            cgBlocker.alpha = 0;
            CanvasGroup.alpha = 0;
            sq = DOTween.Sequence();
            sq.Append(panel.DOScale(1, Duration).SetEase(Ease.OutBack)).onComplete += OnAnimationInEnd;
            sq.Join(cgBlocker.DOFade(1, Duration));
            sq.Join(CanvasGroup.DOFade(1, Duration * 0.4f));
        }

        protected override void OnAnimationOut()
        {
            sq.Kill(true);
            sq = DOTween.Sequence();
            sq.Append(panel.DOScale(0, Duration).SetEase(Ease.InBack)).onComplete +=
                () => OnAnimationOutEnd();
            sq.Join(cgBlocker.DOFade(0, Duration));
            sq.Insert(Duration * 0.6f, CanvasGroup.DOFade(0, Duration * 0.4f));
        }

        protected virtual void OnBackClick()
        {
            UI.Back();
        }
    }
}