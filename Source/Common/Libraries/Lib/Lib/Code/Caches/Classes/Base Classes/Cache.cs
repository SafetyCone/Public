using System.Collections.Generic;


namespace Public.Common.Lib
{
    public class Cache<TKey, TValue> : ICache<TKey, TValue>
    {
        protected Dictionary<TKey, TValue> ValuesByKey = new Dictionary<TKey, TValue>();
        public TValue this[TKey key]
        {
            get
            {
                TValue output = this.ValuesByKey[key];
                return output;
            }
        }
        //public IEnumerable<TKey> Keys => this.zValuesByKey.Keys;


        public void Add(TKey key, TValue value, bool forceReplace = false)
        {
            if(forceReplace)
            {
                this.Remove(key);
            }

            this.ValuesByKey.Add(key, value);
        }

        public void Clear()
        {
            this.ValuesByKey.Clear();
        }

        public bool ContainsKey(TKey key)
        {
            bool output = this.ValuesByKey.ContainsKey(key);
            return output;
        }

        public void Remove(TKey key)
        {
            this.ValuesByKey.Remove(key);
        }

        public TValue GetValue(TKey key)
        {
            TValue output = this.ValuesByKey[key];
            return output;
        }
    }
}
