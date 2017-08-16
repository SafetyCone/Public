using System;
using System.IO;

using Public.Common.Lib.Extensions;
using Public.Common.Lib.Visuals;
using RgbColor = Public.Common.Lib.Visuals.RgbColor<byte>;


namespace Public.Common.Lib.Visuals
{
    /// <remarks>
    /// Another example of loading bitmaps can be found in the CImg header file of the http://cimg.eu/ library, cv::_load_bmp().
    /// </remarks>
    public class BitmapFileSerializer
    {
        #region Static

        public static BitmapFile DeserializeStatic(string filePath)
        {
            BitmapFile output;
            using (FileStream fStream = new FileStream(filePath, FileMode.Open))
            {
                try
                {
                    BitmapFileHeader header = BitmapFileSerializer.ReadHeader(fStream);

                    output = new BitmapFile(header);

                    BitmapFileSerializer.ReadPixelData(output, fStream);

                    bool endOfStream = -1 == fStream.ReadByte();
                    if(!endOfStream)
                    {
                        throw new FileLoadException(@"Finished loading bitmap before end of file reached.");
                    }
                }
                catch (Exception ex)
                {
                    throw new FileLoadException(@"Problem loading bitmap file.", filePath, ex);
                }
            }

            return output;
        }

        /// <summary>
        /// Note: advances the current location in the stream to the point where pixel image data begins.
        /// </summary>
        private static BitmapFileHeader ReadHeader(FileStream fStream)
        {
            // Advance the file stream in this method only, providing buffered data to sub-methods.
            int bufferLength;
            byte[] buffer;

            // Get all bytes required for the header header.
            bufferLength = BitmapFileHeaderHeader.NumberOfBytes;
            buffer = new byte[bufferLength];
            
            fStream.Read(buffer, 0, bufferLength);

            // Ensure we have a bitmap file
            BitmapFile.IsBitmapFile(buffer, true);

            BitmapFileHeaderHeader headerHeader = BitmapFileSerializer.ReadHeaderHeader(buffer);

            // Get all bytes required for the DIB header.
            bufferLength = BitmapFileDIBHeader.DefaultDIBHeaderSize;
            buffer = new byte[bufferLength];

            fStream.Read(buffer, 0, bufferLength);

            BitmapFileDIBHeader dibHeader = BitmapFileSerializer.ReadDIBHeader(buffer);

            // Advance the file stream cursor to the first pixel of the image data.
            fStream.Seek(headerHeader.OffsetToPixelData, SeekOrigin.Begin);

            // Create, validate, and return the header.
            BitmapFileHeader output = new BitmapFileHeader(headerHeader, dibHeader);

            BitmapFileSerializer.ValidateHeader(output);

            return output;
        }

        /// <summary>
        /// Note: requires the that the first two bytes of the image file contain the bitmap signature.
        /// </summary>
        private static BitmapFileHeaderHeader ReadHeaderHeader(byte[] buffer)
        {
            BitmapFileHeaderHeader output = new BitmapFileHeaderHeader();

            output.FileSize = BitConverter.ToInt32(buffer, BitmapFileHeaderHeader.FileSizeOffset);
            output.OffsetToPixelData = BitConverter.ToInt32(buffer, BitmapFileHeaderHeader.OffsetToPixelDataOffset);

            return output;
        }

