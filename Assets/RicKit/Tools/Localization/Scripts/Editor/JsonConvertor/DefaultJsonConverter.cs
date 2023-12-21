using System.Collections.Generic;
using UnityEngine;

namespace RicKit.Tools.Localization.JsonConverter
{
    public class DefaultJsonConverter : IJsonConverter
    {
        public Dictionary<string, string> Convert(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        public string Convert(Dictionary<string, string> dict)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(dict);
        }
    }
}

