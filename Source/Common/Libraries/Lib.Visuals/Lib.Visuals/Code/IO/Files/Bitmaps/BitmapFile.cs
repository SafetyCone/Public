using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

using Public.Common.Lib.IO;
using Public.Common.Lib.IO.Extensions;
using Public.Common.Lib.Visuals;
using RgbColor = Public.Common.Lib.Visuals.RgbColor<byte>;


namespace Public.Common.Lib.Visuals
{
    public class BitmapFile
    {
        public const int bitsPerPixel24bpp = 24;


        #region Static

        /// <summary>
        /// Determines whether the path is to a file with the bitmap file extension.
        /// </summary>
        public static bool IsBitmapFilePath(string filePath)
        {
            bool output = PathExtensions.HasExtension(filePath, FileExtensions.BitmapFileExtension);
            return output;
        }

        /// <summary>
        /// Determines whether the file name has the bitmap file extension.
        /// </summary>
        public static bool IsBitmapFileName(string fileName)
        {
            bool output = PathExtensions.HasExtension(fileName, FileExtensions.BitmapFileExtension);
            return output;
        }

        public static bool IsBitmapFile(string filePath)
        {
            bool output;
            using (FileStream fStream = new FileStream(filePath, FileMode.Open))
            {
                output = BitmapFile.IsBitmapFile(fStream);
            }

            return output;
        }

        /// <summary>
        /// Checks that the first two bytes at the beginning of the file match the bitmap signature.
        /// NOTE: the input file stream is returned to its current position after reading the first two bytes.
        /// </summary>
        public static bool IsBitmapFile(FileStream fStream)
        {
            int bufferLength = BitmapFileHeaderHeader.SignatureByteCount;
            byte[] buffer = new byte[bufferLength];

            long initialPosition = fStream.Position; // Save for reset.
            bool wasAtStart = 0 == initialPosition;

            // Get the first two bytes of the file.
            if(!wasAtStart)
            {
                fStream.Seek(0, SeekOrigin.Begin);
            }

            fStream.Read(buffer, BitmapFileHeaderHeader.SignatureOffset, BitmapFileHeaderHeader.SignatureByteCount);

            if(!wasAtStart)
            {
                fStream.Seek(initialPosition, SeekOrigin.Begin);
            }

            // Parse the bytes to characters.
            bool output = BitmapFile.IsBitmapFile(buffer);
            return output;
        }

        public static bool IsBitmapFile(byte[] buffer, bool throwIfNot)
        {
            string fileSignature = Encoding.ASCII.GetString(buffer, BitmapFileHeaderHeader.SignatureOffset, BitmapFileHeaderHeader.SignatureByteCount);

            bool output = fileSignature == BitmapFileHeaderHeader.Signature;
            if (!output && throwIfNot)
            {
                throw new FormatException(String.Format(@"Incorrect initial signature found for a bitmap. Found '{0}', required '{1}'.", fileSignature, BitmapFileHeaderHeader.Signature));
            }

            return output;
        }

        public static bool IsBitmapFile(byte[] buffer)
        {
            bool output = BitmapFile.IsBitmapFile(buffer, false);
            return output;
        }

        public static Bitmap ConvertToMSBitmap(BitmapFile file)
        {
            int widthX = file.Header.DIBHeader.WidthX;
            int heightY = file.Header.DIBHeader.HeightY;

            Bitmap bitmap = new Bitmap(widthX, heightY, PixelFormat.Format24bppRgb);
            for (int iRow = 0; iRow < heightY; iRow++)
            {
                for (int iCol = 0; iCol < widthX; iCol++)
                {
                    RgbColor rgbColor = file[iRow, iCol];
                    Color color = ColorConversion.RgbToColor(rgbColor);
                    bitmap.SetPixel(iCol, iRow, color);
                }
            }

            return bitmap;
        }

        #endregion


        public BitmapFileHeader Header { get; set; }
        private RgbColor[,] Pixels;
        public RgbColor this[int row, int column]
        {
            get
            {
                RgbColor output = this.Pixels[row, column];
                return output;
            }
            set
            {
                this.Pixels[row, column] = value;
            }
        }


        public BitmapFile(int rows, int columns)
        {
            this.Pixels = new RgbColor[rows, columns];
        }

        public BitmapFile(BitmapFileHeader header)
            : this(header.DIBHeader.HeightY, header.DIBHeader.WidthX)
        {
            this.Header = header;
        }
    }
}
