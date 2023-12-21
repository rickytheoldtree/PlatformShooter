using System.Collections.Generic;
using RicKit.Tools.Localization.JsonConverter;
using UnityEngine;

namespace RicKit.Tools.Localization
{
    public class Config : ScriptableObject
    {
        public List<LanguageEnum> supportedLanguages = new List<LanguageEnum>();
        public WebDriverType webDriver;
        public enum WebDriverType
        {
            Edge,
            Chrome,
            FireFox,
        }
        public IJsonConverter CurrentJsonConverter
        {
            get
            {
                //尝试用反射获取当前的JsonConverter
                var type = System.Type.GetType(currentJsonConverterName);
                if (type != null)
                {
                    return System.Activator.CreateInstance(type) as IJsonConverter;
                }
                else
                {
                    //如果反射失败，就用默认的JsonConverter
                    return new DefaultJsonConverter();
                }
            }
        }
        public string currentJsonConverterName = "RicKit.Tools.Localization.JsonConverter.DefaultJsonConverter";
        public int Count => supportedLanguages.Count;
        
        public bool Contains(LanguageEnum language)
        {
            return supportedLanguages.Contains(language);
        }
        public void Add(LanguageEnum language)
        {
            if (!Contains(language))
            {
                supportedLanguages.Add(language);
            }
        }
        public void Remove(LanguageEnum language)
        {
            if (Contains(language))
            {
                supportedLanguages.Remove(language);
            }
        }
    }

}
