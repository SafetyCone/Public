using System;

using Public.Common.Lib.IO;


namespace Public.NeuralNetworks.Lib
{
    public class NetworkDataFile
    {
        public const string DefaultFileExtension = FileExtensions.DataFileDefaultExtension;


        #region Static

        public static string GetFileName(int numberOfHiddenLayerNodes, string fileExtension)
        {
            string output = String.Format(@"Network Hidden-{0}.{1}", numberOfHiddenLayerNodes, fileExtension);
            return output;
        }

        public static string GetFileName(int numberOfHiddenLayerNodes)
        {
            string output = NetworkDataFile.GetFileName(numberOfHiddenLayerNodes, NetworkDataFile.DefaultFileExtension);
            return output;
        }

        #endregion
    }
}
