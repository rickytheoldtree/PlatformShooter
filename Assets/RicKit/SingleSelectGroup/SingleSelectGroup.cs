using System.Collections.Generic;
using UnityEngine;

namespace RicKit.SingleSelectGroup
{
    public class SingleSelectGroup : MonoBehaviour
    {
        private SSGButton selectedButton;
        private List<SSGButton> Items => items ??= new List<SSGButton>(GetComponentsInChildren<SSGButton>());
        private List<SSGButton> items;
        
        public void Select(int index)
        {
            Select(Items[index]);
        }
        public void Select(SSGButton button)
        {
            if (selectedButton)
            {
                selectedButton.Select(false);
            }
            selectedButton = button;
            selectedButton.Select(true);
        }
    }
}