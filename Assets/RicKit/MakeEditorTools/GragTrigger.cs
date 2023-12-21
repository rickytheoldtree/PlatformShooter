using UnityEngine;

namespace RicKit.MakeEditorTools
{
    public class GragTrigger : MonoBehaviour
    {
        private void OnMouseDown()
        {
            GragSystem.I.RegisterTransform(transform);
        }
    }
}
