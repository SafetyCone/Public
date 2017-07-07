using System;
using System.Collections.Generic;


namespace Public.NeuralNetworks.MNIST
{
    [Serializable]
    public class MnistData
    {
        public List<MnistImage> TrainingData { get; set; }
        public List<MnistImage> ValidationData { get; set; }
        public List<MnistImage> TestData { get; set; }


        public MnistData()
        {
        }

        public MnistData(List<MnistImage> trainingData, List<MnistImage> validationData, List<MnistImage> testData)
        {
            this.TrainingData = trainingData;
            this.ValidationData = validationData;
            this.TestData = testData;
        }
    }
}
