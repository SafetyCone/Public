using System;
using System.Linq;

using MathNet.Numerics.Distributions;
using MathNet.Numerics.Statistics;


namespace Public.Common.Jules
{
    class Program
    {
        private static void Main(string[] args)
        {
            Random rand = new Random(0);
            var samples = new ContinuousUniform(0, 1, rand).Samples().Take(1000);
            var statistics = new DescriptiveStatistics(samples);

            var mean = statistics.Mean;
            var variance = statistics.Variance;
            var skewness = statistics.Skewness;
            var kurtosis = statistics.Kurtosis;
        }
    }
}
