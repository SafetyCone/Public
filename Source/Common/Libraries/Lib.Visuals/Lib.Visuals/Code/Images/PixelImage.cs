using System.Drawing;


namespace Minex.Common.Lib.Visuals
{
    public class PixelImage : GenericCoordinatedImage<Pixel>
    {
        #region Static

        public static PixelImage FromBitmap(Bitmap bitmap)
        {
            PixelImage output = Utilities.ToPixelImage(bitmap);
            return output;
        }

        #endregion


        public PixelImage() : base() { }

        public PixelImage(int rows, int columns) : base(rows, columns) { }
    }
}
