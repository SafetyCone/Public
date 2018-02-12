using System.Collections.Generic;
using System.Linq;

using MetadataExtractor;
using MetadataDirectory = MetadataExtractor.Directory;
using MetadataExtractor.Formats.Exif;


namespace Public.Common.Lib.Visuals.MetadataExtractor
{
    public class ExternalFormatImageSizeProvider : IExternalFormatImageSizeProvider
    {
        public ImageSize GetSize(string externalFormatImageFilePath)
        {
            IEnumerable<MetadataDirectory> directories = ImageMetadataReader.ReadMetadata(externalFormatImageFilePath);
            var subIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
            var imageHeightTag = subIfdDirectory.Tags.Where((x) => x.Name == @"Exif Image Height").FirstOrDefault();
            var imageWidthTag = subIfdDirectory.Tags.Where((x) => x.Name == @"Exif Image Width").FirstOrDefault();

            int imageHeight = subIfdDirectory.GetInt32(imageHeightTag.Type);
            int imageWidth = subIfdDirectory.GetInt32(imageWidthTag.Type);

            ImageSize output = new ImageSize(imageHeight, imageWidth);
            return output;
        }
    }
}
