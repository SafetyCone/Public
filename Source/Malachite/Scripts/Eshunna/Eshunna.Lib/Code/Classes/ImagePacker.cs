using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

using Public.Common.Lib.Visuals;


namespace Eshunna.Lib
{
    public class ImagePacker
    {
        #region Static

        public static Tuple<Bitmap, List<List<Location2Integer>>> PackImages(List<Bitmap> miniImages, List<List<Location2Integer>> vertexPixelsPerMiniImage)
        {
            // Create image sizes.
            int nMiniImages = miniImages.Count;
            var sizesAndIndices = new List<Tuple<ImageSize, int>>(nMiniImages);
            int index = 0;
            foreach (var miniImage in miniImages)
            {
                ImageSize size = new ImageSize(miniImage.Height, miniImage.Width);
                var sizeAndIndex = Tuple.Create(size, index++);
                sizesAndIndices.Add(sizeAndIndex);
            }

            // Sort by mini-image height.
            sizesAndIndices.Sort(ImagePacker.HeightComparer); // Smallest to largest.
            sizesAndIndices.Reverse(); // Largest to smallest.

            // Determine bounding rectangles for all mini-images.
            int compositeImageWidth = 1000;
            int miniImageIndex = 0;
            var rowFirstImageIndices = new List<int>();
            int rowWidth = compositeImageWidth + 1; // Ensure an initial reset on the first image.
            int priorImageHeight = 0;
            int currentImageHeight = 0;
            var miniImageRectangles = new List<RectangleInteger>();
            foreach (var sizeAndIndex in sizesAndIndices)
            {
                rowWidth += sizeAndIndex.Item1.Width;
                if (rowWidth >= compositeImageWidth)
                {
                    rowFirstImageIndices.Add(miniImageIndex);
                    rowWidth = 0; // Reset.

                    priorImageHeight = currentImageHeight;
                    currentImageHeight += sizeAndIndex.Item1.Height;
                }

                var miniImageRectangle = new RectangleInteger(rowWidth, priorImageHeight, sizeAndIndex.Item1.Width, sizeAndIndex.Item1.Height);
                miniImageRectangles.Add(miniImageRectangle);

                miniImageIndex++;
            }

            // Create the composite image.
            int compositeImageHeight = currentImageHeight;
            var compositeImage = new Bitmap(compositeImageWidth, compositeImageHeight, PixelFormat.Format24bppRgb);
            using (var gfx = Graphics.FromImage(compositeImage))
            {
                for (int iMiniImage = 0; iMiniImage < nMiniImages; iMiniImage++)
                {
                    var sizeAndIndex = sizesAndIndices[iMiniImage];
                    var miniImageRectangle = miniImageRectangles[iMiniImage];
                    var miniImage = miniImages[sizeAndIndex.Item2];
                    gfx.DrawImage(miniImage, new Point(miniImageRectangle.X, miniImageRectangle.Y));
                }
            }

            // Translate all mini-image vertex pixel locations.
            var translatedVertexPixelsPerMiniImage = new List<List<Location2Integer>>(vertexPixelsPerMiniImage);
            for (int iMiniImage = 0; iMiniImage < nMiniImages; iMiniImage++)
            {
                var sizeAndIndex = sizesAndIndices[iMiniImage];
                var miniImageRectangle = miniImageRectangles[iMiniImage];

                var vertexPixels = vertexPixelsPerMiniImage[sizeAndIndex.Item2];
                var translatedVertexPixels = vertexPixels + new Location2Integer(miniImageRectangle.X, miniImageRectangle.Y);
                translatedVertexPixelsPerMiniImage[sizeAndIndex.Item2] = translatedVertexPixels;
            }

            var output = Tuple.Create(compositeImage, translatedVertexPixelsPerMiniImage);
            return output;
        }

        private static int HeightComparer(Tuple<ImageSize, int> a, Tuple<ImageSize, int> b)
        {
            int output = a.Item1.Height.CompareTo(b.Item1.Height);
            return output;
        }

        #endregion
    }
}
