using System;
using System.IO;


namespace Public.Common.Lib.Extensions
{
    public static class PathExtensions
    {
        /// <summary>
        /// Similar to System.IO.Path.DirectorySeparatorChar, but for separating a file name from its extenstion.
        /// </summary>
        /// <remarks>
        /// The .NET source code simply hard-codes this (to '.'), but I don't like that.
        /// NOTE: There may be multiple periods in a file name. Only the last token when separated is the file extension.
        /// </remarks>
        public const char WindowsFileExtensionSeparatorChar = '.';


        public static string GetRelativePath(string fromPath, string toPath)
        {
            Uri fromUri = new Uri(fromPath);
            Uri toUri = new Uri(toPath);

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);

            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());
            return relativePath;
        }

        public static string GetResolvedPath(string unresolvedPath)
        {
            Uri unresolvedUri = new Uri(unresolvedPath);
            string localPathUri = unresolvedUri.LocalPath;

            string output = Path.GetFullPath(localPathUri);
            return output;
        }

        /// <summary>
        /// Returns the file extension without the leading file extension character.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetExtensionOnly(string path)
        {
            string fileExtensionWithSeparator = Path.GetExtension(path);

            string output = fileExtensionWithSeparator.Substring(1); // Skip the first char.
            return output;
        }

        /// <summary>
        /// Combines the file name without extension, the Windows file extention separator, and the extension to produce the full file name.
        /// </summary>
        public static string GetFullFileName(string fileNameWithoutExtension, string extension)
        {
            string output = fileNameWithoutExtension + PathExtensions.WindowsFileExtensionSeparatorChar + extension;
            return output;
        }

        /// <summary>
        /// Returns the path to a file given the path of a base file and the relative path of the file.
        /// </summary>
        public static string GetPath(string baseFilePath, string relativeFilePath)
        {
            string directoryName = Path.GetDirectoryName(baseFilePath);
            string unresolvedFilePath = Path.Combine(directoryName, relativeFilePath);

            string output = PathExtensions.GetResolvedPath(unresolvedFilePath);
            return output;
        }

        public static void EnsureDirectoryPathCreated(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        public static void EnsureFilePathDirectoryCreated(string filePath)
        {
            string directoryPath = Path.GetDirectoryName(filePath);
            PathExtensions.EnsureDirectoryPathCreated(directoryPath);
        }

        private static string DateTimeMarkPath(string path, DateTime dateTime)
        {
            string todayYYYYMMDD = dateTime.ToYYYYMMDDStr();

            string directoryPath = Path.GetDirectoryName(path);
            string fileNameOnly = Path.GetFileNameWithoutExtension(path);
            string extension = PathExtensions.GetExtensionOnly(path);

            string datedFileName = String.Format(@"{0} - {1}", fileNameOnly, todayYYYYMMDD);
            string fullDatedFileName = PathExtensions.GetFullFileName(datedFileName, extension);

            string output = Path.Combine(directoryPath, fullDatedFileName);
            return output;
        }
    }
}
