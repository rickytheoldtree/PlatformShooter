using PlatformShooter.Character;
using PlatformShooter.Systems;
using UnityEngine;

namespace PlatformShooter.Weapon
{
    public abstract class RangeWeaponBase : MonoBehaviour
    {
        public float fireRate = 0.1f;
        private float fireTimer;
        protected ProjectileInfo ProjectileInfo { get; set; }

        public bool Fire(CharacterBase user, Vector2 dir)
        {
            dir += new Vector2(0, Random.Range(-0.1f, 0.1f));
            if (fireTimer >= fireRate)
            {
                fireTimer = 0f;
                ProjectileSystem.I.LaunchProjectile(user, transform.position, ProjectileInfo, dir);
                if (user)
                {
                    user.AddAlterForce(-dir.normalized * 1f, 0.1f);
                }
                return true;
            }
            return false;
        }

        private void Update()
        {
            fireTimer += Time.deltaTime;
        }
    }
}