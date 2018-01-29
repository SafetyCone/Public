using SysImageFormat = System.Drawing.Imaging.ImageFormat;

using Public.Common.Lib.IO.Extensions;


namespace Public.Common.Lib.Visuals.MSWindows
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

        public static SysImageFormat DefaultImageFormat { get; } = SysImageFormat.Jpeg;
        public static string DefaultImageFileExtension { get; } = FileExtensions.JpgFileExtension;


        public static SysImageFormat DetermineImageFormatFromFileExtension(string filePath)
        {
            SysImageFormat output;

            string extension = PathExtensions.GetExtensionOnly(filePath).ToLowerInvariant();
            switch(extension)
            {
                case FileExtensions.BitmapFileExtension:
                    output = SysImageFormat.Bmp;
                    break;

                case FileExtensions.GifFileExtension:
                    output = SysImageFormat.Gif;
                    break;

                case FileExtensions.JpegFileExtension:
                case FileExtensions.JpgFileExtension:
                    output = SysImageFormat.Jpeg;
                    break;

                case FileExtensions.TiffFileExtension:
                case FileExtensions.TifFileExtension:
                    output = SysImageFormat.Tiff;
                    break;

                default:
                    output = SysImageFormat.Png;
                    break;
            }

            return output;
        }

        #endregion
    }
}
