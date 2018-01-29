using System.Drawing;


namespace Public.Common.Lib.Visuals.MSWindows
{
    public static class RgbByteImageExtensions
    {
        public static Bitmap ToBitmap(this RgbByteImage rgbByteImage)
        {
            Bitmap output = BitmapConverter.ToBitmap(rgbByteImage);
            return output;
        }
    }
}
