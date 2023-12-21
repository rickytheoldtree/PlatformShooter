using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RicKit.MakeEditorTools
{
    public class ClearTrigger : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
    {
        public Action onClear;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Input.GetMouseButton(1))
            {
                ClearSystem.I.Clear(this);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                ClearSystem.I.Clear(this);
            }
        }
    }
}