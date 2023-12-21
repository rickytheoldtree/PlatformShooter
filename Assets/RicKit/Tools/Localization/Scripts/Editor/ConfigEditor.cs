using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RicKit.Tools.Localization.JsonConverter;
using UnityEditor;
using UnityEngine;

namespace RicKit.Tools.Localization
{
    [CustomEditor(typeof(Config),true)]
    public class ConfigEditor : Editor
    {
        private SerializedProperty supportLanguages;
        private Config config;
        public void OnEnable()
        {
            supportLanguages = serializedObject.FindProperty("supportedLanguages");
            config = (Config)target;
            driverIndex = (int)config.webDriver;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(supportLanguages, true);
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("添加所有语言枚举"))
                {
                    var languages = Enum.GetValues(typeof(LanguageEnum));
                    for (var i = 0; i < languages.Length; i++)
                    {
                        supportLanguages.arraySize++;
                        supportLanguages.GetArrayElementAtIndex(supportLanguages.arraySize - 1).enumValueIndex = i;
                    }
                }
                if (GUILayout.Button("删除所有语言枚举"))
                {
                    supportLanguages.ClearArray();
                }
            }
            EditorGUILayout.Separator();
            ChooseJsonConverter();
            ChooseWebDriver();
            serializedObject.ApplyModifiedProperties();
        }
        private List<IJsonConverter> converters;
        private int converterIndex = -1;
        private int driverIndex;
        private void ChooseJsonConverter()
        {
            if (converters == null)
            {
                converters = LocalizationEditorUtils.GetJsonConverters();
                converterIndex = converters.FindIndex(c => c.GetType() == config.CurrentJsonConverter.GetType());
                if (converterIndex == -1)
                {
                    converterIndex = 0;
                }
            }
            EditorGUI.BeginChangeCheck();
            converterIndex = EditorGUILayout.Popup("JsonConverter", converterIndex, converters.Select(c => c.GetType().Name).ToArray());
            if (EditorGUI.EndChangeCheck())
            {
                config.currentJsonConverterName = converters[converterIndex].GetType().FullName;
                EditorUtility.SetDirty(config);
                AssetDatabase.SaveAssets();
            }
            
        }

        private void ChooseWebDriver()
        {
            EditorGUI.BeginChangeCheck();
            driverIndex = EditorGUILayout.Popup("WebDriver", driverIndex, Enum.GetNames(typeof(Config.WebDriverType)));
            if (EditorGUI.EndChangeCheck())
            {
                config.webDriver = (Config.WebDriverType)driverIndex;
                EditorUtility.SetDirty(config);
                AssetDatabase.SaveAssets();
            }
        }
    }
}

