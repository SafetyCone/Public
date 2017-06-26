using System;
using System.Collections.Generic;
using System.IO;

using PythonFromNet.Lib;


namespace PythonFromNet
{
    /// <summary>
    /// Implements the neural network algorithm for recognizing hand-written numeric digits.
    /// </summary>
    /// <remarks>
    /// Uses a network data object to speed up the algorithm, unlike the original network.
    /// </remarks>
    public class Network2
    {
        public const int RequiredNumberInputLayerNodes = 784; // 28 * 28.
        public const int RequiredNumberOutputLayerNodes = 10; // One for each numeric digit, 0-9.
        public const int DefaultNumberHiddenLayerNodes = 30;
        public const int DefaultMiniBatchSize = 10;
        public const double DefaultEtaValue = 3;

        public const string NablasDataSeriesID = @"Network2 Nablas";
        public const string DatasDataSeriesID = @"Network2 Datas";


        #region Static

        public static int[] GetLayerSizes(int hiddenLayerSize)
        {
            int[] output = new int[]
            {
                Network2.RequiredNumberInputLayerNodes,
                hiddenLayerSize,
                Network2.RequiredNumberOutputLayerNodes,
            };

            return output;
        }

        public static void InputImage(NetworkCalculationData data, NumericImageData image)
        {
            // Copy image pixel values into the first layer of the activations.
            Vector inputActivations = data.Activations[0];

            int numPixels = image.PixelValues.Length;
            for (int iPixel = 0; iPixel < numPixels; iPixel++)
            {
                inputActivations.Values[iPixel] = image.PixelValues[iPixel];
            }
        }

        public static void FeedForward(NetworkCalculationData data)
        {
            // Now work through all subsequent layers.
            for (int iLayer = 1; iLayer < data.NumberOfLayers; iLayer++) // Start at 1.
            {
                Vector dotProduct = data.ScratchBiases[iLayer - 1];
                data.Weights[iLayer - 1].DotProductFromRightInPlace(data.Activations[iLayer - 1], dotProduct);

                Vector biases = data.Biases[iLayer - 1];
                Vector preActivations = data.PreActivations[iLayer - 1];
                Vector.AddInPlace(dotProduct, biases, preActivations);

                Utilities.SigmoidInPlace(data.PreActivations[iLayer - 1], data.Activations[iLayer]);
            }
        }

        public static void FeedForward(NetworkCalculationData data, NumericImageData image)
        {
            Network2.InputImage(data, image);
            Network2.FeedForward(data);
        }

        public static int Evaluate(NetworkCalculationData data, NumericImageData image)
        {
            Network2.FeedForward(data, image);

            int output = Network2.ReadOutputValue(data);
            return output;
        }

        public static int ReadOutputValue(NetworkCalculationData data)
        {
            int output = data.GetOutputValue();
            return output;
        }

        public static bool IsCorrect(NetworkCalculationData data, NumericImageData image)
        {
            int predictedDigit = Network2.Evaluate(data, image);

            bool output = predictedDigit == image.TrueValue;
            return output;
        }

        private static void SetEnd(TrainingPerformanceData perf)
        {
            perf.Run.End = DateTime.Now;
            perf.TotalTrainingElapsedTime = perf.Run.End - perf.Run.Start;
        }

        private static Vector CalculateCostDerivative(NetworkCalculationData data, int trueValue)
        {
            Vector outputActivations = data.OutputActivations;

            data.SetTrueValue(trueValue);
            Vector trueValueAsVector = data.TrueValueAsVector;

            Vector costDerivative = data.CostDerivative;
            Vector.SubtractInPlace(outputActivations, trueValueAsVector, costDerivative);

            return costDerivative;
        }

        #endregion


        public int HiddenLayerSize { get; protected set; }
        public int MiniBatchSize { get; protected set; }
        public double Eta { get; protected set; }
        public double LearningRateAdjustment { get; protected set; }
        public TextWriter Interactive { get; protected set; }

        private NetworkCalculationData zData;


