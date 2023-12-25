using PlatformShooter.UI.UIPanels;
using RicKit.UI;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    private void Start()
    {
        UIManager.I.ShowUI<UIStart>();
    }
}
