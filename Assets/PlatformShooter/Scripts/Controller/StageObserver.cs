using System;
using DG.Tweening;
using PlatformShooter.Systems;
using UnityEngine;

namespace PlatformShooter.Controller
{
    public class StageObserver : MonoBehaviour
    {
        private Sequence sq;
        private readonly float speed = 5f;
        public void StartMove(params Vector2[] points)
        {
            sq?.Kill();
            GameSystem.I.CameraCtrl.SetTargets(transform);
            transform.position = points[0];
            sq = DOTween.Sequence();
            for (var i = 0; i < points.Length; i++)
            {
                var targetPoint = points[i + 1 < points.Length ? i + 1 : 0];
                var point = points[i];
                var distance = Vector2.Distance(point, targetPoint);
                sq.Append(transform.DOMove(targetPoint, distance / speed).SetEase(Ease.Linear));
            }
            sq.SetLoops(-1);
        }

        private void OnDestroy()
        {
            sq?.Kill();
        }
    }
}