        public Network2(int hiddenLayerNodeCount, int miniBatchSize, double eta, TextWriter interactive)
        {
            this.HiddenLayerSize = hiddenLayerNodeCount;
            this.MiniBatchSize = miniBatchSize;
            this.Eta = eta;
            this.LearningRateAdjustment = this.Eta / this.MiniBatchSize;
            this.Interactive = interactive;
        }

        public Network2(
            int hiddenLayerNodeCount = Network2.DefaultNumberHiddenLayerNodes,
            int miniBatchSize = Network2.DefaultMiniBatchSize,
            double eta = Network2.DefaultEtaValue
            )
            : this(hiddenLayerNodeCount, miniBatchSize, eta, Console.Out)
        {
        }

        /// <remarks>
        /// Perform a training run using predetermined (non-random) data.
        /// </remarks>
        public TrainingPerformanceData RunTraining(List<NumericImageData> trainingImages, int numberOfEpochsToRun, NetworkData initialNetworkData, MiniBatchFilePathManager miniBatchFilePathManager,
            List<NumericImageData> testData = null)
        {
            this.zData = new NetworkCalculationData(trainingImages.Count, this.MiniBatchSize, initialNetworkData, miniBatchFilePathManager);

            TrainingPerformanceData output = this.RunTrainingInternal(trainingImages, numberOfEpochsToRun, testData);
            return output;
        }

        /// <remarks>
        /// Perform a training run using predetermined (non-random) data.
        /// </remarks>
        public TrainingPerformanceData RunTraining(List<NumericImageData> trainingImages, int numberOfEpochsToRun, string initialNetworkDataFilePath, MiniBatchFilePathManager miniBatchFilePathManager,
            List<NumericImageData> testData = null)
        {
            NetworkData initialNetworkData = NetworkData.Deserialize(initialNetworkDataFilePath);

            TrainingPerformanceData output = this.RunTraining(trainingImages, numberOfEpochsToRun, initialNetworkData, miniBatchFilePathManager);
            return output;
        }

        /// <remarks>
        /// Perform a training run using randomly generated data.
        /// </remarks>
        public TrainingPerformanceData RunTraining(List<NumericImageData> trainingImages, int numberOfEpochsToRun,
            List<NumericImageData> testData = null)
        {
            this.zData = new NetworkCalculationData(trainingImages.Count, this.MiniBatchSize);

            TrainingPerformanceData output = this.RunTrainingInternal(trainingImages, numberOfEpochsToRun, testData);
            return output;
        }

        private TrainingPerformanceData RunTrainingInternal(List<NumericImageData> trainingImages, int numberOfEpochsToRun, List<NumericImageData> testData)
        {
            TrainingPerformanceData output = this.CreateTrainingPerformanceData(testData);

            bool hasTestData = true;
            if (null == testData)
            {
                hasTestData = false;
            }

            for (int iEpoch = 0; iEpoch < numberOfEpochsToRun; iEpoch++)
            {
                EpochPerformanceData epochPerformanceData = new EpochPerformanceData();
                output.Epochs.Add(epochPerformanceData);

                DateTime epochRunBegin = DateTime.Now;
                this.RunEpoch(iEpoch, trainingImages);
                epochPerformanceData.TrainElapsedTime = DateTime.Now - epochRunBegin;

                if (hasTestData)
                {
                    DateTime evaluateRunBegin = DateTime.Now;
                    int numberCorrect = this.Evaluate(testData);
                    epochPerformanceData.NumberEvaluatedCorrectly = numberCorrect;
                    epochPerformanceData.EvaluateElapsedTime = DateTime.Now - evaluateRunBegin;

                    string message = String.Format(@"Epoch {0}: {1} / {2} correct.", iEpoch, numberCorrect, testData.Count);
                    this.Interactive.WriteLine(message);
                }
                else
                {
                    epochPerformanceData.NumberEvaluatedCorrectly = -1;
                    epochPerformanceData.EvaluateElapsedTime = TimeSpan.Zero;

                    string message = String.Format(@"Epoch {0} complete.", iEpoch);
                    this.Interactive.WriteLine(message);
                }
                TimeSpan elapsedTime = DateTime.Now - epochRunBegin;
                this.Interactive.WriteLine(@"Elapsed time: {0}", elapsedTime);
            }

            Network2.SetEnd(output);
            return output;
        }

