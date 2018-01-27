using System.IO;


namespace Public.Common.Lib
{
    /// <summary>
    /// Contains static helper methods for data file caches.
    /// </summary>
    public static class DataFileCache
    {
        public static string GetCacheDirectoryPath(string generalCacheDirectoryPath, string cacheDirectoryName)
        {
            string output = Path.Combine(generalCacheDirectoryPath, cacheDirectoryName);
            return output;
        }
    }
}
