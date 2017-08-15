using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;


namespace Minex.Common.Lib.Visuals
{
    public static class Utilities
    {
        /// <remarks>
        /// Unsafe section for speed in handling bitmap data.
        /// </remarks>
        private static void HandleBitmap<T>(ICoordinatedImage<T> image, Bitmap bitmap, Func<Coordinate, byte, byte, byte, T> colorConverter)
        {
            int heightInPixels = bitmap.Height;
            int widthInPixels = bitmap.Width;

            image.SetImageSize(heightInPixels, widthInPixels);

            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, widthInPixels, heightInPixels), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            unsafe
            {
                int bytesPerPixel = Bitmap.GetPixelFormatSize(bitmap.PixelFormat) / 8;
                int widthInBytes = widthInPixels * bytesPerPixel;
                byte* ptrFirstPixel = (byte*)bitmapData.Scan0;

                Parallel.For(0, heightInPixels, y =>
                {
                    byte* currentLine = ptrFirstPixel + (y * bitmapData.Stride);
                    int widthPixelCount = 0;
                    for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                    {
                        byte oldBlue = currentLine[x];
                        byte oldGreen = currentLine[x + 1];
                        byte oldRed = currentLine[x + 2];

                        Coordinate coordinate = new Coordinate(y, widthPixelCount);
                        widthPixelCount++;

                        T color = colorConverter(coordinate, oldRed, oldGreen, oldBlue);

                        image[coordinate] = color;
                    }
                });
            }

            bitmap.UnlockBits(bitmapData);
        }

        private static RgbColor ToRgbColor(byte oldRed, byte oldGreen, byte oldBlue)
        {
            double red = ColorConversion.LevelToValue(oldRed);
            double green = ColorConversion.LevelToValue(oldGreen);
            double blue = ColorConversion.LevelToValue(oldBlue);

            RgbColor output = new RgbColor(red, green, blue);
            return output;
        }

        private static RgbPixel ToRgbPixel(Coordinate coordinate, byte oldRed, byte oldGreen, byte oldBlue)
        {
            RgbColor rgbColor = Utilities.ToRgbColor(oldRed, oldBlue, oldGreen);

            RgbPixel output = new RgbPixel(coordinate, rgbColor);
            return output;
        }

        public static RgbImage ToRgbImage(Bitmap bitmap)
        {
            RgbImage output = new RgbImage();
            Utilities.HandleBitmap(output, bitmap, Utilities.ToRgbPixel);

            return output;
        }

        public static RgbImage LoadRgbImage(string filePath)
        {
            Bitmap bitmap = (Bitmap)Bitmap.FromFile(filePath);

            RgbImage output = Utilities.ToRgbImage(bitmap);
            return output;
        }

        private static HsvPixel ToHsvPixel(Coordinate coordinate, byte oldRed, byte oldGreen, byte oldBlue)
        {
            RgbColor rgbColor = Utilities.ToRgbColor(oldRed, oldGreen, oldBlue);

            HsvColor hsvColor = new HsvColor(rgbColor);

            HsvPixel output = new HsvPixel(coordinate, hsvColor);
            return output;
        }

        /// <remarks>
        /// Creating an HSV color requires creating an RGB color as an intermediate step. Consider creating a pixel image which retains the RGB information.
        /// </remarks>
        public static HsvImage ToHsvImage(Bitmap bitmap)
        {
            HsvImage output = new HsvImage();
            Utilities.HandleBitmap(output, bitmap, Utilities.ToHsvPixel);

            return output;
        }

        /// <remarks>
        /// Loading an HSV color requires creating an RGB color as an intermediate step. Consider loading a pixel image which retains the RGB information.
        /// </remarks>
        private static HsvImage LoadHsvImage(string filePath)
        {
            Bitmap bitmap = (Bitmap)Bitmap.FromFile(filePath);

            HsvImage output = Utilities.ToHsvImage(bitmap);
            return output;
        }

        // Gray Image

        private static GrayPixel ToGrayPixel(Coordinate coordinate, byte oldRed, byte oldGreen, byte oldBlue)
        {
            RgbColor rgbColor = Utilities.ToRgbColor(oldRed, oldGreen, oldBlue);

            GrayColor grayColor = new GrayColor(rgbColor);

            GrayPixel output = new GrayPixel(coordinate, grayColor);
            return output;
        }

