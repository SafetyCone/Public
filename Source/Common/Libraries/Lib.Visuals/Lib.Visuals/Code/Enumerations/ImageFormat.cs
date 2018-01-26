using System;


namespace Public.Common.Lib.Visuals
{
    public enum ImageFormat
    {
        Bmp,
        Dat,
        Gif,
        Jpg,
        Png,
        Tiff,
        Unknown
    }


    public static class ImageFormatExtensions
    {
        private const string zBitmapDescription = @"Bitmap";
        public static string BitmapDescription { get => ImageFormatExtensions.zBitmapDescription; }
        private const string zBinaryDataDescription = @"Data";
        public static string BinaryDataDescription { get => ImageFormatExtensions.zBinaryDataDescription; }
        private const string zGifDescription = @"GIF";
        public static string GifDescription { get => ImageFormatExtensions.zGifDescription; }
        private const string zJpegDescription = @"JPEG";
        public static string JpegDescription { get => ImageFormatExtensions.zJpegDescription; }
        private const string zPngDescription = @"PNG";
        public static string PngDescription { get => ImageFormatExtensions.zPngDescription; }
        private const string zTiffDescription = @"TIFF";
        public static string TiffDescription { get => ImageFormatExtensions.zTiffDescription; }


        public static string ToDescriptionString(this ImageFormat imageFormat)
        {
            string output;
            switch (imageFormat)
            {
                case ImageFormat.Bmp:
                    output = ImageFormatExtensions.zBitmapDescription;
                    break;

                case ImageFormat.Dat:
                    output = ImageFormatExtensions.zBinaryDataDescription;
                    break;

                case ImageFormat.Gif:
                    output = ImageFormatExtensions.zGifDescription;
                    break;

                case ImageFormat.Jpg:
                    output = ImageFormatExtensions.zJpegDescription;
                    break;

                case ImageFormat.Png:
                    output = ImageFormatExtensions.zPngDescription;
                    break;

                case ImageFormat.Tiff:
                    output = ImageFormatExtensions.zTiffDescription;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<ImageFormat>(imageFormat);
            }

            return output;
        }

        public static ImageFormat FromDescription(string description)
        {
            if (!ImageFormatExtensions.TryFromDescription(description, out ImageFormat output))
            {
                throw new ArgumentException($"Unrecognized description: {description}.", nameof(description));
            }

            return output;
        }

        public static bool TryFromDescription(string description, out ImageFormat value)
        {
            bool output = true;
            switch(description)
            {
                case ImageFormatExtensions.zBitmapDescription:
                    value = ImageFormat.Bmp;
                    break;

                case ImageFormatExtensions.zBinaryDataDescription:
                    value = ImageFormat.Dat;
                    break;

                case ImageFormatExtensions.zGifDescription:
                    value = ImageFormat.Dat;
                    break;

                case ImageFormatExtensions.zJpegDescription:
                    value = ImageFormat.Jpg;
                    break;

                case ImageFormatExtensions.zPngDescription:
                    value = ImageFormat.Png;
                    break;

                case ImageFormatExtensions.zTiffDescription:
                    value = ImageFormat.Tiff;
                    break;

                default:
                    output = false;
                    value = ImageFormat.Unknown;
                    break;
            }

            return output;
        }
    }
}
