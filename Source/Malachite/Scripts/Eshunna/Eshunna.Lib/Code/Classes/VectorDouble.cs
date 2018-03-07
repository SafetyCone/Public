using System;
using System.Collections.Generic;


namespace Eshunna.Lib
{
    public static class VectorDouble
    {
        public static double DotProduct(double[] values1, double[] values2)
        {
            int nValues = values1.Length;
            double output = 0;
            for (int iValue = 0; iValue < nValues; iValue++)
            {
                double value1 = values1[iValue];
                double value2 = values2[iValue];
                double product = value1 * value2;
                output += product;
            }
            return output;
        }

        public static double L2NormSquared(IEnumerable<double> values)
        {
            double output = 0;
            foreach (var value in values)
            {
                output += (value * value);
            }
            return output;
        }

        public static double L2Norm(IEnumerable<double> values)
        {
            double squaredSum = VectorDouble.L2NormSquared(values);

            double output = Convert.ToSingle(Math.Sqrt(squaredSum));
            return output;
        }

        public static double[] L2Normalize(double[] values)
        {
            double l2Norm = VectorDouble.L2Norm(values);

            int nValues = values.Length;
            double[] output = new double[nValues];
            for (int iValue = 0; iValue < nValues; iValue++)
            {
                double value = values[iValue];
                double normalizedValue = value / l2Norm;
                output[iValue] = normalizedValue;
            }
            return output;
        }
    }
}
