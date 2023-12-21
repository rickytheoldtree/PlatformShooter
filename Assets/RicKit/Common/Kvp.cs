using System;
namespace RicKit.Common
{
    [Serializable]
    public struct Kvp<TK,TV>
    {
        public TK key;
        public TV value;
        public Kvp(TK key, TV value)
        {
            this.key = key;
            this.value = value;
        }
    }
}

