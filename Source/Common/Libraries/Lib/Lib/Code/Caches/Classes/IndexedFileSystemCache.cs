using System;
using System.IO;

using Public.Common.Lib.IO;
using PathExtensions = Public.Common.Lib.IO.Extensions.PathExtensions;
using PathUtilities = Public.Common.Lib.IO.Paths.Utilities;
using Public.Common.Lib.IO.Serialization;


namespace Public.Common.Lib
{
    /// <summary>
    /// A file-system cache that lives within a directory, using an index file to map between key objects and files stored in a sub-directory.
    /// </summary>
    /// <remarks>
    /// The single responsiblity of this class is to the manage the text index file, updating paths 
    /// </remarks>
    public class IndexedFileSystemCache<TKey, TValue> : Cache<string, string>, IPersistedCache<TKey, TValue>, IDisposable
    {
        #region Static

        public static readonly string DefaultCachesDirectoryPath = PathUtilities.DefaultCachesDirectoryPath;
        public static readonly string DefaultSessionName = @"Default";
        public static readonly string DefaultIndexFileName = @"Index.txt";
        public static readonly string DefaultFilesDirectoryName = @"Data";
        public static readonly string DefaultFilesExtension = FileExtensions.DataFileExtension;
        public static readonly string DefaultIndexTokenSeparator = @"|";


        public static string GetIndexFilePath(string directoryPath, string indexFileName)
        {
            string output = Path.Combine(directoryPath, indexFileName);
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

        ~IndexedFileSystemCache()
        {
            this.Dispose(false);
        }

        #endregion


        public string DirectoryPath { get; private set; }
        public string IndexFilePath { get; private set; }
        public string FilesDirectoryPath { get; private set; }
        /// <summary>
        /// Allow specifying the file extension of serialized cache files. This can make it easy to open these files in another program.
        /// </summary>
        public string FilesExtension { get; private set; }
        /// <summary>
        /// Allow using a different token separator if the string representation of TKey uses the default token separator.
        /// </summary>
        public string IndexTokenSeparator { get; private set; }
        public IFileSerializer<TValue> FileSerializer { get; private set; }
        public TValue this[TKey key]
        {
            get
            {
                string keyToken = this.KeyToKeyToken(key);
                string filePath = this.zValuesByKey[keyToken];

                TValue output = this.FileSerializer[filePath];
                return output;
            }
        }


        public IndexedFileSystemCache(IFileSerializer<TValue> fileSerializer)
            : this(IndexedFileSystemCache<TKey, TValue>.DefaultCachesDirectoryPath, IndexedFileSystemCache<TKey, TValue>.DefaultIndexFileName, IndexedFileSystemCache<TKey, TValue>.DefaultFilesDirectoryName, IndexedFileSystemCache<TKey, TValue>.DefaultFilesExtension, IndexedFileSystemCache<TKey, TValue>.DefaultIndexTokenSeparator,
                  fileSerializer)
        {
        }

        public IndexedFileSystemCache(CacheDirectoryPathBuilder cacheDirectoryPathBuilder, IFileSerializer<TValue> fileSerializer)
            : this(cacheDirectoryPathBuilder.ToCacheDirectoryPath(),
                  IndexedFileSystemCache<TKey, TValue>.DefaultIndexFileName, IndexedFileSystemCache<TKey, TValue>.DefaultFilesDirectoryName, IndexedFileSystemCache<TKey, TValue>.DefaultFilesExtension, IndexedFileSystemCache<TKey, TValue>.DefaultIndexTokenSeparator,
                  fileSerializer)
        {
        }

        public IndexedFileSystemCache(CacheDirectoryPathBuilder cacheDirectoryPathBuilder, string cacheDirectoryName, IFileSerializer<TValue> fileSerializer)
            : this(cacheDirectoryPathBuilder.ToCacheDirectoryPath(),
                  IndexedFileSystemCache<TKey, TValue>.DefaultIndexFileName, IndexedFileSystemCache<TKey, TValue>.DefaultFilesDirectoryName, IndexedFileSystemCache<TKey, TValue>.DefaultFilesExtension, IndexedFileSystemCache<TKey, TValue>.DefaultIndexTokenSeparator,
                  fileSerializer)
        {
        }

        public IndexedFileSystemCache(string directoryPath, string indexFileName, string filesDirectoryName, string filesExtension, string indexTokenSeparator, IFileSerializer<TValue> fileSerializer)
        {
            this.FilesExtension = filesExtension;
            this.IndexTokenSeparator = indexTokenSeparator;
            this.FileSerializer = fileSerializer;

            this.DirectoryPath = directoryPath;
            if(!Directory.Exists(this.DirectoryPath))
            {
                Directory.CreateDirectory(this.DirectoryPath);
            }

            this.IndexFilePath = IndexedFileSystemCache<TKey, TValue>.GetIndexFilePath(this.DirectoryPath, indexFileName);
            if(File.Exists(this.IndexFilePath))
            {
                this.ReadIndexFile();
            }

            this.FilesDirectoryPath = IndexedFileSystemCache<TKey, TValue>.GetFilesDirectoryPath(this.DirectoryPath, filesDirectoryName);
            if (!Directory.Exists(this.FilesDirectoryPath))
            {
                Directory.CreateDirectory(this.FilesDirectoryPath);
            }
        }

        private void ReadIndexFile()
        {
            using (StreamReader reader = new StreamReader(this.IndexFilePath))
            {
                string[] separators = new string[] { this.IndexTokenSeparator};
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
                foreach (var pair in this.zValuesByKey)
                {
                    string line = $@"{pair.Key}{separator}{pair.Value}";
                    writer.WriteLine(line);
                }
            }
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
            if(this.ContainsKey(keyToken))
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

            string filePath = IndexedFileSystemCache<TKey, TValue>.GetNewFilePath(this.FilesDirectoryPath, this.FilesExtension);

            this.FileSerializer[filePath] = value;

            this.Add(keyToken, filePath);
        }

        public bool ContainsKey(TKey key)
        {
            string keyToken = this.KeyToKeyToken(key);

            bool output = this.ContainsKey(keyToken);
            return output;
        }

        public void Persist()
        {
            this.WriteIndexFile();
        }

        public void Remove(TKey key)
        {
            string keyToken = this.KeyToKeyToken(key);
            if(this.ContainsKey(keyToken))
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
}
