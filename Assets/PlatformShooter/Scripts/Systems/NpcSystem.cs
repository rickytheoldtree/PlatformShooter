using System.Collections.Generic;
using PlatformShooter.Character;
using RicKit.Common;
using RicKit.Pool;
using UnityEngine;

namespace PlatformShooter.Systems
{
    public class NpcSystem : MonoSingleton<NpcSystem>
    {
        private SimpleNpc npcPrefab;
        private MonoPool<SimpleNpc> npcPool;
        protected override void GetAwake()
        {
            npcPrefab = Resources.Load<SimpleNpc>("Prefabs/Npc");
            npcPool = new MonoPool<SimpleNpc>(() =>
            {
                var npc = Instantiate(npcPrefab, transform);
                npc.gameObject.SetActive(false);
                return npc;
            }, p => p.OnRecycle());
        }
        public void SpawnNpc(Vector3 position)
        {
            var npc = npcPool.Allocate();
            npc.transform.position = position;
            npc.gameObject.SetActive(true);
            npc.Init();
        }

        public void Clear()
        {
            var npcList = FindObjectsOfType<SimpleNpc>();
            foreach (var npc in npcList)
            {
                npcPool.Recycle(npc);
            }
        }
    }
}