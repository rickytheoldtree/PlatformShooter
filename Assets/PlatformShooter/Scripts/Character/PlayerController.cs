using System;
using PlatformShooter.Event;
using PlatformShooter.Systems;
using PlatformShooter.Weapon;
using UnityEngine;

namespace PlatformShooter.Character
{
    public class PlayerController : CharacterBase
    {
        [SerializeField]
        private RangeWeaponBase weapon;
        private float timeFromLastJumpButtonDown;
        protected override Color OriginColor => Color.blue;


        protected override void Awake()
        {
            base.Awake();
            maxSpeed = 10f;
            jumpForce = 15f;
        }

        private void Start()
        {
            Init();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(velocity.y < 0 && other.transform.parent && other.transform.parent.TryGetComponent(out SimpleNpc simpleNpc) 
               && !simpleNpc.IsDead)
            {
                simpleNpc.OnStepOn();
                Jump(jumpForce);
            }
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
            if (Input.GetMouseButton(0))
            {
                Attack();
            }
        }
        private void Attack()
        {
            weapon.Fire(this, faceDirection);
        }

        public GameObject GameObject => gameObject;
        public CharacterBase CharBase => this;
    }
}