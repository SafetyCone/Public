using System;


namespace Public.Common.Lib.IO.Paths
{
    public static class Utilities
    {
        /// <summary>
        /// Allow changing the default directory path used for all caches.
        /// </summary>
        /// <remarks>
        /// NOT thread-safe.
        /// </remarks>
        public static string DefaultCachesDirectoryPath { get; set; } = @"E:\temp\Caches";


        public static string MyDocumentsDirectoryPath
        {
            get
            {
                string output = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                return output;
            }
        }
    }
}
