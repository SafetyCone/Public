using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace Public.Common.Lib.IO
{
	public static class DirectoryExtensions
	{
        /// <summary>
        /// Performs a deep copy of all files and subdirectories from one directory path to another, like Windows Explorer does when copying one directory to another.
        /// </summary>
        /// <remarks>
        /// There is no System.IO.Directory.Copy() method, and the System.IO.File.Copy() method only works on files.
        /// Thus we need our own implementation.
        /// </remarks>
        public static void Copy(string sourceDirectoryPath, string destinationDirectoryPath, bool recursive, bool overwrite)
        {
            DirectoryInfo source = new DirectoryInfo(sourceDirectoryPath);
            if (!source.Exists)
            {
                string message = String.Format(@"Source directory not found: {0}", sourceDirectoryPath);
                throw new DirectoryNotFoundException(message);
            }

            DirectoryInfo destination = new DirectoryInfo(destinationDirectoryPath);
            if(!destination.Exists)
            {
                Directory.CreateDirectory(destination.FullName);
            }
            
            FileInfo[] sourceFiles = source.GetFiles();
            foreach(FileInfo sourceFile in sourceFiles)
            {
                string sourceFilePath = sourceFile.FullName;
                string fileName = sourceFile.Name;
                string destinationFilePath = Path.Combine(destination.FullName, fileName);

                File.Copy(sourceFilePath, destinationFilePath, overwrite);
            }

            if (recursive)
            {
                DirectoryInfo[] sourceSubDirectories = source.GetDirectories();
                foreach (DirectoryInfo sourceSubDirectory in sourceSubDirectories)
                {
                    string sourceSubDirectoryPath = sourceSubDirectory.FullName;
                    string sourceSubDirectoryName = sourceSubDirectory.Name;

                    string destinationSubDirectoryPath = Path.Combine(destination.FullName, sourceSubDirectoryName);
                    DirectoryExtensions.Copy(sourceSubDirectoryPath, destinationSubDirectoryPath, recursive, overwrite);
                }
            }
        }

        /// <summary>
        /// Performs a deep copy of all files and subdirectories from one directory path to another, like Windows Explorer does when copying one directory to another.
        /// </summary>
        public static void Copy(string sourceDirectoryPath, string destinationDirectoryPath)
        {
            DirectoryExtensions.Copy(sourceDirectoryPath, destinationDirectoryPath, true, true);
        }

        public static string[] FilePathsByExtensions(string directoryPath, IEnumerable<string> fileExtensions)
        {
            string[] fileExtensionsArr = fileExtensions?.ToArray() ?? new string[] { };

            int nFileExtensions = fileExtensionsArr.Length;

            string[] output;
            if(0 < nFileExtensions)
            {
                // Build the regex pattern.
                StringBuilder builder = new StringBuilder(fileExtensionsArr[0]);
                for (int iFileExtensions = 1; iFileExtensions < nFileExtensions; iFileExtensions++)
                {
                    string fileExtension = fileExtensionsArr[iFileExtensions];
                    string appendix = $@"|{fileExtension}";
                    builder.Append(appendix);
                }

                string regexPattern = builder.ToString();
                Regex regex = new Regex(regexPattern);

                List<string> pathsOfInterest = new List<string>();
                foreach(var filePath in Directory.EnumerateFiles(directoryPath))
                {
                    string loweredFilePath = filePath.ToLowerInvariant();
                    if(regex.IsMatch(loweredFilePath))
                    {
                        pathsOfInterest.Add(filePath);
                    }
                }

                output = pathsOfInterest.ToArray();
            }
            else
            {
                output = new string[] { };
            }

            return output;
        }
	}
}
