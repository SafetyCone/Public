using System;
using System.Collections.Generic;
using System.IO;


namespace PythonFromNet
{
    // NOTE: The number of images should be an integer multiple of the minibatch size.
    public class Network
    {
        #region Static

        private static int[][] GetIndicesByMiniBatch(int numTrainingImages, int miniBatchSize)
        {
            int[] indices = new int[numTrainingImages];
            for (int iIndex = 0; iIndex < numTrainingImages; iIndex++)
            {
                indices[iIndex] = iIndex;
            }

            Utilities.Shuffle(indices);

            int numMiniBatches = numTrainingImages / miniBatchSize;
            int[][] output = new int[numMiniBatches][];

            int iCount = 0;
            for (int iMiniBatch = 0; iMiniBatch < numMiniBatches; iMiniBatch++)
            {
                int[] miniBatch = new int[miniBatchSize];
                output[iMiniBatch] = miniBatch;

                for (int iElement = 0; iElement < miniBatchSize; iElement++)
                {
                    miniBatch[iElement] = indices[iCount];
                    iCount++;
                }
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


        private int zNumberOfLayers;
        private int zNumberOfDataPlanes;
        private int[] zLayerSizes;
        /// <summary>
        /// The neural node biases for each layer.
        /// </summary>
        /// <remarks>
        /// If there are L layers, including the input layer, the length of the biases array will be L - 1. This is because the input layer has no biases.
        /// 
        /// If there are N nodes in layer 1, there will be N bias values in the vector for layer 1.
        /// </remarks>
        private Vector[] zBiases;
        /// <summary>
        /// The neural node connection weights between each layer.
        /// </summary>
        /// <remarks>
        /// If there are L layers, including the input layer, the length of the weights array will be L - 1. This is becase the last output layer has no output weight network.
        /// 
        /// If there are N nodes in layer 1, and M nodes in layer 2, there will by M x N weights between layers 1 and 2.
        /// </remarks>
        private Matrix[] zWeights;


        public Network(int[] layerSizes)
        {
            this.zLayerSizes = layerSizes;

            int numberOfLayers = this.zLayerSizes.Length;
            this.zNumberOfLayers = numberOfLayers;

            int numberOfLayersMinusOne = numberOfLayers - 1;
            this.zNumberOfDataPlanes = numberOfLayersMinusOne;

            this.zBiases = new Vector[numberOfLayersMinusOne]; // Minus one since the input layer has not biases.
            for (int iLayer = 1; iLayer < this.zNumberOfLayers; iLayer++) // Starts at 1.
            {
                int numberOfNodesInLayer = this.zLayerSizes[iLayer];

                Vector curLayerBiases = Utilities.GetRandomVector(numberOfNodesInLayer);
                this.zBiases[iLayer - 1] = curLayerBiases; // Minus 1.
            }

            this.zWeights = new Matrix[numberOfLayersMinusOne];
            for (int iDataPlane = 0; iDataPlane < numberOfLayersMinusOne; iDataPlane++)
            {
                int numberOfInNodes = this.zLayerSizes[iDataPlane];
                int numberOfOutNodes = this.zLayerSizes[iDataPlane + 1];

                Matrix curInOutLayerConnectionWeights = Utilities.GetRandomMatrix(numberOfOutNodes, numberOfInNodes);
                this.zWeights[iDataPlane] = curInOutLayerConnectionWeights;
            }
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
            Vector[] preActivations = new Vector[this.zNumberOfLayers]; // Number of layers.
            Vector[] activations = new Vector[this.zNumberOfLayers]; // Number of layers.
            activations[0] = inputActivations;

            for (int iLayer = 1; iLayer < this.zNumberOfLayers; iLayer++) // Start at 1.
            {
                Vector dotProduct = this.zWeights[iLayer - 1].DotProductFromRight(activations[iLayer - 1]);
                preActivations[iLayer] = dotProduct + this.zBiases[iLayer - 1];
                activations[iLayer] = Utilities.Sigmoid(preActivations[iLayer]);
            }

            Tuple<Vector[], Vector[]> output = new Tuple<Vector[], Vector[]>(preActivations, activations);
            return output;
        }

        /// <remarks>
        /// NOTE: This function assumes that the output vector is 10 nodes (one for each digit).
        /// </remarks>
        private Vector GetTrueValueAsVector(int trueValue)
        {
            double[] outputValues = new double[10];

            outputValues[trueValue] = 1.0;

            Vector output = new Vector(outputValues);
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

                if(max < curValue)
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
            Vector trueValueAsVector = this.GetTrueValueAsVector(trueValue);

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

                if(outputValue == curImage.TrueValue)
                {
                    output++;
                }
            }

            return output;
        }

        public TrainingPerformanceData StochasticGradientDescent(List<NumericImageData> trainingImages, int numberOfEpochsToRun, int miniBatchSize, double etaTheLearningRate, List<NumericImageData> testData = null, TextWriter interactive = null)
        {
            if(null == interactive)
            {
                interactive = Console.Out;
            }

            bool hasTestData = true;
            if (null == testData)
            {
                hasTestData = false;
            }

            TrainingPerformanceData output = new TrainingPerformanceData();

            TrainingRunData runData = new TrainingRunData();
            output.Run = runData;

            runData.Start = DateTime.Now;
            runData.NetworkLayerSizes = this.zLayerSizes;
            runData.Eta = etaTheLearningRate;
            runData.MiniBatchSize = miniBatchSize;
            
            if(hasTestData)
            {
                output.NumberOfTestSamples = testData.Count;
            }

            for (int iEpoch = 0; iEpoch < numberOfEpochsToRun; iEpoch++)
            {
                EpochPerformanceData epochPerformanceData = new EpochPerformanceData();
                output.Epochs.Add(epochPerformanceData);

                DateTime epochRunBegin = DateTime.Now;
                this.RunEpoch(trainingImages, miniBatchSize, etaTheLearningRate, testData, interactive);
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

        private void RunEpoch(List<NumericImageData> trainingImages, int miniBatchSize, double etaTheLearningRate, List<NumericImageData> testData, TextWriter interactive)
        {
            int numTrainingImages = trainingImages.Count;
            int[][] indiciesByMiniBatch = Network.GetIndicesByMiniBatch(numTrainingImages, miniBatchSize); // Includes a shuffle that should differ between epochs.

            int iCount = 0;
            foreach(int[] miniBatchIndices in indiciesByMiniBatch)
            {
                this.UpdateMiniBatch(trainingImages, etaTheLearningRate, miniBatchIndices);

                iCount++;
                if (iCount % 500 == 0)
                {
                    interactive.WriteLine(@"Mini-batches complete: {0}", iCount);
                }
            }
        }

        /// <summary>
        /// Returns vectors for each layer of nodes, and weights for each network of connections between layers.
        /// </summary>
        /// <returns></returns>
        private Tuple<Vector[], Matrix[]> GetZeroedData()
        {
            Tuple<Vector[], Matrix[]> output = Network.GetEmptyData(this.zNumberOfDataPlanes);

            for (int iLayer = 0; iLayer < this.zNumberOfDataPlanes; iLayer++)
            {
                output.Item1[iLayer] = new Vector(this.zBiases[iLayer].Count);
            }

            for (int iLayer = 0; iLayer < this.zNumberOfDataPlanes; iLayer++)
            {
                output.Item2[iLayer] = new Matrix(this.zWeights[iLayer].Shape);
            }

            return output;
        }

        private void UpdateMiniBatch(List<NumericImageData> trainingImages, double etaTheLearningRate, int[] miniBatchIndices)
        {
            // Setup nablas.
            Tuple<Vector[], Matrix[]> zeroedData = this.GetZeroedData();

            Vector[] nablaBiases = zeroedData.Item1;
            Matrix[] nablaWeights = zeroedData.Item2;

            foreach (int miniBatchIndex in miniBatchIndices)
            {
                NumericImageData trainingImage = trainingImages[miniBatchIndex];

                // Determine the increment to add to the nablas.
                Tuple<Vector[], Matrix[]> deltaNablas = this.BackPropagate(trainingImage);

                // Adjust nablas.
                for (int iLayer = 0; iLayer < this.zNumberOfDataPlanes; iLayer++)
                {
                    nablaBiases[iLayer] = nablaBiases[iLayer] + deltaNablas.Item1[iLayer];
                    nablaWeights[iLayer] = nablaWeights[iLayer] + deltaNablas.Item2[iLayer];
                }
            }

            // Adjust biases and weights.
            double learningRateAdjustment = etaTheLearningRate / miniBatchIndices.Length;

            for (int iLayer = 0; iLayer < this.zNumberOfDataPlanes; iLayer++)
            {
                Vector deltaBiases = nablaBiases[iLayer] * learningRateAdjustment;
                this.zBiases[iLayer] = this.zBiases[iLayer] - deltaBiases;

                Matrix deltaWeights = nablaWeights[iLayer] * learningRateAdjustment;
                this.zWeights[iLayer] = this.zWeights[iLayer] - deltaWeights;
            }
        }

        /// <summary>
        /// Calculates the partial derivatives of the cost function with respect to each of the biases and weights.
        /// </summary>
        /// <returns>Nabla biases for each layer, and nabla weights for each layer.</returns>
        private Tuple<Vector[], Matrix[]> BackPropagate(NumericImageData trainingImage)
        {
            Tuple<Vector[], Matrix[]> output = Network.GetEmptyData(this.zNumberOfDataPlanes);
            Vector[] nablaBiases = output.Item1;
            Matrix[] nablaWeights = output.Item2;

            // Feed forward.
            Vector inputActivations = new Vector(trainingImage.PixelValues);

            Tuple<Vector[], Vector[]> preActivationsAndActivations = this.FeedFowardGetAll(inputActivations);

            // Backward pass.
            Vector outputNodeActivations = preActivationsAndActivations.Item2[this.zNumberOfLayers - 1];
            Vector costDerivative = this.CostDerivative(outputNodeActivations, trainingImage.TrueValue);

            Vector outputNodePreActivations = preActivationsAndActivations.Item1[this.zNumberOfLayers - 1];
            Vector outputNodeSigmoidPrimes = Utilities.SigmoidPrime(outputNodePreActivations);

            Vector biasDelta;
            Matrix weightsDelta;

            biasDelta = costDerivative * outputNodeSigmoidPrimes;
            nablaBiases[this.zNumberOfDataPlanes - 1] = biasDelta;

            weightsDelta = Matrix.VectorOuterMultiply(biasDelta, preActivationsAndActivations.Item2[this.zNumberOfLayers - 2]); // Activations should be transposed, thus making a matrix outer product.
            nablaWeights[this.zNumberOfDataPlanes - 1] = weightsDelta;

            for (int iLayer = 2; iLayer < this.zNumberOfLayers; iLayer++)
            {
                Vector curPreActivations = preActivationsAndActivations.Item1[this.zNumberOfLayers - iLayer];
                Vector curSigmoidPrime = Utilities.SigmoidPrime(curPreActivations);

                Matrix transposedWeights = this.zWeights[this.zNumberOfDataPlanes - iLayer + 1].Transpose();
                Vector shifts = transposedWeights.DotProductFromRight(biasDelta);
                biasDelta = shifts * curSigmoidPrime;
                nablaBiases[this.zNumberOfDataPlanes - iLayer] = biasDelta;

                weightsDelta = Matrix.VectorOuterMultiply(biasDelta, preActivationsAndActivations.Item2[this.zNumberOfLayers - iLayer - 1]); // Activations should be transposed, thus making a matrix outer product.
                nablaWeights[this.zNumberOfDataPlanes - iLayer] = weightsDelta;
            }

            return output;
        }
    }
}
