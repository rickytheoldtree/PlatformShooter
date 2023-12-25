using DG.Tweening;
using PlatformShooter.Weapon;
using UnityEngine;

namespace PlatformShooter.Character
{
    public class SimpleNpc : CharacterBase
    {
        private Vector2 direction;
        [SerializeField] private SpriteRenderer srMelee;
        protected override Color OriginColor => Color.red;

        public override void Init()
        {
            base.Init();
            direction = Vector2.left;
            srMelee.color = Color.white;
        }

        protected override void OnHpZero()
        {
            if (isDead)
            {
                return;
            }
            velocity.y = 3f;
            isDead = true;
            DOTween.Sequence().Append(spriteRenderer.DOFade(0, 1f))
                .Join(srMelee.DOFade(0, 1f))
                .AppendCallback(() => Destroy(gameObject));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<ProjectileBase>(out var projectile))
            {
                OnProjectileImpact(projectile);
            }
        }

        private void OnProjectileImpact(ProjectileBase obj)
        {
            if (isDead) return;
            obj.OnImpact();
            Hp.Value -= obj.Damage;
            if (Hp.Value <= 0)
            {
                AddAlterForce(obj.Direction.normalized * 4f, 1);
            }
            else
            {
                AddAlterForce(obj.Direction.normalized * 8f, 0.2f);
            }
            
        }

        protected override void ComputeVelocity()
        {
            base.ComputeVelocity();
            if (isDead) return;
            if (direction.x > 0.01f)
            {
                Face(Vector2.right);
            }
            else if (direction.x < -0.01f)
            {
                Face(Vector2.left);
            }
            targetVelocity += direction * maxSpeed;
        }
        
        public void Jump()
        {
            if (isGrounded)
            {
                velocity.y = jumpForce;
            }
        }

        public void OnRecycle()
        {
            gameObject.SetActive(false);
        }

        public void OnStepOn()
        {
            Hp.Value -= 50;
            AddAlterForce(-direction * 10f, 0.2f);
        }

        public void ChangeDirection()
        {
            direction = -direction;
        }
    }
}