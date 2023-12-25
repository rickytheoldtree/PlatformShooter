using UnityEngine;

namespace PlatformShooter.Character
{
    public class HitBox : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.layer = LayerMask.NameToLayer("HitBox");
            if (TryGetComponent(out Collider2D c))
            {
                c.isTrigger = true;
            }
        }

        public CharacterBase owner;
    }
}