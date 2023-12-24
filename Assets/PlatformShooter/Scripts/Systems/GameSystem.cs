using System;
using PlatformShooter.Character;
using PlatformShooter.ScriptableObjects;
using RicKit.Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PlatformShooter.Systems
{
    public class GameSystem : MonoSingleton<GameSystem>
    {
        private NpcPosConfig npcPosConfig;
        private PlayerController Player { get; set; }
        public CameraCtrl CameraCtrl { get; private set; }
        public EventSystem EventSystem { get; private set; }
        private readonly Vector2 playerInitPos = new Vector2(0, 2);
        protected override void GetAwake()
        {
            npcPosConfig = Resources.Load<NpcPosConfig>("SO/NpcPosConfig");
            Player = Instantiate(Resources.Load<GameObject>("Prefabs/Player")).GetComponent<PlayerController>();
            Player.transform.position = playerInitPos;
            EventSystem = EventSystem.current;
        }
        public void StartGame()
        {
            NpcSystem.I.Clear();
            foreach (var pos in npcPosConfig.list)
            {
                NpcSystem.I.SpawnNpc(pos);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }
        }

        public void RestartGame()
        {
            NpcSystem.I.Clear();
            Player.Init();
            Player.transform.position = playerInitPos;
        }

        public void RegisterCamera(CameraCtrl cameraCtrl)
        {
            CameraCtrl = cameraCtrl;
            CameraCtrl.SetTargets(new[] {Player.transform, Player.cameraTarget});
        }
    }
}