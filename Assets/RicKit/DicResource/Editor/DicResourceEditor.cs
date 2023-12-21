using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace RicKit.DicResource
{
    public abstract class DicResourceEditor : Editor
    {
        private ReorderableList reorderableList;
        protected abstract string DictName { get; }
        private void OnEnable()
        {
            reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("dict"),
                true, true, true, true)
                {
                    drawHeaderCallback = (rect) =>
                    {
                        EditorGUI.LabelField(rect, DictName);
                    },
                    drawElementCallback = (rect, index, isActive, isFocused) =>
                    {
                        var element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
                        var key = element.FindPropertyRelative("key");
                        var value = element.FindPropertyRelative("value");
                        EditorGUI.PropertyField(new Rect(rect.x, rect.y + 2, rect.width / 2, EditorGUIUtility.singleLineHeight), key, GUIContent.none);
                        EditorGUI.PropertyField(new Rect(rect.x + rect.width / 2, rect.y + 2, rect.width / 2, EditorGUIUtility.singleLineHeight), value, GUIContent.none);
                    },
                    onAddCallback = (list) =>
                    {
                        var index = list.serializedProperty.arraySize;
                        list.serializedProperty.arraySize++;
                        list.index = index;
                    }
                };
        }
        public override void OnInspectorGUI()
        {
            reorderableList.DoLayoutList(); 
            serializedObject.ApplyModifiedProperties();
        }
    }
}
