using Public.Common.Lib.Caches;
using Public.Common.Lib.IO.Serialization;
using Public.Common.Lib.Visuals.IO.Serialization;


namespace Public.Common.Lib.Visuals
{
    public class RgbFloatImageFileCache : AcquiringIndexedFileSystemCacheBase<string, RgbFloatImage>
    {
        #region Static

        public static readonly string DefaultCacheDirectoryName = @"RGB Float Images";


        public static IndexedFileSystemCache<string, RgbFloatImage> GetIndexedFileSystemCache(CacheDirectoryPathBuilder cacheDirectoryPathBuilder, string cacheDirectoryName)
        {
            RgbFloatImageBinarySerializer binaryFileSerializer = new RgbFloatImageBinarySerializer();
            CacheDirectoryPathBuilder pathBuilder = new CacheDirectoryPathBuilder(cacheDirectoryPathBuilder, cacheDirectoryName);

            IndexedFileSystemCache<string, RgbFloatImage> output = new IndexedFileSystemCache<string, RgbFloatImage>(pathBuilder, binaryFileSerializer);
            return output;
        }

        #endregion


        public IRgbFloatImageSerializer ImageSerializer { get; private set; }


        public RgbFloatImageFileCache(CacheDirectoryPathBuilder cacheDirectoryPathBuilder, IRgbFloatImageSerializer imageSerializer)
            : base(RgbFloatImageFileCache.GetIndexedFileSystemCache(cacheDirectoryPathBuilder, RgbFloatImageFileCache.DefaultCacheDirectoryName))
        {
            this.ImageSerializer = imageSerializer;
        }

        public override RgbFloatImage Acquire(string imageFilePath)
        {
            RgbFloatImage output = this.ImageSerializer.Deserialize(imageFilePath);
            return output;
        }
    }
}
