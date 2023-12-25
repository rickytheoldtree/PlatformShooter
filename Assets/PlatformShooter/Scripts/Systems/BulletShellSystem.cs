using System.Collections.Generic;
using RicKit.Common;
using RicKit.Pool;
using UnityEngine;

namespace PlatformShooter.Systems
{
    public class BulletShellSystem : MonoSingleton<BulletShellSystem>
    {
        private GameObject bulletShellPrefab;
        private MonoPool<GameObject> bulletShellPool;
        private const int MaxBulletShellCount = 100;
        private readonly Queue<GameObject> bulletShells = new Queue<GameObject>();
        protected override void GetAwake()
        {
            bulletShellPrefab = Resources.Load<GameObject>("Prefabs/BulletShell");
            bulletShellPool = new MonoPool<GameObject>(() =>
            {
                var bulletShell = Instantiate(bulletShellPrefab, transform);
                bulletShell.SetActive(false);
                return bulletShell;
            }, p =>
            {
                p.SetActive(false);
            });
        }
        public void LaunchBulletShell(Vector3 position, Vector2 dir)
        {
            var bulletShell = bulletShells.Count >= MaxBulletShellCount ? bulletShells.Dequeue() : bulletShellPool.Allocate();
            bulletShells.Enqueue(bulletShell);
            bulletShell.transform.position = position;
            bulletShell.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            bulletShell.SetActive(true);
            if (!bulletShell.TryGetComponent<Rigidbody2D>(out var body)) return;
            body.velocity = dir * Random.Range(3f, 5f);
            body.angularVelocity = Random.Range(-360f, 360f);
        }
        public void Clear()
        {
            while (bulletShells.Count > 0)
            {
                bulletShellPool.Recycle(bulletShells.Dequeue());
            }
        }
    }
}