
namespace RicKit.Tools.Localization.Translator
{
    public static class LangEnumParse
    {
        /// <summary>
        /// 获取对应语言的字符串，如果修改了枚举，需要修改这里
        /// </summary>
        /// <param name="language">语言枚举</param>
        /// <returns>语言缩写</returns>
        public static string GetLanguageString(LanguageEnum language)
        {
            switch (language)
            {
                default:
                case LanguageEnum.Unset:
                case LanguageEnum.English:
                    return "en";
                case LanguageEnum.Bulgarian:
                    return "bg";
                case LanguageEnum.Estonian:
                    return "et";
                case LanguageEnum.Greek:
                    return "el";
                case LanguageEnum.Icelandic:
                    return "is";
                case LanguageEnum.Latvian:
                    return "lv";
                case LanguageEnum.Lithuanian:
                    return "lt";
                case LanguageEnum.Luxembourgish:
                    return "lb";
                case LanguageEnum.SerboCroatian:
                    return "sh";
                case LanguageEnum.Slovenian:
                    return "sl";
                case LanguageEnum.ChineseTW:
                    return "zh-TW";
                case LanguageEnum.Thai:
                    return "th";
                case LanguageEnum.Vietnamese:
                    return "vi";
                case LanguageEnum.ChineseSimplified:
                    return "zh-CN";
                case LanguageEnum.Japanese:
                    return "ja";
                case LanguageEnum.Korean:
                    return "ko";
                case LanguageEnum.French:
                    return "fr";
                case LanguageEnum.German:
                    return "de";
                case LanguageEnum.Italian:
                    return "it";
                case LanguageEnum.Spanish:
                    return "es";
                case LanguageEnum.Portuguese:
                    return "pt";
                case LanguageEnum.Russian:
                    return "ru";
                case LanguageEnum.Arabic:
                    return "ar";
                case LanguageEnum.Dutch:
                    return "nl";
                case LanguageEnum.Polish:
                    return "pl";
                case LanguageEnum.Danish:
                    return "da";
                case LanguageEnum.Finnish:
                    return "fi";
                case LanguageEnum.Norwegian:
                    return "no";
                case LanguageEnum.Swedish:
                    return "sv";
                case LanguageEnum.Turkish:
                    return "tr";
                case LanguageEnum.Ukrainian:
                    return "uk";
                case LanguageEnum.Hungarian:
                    return "hu";
                case LanguageEnum.Czech:
                    return "cs";
                case LanguageEnum.Romanian:
                    return "ro";
                case LanguageEnum.Slovak:
                    return "sk";
                case LanguageEnum.Croatian:
                    return "hr";
                case LanguageEnum.Indonesian:
                    return "id";
                case LanguageEnum.Malay:
                    return "ms";
                case LanguageEnum.Hindi:
                    return "hi";
            }
        }
    }
}

