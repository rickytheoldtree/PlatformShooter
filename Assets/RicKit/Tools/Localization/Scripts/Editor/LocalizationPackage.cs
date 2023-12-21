using System;
using System.Collections.Generic;
using UnityEngine;

namespace RicKit.Tools.Localization
{
    [Serializable]
    public class LocalizationPackage : ScriptableObject
    {
        public LanguageEnum language = LanguageEnum.English;
        public bool isNew;
        [Serializable]
        public struct Kvp
        {
            public string key;
            public string value;
            public Kvp(string key, string value)
            {
                this.key = key;
                this.value = value;
            }
        }
        public List<LanguageEnum> SupportedLanguages => LocalizationEditorUtils.GetSupportedLanguages();
        public List<Kvp> fields = new List<Kvp>();
    }
}
