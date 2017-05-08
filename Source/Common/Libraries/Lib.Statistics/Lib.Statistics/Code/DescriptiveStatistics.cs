using System;
using System.Collections.Generic;


namespace Public.Common.Lib.Statistics
{
    public class DescriptiveStatistics
    {
        #region Static

        public static double MomentSum(IEnumerable<double> values, double aboutConstant, double power, out int count)
        {
            double output = 0;
            count = 0;
            foreach (double value in values)
            {
                double difference = value - aboutConstant;
                double increment = Math.Pow(difference, power);
                output += increment;

                count++;
            }

            return output;
        }

        public static double MomentSum(IEnumerable<double> values, double aboutConstant, double power)
        {
            int dummyCount;
            double output = DescriptiveStatistics.MomentSum(values, aboutConstant, power, out dummyCount);
            return output;
        }

        public static double Mean(IEnumerable<double> values, out int count)
        {
            double firstMomment = DescriptiveStatistics.MomentSum(values, 0, 1, out count);

            double output = firstMomment / (double)count;
            return output;
        }

        public static double Mean(IEnumerable<double> values)
        {
            int dummyCount;
            double output = DescriptiveStatistics.Mean(values, out dummyCount);
            return output;
        }

        public static double CentralMomentSum(IEnumerable<double> values, double power, out int count, out double mean)
        {
            mean = DescriptiveStatistics.Mean(values, out count);

            double output = DescriptiveStatistics.MomentSum(values, mean, power);
            return output;
        }

        public static double CentralMomentSum(IEnumerable<double> values, double power)
        {
            int dummyCount;
            double dummyMean;
            double output = DescriptiveStatistics.CentralMomentSum(values, power, out dummyCount, out dummyMean);
            return output;
        }

        private static double SecondCentralMomentSum(IEnumerable<double> values, out int count)
        {
            double dummyMean;
            double output = DescriptiveStatistics.CentralMomentSum(values, 2, out count, out dummyMean);
            return output;
        }

        /// <remarks>
        /// Sample variance is used when the values are merely samples from the entire population of possible values.
        /// I cannot think of a case in any empirical study when this is NOT the desired function.
        /// </remarks>
        public static double VarianceSample(IEnumerable<double> values)
        {
            int count;
            double secondCentralMomentSum = DescriptiveStatistics.SecondCentralMomentSum(values, out count);

            double output = secondCentralMomentSum / (double)(count - 1);
            return output;
        }

        /// <remarks>
        /// Population variance is used when the values are the entirety of all possible values.
        /// I cannot think of a case where this would be the desired function.
        /// </remarks>
        public static double VariancePopulation(IEnumerable<double> values)
        {
            int count;
            double secondCentralMomentSum = DescriptiveStatistics.SecondCentralMomentSum(values, out count);

            double output = secondCentralMomentSum / (double)count;
            return output;
        }

        public static double SkewSample(IEnumerable<double> values)
        {
            double varianceSample = DescriptiveStatistics.VarianceSample(values);

            int count;
            double dummyMean;
            double thirdCentralMomentSum = DescriptiveStatistics.CentralMomentSum(values, 3, out count, out dummyMean);

            double varianceStandardizationMultiplier = 1 / Math.Pow(varianceSample, 3.0 / 2.0);
            double countDouble = count;
            double countMultiplier = countDouble / ((countDouble - 1) * (countDouble - 2));
            double multiplier = countMultiplier * varianceStandardizationMultiplier;

            double output = thirdCentralMomentSum * multiplier;
            return output;
        }

        public static double SkewPopulation(IEnumerable<double> values)
        {
            double variancePop = DescriptiveStatistics.VariancePopulation(values);

            int count;
            double dummyMean;
            double thirdCentralMomentSum = DescriptiveStatistics.CentralMomentSum(values, 3, out count, out dummyMean);

            double varianceStandardizationMultiplier = 1 / Math.Pow(variancePop, 3.0 / 2.0);
            double multiplier = 1 / (double)count * varianceStandardizationMultiplier;

            double output = thirdCentralMomentSum * multiplier;
            return output;
        }

        /// <remarks>
        /// This is generally the kurtosis of interest, and is the only kurtosis function supported by Excel.
        /// </remarks>
        public static double KurtosisSampleExcess(IEnumerable<double> values)
        {
            double varianceSample = DescriptiveStatistics.VarianceSample(values);

            int count;
            double dummyMean;
            double fourthCentralMomentSum = DescriptiveStatistics.CentralMomentSum(values, 4, out count, out dummyMean);

            double varianceStandardizationMultiplier = 1 / Math.Pow(varianceSample, 2);
            double countDouble = count;
            double countMultiplier = countDouble * (countDouble + 1) / ((countDouble - 1) * (countDouble - 2) * (countDouble - 3));
            double multiplier = varianceStandardizationMultiplier * countMultiplier;
            double subtractand = 3 * Math.Pow(countDouble - 1, 2) / ((countDouble - 2) * (countDouble - 3));

            double output = fourthCentralMomentSum * multiplier - subtractand;
            return output;
        }

        #endregion
    }
}
