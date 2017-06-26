using System;


namespace PythonFromNet.Lib
{
    [Serializable]
    public class NumericImageData
    {
        public const int DefaultNumberOfImagePixels = 784; // 28 x 28.
        public const int DefaultTrueValue = -1;


        public double[] PixelValues { get; set; }
        public int TrueValue { get; set; }


        public NumericImageData()
            : this(new double[NumericImageData.DefaultNumberOfImagePixels], NumericImageData.DefaultTrueValue)
        {
        }

        public NumericImageData(double[] pixelValues, int trueValue)
        {
            this.PixelValues = pixelValues;
            this.TrueValue = trueValue;
        }
    }
}
