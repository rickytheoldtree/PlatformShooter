using PlatformShooter.Config;
using UnityEngine;
using UnityEngine.UI;

namespace PlatformShooter.UI.UIComponents
{
    public class ScreenShakeToggle : MonoBehaviour
    {
        private Toggle mToggle;

        private void Awake()
        {
            mToggle = GetComponent<Toggle>();
            mToggle.isOn = ConfigHelper.IsScreenShakeEnabled.Value;
            mToggle.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(bool arg0)
        {
            ConfigHelper.IsScreenShakeEnabled.Value = arg0;
        }
    }
}