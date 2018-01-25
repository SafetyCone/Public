using System;
using System.Linq;

using Public.Common.Lib;
using PathExtensions = Public.Common.Lib.IO.Extensions.PathExtensions;
using Public.Common.Lib.IO.Serialization;


namespace Public.Common.Lib.Visuals
{
    public interface IRgbFloatImageSerializer : IFileSerializer<RgbFloatImage>
    {
        ImageFormat[] SupportedImageFormats { get; }
    }


    public static class IRgbFloatImageSerializerExtensions
    {
        public static string ToFileExtension(this ImageFormat imageFormat)
        {
            string output;
            switch (imageFormat)
            {
                case ImageFormat.Bmp:
                    output = ImageFormatFileExtensions.BitmapExtension;
                    break;

                case ImageFormat.Dat:
                    output = ImageFormatFileExtensions.BinaryDataExtension;
                    break;

                case ImageFormat.Jpg:
                    output = ImageFormatFileExtensions.JpegExtension;
                    break;

                case ImageFormat.Png:
                    output = ImageFormatFileExtensions.PngExtension;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<ImageFormat>(imageFormat);
            }

            return output;
        }

        public static ImageFormat FromFileExtension(string fileExtension)
        {
            if (!IRgbFloatImageSerializerExtensions.TryFromFileExtension(fileExtension, out ImageFormat output))
            {
                throw new ArgumentException($"Unrecognized description: {fileExtension}.", nameof(fileExtension));
            }

            return output;
        }

        public static bool TryFromFileExtension(string fileExtension, out ImageFormat value)
        {
            string lowerFileExtension = fileExtension.ToLowerInvariant();

            bool output = true;
            switch (lowerFileExtension)
            {
                case ImageFormatFileExtensions.BitmapExtension:
                    value = ImageFormat.Bmp;
                    break;

                case ImageFormatFileExtensions.BinaryDataExtension:
                    value = ImageFormat.Dat;
                    break;

                case ImageFormatFileExtensions.JpegExtension:
                    value = ImageFormat.Jpg;
                    break;

                case ImageFormatFileExtensions.PngExtension:
                    value = ImageFormat.Png;
                    break;

                default:
                    output = false;
                    value = ImageFormat.Unknown;
                    break;
            }

            return output;
        }

        /// <summary>
        /// Determine from the file extension what type of image we are dealing with.
        /// </summary>
        public static ImageFormat GetImageFormat(string imageFilePath)
        {
            string fileExtension = PathExtensions.GetExtensionOnly(imageFilePath);

            ImageFormat output = IRgbFloatImageSerializerExtensions.FromFileExtension(fileExtension);
            return output;
        }

        public static bool IsSupportedImageFormat(this IRgbFloatImageSerializer rgbFloatImageSerializer, ImageFormat imageFormat)
        {
            var supportedFormats = rgbFloatImageSerializer.SupportedImageFormats;

            bool output = supportedFormats.Contains(imageFormat);
            return output;
        }

        public static bool IsSupportedImageFormat(this IRgbFloatImageSerializer rgbFloatImageSerializer, string imageFilePath)
        {
            ImageFormat imageFormat = IRgbFloatImageSerializerExtensions.GetImageFormat(imageFilePath);

            bool output = rgbFloatImageSerializer.IsSupportedImageFormat(imageFormat);
            return output;
        }
    }
}
