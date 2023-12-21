using System;
using UnityEngine;

namespace RicKit.Timer
{
    public enum TimerState
    {
        InitialState,
        Running,
        Paused,
        Finished
    }

    public enum TimerEvent
    {
        Init,
        Start,
        Pause,
        Resume,
        Tick,
        Finish
    }

    public class DefaultTimer : ITimer
    {
        public TimerState State { get; private set; }
        public float TimeLeft => TimeLimit - Time;
        public virtual void OnRegister()
        {
        }

        public virtual void OnUnRegister()
        {
        }

        private float tickInterval;
        private float lastTickTime;
        private Action<ITimer, float> onTick, onStart, onInit, onPause, onResume, onFinish;

        public DefaultTimer()
        {
            TimeLimit = -1f;
            tickInterval = 1f;
        }

        public DefaultTimer(float timeLimit, float tickInterval)
        {
            TimeLimit = timeLimit;
            this.tickInterval = tickInterval;
        }

        public void SetTime(float time)
        {
            Time = time;
        }

        public float Time { get; private set; }

        public void Start()
        {
            Time = 0;
            State = TimerState.Running;
            lastTickTime = Time;
            onStart?.Invoke(this, Time);
        }

        public void Init(float time = 0)
        {
            State = TimerState.InitialState;
            Time = time;
            lastTickTime = time;
            onInit?.Invoke(this, time);
        }

        public void Pause()
        {
            if (State == TimerState.Running)
            {
                State = TimerState.Paused;
            }

            onPause?.Invoke(this, Time);
        }

        public void Resume()
        {
            if (State == TimerState.Paused)
                State = TimerState.Running;
            onResume?.Invoke(this, Time);
        }


        public void SetTimeLimit(float timeLimit)
        {
            TimeLimit = timeLimit;
        }

        public float TimeLimit { get; private set; }

        public void SetTickInterval(float tickInterval)
        {
            this.tickInterval = tickInterval;
        }
        public DefaultTimer AddListener(Action<ITimer, float> action, params TimerEvent[] events)
        {
            foreach (var e in events)
            {
                AddListener(action, e);
            }
            return this;
        }
        private void AddListener(Action<ITimer, float> action, TimerEvent e)
        {
            switch (e)
            {
                case TimerEvent.Start:
                    onStart += action;
                    break;
                case TimerEvent.Init:
                    onInit += action;
                    break;
                case TimerEvent.Pause:
                    onPause += action;
                    break;
                case TimerEvent.Resume:
                    onResume += action;
                    break;
                case TimerEvent.Tick:
                    onTick += action;
                    break;
                case TimerEvent.Finish:
                    onFinish += action;
                    break;
            }
        }

        public void RemoveListener(TimerEvent e, Action<ITimer, float> action)
        {
            switch (e)
            {
                case TimerEvent.Start:
                    onStart -= action;
                    break;
                case TimerEvent.Init:
                    onInit -= action;
                    break;
                case TimerEvent.Pause:
                    onPause -= action;
                    break;
                case TimerEvent.Resume:
                    onResume -= action;
                    break;
                case TimerEvent.Tick:
                    onTick -= action;
                    break;
                case TimerEvent.Finish:
                    onFinish -= action;
                    break;
            }
        }

        public void RemoveAllListeners()
        {
            onStart = null;
            onInit = null;
            onPause = null;
            onResume = null;
            onTick = null;
            onFinish = null;
        }

        public void Update()
        {
            if (State == TimerState.Running)
            {
                Time += UnityEngine.Time.deltaTime;
                if (tickInterval == 0)
                {
                    lastTickTime = Time;
                    onTick?.Invoke(this, Time);
                }
                else if (Time - lastTickTime >= tickInterval)
                {
                    lastTickTime = Time - Time % tickInterval;
                    onTick?.Invoke(this, Time);
                }

                if (TimeLimit > 0 && Time >= TimeLimit)
                {
                    State = TimerState.Finished;
                    onFinish?.Invoke(this, Time);
                }
            }
        }
        public void ForceTick()
        {
            onTick?.Invoke(this, Time);
        }
    }
}