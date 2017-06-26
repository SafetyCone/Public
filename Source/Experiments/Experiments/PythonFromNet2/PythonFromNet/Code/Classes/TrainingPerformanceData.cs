using System;
using System.Collections.Generic;


namespace PythonFromNet
{
    [Serializable]
    public class TrainingPerformanceData
    {
        public TrainingRunData Run { get; set; }
        public TimeSpan TotalTrainingElapsedTime { get; set; }
        public List<EpochPerformanceData> Epochs { get; protected set; }
        public int NumberOfTestSamples { get; set; }


        public TrainingPerformanceData()
        {
            this.Epochs = new List<EpochPerformanceData>();
        }
    }
}
