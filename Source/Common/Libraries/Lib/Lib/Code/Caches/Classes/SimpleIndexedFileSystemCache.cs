using System;
using System.Collections.Generic;
using System.IO;

using Public.Common.Lib.IO;
using PathExtensions = Public.Common.Lib.IO.Extensions.PathExtensions;
using Public.Common.Lib.IO.Serialization;


namespace Public.Common.Lib
{
    public abstract class SimpleIndexedFileSystemCache : Cache<string, string>, IDisposable, IPersisted
    {
        #region Static

        public static readonly string DefaultIndexFileName = @"Index.txt";
        public static readonly string DefaultFilesDirectoryName = @"Data";
        public static readonly string DefaultFilesExtension = FileExtensions.DataFileExtension;
        public static readonly string DefaultIndexTokenSeparator = @"|";


        public static string GetIndexFilePath(string cacheDirectoryPath, string indexFileName)
        {
            string output = Path.Combine(cacheDirectoryPath, indexFileName);
            return output;
        }

        public static string GetFilesDirectoryPath(string directoryPath, string filesDirectoryName)
        {
            string output = Path.Combine(directoryPath, filesDirectoryName);
            return output;
        }

        public static string GetNewFilePath(string filesDirectoryPath, string fileExtenstion)
        {
            string guidStr = Guid.NewGuid().ToString().ToUpperInvariant();

            string fileName = $@"{guidStr}{PathExtensions.WindowsFileExtensionSeparatorChar}{fileExtenstion}";

            string dataFilePath = Path.Combine(filesDirectoryPath, fileName);
            return dataFilePath;
        }

        public static Dictionary<string, string> ReadIndexFilePath(string indexFilePath, string indexTokenSeparator)
        {
            var output = new Dictionary<string, string>();
            using (StreamReader reader = new StreamReader(indexFilePath))
            {
                string[] separators = new string[] { indexTokenSeparator };
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    string[] tokens = line.Split(separators, StringSplitOptions.None);
                    string keyToken = tokens[0];
                    string filePath = tokens[1];

                    output.Add(keyToken, filePath);
                }
            }

            return output;
        }

        public static Dictionary<string, string> ReadIndexFilePath(string indexFilePath)
        {
            var output = SimpleIndexedFileSystemCache.ReadIndexFilePath(indexFilePath, SimpleIndexedFileSystemCache.DefaultIndexTokenSeparator);
            return output;
        }

        public static Dictionary<string, string> ReadIndexFile(string cacheDirectoryPath, string indexFileName, string indexTokenSeparator)
        {
            string indexFilePath = SimpleIndexedFileSystemCache.GetIndexFilePath(cacheDirectoryPath, indexFileName);

            var output = SimpleIndexedFileSystemCache.ReadIndexFilePath(indexFilePath, indexTokenSeparator);
            return output;
        }

        public static Dictionary<string, string> ReadIndexFile(string cacheDirectoryPath, string indexTokenSeparator)
        {
            var output = SimpleIndexedFileSystemCache.ReadIndexFile(cacheDirectoryPath, SimpleIndexedFileSystemCache.DefaultIndexFileName, indexTokenSeparator);
            return output;
        }

        public static Dictionary<string, string> ReadIndexFile(string cacheDirectoryPath)
        {
            var output = SimpleIndexedFileSystemCache.ReadIndexFile(cacheDirectoryPath, SimpleIndexedFileSystemCache.DefaultIndexFileName, SimpleIndexedFileSystemCache.DefaultIndexTokenSeparator);
            return output;
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

        ~SimpleIndexedFileSystemCache()
        {
            this.Dispose(false);
        }

        #endregion


        public string DirectoryPath { get; }
        public string IndexFilePath { get; }
        public string FilesDirectoryPath { get; }
        /// <summary>
        /// Allow specifying the file extension of serialized cache files. This can make it easy to open these files in another program.
        /// </summary>
        public string FilesExtension { get; }
        /// <summary>
        /// Allow using a different token separator if the string representation of TKey uses the default token separator.
        /// </summary>
        public string IndexTokenSeparator { get; }


        public SimpleIndexedFileSystemCache(string directoryPath, string indexFileName, string filesDirectoryName, string filesExtension, string indexTokenSeparator)
        {
            this.FilesExtension = filesExtension;
            this.IndexTokenSeparator = indexTokenSeparator;

            this.DirectoryPath = directoryPath;
            if (!Directory.Exists(this.DirectoryPath))
            {
                Directory.CreateDirectory(this.DirectoryPath);
            }

            this.IndexFilePath = SimpleIndexedFileSystemCache.GetIndexFilePath(this.DirectoryPath, indexFileName);
            if (File.Exists(this.IndexFilePath))
            {
                this.ReadIndexFile();
            }

            this.FilesDirectoryPath = SimpleIndexedFileSystemCache.GetFilesDirectoryPath(this.DirectoryPath, filesDirectoryName);
            if (!Directory.Exists(this.FilesDirectoryPath))
            {
                Directory.CreateDirectory(this.FilesDirectoryPath);
            }
        }

        public SimpleIndexedFileSystemCache(string directoryPath)
            : this(directoryPath, SimpleIndexedFileSystemCache.DefaultIndexFileName, SimpleIndexedFileSystemCache.DefaultFilesDirectoryName, SimpleIndexedFileSystemCache.DefaultFilesExtension, SimpleIndexedFileSystemCache.DefaultIndexTokenSeparator)
        {
        }

        private void ReadIndexFile()
        {
            using (StreamReader reader = new StreamReader(this.IndexFilePath))
            {
                string[] separators = new string[] { this.IndexTokenSeparator };
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    string[] tokens = line.Split(separators, StringSplitOptions.None);
                    string keyToken = tokens[0];
                    string filePath = tokens[1];

                    this.Add(keyToken, filePath);
                }
            }
        }

