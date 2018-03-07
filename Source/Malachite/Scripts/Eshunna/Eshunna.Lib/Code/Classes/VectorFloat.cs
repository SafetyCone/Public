using System;
using System.Collections.Generic;


namespace Eshunna.Lib
{
    public static class VectorFloat
    {
        public static float L2NormSquared(IEnumerable<float> values)
        {
            float output = 0;
            foreach (var value in values)
            {
                output += (value * value);
            }
            return output;
        }

        public static float L2Norm(IEnumerable<float> values)
        {
            float squaredSum = VectorFloat.L2NormSquared(values);

            float output = Convert.ToSingle(Math.Sqrt(squaredSum));
            return output;
        }

        public static float[] L2Normalize(float[] values)
        {
            float l2Norm = VectorFloat.L2Norm(values);

            int nValues = values.Length;
            float[] output = new float[nValues];
            for (int iValue = 0; iValue < nValues; iValue++)
            {
                float value = values[iValue];
                float normalizedValue = value / l2Norm;
                output[iValue] = normalizedValue;
            }
            return output;
        }
    }
}
