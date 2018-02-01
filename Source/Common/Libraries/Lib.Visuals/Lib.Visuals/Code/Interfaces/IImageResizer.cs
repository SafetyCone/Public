

namespace Public.Common.Lib.Visuals
{
    public interface IImageResizer
    {
        RgbByteImage Downsize(RgbByteImage input, int maximumDimensionSize);
    }
}
