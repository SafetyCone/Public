using System;

using Public.Common.Lib.IO.Serialization;
using Public.Common.Lib.Math;


namespace Public.NeuralNetworks.Lib
{
    /// <summary>
    /// Defines a neural network by specifying the node biases and connection weights.
    /// </summary>
    [Serializable]
    public class NetworkData
    {
        #region Static

        public static int GetNumberOfLayers(NetworkData networkData)
        {
            int output = networkData.Biases.Length + 1;
            return output;
        }

        public static int GetNumberOfDataPlanes(NetworkData networkData)
        {
            int output = networkData.Biases.Length;
            return output;
        }

        /// <summary>
        /// NOTE: Assumes that the number of layers properties is already set.
        /// </summary>
        public static int[] GetLayerSizes(NetworkData networkData)
        {
            int numberOfLayers = networkData.NumberOfLayers;
            int numberOfDataPlanes = networkData.NumberOfDataPlanes;

            int[] output = new int[numberOfLayers];
            for (int iLayer = 0; iLayer < numberOfDataPlanes; iLayer++)
            {
                int numNodes = networkData.Weights[iLayer].Columns; // Assumes that input nodes occupy the columns.

                output[iLayer] = numNodes;
            }

            output[numberOfLayers - 1] = networkData.Weights[numberOfLayers - 2].Rows;

            return output;
        }

        public static NetworkData Deserialize(string filePath)
        {
            Tuple<Vector[], Matrix[]> data = BinarySerializer.DeserializeStatic<Tuple<Vector[], Matrix[]>>(filePath);

            NetworkData output = new NetworkData(data.Item1, data.Item2);
            return output;
        }

        public static void Serialize(string filePath, NetworkData data)
        {
            Tuple<Vector[], Matrix[]> serializationData = new Tuple<Vector[], Matrix[]>(data.Biases, data.Weights);

            BinarySerializer.SerializeStatic(serializationData, filePath);
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
        public Vector[] Biases { get; protected set; }
        /// <summary>
        /// The neural node connection weights between each layer.
        /// </summary>
        /// <remarks>
        /// If there are L layers, including the input layer, the length of the weights array will be L - 1. This is becase the last output layer has no output weight network.
        /// 
        /// If there are N nodes in layer 1, and M nodes in layer 2, there will by M x N weights between layers 1 and 2.
        /// </remarks>
        public Matrix[] Weights { get; protected set; }
        /// <remarks>
        /// Non-dynamic field, set in constructor.
        /// </remarks>
        public int NumberOfLayers { get; protected set; }
        /// <remarks>
        /// Non-dynamic field, set in constructor.
        /// </remarks>
        public int NumberOfDataPlanes { get; protected set; }
        /// <remarks>
        /// Non-dynamic field, set in constructor.
        /// </remarks>
        public int[] LayerSizes { get; protected set; }


        public NetworkData()
        {
        }

        public NetworkData(Vector[] biases, Matrix[] weights)
        {
            this.Biases = biases;
            this.Weights = weights;

            this.NumberOfLayers = NetworkData.GetNumberOfLayers(this);
            this.NumberOfDataPlanes = NetworkData.GetNumberOfDataPlanes(this);
            this.LayerSizes = NetworkData.GetLayerSizes(this);
        }

        public NetworkData(int[] layerSizes)
        {
            int numLayers = layerSizes.Length;
            int numDataPlanes = numLayers - 1;

            this.LayerSizes = new int[numLayers];
            Array.Copy(layerSizes, this.LayerSizes, numLayers);

            this.NumberOfLayers = numLayers;
            this.NumberOfDataPlanes = numDataPlanes;

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

        /// <summary>
        /// The zero-based index from the back of the layers.
        /// </summary>
        public int GetLayerIndexFromBack(int indexFromBack)
        {
            int output = this.NumberOfLayers - 1 - indexFromBack;
            return output;
        }

        /// <summary>
        /// The zero-based index from the back of the data planes.
        /// </summary>
        public int GetDataPlaneIndexFromBack(int indexFromBack)
        {
            int output = this.NumberOfDataPlanes - 1 - indexFromBack;
            return output;
        }
    }
}
