using System;
using SysMath = System.Math;
using System.Collections.Generic;
using System.Linq;


namespace Public.Common.Lib.Math.Extensions
{
    public static class RandomExtensions
    {
        /// <summary>
        /// Uses the Box-Mueller transform to get a normally distributed random variable from two uniformly distributed random variables.
        /// </summary>
        /// <param name="random">The random object to use.</param>
        /// <param name="mu">The desired mean or average of the normal distribution.</param>
        /// <param name="sigma">The desired standard deviation of the normal distribution.</param>
        /// <returns>A normally-distributed random variable.</returns>
        public static double NextGaussian(this Random random, double mu = 0, double sigma = 1)
        {
            double u1 = random.NextDouble();
            double u2 = random.NextDouble();

            double randomStandardNormal = SysMath.Sqrt(-2.0 * SysMath.Log(u1)) * SysMath.Sin(2.0 * SysMath.PI * u2);

            double randomNormal = mu + sigma * randomStandardNormal;

            return randomNormal;
        }

        public static int[] GetIndicesWithoutReplacement(this Random random, int numberOfElements, int numberOfDraws)
        {
            // Bifurcate the algorithm based on the ratio of the number of elements to the number of draws.
            int[] output;
            if (4 * numberOfDraws > numberOfElements)
            {
                // If the number of draws (k) is a sizable fraction of the number of elements (N), just shuffle all elements and choose the first k.
                // High-memory use!
                int[] indices = new int[numberOfElements];
                for (int iIndex = 0; iIndex < numberOfElements; iIndex++)
                {
                    indices[iIndex] = iIndex;
                }

                Shuffler.ShuffleStatic(indices, random);

                output = new int[numberOfDraws];
                Array.Copy(indices, output, numberOfDraws);
            }
            else
            {
                // If the number of draws (k) is small relative to the number of elements (N), just draw with replacement until we get k unique indices.
                // Low memory use, but has frequent wated draws.
                var uniqueIndices = new HashSet<int>();
                while(uniqueIndices.Count < numberOfDraws)
                {
                    int curDraw = random.Next(numberOfElements);
                    uniqueIndices.Add(curDraw);
                }

                output = uniqueIndices.ToArray();
            }

            return output;
        }
    }
}
