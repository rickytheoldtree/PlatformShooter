using RicKit.Common;

namespace PlatformShooter.Config
{
    public static class ConfigHelper
    {
        public static EventProperty<bool> IsScreenShakeEnabled { get; } = new EventProperty<bool>(true);
        public static EventProperty<bool> IsInvincible { get; } = new EventProperty<bool>(false);
    }
}