        /// <remarks>
        /// Ignores any extra DIB information (like color channel bitmasks, color space parameters, color channel gamma, and ICC profile parameters).
        /// </remarks>
        private static BitmapFileDIBHeader ReadDIBHeader(byte[] buffer)
        {
            BitmapFileDIBHeader output = new BitmapFileDIBHeader();

            output.DIBHeaderSize = BitConverter.ToInt32(buffer, BitmapFileDIBHeader.DIBHeaderSizeOffset);
            output.WidthX = BitConverter.ToInt32(buffer, BitmapFileDIBHeader.WidthOffset);
            output.HeightY = BitConverter.ToInt32(buffer, BitmapFileDIBHeader.HeightOffset);
            output.NumberOfColorPlanes = BitConverterExtensions.ToInt32FromTwoBytes(buffer, BitmapFileDIBHeader.NumberOfColorPlanesOffset);
            output.BitsPerPixel = BitConverterExtensions.ToInt32FromTwoBytes(buffer, BitmapFileDIBHeader.BitsPerPixelOffset);
            output.CompressionMethod = BitConverter.ToInt32(buffer, BitmapFileDIBHeader.CompressionMethodOffset);
            output.ImageSize = BitConverter.ToInt32(buffer, BitmapFileDIBHeader.ImageSizeOffset);
            output.HorizontalResolution = BitConverter.ToInt32(buffer, BitmapFileDIBHeader.HorizontalResolutionOffset);
            output.VerticalResolution = BitConverter.ToInt32(buffer, BitmapFileDIBHeader.VerticalResolutionOffset);
            output.ColorTableColorCount = BitConverter.ToInt32(buffer, BitmapFileDIBHeader.ColorTableColorCountOffset);
            output.ImportantColorCount = BitConverter.ToInt32(buffer, BitmapFileDIBHeader.ImportantColorCountOffset);

            return output;
        }

        /// <summary>
        /// Ensure that the header contains the necessary information, and that the bitmap image is actually supported.
        /// Restrictions:
        /// Only 24 bits per pixel color.
        /// </summary>
        private static void ValidateHeader(BitmapFileHeader header)
        {
            // A laundry list 
            if(BitmapFile.bitsPerPixel24bpp != header.DIBHeader.BitsPerPixel)
            {
                throw new InvalidDataException(String.Format(@"Unsupported number of bits per pixel. Found: {0}, requires: {1}.", header.DIBHeader.BitsPerPixel, BitmapFile.bitsPerPixel24bpp));
            }
        }

        private static void ReadPixelData(BitmapFile bitmapFile, FileStream fStream)
        {
            // Each row of the width (X) direction of the bitmap is padded to make sure the row is a multiple of 4 bytes. This number of bytes greater-than-or-equal-to bits per pixel * width in pixels is called the stride.
            int bytesPerPixel = bitmapFile.Header.DIBHeader.BitsPerPixel / 8; // Assumes 24 bits per pixel.
            int bytesPerRow = bitmapFile.Header.DIBHeader.WidthX * bytesPerPixel;
            int paddingBytes = (4 - bytesPerRow % 4) % 4; // Bytes per row mod 4 is how many naked bytes there are at the end of the row, 4 minus that number is the number of padding bytes, and last modulus 4 is to map 4 to 0.
            int stride = bytesPerRow + paddingBytes;

            byte[] buffer = new byte[stride];

            for (int iRow = bitmapFile.Header.DIBHeader.HeightY - 1; iRow > -1 ; iRow--) // Data comes in serialized highest height to lowest height.
            {
                // Fill the buffer.
                fStream.Read(buffer, 0, stride);

                for (int iCol = 0; iCol < bitmapFile.Header.DIBHeader.WidthX; iCol++)
                {
                    int currentOffset = iCol * bytesPerPixel;

                    byte blue = buffer[currentOffset + 0];
                    byte green = buffer[currentOffset + 1];
                    byte red = buffer[currentOffset + 2];

                    RgbColor color = new RgbColor(red, green, blue);

                    bitmapFile[iRow, iCol] = color;
                }
            }
        }

        public static BitmapFileHeader DeserializeHeaderStatic(string filePath)
        {
            BitmapFileHeader output;
            using (FileStream fStream = new FileStream(filePath, FileMode.Open))
            {
                try
                {
                    output = BitmapFileSerializer.ReadHeader(fStream);
                }
                catch (Exception ex)
                {
                    throw new FileLoadException(@"Problem loading bitmap file.", filePath, ex);
                }
            }

            return output;
        }

        #endregion
    }
}
