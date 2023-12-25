using RicKit.Common;
using UnityEngine;

namespace PlatformShooter.Systems
{
    public class ParticleSystem : MonoSingleton<ParticleSystem>
    {
        UnityEngine.ParticleSystem psHitEffect;
        protected override void GetAwake()
        {
            psHitEffect = Instantiate(Resources.Load<UnityEngine.ParticleSystem>("Particles/HitEffect"),transform);
        }
        public void ShowHitEffect(Vector3 position)
        {
            psHitEffect.transform.position = position;
            psHitEffect.Play();
        }
    }
}