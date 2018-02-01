

namespace Public.Common.Lib.Visuals
{
    public interface IRgbByteToRgbFloatImageConverter
    {
        RgbFloatImage this[RgbByteImage byteImage] { get; }
    }
}
