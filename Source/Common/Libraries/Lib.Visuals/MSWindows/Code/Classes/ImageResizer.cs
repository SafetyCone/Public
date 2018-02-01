using System;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace Public.Common.Lib.Visuals.MSWindows
{
    public class ImageResizer : IImageResizer
    {
        public RgbByteImage Downsize(RgbByteImage input, int maximumDimensionSize)
        {
            RgbByteImage output;

            int originalHeight = input.Height;
            int originalWidth = input.Width;

            int originalMaximumDimension = originalHeight > originalWidth ? originalHeight : originalWidth;
            if(originalMaximumDimension > maximumDimensionSize)
            {
                float multiplier = Convert.ToSingle(maximumDimensionSize) / Convert.ToSingle(originalMaximumDimension);
                int newHeight = originalHeight > originalWidth ? maximumDimensionSize : Convert.ToInt32(Convert.ToSingle(originalHeight) * multiplier);
                int newWidth = originalWidth > originalHeight ? maximumDimensionSize : Convert.ToInt32(Convert.ToSingle(originalWidth) * multiplier);

                Bitmap oldBitmap = input.ToBitmap();
                Bitmap newBitmap = new Bitmap(newWidth, newHeight);
                using (Graphics graphics = Graphics.FromImage(newBitmap))
                {
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.DrawImage(oldBitmap, 0, 0, newWidth, newHeight);
                }

                output = newBitmap.ToRgbByteImage();
            }
            else
            {
                // No need to resize the image. Return a new image to keep a consistent behavior with the resized case.
                output = new RgbByteImage(input);
            }

            return output;
        }
    }
}