        private void RunEpoch(int epochNumber, List<NumericImageData> trainingImages)
        {
            this.zData.GenerateNewMiniBatch();

            int iCount = 0;
            foreach(int[] miniBatchIndices in this.zData.MiniBatches)
            {
                this.UpdateMiniBatch(epochNumber, iCount, trainingImages, miniBatchIndices);

                iCount++;
                if (iCount % 500 == 0)
                {
                    this.Interactive.WriteLine(@"Mini-batches complete: {0}", iCount);

                    // Save weights.
                    Tuple<Vector[], Matrix[]> datas = new Tuple<Vector[], Matrix[]>(this.zData.Biases, this.zData.Weights);
                    Utilities.SerializeNetworkData(Network2.DatasDataSeriesID, datas, epochNumber, iCount);
                }
            }
        }

        private void UpdateMiniBatch(int epochNumber, int miniBatchNumber, List<NumericImageData> trainingImages, int[] miniBatchIndices)
        {
            foreach(int miniBatchIndex in miniBatchIndices)
            {
                NumericImageData trainingImage = trainingImages[miniBatchIndex];

                Network2.FeedForward(this.zData, trainingImage);

                this.BackPropagate(epochNumber, miniBatchNumber, miniBatchIndex, trainingImage);

                // Adjust nablas.
                for (int iLayer = 0; iLayer < this.zData.NumberOfDataPlanes; iLayer++)
                {
                    Vector nablaBiases = this.zData.NablaBiases[iLayer];
                    Vector deltaNablaBiases = this.zData.DeltaNablaBiases[iLayer];
                    Vector.AddInPlace(nablaBiases, deltaNablaBiases, nablaBiases);

                    Matrix nablaWeights = this.zData.NablaWeights[iLayer];
                    Matrix deltaNablaWeights = this.zData.DeltaNablaWeights[iLayer];
                    Matrix.AddInPlace(nablaWeights, deltaNablaWeights, nablaWeights);
                }
            }

            //// Save nablas.
            //Tuple<Vector[], Matrix[]> nablas = new Tuple<Vector[], Matrix[]>(this.zData.NablaBiases, this.zData.NablaWeights);
            //Utilities.SerializeNetworkData(Network2.NablasDataSeriesID, nablas, epochNumber, miniBatchNumber);

            // Adjust biases and weights.
            for (int iLayer = 0; iLayer < this.zData.NumberOfDataPlanes; iLayer++)
            {
                Vector nablaBiases = this.zData.NablaBiases[iLayer];
                Vector deltaBiases = this.zData.ScratchBiases[iLayer];
                Vector.MultiplyByConstantInPlace(nablaBiases, deltaBiases, this.LearningRateAdjustment);
                Vector.ZeroOut(nablaBiases); // Important to clear for next round.

                Vector biases = this.zData.Biases[iLayer];
                Vector.SubtractInPlace(biases, deltaBiases, biases);

                Matrix nablaWeights = this.zData.NablaWeights[iLayer];
                Matrix deltaWeights = this.zData.ScratchWeights[iLayer];
                Matrix.MultiplyByConstantInPlace(nablaWeights, deltaWeights, this.LearningRateAdjustment);
                Matrix.ZeroOut(nablaWeights); // Important to clear for next round.

                Matrix weights = this.zData.Weights[iLayer];
                Matrix.SubtractInPlace(weights, deltaWeights, weights);
            }

            //// Save weights.
            //Tuple<Vector[], Matrix[]> datas = new Tuple<Vector[], Matrix[]>(this.zData.Biases, this.zData.Weights);
            //Utilities.SerializeNetworkData(Network2.DatasDataSeriesID, datas, epochNumber, miniBatchNumber);
        }

