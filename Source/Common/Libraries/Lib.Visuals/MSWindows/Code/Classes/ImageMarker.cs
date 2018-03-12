using System;
using System.Collections.Generic;
using System.Drawing;
using SysImageFormat = System.Drawing.Imaging.ImageFormat;

namespace Public.Common.Lib.Visuals.MSWindows
{
    public class ImageMarker
    {
        #region Static

        public static void MarkImage(string inputImagePath, string outputImagePath, int pixelX, int pixelY)
        {
            var image = Image.FromFile(inputImagePath);
            using (var pen = new Pen(Color.LightGreen, 20))
            using (var graphics = Graphics.FromImage(image))
            {
                graphics.DrawEllipse(pen, pixelX - 5, pixelY - 5, 10, 10);
            }

            image.Save(outputImagePath);
        }

        public static void MarkImage(string inputImagePath, string outputImagePath, IEnumerable<Tuple<int, int>> pixelLocations)
        {
            ImageMarker.MarkImage(inputImagePath, outputImagePath, pixelLocations, SysImageFormat.Jpeg, true); // Ignore the dummy specified image format.
        }

        public static void MarkImage(string inputImagePath, string outputImagePath, IEnumerable<Tuple<int, int>> pixelLocations, SysImageFormat imageFormat, bool useDefaultImageFormat = true)
        {
            var image = Image.FromFile(inputImagePath);
            using (var pen = new Pen(Color.LightGreen, 1))
            using (var graphics = Graphics.FromImage(image))
            {
                var points = new List<Point>();
                foreach (var pixelLocation in pixelLocations)
                {
                    points.Add(new Point(pixelLocation.Item1, pixelLocation.Item2));
                }

                graphics.DrawPolygon(pen, points.ToArray());
            }

            if (useDefaultImageFormat)
            {
                image.Save(outputImagePath);
            }
            else
            {
                image.Save(outputImagePath, imageFormat);
            }
        }

        #endregion
    }
}
