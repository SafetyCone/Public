using System;


namespace Public.Common.Lib.Visuals
{
    public class ImageConverter : IRgbByteToRgbFloatImageConverter
    {
        #region Static

        public static RgbFloatImage ToRgbFloatImage(RgbByteImage byteImage)
        {
            byte[] byteValues = byteImage.Data;

            float byteMax = 255.0f;

            int numValues = byteValues.Length;
            float[] floatValues = new float[numValues];
            for (int iValue = 0; iValue < numValues; iValue++)
            {
                byte byteValue = byteValues[iValue];
                float byteValueFloat = Convert.ToSingle(byteValue);
                float value = byteValueFloat / byteMax;
                floatValues[iValue] = value;
            }

            RgbFloatImage output = new RgbFloatImage(byteImage.Rows, byteImage.Columns, floatValues);
            return output;
        }

        #endregion


        public RgbFloatImage this[RgbByteImage byteImage] => ImageConverter.ToRgbFloatImage(byteImage);
    }
}