        private void WriteIndexFile()
        {
            using (StreamWriter writer = new StreamWriter(this.IndexFilePath))
            {
                string separator = this.IndexTokenSeparator;
                foreach (var pair in this.ValuesByKey)
                {
                    string line = $@"{pair.Key}{separator}{pair.Value}";
                    writer.WriteLine(line);
                }
            }
        }

        public void Persist()
        {
            this.WriteIndexFile();
        }
    }



    /// <summary>
    /// A file-system cache that lives within a directory, using an index file to map between key objects and files stored in a sub-directory.
    /// </summary>
    /// <remarks>
    /// The single responsiblity of this class is to the manage the text index file, updating paths 
    /// </remarks>
    public class SimpleIndexedFileSystemCache<TKey, TValue> : SimpleIndexedFileSystemCache, ICache<TKey, TValue>
    {
        public IFileSerializer<TValue> FileSerializer { get; }
        public TValue this[TKey key]
        {
            get
            {
                string keyToken = this.KeyToKeyToken(key);
                string filePath = this.ValuesByKey[keyToken];

                TValue output = this.FileSerializer[filePath];
                return output;
            }
        }


        public SimpleIndexedFileSystemCache(string directoryPath, string indexFileName, string filesDirectoryName, string filesExtension, string indexTokenSeparator, IFileSerializer<TValue> fileSerializer)
            : base(directoryPath, indexFileName, filesDirectoryName, filesExtension, indexTokenSeparator)
        {
            this.FileSerializer = fileSerializer;
        }

        public SimpleIndexedFileSystemCache(string directoryPath, IFileSerializer<TValue> fileSerializer)
            : this(directoryPath, SimpleIndexedFileSystemCache.DefaultIndexFileName, SimpleIndexedFileSystemCache.DefaultFilesDirectoryName, SimpleIndexedFileSystemCache.DefaultFilesExtension, SimpleIndexedFileSystemCache.DefaultIndexTokenSeparator, fileSerializer)
        {
        }

        public SimpleIndexedFileSystemCache(string directoryPath, IFileSerializer<TValue> fileSerializer, string filesExtension)
            : this(directoryPath, SimpleIndexedFileSystemCache.DefaultIndexFileName, SimpleIndexedFileSystemCache.DefaultFilesDirectoryName, filesExtension, SimpleIndexedFileSystemCache.DefaultIndexTokenSeparator, fileSerializer)
        {
        }

        /// <summary>
        /// Allow overriding TKey key to string key token conversion. Default is ToString().
        /// </summary>
        protected virtual string KeyToKeyToken(TKey key)
        {
            string output = key.ToString();
            return output;
        }

        public void Add(TKey key, TValue value, bool forceReplace = false)
        {
            string keyToken = this.KeyToKeyToken(key);
            if (this.ContainsKey(keyToken))
            {
                if (forceReplace)
                {
                    this.Remove(key);
                }
                else
                {
                    // Conform to the default dictionary behavior - adding the same key twice is an error.
                    this.Add(keyToken, null); // Will error.
                }
            }

            string filePath = SimpleIndexedFileSystemCache.GetNewFilePath(this.FilesDirectoryPath, this.FilesExtension);

            this.FileSerializer[filePath] = value;

            this.Add(keyToken, filePath);
        }

        public bool ContainsKey(TKey key)
        {
            string keyToken = this.KeyToKeyToken(key);

            bool output = this.ContainsKey(keyToken);
            return output;
        }

        public void Remove(TKey key)
        {
            string keyToken = this.KeyToKeyToken(key);
            if (this.ContainsKey(keyToken))
            {
                string filePath = this[keyToken];
                File.Delete(filePath);
            }

            this.Remove(keyToken);
        }

        public TValue GetValue(TKey key)
        {
            TValue output = this[key];
            return output;
        }
    }


    /// <summary>
    /// A file-system cache that lives within a directory, using an index file to map between key objects and files stored in a sub-directory.
    /// </summary>
    /// <remarks>
    /// The single responsiblity of this class is to the manage the text index file, updating paths 
    /// </remarks>
    public class SimpleIndexedFileSystemCache<TKey1, TKey2, TValue> : SimpleIndexedFileSystemCache, ICache<TKey1, TKey2, TValue>
    {
        public static string DefaultStringCombinationSeparator { get; } = @"<>";


        public string StringCombinationSeparator { get; }
        public IFileSerializer<TValue> FileSerializer { get; }
        public TValue this[TKey1 key1, TKey2 key2]
        {
            get
            {
                string keyToken = this.KeysToKeyToken(key1, key2);
                string filePath = this.ValuesByKey[keyToken];

                TValue output = this.FileSerializer[filePath];
                return output;
            }
        }


        public SimpleIndexedFileSystemCache(string directoryPath, string indexFileName, string filesDirectoryName, string filesExtension, string indexTokenSeparator, string stringCombinationSeparator, IFileSerializer<TValue> fileSerializer)
            : base(directoryPath, indexFileName, filesDirectoryName, filesExtension, indexTokenSeparator)
        {
            this.StringCombinationSeparator = stringCombinationSeparator;
            this.FileSerializer = fileSerializer;
        }

        public SimpleIndexedFileSystemCache(string directoryPath, IFileSerializer<TValue> fileSerializer)
            : this(directoryPath, SimpleIndexedFileSystemCache.DefaultIndexFileName, SimpleIndexedFileSystemCache.DefaultFilesDirectoryName, SimpleIndexedFileSystemCache.DefaultFilesExtension, SimpleIndexedFileSystemCache.DefaultIndexTokenSeparator, SimpleIndexedFileSystemCache<TKey1, TKey2, TValue>.DefaultStringCombinationSeparator, fileSerializer)
        {
        }

        /// <summary>
        /// Allow overriding TKey key to string key token conversion. Default is ToString().
        /// </summary>
        protected virtual string KeysToKeyToken(TKey1 key1, TKey2 key2)
        {
            string output = key1.ToString() + this.StringCombinationSeparator + key2.ToString();
            return output;
        }

        public void Add(TKey1 key1, TKey2 key2, TValue value, bool forceReplace = false)
        {
            string keyToken = this.KeysToKeyToken(key1, key2);
            if (this.ContainsKey(keyToken))
            {
                if (forceReplace)
                {
                    this.Remove(keyToken);
                }
                else
                {
                    // Conform to the default dictionary behavior - adding the same key twice is an error.
                    this.Add(keyToken, null); // Will error.
                }
            }

            string filePath = SimpleIndexedFileSystemCache.GetNewFilePath(this.FilesDirectoryPath, this.FilesExtension);

            this.FileSerializer[filePath] = value;

            this.Add(keyToken, filePath);
        }

        public bool ContainsKey(TKey1 key1, TKey2 key2)
        {
            string keyToken = this.KeysToKeyToken(key1, key2);

            bool output = this.ContainsKey(keyToken);
            return output;
        }

        public void Remove(TKey1 key1, TKey2 key2)
        {
            string keyToken = this.KeysToKeyToken(key1, key2);
            if (this.ContainsKey(keyToken))
            {
                string filePath = this[keyToken];
                File.Delete(filePath);
            }

            this.Remove(keyToken);
        }

        public TValue GetValue(TKey1 key1, TKey2 key2)
        {
            TValue output = this[key1, key2];
            return output;
        }
    }


    public static class SimpleIndexedFileSystemCacheExtensions
    {
        public static string GetKeyTokenFromKey1Key2<TKey1, TKey2, TValue>(this SimpleIndexedFileSystemCache<TKey1, TKey2, TValue> cache, TKey1 key1, TKey2 key2)
        {
            string keyToken = key1.ToString() + cache.StringCombinationSeparator + key2.ToString();
            return keyToken;
        }

        public static Tuple<string, string> GetKey1Key2FromKeyToken<TKey1, TKey2, TValue>(this SimpleIndexedFileSystemCache<TKey1, TKey2, TValue> cache, string keyToken)
        {
            string[] keys = keyToken.Split(new string[] { cache.StringCombinationSeparator }, StringSplitOptions.None);

            var output = Tuple.Create(keys[0], keys[1]);
            return output;
        }
    }
}
