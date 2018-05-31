using System;
using System.IO;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;


namespace ImageSharpNetCore
{
    class Program
    {
        private static string GetFileRootedPath()
        {   
            // Win.
            string directoryRootedPath = @"C:\Users\David\Downloads";
            string fileName = @"20180303_151739.jpg";

            string fileRootedPath = Path.Combine(directoryRootedPath, fileName);
            return fileRootedPath;
        }

        static void Main(string[] args)
        {
            string fileRootedPath = Program.GetFileRootedPath();

            Console.WriteLine($"Image file path: {fileRootedPath}");

            using (Image<Rgba32> image = Image.Load(fileRootedPath))
            {
                Console.WriteLine($"Height: {image.Height.ToString()}");
            }
        }
    }
}
