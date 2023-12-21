using System;

namespace RicKit.Timer
{
    public static class TimerUtils
    {
        public static string FormatTime(float time,bool showMilliseconds = false)
        {
            var timeSpan = TimeSpan.FromSeconds(time);
            if (showMilliseconds)
            {
                return $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}:{timeSpan.Milliseconds:D3}";
            }
            return $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        }
    }
}