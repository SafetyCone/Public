using System;
using System.Collections.Generic;


namespace PythonFromNet
{
    public static class Utilities
    {
        public const int Seed = 31415;
        public static readonly Random Random = new Random(Utilities.Seed);


        public static Vector GetRandomVector(int count, Random random)
        {
            double[] values = new double[count];
            for (int iIndex = 0; iIndex < count; iIndex++)
            {
                values[iIndex] = (float)random.NextGaussian();
            }

            Vector output = new Vector(values);
            return output;
        }

        public static Vector GetRandomVector(int count)
        {
            Vector output = Utilities.GetRandomVector(count, Utilities.Random);
            return output;
        }

        public static Matrix GetRandomMatrix(int rows, int columns, Random random)
        {
            double[,] values = new double[rows, columns];
            for (int iRow = 0; iRow < rows; iRow++)
            {
                for (int iCol = 0; iCol < columns; iCol++)
                {
                    values[iRow, iCol] = (float)random.NextGaussian();
                }
            }

            Matrix output = new Matrix(values);
            return output;
        }

        public static Matrix GetRandomMatrix(int rows, int columns)
        {
            Matrix output = Utilities.GetRandomMatrix(rows, columns, Utilities.Random);
            return output;
        }

        public static double Sigmoid(double z)
        {
            double output = 1.0 / (1.0 + Math.Exp(-z));
            return output;
        }

        public static double SigmoidPrime(double z)
        {
            double output = Utilities.Sigmoid(z) * (1 - Utilities.Sigmoid(z));
            return output;
        }

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

        public static void VectorizedOperationInPlace(Vector input, Vector output, Func<double, double> operation)
        {
            int numValues = input.Count;
            for (int iValue = 0; iValue < numValues; iValue++)
            {
                double inputValue = input.Values[iValue];
                double outputValue = operation(inputValue);
                output.Values[iValue] = outputValue;
            }
        }

        public static Vector Sigmoid(Vector z)
        {
            Vector output = Utilities.VectorizedOperation(z, Utilities.Sigmoid);
            return output;
        }

        //public static 

        public static Vector SigmoidPrime(Vector z)
        {
            Vector output = Utilities.VectorizedOperation(z, Utilities.SigmoidPrime);
            return output;
        }

        /// <remarks>
        /// Implements the Fisher-Yates shuffle algorithm.
        /// </remarks>
        public static void Shuffle<T>(IList<T> list, Random random)
        {
            int n = list.Count;
            while(n > 1)
            {
                n--;

                int k = random.Next(n + 1);

                // Swap.
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void Shuffle<T>(IList<T> list)
        {
            Utilities.Shuffle(list, Utilities.Random);
        }
    }

    public static class RandomExtensions
    {
        /// <summary>
        /// Uses the Box-Mueller transform to get a normally distributed random variable from two uniformly distributed random variables.
        /// </summary>
        /// <param name="random">The random object to use.</param>
        /// <param name="mu">The desired mean or average of the normal distribution.</param>
        /// <param name="sigma">The desired standard deviation of the normal distribution.</param>
        /// <returns>A normally-distributed random variable.</returns>
        public static double NextGaussian(this Random random, double mu=0, double sigma=1)
        {
            double u1 = random.NextDouble();
            double u2 = random.NextDouble();

            double randomStandardNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

            double randomNormal = mu + sigma * randomStandardNormal;

            return randomNormal;
        }
    }
}
