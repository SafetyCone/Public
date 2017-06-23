using System;
using System.Collections.Generic;


namespace PythonFromNet
{
    [Serializable]
    public class MnistDataSet
    {
        public List<CharacterImageData> TrainingData { get; set; }
        public List<CharacterImageData> ValidationData { get; set; }
        public List<CharacterImageData> TestData { get; set; }


        public MnistDataSet()
        {
        }
    }
}
