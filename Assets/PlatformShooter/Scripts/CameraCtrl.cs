using PlatformShooter.Config;
using PlatformShooter.Systems;
using UnityEngine;

namespace PlatformShooter
{
    [RequireComponent(typeof(Camera))]
    public class CameraCtrl : MonoBehaviour
    {
        private Camera cam;
        private Transform[] targets;
        private void Awake()
        {
            cam = GetComponent<Camera>();
            GameSystem.I.RegisterCamera(this);
        }

        private void FixedUpdate()
        {
            var camPos = transform.position;
            var targetPos = Vector3.zero;
            if(targets == null || targets.Length == 0) return;
            foreach (var target in targets)
            {
                targetPos += target.position;
            }
            targetPos /= targets.Length;
            targetPos.z = camPos.z;
            var distance = Vector3.Distance(camPos, targetPos);
            var move = (targetPos - camPos).normalized * distance;
            transform.position += move * (Time.deltaTime * 5);
        }

        public void SetTargets(params Transform[] transforms)
        {
            targets = transforms;
        }

        public void Shake(Vector2 vector2)
        {
            if(!ConfigHelper.IsScreenShakeEnabled.Value) return;
            transform.position += (Vector3)vector2;
        }
    }
}