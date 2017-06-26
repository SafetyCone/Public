using System;


namespace PythonFromNet
{
    [Serializable]
    public class EpochPerformanceData
    {
        public TimeSpan TrainElapsedTime { get; set; }
        public TimeSpan EvaluateElapsedTime { get; set; }
        public TimeSpan TotalElapsedTime
        {
            get
            {
                TimeSpan output = this.TrainElapsedTime + this.EvaluateElapsedTime;
                return output;
            }
        }
        public int NumberEvaluatedCorrectly { get; set; }


        public EpochPerformanceData()
        {
        }
    }
}
