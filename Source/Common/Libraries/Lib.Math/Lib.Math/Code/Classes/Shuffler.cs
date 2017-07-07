using System;
using System.Collections.Generic;


namespace Public.Common.Lib.Math
{
    public class Shuffler
    {
        #region Static

        /// <remarks>
        /// Implements the Fisher-Yates shuffle algorithm.
        /// </remarks>
        public static void ShuffleStatic<T>(IList<T> list, Random random)
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

        /// <summary>
        /// Performs a shuffle using the singleton-random offered in utilities.
        /// </summary>
        public static void ShuffleStatic<T>(IList<T> list)
        {
            Shuffler.ShuffleStatic(list, Utilities.SingletonRandom);
        }

        #endregion


        public readonly Random Random;


        public Shuffler(Random random)
        {
            this.Random = random;
        }

        public Shuffler(int randomSeed)
            : this(new Random(randomSeed))
        {
        }

        public Shuffler()
            : this(Constants.DefaultRandomSeed)
        {
        }

        public void Shuffle<T>(IList<T> list)
        {
            Shuffler.ShuffleStatic(list, this.Random);
        }
    }
}
