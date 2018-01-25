using System;


namespace Public.Common.Lib
{
    /// <summary>
    /// Base-class for all acquiring caches that use an indexed file system for cache value storage.
    /// </summary>
    /// <remarks>
    /// The single responsibility of this class is to dispose of its contained indexed file system cache.
    /// </remarks>
    public abstract class AcquiringIndexedFileSystemCacheBase<TKey, TValue> : IAcquiringCache<TKey, TValue>, IDisposable
    {
        #region IDisposable

        private bool zDisposed = false;


        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.zDisposed)
            {
                if (disposing)
                {
                    // Clean-up managed resource here.
                    this.IndexedFileSystemCache.Dispose();
                }

                // Clean-up unmanaged resources here.
            }

            this.zDisposed = true;
        }

        ~AcquiringIndexedFileSystemCacheBase()
        {
            this.Dispose(false);
        }

        #endregion


        protected IndexedFileSystemCache<TKey, TValue> IndexedFileSystemCache { get; private set; } // The value storage.
        public TValue this[TKey key]
        {
            get
            {
                TValue output = this.IfNotContainedThenAcquireAndAdd(key);
                return output;
            }
        }


        public AcquiringIndexedFileSystemCacheBase(IndexedFileSystemCache<TKey, TValue> indexedFileSystemCache)
        {
            this.IndexedFileSystemCache = indexedFileSystemCache;
        }

        public abstract TValue Acquire(TKey key);

        public bool ContainsKey(TKey key)
        {
            bool output = this.IndexedFileSystemCache.ContainsKey(key);
            return output;
        }

        public void Add(TKey key, TValue value, bool forceReplace = false)
        {
            this.IndexedFileSystemCache.Add(key, value, forceReplace);
        }

        public void Remove(TKey key)
        {
            this.IndexedFileSystemCache.Remove(key);
        }

        public TValue GetValue(TKey key)
        {
            TValue output = this.IndexedFileSystemCache.GetValue(key);
            return output;
        }

        public void Clear()
        {
            this.IndexedFileSystemCache.Clear();
        }

        public void Persist()
        {
            this.IndexedFileSystemCache.Persist();
        }
    }


    /// <summary>
    /// Base-class for all 2D acquiring caches that use an indexed file system for cache value storage.
    /// </summary>
    /// <remarks>
    /// The responsibilities of this class are:
    /// * Dispose of its contained indexed file system cache.
    /// * Map TKey1 and TKey2 to TKeyStore so that a single-keyed indexed file system cache can store the 
    /// </remarks>
    public abstract class AcquiringIndexedFileSystemCacheBase<TKey1, TKey2, TKeyStore, TValue> : IAcquiringCache<TKey1, TKey2, TValue>, IDisposable
    {
        #region IDisposable

        private bool zDisposed = false;


        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.zDisposed)
            {
                if (disposing)
                {
                    // Clean-up managed resource here.
                    this.IndexedFileSystemCache.Dispose();
                }

                // Clean-up unmanaged resources here.
            }

            this.zDisposed = true;
        }

        ~AcquiringIndexedFileSystemCacheBase()
        {
            this.Dispose(false);
        }

        #endregion


        protected IndexedFileSystemCache<TKeyStore, TValue> IndexedFileSystemCache { get; private set; } // The value storage.
        public TValue this[TKey1 key1, TKey2 key2]
        {
            get
            {
                TValue output = this.IfNotContainedThenAcquireAndAdd(key1, key2);
                return output;
            }
        }


        public AcquiringIndexedFileSystemCacheBase(IndexedFileSystemCache<TKeyStore, TValue> indexedFileSystemCache)
        {
            this.IndexedFileSystemCache = indexedFileSystemCache;
        }

        protected abstract TKeyStore Key1Key2ToKeyStore(TKey1 key1, TKey2 key2);

        public abstract TValue Acquire(TKey1 key1, TKey2 key2);

        public bool ContainsKey(TKey1 key1, TKey2 key2)
        {
            TKeyStore keyStore = this.Key1Key2ToKeyStore(key1, key2);

            bool output = this.IndexedFileSystemCache.ContainsKey(keyStore);
            return output;
        }

        public void Add(TKey1 key1, TKey2 key2, TValue value, bool forceReplace = false)
        {
            TKeyStore keyStore = this.Key1Key2ToKeyStore(key1, key2);

            this.IndexedFileSystemCache.Add(keyStore, value, forceReplace);
        }

        public void Remove(TKey1 key1, TKey2 key2)
        {
            TKeyStore keyStore = this.Key1Key2ToKeyStore(key1, key2);

            this.IndexedFileSystemCache.Remove(keyStore);
        }

        public TValue GetValue(TKey1 key1, TKey2 key2)
        {
            TKeyStore keyStore = this.Key1Key2ToKeyStore(key1, key2);

            TValue output = this.IndexedFileSystemCache.GetValue(keyStore);
            return output;
        }

        public void Clear()
        {
            this.IndexedFileSystemCache.Clear();
        }

        public void Persist()
        {
            this.IndexedFileSystemCache.Persist();
        }
    }


    public static class AcquiringIndexedFileSystemCacheBaseExtensions
    {
        public static readonly string StringCombinationSeparator = @"<>";


        public static string CombineKey1Key2(string key1, string key2)
        {
            string output = key1 + AcquiringIndexedFileSystemCacheBaseExtensions.StringCombinationSeparator + key2;
            return output;
        }

        public static string CombineKey1Key2<TValue>(this AcquiringIndexedFileSystemCacheBase<string, string, string, TValue> cache, string key1, string key2)
        {
            var output = AcquiringIndexedFileSystemCacheBaseExtensions.CombineKey1Key2(key1, key2);
            return output;
        }

        public static Tuple<string, string> SeparateKey1Key2(string combinedKey)
        {
            string[] keys = combinedKey.Split(new string[] { AcquiringIndexedFileSystemCacheBaseExtensions.StringCombinationSeparator }, StringSplitOptions.None);

            var output = Tuple.Create(keys[0], keys[1]);
            return output;
        }

        public static Tuple<string, string> SeparateKey1Key2<TValue>(this AcquiringIndexedFileSystemCacheBase<string, string, string, TValue> cache, string combinedKey)
        {
            var output = AcquiringIndexedFileSystemCacheBaseExtensions.SeparateKey1Key2(combinedKey);
            return output;
        }
    }
}
