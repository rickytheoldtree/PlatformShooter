using PlatformShooter.ScriptableObjects;
using RicKit.Common;
using UnityEngine;

namespace PlatformShooter.Systems
{
    public class GameSystem : Singleton<GameSystem>
    {
        private NpcPosConfig npcPosConfig = Resources.Load<NpcPosConfig>("SO/NpcPosConfig");

        public void StartGame()
        {
            NpcSystem.I.Clear();
            foreach (var pos in npcPosConfig.list)
            {
                NpcSystem.I.SpawnNpc(pos);
            }
        }
    }
}