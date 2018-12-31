using System;
using System.IO;


namespace Scratch
{
    public static class TestSystemPath
    {
        public static void SubMain()
        {
            //TestSystemPath.TestFileParts();
            //TestSystemPath.TestChangeExtension();
            //TestSystemPath.TestCombine();
            //TestSystemPath.TestGetExtension();
            //TestSystemPath.TestGetFileName();
            //TestSystemPath.TestGetFileNameWithoutExtension();
            //TestSystemPath.TestGetFullPath();
            //TestSystemPath.TestGetInvalidFileNameChars();
            //TestSystemPath.TestGetInvalidPathChars();
            //TestSystemPath.TestGetPathRoot();
            //TestSystemPath.TestGetRandomFileName();
            //TestSystemPath.TestGetRelativePath();
            //TestSystemPath.TestGetTempFileName();
            //TestSystemPath.TestGetTempPath();
            //TestSystemPath.TestHasExtension();
            //TestSystemPath.TestInvalidPathChars();
            //TestSystemPath.TestIsPathFullyQualified();
            //TestSystemPath.TestIsPathRooted();
            //TestSystemPath.TestJoin();
            TestSystemPath.TestSeparatorChars();
        }

        public static void TestSeparatorChars()
        {
            var writer = Console.Out;

            writer.WriteLine($@"Path separator char: '{Path.PathSeparator}'");
            writer.WriteLine($@"Volume separator char: '{Path.VolumeSeparatorChar}'");
            writer.WriteLine($@"Directory separator char: '{Path.DirectorySeparatorChar}'");
            writer.WriteLine($@"Alt directory separator char: '{Path.AltDirectorySeparatorChar}'");
        }

        //public static void TestJoin()
        //{
        //    var writer = Console.Out;

        //    var isRooted1 = Path.Join(@"C:\temp\temp.txt");
        //    writer.WriteLine($@"Is rooted 1: {isRooted1}"); // Example output: True
        //}

        //public static void TestIsPathRooted()
        //{
        //    var writer = Console.Out;

        //    var isRooted1 = Path.IsPathRooted(@"C:\temp\temp.txt");
        //    writer.WriteLine($@"Is rooted 1: {isRooted1}"); // Example output: True

        //    var isRooted2 = Path.IsPathRooted(@"temp\temp.txt");
        //    writer.WriteLine($@"Is rooted2: {isRooted2}"); // Example output: False

        //    var isRooted3 = Path.IsPathRooted(@"/mnt/efs/temp.txt");
        //    writer.WriteLine($@"Is rooted 3: {isRooted3}"); // Example output: True

        //    var isRooted4 = Path.IsPathRooted(@"temp/temp.txt");
        //    writer.WriteLine($@"Is rooted 4: {isRooted4}"); // Example output: False

        //    var isRooted5 = Path.IsPathRooted(@"\temp\temp.txt");
        //    writer.WriteLine($@"Is rooted5: {isRooted5}"); // Example output: True

        //    var isRooted6 = Path.IsPathRooted(@"/temp\temp.txt");
        //    writer.WriteLine($@"Is rooted6: {isRooted6}"); // Example output: True
        //}

        //public static void TestIsPathFullyQualified()
        //{
        //    var writer = Console.Out;

        //    var isRooted1 = Path.IsPathFullyQualified(@"C:\temp\temp.txt");
        //    writer.WriteLine($@"Is rooted 1: {isRooted1}"); // Example output: True

        //    var isRooted2 = Path.IsPathFullyQualified(@"temp\temp.txt");
        //    writer.WriteLine($@"Is rooted2: {isRooted2}"); // Example output: False

        //    var isRooted3 = Path.IsPathFullyQualified(@"/mnt/efs/temp.txt");
        //    writer.WriteLine($@"Is rooted 3: {isRooted3}"); // Example output: False

        //    var isRooted4 = Path.IsPathFullyQualified(@"temp/temp.txt");
        //    writer.WriteLine($@"Is rooted 4: {isRooted4}"); // Example output: False
        //}

        //// Obsolete.
        //public static void TestInvalidPathChars()
        //{
        //    var output = Path.InvalidPathChars;
        //}

