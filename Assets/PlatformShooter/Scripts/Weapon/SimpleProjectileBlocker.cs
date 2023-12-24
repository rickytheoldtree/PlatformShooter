using UnityEngine;

namespace PlatformShooter.Weapon
{
    public class SimpleProjectileBlocker : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.TryGetComponent<ProjectileBase>(out var projectile))
            {
                projectile.OnImpact();
            }
        }
    }
}