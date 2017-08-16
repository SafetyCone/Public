using System;
using System.IO;

using Public.Common.Lib.Extensions;
using Public.Common.Lib.IO;
using Public.Common.Lib.IO.Extensions;
using Public.Common.Lib.Visuals;


namespace Benedek
{
    class Program
    {
        static void Main(string[] args)
        {
            //Program.SubMain(args);
            Program.SubMain();
        }

        private static void SubMain()
        {
            string[] args =
            {
                @"C:\temp\DSC_0729.bmp"
            };

            Program.SubMain(args);
        }

        private static void SubMain(string[] args)
        {
            IOutputStream interactive = MultipleOutputStream.DebugAndConsoleOutputStream;

            Configuration config;
            if(Configuration.ParseArguments(out config, args, interactive))
            {
                try
                {
                    BitmapFileHeader header = BitmapFileSerializer.DeserializeHeaderStatic(config.BitmapFilePath);

                    using (StreamWriter fWriter = new StreamWriter(config.OutputFilePath))
                    {
                        string line;

                        fWriter.WriteLine(@"{0}", DateTime.Now.ToYYYYMMDD_HHMMSSStr());
                        fWriter.WriteLine(@"Bitmap header information for file: {0}", config.BitmapFilePath);
                        fWriter.WriteLine();

                        BitmapFileHeaderHeader headerHeader = header.HeaderHeader;
                        fWriter.WriteLine(@"Header header information:");

                        line = String.Format(@"File size (bytes): {0:n0}", headerHeader.FileSize);
                        fWriter.WriteLine(line);

                        line = String.Format(@"Offset from start of file to start of pixel data (bytes): {0:n0}", headerHeader.OffsetToPixelData);
                        fWriter.WriteLine(line);

                        fWriter.WriteLine();

                        BitmapFileDIBHeader dibHeader = header.DIBHeader;
                        fWriter.WriteLine(@"Device independent bitmap (DIB) header information:");

                        line = String.Format(@"DIB header size (bytes): {0:n0}", dibHeader.DIBHeaderSize);
                        fWriter.WriteLine(line);

                        line = String.Format(@"Image width (pixels): {0:n0}", dibHeader.WidthX);
                        fWriter.WriteLine(line);

                        line = String.Format(@"Image height (pixels): {0:n0}", dibHeader.HeightY);
                        fWriter.WriteLine(line);

                        line = String.Format(@"Number of color planes (should be 1): {0}", dibHeader.NumberOfColorPlanes);
                        fWriter.WriteLine(line);

                        line = String.Format(@"Image bits per pixel: {0}", dibHeader.BitsPerPixel);
                        fWriter.WriteLine(line);

                        line = String.Format(@"Compression method: {0}", dibHeader.CompressionMethod);
                        fWriter.WriteLine(line);

                        line = String.Format(@"Size of image pixel data (bytes): {0:n0}", dibHeader.ImageSize);
                        fWriter.WriteLine(line);

                        line = String.Format(@"Image width (X direction) resolution (pixels per meter): {0:n0}", dibHeader.HorizontalResolution);
                        fWriter.WriteLine(line);

                        line = String.Format(@"Image height (Y direction) resolution (pixels per meter): {0:n0}", dibHeader.VerticalResolution);
                        fWriter.WriteLine(line);

                        line = String.Format(@"Color table color count: {0}", dibHeader.ColorTableColorCount);
                        fWriter.WriteLine(line);

                        line = String.Format(@"Important color count: {0}", dibHeader.ImportantColorCount);
                        fWriter.WriteLine(line);
                    }
                }
                catch (Exception ex)
                {
                    interactive.WriteLine(@"ERROR");
                    interactive.Write(ex.ToString());
                }
            }
        }
    }
}
