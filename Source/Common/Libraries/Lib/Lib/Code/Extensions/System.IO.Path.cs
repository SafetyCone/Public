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
    }
}
