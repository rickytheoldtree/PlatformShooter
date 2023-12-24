using System;
using PlatformShooter.Character;
using PlatformShooter.Systems;
using UnityEngine;

namespace PlatformShooter.Event
{
    public class StartGameArea : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.TryGetComponent<PlayerController>(out var player) && player.FaceDirection == Vector2.right)
            {
                GameSystem.I.StartGame();
            }
        }
    }
}