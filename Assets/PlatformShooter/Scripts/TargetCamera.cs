using UnityEngine;

namespace PlatformShooter
{
    [RequireComponent(typeof(Camera))]
    public class TargetCamera : MonoBehaviour
    {
        private Camera cam;
        [SerializeField]
        private Transform[] targets;
        private void Awake()
        {
            cam = GetComponent<Camera>();
        }

        private void FixedUpdate()
        {
            var camPos = transform.position;
            var targetPos = Vector3.zero;
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
    }
}