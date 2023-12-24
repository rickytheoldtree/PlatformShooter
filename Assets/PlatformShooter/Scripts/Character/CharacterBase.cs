using System;
using System.Collections;
using DG.Tweening;
using RicKit.Common;
using UnityEngine;

namespace PlatformShooter.Character
{
    public class CharacterBase : Physics2DBase
    {
        public float maxSpeed = 10f;
        public float jumpForce = 10f;
        protected Vector2 faceDirection = Vector2.right;
        protected SpriteRenderer spriteRenderer;
        protected virtual Color OriginColor => Color.white;
        protected Vector2 alterForce;
        protected EventProperty<int> Hp { get; } = new EventProperty<int>(100);
        public bool IsDead => isDead;
        protected bool isDead;
        public Vector2 FaceDirection => faceDirection;
        protected override void Awake()
        {
            base.Awake();
            spriteRenderer = GetComponent<SpriteRenderer>();
            Hp.AddListener(OnHpChanged);
        }

        public virtual void Init()
        {
            velocity = Vector2.zero;
            isDead = false;
            groundNormal = Vector2.zero;
            Hp.SetWithoutNotify(100);
            spriteRenderer.color = OriginColor;
            transform.localScale = Vector3.one;
        }
        private Tween damageTween;
        private void OnHpChanged(int value, int oldValue)
        {
            if(oldValue > value)
            {
                OnHpDecrease();
            }
            if (value <= 0)
            {
                OnHpZero();
            }
        }
        protected virtual void OnHpDecrease()
        {
            damageTween?.Kill();
            damageTween = DOTween.Sequence()
                .Append(spriteRenderer.DOColor(Color.white, 0.05f))
                .Append(spriteRenderer.DOColor(OriginColor, 0.05f));
        }
        protected virtual void OnHpZero()
        {
            
        }

        /// <summary>
        /// 为了游戏性，重力加速度会随着空中停留时间增加而增加
        /// </summary>
        protected override void ComputeGravity()
        {
            GravityMod = Mathf.Lerp(1f, 3f, airTime);
        }

        protected override void ComputeVelocity()
        {
            targetVelocity += alterForce;
            alterForce = Vector2.zero;
        }

        /// <summary>
        ///  当面朝方向改变时，改变物体的朝向
        /// </summary>
        protected void Face(Vector2 dir)
        {
            faceDirection = dir;
            transform.localScale = new Vector3(dir.x, 1, 1);
        }
        
        public void AddAlterForce(Vector2 v, float time)
        {
            StartCoroutine(AlterForce(v, time));
        }
        private IEnumerator AlterForce(Vector2 v, float time)
        {
            var timer = 0f;
            while (timer < time)
            {
                alterForce += Vector2.Lerp(v, Vector2.zero, timer / time);
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
}