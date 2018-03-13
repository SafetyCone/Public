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
            int targetCompositeImageWidth = 1000; // Might be more than this.
            int miniImageIndex = 0;
            int upperLeftX = 0;
            int upperLeftY = 0;
            var rowFirstImageIndices = new List<int>();
            var miniImageRectangles = new List<RectangleInteger>();
            int rowWidth = targetCompositeImageWidth; // Make sure the first image causes a reset below and is added as the first image of a row.
            int maxRowWidth = targetCompositeImageWidth;
            int rowHeight = 0;
            int imageHeight = 0;
            foreach (var sizeAndIndex in sizesAndIndices)
            {
                int miniImageWidth = sizeAndIndex.Item1.Width;
                int miniImageHeight = sizeAndIndex.Item1.Height;

                // If adding this image to the row would cause the row to be wider than the target composite image width, make it the first image of the next row.
                // Ensure to do this only once, so that large mini-images will actually be placed instead of infinite looping.
                if(rowWidth + miniImageWidth > targetCompositeImageWidth)
                {
                    if(rowWidth > maxRowWidth)
                    {
                        maxRowWidth = rowWidth;
                    }

                    rowFirstImageIndices.Add(miniImageIndex);

                    upperLeftX = 0;
                    upperLeftY += rowHeight; // Prior row height.
                    rowWidth = 0;
                    rowHeight = miniImageHeight; // New row height. Mini-images had been sorted according to height, largest to smallest, thus all following mini-images will have less height than this first mini-image of the row.
                    imageHeight += miniImageHeight;
                }

                var miniImageRectangle = new RectangleInteger(upperLeftX, upperLeftY, miniImageWidth, miniImageHeight);
                miniImageRectangles.Add(miniImageRectangle);

                upperLeftX += miniImageWidth;
                rowWidth += miniImageWidth;

                miniImageIndex++;
            }

            // Create the composite image.
            int compositeImageHeight = imageHeight;
            var compositeImage = new Bitmap(maxRowWidth, compositeImageHeight, PixelFormat.Format24bppRgb);
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
