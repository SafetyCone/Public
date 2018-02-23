using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Public.Common.Lib.Extensions;


namespace Public.Common.Lib.IO
{
    public static class Utilities
    {
        /// <summary>
        /// Read a text file containing lines, removing blank lines.
        /// </summary>
        /// <param name="textFilePath">The path of a text file.</param>
        /// <param name="removeBlankLines">Optionally remove blank lines.</param>
        /// <returns>A plain string array of lines within a file.</returns>
        public static string[] ReadAllLines(string textFilePath, bool removeBlankLines = true)
        {
            string[] allLines = File.ReadAllLines(textFilePath);

            string[] output;
            if (removeBlankLines)
            {
                output = allLines.Where((line) => !String.IsNullOrWhiteSpace(line)).ToArray();
            }
            else
            {
                output = allLines; // Just provide all lines.
            }

            return output;
        }

        public static void ActionPaths(IEnumerable<string> paths, Action<string> action)
        {
            foreach (var path in paths)
            {
                action(path);
            }
        }

        private static Regex GetFileExtensionsRegex(IEnumerable<string> fileExtensions)
        {
            // Build a regex pattern to find files with image file extensions of interest.
            StringBuilder fileExtensionsRegexPatternBuilder = new StringBuilder();
            foreach (var fileExtension in fileExtensions)
            {
                fileExtensionsRegexPatternBuilder.Append($@"\.{fileExtension}$|");
            }
            fileExtensionsRegexPatternBuilder.RemoveLast();

            // Make the regex.
            string regexPattern = fileExtensionsRegexPatternBuilder.ToString();
            Regex regex = new Regex(regexPattern, RegexOptions.IgnoreCase);
            return regex;
        }

        private static Regex GetPrefixesRegex(IEnumerable<string> prefixes)
        {
            // Build a regex pattern to find files with image file extensions of interest.
            StringBuilder fileExtensionsRegexPatternBuilder = new StringBuilder();
            foreach (var prefix in prefixes)
            {
                fileExtensionsRegexPatternBuilder.Append($@"^{prefix}|");
            }
            fileExtensionsRegexPatternBuilder.RemoveLast();

            // Make the regex.
            string regexPattern = fileExtensionsRegexPatternBuilder.ToString();
            Regex regex = new Regex(regexPattern, RegexOptions.IgnoreCase);
            return regex;
        }

        public static string[] FilePathsByFileNameRegex(string directoryPath, Regex regex)
        {
            // Search all file paths in the directory using the regex, keeping the paths that match the image file extensions.
            var imageFilePaths = new List<string>();
            foreach (var filePath in Directory.EnumerateFiles(directoryPath))
            {
                string fileName = Path.GetFileName(filePath);
                if (regex.IsMatch(fileName))
                {
                    imageFilePaths.Add(filePath);
                }
            }

            // Return an enumerator to this list of image file paths.
            return imageFilePaths.ToArray();
        }

        public static string[] FilePathsByPrefixes(string directoryPath, IEnumerable<string> prefixes)
        {
            var regex = Utilities.GetPrefixesRegex(prefixes);

            string[] filePaths = Utilities.FilePathsByFileNameRegex(directoryPath, regex);
            return filePaths;
        }

        public static string[] FilePathsByExtensions(string directoryPath, IEnumerable<string> fileExtensions)
        {
            var regex = Utilities.GetFileExtensionsRegex(fileExtensions);

            string[] filePaths = Utilities.FilePathsByFileNameRegex(directoryPath, regex);
            return filePaths;
        }

        public static void DeleteFilePathsByPrefixes(string directoryPath, IEnumerable<string> prefixes)
        {
            var filePaths = Utilities.FilePathsByPrefixes(directoryPath, prefixes);
            Utilities.ActionPaths(filePaths, File.Delete);
        }

        public static void DeleteFilePathsByExtensions(string directoryPath, IEnumerable<string> fileExtensions)
        {
            var filePaths = Utilities.FilePathsByExtensions(directoryPath, fileExtensions);
            Utilities.ActionPaths(filePaths, File.Delete);
        }

        public static string[] SubDirectoryPathsByDirectoryNameRegex(string directoryPath, Regex regex)
        {
            // Search all file paths in the directory using the regex, keeping the paths that match the image file extensions.
            var imageFilePaths = new List<string>();
            foreach (var filePath in Directory.EnumerateDirectories(directoryPath))
            {
                string directoryName = Path.GetFileName(filePath);
                if (regex.IsMatch(directoryName))
                {
                    imageFilePaths.Add(filePath);
                }
            }

            // Return an enumerator to this list of image file paths.
            return imageFilePaths.ToArray();
        }

        public static string[] SubDirectoryPathsByPrefixes(string directoryPath, IEnumerable<string> prefixes)
        {
            var regex = Utilities.GetPrefixesRegex(prefixes);

            string[] filePaths = Utilities.SubDirectoryPathsByDirectoryNameRegex(directoryPath, regex);
            return filePaths;
        }

        public static string[] SubDirectoryPathsByExtension(string directoryPath, IEnumerable<string> fileExtensions)
        {
            var regex = Utilities.GetFileExtensionsRegex(fileExtensions);

            string[] filePaths = Utilities.SubDirectoryPathsByDirectoryNameRegex(directoryPath, regex);
            return filePaths;
        }

        public static void DeleteSubDirectoryPathsByPrefixes(string directoryPath, IEnumerable<string> prefixes)
        {
            var filePaths = Utilities.SubDirectoryPathsByPrefixes(directoryPath, prefixes);
            Utilities.ActionPaths(filePaths, (x) => Directory.Delete(x, true));
        }

        public static void DeleteSubDirectoryPathsByExtensions(string directoryPath, IEnumerable<string> fileExtensions)
        {
            var filePaths = Utilities.SubDirectoryPathsByExtension(directoryPath, fileExtensions);
            Utilities.ActionPaths(filePaths, Directory.Delete);
        }

        public static string GetFilePathIfCopiedToDirectory(string filePath, string directoryPath)
        {
            string fileName = Path.GetFileName(filePath);

            string output = Path.Combine(directoryPath, fileName);
            return output;
        }

        public static void CopyFileToDirectory(string filePath, string directoryPath, bool overwrite = true)
        {
            string destinationFilePath = Utilities.GetFilePathIfCopiedToDirectory(filePath, directoryPath);

            File.Copy(filePath, destinationFilePath, overwrite);
        }
    }
}