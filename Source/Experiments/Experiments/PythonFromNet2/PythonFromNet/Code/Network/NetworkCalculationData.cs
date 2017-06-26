using System;

using PythonFromNet.Lib;


namespace PythonFromNet
{
    /// <summary>
    /// Contains all data for the neural network used in hand-written numeric digit recognition.
    /// </summary>
    /// <remarks>
    /// By allocating all memory required for the algorithm only once, we do not waste time in multiple allocations nor garbage collection.
    /// </remarks>
    public class NetworkCalculationData
    {
        // Defines the network.
        public NetworkData NetworkData { get; set; }
        public Vector[] Biases
        {
            get
            {
                Vector[] output = this.NetworkData.Biases;
                return output;
            }
        }
        public Matrix[] Weights
        {
            get
            {
                Matrix[] output = this.NetworkData.Weights;
                return output;
            }
        }
        public int NumberOfLayers {
            get
            {
                int output = this.NetworkData.NumberOfLayers;
                return output;
            }
        }
        public int NumberOfDataPlanes {
            get
            {
                int output = this.NetworkData.NumberOfDataPlanes;
                return output;
            }
        }
        public int[] LayerSizes
        {
            get
            {
                int[] output = this.NetworkData.LayerSizes;
                return output;
            }
        }

        // Not sure whether to make this setable. How to handle re-running network training? Exact same run, 
        //public Random Random { get; protected set; }
        
        public int NumberOfTrainingImages { get; protected set; }
        public int MiniBatchSize { get; protected set; }
        public int NumberOfMiniBatches { get; protected set; }

        public MiniBatchProvider MiniBatchProvider { get; protected set; }
        public int[][] MiniBatches { get; protected set; }
        public MiniBatchFilePathManager MiniBatchFilePathManager { get; protected set; }

        // Used in feed-forwarding.
        /// <remarks>
        /// If there are L layers, there will only be L - 1 pre-activations because the input nodes do not have pre-activations, only activations.
        /// 
        /// NOTE: there is an indexing mis-match with the activations array, which has L vectors for L layers.
        /// </remarks>
        public Vector[] PreActivations { get; protected set; }
        public Vector OutputPreActivations
        {
            get
            {
                Vector output = this.PreActivations[this.NumberOfDataPlanes - 1];
                return output;
            }
        }
        /// <remarks>
        /// If there are L layers, there are L activation vectors.
        /// </remarks>
        public Vector[] Activations { get; protected set; }
        public Vector OutputActivations
        {
            get
            {
                Vector output = this.Activations[this.NumberOfLayers - 1];
                return output;
            }
        }        

        /// <remarks>
        /// If there are L layers, there will only be L - 1 biases because the input nodes do not have biases.
        /// </remarks>
        public Vector[] DeltaBiases { get; protected set; }
        /// <remarks>
        /// If there are L layers, there will only be L - 1  weights because the output nodes do not have a down-stream weight network.
        /// </remarks>
        public Matrix[] DeltaWeights { get; protected set; }

        // Used in training.
        /// <remarks>
        /// If there are L layers, there will only be L - 1 biases because the input nodes do not have biases.
        /// </remarks>
        public Vector[] NablaBiases { get; protected set; }
        /// <remarks>
        /// If there are L layers, there will only be L - 1  weights because the output nodes do not have a down-stream weight network.
        /// </remarks>
        public Matrix[] NablaWeights { get; protected set; }

        // Used in back propagation.
        /// <remarks>
        /// If there are L layers, there will only be L - 1  biases because the input nodes do not have biases.
        /// </remarks>
        public Vector[] DeltaNablaBiases { get; protected set; }
        /// <remarks>
        /// If there are L layers, there will only be L - 1  weights because the output nodes do not have a down-stream weight network.
        /// </remarks>
        public Matrix[] DeltaNablaWeights { get; protected set; }

        // For temporary scratch work.
        /// <remarks>
        /// If there are L layers, there will only be L - 1  biases because the input nodes do not have biases.
        /// </remarks>
        public Vector[] ScratchBiases { get; protected set; }
        public Vector[] ScratchBiases2 { get; protected set; }
        /// <remarks>
        /// If there are L layers, there will only be L - 1  weights because the output nodes do not have a down-stream weight network.
        /// </remarks>
        public Matrix[] ScratchWeights { get; protected set; }

        public Vector CostDerivative { get; protected set; }
        private int zPriorTrueValue;
        public Vector TrueValueAsVector { get; protected set; }


        private NetworkCalculationData(int numberOfTrainingImages, int miniBatchSize)
        {
            this.NumberOfTrainingImages = numberOfTrainingImages;
            this.MiniBatchSize = miniBatchSize;

            this.SetNumberOfMiniBatches();
        }

        private void SetNumberOfMiniBatches()
        {
            if (this.NumberOfTrainingImages % this.MiniBatchSize != 0)
            {
                throw new Exception($@"Number of training images was not an integer multiple of mini-batch size. This is required. Number of training images: {this.NumberOfTrainingImages}, mini-batch size: {this.MiniBatchSize}");
            }
            this.NumberOfMiniBatches = this.NumberOfTrainingImages / this.MiniBatchSize;

            this.MiniBatchProvider = new MiniBatchProvider(this.NumberOfTrainingImages);
        }

        public NetworkCalculationData(int numberOfTrainingImages, int miniBatchSize, NetworkData initialNetworkData, MiniBatchFilePathManager miniBatchFilePathManager)
            : this(numberOfTrainingImages, miniBatchSize)
        {
            this.NetworkData = initialNetworkData;
            this.MiniBatchFilePathManager = miniBatchFilePathManager;

            this.CreateDataSpaces();
        }

