using System;
using System.Collections.Generic;
using System.Linq;
using RicKit.Tools.Localization.Translator;
using UnityEditor;
using UnityEngine;
using Kvp = RicKit.Tools.Localization.LocalizationPackage.Kvp;
using Object = UnityEngine.Object;

namespace RicKit.Tools.Localization
{
    public abstract class LocalizationPackageAbstractEditor : Editor
    {
        public static string searchKey;
        private string lastSearchKey, newKey, newValue;
        private Dictionary<int, Kvp> searchResult = new Dictionary<int, Kvp>();
        private bool foldAdd, foldTxtImport, foldMutiAdd, foldOthers, foldTranslation, forceUpdate;
        public static bool foldSearch;
        private static bool foldSupportLangs;
        protected LocalizationPackage package;
        private SerializedProperty langProp;
        private Vector2 scrollPos;

        private void OnEnable()
        {
            langProp = serializedObject.FindProperty("language");
            package = (LocalizationPackage)target;
        }

        private void ShowSupportedLanguages()
        {
            var supportedLanguages = package.SupportedLanguages;
            if (supportedLanguages.Count == 0)
            {
                EditorGUILayout.HelpBox(
                    $"请在\"{LocalizationEditorUtils.DefaultRoot}/{LocalizationEditorUtils.ConfigName}\"设置游戏将会支持的语言",
                    MessageType.Warning);
                return;
            }

            var lang = package.language;
            var richTextStyle = new GUIStyle(EditorStyles.label)
            {
                richText = true
            };
            EditorGUI.indentLevel++;
            foldSupportLangs = EditorGUILayout.Foldout(foldSupportLangs, "Supported Languages");
            if (foldSupportLangs)
            {
                EditorGUI.indentLevel++;
                foreach (var language in supportedLanguages)
                {
                    EditorGUILayout.BeginHorizontal();
                    var str = language.ToString();
                    if (language == lang)
                        str = $"<color=yellow>{str}</color>";
                    EditorGUILayout.LabelField(str, richTextStyle);
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUI.indentLevel--;
            }

            EditorGUI.indentLevel--;
        }
        private static int translateSourceIndex = 0;
        // 如果修改了LanguageEnum，这里也要修改
        private static readonly string[] TranslateSource =
        {
            LanguageEnum.English.ToString(),
            LanguageEnum.ChineseSimplified.ToString()
        };
        private void ShowTranslationTools()
        {
            foldTranslation = EditorGUILayout.Foldout(foldTranslation, "翻译工具");
            if (foldTranslation)
            {
                //helpbox
                EditorGUILayout.HelpBox("翻译工具需要WebDriver，如有疑问请看README; 翻译时读取的是JSON文件夹下的.json文件为源文件，翻译后会保存到TranslateJSON文件夹", MessageType.Info);
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField($"当前WebDriver：{LocalizationEditorUtils.GetWebDriver()}");
                //选择源语言
                translateSourceIndex = EditorGUILayout.Popup("翻译源语言：", translateSourceIndex, TranslateSource);
                var sourceLang = (LanguageEnum)Enum.Parse(typeof(LanguageEnum), TranslateSource[translateSourceIndex]);
                EditorGUI.indentLevel--;
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField($"翻译成：{package.language}");
                if (GUILayout.Button($"翻译成{package.language}"))
                {
                    package.TranslateJson(sourceLang, package.language);
                    GoogleTranslator.Close();
                }
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField($"全部翻译");
                if (GUILayout.Button("翻译成所有支持语言"))
                {
                    foreach (var lang in package.SupportedLanguages)
                    {
                        if (!package.TranslateJson(sourceLang, lang))
                        {
                            Debug.LogError($"任务中断，翻译到{lang}失败");
                            break;
                        }
                    }
                    GoogleTranslator.Close();
                }
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField($"翻译剩下语言（TranslateJson中没有的）");
                if (GUILayout.Button("翻译成还未翻译过的支持的语言"))
                {
                    foreach (var lang in package.SupportedLanguages)
                    {
                        if(package.IsTranslated(lang))
                        {
                            continue;
                        }
                        if (!package.TranslateJson(sourceLang, lang))
                        {
                            Debug.LogError($"任务中断，翻译到{lang}失败");
                            break;
                        }
                    }
                    GoogleTranslator.Close();
                }
            }
        }
        public override void OnInspectorGUI()
        {
            if (langProp == null)
                OnEnable();

            #region 常规功能

            EditorGUILayout.PropertyField(langProp);
            ShowSupportedLanguages();
            //获取target的父文件夹
            var path = AssetDatabase.GetAssetPath(target);
            path = path.Substring(0, path.LastIndexOf('/'));
            path = $"{path}/JSON/{package.language}.json";
            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUI.DisabledScope(true))
                {
                    EditorGUILayout.TextField("路径：", path);
                }
                var obj = AssetDatabase.LoadAssetAtPath<Object>(path);
                if (obj && GUILayout.Button("打开文件", GUILayout.Width(100)))
                {
                    //打开json文件
                    AssetDatabase.OpenAsset(obj);
                }
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("保存到JSON"))
            {
                package.Save();
            }

