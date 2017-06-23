using System;


namespace PythonFromNet
{
    [Serializable]
    public class CharacterImageData
    {
        public const int DefaultNumberOfImagePixels = 784; // 28 x 28.


        public float[] PixelData { get; set; }
        public int TrueValue { get; set; }


        public CharacterImageData()
            : this(CharacterImageData.DefaultNumberOfImagePixels)
        {
        }

        public CharacterImageData(int numberOfImagePixels)
        {
            this.PixelData = new float[numberOfImagePixels];
        }

        public CharacterImageData(float[] pixelData, int trueValue)
        {
            this.PixelData = pixelData;
            this.TrueValue = trueValue;
        }
    }
}
