using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RicKit.Tools.Localization
{
    public class SupportedLanguages : ScriptableObject, IEnumerable<LanguageEnum>
    {
        public List<LanguageEnum> supportedLanguages = new List<LanguageEnum>();
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
        public IEnumerator<LanguageEnum> GetEnumerator()
        {
            return supportedLanguages.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

