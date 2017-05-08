using System;
using System.Collections.Generic;

using Public.Common.Excel;


namespace Public.Common.Georges
{
    public class ExcelRandomValueStatistics : IDisposable
    {
        public const string ExcelStatisticsFilePath = @"C:\Organizations\Minex\Data\Random Value Statistics.xlsx";
        public const string RandomValuesRangeName = @"RandomValues";
        public const string MeanValueRangeName = @"MeanValue";
        public const string VarianceSampleRangeName = @"VarianceSample";
        public const string VariancePopulationRangeName = @"VariancePopulation";
        public const string SkewSampleRangeName = @"SkewSample";
        public const string SkewPopulationRangeName = @"SkewPopulation";
        public const string KurtosisValueRangeName = @"KurtosisValue";


        #region IDisposable Members

        private bool zDisposed = false;


        public void Dispose()
        {
            this.CleanUp(true);

            GC.SuppressFinalize(this);
        }

        private void CleanUp(bool disposing)
        {
            if (!this.zDisposed)
            {
                if (disposing)
                {
                    // Call Dispose() on any contained managed disposable resources here.
                }

                // Clean-up unmanaged resources here.
            }

            this.zDisposed = true;
        }

        ~ExcelRandomValueStatistics()
        {
            this.CleanUp(false);
        }

        #endregion


        private ExcelApplication zApp;
        private Workbook zWkbk;

        
        public ExcelRandomValueStatistics()
        {
            this.zApp = new ExcelApplication();
            this.zWkbk = this.zApp.OpenWorkbook(ExcelRandomValueStatistics.ExcelStatisticsFilePath);
        }

        public List<double> GetConstantRandomValues()
        {
            Range rng = this.zWkbk.GetNamedRange(ExcelRandomValueStatistics.RandomValuesRangeName);

            object[,] values = rng.Values; // Excel starts at 1.

            int upperLimit = values.GetLength(0) + 1;
            List<double> output = new List<double>();
            for (int iRow = 1; iRow < upperLimit; iRow++)
            {
                double value = Convert.ToDouble(values[iRow, 1]);
                output.Add(value);
            }

            return output;
        }

        public double GetMean()
        {
            Range rng = this.zWkbk.GetNamedRange(ExcelRandomValueStatistics.MeanValueRangeName);

            double output = rng.ValueDouble;
            return output;
        }

        public double GetVarianceSample()
        {
            Range rng = this.zWkbk.GetNamedRange(ExcelRandomValueStatistics.VarianceSampleRangeName);

            double output = rng.ValueDouble;
            return output;
        }

        public double GetVariancePopulation()
        {
            Range rng = this.zWkbk.GetNamedRange(ExcelRandomValueStatistics.VariancePopulationRangeName);

            double output = rng.ValueDouble;
            return output;
        }

        public double GetSkewSample()
        {
            Range rng = this.zWkbk.GetNamedRange(ExcelRandomValueStatistics.SkewSampleRangeName);

            double output = rng.ValueDouble;
            return output;
        }

        public double GetSkewPopulation()
        {
            Range rng = this.zWkbk.GetNamedRange(ExcelRandomValueStatistics.SkewPopulationRangeName);

            double output = rng.ValueDouble;
            return output;
        }

        /// <remarks>
        /// Note, Excel only calculations sample excess kurtosis.
        /// </remarks>
        public double GetKurtosis()
        {
            Range rng = this.zWkbk.GetNamedRange(ExcelRandomValueStatistics.KurtosisValueRangeName);

            double output = rng.ValueDouble;
            return output;
        }
    }
}
