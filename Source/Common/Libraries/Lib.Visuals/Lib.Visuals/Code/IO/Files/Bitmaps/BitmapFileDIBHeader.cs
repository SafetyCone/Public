using System;


namespace Public.Common.Lib.Visuals
{
    /// <summary>
    /// Represents the device independent bitmap header data.
    /// </summary>
    /// <remarks>
    /// This follows the Windows BITMAPINFOHEADER format.
    /// 
    /// Note that many of the properties of the header date from an earlier time when full color was not generally available and now generally accepted use cases for bitmaps had not yet been established.
    /// For example, bitmaps may have been a contender for schematics as values for pixels per meter resolutions are available for use.
    /// Also, the color table is generally not used now that all displays can display full 24-bit color. But the table was previously used for optimization of display on limited devices and conversion between limited color palettes.
    /// </remarks>
    public class BitmapFileDIBHeader
    {
        public const int DefaultDIBHeaderSize = 40; // All fields are 4 bytes except the number of color planes and bits per pixel.
        public const int DIBHeaderSizeOffset = 0;
        public const int WidthOffset = 4;
        public const int HeightOffset = 8;
        public const int NumberOfColorPlanesOffset = 12;
        public const int BitsPerPixelOffset = 14;
        public const int CompressionMethodOffset = 16;
        public const int ImageSizeOffset = 20;
        public const int HorizontalResolutionOffset = 24;
        public const int VerticalResolutionOffset = 28;
        public const int ColorTableColorCountOffset = 32;
        public const int ImportantColorCountOffset = 36;


        /// <summary>
        /// Size of the header. Generally 40 bytes, but can be larger.
        /// </summary>
        public int DIBHeaderSize { get; set; }
        /// <summary>
        /// The width (X dimension) of the bitmap image in pixels.
        /// </summary>
        public int WidthX { get; set; }
        /// <summary>
        /// The height (Y dimension) of the bitmap image in pixels.
        /// </summary>
        public int HeightY { get; set; }
        /// <summary>
        /// The number of color planes in the bitmap image. This is empirically found to always be 1.
        /// </summary>
        /// <remarks>
        /// This value is specified by 2 bytes, not the usual 4.
        /// </remarks>
        public int NumberOfColorPlanes { get; set; }
        /// <summary>
        /// The number of bits specifying the color for each pixel. Typical values are 1, 4, 8, 16, 24, and 32.
        /// Only 24 is supported.
        /// </summary>
        /// /// <remarks>
        /// This value is specified by 2 bytes, not the usual 4.
        /// </remarks>
        public int BitsPerPixel { get; set; }
        /// <summary>
        /// The compression method applied to the pixel data. Generally zero (0), the value corresponding to BI_RGB which is no compression.
        /// </summary>
        public int CompressionMethod { get; set; }
        /// <summary>
        /// The size of the raw bitmap data, can have a dummy value of 0 when the no compression BI_RGB method is used.
        /// </summary>
        public int ImageSize { get; set; }
        /// <summary>
        /// The horizontal resolution of the image in pixels per meter.
        /// </summary>
        public int HorizontalResolution { get; set; }
        /// <summary>
        /// The horizontal resolution of the image in pixels per meter.
        /// </summary>
        public int VerticalResolution { get; set; }
        /// <summary>
        /// The number of colors in the color table. Zero (0) if a color table is not used.
        /// </summary>
        public int ColorTableColorCount { get; set; }
        /// <summary>
        /// The number of important colors in the image, generally a dummy value of zero (0) since all colors are important.
        /// </summary>
        public int ImportantColorCount { get; set; }


        public BitmapFileDIBHeader()
        {
        }

        /// <summary>
        /// The preferred constructor. Only header size, width, height, and bits per pixel are generally needed to load a bitmap file.
        /// </summary>
        public BitmapFileDIBHeader(int dibHeaderSize, int widthX, int heightY, int bitsPerPixel)
        {
            this.DIBHeaderSize = dibHeaderSize;
            this.WidthX = widthX;
            this.HeightY = heightY;
            this.BitsPerPixel = bitsPerPixel;
        }
    }
}
