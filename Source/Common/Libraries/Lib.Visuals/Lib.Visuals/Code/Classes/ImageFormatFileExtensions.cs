using System;


namespace Public.Common.Lib.Visuals
{
    public static class ImageFormatFileExtensions
    {
        public const string BinaryDataExtension = @"dat";
        public const string BitmapExtension = @"bmp";
        public const string JpegExtension = @"jpg";
        public const string PngExtension = @"png";


        public static readonly string[] All = new string[] { ImageFormatFileExtensions.BinaryDataExtension, ImageFormatFileExtensions.BitmapExtension, ImageFormatFileExtensions.JpegExtension, ImageFormatFileExtensions.PngExtension };
    }
}
