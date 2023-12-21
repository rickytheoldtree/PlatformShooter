using System;
using System.Collections.Generic;
using UnityEngine;

namespace RicKit.DicResource
{
    [Serializable]
    internal struct Kvp<TK, TV>
    {
        public TK key;
        public TV value;
        public Kvp(TK key, TV value)
        {
            this.key = key;
            this.value = value;
        }
            
    }
    [Serializable]
    public abstract class DicResource<TK, TV> : ScriptableObject
    {

        [SerializeField]
        private List<Kvp<TK, TV>> dict;
        private Dictionary<TK,TV> RealDict
        {
            get
            {
                if (realDic == null)
                {
                    Init();
                }
                return realDic;
            }
        }
        private Dictionary<TK, TV> realDic;
        private void Init()
        {
            realDic = new Dictionary<TK, TV>();
            foreach (var kvp in dict)
            {
                realDic.Add(kvp.key, kvp.value);
            }
        }
        public TV this[TK key]
        {
            get => RealDict[key];
            set => RealDict[key] = value;
        }
        public bool Contains(TK key)
        {
            return RealDict.ContainsKey(key);
        }
        public void Remove(TK key)
        {
            RealDict.Remove(key);
        }
        public void Clear()
        {
            RealDict.Clear();
        }
        public void Add(TK key, TV value)
        {
            RealDict.Add(key, value);
        }
    }
    
}
