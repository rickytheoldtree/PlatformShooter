using UnityEditor;

namespace RicKit.Tools.Localization
{
    [CustomEditor(typeof(LocalizationPackage),true)]
    public class DefaultPacakgeEditor : LocalizationPackageAbstractEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}

