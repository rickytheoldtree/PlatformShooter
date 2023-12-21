using System;
using System.Diagnostics;
using Microsoft.Win32;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace RicKit.EditorTools
{
    public static class PlayerPrefTools
    {
        [MenuItem("RicKit/Player Pref/Clear Player Pref")]
        public static void Clear()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        [MenuItem("RicKit/Player Pref/Open Player Pref")]
        public static void OpenPlayerPref()
        {
            var registryLocation =
                @$"HKEY_CURRENT_USER\Software\Unity\UnityEditor\{Application.companyName}\{Application.productName}";
            var registryLastKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Applets\Regedit";
            try
            {
                Registry.SetValue(registryLastKey, "LastKey",
                    registryLocation); // Set LastKey value that regedit will go directly to
                Process.Start("regedit.exe");
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }
    }
}