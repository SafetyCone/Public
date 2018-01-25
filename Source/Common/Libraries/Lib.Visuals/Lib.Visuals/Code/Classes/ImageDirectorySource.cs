using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;


namespace Public.Common.Lib.Visuals
{
    public class ImageDirectorySource : IRgbFloatImageSource
    {
        public string DirectoryPath { get; set; }
        public IRgbFloatImageSerializer Serializer { get; set; }


        public ImageDirectorySource() { }

        public ImageDirectorySource(string directoryPath, IRgbFloatImageSerializer serializer)
        {
            this.DirectoryPath = directoryPath;
            this.Serializer = serializer;
        }

        public IEnumerator<Tuple<ImageID, RgbFloatImage>> GetEnumerator()
        {
            ImagePathSource imagePathSource = new ImagePathSource(this.DirectoryPath);

            // Now deserialize images and return them one-by-one.
            foreach(string imageFilePath in imagePathSource)
            {
                RgbFloatImage image = this.Serializer[imageFilePath];

                string imageTitle = Path.GetFileName(imageFilePath);
                ImageID imageID = new ImageID(imageTitle, imageFilePath);

                yield return Tuple.Create(imageID, image);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
