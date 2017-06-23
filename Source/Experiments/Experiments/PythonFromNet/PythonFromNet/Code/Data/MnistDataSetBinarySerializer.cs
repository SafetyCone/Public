using System;
using System.Collections.Generic;
using System.IO;


namespace PythonFromNet.Code.Classes
{
    /// <remarks>
    /// Hard-coded image size to 28 x 28 = 784 pixels.
    /// </remarks>
    public class MnistDataSetBinarySerializer
    {
        #region Static

        public static void Serialize(string filePath, MnistDataSet dataSet)
        {
            int numTrainingImages = dataSet.TrainingData.Count;
            int numValidationImages = dataSet.ValidationData.Count;
            int numTestImages = dataSet.TestData.Count;


        }

        private static void Serialize(FileStream fStream, List<CharacterImageData> images)
        {
            byte[] bits;

            int numImages = images.Count;
            bits = BitConverter.GetBytes(numImages);

            fStream.Write(bits, 0, bits.Length);

            foreach(CharacterImageData image in images)
            {

            }
        }

        #endregion
    }
}
