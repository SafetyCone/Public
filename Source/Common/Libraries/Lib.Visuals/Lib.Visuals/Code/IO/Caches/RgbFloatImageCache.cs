

namespace Public.Common.Lib.Visuals
{
    public class RgbFloatImageCache : CacheBase<RgbFloatImage>
    {
        #region Static

        public static readonly string DefaultRgbFloatImageCacheDirectoryName = @"RGB Float Images";

        #endregion


        public IRgbFloatImageSerializer RgbImageSerializer { get; private set; }


        public RgbFloatImageCache(IRgbFloatImageSerializer rgbImageSerializer)
            : base(RgbFloatImageCache.DefaultRgbFloatImageCacheDirectoryName)
        {
            this.RgbImageSerializer = rgbImageSerializer;
        }

        public RgbFloatImageCache(string sessionDirectoryName, IRgbFloatImageSerializer rgbImageSerializer)
            : base(RgbFloatImageCache.DefaultRgbFloatImageCacheDirectoryName, sessionDirectoryName)
        {
            this.RgbImageSerializer = rgbImageSerializer;
        }

        public RgbFloatImageCache(string cachesDirectoryPath, string sessionDirectoryName, IRgbFloatImageSerializer rgbImageSerializer)
            : base(cachesDirectoryPath, RgbFloatImageCache.DefaultRgbFloatImageCacheDirectoryName, sessionDirectoryName)
        {
            this.RgbImageSerializer = rgbImageSerializer;
        }

        public override RgbFloatImage Acquire(string imageFilePath)
        {
            RgbFloatImage output = this.RgbImageSerializer[imageFilePath];
            return output;
        }

        protected override RgbFloatImage Deserialize(string dataFilePath)
        {
            RgbFloatImage rgbFloatImage = RgbFloatImageBinarySerializer.Deserialize(dataFilePath);
            return rgbFloatImage;
        }

        protected override void Serialize(string dataFilePath, RgbFloatImage obj)
        {
            RgbFloatImageBinarySerializer.Serialize(dataFilePath, obj);
        }
    }
}
