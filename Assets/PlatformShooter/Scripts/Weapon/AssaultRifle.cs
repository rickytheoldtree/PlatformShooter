using PlatformShooter.Systems;

namespace PlatformShooter.Weapon
{
    public class AssaultRifle : RangeWeaponBase
    {

        private void Awake()
        {
            ProjectileInfo = new ProjectileInfo
            {
                speed = 30f,
                lifeTime = 1f,
                damage = 20,
            };
        }
    }
}