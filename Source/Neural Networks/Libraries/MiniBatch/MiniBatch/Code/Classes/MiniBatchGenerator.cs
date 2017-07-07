using System;

using Public.Common.Lib.Math;
using MathUtilities = Public.Common.Lib.Math.Utilities;


namespace Public.NeuralNetworks.MiniBatch
{
    public class MiniBatchGenerator
    {
        #region Static

        public static int[] GetIndicesInOrder(int numberOfElements)
        {
            int[] output = new int[numberOfElements];
            for (int iElement = 0; iElement < numberOfElements; iElement++)
            {
                output[iElement] = iElement;
            }

            return output;
        }

        public static void ShuffleIndices(int[] indices, Shuffler shuffler)
        {
            shuffler.Shuffle(indices);
        }

        public static int[] GetIndicesShuffled(int numberOfElements, Shuffler shuffler)
        {
            int[] output = MiniBatchGenerator.GetIndicesInOrder(numberOfElements);

            MiniBatchGenerator.ShuffleIndices(output, shuffler);

            return output;
        }

        public static MiniBatch GetMiniBatch(int[] shuffledIndices, int sizeOfMiniBatches)
        {
            if(0 != shuffledIndices.Length % sizeOfMiniBatches)
            {
                string message = String.Format(@"Number of minibatches must be an integer. Number of items: {0}, size of mini-batch: {1}.", shuffledIndices.Length, sizeOfMiniBatches);
                throw new ArgumentException(message);
            }

            int numberOfMiniBatches = shuffledIndices.Length / sizeOfMiniBatches;

            MiniBatch output = new MiniBatch(numberOfMiniBatches, sizeOfMiniBatches);

            int count = 0;
            foreach (Tuple<int, int> position in output.EnumeratePositions())
            {
                int index = shuffledIndices[count];
                output.Values[position.Item1][position.Item2] = index;

                count++;
            }

            return output;
        }

        #endregion


        public readonly Shuffler Shuffler;


        public MiniBatchGenerator(Shuffler shuffler)
        {
            this.Shuffler = shuffler;
        }

        public MiniBatchGenerator()
            : this(new Shuffler())
        {
        }

        public int[] GetShuffledIndices(int numberOfElements)
        {
            int[] output = MiniBatchGenerator.GetIndicesShuffled(numberOfElements, this.Shuffler);
            return output;
        }

        public void ShuffleIndices(int[] indices)
        {
            MiniBatchGenerator.ShuffleIndices(indices, this.Shuffler);
        }

        public MiniBatch GetMinibatch(int numberOfMiniBatches, int sizeOfMiniBatches)
        {
            int numberOfElements = numberOfMiniBatches * sizeOfMiniBatches;

            int[] shuffledIndices = this.GetShuffledIndices(numberOfElements);

            MiniBatch output = MiniBatchGenerator.GetMiniBatch(shuffledIndices, sizeOfMiniBatches);
            return output;
        }
    }
}
