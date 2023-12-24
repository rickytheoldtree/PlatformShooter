using PlatformShooter.Character;
using PlatformShooter.Systems;
using UnityEngine;

namespace PlatformShooter.Weapon
{
    public interface IWeapon
    {
        void Fire(CharacterBase user,  Vector2 dir);
    }
}