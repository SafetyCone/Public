using System;


namespace Public.NeuralNetworks.MNIST
{
    [Serializable]
    public class MnistImage
    {
        public const int SizeInPixels = 28;
        public const int ImageWidthInPixels = MnistImage.SizeInPixels;
        public const int ImageHeightInPixels = MnistImage.SizeInPixels; // Square.
        public const int NumberOfImagePixels = 784; // 28 x 28.
        public const int DefaultTrueValue = -1;


        public double[] PixelValues { get; set; }
        public int TrueValue { get; set; }


        public MnistImage()
            : this(new double[MnistImage.NumberOfImagePixels], MnistImage.DefaultTrueValue)
        {
        }

        public MnistImage(double[] pixelValues, int trueValue)
        {
            this.PixelValues = pixelValues;
            this.TrueValue = trueValue;
        }
    }
}
