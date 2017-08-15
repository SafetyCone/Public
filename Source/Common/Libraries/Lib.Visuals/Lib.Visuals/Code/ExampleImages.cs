using System.IO;


namespace Minex.Common.Lib
{
    public class ExampleImages
    {
        public const string ExampleImageFolder = @"E:\Organizations\Minex\Projects\RGB2HSV\Example Images";
        public const string ExampleImage1Filename = @"Example Image 1.bmp";
        public const string ExampleImage2Filename = @"Example Image 2.bmp";
        public const string ExampleImage3Filename = @"Example Image 3.bmp";
        public const string ExampleImage4Filename = @"Example Image 4.bmp";
        public static readonly string[] ExampleImagePaths = new string[]
        {
            Path.Combine(ExampleImages.ExampleImageFolder, ExampleImages.ExampleImage1Filename),
            Path.Combine(ExampleImages.ExampleImageFolder, ExampleImages.ExampleImage2Filename),
            Path.Combine(ExampleImages.ExampleImageFolder, ExampleImages.ExampleImage3Filename),
            Path.Combine(ExampleImages.ExampleImageFolder, ExampleImages.ExampleImage4Filename),
        };
    }
}
