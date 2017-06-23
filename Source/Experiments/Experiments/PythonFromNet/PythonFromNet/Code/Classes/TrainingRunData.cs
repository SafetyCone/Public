using System;


namespace PythonFromNet
{
    [Serializable]
    public class TrainingRunData
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int[] NetworkLayerSizes { get; set; }
        public double Eta { get; set; }
        public int MiniBatchSize { get; set; }


        public TrainingRunData()
        {
        }
    }
}
