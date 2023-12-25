using PlatformShooter.Character;
using PlatformShooter.Systems;
using UnityEngine;
using ParticleSystem = PlatformShooter.Systems.ParticleSystem;

namespace PlatformShooter.Weapon
{
    public class ProjectileBase : MonoBehaviour
    {
        public Vector2 Direction { get; private set; }
        public CharacterBase Parent { get; private set; }
        private float speed;
        private float lifeTime;
        public int Damage { get; private set; }
        private float lifeTimer;
        public void Init(CharacterBase mParent, ProjectileInfo info)
        {
            Parent = mParent;
            speed = info.speed;
            lifeTime = info.lifeTime;
            Damage = info.damage;
            lifeTimer = 0f;
        }
        public virtual void OnImpact()
        {
            ParticleSystem.I.ShowHitEffect(transform.position);
            ProjectileSystem.I.Recycle(this);
        }
        public void Launch(Vector2 dir)
        {
            Direction = dir;
        }
        public void OnRecycle()
        {
            gameObject.SetActive(false);
        }
        private void Update()
        {
            transform.Translate(Direction * (speed * Time.deltaTime));
            lifeTimer += Time.deltaTime;
            if (lifeTimer >= lifeTime)
            {
                ProjectileSystem.I.Recycle(this);
            }
        }
    }
}