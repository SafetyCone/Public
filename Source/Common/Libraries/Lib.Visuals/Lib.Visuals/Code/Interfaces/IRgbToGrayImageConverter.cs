

namespace Public.Common.Lib.Visuals
{
    public interface IRgbToGrayImageConverter
    {
        /// <remarks>
        /// Use an indexer as the functionality semantic, add a method as an extension method.
        /// </remarks>
        GrayFloatImage this[RgbFloatImage rgbImage] { get; }
    }


    public static class IRgbToGrayImageConverterExtensions
    {
        public static GrayFloatImage ToGray(this IRgbToGrayImageConverter converter, RgbFloatImage rgbImage)
        {
            var output = converter[rgbImage];
            return output;
        }
    }
}
