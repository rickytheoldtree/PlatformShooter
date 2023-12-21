using UnityEngine;

namespace RicKit.MakeEditorTools
{
    public class GridDrawer : MonoBehaviour
    {
        public float intervalX, intervalY;
        public int row, col;
        public float lineWidth = 0.1f;
        [SerializeField]
        private LineRenderer line;
        
        private void Awake()
        {
            for (var i = 0; i < col; i++)
            {
                var l = Instantiate(line, transform);
                l.transform.localPosition = Vector3.zero;
                l.positionCount = 2;
                l.startWidth = l.endWidth = lineWidth;
                l.startColor = l.endColor = i == col / 2 ? Color.red : Color.white;
                l.SetPosition(0, new Vector3(intervalX * (i - (col - 1) * 0.5f),  (row - 1) * intervalY * 0.5f, 0));
                l.SetPosition(1, new Vector3(intervalX * (i - (col - 1) * 0.5f), -(row - 1) * intervalY * 0.5f, 0));
            }
            for (var i = 0; i < row; i++)
            {
                var l = Instantiate(line, transform);
                l.transform.localPosition = Vector3.zero;
                l.positionCount = 2;
                l.startWidth = l.endWidth = lineWidth;
                l.startColor = l.endColor = i == row / 2 ? Color.red : Color.white;
                l.SetPosition(0, new Vector3(-(col - 1) * intervalX * 0.5f, intervalY * (i - (row - 1) * 0.5f), 0));
                l.SetPosition(1, new Vector3( (col - 1) * intervalX * 0.5f, intervalY * (i - (row - 1) * 0.5f), 0));
            }
        }
    }
}

