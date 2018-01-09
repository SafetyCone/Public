using System;
using SysMath = System.Math;


namespace Public.Common.Lib.Math
{
    public class Utilities
    {
        #region Static

        public static readonly Random SingletonRandom = new Random(Constants.DefaultRandomSeed);


        public static Vector VectorizedOperation(Vector input, Func<double, double> operation)
        {
            Vector output = new Vector(input.Count);
            for (int iValue = 0; iValue < input.Count; iValue++)
            {
                double curValue = input.Values[iValue];

                double outputValue = operation(curValue);
                output.Values[iValue] = outputValue;
            }

            return output;
        }

        public static double Sigmoid(double z)
        {
            double output = 1.0 / (1.0 + SysMath.Exp(-z));
            return output;
        }

        public static double SigmoidPrime(double z)
        {
            double output = Utilities.Sigmoid(z) * (1 - Utilities.Sigmoid(z));
            return output;
        }

        public static Vector Sigmoid(Vector z)
        {
            Vector output = Utilities.VectorizedOperation(z, Utilities.Sigmoid);
            return output;
        }

        public static Vector SigmoidPrime(Vector z)
        {
            Vector output = Utilities.VectorizedOperation(z, Utilities.SigmoidPrime);
            return output;
        }

        #endregion
    }
}
