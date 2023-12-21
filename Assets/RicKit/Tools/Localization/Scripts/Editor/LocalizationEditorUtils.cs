using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RicKit.Tools.Localization.JsonConverter;
using RicKit.Tools.Localization.Translator;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using Kvp = RicKit.Tools.Localization.LocalizationPackage.Kvp;

namespace RicKit.Tools.Localization
{
    public static class LocalizationEditorUtils
    {

        public const string DefaultRoot = "Assets/Localization";
        public const string MainPackageRoot = DefaultRoot + "/MainPackage";
        public const string MainPackageName = "MainEditor.asset";
        private const string NewPackageRoot = DefaultRoot + "/NewPacakge";
        private const string NewPacakgeName = "NewEditor.asset";
        public const string ConfigName = "Config.asset";
        public static LanguageEnum langShow = LanguageEnum.English;
        private static LanguageEnum langFrom;
        [MenuItem("RicKit/Localization/Create MainEditor")]
        private static void GetMainEditorMenu()
        {
            GetMainEditor();
        }

        private static LocalizationPackage GetMainEditor()
        {
            var localization = AssetDatabase.LoadAssetAtPath<LocalizationPackage>($"{MainPackageRoot}/{MainPackageName}");
            if(!localization)
            {
                if (!Directory.Exists(MainPackageRoot))
                {
                    Directory.CreateDirectory(MainPackageRoot);
                    AssetDatabase.Refresh();
                }
                localization = ScriptableObject.CreateInstance<LocalizationPackage>();
                AssetDatabase.CreateAsset(localization, $"{MainPackageRoot}/{MainPackageName}");
                Debug.Log($"Create {MainPackageName} at {MainPackageRoot}");
            }
            else
            {
                Debug.Log($"{MainPackageName} 在 {MainPackageRoot}");
            }
            GetConfig();
            return localization;
        }

        private static Config GetConfig()
        {
            var config = AssetDatabase.LoadAssetAtPath<Config>($"{DefaultRoot}/{ConfigName}");
            if(!config)
            {
                if (!Directory.Exists(DefaultRoot))
                {
                    Directory.CreateDirectory(DefaultRoot);
                    AssetDatabase.Refresh();
                }
                config = ScriptableObject.CreateInstance<Config>();
                AssetDatabase.CreateAsset(config, $"{DefaultRoot}/{ConfigName}");
                Debug.Log($"Create {ConfigName} at {DefaultRoot}");
            }
            return config;
        }
        public static List<LanguageEnum> GetSupportedLanguages()
        {
            var config = GetConfig();
            return config.supportedLanguages;
        }
        public static Config.WebDriverType GetWebDriver()
        {
            var config = GetConfig();
            return config.webDriver;
        }
        private static LocalizationPackage GetNewEditor()
        {
            var localization = AssetDatabase.LoadAssetAtPath<LocalizationPackage>($"{NewPackageRoot}/{NewPacakgeName}");
            if(!localization)
            {
                if (!Directory.Exists(NewPackageRoot))
                {
                    Directory.CreateDirectory(NewPackageRoot);
                    AssetDatabase.Refresh();
                }
                localization = ScriptableObject.CreateInstance<LocalizationPackage>();
                AssetDatabase.CreateAsset(localization, $"{NewPackageRoot}/{NewPacakgeName}");
                Debug.Log($"Create {NewPacakgeName} at {NewPackageRoot}");
            }
            else
            {
                Debug.Log($"{NewPacakgeName} 在 {NewPackageRoot}");
            }

            localization.isNew = true;
            return localization;
        }

        public static void CreateNewJsonFolder(LocalizationPackage localization)
        {
            if (!Directory.Exists($"{GetRootPath(localization)}/NEWJSON"))
            {
                Directory.CreateDirectory($"{GetRootPath(localization)}/NEWJSON");
                AssetDatabase.Refresh();
            }
        }
        public static List<IJsonConverter> GetJsonConverters()
        {
            var types = typeof(IJsonConverter).Assembly.GetTypes();
            var converters = new List<IJsonConverter>();
            foreach (var type in types)
            {
                if (type.IsAbstract || type.IsInterface)
                    continue;
                if (typeof(IJsonConverter).IsAssignableFrom(type))
                {
                    converters.Add((IJsonConverter)Activator.CreateInstance(type));
                }
            }
            return converters;
        }

