


namespace Public.Common.Lib.Visuals
{
    public class GrayFloatImageCache : CacheBase<GrayFloatImage>
    {
        #region Static

        public static readonly string DefaultRgbFloatImageCacheDirectoryName = @"Gray Float Images";
        public static readonly IRgbToGrayImageConverter DefaultConverter = new Rec709FloatImageConverter();

        #endregion


        #region IDisposable

        private bool zDisposed = false;


        protected override void Dispose(bool disposing)
        {
            if (!this.zDisposed)
            {
                if (disposing)
                {
                    // Clean-up managed resource here.
                    this.RgbCache.Dispose();
                }

                // Clean-up unmanaged resources here
            }

            base.Dispose(disposing);

            this.zDisposed = true;
        }

        #endregion


        public RgbFloatImageCache RgbCache { get; private set; }
        public IRgbToGrayImageConverter Converter { get; private set; }


        public GrayFloatImageCache(RgbFloatImageCache rgbCache)
            : this(rgbCache, GrayFloatImageCache.DefaultConverter)
        {
        }

        public GrayFloatImageCache(RgbFloatImageCache rgbCache, IRgbToGrayImageConverter converter)
            : base(GrayFloatImageCache.DefaultRgbFloatImageCacheDirectoryName)
        {
            this.RgbCache = rgbCache;
            this.Converter = converter;
        }

        public GrayFloatImageCache(string sessionDirectoryName, RgbFloatImageCache rgbCache)
            : this(sessionDirectoryName, rgbCache, GrayFloatImageCache.DefaultConverter)
        {
        }

        public GrayFloatImageCache(string sessionDirectoryName, RgbFloatImageCache rgbCache, IRgbToGrayImageConverter converter)
            : base(GrayFloatImageCache.DefaultRgbFloatImageCacheDirectoryName, sessionDirectoryName)
        {
            this.RgbCache = rgbCache;
            this.Converter = converter;
        }

        public GrayFloatImageCache(string cachesDirectoryPath, string sessionDirectoryName, RgbFloatImageCache rgbCache)
            : this(cachesDirectoryPath, sessionDirectoryName, rgbCache, GrayFloatImageCache.DefaultConverter)
        {
        }

        public GrayFloatImageCache(string cachesDirectoryPath, string sessionDirectoryName, RgbFloatImageCache rgbCache, IRgbToGrayImageConverter converter)
            : base(cachesDirectoryPath, GrayFloatImageCache.DefaultRgbFloatImageCacheDirectoryName, sessionDirectoryName)
        {
            this.RgbCache = rgbCache;
            this.Converter = converter;
        }

        public override GrayFloatImage Acquire(string key)
        {
            RgbFloatImage rgbImage = this.RgbCache.AcquireAndAddIfNotPresent(key);

            GrayFloatImage output = this.Converter.ToGray(rgbImage);
            return output;
        }

        protected override GrayFloatImage Deserialize(string dataFilePath)
        {
            GrayFloatImage grayFloatImage = GrayFloatImageBinarySerializer.Deserialize(dataFilePath);
            return grayFloatImage;
        }

        protected override void Serialize(string dataFilePath, GrayFloatImage obj)
        {
            GrayFloatImageBinarySerializer.Serialize(dataFilePath, obj);
        }
    }
}
