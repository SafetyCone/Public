using System;


namespace Public.NeuralNetworks.MiniBatch
{
    public class MiniBatchSetDataFile
    {
        public const string DefaultFileExtension = @"dat";


        #region Static

        public static string GetMiniBatchSetFileName(int numberOfItems, int numberOfSets, string fileExtension)
        {
            string output = String.Format(@"MiniBatches N-{0} Epochs-{1}.{2}", numberOfItems, numberOfSets, fileExtension);
            return output;
        }

        public static string GetMiniBatchSetFileName(int numberOfItems, int numberOfSets)
        {
            string output = MiniBatchSetDataFile.GetMiniBatchSetFileName(numberOfItems, numberOfSets, MiniBatchSetDataFile.DefaultFileExtension);
            return output;
        }

        #endregion
    }
}
