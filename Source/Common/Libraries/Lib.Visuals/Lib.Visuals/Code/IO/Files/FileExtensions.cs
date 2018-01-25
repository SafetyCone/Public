using System.Drawing.Imaging;

using Public.Common.Lib.IO.Extensions;


namespace Public.Common.Lib.Visuals.IO
{
    public static class FileExtensions
    {
        public const string BitmapFileExtension = @"bmp";
        public const string GifFileExtension = @"gif";
        public const string JpgFileExtension = @"jpg";
        public const string JpegFileExtension = @"jpeg";
        public const string TifFileExtension = @"tif";
        public const string TiffFileExtension = @"tiff";


        #region Static

        public static ImageFormat DetermineImageFormatFromFileExtension(string filePath)
        {
            ImageFormat output;

            string extension = PathExtensions.GetExtensionOnly(filePath).ToLowerInvariant();
            switch(extension)
            {
                case FileExtensions.BitmapFileExtension:
                    output = ImageFormat.Bmp;
                    break;

                case FileExtensions.GifFileExtension:
                    output = ImageFormat.Gif;
                    break;

                case FileExtensions.JpegFileExtension:
                case FileExtensions.JpgFileExtension:
                    output = ImageFormat.Jpg;
                    break;

                case FileExtensions.TiffFileExtension:
                case FileExtensions.TifFileExtension:
                    output = ImageFormat.Tiff;
                    break;

                default:
                    output = ImageFormat.Png;
                    break;
            }

            return output;
        }

        #endregion
    }
}
