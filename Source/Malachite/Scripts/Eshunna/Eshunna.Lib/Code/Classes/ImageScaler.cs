using System;

using Public.Common.Lib.Visuals;


namespace Eshunna.Lib
{
    public class ImageScaler
    {
        #region Static

        public static double MaximumDimensionScaleFactorGet(ImageSize from, ImageSize to)
        {
            int maxFromDimensionSize = from.Width > from.Height ? from.Width : from.Height;
            int maxToDimensionSize = to.Width > to.Height ? to.Width : to.Height;

            double maxFromDbl = Convert.ToDouble(maxFromDimensionSize);
            double maxToDbl = Convert.ToDouble(maxToDimensionSize);

            double output = maxToDbl / maxFromDbl;
            return output;
        }

        #endregion
    }
}
