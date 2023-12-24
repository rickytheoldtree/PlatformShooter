using PlatformShooter.Character;
using PlatformShooter.Systems;
using UnityEngine;

namespace PlatformShooter.Weapon
{
    public abstract class RangeWeaponBase : MonoBehaviour, IWeapon
    {
        public float fireRate = 0.1f;
        private float fireTimer;
        protected ProjectileInfo ProjectileInfo { get; set; }

        public void Fire(CharacterBase user, Vector2 dir)
        {
            //dir 做细微偏移
            dir += new Vector2(0, Random.Range(-0.1f, 0.1f));
            if (fireTimer >= fireRate)
            {
                fireTimer = 0f;
                ProjectileSystem.I.LaunchProjectile(user, transform.position, ProjectileInfo, dir);
                if (user)
                {
                    user.AddAlterForce(-dir.normalized * 3f, 0.1f);
                }
            }
        }

        private void Update()
        {
            fireTimer += Time.deltaTime;
        }
    }
}