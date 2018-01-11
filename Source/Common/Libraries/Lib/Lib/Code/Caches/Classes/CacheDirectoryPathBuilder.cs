using System.IO;


namespace Public.Common.Lib
{
    public sealed class CacheDirectoryPathBuilder
    {
        public string CachesDirectoryPath { get; set; }
        public string SessionDirectoryName { get; set; }
        public string CacheDirectoryName { get; set; }


        public CacheDirectoryPathBuilder() { }

        public CacheDirectoryPathBuilder(string cachesDirectoryPath, string sessionDirectoryName)
        {
            this.CachesDirectoryPath = cachesDirectoryPath;
            this.SessionDirectoryName = sessionDirectoryName;
        }

        public CacheDirectoryPathBuilder(string cachesDirectoryPath, string sessionDirectoryName, string cacheDirectoryName)
        {
            this.CachesDirectoryPath = cachesDirectoryPath;
            this.SessionDirectoryName = sessionDirectoryName;
            this.CacheDirectoryName = cacheDirectoryName;
        }

        public CacheDirectoryPathBuilder(CacheDirectoryPathBuilder basePathBuilder, string cacheDirectoryName)
        {
            this.CachesDirectoryPath = basePathBuilder.CachesDirectoryPath;
            this.SessionDirectoryName = basePathBuilder.SessionDirectoryName;
            this.CacheDirectoryName = cacheDirectoryName;
        }

        public CacheDirectoryPathBuilder(string cacheDirectoryPath)
        {
            this.FromCacheDirectoryPath(cacheDirectoryPath);
        }

        public void FromCacheDirectoryPath(string cacheDirectoryPath)
        {
            DirectoryInfo cacheDirectory = new DirectoryInfo(cacheDirectoryPath);
            DirectoryInfo sessionDirectory = cacheDirectory.Parent;
            DirectoryInfo cachesDirectory = sessionDirectory.Parent;

            this.CachesDirectoryPath = cacheDirectory.FullName;
            this.SessionDirectoryName = sessionDirectory.Name;
            this.CacheDirectoryName = cachesDirectory.Name;
        }

        public string ToCacheDirectoryPath()
        {
            string output = Path.Combine(this.CachesDirectoryPath, this.SessionDirectoryName, this.CacheDirectoryName);
            return output;
        }

        public override string ToString()
        {
            string output = this.ToCacheDirectoryPath();
            return output;
        }
    }
}
