using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RicKit.UI
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;

        public static UIManager I
        {
            get
            {
                if (!instance)
                    Instantiate(Resources.Load<GameObject>("UIManager")).TryGetComponent(out instance);
                return instance;
            }
        }

        [SerializeField] private CanvasGroup blockerCg;
        public Transform defaultRoot;
        private readonly IPanelLoader panelLoader = new DefaultPanelLoader();
        private readonly Stack<AbstractUIPanel> showFormStack = new Stack<AbstractUIPanel>();
        private readonly List<AbstractUIPanel> uiFormsList = new List<AbstractUIPanel>();
        private Canvas canvas;
        private AbstractUIPanel CurrentAbstractUIPanel { get; set; }
        public RectTransform RT { get; private set; }
        public Camera UICamera { get; private set; }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && CurrentAbstractUIPanel && CurrentAbstractUIPanel.CanInteract)
                CurrentAbstractUIPanel.OnESCClick();
        }

        protected void Awake()
        {
            UICamera = GetComponentInChildren<Camera>();
            canvas = GetComponentInChildren<Canvas>();
            RT = canvas.GetComponent<RectTransform>();
            DontDestroyOnLoad(gameObject);
            var eventSystem = FindObjectOfType<EventSystem>();
            eventSystem = eventSystem
                ? eventSystem
                : new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule))
                    .GetComponent<EventSystem>();
            DontDestroyOnLoad(eventSystem);
        }

        #region 同步

        public void ShowUI<T>(Action<T> onInit = null) where T : AbstractUIPanel
        {
            ShowUIAsync(onInit).Forget();
        }

        public void HideThenShowUI<T>(Action<T> onInit = null) where T : AbstractUIPanel
        {
            HideThenShowUIAsync(onInit).Forget();
        }

        public void CloseThenShowUI<T>(Action<T> onInit = null) where T : AbstractUIPanel
        {
            CloseThenShowUIAsync(onInit).Forget();
        }

        public void ShowUIUnmanagable<T>(Action<T> onInit = null) where T : AbstractUIPanel
        {
            ShowUIUnmanagableAsync(onInit).Forget();
        }
        public void Back()
        {
            BackAsync().Forget();
        }

        public void CloseCurrent()
        {
            CloseCurrentAsync().Forget();
        }

        public void HideCurrent()
        {
            HideCurrentAsync().Forget();
        }

        public void HideUntil<T>() where T : AbstractUIPanel
        {
            HideUntilAsync<T>().Forget();
        }

        public void BackThenShow<T>(Action<T> onInit = null) where T : AbstractUIPanel
        {
            BackThenShowAsync(onInit).Forget();
        }

        #endregion


        #region 异步

        public async UniTask ShowUIAsync<T>(Action<T> onInit = null) where T : AbstractUIPanel
        {
            var sortOrder = showFormStack.Count == 0 ? 1 : showFormStack.Peek().SortOrder + 5;
            var form = GetUI<T>();
            if (!form)
                form = NewUI<T>();
            form.gameObject.SetActive(false);
            onInit?.Invoke(form);
            form.SetSortOrder(sortOrder);
            showFormStack.Push(form);
            if (!uiFormsList.Contains(form))
                uiFormsList.Add(form);
            CurrentAbstractUIPanel = form;
            form.BeforeShow();
            await form.OnShowAsync();
        }


        public async UniTask HideThenShowUIAsync<T>(Action<T> onInit = null) where T : AbstractUIPanel
        {
            await HideCurrentAsync();
            await ShowUIAsync(onInit);
        }

        public async UniTask CloseThenShowUIAsync<T>(Action<T> onInit = null) where T : AbstractUIPanel
        {
            await CloseCurrentAsync();
            await ShowUIAsync(onInit);
        }

        public async UniTask ShowUIUnmanagableAsync<T>(Action<T> onInit = null) where T : AbstractUIPanel
        {
            const int sortOrder = 900;
            var form = GetUI<T>();
            if (!form)
                form = NewUI<T>();
            form.gameObject.SetActive(false);
            onInit?.Invoke(form);
            form.SetSortOrder(sortOrder);
            if (!uiFormsList.Contains(form)) uiFormsList.Add(form);
            form.BeforeShow();
            await form.OnShowAsync();
        }
        public async UniTask BackAsync()
        {
            await CloseCurrentAsync();
            if (showFormStack.Count == 0) return;
            var form = showFormStack.Peek();
            CurrentAbstractUIPanel = form;
            if (!form.IsShow)
            {
                await form.OnShowAsync();
            }
        }


        public async UniTask CloseCurrentAsync()
        {
            if (showFormStack.Count == 0) return;
            var form = showFormStack.Pop();
            CurrentAbstractUIPanel = null;
            await form.OnHideAsync();
        }


        public async UniTask HideCurrentAsync()
        {
            if (showFormStack.Count == 0) return;
            var form = showFormStack.Peek();
            await form.OnHideAsync();
        }

        private async UniTask HideUntilAsync<T>() where T : AbstractUIPanel
        {
            while (showFormStack.Count > 0)
            {
                var form = showFormStack.Peek();
                if (form is T)
                {
                    if (!form.isActiveAndEnabled)
                        await form.OnShowAsync();
                    CurrentAbstractUIPanel = form;
                    return;
                }

                CloseCurrentAsync().Forget();
            }
        }

        public async UniTask BackThenShowAsync<T>(Action<T> onInit) where T : AbstractUIPanel
        {
            await BackAsync();
            await ShowUIAsync(onInit);
        }

        #endregion


        #region LockScreen

        private int lockCount;

        public void LockInput(bool on)
        {
            lockCount += on ? 1 : -1;
            lockCount = lockCount < 0 ? 0 : lockCount;
#if UNITY_EDITOR
            blockerCg.name = $"blockerCg {lockCount}";
#endif
            blockerCg.blocksRaycasts = lockCount > 0;
        }

        #endregion


        public T GetUI<T>() where T : AbstractUIPanel
        {
            return uiFormsList.Where(form => form is T).Cast<T>().FirstOrDefault();
        }

        private T NewUI<T>() where T : AbstractUIPanel
        {
            var go = Instantiate(panelLoader.LoadPrefab(typeof(T).Name), defaultRoot);
            go.TryGetComponent(out T form);
            return form;
        }

        public void ClearAll()
        {
            foreach (var uIForm in uiFormsList.Where(uIForm => !uIForm.DontDestroyOnClear))
                Destroy(uIForm.gameObject);
            uiFormsList.Clear();
            showFormStack.Clear();
            StopAllCoroutines();
            CurrentAbstractUIPanel = null;
        }
    }

    public interface IPanelLoader
    {
        GameObject LoadPrefab(string name);
    }

    public class DefaultPanelLoader : IPanelLoader
    {
        private const string PrefabPath = "UIPanels/";

        public GameObject LoadPrefab(string name)
        {
            return Resources.Load<GameObject>(PrefabPath + name);
        }
    }
}