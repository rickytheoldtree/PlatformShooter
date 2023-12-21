using UnityEngine;
using UnityEngine.UI;

namespace RicKit.DebugMode
{
    public class DebugCtrl : MonoBehaviour
    {
        [SerializeField] 
        private CanvasGroup panelCg;
        [SerializeField]
        private Button btn;
        private bool isShow;

        protected virtual void Awake()
        {
            btn.onClick.AddListener(OnClick);
            panelCg.alpha = 0;
            panelCg.blocksRaycasts = false;
        }

        private void OnClick()
        {
            isShow = !isShow;
            panelCg.alpha = isShow ? 1 : 0;
            panelCg.blocksRaycasts = isShow;
        }
    }
}

