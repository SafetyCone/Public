using System;

using Public.Common.Lib.Math.Extensions;


namespace Public.Common.Lib.Math
{
    public class RandomNumberGenerator
    {
        #region Static

        public static int[] GetIndicesWithoutReplacement(int numberOfElements, int numberOfDraws, Random random)
        {
            int[] output = random.GetIndicesWithoutReplacement(numberOfElements, numberOfDraws);
            return output;
        }

        public static int[] GetIndicesWithoutReplacement(int numberOfElements, int numberOfDraws)
        {
            int[] output = RandomNumberGenerator.GetIndicesWithoutReplacement(numberOfElements, numberOfDraws, Utilities.SingletonRandom);
            return output;
        }

        #endregion
    }
}
