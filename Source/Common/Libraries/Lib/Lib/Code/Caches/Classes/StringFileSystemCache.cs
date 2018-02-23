//using System;
//using System.Collections.Generic;
//using System.IO;


//namespace Public.Common.Lib.Caches
//{
//    /// <summary>
//    /// A cache for string-string pairs, persisted as a file.
//    /// </summary>
//    /// <remarks>
//    /// This is a Dictionary(string, string) that is saved to a file.
//    /// </remarks>
//    public class StringFileSystemCache : Cache<string, string>, IDisposable
//    {
//        #region Static

//        public static string DefaultFileName = @"StringStringPairs.txt";
//        public static readonly string DefaultFileTokenSeparator = @"|";

//        #endregion

//        #region IDisposable

//        private bool zDisposed = false;


//        public void Dispose()
//        {
//            this.Dispose(true);

//            GC.SuppressFinalize(this);
//        }

//        protected virtual void Dispose(bool disposing)
//        {
//            if (!this.zDisposed)
//            {
//                if (disposing)
//                {
//                    // Clean-up managed resource here.
//                }

//                // Clean-up unmanaged resources here.
//                this.WriteFile(); // Note, even though the file is written with all managed code, since it is a file it is an unmanaged resourece.
//            }

//            this.zDisposed = true;
//        }

//        ~StringFileSystemCache()
//        {
//            this.Dispose(false);
//        }

//        #endregion


//        public string FilePath { get; }
//        public string FileTokenSeparator { get; }


//        public StringFileSystemCache(string filePath, string fileTokenSeparator)
//        {
//            this.FilePath = filePath;
//            this.FileTokenSeparator = fileTokenSeparator;

//            if(File.Exists(this.FilePath))
//            {
//                this.ReadFile();
//            }
//        }

//        private void ReadFile()
//        {
//            string[] lines = File.ReadAllLines(this.FilePath);

//            string[] separators = new string[] { this.FileTokenSeparator };
//            foreach(var line in lines)
//            {
//                string[] tokens = line.Split(separators, StringSplitOptions.None);
//                string keyToken = tokens[0];
//                string filePath = tokens[1];

//                this.Add(keyToken, filePath);
//            }
//        }

//        private void WriteFile()
//        {
//            var lines = new List<string>();
//            foreach(var pair in this.ValuesByKey)
//            {
//                string line = $@"{pair.Key}{this.FileTokenSeparator}{pair.Value}";
//                lines.Add(line);
//            }

//            File.WriteAllLines(this.FilePath, lines);
//        }
//    }
//}
