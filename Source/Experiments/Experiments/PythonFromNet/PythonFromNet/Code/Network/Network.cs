using System;
using System.Collections.Generic;
using System.IO;

using PythonFromNet.Lib;


namespace PythonFromNet
{
    /// <summary>
    /// Implements the neural network algorithm for recognizing hand-written numeric digits.
    /// </summary>
    // NOTE: The number of images should be an integer multiple of the minibatch size.
    public class Network
    {
        public const string NablasDataSeriesID = @"Network1 Nablas";
        public const string DatasDataSeriesID = @"Network1 Datas";


        #region Static

        /// <remarks>
        /// NOTE: This function assumes that the output vector is 10 nodes (one for each digit).
        /// </remarks>
        public static Vector GetTrueValueAsVector(int trueValue)
        {
            double[] outputValues = new double[10];

            outputValues[trueValue] = 1.0;

            Vector output = new Vector(outputValues);
            return output;
        }

        private static int[][] GetIndicesByMiniBatch(int numberOfTrainingImages, int sizeOfMiniBatches, MiniBatchProvider provider, MiniBatchFilePathManager filePathManager)
        {
            int numberOfMiniBatches = numberOfTrainingImages / sizeOfMiniBatches;

            int[][] output;
            if (null == filePathManager)
            {
                output = provider.GetNewMiniBatches(numberOfMiniBatches, sizeOfMiniBatches);
            }
            else
            {
                string nextFilePath = filePathManager.GetNextEpochFilePath();
                provider.DeserializeIndices(nextFilePath);

                output = provider.GetMiniBatches(numberOfMiniBatches, sizeOfMiniBatches);
            }

            return output;
        }

        private static Tuple<Vector[], Matrix[]> GetEmptyData(int numberOfLayers)
        {
            Vector[] biasesByLayer = new Vector[numberOfLayers];
            Matrix[] weightsByLayer = new Matrix[numberOfLayers];

            Tuple<Vector[], Matrix[]> output = new Tuple<Vector[], Matrix[]>(biasesByLayer, weightsByLayer);
            return output;
        }

