using System;
using System.IO;


namespace ExaminingCSharp
{
    public static class IOExperiments
    {
        public static void SubMain()
        {
            //IOExperiments.DeletionOfNonExistentDirectoryPath();
            //IOExperiments.DeletionOfNonExistentFilePath();
            //IOExperiments.GetDirectoryNameOfNonWindowsDirectoryPath();
            //IOExperiments.DoesDefaultStreamWriterProduceBOM();
            IOExperiments.CreationOfAlreadyExistingDirectory();
        }

        /// <summary>
        /// What happens if you create an already existing directory?
        /// Result: Expected, no exception thrown.
        /// Expected: No exception is thrown. The <see cref="Directory.CreateDirectory(string)"/> documentation suggests that directories will be created unless they already exist, and that exceptions are thrown only when the specified directory path already exists and is a file.
        /// Documentation: https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.createdirectory?view=netframework-4.8
        /// </summary>
        public static void CreationOfAlreadyExistingDirectory()
        {
            var random = new Random(314);

            var directoryName = random.Next().ToString();
            var directoryPath = $@"C:\Temp\{directoryName}";

            Directory.CreateDirectory(directoryPath);
            Directory.CreateDirectory(directoryPath);

            Directory.Delete(directoryPath);
        }

        /// <summary>
        /// Result: Expected.
        /// The UTF8 byte-order mark (BOM) is a real pain. It is three (3) bytes, an odd-number, and thus wreaks havoc with double-byte Unicode. It is sometimes added by default, especially in the <see cref="System.Text.Encoding.UTF8"/> encoding. And it is largely invisible in text editors, so you never know why your text stream is messed up.
        /// This is especially problematic when using a <see cref="StreamWriter"/> to write to a <see cref="MemoryStream"/> that you want to keep open, since you will need to specify the encoding for the <see cref="StreamWriter"/> and again, the <see cref="System.Text.Encoding.UTF8"/> encoding supplies a BOM. (Note, a StreamWriterHelper.NewLeaveOpen() method to construct a <see cref="StreamWriter"/> for this case is a good idea.)
        /// Expected: No BOM, as explained here: https://docs.microsoft.com/en-us/dotnet/api/system.io.streamwriter.-ctor?view=netcore-2.2.
        /// </summary>
        private static void DoesDefaultStreamWriterProduceBOM()
        {
            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream))
            {
                streamWriter.Write(@"A");

                streamWriter.Flush();

                var numberOfBytesWritten = memoryStream.Position;

                Console.WriteLine($@"Number of bytes written: {numberOfBytesWritten}"); // Should be 1!

                memoryStream.Seek(0, SeekOrigin.Begin);

                Console.WriteLine(@"Bytes:");

                int @byte;
                while ((@byte = memoryStream.ReadByte()) != -1)
                {
                    Console.WriteLine(@byte);
                }
            }
        }

        /// <summary>
        /// Result: UNEXPECTED! No exception, but directory path returned uses the current platform directory separator, not the directory separator it originally used.
        /// How does the System.IO functionality handle directory separators from a different platform?
        /// Expected: An exception will be thrown if the path does not have the platform (currently Windows) directory separator.
        /// </summary>
        private static void GetDirectoryNameOfNonWindowsDirectoryPath()
        {
            var path = @"/mnt/eft/ts1.txt";

            var directoryName = Path.GetDirectoryName(path);

            Console.WriteLine($@"Directory path: {directoryName}");
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