        private static void TestHasExtension()
        {
            var writer = Console.Out;

            var hasExtension1 = Path.HasExtension(@"C:\temp\temp.txt");
            writer.WriteLine($@"Has extension 1: {hasExtension1}"); // Example output: True

            var hasExtension2 = Path.HasExtension(@"C:\temp\temp");
            writer.WriteLine($@"Has extension 2: {hasExtension2}"); // Example output: False

            var hasExtension3 = Path.HasExtension(@"C:\temp\temp.");
            writer.WriteLine($@"Has extension 3: {hasExtension3}"); // Example output: False
        }

        private static void TestGetTempPath()
        {
            var writer = Console.Out;

            var tempPath = Path.GetTempPath();
            writer.WriteLine($@"Temp path: {tempPath}"); // Example output: C:\Users\david\AppData\Local\Temp\
        }

        private static void TestGetTempFileName()
        {
            var writer = Console.Out;

            var tempFileName = Path.GetTempFileName();
            writer.WriteLine($@"Temp file name: {tempFileName}"); // Example output: C:\Users\david\AppData\Local\Temp\tmpB013.tmp
        }

        private static void TestGetRelativePath()
        {
            var writer = Console.Out;

            var relativeFilePath1 = Path.GetRelativePath(@"C:\temp\temp\temp.txt", @"C:\temp\temp2\temp2.txt");
            writer.WriteLine($@"Relative file path 1: {relativeFilePath1}"); // ..\..\temp2\temp2.txt

            var relativeFilePath2 = Path.GetRelativePath(@"/mnt/efs/temp/temp.txt", @"/mnt/efs/temp2/temp2.txt");
            writer.WriteLine($@"Relative file path 2: {relativeFilePath2}"); // ..\..\temp2\temp2.txt
        }

        private static void TestGetRandomFileName()
        {
            var writer = Console.Out;

            var randomPathSegment = Path.GetRandomFileName();
            writer.WriteLine($@"Random path segment: {randomPathSegment}"); // Example output: ulcdtig4.v53
        }

        private static void TestGetPathRoot()
        {
            var writer = Console.Out;

            var rootPath1 = Path.GetPathRoot(@"C:\temp\temp.txt");
            writer.WriteLine($@"Root path 1: {rootPath1}"); // C:\

            var rootPath2 = Path.GetPathRoot(@"C:\temp\temp");
            writer.WriteLine($@"Root path 2: {rootPath2}"); // C:\

            var rootPath3 = Path.GetPathRoot(@"/mnt/efs");
            writer.WriteLine($@"Root path 3: {rootPath3}"); // \

            var rootPath4 = Path.GetPathRoot(@"E:\temp\temp.txt");
            writer.WriteLine($@"Root path 4: {rootPath4}"); // E:\
        }

        private static void TestGetInvalidPathChars()
        {
            var writer = Console.Out;

            var invalidPathChars = Path.GetInvalidPathChars();
            writer.WriteLine(@"Invalid path chars:"); // |, plus ~35 others that are not printable in the console window.
            foreach (var invalidPathChar in invalidPathChars)
            {
                writer.WriteLine(invalidPathChar);
            }

            // Write to a file to use a different viewer that can print the unprintable chars.
            using (var fileWriter = new StreamWriter(@"C:\temp\temp.txt"))
            {
                foreach (var invalidPathChar in invalidPathChars)
                {
                    fileWriter.WriteLine(invalidPathChar);
                }
            }
        }

        private static void TestGetInvalidFileNameChars()
        {
            var writer = Console.Out;

            var invalidFileNameChars = Path.GetInvalidFileNameChars();
            writer.WriteLine(@"Invalid file-name chars:"); // ",<,>,|,:,*,?,\,/ on Windows, plus ~35 that are not printable in the console window.
            foreach (var invalidFileNameChar in invalidFileNameChars)
            {
                writer.WriteLine(invalidFileNameChar);
            }

            // Write to a file to use a different viewer that can print the unprintable chars.
            using (var fileWriter = new StreamWriter(@"C:\temp\temp.txt"))
            {
                foreach (var invalidFileNameChar in invalidFileNameChars)
                {
                    fileWriter.WriteLine(invalidFileNameChar);
                }
            }
        }

