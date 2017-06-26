using System;


namespace PythonFromNet.Lib
{
    /// <summary>
    /// Defines a neural network by specifying the node biases and connection weights.
    /// </summary>
    public class NetworkData
    {
        public const string DefaultInitialNetworkDataFilePath = @"C:\temp\Initial Network.dat";


        #region Static

        public static NetworkData Deserialize(string filePath)
        {
            Tuple<Vector[], Matrix[]> data = BinarySerializer<Tuple<Vector[], Matrix[]>>.DeserializeStatic(filePath);

            NetworkData output = new NetworkData(data.Item1, data.Item2);
            return output;
        }

        public static void Serialize(string filePath, NetworkData data)
        {
            Tuple<Vector[], Matrix[]> serializationData = new Tuple<Vector[], Matrix[]>(data.Biases, data.Weights);

            BinarySerializer<Tuple<Vector[], Matrix[]>>.SerializeStatic(serializationData, filePath);
        }

        /// <summary>
        /// Sets the biases and weights to be Gaussian random numbers.
        /// </summary>
        public static void SetInitialBiasesAndWeightsStatic(NetworkData networkData, Random random)
        {
            Func<double> generateGaussian = () => random.NextGaussian();
            for (int iLayer = 0; iLayer < networkData.NumberOfDataPlanes; iLayer++)
            {
                Vector.IterateSetValue(networkData.Biases[iLayer], generateGaussian);
                Matrix.IterateSetValue(networkData.Weights[iLayer], generateGaussian);
            }
        }

        #endregion


        /// <summary>
        /// The neural node biases for each layer.
        /// </summary>
        /// <remarks>
        /// If there are L layers, including the input layer, the length of the biases array will be L - 1. This is because the input layer has no biases.
        /// 
        /// If there are N nodes in layer 1, there will be N bias values in the vector for layer 1.
        /// </remarks>
        public Vector[] Biases { get; set; }
        /// <summary>
        /// The neural node connection weights between each layer.
        /// </summary>
        /// <remarks>
        /// If there are L layers, including the input layer, the length of the weights array will be L - 1. This is becase the last output layer has no output weight network.
        /// 
        /// If there are N nodes in layer 1, and M nodes in layer 2, there will by M x N weights between layers 1 and 2.
        /// </remarks>
        public Matrix[] Weights { get; set; }
        public int NumberOfLayers
        {
            get
            {
                int output = Biases.Length + 1;
                return output;
            }
        }
        public int NumberOfDataPlanes
        {
            get
            {
                int output = Biases.Length;
                return output;
            }
        }
        /// <remarks>
        /// Calculated field.
        /// </remarks>
        public int[] LayerSizes
        {
            get
            {
                int numberOfLayers = this.NumberOfLayers;

                int[] output = new int[numberOfLayers];
                for (int iLayer = 0; iLayer < numberOfLayers - 1; iLayer++)
                {
                    int numNodes = this.Weights[iLayer].Columns; // Assumes that input nodes occupy the columns.

                    output[iLayer] = numNodes;
                }

                output[numberOfLayers - 1] = this.Weights[numberOfLayers - 2].Rows;

                return output;
            }
        }


        public NetworkData()
        {
        }

        public NetworkData(Vector[] biases, Matrix[] weights)
        {
            this.Biases = biases;
            this.Weights = weights;
        }

        public NetworkData(int[] layerSizes)
        {
            int numLayers = layerSizes.Length;
            int numDataPlanes = numLayers - 1;

            this.Biases = new Vector[numDataPlanes];
            this.Weights = new Matrix[numDataPlanes];
            for (int iLayer = 0; iLayer < numDataPlanes; iLayer++)
            {
                int layerSize = layerSizes[iLayer + 1];
                this.Biases[iLayer] = new Vector(layerSize);

                int inputNodeCount = layerSizes[iLayer];
                int outputNodeCount = layerSizes[iLayer + 1];
                this.Weights[iLayer] = new Matrix(outputNodeCount, inputNodeCount); // Input nodes occupy the columns.
            }
        }

        /// <summary>
        /// Sets the biases and weights to be Gaussian random numbers.
        /// </summary>
        public void SetInitialBiasesAndWeights(Random random)
        {
            NetworkData.SetInitialBiasesAndWeightsStatic(this, random);
        }
    }
}
