using PlatformShooter.Character;
using UnityEngine;

namespace PlatformShooter.Event
{
    public class NpcChangeDirectionArea : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.TryGetComponent<SimpleNpc>(out var npc))
            {
                npc.ChangeDirection();
            }
        }
    }
}