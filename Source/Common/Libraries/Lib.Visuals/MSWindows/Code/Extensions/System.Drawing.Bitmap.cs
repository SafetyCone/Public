using System.Drawing;


namespace Public.Common.Lib.Visuals.MSWindows
{
    public static class BitmapExtensions
    {
		public static RgbFloatImage ToRgbFloatImage(this Bitmap bitmap)
        {
            RgbFloatImage output = BitmapConverter.ToRgbFloatImage(bitmap);
            return output;
        }

        public static RgbByteImage ToRgbByteImage(this Bitmap bitmap)
        {
            RgbByteImage output = BitmapConverter.ToRgbByteImage(bitmap);
            return output;
        }
    }
}
