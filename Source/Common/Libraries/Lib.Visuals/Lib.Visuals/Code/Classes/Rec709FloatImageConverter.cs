using System;
using System.Collections.Generic;


namespace Public.Common.Lib.Visuals
{
    public class Rec709FloatImageConverter : IRgbToGrayImageConverter
    {
        private const float Rec709LuminanceRedWeight = 0.2126f;
        private const float Rec709LuminanceGreenWeight = 0.7125f;
        private const float Rec709LuminanceBlueWeight = 0.0722f;


        #region Static

        public static GrayFloatImage ToGray(RgbFloatImage rgbImage)
        {
            int rows = rgbImage.Rows;
            int cols = rgbImage.Columns;

            GrayFloatImage output = new GrayFloatImage(rows, cols);

            float[] sourceValues = rgbImage.Data;
            float[] destinationValues = output.Data;
            int iSourceIndex = 0;
            int iDestinationIndex = 0;
            for (int iRow = 0; iRow < rows; iRow++)
            {
                for (int iCol = 0; iCol < cols; iCol++)
                {
                    float grayValue =
                        Rec709FloatImageConverter.Rec709LuminanceRedWeight * sourceValues[iSourceIndex] +
                        Rec709FloatImageConverter.Rec709LuminanceGreenWeight * sourceValues[iSourceIndex + 1] +
                        Rec709FloatImageConverter.Rec709LuminanceBlueWeight * sourceValues[iSourceIndex + 2];
                    destinationValues[iDestinationIndex] = grayValue;

                    iSourceIndex += RgbFloatImage.NumberOfRgbColorChannels;
                    iDestinationIndex += GrayFloatImage.NumberOfGrayColorChannels;
                }
            }

            return output;
        }

        #endregion


        public GrayFloatImage this[RgbFloatImage rgbImage] => Rec709FloatImageConverter.ToGray(rgbImage);
    }
}
