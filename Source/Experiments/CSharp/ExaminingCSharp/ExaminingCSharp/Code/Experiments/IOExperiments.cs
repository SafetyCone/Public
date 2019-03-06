using System;
using System.IO;


namespace ExaminingCSharp
{
    public static class IOExperiments
    {
        public static void SubMain()
        {
            //IOExperiments.DeletionOfNonExistentDirectoryPath();
            IOExperiments.DeletionOfNonExistentFilePath();
        }

        /// <summary>
        /// Result: UNEXPECTED! Calling <see cref="File.Delete(string)"/> on a non-existent file path does NOT cause an exception.
        /// Does calling <see cref="File.Delete(string)"/> on a non-existent file path cause an exception?
        /// Expected: Yes. Since <see cref="Directory.Delete(string)"/> throws an exception, I expect a <see cref="FileNotFoundException"/> to be thrown.
        /// </summary>
        private static void DeletionOfNonExistentFilePath()
        {
            var filePath = @"C:\Temp\Temp.txt";

            File.Delete(filePath);
        }

        /// <summary>
        /// Result: UNEXPECTED! A <see cref="DirectoryNotFoundException"/> is thrown indicating that the directory could not be found.
        /// Does calling <see cref="Directory.Delete(string)"/> on a non-existent directory path cause an exception?
        /// Expected: No, directory is not present, so Delete() will behave sensibly.
        /// </summary>
        private static void DeletionOfNonExistentDirectoryPath()
        {
            var directoryPath = @"C:\Temp\Temp";

            Directory.Delete(directoryPath, true); // System.IO.DirectoryNotFoundException: 'Could not find a part of the path 'C:\Temp\Temp'.'
        }
    }
}
