using System;
using System.Drawing;
//using System.Drawing.

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

        public static void MarkImage(string inputImagePath, string outputImagePath, Tuple<int, int>[] pixelLocations)
        {
            int nPoints = pixelLocations.Length;

            var image = Image.FromFile(inputImagePath);
            using (var pen = new Pen(Color.LightGreen, 3))
            using (var graphics = Graphics.FromImage(image))
            {
                Point[] points = new Point[nPoints];
                for (int iPoint = 0; iPoint < nPoints; iPoint++)
                {
                    points[iPoint] = new Point(pixelLocations[iPoint].Item1, pixelLocations[iPoint].Item2);
                }

                graphics.DrawPolygon(pen, points);
            }

            image.Save(outputImagePath);
        }

        #endregion
    }
}
