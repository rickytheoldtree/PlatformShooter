using System;
using System.Collections.Generic;
using RicKit.Common;
using UnityEngine;

namespace RicKit.Timer
{
    public class TimerSystem : MonoSingleton<TimerSystem>
    {
        private readonly Dictionary<string, ITimer> timers = new Dictionary<string, ITimer>();
        private Action update;
        protected override void GetAwake()
        {
            
        }

        private void Update()
        {
            update?.Invoke();
        }

        public static T GetTimer<T>(string name) where T : ITimer, new()
        {
            if (I.timers.TryGetValue(name, out var t))
            {
                return (T)t;
            }
            t = new T();
            RegisterTimer(t, name);
            return (T)t;
        }
        public static void RegisterTimer<T>(T timer, string name) where T : ITimer
        {
            if (I.timers.ContainsKey(name))
            {
                Debug.Log($"{name} already exists");
                return;
            }
            timer.OnRegister();
            I.update += timer.Update;
            I.timers.Add(name, timer);
        }
        public static void UnregisterTimer(string name)
        {
            if (!I.timers.TryGetValue(name, out var timer)) return;
            timer.OnUnRegister();
            I.update -= timer.Update;
            I.timers.Remove(name);
        }
    }
}