using System;
using System.Collections.Generic;

using Public.Common.Lib.Visuals;


namespace Eshunna.Lib
{
    /// <summary>
    /// Scales an image down to a maximum allowable size. The maximum size is given in terms of a maximum dimension size, such that whether landscape or portrait oriented (whether height or width respectively is the maximum dimension), the maximum dimension will be as specified.
    /// Images for which the maximum dimension size is already smaller than the desired maximum dimension size will remain the same size (scale factor of 1). I.e., these images will NOT be scaled up.
    /// </summary>
    public class MaximumDimensionSizeImageScaler
    {
        #region Static

        public static readonly int MaximumAllowedImageDimension = 800;


        /// <summary>
        /// Gets the scale factor required to shrink an image from its current size to a size that has a largest dimension equal to the maximum allowed dimension size.
        /// </summary>
        /// <param name="imageSize"></param>
        /// <returns></returns>
        public static double ScaleFactorToMaximumAllowedImageDimension(ImageSize imageSize)
        {
            int largestImageDimension = imageSize.Width > imageSize.Height ? imageSize.Width : imageSize.Height;

            double maximumAllowedImageDimension = Convert.ToDouble(MaximumDimensionSizeImageScaler.MaximumAllowedImageDimension);
            double largestImageDimensionDouble = Convert.ToDouble(largestImageDimension);

            double output;
            if(largestImageDimensionDouble > maximumAllowedImageDimension)
            {
                output = largestImageDimension / maximumAllowedImageDimension;
            }
            else
            {
                output = 1;
            }
            return output;
        }

        public static double ScaleFactorFromMaximumAllowedImageDimension(ImageSize current, ImageSize original)
        {
            int largestCurrentImageDimension = current.Width > current.Height ? current.Width : current.Height;
            int largestOriginalImageDimension = original.Width > original.Height ? original.Width : original.Height;

            double largestCurrentImageDimensionDbl = Convert.ToDouble(largestCurrentImageDimension);
            double largestOriginalImageDimensionDbl = Convert.ToDouble(largestOriginalImageDimension);

            double output = largestOriginalImageDimensionDbl / largestCurrentImageDimensionDbl;
            return output;
        }

        #endregion
    }
}
