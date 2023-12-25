using DG.Tweening;
using PlatformShooter.Config;
using PlatformShooter.Systems;
using PlatformShooter.Weapon;
using UnityEngine;

namespace PlatformShooter.Character
{
    public class PlayerController : CharacterBase
    {
        [SerializeField]
        private RangeWeaponBase weapon;
        private SpriteRenderer srWeapon;
        public Transform cameraTarget;
        private float timeFromLastJumpButtonDown;
        protected override Color OriginColor => Color.blue;

        protected override void Awake()
        {
            base.Awake();
            maxSpeed = 10f;
            jumpForce = 15f;
            srWeapon = weapon.GetComponent<SpriteRenderer>();
            
        }

        public override void Init()
        {
            base.Init();
            srWeapon.color = Color.white;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.TryGetComponent<HitBox>(out var hitBox) && hitBox.owner 
                                                             && hitBox.owner is SimpleNpc npc)
            {
                switch (hitBox.name)
                {
                    case "Head" when !isDead && !npc.IsDead && velocity.y < 0 && !isGrounded:
                        npc.OnStepOn();
                        Jump(jumpForce);
                        break;
                    case "Melee" when !ConfigHelper.IsInvincible.Value && !isDead && !npc.IsDead:
                        OnHit(npc);
                        break;
                }
            }
        }

        private void OnHit(SimpleNpc npc)
        {
            if (isDead) return;
            Hp.Value -= 10;
            var dir = (npc.transform.position - transform.position).x > 0 ? Vector2.left : Vector2.right;
            if (Hp.Value <= 0)
            {
                AddAlterForce(dir * 3f, 1);
            }
            else
            {
                AddAlterForce(dir * 12f, 0.2f);
            }
        }

        protected override void OnHpZero()
        {
            if (isDead) return;
            velocity.y = 3f;
            isDead = true;
            DOTween.Sequence().Append(spriteRenderer.DOFade(0, 1f))
                .Join(srWeapon.DOFade(0, 1f))
                .AppendCallback(GameSystem.I.InitGame);
        }

        private void Jump(float force)
        {
            velocity.y = force;
        }

        /// <summary>
        /// 0.1s前按下跳跃键都算作跳跃
        /// </summary>
        /// <returns></returns>
        private bool InputJump()
        {
            if (Input.GetButtonDown("Jump"))
            {
                timeFromLastJumpButtonDown = 0;
            }
            else
            {
                timeFromLastJumpButtonDown += Time.deltaTime;
            }
            return timeFromLastJumpButtonDown < 0.1f;
        }
        /// <summary>
        /// 计算速度
        /// </summary>
        protected override void ComputeVelocity()
        {
            base.ComputeVelocity();
            if(isDead) return;
            var move = Vector2.zero;
            move.x = Input.GetAxis("Horizontal");
            if (move.x > 0.01f)
            {
                Face(Vector2.right);
            }
            else if (move.x < -0.01f)
            {
                Face(Vector2.left);
            }
            if (InputJump() && isGrounded)
            {
                velocity.y = jumpForce;
            }
            else if (Input.GetButtonUp("Jump"))
            {
                if (velocity.y > 0)
                {
                    velocity.y *= 0.5f;
                }
            }
            targetVelocity += move * maxSpeed;
        }
        protected override void Update()
        {
            base.Update();
            
            if (Input.GetButton("Fire1") && GameSystem.I.EventSystem.IsPointerOverGameObject() == false)
            {
                Attack();
            }
        }
        private void Attack()
        {
            if(isDead) return;
            if (weapon.Fire(this, faceDirection))
            {
                GameSystem.I.CameraCtrl.Shake(-faceDirection * 0.1f);
            }
        }
    }
}