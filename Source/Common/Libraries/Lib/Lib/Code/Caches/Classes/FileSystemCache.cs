using PathUtilities = Public.Common.Lib.IO.Paths.Utilities;


namespace Public.Common.Lib.Caches
{
    public class FileSystemCache
    {
        #region Static

        public static readonly string DefaultCachesDirectoryPath = PathUtilities.DefaultCachesDirectoryPath;
        public static readonly string DefaultSessionName = @"Default";

        #endregion
    }
}
