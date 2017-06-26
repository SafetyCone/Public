using System;


namespace PythonFromNet.Lib
{
    public class MiniBatchProvider
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

        public static int[][] GetEmptyMiniBatches(int numberOfMiniBatches, int sizeOfMiniBatches)
        {
            int[][] output = new int[numberOfMiniBatches][];
            for (int iMiniBatch = 0; iMiniBatch < numberOfMiniBatches; iMiniBatch++)
            {
                output[iMiniBatch] = new int[sizeOfMiniBatches];
            }

            return output;
        }

        #endregion


        public int[] Indices { get; set; }
        public Random Shuffler { get; set; }


        public MiniBatchProvider(int[] indices, Random shuffler)
        {
            this.Indices = indices;
            this.Shuffler = shuffler;
        }

        public MiniBatchProvider(int[] indices)
            : this(indices, Utilities.SingletonRandom)
        {
        }

        public MiniBatchProvider(int numberOfElements, Random shuffler)
            : this(MiniBatchProvider.GetIndicesInOrder(numberOfElements), shuffler)
        {
        }

        public MiniBatchProvider(int numberOfElements)
            : this(MiniBatchProvider.GetIndicesInOrder(numberOfElements), Utilities.SingletonRandom)
        {
        }

        public void FillMiniBatches(int[][] miniBatches)
        {
            int numMiniBatches = miniBatches.Length;
            int sizeMiniBatch = miniBatches[0].Length;

            int iCount = 0;
            for (int iMiniBatch = 0; iMiniBatch < numMiniBatches; iMiniBatch++)
            {
                int[] curMiniBatch = miniBatches[iMiniBatch];
                for (int iElement = 0; iElement < sizeMiniBatch; iElement++)
                {
                    int curIndex = this.Indices[iCount];
                    curMiniBatch[iElement] = curIndex;

                    iCount++;
                }
            }
        }

        public void Shuffle()
        {
            Utilities.Shuffle(this.Indices, this.Shuffler);
        }

        public void FillNewMiniBatches(int[][] miniBatches)
        {
            this.Shuffle();

            this.FillMiniBatches(miniBatches);
        }

        public int[][] GetMiniBatches(int numberOfMiniBatches, int sizeOfMiniBatches)
        {
            int[][] output = MiniBatchProvider.GetEmptyMiniBatches(numberOfMiniBatches, sizeOfMiniBatches);
            this.FillMiniBatches(output);

            return output;
        }

        public int[][] GetNewMiniBatches(int numberOfMiniBatches, int sizeOfMiniBatches)
        {
            this.Shuffle();

            int[][] output = this.GetMiniBatches(numberOfMiniBatches, sizeOfMiniBatches);
            return output;
        }

        public void DeserializeIndices(string filePath)
        {
            int[] indicesFromFile = BinarySerializer<int[]>.DeserializeStatic(filePath);

            for (int iElement = 0; iElement < this.Indices.Length; iElement++)
            {
                this.Indices[iElement] = indicesFromFile[iElement];
            }
        }

        public void SerializeIndices(string filePath)
        {
            BinarySerializer<int[]>.SerializeStatic(this.Indices, filePath);
        }
    }
}
