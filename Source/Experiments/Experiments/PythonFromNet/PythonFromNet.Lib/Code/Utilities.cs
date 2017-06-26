using System;
using System.Collections.Generic;
using System.IO;


namespace PythonFromNet.Lib
{
    public static class Utilities
    {
        public const int RandomSeed = 31415;
        public static readonly Random SingletonRandom = Utilities.GetNewSeededRandom();


        public static Random GetNewSeededRandom()
        {
            Random output = new Random(Utilities.RandomSeed);
            return output;
        }

        /// <remarks>
        /// Implements the Fisher-Yates shuffle algorithm.
        /// </remarks>
        public static void Shuffle<T>(IList<T> list, Random random)
        {
            int n = list.Count;
            while (n > 1)
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
            Utilities.Shuffle(list, Utilities.SingletonRandom);
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

        public static void SigmoidInPlace(Vector input, Vector output)
        {
            Utilities.VectorizedOperationInPlace(input, output, Utilities.Sigmoid);
        }

        public static Vector SigmoidPrime(Vector z)
        {
            Vector output = Utilities.VectorizedOperation(z, Utilities.SigmoidPrime);
            return output;
        }

        public static void SigmoidPrimeInPlace(Vector input, Vector output)
        {
            Utilities.VectorizedOperationInPlace(input, output, Utilities.SigmoidPrime);
        }

        public static string GetSeriesFilePath(string seriesID, int epochNumber = -1, int miniBatchNumber = -1, int imageNumber = -1)
        {
            string epochToken = String.Empty;
            if(-1 != epochNumber)
            {
                epochToken = String.Format($@"Epoch-{epochNumber} ");
            }

            string miniBatchToken = String.Empty;
            if(-1 != miniBatchNumber)
            {
                miniBatchToken = String.Format($@"MiniBatch-{miniBatchNumber} ");
            }

            string imageToken = String.Empty;
            if(-1 != imageNumber)
            {
                imageToken = String.Format($@"Image-{imageNumber}");
            }

            string tokens = String.Format($@"{epochToken}{miniBatchToken}{imageToken}");
            if(String.Empty == tokens)
            {
                tokens = Guid.NewGuid().ToString();
            }

            string directoryPath = FileSeriesManager.GetSeriesDirectoryPath(seriesID);
            string fileName = String.Format($@"{tokens}.dat");

            string filePath = Path.Combine(directoryPath, fileName);
            return filePath;
        }

        public static void SerializeNetworkData<T>(string seriesID, T networkData, int epochNumber = -1, int miniBatchNumber = -1, int imageNumber = -1)
        {
            string filePath = Utilities.GetSeriesFilePath(seriesID, epochNumber, miniBatchNumber);

            BinarySerializer<T>.SerializeStatic(networkData, filePath);
        }

        public static T DeserializeNetworkData<T>(string seriesID, int epochNumber = -1, int miniBatchNumber = -1, int imageNumber = -1)
        {
            string filePath = Utilities.GetSeriesFilePath(seriesID, epochNumber, miniBatchNumber);

            T output = BinarySerializer<T>.DeserializeStatic(filePath);
            return output;
        }
    }
}
