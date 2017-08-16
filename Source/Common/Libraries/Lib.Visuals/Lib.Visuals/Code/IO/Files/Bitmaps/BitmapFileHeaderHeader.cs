using System;


namespace Public.Common.Lib.Visuals
{
    /// <summary>
    /// Represents the header of the bitmap file header, which comes before device independent bitmap file header and color table.
    /// </summary>
    public class BitmapFileHeaderHeader
    {
        // The first two bytes of the bitmap file will be the signature characters 'B' (66) and 'M' (77).
        public const string Signature = @"BM";
        public const int NumberOfBytes = 14; // 4 each for the size of the file, and the offset to the pixel data. 2 each for the initial signature, reserved value 1 (unused), and reserved value 2 (unused).
        public const int SignatureOffset = 0;
        public const int SignatureByteCount = 2;
        public const int FileSizeOffset = 2;
        public const int OffsetToPixelDataOffset = 10;

        /// <summary>
        /// The size of the entire bitmap file.
        /// </summary>
        public int FileSize { get; set; }
        /// <summary>
        /// The offset from the start of the file to the first byte of the pixel data.
        /// </summary>
        public int OffsetToPixelData { get; set; }


        public BitmapFileHeaderHeader()
        {
        }

        public BitmapFileHeaderHeader(int fileSize, int offsetToPixelData)
        {
            this.FileSize = fileSize;
            this.OffsetToPixelData = offsetToPixelData;
        }
    }
}