            if (GUILayout.Button("从JSON加载") ||
                LocalizationEditorUtils.langShow != package.language)
            {
                forceUpdate = true;
                package.Load();
            }
            EditorGUILayout.EndHorizontal();
            if (package.isNew && GUILayout.Button("将NewPackage/JSON复制到MainPackage/NEWJSON"))
            {
                package.MoveNewPacakgeJson2MainPackageNewJson();
            }
            EditorGUILayout.Separator();

            #endregion

            #region 搜索与修改

            foldSearch = EditorGUILayout.Foldout(foldSearch, "搜索与修改");
            if (foldSearch)
            {
                searchKey =
                    EditorGUILayout.TextField("搜索：", searchKey);
                if (searchKey != lastSearchKey || forceUpdate)
                {
                    searchResult = package.Search(searchKey);
                    lastSearchKey = searchKey;
                    forceUpdate = false;
                }

                if (searchResult.Count > 0)
                {
                    EditorGUILayout.HelpBox("\"删除\"会删除缓存中的该键值对，\"添加到New\"会将key添加到NewPacakge，\"保存\"会保存到缓存",
                        MessageType.Info);
                    var localization = package.fields;
                    var richTextStyle = new GUIStyle(EditorStyles.label)
                    {
                        richText = true
                    };
                    //显示数量
                    EditorGUILayout.LabelField($"搜索到{searchResult.Count}个结果");
                    scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                    {
                        for (var i = 0; i < localization.Count; i++)
                        {
                            if (!searchResult.TryGetValue(i, out var kvp)) continue;
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                var str = kvp.key;
                                if (!string.IsNullOrEmpty(searchKey))
                                    str = str.Replace(searchKey, $"<color=yellow>{searchKey}</color>");
                                EditorGUILayout.LabelField(str, richTextStyle);
                                var v = EditorGUILayout.TextField(kvp.value);
                                kvp.value = v;
                                searchResult[i] = kvp;
                                if (GUILayout.Button("删除"))
                                {
                                    localization.Remove(kvp);
                                    searchResult.Remove(i);
                                    EditorUtility.SetDirty(target);
                                    AssetDatabase.SaveAssets();
                                    forceUpdate = true;
                                    break;
                                }

                                if (GUILayout.Button("添加到New"))
                                {
                                    LocalizationEditorUtils.AddKeyToNewPackageEnglish(kvp.key, kvp.value);
                                    break;
                                }

                                if (GUILayout.Button("保存"))
                                {
                                    localization[i] = kvp;
                                    EditorUtility.SetDirty(target);
                                    AssetDatabase.SaveAssets();
                                    // 取消选中textfeild
                                    GUI.FocusControl(null);
                                    forceUpdate = true;
                                    break;
                                }
                            }
                        }
                    }

                    EditorGUILayout.EndScrollView();
                }
            }

            #endregion

            #region 加Key

            foldAdd = EditorGUILayout.Foldout(foldAdd, "加key");
            if (foldAdd)
            {
                EditorGUI.indentLevel++;
                newKey = EditorGUILayout.TextField("Key:", newKey);
                newValue = EditorGUILayout.TextField("Value:", newValue);
                if (GUILayout.Button("加key"))
                {
                    forceUpdate = true;
                    package.AddKey(newKey, newValue);
                    serializedObject.Update();
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.HelpBox("只会加在英文里", MessageType.Info);
            }

            #endregion

            #region txt json互转(Supported Languages)

            foldTxtImport = EditorGUILayout.Foldout(foldTxtImport, "TXT-JSON互转");
            if (foldTxtImport)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("转txt"))
                    package.ExportForTranslation();
                if (GUILayout.Button("批量转txt"))
                    package.ExportAllSupportedForTranslation();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("转json"))
                    package.ImportFromTranslation(package.language);
                if (GUILayout.Button("批量转json"))
                    package.ImportAllSupportedFromTranslation();
                EditorGUILayout.EndHorizontal();
            }

            #endregion

            #region 批量增改Key工具

            foldMutiAdd = EditorGUILayout.Foldout(foldMutiAdd, "批量增改Key工具");
            if (foldMutiAdd)
            {
                LocalizationEditorUtils.CreateNewJsonFolder(package);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("打开补充包生成工具");
                if (GUILayout.Button("打开"))
                    package.OpenNewPacakge();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("批量导入Json");
                if (GUILayout.Button("相同key取新值"))
                    package.MergeJsons();
                if (GUILayout.Button("相同key取旧值"))
                    package.MergeJsonsIncreasementOnly();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.HelpBox("将NEWJSON文件夹下的Json文件和JSON文件夹中的文件合并。输出到JSON文件夹，所以记得使用前备份JSON文件夹",
                    MessageType.Info);
            }

            #endregion

            #region 翻译
            ShowTranslationTools();
            #endregion
            serializedObject.ApplyModifiedProperties();
        }
    }
}