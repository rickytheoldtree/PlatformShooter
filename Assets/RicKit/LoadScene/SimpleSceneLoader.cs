using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RicKit.LoadScene
{
    public class SimpleSceneLoader
    {
        private readonly AsyncOperation handler;
        private readonly Action onCompleted;
        private readonly Action<float> onUpdate;

        private float fakeProgress;

        public SimpleSceneLoader(string sceneName, Action<float> onUpdate, Action onCompleted)
        {
            handler = SceneManager.LoadSceneAsync(sceneName);
            this.onCompleted = onCompleted;
            this.onUpdate = onUpdate;
        }

        public IEnumerator Loading()
        {
            fakeProgress = 0;
            while (fakeProgress < 0.998f || !handler.isDone)
            {
                fakeProgress = Mathf.Lerp(fakeProgress, handler.progress, Time.deltaTime * 10);
                onUpdate?.Invoke(fakeProgress);
                yield return null;
            }

            onUpdate?.Invoke(1);
            onCompleted?.Invoke();
        }
    }
}