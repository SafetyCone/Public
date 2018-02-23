using System.Collections;
using System.Collections.Generic;

using PublicIoUtilities = Public.Common.Lib.IO.Utilities;


namespace Public.Common.Lib.Visuals
{
    /// <summary>
    /// Lists all image file paths in a given directory.
    /// </summary>
    public class ImagePathSource : IEnumerable<string>
    {
        public static string[] DefaultImageFileExtensions = ImageFormatFileExtensions.All;


        public string DirectoryPath { get; private set; }
        public string[] ImageFileExtensions { get; private set; }
        public List<string> ImagePaths
        {
            get
            {
                List<string> output = new List<string>(this);
                return output;
            }
        }


        public ImagePathSource(string directoryPath)
            : this(directoryPath, ImagePathSource.DefaultImageFileExtensions)
        {
        }

        public ImagePathSource(string directoryPath, string[] fileExtensions)
        {
            this.DirectoryPath = directoryPath;
            this.ImageFileExtensions = fileExtensions; 
        }

        public IEnumerator<string> GetEnumerator()
        {
            string[] filePaths = PublicIoUtilities.FilePathsByExtensions(this.DirectoryPath, ImageFormatFileExtensions.All);

            // Return an enumerator to this list of image file paths.
            var output = new List<string>(filePaths);
            return output.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