        public NetworkCalculationData(int numberOfTrainingImages, int miniBatchSize, string initialNetworkDataFilePath, MiniBatchFilePathManager miniBatchFilePathManager)
            :this(numberOfTrainingImages, miniBatchSize, NetworkData.Deserialize(initialNetworkDataFilePath), miniBatchFilePathManager)
        {
        }

        // Use random numbers to generate the initial network data and the mini-batches for each epoch.
        public NetworkCalculationData(int numberOfTrainingImages, int miniBatchSize, int numberOfHiddenLayerNodes = 30, Random random = null)
            : this(numberOfTrainingImages, miniBatchSize)
        {
            // Setup the network data.
            int[] layerSizes = Network2.GetLayerSizes(numberOfHiddenLayerNodes);

            this.NetworkData = new NetworkData(layerSizes);

            // Create space.
            this.CreateDataSpaces();

            // Setup the objects which require use of random numbers.
            if (null == random)
            {
                random = Utilities.SingletonRandom;
            }
            this.NetworkData.SetInitialBiasesAndWeights(random);

            // Setup the mini-batch architecture.
            this.MiniBatchProvider = new MiniBatchProvider(this.NumberOfTrainingImages, random);
        }

        private void CreateDataSpaces()
        {
            // Setup space for the mini-batches.
            this.MiniBatches = new int[this.NumberOfMiniBatches][];
            for (int iMiniBatch = 0; iMiniBatch < this.NumberOfMiniBatches; iMiniBatch++)
            {
                this.MiniBatches[iMiniBatch] = new int[this.MiniBatchSize];
            }

            // Create calculation spaces.
            this.Activations = new Vector[this.NumberOfLayers];
            for (int iLayer = 0; iLayer < this.NumberOfLayers; iLayer++)
            {
                this.Activations[iLayer] = new Vector(this.LayerSizes[iLayer]);
            }

            this.PreActivations = new Vector[this.NumberOfDataPlanes];
            this.DeltaBiases = new Vector[this.NumberOfDataPlanes];
            this.NablaBiases = new Vector[this.NumberOfDataPlanes];
            this.DeltaNablaBiases = new Vector[this.NumberOfDataPlanes];
            this.ScratchBiases = new Vector[this.NumberOfDataPlanes];
            this.ScratchBiases2 = new Vector[this.NumberOfDataPlanes];
            for (int iLayer = 1; iLayer < this.NumberOfLayers; iLayer++) // Starts at 1.
            {
                int layerNodeCount = this.LayerSizes[iLayer];

                this.PreActivations[iLayer - 1] = new Vector(layerNodeCount);
                this.DeltaBiases[iLayer - 1] = new Vector(layerNodeCount);
                this.NablaBiases[iLayer - 1] = new Vector(layerNodeCount);
                this.DeltaNablaBiases[iLayer - 1] = new Vector(layerNodeCount);
                this.ScratchBiases[iLayer - 1] = new Vector(layerNodeCount);
                this.ScratchBiases2[iLayer - 1] = new Vector(layerNodeCount);
            }

            this.DeltaWeights = new Matrix[this.NumberOfDataPlanes];
            this.NablaWeights = new Matrix[this.NumberOfDataPlanes];
            this.DeltaNablaWeights = new Matrix[this.NumberOfDataPlanes];
            this.ScratchWeights = new Matrix[this.NumberOfDataPlanes];
            for (int iLayer = 0; iLayer < this.NumberOfLayers - 1; iLayer++) // Ends at 1 before the end.
            {
                int numberOfInNodes = this.LayerSizes[iLayer];
                int numberOfOutNodes = this.LayerSizes[iLayer + 1];

                this.DeltaWeights[iLayer] = new Matrix(numberOfOutNodes, numberOfInNodes);
                this.NablaWeights[iLayer] = new Matrix(numberOfOutNodes, numberOfInNodes);
                this.DeltaNablaWeights[iLayer] = new Matrix(numberOfOutNodes, numberOfInNodes);
                this.ScratchWeights[iLayer] = new Matrix(numberOfOutNodes, numberOfInNodes);
            }

            this.TrueValueAsVector = new Vector(Network2.RequiredNumberOutputLayerNodes);
            this.CostDerivative = new Vector(Network2.RequiredNumberOutputLayerNodes);
        }

        public void GenerateNewMiniBatch()
        {
            if(null == this.MiniBatchFilePathManager)
            {
                this.MiniBatchProvider.FillNewMiniBatches(this.MiniBatches);
            }
            else
            {
                string nextFilePath = this.MiniBatchFilePathManager.GetNextEpochFilePath();
                this.MiniBatchProvider.DeserializeIndices(nextFilePath);

                this.MiniBatchProvider.FillMiniBatches(this.MiniBatches);
            }
        }

        /// <summary>
        /// Takes an integer 0-9 and sets the corresponding index in the true value vector.
        /// </summary>
        /// <param name="trueValue">The true digit value, 0-9 TODO: UNCHECKED</param>
        public void SetTrueValue(int trueValue)
        {
            this.TrueValueAsVector.Values[this.zPriorTrueValue] = 0;

            this.TrueValueAsVector.Values[trueValue] = 1;
            this.zPriorTrueValue = trueValue;
        }

        public int GetOutputValue()
        {
            int output = -1;

            Vector outputActivations = this.Activations[this.NumberOfLayers - 1];
            double max = Double.MinValue;
            for (int iNode = 0; iNode < Network2.RequiredNumberOutputLayerNodes; iNode++)
            {
                double curValue = outputActivations.Values[iNode];

                if (max < curValue)
                {
                    max = curValue;
                    output = iNode;
                }
            }

            return output;
        }
    }
}
