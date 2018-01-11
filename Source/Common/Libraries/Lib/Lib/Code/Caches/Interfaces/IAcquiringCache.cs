

namespace Public.Common.Lib
{
    /// <summary>
    /// A cache that can acquire the value corresponding to a key if desired.
    /// </summary>
    /// <remarks>
    /// Note to implementers: the behavior of the this[TKey key] indexer method is that it should check if the cache contains the key, and if not, acquire and add the value acquired for the key.
    /// The easy way to do this is to call the IfNotContainedThenAcquireAndAdd() extension method.
    /// 
    /// This is somewhat of a factory pattern.
    /// </remarks>
    public interface IAcquiringCache<TKey, TValue> : ICache<TKey, TValue>
    {
        /// <summary>
        /// Acquire the value for a given key.
        /// </summary>
        /// <remarks>
        /// The acquire function does only that: acquires. It does not add, or check if the key is already present.
        /// That functionality will be handled by an extension method.
        /// </remarks>
        TValue Acquire(TKey key);
    }


    /// <summary>
    /// A 2D acquiring cache.
    /// </summary>
    /// <remarks>
    /// Note to implementers: the behavior of the this[TKey key] indexer method is that it should check if the cache contains the key, and if not, acquire and add the value acquired for the key.
    /// The easy way to do this is to call the IfNotContainedThenAcquireAndAdd() extension method.
    /// </remarks>
    public interface IAcquiringCache<TKey1, TKey2, TValue> : ICache<TKey1, TKey2, TValue>
    {
        /// <summary>
        /// Acquire the value for a given key.
        /// </summary>
        /// <remarks>
        /// The acquire function does only that: acquires. It does not add, or check if the key is already present.
        /// That functionality will be handled by an extension method.
        /// </remarks>
        TValue Acquire(TKey1 key1, TKey2 key2);
    }


    /// <summary>
    /// Ease of implementation extenion method.
    /// </summary>
    public static class IAcquiringCacheExtensions
    {
        public static TValue IfNotContainedThenAcquireAndAdd<TKey, TValue>(this IAcquiringCache<TKey, TValue> cache, TKey key)
        {
            TValue output;
            if(cache.ContainsKey(key))
            {
                output = cache.GetValue(key);
            }
            else
            {
                output = cache.Acquire(key);
                cache.Add(key, output);
            }

            return output;
        }

        public static TValue IfNotContainedThenAcquireAndAdd<TKey1,TKey2, TValue>(this IAcquiringCache<TKey1, TKey2, TValue> cache, TKey1 key1, TKey2 key2)
        {
            TValue output;
            if (cache.ContainsKey(key1, key2))
            {
                output = cache.GetValue(key1, key2);
            }
            else
            {
                output = cache.Acquire(key1, key2);
                cache.Add(key1, key2, output);
            }

            return output;
        }
    }
}
