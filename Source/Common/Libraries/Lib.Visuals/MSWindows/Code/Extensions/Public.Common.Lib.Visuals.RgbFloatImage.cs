using System.Drawing;


namespace Public.Common.Lib.Visuals.MSWindows
{
    public static class RgbFloatImageExtensions
    {
		public static Bitmap ToBitmap(this RgbFloatImage rgbFloatImage)
        {
            Bitmap output = BitmapConverter.ToBitmap(rgbFloatImage);
            return output;
        }
    }
}