        private static void TestGetFullPath()
        {
            var writer = Console.Out;

            var fullPath1 = Path.GetFullPath(@"temp.txt");
            writer.WriteLine($@"Full path 1: {fullPath1}"); // {Current Directory}\temp.txt

            var fullPath2 = Path.GetFullPath(@"\temp.txt");
            writer.WriteLine($@"Full path 2: {fullPath2}"); // C:\temp.txt

            var fullPath3 = Path.GetFullPath(@"/temp.txt");
            writer.WriteLine($@"Full path 3: {fullPath3}"); // C:\temp.txt
        }

        private static void TestGetFileNameWithoutExtension()
        {
            var writer = Console.Out;

            var fileName1 = Path.GetFileNameWithoutExtension(@"C:\temp\temp.txt");
            writer.WriteLine($@"File name 1: {fileName1}"); // temp

            var fileName2 = Path.GetFileNameWithoutExtension(@"/mnt/efs/temp.txt");
            writer.WriteLine($@"File name 2: {fileName2}"); // temp
        }

        private static void TestGetFileName()
        {
            var writer = Console.Out;

            var fileName1 = Path.GetFileName(@"C:\temp\temp.txt");
            writer.WriteLine($@"File name 1: {fileName1}"); // temp.txt

            var fileName2 = Path.GetFileName(@"/mnt/efs/temp.txt");
            writer.WriteLine($@"File name 2: {fileName2}"); // temp.txt
        }

        private static void TestGetExtension()
        {
            var writer = Console.Out;

            var fileExtension1 = Path.GetExtension(@"C:\temp\temp.txt");
            writer.WriteLine($@"File extension 1: {fileExtension1}"); // .txt

            var fileExtension2 = Path.GetExtension(@"C:\temp\temp.");
            writer.WriteLine($@"File extension 2: '{fileExtension2}'"); // String.Empty

            var fileExtension3 = Path.GetExtension(@"C:\temp\temp");
            writer.WriteLine($@"File extension 3: '{fileExtension3}'"); // String.Empty

            // Works on non-Windows paths.
            var fileExtension4 = Path.GetExtension(@"/mnt/efs/temp.txt");
            writer.WriteLine($@"File extension 4: {fileExtension4}"); // .txt

            // Does not change any capitalization.
            var fileExtension5 = Path.GetExtension(@"/mnt/efs/temp.TXT");
            writer.WriteLine($@"File extension 5: {fileExtension5}"); // .TXT
        }

