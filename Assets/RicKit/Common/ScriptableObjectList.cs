using System.Collections.Generic;
using UnityEngine;

namespace RicKit.Common
{
    public abstract class ScriptableObjectList<T> : ScriptableObject
    {
        public List<T> list = new List<T>();
    }
}