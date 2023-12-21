using System.Collections.Generic;
using UnityEngine;

namespace RicKit.Tools.Localization.JsonConverter
{
    public interface IJsonConverter
    {
        Dictionary<string,string> Convert(string json);
        string Convert(Dictionary<string,string> dict);
    }
}