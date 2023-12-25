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

        private PlayerController Player
        {
            get
            {
                if (!mPlayer)
                {
                    mPlayer = Instantiate(Resources.Load<GameObject>("Prefabs/Player")).GetComponent<PlayerController>();
                }

                return mPlayer;
            }
        }

        private PlayerController mPlayer;
        public CameraCtrl CameraCtrl { get; private set; }
        public EventSystem EventSystem { get; private set; }
        private readonly Vector2 playerInitPos = new Vector2(0, 2);
        protected override void GetAwake()
        {
            Application.targetFrameRate = 60;
            npcPosConfig = Resources.Load<NpcPosConfig>("SO/NpcPosConfig");
            EventSystem = EventSystem.current;
        }
        public void SpawnNpcs()
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
                InitGame();
            }
        }

        public void InitGame()
        {
            NpcSystem.I.Clear();
            BulletShellSystem.I.Clear();
            Player.Init();
            Player.transform.position = playerInitPos;
            CameraCtrl.SetTargets(new []{Player.transform, Player.cameraTarget});
        }

        public void RegisterCamera(CameraCtrl cameraCtrl)
        {
            CameraCtrl = cameraCtrl;
        }
    }
}