        private static void OutputJson(string path, Dictionary<string, string> dic)
        {
            File.WriteAllText(path, GetConfig().CurrentJsonConverter.Convert(dic));
            AssetDatabase.Refresh();
        }
        private static Dictionary<string, string> InputJson(string path)
        {
            return GetConfig().CurrentJsonConverter.Convert(File.ReadAllText(path));
        }
        
        
        public static void AddKeyToNewPackageEnglish(string key, string value)
        {
            var localization = GetNewEditor();
            Selection.activeObject = localization;
            
            EditorApplication.delayCall += () =>
            {
                localization.language = LanguageEnum.English;
                localization.Load();
                localization.AddKey(key, value);
            };
        }
        public static void AddKey(this LocalizationPackage local, string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.Log("key is empty");
                return;
            }
            value ??= "";
            if (local.fields.Any(f => f.key == key))
            {
                Debug.Log($"key {key} already exist");
                return;
            }
            local.fields.Add(new Kvp(key, value));
            Debug.Log($"Add key \"{key}\" to {local.name}: {local.language}");
            EditorUtility.SetDirty(local);
            AssetDatabase.SaveAssets();
            LocalizationPackageAbstractEditor.foldSearch = true;
            LocalizationPackageAbstractEditor.searchKey = key;
        }
        public static void Save(this LocalizationPackage local)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var l in local.fields)
            {
                if (string.IsNullOrEmpty(l.key))
                {
                    Debug.LogError("key is null");
                    return;
                }
                if (dic.ContainsKey(l.key))
                {
                    Debug.LogError($"key {l.key} is repeat");
                    return;
                }
                dic.Add(l.key, l.value);
            }
            if (!Directory.Exists($"{GetRootPath(local)}/JSON"))
            {
                Directory.CreateDirectory($"{GetRootPath(local)}/JSON");
            }
            OutputJson($"{GetRootPath(local)}/JSON/{local.language}.json", dic);
        }
        public static void Load(this LocalizationPackage local)
        {
            langShow = local.language;
            var lang = LanguageEnum.English;
            if (!Directory.Exists($"{GetRootPath(local)}/JSON"))
            {
                Directory.CreateDirectory($"{GetRootPath(local)}/JSON");
            }
            if(File.Exists($"{GetRootPath(local)}/JSON/{local.language}.json"))
            {
                lang = local.language;
            }

            var dic = File.Exists($"{GetRootPath(local)}/JSON/{lang}.json") ? 
                InputJson($"{GetRootPath(local)}/JSON/{lang}.json") 
                : new Dictionary<string, string>();
            local.fields.Clear();
            foreach (var d in dic)
            {
                local.fields.Add(new Kvp(d.Key, d.Value));
            }
            EditorUtility.SetDirty(local);
            AssetDatabase.SaveAssets();
        }
        public static void Sort(this LocalizationPackage local)
        {
            local.fields.Sort((a, b) => string.Compare(a.key, b.key, StringComparison.Ordinal));
            EditorUtility.SetDirty(local);
            AssetDatabase.SaveAssets();
        }

        public static void ExportForTranslation(this LocalizationPackage local)
        {
            var sb = new StringBuilder();
            foreach (var l in local.fields)
            {
                sb.AppendLine(l.value.Trim());
                sb.AppendLine("-----------------------");
            }
            if(!Directory.Exists($"{GetRootPath(local)}/TXT"))
                Directory.CreateDirectory($"{GetRootPath(local)}/TXT");
            File.WriteAllText($"{GetRootPath(local)}/TXT/{local.language}.txt", sb.ToString());
            AssetDatabase.Refresh();
        }

        public static void ExportAllSupportedForTranslation(this LocalizationPackage local)
        {
            foreach (LanguageEnum lang in local.SupportedLanguages)
            {
                local.language = lang;
                Load(local);
                ExportForTranslation(local);
            }
        }

        public static bool GetSplit(string text, out List<string> values)
        {
            
            //从第一个-开始到第一个不为-结束
            var startIndex = text.IndexOf('-');
            if (startIndex == -1)
            {
                values = null;
                Debug.LogError("找不到分隔符");
                return false;
            }
            var length = 0;
            while (text.Length > startIndex + length && text[startIndex + length] == '-')
            {
                length++;
            }
            var splitString = text.Substring(startIndex, length);
            values = text.Split(new[] { splitString }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim())
                .Where(s => !string.IsNullOrEmpty(s)).ToList();
            return true;
        }
        public static void ImportFromTranslation(this LocalizationPackage local, LanguageEnum targetLanguage)
        {
            if(!File.Exists($"{GetRootPath(local)}/TXT/{targetLanguage}.txt"))
            {
                Debug.LogError($"文件 {targetLanguage}.txt 不存在");
                return;
            }
            var str = File.ReadAllText($"{GetRootPath(local)}/TXT/{targetLanguage}.txt");
            if (!GetSplit(str, out var values))
            {
                return;
            }
            var englishJson = InputJson($"{GetRootPath(local)}/JSON/English.json");
            var dic = new Dictionary<string, string>();
            var keys = englishJson.Keys.ToList();
            if(keys.Count != values.Count)
            {
                Debug.LogError("翻译后数量与原文本数量不一致");
                return;
            }
            for (var i = 0; i < keys.Count; i++)
            {
                dic.Add(keys[i], values[i].Trim());
            }
            OutputJson($"{GetRootPath(local)}/JSON/{targetLanguage}.json", dic);
            Load(local);
        }

        public static void ImportAllSupportedFromTranslation(this LocalizationPackage local)
        {
            foreach (var lang in local.SupportedLanguages)
            {
                ImportFromTranslation(local, lang);
            }
        }
        public static Dictionary<int, Kvp> Search(this LocalizationPackage local, string searchKey)
        {
            var result = new Dictionary<int, Kvp>();
            if (string.IsNullOrEmpty(searchKey))
            {
                //如果searchKey为空，就返回所有的localization
                for (var i = 0; i < local.fields.Count; i++)
                {
                    var kvp = local.fields[i];
                    result.Add(i, kvp);
                }
                return result;
            }
            //如果target的localization中的key或者value包含searchKey，就加入到result中
            for (var i = 0; i < local.fields.Count; i++)
            {
                var kvp = local.fields[i];
                if ((kvp.key != null && kvp.key.Contains(searchKey)) || 
                    (kvp.value != null && kvp.value.Contains(searchKey)))
                {
                    result.Add(i, kvp);
                }
            }

            return result;
        }

        private static string GetRootPath(Object local)
        {
            //获取LocalizationEditor.asset的路径
            var path = AssetDatabase.GetAssetPath(local);
            path = System.IO.Path.GetDirectoryName(path);
            return path;
        }
        /// <summary>
        /// 合并NEWJSON文件夹中的json文件到JSON文件夹中，如果key相同，value会被覆盖
        /// </summary>
        /// <param name="local"></param>
        public static void MergeJsons(this LocalizationPackage local)
        {
            foreach (var lang in local.SupportedLanguages)
            {
                if (!Directory.Exists($"{GetRootPath(local)}/NEWJSON"))
                {
                    Directory.CreateDirectory($"{GetRootPath(local)}/NEWJSON");
                    AssetDatabase.Refresh();
                    Debug.Log("请将需要合并的json文件放入NEWJSON文件夹中");
                    return;
                }
                if (!File.Exists($"{GetRootPath(local)}/NEWJSON/{lang}.json"))
                {
                    Debug.Log($"文件{lang}.json不存在");
                    continue;
                }
                var dic = InputJson($"{GetRootPath(local)}/NEWJSON/{lang}.json");
                var dicOld = InputJson($"{GetRootPath(local)}/JSON/{lang}.json");
                foreach (var d in dic)
                {
                    dicOld[d.Key] = d.Value;
                }
                OutputJson($"{GetRootPath(local)}/JSON/{lang}.json", dicOld);
            }
            AssetDatabase.Refresh();
        }

        public static void MergeJsonsIncreasementOnly(this LocalizationPackage local)
        {
            foreach (var lang in local.SupportedLanguages)
            {
                if (!Directory.Exists($"{GetRootPath(local)}/NEWJSON"))
                {
                    Directory.CreateDirectory($"{GetRootPath(local)}/NEWJSON");
                    AssetDatabase.Refresh();
                    Debug.Log("请将需要合并的json文件放入NEWJSON文件夹中");
                    return;
                }
                if (!File.Exists($"{GetRootPath(local)}/NEWJSON/{lang}.json"))
                {
                    Debug.Log($"文件{lang}.json不存在");
                    continue;
                }
                var dic = InputJson($"{GetRootPath(local)}/NEWJSON/{lang}.json");
                var dicOld = InputJson($"{GetRootPath(local)}/JSON/{lang}.json");
                foreach (var d in dic)
                {
                    if (!dicOld.ContainsKey(d.Key))
                    {
                        dicOld.Add(d.Key, d.Value);
                    }
                    else
                    {
                        Debug.Log($"跳过{d.Key}，因为已经存在");
                    }
                }
                OutputJson($"{GetRootPath(local)}/JSON/{lang}.json", dicOld);
            }
            AssetDatabase.Refresh();
        }
        public static void OpenNewPacakge(this LocalizationPackage local)
        {
            var newEditor = GetNewEditor();
            // 选中NewPacakge.asset
            Selection.activeObject = newEditor;
        }

        public static void MoveNewPacakgeJson2MainPackageNewJson(this LocalizationPackage newEditor)
        {
            var mainEditor = GetMainEditor();
            //判断mainPackage的NEWJSOn文件夹是否为空
            if (!Directory.Exists($"{GetRootPath(mainEditor)}/NEWJSON"))
            {
                Directory.CreateDirectory($"{GetRootPath(mainEditor)}/NEWJSON");
            }
            //判断NewJSON里面是否有文件
            if (Directory.GetFiles($"{GetRootPath(mainEditor)}/NEWJSON").Length != 0)
            {
                Debug.LogWarning("【移动取消】MainPackage/NEWJSON文件夹内有东西，请自行确认后删除");
                return;
            }
            //将NewPackage的JSON文件夹复制到MainPackage的NEWJSON文件夹
            foreach (var lang in newEditor.SupportedLanguages)
            {
                if (!File.Exists($"{GetRootPath(newEditor)}/JSON/{lang}.json"))
                {
                    Debug.Log($"文件{lang}.json不存在");
                    continue;
                }
                File.Copy($"{GetRootPath(newEditor)}/JSON/{lang}.json", $"{GetRootPath(mainEditor)}/NEWJSON/{lang}.json");
            }
            AssetDatabase.Refresh();
        }

        #region 翻译
        public static bool TranslateJson(this LocalizationPackage local, LanguageEnum from, LanguageEnum to)
        {
            var translator = GoogleTranslator.GetInstance(GetConfig().webDriver);
            if(from == to)
            {
                Debug.Log($"跳过翻译{to}，因为与源语言相同");
                return true;
            }
            var rootPath = GetRootPath(local);
            var originJsonPath = $"{rootPath}/JSON/{from}.json";
            if (!File.Exists(originJsonPath))
            {
                Debug.LogError($"文件{rootPath}/JSON/{from}.json不存在");
                return false;
            }
            var sb = new StringBuilder();
            var originJson = InputJson(originJsonPath);
            var values = new List<string>();
            foreach (var d in originJson)
            {
                var newText = $"{d.Value.Trim()}%0A-----------------------%0A";
                if (newText.Length > 5000)
                {
                    Debug.LogError($"单个文本长度超过5000，无法翻译：{d.Key}；请自行切分后重新尝试");
                    return false;
                }
                if(sb.Length + newText.Length <= 5000)
                {
                    sb.Append(newText);
                }
                else
                {
                    if (!translator.Translate(sb.ToString(), from, to, (result) => { values.AddRange(result); }))
                    {
                        return false;
                    }
                    sb.Clear();
                    sb.Append(newText);
                }
            }
            if(sb.Length > 0)
            {
                if (!translator.Translate(sb.ToString(), from, to, (result) => { values.AddRange(result); }))
                {
                    return false;
                }
            }
            var dic = new Dictionary<string, string>();
            var keys = originJson.Keys.ToList();
            if(keys.Count != values.Count)
            {
                Debug.LogError("翻译后数量与原文本数量不一致");
                return false;
            }
            for (var i = 0; i < keys.Count; i++)
            {
                dic.Add(keys[i], values[i]);
            }
            var foldPath = $"{rootPath}/TranslateJSON";
            if (!Directory.Exists(foldPath))
            {
                Directory.CreateDirectory(foldPath);
            }
            OutputJson($"{foldPath}/{to}.json", dic);
            Debug.Log($"{to}翻译完成，文件路径：{foldPath}/{to}.json");
            return true;
        }
        public static bool IsTranslated(this LocalizationPackage local, LanguageEnum lang)
        {
            var rootPath = $"{GetRootPath(local)}/TranslateJSON";
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }
            var path = $"{rootPath}/{lang}.json";
            return File.Exists(path);
        }
        #endregion

        
    }
}
