using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using Public.Common.Lib.Extensions;


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
            // Build a regex pattern to find files with image file extensions of interest.
            StringBuilder fileExtensionsRegexPatternBuilder = new StringBuilder();
            foreach (var fileExtension in ImageFormatFileExtensions.All)
            {
                fileExtensionsRegexPatternBuilder.Append($@"\.{fileExtension}$|");
            }
            fileExtensionsRegexPatternBuilder.RemoveLast();

            // Make the regex.
            string regexPattern = fileExtensionsRegexPatternBuilder.ToString();
            Regex regex = new Regex(regexPattern);

            // Search all file paths in the directory using the regex, keeping the paths that match the image file extensions.
            var imageFilePaths = new List<string>();
            foreach (var filePath in Directory.EnumerateFiles(this.DirectoryPath))
            {
                if (regex.IsMatch(filePath))
                {
                    imageFilePaths.Add(filePath);
                }
            }

            // Return an enumerator to this list of image file paths.
            return imageFilePaths.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
