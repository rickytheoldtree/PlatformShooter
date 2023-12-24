using PlatformShooter.Character;
using PlatformShooter.Weapon;
using RicKit.Common;
using RicKit.Pool;
using UnityEngine;

namespace PlatformShooter.Systems
{
    public class ProjectileSystem : MonoSingleton<ProjectileSystem>
    {
        private MonoPool<ProjectileBase> projectilePool;
        private ProjectileBase projectilePrefab;
        protected override void GetAwake()
        {
            projectilePrefab = Resources.Load<ProjectileBase>("Prefabs/Projectile");
            projectilePool = new MonoPool<ProjectileBase>(() =>
            {
                var projectile = Instantiate(projectilePrefab, transform);
                projectile.gameObject.SetActive(false);
                return projectile;
            }, p => p.OnRecycle());
        }
        public void LaunchProjectile(CharacterBase user, Vector3 position, ProjectileInfo info, Vector2 dir)
        {
            var projectile = projectilePool.Allocate();
            projectile.Init(user, info);
            projectile.transform.position = position;
            projectile.Launch(dir);
            projectile.gameObject.SetActive(true);
        }

        public void Recycle(ProjectileBase p)
        {
            projectilePool.Recycle(p);
        }
    }
    public struct ProjectileInfo
    {
        public float speed;
        public float lifeTime;
        public int damage;
    }
}