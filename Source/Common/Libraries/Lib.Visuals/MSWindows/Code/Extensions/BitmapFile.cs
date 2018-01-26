using System.Drawing;
using System.Drawing.Imaging;

using RgbColorByte = Public.Common.Lib.Visuals.RgbColor<byte>;


namespace Public.Common.Lib.Visuals.MSWindows
{
    public static class BitmapFileExtensions
    {
        public static Bitmap ToBitmap(BitmapFile file)
        {
            int widthX = file.Header.DIBHeader.WidthX;
            int heightY = file.Header.DIBHeader.HeightY;

            Bitmap bitmap = new Bitmap(widthX, heightY, PixelFormat.Format24bppRgb);
            for (int iRow = 0; iRow < heightY; iRow++)
            {
                for (int iCol = 0; iCol < widthX; iCol++)
                {
                    RgbColorByte rgbColor = file[iRow, iCol];
                    Color color = ColorConversion.RgbToColor(rgbColor);
                    bitmap.SetPixel(iCol, iRow, color);
                }
            }

            return bitmap;
        }
    }
}
