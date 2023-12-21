using System;
using UnityEngine;
using UnityEngine.UI;

namespace RicKit.SingleSelectGroup
{
    [RequireComponent(typeof(Button))]
    public class SSGButton : MonoBehaviour
    {
        private Action<bool> onSelected;
        private Func<bool> canSelected;
        private SingleSelectGroup Ssg => ssg ? ssg : ssg = GetComponentInParent<SingleSelectGroup>();
        private SingleSelectGroup ssg;
        protected virtual void Awake()
        {
            var btn = GetComponentInChildren<Button>();
            btn.onClick.AddListener(() =>
            {
                if(canSelected!= null && canSelected?.Invoke() == false)
                    return;
                Ssg.Select(this);
            });
        }

        public void Init(Action<bool> onSelected, Func<bool> canSelected = null, bool selected = false)
        {
            this.onSelected = onSelected;
            this.canSelected = canSelected;
            Select(selected);
        }
        public virtual void Select(bool selected)
        {
            onSelected?.Invoke(selected);
        }
    }
}