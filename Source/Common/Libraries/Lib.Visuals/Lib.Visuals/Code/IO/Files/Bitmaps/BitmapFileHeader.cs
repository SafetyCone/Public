using System;


namespace Public.Common.Lib.Visuals
{
    /// <summary>
    /// Contains information describing the bitmap file, and the pixel data within.
    /// </summary>
    /// <remarks>
    /// An excellent description of the bitmap file header is here: https://en.wikipedia.org/wiki/BMP_file_format.
    /// </remarks>
    public class BitmapFileHeader
    {
        public BitmapFileHeaderHeader HeaderHeader { get; set; }
        public BitmapFileDIBHeader DIBHeader { get; set; }
        //public BitmapFileColorTable ColorTable { get; set; } // Ignore the color table.


        public BitmapFileHeader()
        {
        }

        public BitmapFileHeader(BitmapFileHeaderHeader headerHeader, BitmapFileDIBHeader dibHeader)
        {
            this.HeaderHeader = headerHeader;
            this.DIBHeader = dibHeader;
        }
    }
}
