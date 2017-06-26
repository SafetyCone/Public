using System;


namespace PythonFromNet.Lib
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

            double randomStandardNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

            double randomNormal = mu + sigma * randomStandardNormal;

            return randomNormal;
        }
    }
}
