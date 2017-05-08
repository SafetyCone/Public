using System;
using System.Collections.Generic;
using MathNet.Numerics.Statistics;

using Public.Common.Georges.Lib;
using MyStatistics = Public.Common.Georges.Lib.Statistics;



namespace Public.Common.Georges
{
    class Program
    {
        private static void Main(string[] args)
        {
            Program.TestAllMathNetStatisticsVsExcel();
            //Program.TestSampleExcessKurtosisVsExcel();
            //Program.TestPopulationSkewVsExcel();
            //Program.TestSampleSkewVsExcel();
            //Program.TestPopulationVarianceVsExcel();
            //Program.TestSampleVarianceVsExcel();
            //Program.TestMeanVsExcel();
            //Program.TestGetRandomValues();
        }

        private static void TestAllMathNetStatisticsVsExcel()
        {
            ExcelRandomValueStatistics statistics = new ExcelRandomValueStatistics();

            List<double> values = statistics.GetConstantRandomValues();
            double excelMean = statistics.GetMean();
            double excelVarianceSample = statistics.GetVarianceSample();
            double excelVariancePopulation = statistics.GetVariancePopulation();
            double excelSkewSample = statistics.GetSkewSample();
            double excelSkewPopulation = statistics.GetSkewPopulation();
            double excelKurtosisSampleExtra = statistics.GetKurtosis();

            DescriptiveStatistics descriptiveStats = new DescriptiveStatistics(values);

            double varianceSample = descriptiveStats.Variance;
            double skewSample = descriptiveStats.Skewness;
            double kurtosisSampleExtra = descriptiveStats.Kurtosis;
        }

        private static void TestSampleExcessKurtosisVsExcel()
        {
            ExcelRandomValueStatistics statistics = new ExcelRandomValueStatistics();

            List<double> values = statistics.GetConstantRandomValues();
            double excelKurtosis = statistics.GetKurtosis();

            double calculatedKurtosisSampleExcess = MyStatistics.KurtosisSampleExcess(values);

            Program.CompareDoubles(excelKurtosis, calculatedKurtosisSampleExcess);
        }

        private static void TestPopulationSkewVsExcel()
        {
            ExcelRandomValueStatistics statistics = new ExcelRandomValueStatistics();

            List<double> values = statistics.GetConstantRandomValues();
            double excelSkewPop = statistics.GetSkewPopulation();

            double calculatedSkewPop = MyStatistics.SkewPopulation(values);

            Program.CompareDoubles(excelSkewPop, calculatedSkewPop);
        }

        private static void TestSampleSkewVsExcel()
        {
            ExcelRandomValueStatistics statistics = new ExcelRandomValueStatistics();

            List<double> values = statistics.GetConstantRandomValues();
            double excelSkewSample = statistics.GetSkewSample();

            double calculatedSkewSample = MyStatistics.SkewSample(values);

            Program.CompareDoubles(excelSkewSample, calculatedSkewSample);
        }

        private static void TestPopulationVarianceVsExcel()
        {
            ExcelRandomValueStatistics statistics = new ExcelRandomValueStatistics();

            List<double> values = statistics.GetConstantRandomValues();
            double excelPopVar = statistics.GetVarianceSample();

            double calculatedPopVar = MyStatistics.VarianceSample(values);

            Program.CompareDoubles(excelPopVar, calculatedPopVar);
        }

        private static void TestSampleVarianceVsExcel()
        {
            ExcelRandomValueStatistics statistics = new ExcelRandomValueStatistics();

            List<double> values = statistics.GetConstantRandomValues();
            double excelSampleVar = statistics.GetVarianceSample();

            double calculatedSampleVar = MyStatistics.VarianceSample(values);

            Program.CompareDoubles(excelSampleVar, calculatedSampleVar);
        }

        private static void TestMeanVsExcel()
        {
            ExcelRandomValueStatistics statistics = new ExcelRandomValueStatistics();

            List<double> values = statistics.GetConstantRandomValues();
            double excelMean = statistics.GetMean();

            double calculatedMean = MyStatistics.Mean(values);

            Program.CompareDoubles(excelMean, calculatedMean);
        }

        private static void CompareDoubles(double num1, double num2)
        {
            if (Math.Abs(num1 - num2) > Math.Abs(0.000001 * num1))
            {
                throw new Exception();
            }
        }

        private static void TestGetRandomValues()
        {
            ExcelRandomValueStatistics statistics = new ExcelRandomValueStatistics();

            List<double> values = statistics.GetConstantRandomValues();
        }
    }
}
