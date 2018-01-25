

namespace Public.Common.Lib.Visuals
{
    public class GrayFloatImageFileCache : AcquiringIndexedFileSystemCacheBase<string, GrayFloatImage>
    {
        #region Static

        public static readonly string DefaultCacheDirectoryName = @"Gray Float Images";


        public static IndexedFileSystemCache<string, GrayFloatImage> GetIndexedFileSystemCache(CacheDirectoryPathBuilder cacheDirectoryPathBuilder, string cacheDirectoryName)
        {
            var binaryFileSerializer = new GrayFloatImageBinarySerializer();
            var pathBuilder = new CacheDirectoryPathBuilder(cacheDirectoryPathBuilder, cacheDirectoryName);

            var output = new IndexedFileSystemCache<string, GrayFloatImage>(pathBuilder, binaryFileSerializer);
            return output;
        }

        #endregion


        public ICache<string, RgbFloatImage> RgbFloatImageCache { get; private set; }
        public IRgbToGrayImageConverter Converter { get; private set; }


        public GrayFloatImageFileCache(CacheDirectoryPathBuilder cacheDirectoryPathBuilder, ICache<string, RgbFloatImage> rgbFloatImageCache, IRgbToGrayImageConverter converter)
            : base(GrayFloatImageFileCache.GetIndexedFileSystemCache(cacheDirectoryPathBuilder, GrayFloatImageFileCache.DefaultCacheDirectoryName))
        {
            this.RgbFloatImageCache = rgbFloatImageCache;
            this.Converter = converter;
        }

        public override GrayFloatImage Acquire(string imageFilePath)
        {
            RgbFloatImage rgbImage = this.RgbFloatImageCache[imageFilePath];

            GrayFloatImage grayImage = this.Converter[rgbImage];
            return grayImage;
        }
    }
}
