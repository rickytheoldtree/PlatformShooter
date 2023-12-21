using System;
using UnityEngine;

namespace RicKit.MakeEditorTools
{
    public class CamCtrl : MonoBehaviour
    {
        public Vector2 sizeRange = new Vector2(1, 10);
        private float size;
        private Camera cam;

        private void Awake()
        {
            cam = GetComponent<Camera>();
            size = cam.orthographicSize;
            size = Mathf.Clamp(size, sizeRange.x, sizeRange.y);
        }

        private void Update()
        {
            if(Input.GetKey(KeyCode.LeftControl)) return;
            //如果滚轮向前滚动
            var mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");
            if (Math.Abs(mouseScrollWheel) > 0.01f)
            {
                //如果滚轮向前滚动
                if (mouseScrollWheel > 0)
                {
                    //放大
                    size -= 0.5f;
                }
                else
                {
                    //缩小
                    size += 0.5f;
                }
                //限制size的值
                size = Mathf.Clamp(size, sizeRange.x, sizeRange.y);
                //设置相机的正交投影大小
                cam.orthographicSize = size;
            }
        }
    }
}