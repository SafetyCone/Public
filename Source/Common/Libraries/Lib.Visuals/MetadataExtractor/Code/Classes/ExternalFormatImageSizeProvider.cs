using System.Collections.Generic;
using System.Linq;

using MetadataExtractor;
using MetadataDirectory = MetadataExtractor.Directory;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.Jpeg;


namespace Public.Common.Lib.Visuals.MetadataExtractor
{
    public class ExternalFormatImageSizeProvider : IExternalFormatImageSizeProvider
    {
        #region Static

        public static ImageSize GetSizeS(string externalFormatImageFilePath)
        {
            IEnumerable<MetadataDirectory> directories = ImageMetadataReader.ReadMetadata(externalFormatImageFilePath);
            Directory dir = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
            string imageHeightTagName;
            string imageWidthTagName;
            if (null == dir)
            {
                dir = directories.OfType<JpegDirectory>().FirstOrDefault();
                imageHeightTagName = @"Image Height";
                imageWidthTagName = @"Image Width";
            }
            else
            {
                imageHeightTagName = @"Exif Image Height";
                imageWidthTagName = @"Exif Image Width";
            }
            var imageHeightTag = dir.Tags.Where((x) => x.Name == imageHeightTagName).FirstOrDefault();
            var imageWidthTag = dir.Tags.Where((x) => x.Name == imageWidthTagName).FirstOrDefault();

            int imageHeight = dir.GetInt32(imageHeightTag.Type);
            int imageWidth = dir.GetInt32(imageWidthTag.Type);

            ImageSize output = new ImageSize(imageHeight, imageWidth);
            return output;
        }

        #endregion


        public ImageSize GetSize(string externalFormatImageFilePath)
        {
            var output = ExternalFormatImageSizeProvider.GetSizeS(externalFormatImageFilePath);
            return output;
        }
    }
}
