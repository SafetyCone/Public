using System.Collections.Generic;


namespace Public.Common.Lib
{
    public interface ICache<T>
    {
        T this[string key] { get; }
    }

    /// <summary>
    /// The plain cache interface.
    /// </summary>
    /// <remarks>
    /// Note to implementers: the assumed behavior of the this[TKey key] indexer method is that it should throw an error if the key is not found.
    /// 
    /// Some additional functionality that is questionable:
    /// * List all keys.
    /// * List all values.
    /// * IEnumerable(Tuple(Tkey,TValue))
    /// </remarks>
    public interface ICache<TKey, TValue>
    {
        /// <remarks>
        /// Note to implementers: The assumed behavior is to error if key is not found.
        /// </remarks>
        TValue this[TKey key] { get; }
        //IEnumerable<TKey> Keys { get; }


        bool ContainsKey(TKey key);
        /// <remarks>
        /// Note to implementers: the assumed behavior is the default dictionary behavior: error if the cache already contains the key.
        /// </remarks>
        void Add(TKey key, TValue value, bool forceReplace = false);
        /// <remarks>
        /// Note to implementers: the assumed behavior is the default dictionary behavior: no error if the key does not exist.
        /// </remarks>
        void Remove(TKey key);
        /// <summary>
        /// Returns a value for a key.
        /// </summary>
        /// <remarks>
        /// While this may duplicate the indexer property get functionality, it directly retrieves a value from the cache.
        /// 
        /// Note to implementers: this method should directly retrieve a value from the cache, no extra steps. Thus, if the key is not present, the cache should throw an exception.
        /// This is useful for caches that add extra functionality to the indexer property get.
        /// </remarks>
        TValue GetValue(TKey key);
        /// <summary>
        /// Remove all entries from the cache.
        /// </summary>
        void Clear();
    }


    /// <summary>
    /// A plain 2D cache.
    /// </summary>
    /// <remarks>
    /// Note to implementers: the assumed behavior of the this[TKey key] indexer method is that it should throw an error if the key is not found.
    /// </remarks>
    public interface ICache<TKey1, TKey2, TValue>
    {
        /// <remarks>
        /// Note to implementers: The assumed behavior is to error if key is not found.
        /// </remarks>
        TValue this[TKey1 key1, TKey2 key2] { get; }


        bool ContainsKey(TKey1 key1, TKey2 key2);
        /// <remarks>
        /// Note to implementers: the assumed behavior is the default dictionay behavior: error if the cache already contains the key.
        /// </remarks>
        void Add(TKey1 key1, TKey2 key2, TValue value, bool forceReplace = false);
        /// <remarks>
        /// Note to implementers: the assumed behavior is the default dictionary behavior: no error if the key does not exist.
        /// </remarks>
        void Remove(TKey1 key1, TKey2 key2);
        /// <summary>
        /// Returns a value for a key.
        /// </summary>
        /// <remarks>
        /// While this may duplicate the indexer property get functionality, it directly retrieves a value from the cache.
        /// 
        /// Note to implementers: this method should directly retrieve a value from the cache, no extra steps. Thus, if the key is not present, the cache should throw an exception.
        /// This is useful for caches that add extra functionality to the indexer property get.
        /// </remarks>
        TValue GetValue(TKey1 key1, TKey2 key2);
        /// <summary>
        /// Remove all entries from the cache.
        /// </summary>
        void Clear();
    }
}
