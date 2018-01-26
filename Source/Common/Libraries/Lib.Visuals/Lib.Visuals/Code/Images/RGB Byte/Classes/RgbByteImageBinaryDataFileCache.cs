using Public.Common.Lib.Visuals.IO.Serialization;


namespace Public.Common.Lib.Visuals.IO
{
    public class RgbByteImageBinaryDataFileCache<TKey> : SimpleIndexedFileSystemCache<TKey, RgbByteImage>
    {
        public RgbByteImageBinaryDataFileCache(string directoryPath)
            : base(directoryPath, new RgbByteImageBinarySerializer())
        {
        }
    }

}
