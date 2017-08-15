using System;
using System.IO;


namespace Public.Common.Lib.Extensions
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
	}
}
