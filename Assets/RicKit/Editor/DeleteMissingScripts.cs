using UnityEditor;
using UnityEngine;

namespace RicKit.EditorTools
{
    public static class DeleteMissingScripts
    {
        [MenuItem("RicKit/Others/Delete Missing Scripts")]
        private static void CleanupMissingScript()
        {
            foreach (var o in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
            {
                var gameObject = (GameObject)o;
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);
            }
        }
    }
}