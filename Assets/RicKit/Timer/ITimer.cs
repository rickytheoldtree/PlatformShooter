using System;

namespace RicKit.Timer
{
    public interface ITimer
    {
        TimerState State { get; }
        void OnRegister();
        void OnUnRegister();
        void Start();
        void Init(float time = 0);
        void Pause();
        void Resume();
        float Time { get; }
        void SetTime(float time);
        float TimeLimit { get; }
        void SetTimeLimit(float timeLimit);
        void SetTickInterval(float tickInterval);
        void RemoveAllListeners();
        void Update();
        void ForceTick();
    }
}