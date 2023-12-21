using System.Collections.Generic;
using RicKit.Common;
using UnityEngine;

namespace RicKit.MakeEditorTools
{
    public class GragSystem : MonoSingleton<GragSystem>
    {
        private readonly List<Vector2> points = new List<Vector2>();
        private Transform selectTransform;
        private Camera cam;
        protected override void GetAwake()
        {
            cam = Camera.main;
        }
        public void SetPoints(params Vector2[] points)
        {
            this.points.AddRange(points);
        }
        public void RegisterTransform(Transform transform)
        {
            selectTransform = transform;
        }
        private void Update()
        {
            if (Input.GetMouseButton(0) && selectTransform)
            {
                var mousePosition = Input.mousePosition;
                var worldPosition = cam.ScreenToWorldPoint(mousePosition);
                var position = new Vector2(worldPosition.x, worldPosition.y);
                if (points.Count > 0)
                {
                    var minDistance = float.MaxValue;
                    var minIndex = -1;
                    for (var i = 0; i < points.Count; i++)
                    {
                        var distance = Vector2.Distance(position, points[i]);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            minIndex = i;
                        }
                    }
                    if (minDistance < 0.5f)
                    {
                        selectTransform.position = points[minIndex];
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                selectTransform = null;
            }
        }
    }
}