        #endregion


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
        public int NumberOfLayers
        {
            get
            {
                int output = this.NetworkData.NumberOfLayers;
                return output;
            }
        }
        public int NumberOfDataPlanes
        {
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

        private MiniBatchProvider zMiniBatchProvider;
        private MiniBatchFilePathManager zMiniBatchFilePathManager;


        public Network()
        {
        }

        public Network(int[] layerSizes)
        {
            this.NetworkData = new NetworkData(layerSizes);
            this.NetworkData.SetInitialBiasesAndWeights(Utilities.SingletonRandom);
        }

        /// <summary>
        /// Feeds an input vector of activations through the layers of the neural network to produce the output vector of activations.
        /// </summary>
        public Vector FeedFoward(Vector inputActivations)
        {
            Tuple<Vector[], Vector[]> preActivationsAndActivations = this.FeedFowardGetAll(inputActivations);

            Vector output = preActivationsAndActivations.Item2[preActivationsAndActivations.Item2.Length - 1];
            return output;
        }

        /// <summary>
        /// Feeds an input vector of activations through the layers of the neural network to produce the output vector of activations.
        /// </summary>
        /// <param name="inputActivations">The initial input activations (from the data source).</param>
        /// <returns>A tuple of node pre-activation values for each layer, and activation values for each layer.</returns>
        public Tuple<Vector[], Vector[]> FeedFowardGetAll(Vector inputActivations)
        {
            Vector[] preActivations = new Vector[this.NumberOfLayers]; // Number of layers.
            Vector[] activations = new Vector[this.NumberOfLayers]; // Number of layers.
            activations[0] = inputActivations;

            for (int iLayer = 1; iLayer < this.NumberOfLayers; iLayer++) // Start at 1.
            {
                Vector dotProduct = this.Weights[iLayer - 1].DotProductFromRight(activations[iLayer - 1]);
                preActivations[iLayer] = dotProduct + this.Biases[iLayer - 1];
                activations[iLayer] = Utilities.Sigmoid(preActivations[iLayer]);
            }

            Tuple<Vector[], Vector[]> output = new Tuple<Vector[], Vector[]>(preActivations, activations);
            return output;
        }

        /// <remarks>
        /// Basically just ArgMax().
        /// </remarks>
        private int GetTrueValueFromVector(Vector outputNodeActivations)
        {
            int output = -1;

            double max = Double.MinValue;
            for (int iNode = 0; iNode < outputNodeActivations.Count; iNode++)
            {
                double curValue = outputNodeActivations.Values[iNode];

                if (max < curValue)
                {
                    max = curValue;
                    output = iNode;
                }
            }

            return output;
        }

        public Vector CostDerivative(Vector outputNodeActivations, Vector trueValueAsVector)
        {
            Vector output = outputNodeActivations - trueValueAsVector;
            return output;
        }

        public Vector CostDerivative(Vector outputNodeActivations, int trueValue)
        {
            Vector trueValueAsVector = Network.GetTrueValueAsVector(trueValue);

            Vector output = this.CostDerivative(outputNodeActivations, trueValueAsVector);
            return output;
        }

        public int Evaluate(List<NumericImageData> testData)
        {
            int output = 0;
            for (int iImage = 0; iImage < testData.Count; iImage++)
            {
                NumericImageData curImage = testData[iImage];

                Vector inputActivations = new Vector(curImage.PixelValues);

                Vector outputActivations = this.FeedFoward(inputActivations);

                int outputValue = this.GetTrueValueFromVector(outputActivations);

                if (outputValue == curImage.TrueValue)
                {
                    output++;
                }
            }

            return output;
        }

        /// <remarks>
        /// Perform a training run using predetermined (non-random) data.
        /// </remarks>
        public TrainingPerformanceData StochasticGradientDescent(List<NumericImageData> trainingImages, int numberOfEpochsToRun, int miniBatchSize, double etaTheLearningRate, NetworkData initialNetworkData, MiniBatchFilePathManager miniBatchFilePathManager,
            List<NumericImageData> testData = null,
            TextWriter interactive = null)
        {
            this.NetworkData = initialNetworkData;
            this.zMiniBatchFilePathManager = miniBatchFilePathManager;

            TrainingPerformanceData output = this.StochasticGradientDescent(trainingImages, numberOfEpochsToRun, miniBatchSize, etaTheLearningRate, testData, interactive);
            return output;
        }

        /// <remarks>
        /// Perform a training run using predetermined (non-random) data.
        /// </remarks>
        public TrainingPerformanceData StochasticGradientDescent(List<NumericImageData> trainingImages, int numberOfEpochsToRun, int miniBatchSize, double etaTheLearningRate, string initialNetworkDataFilePath, MiniBatchFilePathManager miniBatchFilePathManager,
            List<NumericImageData> testData = null,
            TextWriter interactive = null)
        {
            NetworkData initialNetworkData = NetworkData.Deserialize(initialNetworkDataFilePath);

            TrainingPerformanceData output = this.StochasticGradientDescent(trainingImages, numberOfEpochsToRun, miniBatchSize, etaTheLearningRate, initialNetworkData, miniBatchFilePathManager, testData, interactive);
            return output;
        }

        /// <remarks>
        /// Perform a training run using randomly generated data.
        /// </remarks>
        public TrainingPerformanceData StochasticGradientDescent(List<NumericImageData> trainingImages, int numberOfEpochsToRun, int miniBatchSize, double etaTheLearningRate,
            List<NumericImageData> testData = null,
            TextWriter interactive = null)
        {
            bool hasTestData = true;
            if (null == testData)
            {
                hasTestData = false;
            }

            if (null == interactive)
            {
                interactive = Console.Out;
            }

            TrainingPerformanceData output = new TrainingPerformanceData();

            TrainingRunData runData = new TrainingRunData();
            output.Run = runData;

            runData.Start = DateTime.Now;
            runData.NetworkLayerSizes = this.LayerSizes;
            runData.Eta = etaTheLearningRate;
            runData.MiniBatchSize = miniBatchSize;
            
            if(hasTestData)
            {
                output.NumberOfTestSamples = testData.Count;
            }

            this.zMiniBatchProvider = new MiniBatchProvider(trainingImages.Count);

            for (int iEpoch = 0; iEpoch < numberOfEpochsToRun; iEpoch++)
            {
                EpochPerformanceData epochPerformanceData = new EpochPerformanceData();
                output.Epochs.Add(epochPerformanceData);

                DateTime epochRunBegin = DateTime.Now;
                this.RunEpoch(iEpoch, trainingImages, miniBatchSize, etaTheLearningRate, interactive);
                epochPerformanceData.TrainElapsedTime = DateTime.Now - epochRunBegin;

                if (hasTestData)
                {
                    DateTime evaluateRunBegin = DateTime.Now;
                    int numberCorrect = this.Evaluate(testData);
                    epochPerformanceData.NumberEvaluatedCorrectly = numberCorrect;
                    epochPerformanceData.EvaluateElapsedTime = DateTime.Now - evaluateRunBegin;

                    string message = String.Format(@"Epoch {0}: {1} / {2} correct.", iEpoch, numberCorrect, testData.Count);
                    interactive.WriteLine(message);
                }
                else
                {
                    epochPerformanceData.NumberEvaluatedCorrectly = -1;
                    epochPerformanceData.EvaluateElapsedTime = TimeSpan.Zero;

                    string message = String.Format(@"Epoch {0} complete.", iEpoch);
                    interactive.WriteLine(message);
                }
                TimeSpan elapsedTime = DateTime.Now - epochRunBegin;
                interactive.WriteLine(@"Elapsed time: {0}", elapsedTime);
            }

            output.Run.End = DateTime.Now;
            output.TotalTrainingElapsedTime = output.Run.End - output.Run.Start;

            return output;
        }

        private void RunEpoch(int epochNumber, List<NumericImageData> trainingImages, int miniBatchSize, double etaTheLearningRate, TextWriter interactive)
        {
            int numTrainingImages = trainingImages.Count;
            int[][] indiciesByMiniBatch = Network.GetIndicesByMiniBatch(numTrainingImages, miniBatchSize, this.zMiniBatchProvider, this.zMiniBatchFilePathManager); // Includes a shuffle that should differ between epochs.

            int iCount = 0;
            foreach(int[] miniBatchIndices in indiciesByMiniBatch)
            {
                this.UpdateMiniBatch(epochNumber, iCount, trainingImages, etaTheLearningRate, miniBatchIndices);

                iCount++;
                if (iCount % 500 == 0)
                {
                    interactive.WriteLine(@"Mini-batches complete: {0}", iCount);

                    // Save weights.
                    Tuple<Vector[], Matrix[]> datas = new Tuple<Vector[], Matrix[]>(this.Biases, this.Weights);
                    Utilities.SerializeNetworkData(Network.DatasDataSeriesID, datas, epochNumber, iCount);
                }
            }
        }

        /// <summary>
        /// Returns vectors for each layer of nodes, and weights for each network of connections between layers.
        /// </summary>
        /// <returns></returns>
        private Tuple<Vector[], Matrix[]> GetZeroedData()
        {
            Tuple<Vector[], Matrix[]> output = Network.GetEmptyData(this.NumberOfDataPlanes);

            for (int iLayer = 0; iLayer < this.NumberOfDataPlanes; iLayer++)
            {
                output.Item1[iLayer] = new Vector(this.Biases[iLayer].Count);
            }

            for (int iLayer = 0; iLayer < this.NumberOfDataPlanes; iLayer++)
            {
                output.Item2[iLayer] = new Matrix(this.Weights[iLayer].Shape);
            }

            return output;
        }

        private void UpdateMiniBatch(int epochNumber, int miniBatchNumber, List<NumericImageData> trainingImages, double etaTheLearningRate, int[] miniBatchIndices)
        {
            // Setup nablas.
            Tuple<Vector[], Matrix[]> zeroedData = this.GetZeroedData();

            Vector[] nablaBiases = zeroedData.Item1;
            Matrix[] nablaWeights = zeroedData.Item2;

            foreach (int miniBatchIndex in miniBatchIndices)
            {
                NumericImageData trainingImage = trainingImages[miniBatchIndex];

                // Determine the increment to add to the nablas.
                Tuple<Vector[], Matrix[]> deltaNablas = this.BackPropagate(epochNumber, miniBatchNumber, miniBatchIndex, trainingImage);

                // Adjust nablas.
                for (int iLayer = 0; iLayer < this.NumberOfDataPlanes; iLayer++)
                {
                    nablaBiases[iLayer] = nablaBiases[iLayer] + deltaNablas.Item1[iLayer];
                    nablaWeights[iLayer] = nablaWeights[iLayer] + deltaNablas.Item2[iLayer];
                }
            }

            //// Save nablas.
            //Tuple<Vector[], Matrix[]> nablas = new Tuple<Vector[], Matrix[]>(nablaBiases, nablaWeights);
            //Utilities.SerializeNetworkData(Network.NablasDataSeriesID, nablas, epochNumber, miniBatchNumber);

            // Adjust biases and weights.
            double learningRateAdjustment = etaTheLearningRate / miniBatchIndices.Length;

            for (int iLayer = 0; iLayer < this.NumberOfDataPlanes; iLayer++)
            {
                Vector deltaBiases = nablaBiases[iLayer] * learningRateAdjustment;
                this.Biases[iLayer] = this.Biases[iLayer] - deltaBiases;

                Matrix deltaWeights = nablaWeights[iLayer] * learningRateAdjustment;
                this.Weights[iLayer] = this.Weights[iLayer] - deltaWeights;
            }

            //// Save weights.
            //Tuple<Vector[], Matrix[]> datas = new Tuple<Vector[], Matrix[]>(this.Biases, this.Weights);
            //Utilities.SerializeNetworkData(Network.DatasDataSeriesID, datas, epochNumber, miniBatchNumber);
        }

        /// <summary>
        /// Calculates the partial derivatives of the cost function with respect to each of the biases and weights.
        /// </summary>
        /// <returns>Nabla biases for each layer, and nabla weights for each layer.</returns>
        private Tuple<Vector[], Matrix[]> BackPropagate(int epochNumber, int miniBatchNumber, int imageNumber, NumericImageData trainingImage)
        {
            Tuple<Vector[], Matrix[]> output = Network.GetEmptyData(this.NumberOfDataPlanes);
            Vector[] deltaNablaBiases = output.Item1;
            Matrix[] deltaNablaWeights = output.Item2;

            // Feed forward.
            Vector inputActivations = new Vector(trainingImage.PixelValues);

            Tuple<Vector[], Vector[]> preActivationsAndActivations = this.FeedFowardGetAll(inputActivations);

            // Backward pass.
            Vector outputNodeActivations = preActivationsAndActivations.Item2[this.NumberOfLayers - 1];
            Vector costDerivative = this.CostDerivative(outputNodeActivations, trainingImage.TrueValue);

            Vector outputNodePreActivations = preActivationsAndActivations.Item1[this.NumberOfLayers - 1];
            Vector outputNodeSigmoidPrimes = Utilities.SigmoidPrime(outputNodePreActivations);

            Vector biasDelta;
            Matrix weightsDelta;

            biasDelta = costDerivative * outputNodeSigmoidPrimes;
            deltaNablaBiases[this.NumberOfDataPlanes - 1] = biasDelta;

            weightsDelta = Matrix.VectorOuterMultiply(biasDelta, preActivationsAndActivations.Item2[this.NumberOfLayers - 2]); // Activations should be transposed, thus making a matrix outer product.
            deltaNablaWeights[this.NumberOfDataPlanes - 1] = weightsDelta;

            for (int iLayer = 2; iLayer < this.NumberOfLayers; iLayer++)
            {
                Vector curPreActivations = preActivationsAndActivations.Item1[this.NumberOfLayers - iLayer];
                Vector curSigmoidPrime = Utilities.SigmoidPrime(curPreActivations);

                Matrix transposedWeights = this.Weights[this.NumberOfDataPlanes - iLayer + 1].Transpose();
                Vector shifts = transposedWeights.DotProductFromRight(biasDelta);
                biasDelta = shifts * curSigmoidPrime;
                deltaNablaBiases[this.NumberOfDataPlanes - iLayer] = biasDelta;

                weightsDelta = Matrix.VectorOuterMultiply(biasDelta, preActivationsAndActivations.Item2[this.NumberOfLayers - iLayer - 1]); // Activations should be transposed, thus making a matrix outer product.
                deltaNablaWeights[this.NumberOfDataPlanes - iLayer] = weightsDelta;
            }

            return output;
        }
    }
}
