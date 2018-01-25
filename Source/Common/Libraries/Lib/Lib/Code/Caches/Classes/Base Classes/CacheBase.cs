using System;
using System.Collections.Generic;
using System.IO;

using Public.Common.Lib.IO;
using PathExtensions = Public.Common.Lib.IO.Extensions.PathExtensions;
using PathUtilities = Public.Common.Lib.IO.Paths.Utilities;


namespace Public.Common.Lib
{
    /// <summary>
    /// Base class for a file-based cache.
    /// </summary>
    public abstract class CacheBase : IDisposable
    {
        #region Static

        public static readonly string DefaultCachesDirectoryPath = PathUtilities.DefaultCachesDirectoryPath;
        public static readonly string DefaultSessionName = @"Default";
        public static readonly string IndexFileName = @"Index.txt";
        public static readonly string DataDirectoryName = @"Data";
        public static readonly char IndexTokenSeparator = '|';


        public static string GetCacheDirectoryPath(string cachesDirectoryPath, string sessionDirectoryName, string cacheDirectoryName)
        {
            string output = Path.Combine(cachesDirectoryPath, sessionDirectoryName, cacheDirectoryName);
            return output;
        }

        public static string GetNewDataFilePath(string dataDirectoryPath)
        {
            string guidStr = Guid.NewGuid().ToString().ToUpperInvariant();

            string fileName = $@"{guidStr}{PathExtensions.WindowsFileExtensionSeparatorChar}{FileExtensions.DataFileExtension}";

            string dataFilePath = Path.Combine(dataDirectoryPath, fileName);
            return dataFilePath;
        }

        public static void DeleteCache(string cachesDirectoryPath, string sessionDirectoryName, string cacheDirectoryName)
        {
            string cacheDirectoryPath = CacheBase.GetCacheDirectoryPath(cachesDirectoryPath, sessionDirectoryName, cacheDirectoryName);
            
            if(Directory.Exists(cacheDirectoryPath))
            {
                Directory.Delete(cacheDirectoryPath);
            }
        }

        #endregion

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
                }

                // Clean-up unmanaged resources here.
                this.WriteIndexFile(); // Note, the index file is written with all managed code, but since it is a file it is an unmanaged resourece.
            }

            this.zDisposed = true;
        }

        ~CacheBase()
        {
            this.Dispose(false);
        }

        #endregion


        public string DirectoryPath { get; private set; }
        protected string IndexFilePath { get; private set; }
        protected string DataDirectoryPath { get; private set; }
        protected Dictionary<string, string> DataFilePathsByKey { get; private set; } = new Dictionary<string, string>();


        protected CacheBase(string cachesDirectoryPath, string sessionDirectoryName, string cacheDirectoryName)
        {
            this.DirectoryPath = CacheBase.GetCacheDirectoryPath(cachesDirectoryPath, sessionDirectoryName, cacheDirectoryName);
            if(!Directory.Exists(this.DirectoryPath))
            {
                Directory.CreateDirectory(this.DirectoryPath);
            }

            this.IndexFilePath = Path.Combine(this.DirectoryPath, CacheBase.IndexFileName);
            if(File.Exists(this.IndexFilePath))
            {
                this.ReadIndexFile();
            }

            this.DataDirectoryPath = Path.Combine(this.DirectoryPath, CacheBase.DataDirectoryName);
            if(!Directory.Exists(this.DataDirectoryPath))
            {
                Directory.CreateDirectory(this.DataDirectoryPath);
            }
        }

        protected CacheBase(string cacheDirectoryName)
            : this(cacheDirectoryName, CacheBase.DefaultSessionName)
        {
        }

        protected CacheBase(string cacheDirectoryName, string sessionDirectoryName)
            : this(CacheBase.DefaultCachesDirectoryPath, sessionDirectoryName, cacheDirectoryName)
        {
        }

        private void WriteIndexFile()
        {
            using (StreamWriter writer = new StreamWriter(this.IndexFilePath))
            {
                string separator = CacheBase.IndexTokenSeparator.ToString();
                foreach (var pair in this.DataFilePathsByKey)
                {
                    string line = $@"{pair.Key}{separator}{pair.Value}";
                    writer.WriteLine(line);
                }
            }
        }

        /// <summary>
        /// Flushes to the cache to disk.
        /// </summary>
        public void Flush()
        {
            this.WriteIndexFile();
        }

        private void ReadIndexFile()
        {
            using (StreamReader reader = new StreamReader(this.IndexFilePath))
            {
                char[] separators = new char[] { CacheBase.IndexTokenSeparator };
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    string[] tokens = line.Split(separators, StringSplitOptions.None);
                    string keyToken = tokens[0];
                    string dataFilePathToken = tokens[1];

                    this.DataFilePathsByKey.Add(keyToken, dataFilePathToken);
                }
            }
        }

        public bool Contains(string key)
        {
            bool output = this.DataFilePathsByKey.ContainsKey(key);
            return output;
        }

        protected string GetNewDataFilePath()
        {
            string dataFilePath = CacheBase.GetNewDataFilePath(this.DataDirectoryPath);
            return dataFilePath;
        }
    }


    /// <summary>
    /// Adds generic type methods to the base cache.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CacheBase<T> : CacheBase, ICache<T>
    {
        public T this[string key]
        {
            get
            {
                string dataFilePath = this.DataFilePathsByKey[key];

                T output = this.Deserialize(dataFilePath);
                return output;
            }
        }


        protected CacheBase(string cachesDirectoryPath, string cacheDirectoryName, string sessionDirectoryName)
            : base(cachesDirectoryPath, sessionDirectoryName, cachesDirectoryPath)
        {
        }

        protected CacheBase(string cacheDirectoryName)
            : base(cacheDirectoryName)
        {
        }

        protected CacheBase(string cacheDirectoryName, string sessionDirectoryName)
            : base(cacheDirectoryName, sessionDirectoryName)
        {
        }

        protected abstract void Serialize(string dataFilePath, T obj);

        protected abstract T Deserialize(string dataFilePath);

        public abstract T Acquire(string key);

        public void Add(string key, T obj, bool forceReplace = false)
        {
            if(this.Contains(key))
            {
                if(forceReplace)
                {
                    this.Remove(key);
                }
                else
                {
                    return; // Nothing to do.
                }
            }

            string dataFilePath = this.GetNewDataFilePath();

            this.Serialize(dataFilePath, obj);

            this.DataFilePathsByKey.Add(key, dataFilePath);
        }

        public void Remove(string key)
        {
            string dataFilePath = this.DataFilePathsByKey[key];
            File.Delete(dataFilePath);

            this.DataFilePathsByKey.Remove(key);
        }
    }


    public static class CacheExtensionMethods
    {
        public static T AcquireAndAddIfNotPresent<T>(this CacheBase<T> cacheBase, string key, bool forceReplace = false)
        {
            T acquisition;
            if (cacheBase.Contains(key))
            {
                acquisition = cacheBase[key];
            }
            else
            {
                acquisition = cacheBase.Acquire(key);
                cacheBase.Add(key, acquisition, forceReplace);
            }

            return acquisition;
        }
    }
}
