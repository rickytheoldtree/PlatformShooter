using PlatformShooter.Character;
using PlatformShooter.Systems;
using UnityEngine;

namespace PlatformShooter.Area
{
    public class StartGameArea : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.TryGetComponent<PlayerController>(out var player) && player.FaceDirection == Vector2.right
               && other.transform.position.x < transform.position.x)
            {
                GameSystem.I.StartGame();
            }
        }
    }
}