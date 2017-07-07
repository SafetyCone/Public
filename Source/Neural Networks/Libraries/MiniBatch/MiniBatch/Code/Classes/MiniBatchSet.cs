using System;
using System.Collections.Generic;


namespace Public.NeuralNetworks.MiniBatch
{
    [Serializable]
    public class MiniBatchSet
    {
        private List<int[]> ShuffledIndexSets;


        public int NumberOfSets
        {
            get
            {
                int output = this.ShuffledIndexSets.Count;
                return output;
            }
        }
        public int NumberOfItemsInSet { get; private set; }
        public int[] this[int setIndex]
        {
            get
            {
                int[] output = this.ShuffledIndexSets[setIndex];
                return output;
            }
            set
            {
                this.CheckSetSize(value);

                this.ShuffledIndexSets[setIndex] = value;
            }
        }


        private MiniBatchSet()
        {
            this.ShuffledIndexSets = new List<int[]>();
        }

        public MiniBatchSet(int numberOfItemsInSet)
            : this()
        {
            this.NumberOfItemsInSet = numberOfItemsInSet;
        }

        private void CheckSetSize(int[] set)
        {
            if (set.Length != this.NumberOfItemsInSet)
            {
                string message = String.Format(@"Unallowed set size. Allows sets of size: {0}, found: {0}.", this.NumberOfItemsInSet, set.Length);
                throw new ArgumentException(message);
            }
        }

        public void AddSet(int[] set)
        {
            this.CheckSetSize(set);

            this.ShuffledIndexSets.Add(set);
        }
    }
}
