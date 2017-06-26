using System;
using System.Collections.Generic;


namespace PythonFromNet.Lib
{
    [Serializable]
    public class MnistData
    {
        public List<NumericImageData> TrainingData { get; set; }
        public List<NumericImageData> ValidationData { get; set; }
        public List<NumericImageData> TestData { get; set; }


        public MnistData()
        {
        }
    }
}
