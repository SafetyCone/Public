using System;
using System.IO;

using SkiaSharp;


namespace SkiaSharpNetCore
{
    class Program
    {
        private static string GetFileRootedPath()
        {
            string directoryRootedPath = @"C:\Users\David\Downloads";
            string fileName = @"20180303_151739.jpg";

            string fileRootedPath = Path.Combine(directoryRootedPath, fileName);
            return fileRootedPath;
        }

        static void Main(string[] args)
        {
            string fileRootedPath = Program.GetFileRootedPath();

            Console.WriteLine($"Image file path: {fileRootedPath}");

            using (var input = File.OpenRead(fileRootedPath))
            {
                using (var inputStream = new SKManagedStream(input))
                {
                    using (var image = SKBitmap.Decode(inputStream))
                    {
                        Console.WriteLine($"Height: {image.Height.ToString()}");
                    }
                }
            }
        }
    }
}
