﻿using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace RicKit.UI
{
    [RequireComponent(typeof(Canvas), typeof(CanvasGroup), typeof(GraphicRaycaster))]
    public abstract class AbstractUIPanel : MonoBehaviour
    {
        public int SortOrder { get; private set; }
        private bool IsNull => !this;
        public bool IsShow =>  gameObject.activeSelf;

        public bool CanInteract => IsShow && CanvasGroup.interactable;
        protected CanvasGroup CanvasGroup { get; private set; }
        protected RectTransform CanvasRect { get; private set; }
        private Canvas Canvas { get; set; }
        public virtual bool DontDestroyOnClear => false;
        public virtual bool ShowHideSound => true;
        protected static UIManager UI => UIManager.I;
        protected virtual void Awake()
        {
            Canvas = GetComponent<Canvas>();
            Canvas.overrideSorting = true;
            Canvas.sortingLayerName = "UI";
            CanvasGroup = GetComponent<CanvasGroup>();
            CanvasRect = Canvas.GetComponent<RectTransform>();
        }
        public async UniTask OnShowAsync()
        {
            UI.LockInput(true);
            gameObject.SetActive(true);
            CanvasGroup.blocksRaycasts = true;
            CanvasGroup.interactable = false;
            OnAnimationIn();
            await UniTask.WaitUntil(() => IsNull || CanInteract);
            UI.LockInput(false);
        }
        public async UniTask OnHideAsync()
        {
            UI.LockInput(true);
            CanvasGroup.blocksRaycasts = true;
            CanvasGroup.interactable = false;
            OnAnimationOut();
            await UniTask.WaitUntil(() => IsNull || !IsShow);
            UI.LockInput(false);
        }

        public void UpdateData<T>(Action<T> onUpdateData) where T : AbstractUIPanel
        {
            onUpdateData?.Invoke((T)this);
        }

        public virtual void BeforeShow()
        {
        }

        public abstract void OnESCClick();
        protected abstract void OnAnimationIn();

        protected abstract void OnAnimationOut();

        protected virtual void OnAnimationInEnd()
        {
            CanvasGroup.blocksRaycasts = true;
            CanvasGroup.interactable = true;
        }

        protected virtual void OnAnimationOutEnd()
        {
            gameObject.SetActive(false);
        }

        public void SetSortOrder(int order)
        {
            SortOrder = order;
            Canvas.overrideSorting = true;
            Canvas.sortingOrder = order;
        }


    }
}