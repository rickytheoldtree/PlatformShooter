using System;
using UnityEngine;
using UnityEngine.Events;

namespace RicKit.Common
{
    public class GlobalUpdater : MonoBehaviour
    {
        private static GlobalUpdater instance;
        private readonly UnityEvent mOnUpdate = new UnityEvent();

        private static GlobalUpdater I
        {
            get
            {
                if (!instance)
                {
                    instance = FindObjectOfType<GlobalUpdater>();
                    if (!instance)
                    {
                        var go = new GameObject(nameof(GlobalUpdater));
                        instance = go.AddComponent<GlobalUpdater>();
                        DontDestroyOnLoad(go);
                    }
                }

                return instance;
            }
        }

        private void Update()
        {
            mOnUpdate.Invoke();
        }

        public static void Register(Action action)
        {
            I.mOnUpdate.AddListener(action.Invoke);
        }

        public static void Unregister(Action action)
        {
            I.mOnUpdate.RemoveListener(action.Invoke);
        }
    }
}