using System;
using UnityEngine;

public class SimpleMono : MonoBehaviour
{
    private Action<SimpleMono> mOnUpdate,
        mOnFixedUpdate,
        mOnLateUpdate,
        mOnDestroy,
        mOnEnable,
        mOnDisable,
        mOnStart,
        mOnAwake;

    private void Awake()
    {
        mOnAwake?.Invoke(this);
    }

    private void Start()
    {
        mOnStart?.Invoke(this);
    }

    private void Update()
    {
        mOnUpdate?.Invoke(this);
    }

    private void FixedUpdate()
    {
        mOnFixedUpdate?.Invoke(this);
    }

    private void LateUpdate()
    {
        mOnLateUpdate?.Invoke(this);
    }

    private void OnEnable()
    {
        mOnEnable?.Invoke(this);
    }

    public SimpleMono OnEnable(Action<SimpleMono> onEnable)
    {
        mOnEnable = onEnable;
        return this;
    }

    private void OnDisable()
    {
        mOnDisable?.Invoke(this);
    }

    public SimpleMono OnDisable(Action<SimpleMono> onDisable)
    {
        mOnDisable = onDisable;
        return this;
    }

    private void OnDestroy()
    {
        mOnDestroy?.Invoke(this);
    }

    public SimpleMono OnDestroy(Action<SimpleMono> onDestroy)
    {
        mOnDestroy = onDestroy;
        return this;
    }

    public static SimpleMono Create(string name, bool dontDestroyOnLoad = true)
    {
        var go = new GameObject(name);
        if (dontDestroyOnLoad) DontDestroyOnLoad(go);
        return go.AddComponent<SimpleMono>();
    }

    public SimpleMono OnUpdate(Action<SimpleMono> onUpdate)
    {
        mOnUpdate = onUpdate;
        return this;
    }

    public SimpleMono OnFixedUpdate(Action<SimpleMono> onFixedUpdate)
    {
        mOnFixedUpdate = onFixedUpdate;
        return this;
    }

    public SimpleMono OnLateUpdate(Action<SimpleMono> onLateUpdate)
    {
        mOnLateUpdate = onLateUpdate;
        return this;
    }

    public SimpleMono OnStart(Action<SimpleMono> onStart)
    {
        mOnStart = onStart;
        return this;
    }

    public SimpleMono OnAwake(Action<SimpleMono> onAwake)
    {
        mOnAwake = onAwake;
        return this;
    }
}