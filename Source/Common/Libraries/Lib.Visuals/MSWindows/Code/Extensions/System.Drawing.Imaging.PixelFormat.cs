using System.Drawing.Imaging;


namespace Public.Common.Lib.Visuals.MSWindows
{
    public static class PixelFormatExtensions
    {
		public static int BytesPerPixel(this PixelFormat pixelFormat)
        {
            int output = -1;
			switch(pixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                    output = 3;
                    break;

                case PixelFormat.Format32bppArgb:
                    output = 4;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<PixelFormat>(pixelFormat);
            }

            return output;
        }
    }
}