        private static void TestCombine()
        {
            var writer = Console.Out;

            // Basic good example. Result: C:\temp\temp.txt
            var tempTextFilePath1 = Path.Combine(@"C:", @"temp", @"temp.txt");
            writer.WriteLine($@"Temp text file path 1: {tempTextFilePath1}");

            // Basic good example. Result: C:\temp\temp.txt
            var tempTextFilePath2 = Path.Combine(@"C:\", @"temp\", @"temp.txt");
            writer.WriteLine($@"Temp text file path 2: {tempTextFilePath2}");

            // Basic bad example 1. Result: \temp\temp.txt. WTF did the initial 'C:' go?!?!
            var tempBadFilePath1 = Path.Combine(@"C:\", @"\temp", @"temp.txt");
            writer.WriteLine($@"Temp bad file path 1: {tempBadFilePath1}");

            // Basic bad example 2. Result: \temp.txt. WTF did the initial 'C:\temp' go?!?!
            var tempBadFilePath2 = Path.Combine(@"C:\", @"\temp", @"\temp.txt");
            writer.WriteLine($@"Temp bad file path 2: {tempBadFilePath2}");

            // Basic non-Windows example. Result: /mnt\efs\temp.txt. Uses current platform's path separator with no overload.
            var tempNonWindowsTextFilePath1 = Path.Combine(@"/mnt", @"efs", @"temp.txt");
            writer.WriteLine($@"Non-Windows temp text file path 1: {tempNonWindowsTextFilePath1}");

            // Basic non-Windows example. Result: /mnt\efs/temp.txt.
            var tempNonWindowsTextFilePath2 = Path.Combine(@"/mnt", @"efs/", @"temp.txt");
            writer.WriteLine($@"Non-Windows temp text file path 2: {tempNonWindowsTextFilePath2}");

            // Basic non-Windows example. Result: /temp.txt.
            var tempNonWindowsTextFilePath3 = Path.Combine(@"/mnt", @"/efs", @"/temp.txt");
            writer.WriteLine($@"Non-Windows temp text file path 3: {tempNonWindowsTextFilePath3}");

            // Basic non-Windows example. Result: /efs\temp.txt.
            var tempNonWindowsTextFilePath4 = Path.Combine(@"/mnt", @"/efs", @"temp.txt");
            writer.WriteLine($@"Non-Windows temp text file path 4: {tempNonWindowsTextFilePath4}");
        }

        private static void TestChangeExtension()
        {
            var writer = Console.Out;

            writer.WriteLine(@"--- WINDOWS ---");

            var pathWindows = @"C:\temp\temp.txt";
            writer.WriteLine($@"Path: {pathWindows}");

            var changedExtension = Path.ChangeExtension(pathWindows, @"jpg");
            writer.WriteLine($@"Changed extension: {changedExtension}"); // C:\temp\temp.jpg

            var changedExtension2 = Path.ChangeExtension(pathWindows, @".jpeg");
            writer.WriteLine($@"Changed extension: {changedExtension2}"); // C:\temp\temp.jpeg
        }

        private static void TestFileParts()
        {
            var writer = Console.Out;

            writer.WriteLine(@"--- WINDOWS on Windows---");

            var pathWindows = @"C:\temp\temp.txt";
            writer.WriteLine($@"Path: {pathWindows}");

            var directoryNameWindows = Path.GetDirectoryName(pathWindows);
            writer.WriteLine($@"Directory-name: {directoryNameWindows}"); // C:\temp

            var fileNameWindows = Path.GetFileName(pathWindows);
            writer.WriteLine($@"File-name: {fileNameWindows}"); // temp.txt

            var fileNameWithoutExtensionWindows = Path.GetFileNameWithoutExtension(pathWindows);
            writer.WriteLine($@"File-name without extension: {fileNameWithoutExtensionWindows}"); // temp

            var extensionWindows = Path.GetExtension(pathWindows);
            writer.WriteLine($@"Extension: {extensionWindows}"); // .txt

            writer.WriteLine();

            writer.WriteLine(@"--- NON-WINDOWS, Run on Windows ---");

            var pathNonWindows = @"/mnt/efs/temp.txt";
            writer.WriteLine($@"Path: {pathNonWindows}");

            var directoryNameNonWindows = Path.GetDirectoryName(pathNonWindows);
            writer.WriteLine($@"Directory-name: {directoryNameNonWindows}"); // \mnt\efs

            var fileNameNonWindows = Path.GetFileName(pathNonWindows);
            writer.WriteLine($@"File-name: {fileNameNonWindows}"); // temp.txt

            var fileNameWithoutExtensionNonWindows = Path.GetFileNameWithoutExtension(pathNonWindows);
            writer.WriteLine($@"File-name without extension: {fileNameWithoutExtensionNonWindows}"); // temp

            var extensionNonWindows = Path.GetExtension(pathNonWindows);
            writer.WriteLine($@"Extension: {extensionNonWindows}"); // .txt

            writer.WriteLine();

            var directoryNameForDirectory1 = Path.GetDirectoryName(@"C:\temp\temp2");
            writer.WriteLine($@"Directory name for directory 1: {directoryNameForDirectory1}"); // C:\temp

            var directoryNameForDirectory2 = Path.GetDirectoryName(@"C:\temp\temp2\");
            writer.WriteLine($@"Directory name for directory 2: {directoryNameForDirectory2}"); // C:\temp\temp2

            var directoryNameForDirectory3 = Path.GetDirectoryName(@"/mnt/efs/temp");
            writer.WriteLine($@"Directory name for directory 3: {directoryNameForDirectory3}"); // \mnt\efs

            var directoryNameForDirectory4 = Path.GetDirectoryName(@"/mnt/efs/temp/");
            writer.WriteLine($@"Directory name for directory 4: {directoryNameForDirectory4}"); // \mnt\efs\temp
        }
    }
}
