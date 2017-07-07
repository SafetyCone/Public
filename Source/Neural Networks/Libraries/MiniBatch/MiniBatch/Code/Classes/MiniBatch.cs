using System;
using System.Collections.Generic;


namespace Public.NeuralNetworks.MiniBatch
{
    [Serializable]
    public class MiniBatch
    {
        #region Static

        public static int[][] GetEmptyValues(int numberOfMiniBatches, int sizeOfMiniBatches)
        {
            int[][] output = new int[numberOfMiniBatches][];
            for (int iMiniBatch = 0; iMiniBatch < numberOfMiniBatches; iMiniBatch++)
            {
                output[iMiniBatch] = new int[sizeOfMiniBatches];
            }

            return output;
        }

        #endregion


        public readonly int[][] Values;
        public int NumberOfMiniBatches
        {
            get
            {
                int output = this.Values.Length;
                return output;
            }
        }
        public int SizeOfMiniBatches
        {
            get
            {
                int output = this.Values[0].Length;
                return output;
            }
        }


        public MiniBatch(int[][] values)
        {
            this.Values = values;
        }

        public MiniBatch(int numberOfMiniBatches, int sizeOfMiniBatches)
        {
            this.Values = MiniBatch.GetEmptyValues(numberOfMiniBatches, sizeOfMiniBatches);
        }

        public IEnumerable<int> EnumerateValues()
        {
            foreach (Tuple<int, int> position in this.EnumeratePositions())
            {
                yield return this.Values[position.Item1][position.Item2];
            }
        }

        public IEnumerable<Tuple<int, int>> EnumeratePositions()
        {
            int numberOfMiniBatches = this.NumberOfMiniBatches;
            int sizeOfMiniBatches = this.SizeOfMiniBatches;

            for (int iBatch = 0; iBatch < numberOfMiniBatches; iBatch++)
            {
                int[] batch = this.Values[iBatch];
                for (int iIndex = 0; iIndex < sizeOfMiniBatches; iIndex++)
                {
                    yield return new Tuple<int, int>(iBatch, iIndex);
                }
            }
        }
    }
}
