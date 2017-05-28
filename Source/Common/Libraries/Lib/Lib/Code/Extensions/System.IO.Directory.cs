using System;
using System.IO;


namespace Public.Common.Lib.Extensions
{
	public static class DirectoryExtensions
	{
        public static void Copy(string sourceDirectoryPath, string destinationDirectoryPath, bool recursive, bool overwrite)
        {
            DirectoryInfo source = new DirectoryInfo(sourceDirectoryPath);
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
        /// Performs a full copy of all files and subdirectories from one directory path to another.
        /// </summary>
        /// <remarks>
        /// This the the deep-copy behavior of Windows Explorer, when copying one directory to another.
        /// </remarks>
        public static void Copy(string sourceDirectoryPath, string destinationDirectoryPath)
        {
            DirectoryExtensions.Copy(sourceDirectoryPath, destinationDirectoryPath, true, true);
        }
	}
}
