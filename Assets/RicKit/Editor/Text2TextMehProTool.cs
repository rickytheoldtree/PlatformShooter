using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace RicKit.EditorTools
{
    public class Text2TextMeshProTool : EditorWindow
{
    private static List<string> scriptsFolders;

    private static EditorWindow myWindow;
    private static List<string> directoryPrefabs;
    private static List<string> allAssetPaths;
    private static TMP_FontAsset tmpFont;
    private readonly string[] replaceMode = { "批量替换", "单个替换" };
    private bool isAutoFixLinkedScripts;
    private string path;
    private int replaceModeIndex;
    private Vector2 scrollPos = Vector2.zero;
    private bool showPrefabs = true;

    private string Path
    {
        get => path;
        set
        {
            if (value != path)
            {
                if (directoryPrefabs == null)
                    directoryPrefabs = new List<string>();
                else
                    directoryPrefabs.Clear();
                if (allAssetPaths == null)
                    allAssetPaths = new List<string>();
                else
                    allAssetPaths.Clear();
                path = value;
            }
        }
    }

    private int ReplaceModeIndex
    {
        get => replaceModeIndex;
        set
        {
            if (value != replaceModeIndex)
            {
                if (directoryPrefabs == null)
                    directoryPrefabs = new List<string>();
                else
                    directoryPrefabs.Clear();
                if (allAssetPaths == null)
                    allAssetPaths = new List<string>();
                else
                    allAssetPaths.Clear();
                Path = "";
                replaceModeIndex = value;
            }
        }
    }

    private void OnGUI()
    {
        isAutoFixLinkedScripts = EditorGUILayout.BeginToggleGroup("修改关联脚本", isAutoFixLinkedScripts);
        ShowScriptsFolders();
        EditorGUILayout.EndToggleGroup();
        ReplaceModeIndex = GUILayout.Toolbar(ReplaceModeIndex, replaceMode);
        if (ReplaceModeIndex == 0)
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(myWindow.position.width),
                GUILayout.Height(myWindow.position.height - scriptsFolders.Count * 20 - 70));
            GetPath();
            ShowAllPrefabs();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.BeginHorizontal();
            LoadPrefab();
            Text2TextMeshPro();
            EditorGUILayout.EndHorizontal();
        }
        else if (ReplaceModeIndex == 1)
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(myWindow.position.width),
                GUILayout.Height(myWindow.position.height - scriptsFolders.Count * 20 - 70));
            GetPath(false);
            ShowAllPrefabs();
            EditorGUILayout.EndScrollView();
            Text2TextMeshPro();
        }
    }

    [MenuItem("RicKit/Text2TextMeshPro")]
    private static void Init()
    {
        myWindow = GetWindow(typeof(Text2TextMeshProTool));
        directoryPrefabs = new List<string>();
        allAssetPaths = new List<string>();
        scriptsFolders = new List<string> { "" };
        myWindow.minSize = new Vector2(500, 300);
    }

    private void GetPath(bool isFolder = true)
    {
        var e = Event.current;
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Path:", GUILayout.Width(40));
        Path = GUILayout.TextField(Path);
        EditorGUILayout.EndHorizontal();
        if (Event.current.type == EventType.DragExited || Event.current.type == EventType.DragUpdated)
        {
            if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                if (Event.current.type == EventType.DragExited)
                {
                    DragAndDrop.AcceptDrag();
                    if (!isFolder)
                    {
                        if (DragAndDrop.objectReferences != null && DragAndDrop.objectReferences.Length > 0)
                        {
                            var objectReferences = DragAndDrop.objectReferences;
                            for (var i = 0; i < objectReferences.Length; i++)
                            {
                                var index = i;
                                if (AssetDatabase.GetAssetPath(objectReferences[index]).EndsWith(".prefab"))
                                    if (!directoryPrefabs.Contains(AssetDatabase.GetAssetPath(objectReferences[index])))
                                        directoryPrefabs.Add(AssetDatabase.GetAssetPath(objectReferences[index]));
                            }
                        }
                    }
                    else
                    {
                        if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
                        {
                            if (File.Exists(DragAndDrop.paths[0]))
                            {
                                EditorUtility.DisplayDialog("警告", "批量模式下请拖拽文件夹！", "确定");
                                return;
                            }

                            Path = DragAndDrop.paths[0];
                        }
                    }
                }

                e.Use();
            }
            else
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
            }
        }
    }

    private void ShowAllPrefabs()
    {
        if (directoryPrefabs != null && directoryPrefabs.Count > 0)
        {
            showPrefabs = EditorGUILayout.Foldout(showPrefabs, "显示预制体");
            if (showPrefabs)
            {
                for (var i = 0; i < directoryPrefabs.Count; i++)
                {
                    var index = i;
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.SelectableLabel($"预制体路径：{directoryPrefabs[index]}");
                    if (GUILayout.Button("查看", GUILayout.Width(60)))
                    {
                        EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Object>(directoryPrefabs[index]));
                        Selection.activeGameObject =
                            AssetDatabase.LoadAssetAtPath<Object>(directoryPrefabs[index]) as GameObject;
                    }

                    if (GUILayout.Button("删除", GUILayout.Width(60))) directoryPrefabs.RemoveAt(index);
                    EditorGUILayout.EndHorizontal();
                }
            }
            else
            {
                if (GUILayout.Button("清空选择", GUILayout.Width(60)))
                {
                    directoryPrefabs.Clear();
                    allAssetPaths.Clear();
                    Path = "";
                }
            }
        }
    }

    private void LoadPrefab()
    {
        if (GUILayout.Button("加载预制体"))
            if (!string.IsNullOrEmpty(Path))
            {
                var direction = new DirectoryInfo(Path);
                var files = direction.GetFiles("*.prefab", SearchOption.AllDirectories);
                for (var i = 0; i < files.Length; i++)
                {
                    var startindex = files[i].FullName.IndexOf("Assets");
                    var unityPath = files[i].FullName.Substring(startindex);
                    if (!directoryPrefabs.Contains(unityPath)) directoryPrefabs.Add(unityPath);
                }
            }
    }

    private void Text2TextMeshPro()
    {
        if (directoryPrefabs != null && directoryPrefabs.Count > 0)
            if (GUILayout.Button("一键替换"))
            {
                for (var i = 0; i < directoryPrefabs.Count; i++)
                {
                    var index = i;
                    Text2TextMeshPro(directoryPrefabs[index]);
                    EditorUtility.DisplayProgressBar("替换进度", "当前进度", index / (float)directoryPrefabs.Count);
                }

                EditorUtility.ClearProgressBar();
            }
    }

    private void Text2TextMeshPro(string path)
    {
        var root = PrefabUtility.LoadPrefabContents(path);
        if (root)
        {
            var list = root.GetComponentsInChildren<Text>(true);
            for (var i = 0; i < list.Length; i++)
            {
                var text = list[i];
                var target = text.transform;
                var size = text.rectTransform.sizeDelta;
                var strContent = text.text;
                var color = text.color;
                var fontSize = text.fontSize;
                var fontStyle = text.fontStyle;
                var textAnchor = text.alignment;
                var richText = text.supportRichText;
                var horizontalWrapMode = text.horizontalOverflow;
                var verticalWrapMode = text.verticalOverflow;
                var raycastTarget = text.raycastTarget;
                DestroyImmediate(text);

                var textMeshPro = target.gameObject.AddComponent<TextMeshProUGUI>();
                if (tmpFont == null) tmpFont = Resources.Load<TMP_FontAsset>("MyTextMesh");
                textMeshPro.rectTransform.sizeDelta = size;
                textMeshPro.text = strContent;
                textMeshPro.color = color;
                textMeshPro.fontSize = fontSize;
                textMeshPro.fontStyle = fontStyle == FontStyle.BoldAndItalic ? FontStyles.Bold : (FontStyles)fontStyle;
                switch (textAnchor)
                {
                    case TextAnchor.UpperLeft:
                        textMeshPro.alignment = TextAlignmentOptions.TopLeft;
                        break;
                    case TextAnchor.UpperCenter:
                        textMeshPro.alignment = TextAlignmentOptions.Top;
                        break;
                    case TextAnchor.UpperRight:
                        textMeshPro.alignment = TextAlignmentOptions.TopRight;
                        break;
                    case TextAnchor.MiddleLeft:
                        textMeshPro.alignment = TextAlignmentOptions.MidlineLeft;
                        break;
                    case TextAnchor.MiddleCenter:
                        textMeshPro.alignment = TextAlignmentOptions.Midline;
                        break;
                    case TextAnchor.MiddleRight:
                        textMeshPro.alignment = TextAlignmentOptions.MidlineRight;
                        break;
                    case TextAnchor.LowerLeft:
                        textMeshPro.alignment = TextAlignmentOptions.BottomLeft;
                        break;
                    case TextAnchor.LowerCenter:
                        textMeshPro.alignment = TextAlignmentOptions.Bottom;
                        break;
                    case TextAnchor.LowerRight:
                        textMeshPro.alignment = TextAlignmentOptions.BottomRight;
                        break;
                }

                textMeshPro.richText = richText;
                if (verticalWrapMode == VerticalWrapMode.Overflow)
                {
                    textMeshPro.enableWordWrapping = true;
                    textMeshPro.overflowMode = TextOverflowModes.Overflow;
                }
                else
                {
                    textMeshPro.enableWordWrapping = horizontalWrapMode == HorizontalWrapMode.Overflow ? false : true;
                }

                textMeshPro.raycastTarget = raycastTarget;
            }
        }

        PrefabUtility.SaveAsPrefabAsset(root, path, out var success);
        if (!success) Debug.LogError($"预制体：{path} 保存失败!");
        if (isAutoFixLinkedScripts) ChangeScriptsText2TextMeshPro();
    }

    private void ShowScriptsFolders()
    {
        for (var i = 0; i < scriptsFolders.Count; i++)
        {
            var index = i;
            EditorGUILayout.BeginHorizontal();
            SelectScriptsFolder(index);
            EditorGUILayout.EndHorizontal();
        }
    }

    private void SelectScriptsFolder(int index)
    {
        var e = Event.current;
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label($"scriptsFolder{index + 1}:", GUILayout.Width(80));
        scriptsFolders[index] = GUILayout.TextField(scriptsFolders[index], GUILayout.Width(200));
        if (Event.current.type == EventType.DragExited || Event.current.type == EventType.DragUpdated)
        {
            if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                if (Event.current.type == EventType.DragExited)
                {
                    DragAndDrop.AcceptDrag();
                    if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
                    {
                        if (File.Exists(DragAndDrop.paths[0]))
                        {
                            EditorUtility.DisplayDialog("警告", "请选择文件夹！", "确定");
                            return;
                        }

                        scriptsFolders[index] = DragAndDrop.paths[0];
                    }
                }

                e.Use();
            }
            else
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
            }
        }

        if (GUILayout.Button("添加路径")) scriptsFolders.Add(scriptsFolders[scriptsFolders.Count - 1]);
        if (GUILayout.Button("删除路径"))
        {
            if (scriptsFolders.Count == 1)
            {
                EditorUtility.DisplayDialog("警告", "仅剩最后一个文件夹，删除将会出错！", "确定");
                return;
            }

            scriptsFolders.RemoveAt(index);
        }

        EditorGUILayout.EndHorizontal();
    }

    private void ChangeScriptsText2TextMeshPro()
    {
        allAssetPaths.Clear();
        Debug.LogError(directoryPrefabs.Count);
        if (directoryPrefabs != null && directoryPrefabs.Count > 0)
        {
            for (var i = 0; i < directoryPrefabs.Count; i++)
            {
                var index = i;
                var scriptsName = System.IO.Path.GetFileNameWithoutExtension(directoryPrefabs[index]);
                var tmp = AssetDatabase.FindAssets($"{scriptsName} t:Script", scriptsFolders.ToArray());
                if (tmp != null && tmp.Length > 0) allAssetPaths.AddRange(tmp);
            }

            for (var i = 0; i < allAssetPaths.Count; i++)
            {
                var index = i;
                ChangeScriptsText2TextMeshPro(AssetDatabase.GUIDToAssetPath(allAssetPaths[index]));
            }
        }

        AssetDatabase.Refresh();
    }

    private void ChangeScriptsText2TextMeshPro(string script)
    {
        var sr = new StreamReader(script);
        var str = sr.ReadToEnd();
        sr.Close();
        str = str.Replace("<Text>", "<TMPro.TextMeshProUGUI>");
        str = str.Replace(" Text ", " TMPro.TextMeshProUGUI ");
        var sw = new StreamWriter(script, false, Encoding.UTF8);
        sw.Write(str);
        sw.Close();
    }
}
}
