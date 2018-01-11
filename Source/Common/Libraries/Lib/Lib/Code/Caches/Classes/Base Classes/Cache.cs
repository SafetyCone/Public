using System.Collections.Generic;


namespace Public.Common.Lib
{
    public class Cache<TKey, TValue> : ICache<TKey, TValue>
    {
        protected Dictionary<TKey, TValue> zValuesByKey = new Dictionary<TKey, TValue>();
        public TValue this[TKey key]
        {
            get
            {
                TValue output = this.zValuesByKey[key];
                return output;
            }
        }


        public void Add(TKey key, TValue value, bool forceReplace = false)
        {
            if(forceReplace)
            {
                this.Remove(key);
            }

            this.zValuesByKey.Add(key, value);
        }

        public void Clear()
        {
            this.zValuesByKey.Clear();
        }

        public bool ContainsKey(TKey key)
        {
            bool output = this.zValuesByKey.ContainsKey(key);
            return output;
        }

        public void Remove(TKey key)
        {
            this.zValuesByKey.Remove(key);
        }

        public TValue GetValue(TKey key)
        {
            TValue output = this.zValuesByKey[key];
            return output;
        }
    }
}
