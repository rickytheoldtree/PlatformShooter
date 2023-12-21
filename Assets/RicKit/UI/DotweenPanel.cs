using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RicKit.UI;
using UnityEngine;

namespace RicKit.UI
{
    public abstract class DoTweenUIPanel : AbstractUIPanel
    {
        protected const float FadeTime = 0.2f;
        protected Sequence sq;

        protected override void OnAnimationIn()
        {
            CanvasGroup.alpha = 0;
            sq.Kill(true);
            sq = DOTween.Sequence();
            sq.Join(CanvasGroup.DOFade(1, FadeTime));
            sq.onComplete += OnAnimationInEnd;
        }

        protected override void OnAnimationOut()
        {
            sq.Kill(true);
            sq = DOTween.Sequence();
            sq.Join(CanvasGroup.DOFade(0, FadeTime));
            sq.onComplete += () => OnAnimationOutEnd();
        }
    }
}

