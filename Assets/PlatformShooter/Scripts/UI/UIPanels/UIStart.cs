using PlatformShooter.Controller;
using PlatformShooter.Systems;
using RicKit.UI;
using UnityEngine;
using UnityEngine.UI;

namespace PlatformShooter.UI.UIPanels
{
    public class UIStart : DoTweenUIPanel
    {
        [SerializeField] Button mStartButton;
        private StageObserver mStageObserver;
        protected override void Awake()
        {
            base.Awake();
            mStartButton.onClick.AddListener(OnStartButtonClick);
        }

        protected override void OnAnimationIn()
        {
            base.OnAnimationIn();
            GameSystem.I.SpawnNpcs();
            mStageObserver = new GameObject("StageObserver").AddComponent<StageObserver>();
            mStageObserver.StartMove(new Vector2(0, 3), new Vector2(50, 3), new Vector2(67, 14));
        }

        protected override void OnAnimationOutEnd()
        {
            base.OnAnimationOutEnd();
            Destroy(mStageObserver.gameObject);
        }

        private void OnStartButtonClick()
        {
            UIManager.I.BackThenShow<UIGame>();
            GameSystem.I.InitGame();
        }

        public override void OnESCClick()
        {
            
        }
    }
}