        private void BackPropagate(int epochNumber, int miniBatchNumber, int imageNumber, NumericImageData image)
        {
            Vector costDerivative = Network2.CalculateCostDerivative(this.zData, image.TrueValue);

            Vector outputLayerPreActivations = this.zData.OutputPreActivations;
            Vector outputSigmoidPrimes = this.zData.ScratchBiases[this.zData.NumberOfDataPlanes - 1];
            Utilities.SigmoidPrimeInPlace(outputLayerPreActivations, outputSigmoidPrimes);

            Vector deltaBiases = this.zData.DeltaBiases[this.zData.NumberOfDataPlanes - 1];
            Vector.MultiplyInPlace(costDerivative, outputSigmoidPrimes, deltaBiases);

            Vector deltaNablaBiases = this.zData.DeltaNablaBiases[this.zData.NumberOfDataPlanes - 1];
            Vector.Copy(deltaBiases, deltaNablaBiases);

            Vector lastHiddenLayerActivations = this.zData.Activations[this.zData.NumberOfLayers - 2];
            Matrix deltaWeights = this.zData.DeltaWeights[this.zData.NumberOfLayers - 2];
            Matrix.VectorOuterMultiplyInPlace(deltaBiases, lastHiddenLayerActivations, deltaWeights);

            Matrix deltaNablaWeights = this.zData.DeltaNablaWeights[this.zData.NumberOfDataPlanes - 1];
            Matrix.Copy(deltaWeights, deltaNablaWeights);

            for (int iLayer = 2; iLayer < this.zData.NumberOfLayers; iLayer++) // Start at 2 and will work backwards.
            {
                Vector curLayerPreActivations = this.zData.PreActivations[this.zData.NumberOfLayers - iLayer - 1];
                Vector curSigmoidPrimes = this.zData.ScratchBiases[this.zData.NumberOfLayers - iLayer - 1];
                Utilities.SigmoidPrimeInPlace(curLayerPreActivations, curSigmoidPrimes);

                Vector shifts = this.zData.ScratchBiases2[this.zData.NumberOfLayers - iLayer - 1];
                Matrix curWeights = this.zData.Weights[this.zData.NumberOfLayers - iLayer];
                curWeights.DotProductFromLeftInPlace(deltaBiases, shifts); // Use initial delta biases.

                Vector curDeltaBiases = this.zData.DeltaBiases[this.zData.NumberOfLayers - iLayer - 1];
                Vector.MultiplyInPlace(shifts, curSigmoidPrimes, curDeltaBiases);

                Vector curDeltaNablaBiases = this.zData.DeltaNablaBiases[this.zData.NumberOfDataPlanes - iLayer];
                Vector.Copy(curDeltaBiases, curDeltaNablaBiases);

                Vector priorLayerActivations = this.zData.Activations[this.zData.NumberOfLayers - iLayer - 1];
                Matrix curWeightsDelta = this.zData.DeltaWeights[this.zData.NumberOfLayers - iLayer - 1];
                Matrix.VectorOuterMultiplyInPlace(curDeltaBiases, priorLayerActivations, curWeightsDelta);

                Matrix curDeltaNablaWeights = this.zData.DeltaNablaWeights[this.zData.NumberOfLayers - iLayer - 1];
                Matrix.Copy(curWeightsDelta, curDeltaNablaWeights);
            }
        }

        /// <summary>
        /// Returns the number of images correctly classified.
        /// </summary>
        private int Evaluate(List<NumericImageData> testData)
        {
            int output = 0;
            foreach (NumericImageData image in testData)
            {
                if(Network2.IsCorrect(this.zData, image))
                {
                    output++;
                }
            }

            return output;
        }

        private TrainingPerformanceData CreateTrainingPerformanceData(List<NumericImageData> testData)
        {
            TrainingPerformanceData output = new TrainingPerformanceData();

            TrainingRunData runData = new TrainingRunData();
            output.Run = runData;

            runData.Start = DateTime.Now;
            runData.NetworkLayerSizes = Network2.GetLayerSizes(this.HiddenLayerSize);
            runData.Eta = this.Eta;
            runData.MiniBatchSize = this.MiniBatchSize;

            if (null != testData)
            {
                output.NumberOfTestSamples = testData.Count;
            }

            return output;
        }
    }
}