        /// <remarks>
        /// Creating a Gray color requires creating an RGB color as an intermediate step. Consider creating a pixel image which retains the RGB information.
        /// </remarks>
        public static GrayImage ToGrayImage(Bitmap bitmap)
        {
            GrayImage output = new GrayImage();
            Utilities.HandleBitmap(output, bitmap, Utilities.ToGrayPixel);

            return output;
        }

        /// <remarks>
        /// Loading a Gray color requires creating an RGB color as an intermediate step. Consider loading a pixel image which retains the RGB information.
        /// </remarks>
        private static GrayImage LoadGrayImage(string filePath)
        {
            Bitmap bitmap = (Bitmap)Bitmap.FromFile(filePath);

            GrayImage output = Utilities.ToGrayImage(bitmap);
            return output;
        }

        // Pixel Image

        private static Pixel ToPixel(Coordinate coordinate, byte oldRed, byte oldGreen, byte oldBlue)
        {
            RgbColor rgbColor = Utilities.ToRgbColor(oldRed, oldGreen, oldBlue);

            HsvColor hsvColor = new HsvColor(rgbColor);
            GrayColor grayColor = new GrayColor(rgbColor);

            Pixel output = new Pixel(coordinate, rgbColor, hsvColor, grayColor);
            return output;
        }

        public static PixelImage ToPixelImage(Bitmap bitmap)
        {
            PixelImage output = new PixelImage();
            Utilities.HandleBitmap(output, bitmap, Utilities.ToPixel);

            return output;
        }

        public static PixelImage LoadPixelImage(string filePath)
        {
            Bitmap bitmap = (Bitmap)Bitmap.FromFile(filePath);

            PixelImage output = Utilities.ToPixelImage(bitmap);
            return output;
        }

        public static Bitmap CreateBitmap(int numberOfRows, int numberOfColumns, List<Coordinate> coordinates, Color color, bool included)
        {
            BitmapLocker locker = new BitmapLocker(numberOfRows, numberOfColumns, PixelFormat.Format32bppArgb);
            if (included)
            {
                foreach (Coordinate coordinate in coordinates)
                {
                    locker[coordinate.Row, coordinate.Column] = color;
                }
            }
            else
            {
                Dictionary<int, HashSet<int>> includedCoordinates = new Dictionary<int, HashSet<int>>();
                foreach (Coordinate coordinate in coordinates)
                {
                    HashSet<int> rowSet;
                    if (includedCoordinates.ContainsKey(coordinate.Row))
                    {
                        rowSet = includedCoordinates[coordinate.Row];
                    }
                    else
                    {
                        rowSet = new HashSet<int>();
                        includedCoordinates.Add(coordinate.Row, rowSet);
                    }

                    rowSet.Add(coordinate.Column);
                }

                for (int iRow = 0; iRow < numberOfRows; iRow++)
                {
                    for (int iCol = 0; iCol < numberOfColumns; iCol++)
                    {
                        if (!includedCoordinates.ContainsKey(iRow) || !includedCoordinates[iRow].Contains(iCol))
                        {
                            locker[iRow, iCol] = color;
                        }
                    }
                }
            }

            Bitmap output = locker.GetBitmap();
            return output;
        }

        public static void OverlayBitmap(Bitmap image, List<Coordinate> coordinates, Color color, bool included)
        {
            int numRows = image.Height;
            int numCols = image.Width;

            BitmapLocker locker = new BitmapLocker(image);
            if (included)
            {
                foreach (Coordinate coordinate in coordinates)
                {
                    locker[coordinate.Row, coordinate.Column] = color;
                }
            }
            else
            {
                Dictionary<int, HashSet<int>> includedCoordinates = new Dictionary<int, HashSet<int>>();
                foreach (Coordinate coordinate in coordinates)
                {
                    HashSet<int> rowSet;
                    if (includedCoordinates.ContainsKey(coordinate.Row))
                    {
                        rowSet = includedCoordinates[coordinate.Row];
                    }
                    else
                    {
                        rowSet = new HashSet<int>();
                        includedCoordinates.Add(coordinate.Row, rowSet);
                    }

                    rowSet.Add(coordinate.Column);
                }

                for (int iRow = 0; iRow < numRows; iRow++)
                {
                    for (int iCol = 0; iCol < numCols; iCol++)
                    {
                        if (!includedCoordinates.ContainsKey(iRow) || !includedCoordinates[iRow].Contains(iCol))
                        {
                            locker[iRow, iCol] = color;
                        }
                    }
                }
            }

            locker.UnlockBits();
        }
    }
}
