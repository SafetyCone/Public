using SysImageFormat = System.Drawing.Imaging.ImageFormat;

using Public.Common.Lib.IO.Extensions;
using VisualFileExtensions = Public.Common.Lib.Visuals.IO.FileExtensions;


namespace Public.Common.Lib.Visuals.MSWindows
{
    public static class FileExtensions
    {
        #region Static

        public static SysImageFormat DefaultImageFormat { get; } = SysImageFormat.Jpeg;
        public static string DefaultImageFileExtension { get; } = VisualFileExtensions.JpgFileExtension;


        public static SysImageFormat DetermineImageFormatFromFileExtension(string filePath)
        {
            SysImageFormat output;

            string extension = PathExtensions.GetExtensionOnly(filePath).ToLowerInvariant();
            switch(extension)
            {
                case VisualFileExtensions.BitmapFileExtension:
                    output = SysImageFormat.Bmp;
                    break;

                case VisualFileExtensions.GifFileExtension:
                    output = SysImageFormat.Gif;
                    break;

                case VisualFileExtensions.JpegFileExtension:
                case VisualFileExtensions.JpgFileExtension:
                    output = SysImageFormat.Jpeg;
                    break;

                case VisualFileExtensions.TiffFileExtension:
                case VisualFileExtensions.TifFileExtension:
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
