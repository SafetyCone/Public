using System;
using System.IO;


namespace ExaminingCSharp
{
    public static class IODemonstrations
    {
        public static void SubMain()
        {
            //IODemonstrations.DeletionOfNonExistentDirectoryPathThrowsException();
            IODemonstrations.DeletionOfNonExistentFilePathNoException();
        }

        /// <summary>
        /// Demonstrates that calling <see cref="File.Delete(string)"/> on a non-existent file path does not cause an exception!
        /// 
        /// It's a specific behavioral question of how <see cref="File.Delete(string)"/> treats non-existent paths.
        /// </summary>
        private static void DeletionOfNonExistentFilePathNoException()
        {
            var writer = Console.Out;

            var newGuid = Guid.NewGuid();
            var directoryPath = $@"C:\{newGuid}.txt"; // A definitely non-existent file!

            File.Delete(directoryPath);
        }

        /// <summary>
        /// Demonstrates that calling <see cref="Directory.Delete(string)"/> on a non-existent directory path results in a <see cref="DirectoryNotFoundException"/>.
        /// 
        /// It's a specific behavioral question of how <see cref="Directory.Delete(string)"/> treats non-existent paths.
        /// </summary>
        private static void DeletionOfNonExistentDirectoryPathThrowsException()
        {
            var writer = Console.Out;

            var newGuid = Guid.NewGuid();
            var directoryPath = $@"C:\{newGuid}"; // A definitely non-existent directory!

            try
            {
                Directory.Delete(directoryPath);
            }
            catch(DirectoryNotFoundException)
            {
                writer.WriteLine($@"{nameof(DirectoryNotFoundException)} thrown!");
            }
        }
    }
}
