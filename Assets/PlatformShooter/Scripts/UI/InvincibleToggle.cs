using PlatformShooter.Config;
using UnityEngine;
using UnityEngine.UI;

namespace PlatformShooter.UI
{
    public class InvincibleToggle : MonoBehaviour
    {
        private Toggle mToggle;

        private void Awake()
        {
            mToggle = GetComponent<Toggle>();
            mToggle.isOn = ConfigHelper.IsInvincible.Value;
            mToggle.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(bool arg0)
        {
            ConfigHelper.IsInvincible.Value = arg0;
        }
    }
}