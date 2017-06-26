using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

using Python.Runtime;

using PythonFromNet.Lib;


namespace PythonFromNet
{
    class Program
    {
        #region Constants

        public const string NielsenMnistDataFilePath = @"C:\Organizations\Minex\Projects\Neural Networks\neural-networks-and-deep-learning-master\data\mnist.pkl.gz";
        public const string NielsenMnistNetDataFilePath = @"C:\Organizations\Minex\Projects\Neural Networks\Data\mnistNET.dat";

        public const string RawMnistZipFilesDirectoryPath = @"C:\Organizations\Minex\Projects\Neural Networks\Data\MNIST\Raw";
        public const string RawMnistFilesDirectoryPath = @"C:\Organizations\Minex\Projects\Neural Networks\Data\MNIST";
        public const string RawBasedMnistNetDataFilePath = @"C:\Organizations\Minex\Projects\Neural Networks\Data\mnistNETRaw.dat";

        public const string RawBasedMnistNetDoubleDataFilePath = @"C:\Organizations\Minex\Projects\Neural Networks\Data\mnistDoubleNETRaw.dat";

        public const string TrainingImagesZipFileName = @"train-images-idx3-ubyte.gz";
        public static string TrainingImagesFileName
        {
            get
            {
                string output = Program.ChangeFileExtensionToDat(Program.TrainingImagesZipFileName);
                return output;
            }
        }
        public const string TrainingLabelsZipFileName = @"train-labels-idx1-ubyte.gz";
        public static string TrainingLabelsFileName
        {
            get
            {
                string output = Program.ChangeFileExtensionToDat(Program.TrainingLabelsZipFileName);
                return output;
            }
        }
        public const string TestImagesZipFileName = @"t10k-images-idx3-ubyte.gz";
        public static string TestImagesFileName
        {
            get
            {
                string output = Program.ChangeFileExtensionToDat(Program.TestImagesZipFileName);
                return output;
            }
        }
        public const string TestLabelsZipFileName = @"t10k-labels-idx1-ubyte.gz";
        public static string TestLabelsFileName
        {
            get
            {
                string output = Program.ChangeFileExtensionToDat(Program.TestLabelsZipFileName);
                return output;
            }
        }
        public static string[] AllZipFileNames
        {
            get
            {
                string[] output = new string[]
                {
                    Program.TrainingImagesZipFileName,
                    Program.TrainingLabelsZipFileName,
                    Program.TestImagesZipFileName,
                    Program.TestLabelsZipFileName,
                };
                return output;
            }
        }
        public static string[] AllFileNames
        {
            get
            {
                string[] output = new string[]
                {
                    Program.TrainingImagesFileName,
                    Program.TrainingLabelsFileName,
                    Program.TestImagesFileName,
                    Program.TestLabelsFileName,
                };
                return output;
            }
        }
        public static string[] AllFilePaths
        {
            get
            {
                string[] allFileNames = Program.AllFileNames;

                int numFiles = allFileNames.Length;
                string[] output = new string[numFiles];
                for (int iFile = 0; iFile < numFiles; iFile++)
                {
                    string fileName = allFileNames[iFile];

                    string filePath = Path.Combine(Program.RawMnistFilesDirectoryPath, fileName);
                    output[iFile] = filePath;
                }

                return output;
            }
        }
        public static Tuple<string, string>[] AllRawZipFileToFilePaths
        {
            get
            {
                string sourceDirectoryPath = Program.RawMnistZipFilesDirectoryPath;
                string destinationDirectoryPath = Program.RawMnistFilesDirectoryPath;
                string[] zipFileNames = Program.AllZipFileNames;
                string[] fileNames = Program.AllFileNames;

                int numFiles = zipFileNames.Length;
                Tuple<string, string>[] output = new Tuple<string, string>[numFiles];
                for (int iFile = 0; iFile < numFiles; iFile++)
                {
                    string sourceFileName = zipFileNames[iFile];
                    string destinationFileName = fileNames[iFile];

                    string sourcePath = Path.Combine(sourceDirectoryPath, sourceFileName);
                    string destinationPath = Path.Combine(destinationDirectoryPath, destinationFileName);

                    output[iFile] = new Tuple<string, string>(sourcePath, destinationPath);
                }

                return output;
            }
        }


        private static string ChangeFileExtensionToDat(string fileName)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

            string output = fileNameWithoutExtension + @".dat";
            return output;
        }

        #endregion


        static void Main(string[] args)
        {
            Program.SubMain(args);
        }

        private static void SubMain(string[] args)
        {
            //Program.EnsureNetworkNablasEquality();
            //Program.GenerateInitialNetworkData();
            //Program.GenerateMiniBatchFiles();
            //Program.ConvertMnistFloatToDouble();
            //Program.TryRunNeuralNetwork();
            //Program.TryDrawImage();
            //Program.TryCompareNielsenAndRawMnistData();
            //Program.TryReadRawMnistData();
            //Program.UnZipAllFiles();
            //Program.TrySerializeDeserializeMnist();
            //Program.TryParseTestData();
            //Program.TryRunPythonNet();
            //Program.Test();
        }

        private static void Test()
        {
            DateTime now = DateTime.Now;
            MnistData data = BinarySerializer<MnistData>.DeserializeStatic(Program.RawBasedMnistNetDoubleDataFilePath);
            TimeSpan elapsedTime = DateTime.Now - now;
        }

        #region Neural Network

        private static void EnsureNetworkNablasEquality()
        {
            //Tuple<Vector[], Matrix[]> network1Nablas = Utilities.DeserializeNetworkData<Tuple<Vector[], Matrix[]>>(Network.NablasDataSeriesID, 0, 0);
            //Tuple<Vector[], Matrix[]> network2Nablas = Utilities.DeserializeNetworkData<Tuple<Vector[], Matrix[]>>(@"Network2 Nablas", 0, 0);

            for (int i = 500; i < 5001; i += 500)
            {
                Tuple<Vector[], Matrix[]> network1Datas = Utilities.DeserializeNetworkData<Tuple<Vector[], Matrix[]>>(Network.DatasDataSeriesID, 0, i);
                Tuple<Vector[], Matrix[]> network2Datas = Utilities.DeserializeNetworkData<Tuple<Vector[], Matrix[]>>(@"Network2 Datas", 0, i);
                Program.EnsureEquality(network1Datas, network2Datas);
            }
        }

        private static void EnsureEquality(Tuple<Vector[], Matrix[]> data1, Tuple<Vector[], Matrix[]> data2)
        {
            Vector[] vectorArray1 = data1.Item1;
            Vector[] vectorArray2 = data2.Item1;
            if(vectorArray1.Length != vectorArray2.Length)
            {
                throw new Exception();
            }

            for (int iVector = 0; iVector < vectorArray1.Length; iVector++)
            {
                Vector v1 = vectorArray1[iVector];
                Vector v2 = vectorArray2[iVector];

                if(v1.Count != v2.Count)
                {
                    throw new Exception();
                }

                for (int iElement = 0; iElement < v1.Count; iElement++)
                {
                    if(v1.Values[iElement] != v2.Values[iElement])
                    {
                        throw new Exception();
                    }
                }
            }

            Matrix[] matrixArray1 = data1.Item2;
            Matrix[] matrixArray2 = data2.Item2;
            if (matrixArray1.Length != matrixArray2.Length)
            {
                throw new Exception();
            }

            for (int iMatrix = 0; iMatrix < matrixArray2.Length; iMatrix++)
            {
                Matrix m1 = matrixArray1[iMatrix];
                Matrix m2 = matrixArray2[iMatrix];

                if (m1.Rows != m2.Rows || m1.Columns != m2.Columns)
                {
                    throw new Exception();
                }

                for (int iRow = 0; iRow < m1.Rows; iRow++)
                {
                    for (int iCol = 0; iCol < m1.Columns; iCol++)
                    {
                        if (m1.Values[iRow, iCol] != m2.Values[iRow, iCol])
                        {
                            throw new Exception();
                        }
                    }
                }
            }
        }

        private static void GenerateInitialNetworkData()
        {
            NetworkData initialNetworkData = new NetworkData(new int[] { 784, 30, 10 });

            initialNetworkData.SetInitialBiasesAndWeights(Utilities.SingletonRandom);

            NetworkData.Serialize(NetworkData.DefaultInitialNetworkDataFilePath, initialNetworkData);
        }

        private static void GenerateMiniBatchFiles()
        {
            int numberOfEpochs = 10;

            int numberOfElements = 50000;
            int sizeOfMiniBatch = 10;
            int numberOfMiniBatches = numberOfElements / sizeOfMiniBatch;

            MiniBatchFilePathManager filePathManager = new MiniBatchFilePathManager();
            MiniBatchProvider provider = new MiniBatchProvider(numberOfElements);


            for (int iEpoch = 0; iEpoch < numberOfEpochs; iEpoch++)
            {
                string filePath = filePathManager.GetNextEpochFilePath();

                provider.Shuffle();
                provider.SerializeIndices(filePath);
            }
        }

        // NOTE: double data is 431MB, only 2.5 seconds to deserialize.
        private static void ConvertMnistFloatToDouble()
        {
            MnistDataSet characterImageDataSet = BinarySerializer<MnistDataSet>.DeserializeStatic(Program.RawBasedMnistNetDataFilePath);

            MnistData dataSet = Program.Convert(characterImageDataSet);

            BinarySerializer<MnistData>.SerializeStatic(dataSet, Program.RawBasedMnistNetDoubleDataFilePath);
        }

        private static void TryRunNeuralNetwork()
        {
            MnistData dataSet = BinarySerializer<MnistData>.DeserializeStatic(Program.RawBasedMnistNetDoubleDataFilePath);

            NetworkData initialNetworkData = NetworkData.Deserialize(NetworkData.DefaultInitialNetworkDataFilePath);
            MiniBatchFilePathManager miniBatchFilePathManager = new MiniBatchFilePathManager();

            Network network = new Network();

            TrainingPerformanceData performanceData = network.StochasticGradientDescent(dataSet.TrainingData, 10, 10, 3.0, initialNetworkData, miniBatchFilePathManager, dataSet.TestData, Console.Out);

            string filePath;
            
            filePath = String.Format(@"C:\temp\Neural Network Training Performance - {0:yyyyMMdd-hhmmss}.txt", performanceData.Run.Start);
            Program.WriteOutPerformanceData(performanceData, filePath);

            filePath = String.Format(@"C:\temp\Neural Network Training Performance - {0:yyyyMMdd-hhmmss}.dat", performanceData.Run.Start);
            using (Stream fStream = new FileStream(filePath, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fStream, performanceData);
            }
        }

        private static void WriteOutPerformanceData(TrainingPerformanceData performanceData, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                string line;

                writer.WriteLine(@"Network description:");

                int[] layerSizes = performanceData.Run.NetworkLayerSizes;
                writer.WriteLine($@"    Network size: [{layerSizes[0]} {layerSizes[1]} {layerSizes[2]}]");
                writer.WriteLine($@"    Eta = {performanceData.Run.Eta}");
                writer.WriteLine($@"    Mini-batch size: {performanceData.Run.MiniBatchSize}");

                line = String.Format(@"Run time started: {0}", performanceData.Run.Start);
                writer.WriteLine(line);

                line = String.Format(@"Run time finished: {0}", performanceData.Run.End);
                writer.WriteLine(line);

                line = String.Format(@"Total elapsed time: {0}", performanceData.TotalTrainingElapsedTime);
                writer.WriteLine(line);

                line = String.Format(@"Number of epochs: {0}", performanceData.Epochs.Count);
                writer.WriteLine(line);

                int iCount = 0;
                foreach (EpochPerformanceData epochPerformanceData in performanceData.Epochs)
                {
                    line = String.Format(@"Epoch {0} - Total elapsed time: {1}", iCount, epochPerformanceData.TotalElapsedTime);
                    writer.WriteLine(line);

                    line = String.Format(@"    Train elapsed time: {0}", epochPerformanceData.TrainElapsedTime);
                    writer.WriteLine(line);

                    if(-1 == epochPerformanceData.NumberEvaluatedCorrectly)
                    {
                        writer.WriteLine(@"No test data evaluation performed.");
                    }
                    else
                    {
                        line = String.Format(@"    Evaluate elapsed time: {0}", epochPerformanceData.EvaluateElapsedTime);
                        writer.WriteLine(line);

                        line = String.Format(@"    Evaluatation: {0} / {1} correct.", epochPerformanceData.NumberEvaluatedCorrectly, performanceData.NumberOfTestSamples);
                        writer.WriteLine(line);
                    }

                    iCount++;
                }

                writer.WriteLine(@"--- END ---");
            }
        }

        private static NumericImageData Convert(CharacterImageData characterImage)
        {
            double[] pixelValues = Array.ConvertAll<float, double>(characterImage.PixelData, (x) => System.Convert.ToDouble(x));

            NumericImageData output = new NumericImageData(pixelValues, characterImage.TrueValue);
            return output;
        }

        private static List<NumericImageData> Convert(List<CharacterImageData> characterImages)
        {
            List<NumericImageData> output = new List<NumericImageData>(characterImages.Count);
            foreach(CharacterImageData characterImage in characterImages)
            {
                NumericImageData numericImage = Program.Convert(characterImage);
                output.Add(numericImage);
            }

            return output;
        }

        private static MnistData Convert(MnistDataSet characterImageDataSet)
        {
            MnistData output = new MnistData();

            output.TestData = Program.Convert(characterImageDataSet.TestData);
            output.TrainingData = Program.Convert(characterImageDataSet.TrainingData);
            output.ValidationData = Program.Convert(characterImageDataSet.ValidationData);

            return output;
        }

        #endregion

        #region Drawing an Image

        private static void TryDrawImage()
        {
            MnistDataSet mnistData = BinarySerializer<MnistDataSet>.DeserializeStatic(Program.RawBasedMnistNetDataFilePath);

            CharacterImageData image = mnistData.TestData[0];

            Bitmap bitmap = Program.CreateBitmap(image);

            string imageFilePath = @"C:\temp\temp.bmp";
            bitmap.Save(imageFilePath, ImageFormat.Bmp);
        }

        /// <remarks>
        /// No scale argument because with Windows Photo Viewer you can just zoom in.
        /// </remarks>
        private static Bitmap CreateBitmap(CharacterImageData image)
        {
            int numPixels = 28;
            int numHeightPixels = numPixels;
            int numWidthPixels = numPixels;

            Bitmap bitmap = new Bitmap(numWidthPixels, numHeightPixels, PixelFormat.Format24bppRgb);
            int iCount = 0;
            for (int iXPixel = 0; iXPixel < numHeightPixels; iXPixel++)
            {
                for (int iYPixel = 0; iYPixel < numWidthPixels; iYPixel++)
                {
                    float value = image.PixelData[iCount];

                    int level = System.Convert.ToInt32(value * 256);
                    int invertLevel = 255 - level;

                    Color color = Color.FromArgb(invertLevel, invertLevel, invertLevel);
                    bitmap.SetPixel(iYPixel, iXPixel, color);

                    iCount++;
                }
            }

            return bitmap;
        }

        #endregion

        #region Deserialize, Serialize, Compare Data Sets

        private static void TryCompareNielsenAndRawMnistData()
        {
            string nielsenBasedNetDataFilePath = Program.NielsenMnistNetDataFilePath;
            string rawBasedNetDataFilePath = Program.RawBasedMnistNetDataFilePath;

            MnistDataSet nielsenMnist = BinarySerializer<MnistDataSet>.DeserializeStatic(nielsenBasedNetDataFilePath);
            MnistDataSet rawMnist = BinarySerializer<MnistDataSet>.DeserializeStatic(rawBasedNetDataFilePath);

            List<Tuple<List<CharacterImageData>, List<CharacterImageData>>> nielsenRawPairs = new List<Tuple<List<CharacterImageData>, List<CharacterImageData>>>();
            nielsenRawPairs.Add(new Tuple<List<CharacterImageData>, List<CharacterImageData>>(nielsenMnist.TestData, rawMnist.TestData));
            nielsenRawPairs.Add(new Tuple<List<CharacterImageData>, List<CharacterImageData>>(nielsenMnist.TrainingData, rawMnist.TrainingData));
            nielsenRawPairs.Add(new Tuple<List<CharacterImageData>, List<CharacterImageData>>(nielsenMnist.ValidationData, rawMnist.ValidationData));

            foreach(Tuple<List<CharacterImageData>, List<CharacterImageData>> nielsenRawPair in nielsenRawPairs)
            {
                List<CharacterImageData> nielsenImages = nielsenRawPair.Item1;
                List<CharacterImageData> rawImages = nielsenRawPair.Item2;

                if(nielsenImages.Count != rawImages.Count)
                {
                    throw new Exception();
                }

                int numImages = nielsenImages.Count;
                for (int iImage = 0; iImage < numImages; iImage++)
                {
                    CharacterImageData nielsenImage = nielsenImages[iImage];
                    CharacterImageData rawImage = rawImages[iImage];

                    if(nielsenImage.PixelData.Length != rawImage.PixelData.Length)
                    {
                        throw new Exception();
                    }

                    int numPixels = nielsenImage.PixelData.Length;
                    for (int iPixel = 0; iPixel < numPixels; iPixel++)
                    {
                        float nielsenValue = nielsenImage.PixelData[iPixel];
                        float rawValue = rawImage.PixelData[iPixel];

                        if(nielsenValue != rawValue)
                        {
                            throw new Exception();
                        }
                    }

                    if(nielsenImage.TrueValue != rawImage.TrueValue)
                    {
                        throw new Exception();
                    }
                }
            }

            // If we get to the end without any exceptions, the two data sets are the same.
        }

        private static void TryReadRawMnistData()
        {
            MnistDataSet mnistData = new MnistDataSet();

            Tuple<List<CharacterImageData>, List<CharacterImageData>> trainingAndValidation = Program.GetTrainingAndValidationData();
            mnistData.TrainingData = trainingAndValidation.Item1;
            mnistData.ValidationData = trainingAndValidation.Item2;

            mnistData.TestData = Program.GetTestData();

            string serializationFilePath = Program.RawBasedMnistNetDataFilePath;
            BinarySerializer<MnistDataSet>.SerializeStatic(mnistData, serializationFilePath);
        }

        private static Tuple<List<CharacterImageData>, List<CharacterImageData>> GetTrainingAndValidationData()
        {
            string imagesDataFilePath = Path.Combine(Program.RawMnistFilesDirectoryPath, Program.TrainingImagesFileName);
            string labelsDataFilePath = Path.Combine(Program.RawMnistFilesDirectoryPath, Program.TrainingLabelsFileName);

            Tuple<uint[,,], uint[]> rawData = Program.GetRawData(imagesDataFilePath, labelsDataFilePath);

            List<CharacterImageData> allImages = Program.BuildCharacterImages(rawData.Item1, rawData.Item2);

            int numImages = allImages.Count; // Should be 60,000.
            if(60000 != numImages)
            {
                string message = String.Format(@"Unexpected number of images found. Expected 50,000, found: {0}", numImages);
                throw new Exception(message);
            }

            // Split into training and validation image sets.
            int numTrainingImages = 50000;

            List<CharacterImageData> trainingData = new List<CharacterImageData>();
            for (int iImage = 0; iImage < numTrainingImages; iImage++)
            {
                trainingData.Add(allImages[iImage]);
            }

            List<CharacterImageData> validationData = new List<CharacterImageData>();
            for (int iImage = numTrainingImages; iImage < numImages; iImage++)
            {
                validationData.Add(allImages[iImage]);
            }

            return new Tuple<List<CharacterImageData>, List<CharacterImageData>>(trainingData, validationData);
        }

        private static List<CharacterImageData> GetTestData()
        {
            string imagesDataFilePath = Path.Combine(Program.RawMnistFilesDirectoryPath, Program.TestImagesFileName);
            string labelsDataFilePath = Path.Combine(Program.RawMnistFilesDirectoryPath, Program.TestLabelsFileName);

            Tuple<uint[,,], uint[]> rawData = Program.GetRawData(imagesDataFilePath, labelsDataFilePath);

            List<CharacterImageData> output = Program.BuildCharacterImages(rawData.Item1, rawData.Item2);
            return output;
        }

        private static Tuple<uint[,,], uint[]> GetRawData(string imagesDataFilePath, string labelsDataFilePath)
        {
            uint[,,] images;
            using (FileStream fStream = new FileStream(imagesDataFilePath, FileMode.Open))
            {
                images = Program.DeserializeMnistImages(fStream);
            }

            uint[] labels;
            using (FileStream fStream = new FileStream(labelsDataFilePath, FileMode.Open))
            {
                labels = Program.TryDeserializeMnistLabels(fStream);
            }

            return new Tuple<uint[,,], uint[]>(images, labels);
        }

        private static List<CharacterImageData> BuildCharacterImages(uint[,,] images, uint[] labels)
        {
            int numImages = images.GetLength(0);
            int numLabels = labels.Length;
            if(numImages != numLabels)
            {
                string message = String.Format(@"Different numbers of images and labels. Images: {0}, labels: {1}", numImages, numLabels);
                throw new Exception(message);
            }

            int numPixels = 28;

            int numXPixels = images.GetLength(1);
            if(numPixels != numXPixels)
            {
                string message = String.Format(@"Wrong number of image X pixels found. Expected 28, found: {0}", numXPixels);
            }
            int numYPixels = images.GetLength(2);
            if (numPixels != numYPixels)
            {
                string message = String.Format(@"Wrong number of image Y pixels found. Expected 28, found: {0}", numYPixels);
            }

            List<CharacterImageData> output = new List<CharacterImageData>();
            for (int iImage = 0; iImage < numImages; iImage++)
            {
                float[] pixelData = new float[784]; // 28 x 28.
                int iCount = 0;
                for (int iXPixel = 0; iXPixel < numPixels; iXPixel++)
                {
                    for (int iYPixel = 0; iYPixel < numPixels; iYPixel++)
                    {
                        float value = images[iImage, iXPixel, iYPixel] / 256F;

                        pixelData[iCount] = value;
                        iCount++;
                    }
                }

                int trueValue = (int)labels[iImage];

                CharacterImageData imageData = new CharacterImageData(pixelData, trueValue);
                output.Add(imageData);
            }

            return output;
        }

        private static uint[] TryDeserializeMnistLabels(FileStream fStream)
        {
            // First two alignments bytes are zero.
            int firstByte = fStream.ReadByte();
            if (0 != firstByte)
            {
                string message = String.Format(@"Expected 0 for the first byte, found: {0}", firstByte);
                throw new Exception(message);
            }

            int secondByte = fStream.ReadByte();
            if (0 != secondByte)
            {
                string message = String.Format(@"Expected 0 for the second byte, found: {0}", secondByte);
                throw new Exception(message);
            }

            // Type of data.
            int thirdByte = fStream.ReadByte();
            if (8 != thirdByte)
            {
                string message = String.Format(@"Expected 8 for the third byte indicating datatype is uint, found: {0}", thirdByte);
                throw new Exception(message);
            }

            // Number of data dimensions.
            int fourthByte = fStream.ReadByte();
            if (1 != fourthByte)
            {
                string message = String.Format(@"Expected 1 for the fourth byte indicating 1 dimension for the data, found: {0}", thirdByte);
                throw new Exception(message);
            }

            bool isLittleEndian = BitConverter.IsLittleEndian;

            byte[] temp = new byte[4];
            fStream.Read(temp, 0, 4);

            if (isLittleEndian)
            {
                Array.Reverse(temp);
            }

            int numLabels = BitConverter.ToInt32(temp, 0);

            uint[] output = new uint[numLabels];
            for (int iLabel = 0; iLabel < numLabels; iLabel++)
            {
                int value = fStream.ReadByte(); // Each unsigned integer is one byte.
                if (-1 == value)
                {
                    throw new Exception(@"End of file reached prematurely!");
                }

                uint valueU = (uint)value;

                output[iLabel] = valueU;
            }

            int shouldBeNeg1 = fStream.ReadByte();
            if (-1 != shouldBeNeg1)
            {
                throw new Exception(@"End of file not yet reached!");
            }

            return output;
        }

        private static uint[,,] DeserializeMnistImages(FileStream fStream)
        {
            // First two alignments bytes are zero.
            int firstByte = fStream.ReadByte();
            if (0 != firstByte)
            {
                string message = String.Format(@"Expected 0 for the first byte, found: {0}", firstByte);
                throw new Exception(message);
            }

            int secondByte = fStream.ReadByte();
            if (0 != secondByte)
            {
                string message = String.Format(@"Expected 0 for the second byte, found: {0}", secondByte);
                throw new Exception(message);
            }

            // Type of data.
            int thirdByte = fStream.ReadByte();
            if (8 != thirdByte)
            {
                string message = String.Format(@"Expected 8 for the third byte indicating datatype is uint, found: {0}", thirdByte);
                throw new Exception(message);
            }

            // Number of data dimensions.
            int fourthByte = fStream.ReadByte();
            if (3 != fourthByte)
            {
                string message = String.Format(@"Expected 3 for the fourth byte indicating 3 dimensions for the data, found: {0}", thirdByte);
                throw new Exception(message);
            }
            int numDimensions = 3;

            bool isLittleEndian = BitConverter.IsLittleEndian;

            int[] sizes = new int[numDimensions];
            for (int iDimension = 0; iDimension < numDimensions; iDimension++)
            {
                byte[] temp = new byte[4];
                fStream.Read(temp, 0, 4);

                if (isLittleEndian)
                {
                    Array.Reverse(temp);
                }

                sizes[iDimension] = BitConverter.ToInt32(temp, 0);
            }

            uint[,,] output = new uint[sizes[0], sizes[1], sizes[2]];

            for (int iDim1 = 0; iDim1 < sizes[0]; iDim1++)
            {
                for (int iDim2 = 0; iDim2 < sizes[1]; iDim2++)
                {
                    for (int iDim3 = 0; iDim3 < sizes[2]; iDim3++)
                    {
                        int value = fStream.ReadByte(); // Each unsigned integer is one byte.
                        if (-1 == value)
                        {
                            throw new Exception(@"End of file reached prematurely!");
                        }

                        uint valueU = (uint)value;

                        output[iDim1, iDim2, iDim3] = valueU;
                    }
                }
            }

            int shouldBeNeg1 = fStream.ReadByte();
            if (-1 != shouldBeNeg1)
            {
                throw new Exception(@"End of file not yet reached!");
            }

            return output;
        }

        private static void UnZipAllFiles(bool forceReUnzip = false)
        {
            Tuple<string, string>[] unzipFilePairs = Program.AllRawZipFileToFilePaths;
            foreach(Tuple<string, string> unzipFilePair in unzipFilePairs)
            {
                if(!File.Exists(unzipFilePair.Item2) || (File.Exists(unzipFilePair.Item2) && forceReUnzip))
                {
                    using (FileStream compressedFile = new FileStream(unzipFilePair.Item1, FileMode.Open))
                    using (FileStream decompressedFile = new FileStream(unzipFilePair.Item2, FileMode.Create))
                    using (GZipStream gzip = new GZipStream(compressedFile, CompressionMode.Decompress))
                    {
                        gzip.CopyTo(decompressedFile);
                    }
                }
            }
        }

        private static void TrySerializeDeserializeMnist()
        {
            string dataFilePath = Program.NielsenMnistDataFilePath;

            MnistDataSet mnistData = Program.ParseMnistData(dataFilePath);

            DateTime now;
            TimeSpan elapsedTime;

            BinaryFormatter formatter = new BinaryFormatter();

            now = DateTime.Now;
            string serializationFilePath = Program.NielsenMnistNetDataFilePath;
            BinarySerializer<MnistDataSet>.SerializeStatic(mnistData, serializationFilePath);
            elapsedTime = DateTime.Now - now;
            Console.WriteLine(String.Format(@"Serialization elapsed time: {0}", elapsedTime)); // < 1 second.

            now = DateTime.Now;
            
            elapsedTime = DateTime.Now - now;
            Console.WriteLine(String.Format(@"Deserialization elapsed time: {0}", elapsedTime)); // < 2.5 seconds.
        }

        private static void TryParseTestData()
        {
            string dataFilePath = Program.NielsenMnistDataFilePath;

            DateTime now = DateTime.Now;
            MnistDataSet mnistData = Program.ParseMnistData(dataFilePath);
            TimeSpan elapsedTime = DateTime.Now - now;
            Console.WriteLine(String.Format(@"Elapsed time: {0}", elapsedTime));
        }

        /// <remarks>
        /// The parse operation takes about 1.25 minutes.
        /// </remarks>
        private static MnistDataSet ParseMnistData(string dataFilePath)
        {
            List<CharacterImageData> trainingImageData;
            List<CharacterImageData> validationImageData;
            List<CharacterImageData> testImageData;
            using (Py.GIL())
            {
                dynamic mnist_loader = Py.Import(@"mnist_loader");

                PyObject dataSets = mnist_loader.load_data_wrapper(dataFilePath);

                PyTuple tup = new PyTuple(dataSets);

                PyList trainingData = new PyList(tup.GetItem(0));
                PyList validationData = new PyList(tup.GetItem(1));
                PyList testData = new PyList(tup.GetItem(2));

                trainingImageData = Program.ParseList(trainingData, Program.ParseType2TrueValue);
                validationImageData = Program.ParseList(validationData, Program.ParseType1TrueValue);
                testImageData = Program.ParseList(testData, Program.ParseType1TrueValue);
            }

            MnistDataSet output = new MnistDataSet();
            output.TrainingData = trainingImageData;
            output.ValidationData = validationImageData;
            output.TestData = testImageData;

            return output;
        }

        private static List<CharacterImageData> ParseList(PyList list, Func<PyObject, int> trueValueParser)
        {
            List<CharacterImageData> output = new List<CharacterImageData>();
            foreach (PyObject item in list)
            {
                PyTuple itemTuple = new PyTuple(item);

                // Pixels.
                PyObject pixelsObj = itemTuple[0]; // Type is ndarray.
                PyObject pixelsLenFunctionObj = pixelsObj.GetAttr(@"__len__");

                PyObject pixelsLenObj = pixelsLenFunctionObj.Invoke();
                int numberOfPixels = (int)pixelsLenObj.AsManagedObject(typeof(int));
                float[] pixelData = new float[numberOfPixels];
                int iCount = 0;
                foreach(PyObject pixelObj in pixelsObj)
                {
                    float value = (float)pixelObj.AsManagedObject(typeof(float));

                    pixelData[iCount] = value;
                    iCount++;
                }

                // True value.
                PyObject trueValueObj = itemTuple[1];

                int trueValue = trueValueParser(trueValueObj);

                CharacterImageData image = new CharacterImageData(pixelData, trueValue);
                output.Add(image);
            }

            return output;
        }

        /// <summary>
        /// A type 2 true value object is a vector in which one of the elements is non-zero.
        /// This includes the training data as produced by the mnist loader.
        /// </summary>
        private static int ParseType2TrueValue(PyObject trueValueObject)
        {
            int output = -1;
            int iCount = 0;
            foreach(PyObject index in trueValueObject)
            {
                int curIndexValue = (int)index.AsManagedObject(typeof(int));
                if(0 != curIndexValue)
                {
                    output = iCount;
                    break;
                }

                iCount++;
            }

            return output;
        }

        /// <summary>
        /// A type 1 true value object is just in integer.
        /// This includes validation data and test data as produced by mnist loader.
        /// </summary>
        private static int ParseType1TrueValue(PyObject trueValueObject)
        {
            int output = (int)trueValueObject.AsManagedObject(typeof(int));
            return output;
        }

        private static string GetClassName(PyObject pyObj)
        {
            string output = (string)pyObj.GetAttr(@"__class__").GetAttr(@"__name__").AsManagedObject(typeof(string));
            return output;
        }

        private static void TryRunPythonNet()
        {
            string path = Environment.GetEnvironmentVariable(@"PATH");
            string newPath = @"C:\Organizations\Minex\Repositories\Public\Source\Experiments\Experiments\NeuralNetworksPython2\NeuralNetworksPython2\Code\;" + path;
            Environment.SetEnvironmentVariable(@"PATH", newPath);

            string pythonPath = Environment.GetEnvironmentVariable(@"PYTHONPATH");
            Console.WriteLine(pythonPath);

            PythonEngine.Initialize();
            using (Py.GIL())
            {
                //dynamic np = Py.Import(@"numpy");
                //Console.WriteLine(np.cos(np.pi * 2));

                Console.WriteLine(PythonEngine.PythonPath);

                dynamic mnist_loader = Py.Import(@"mnist_loader");
                dynamic temp = mnist_loader.load_data_wrapper();
                PyObject pyObj = (PyObject)temp;


                PyObject training_data = pyObj.GetItem(0);
                PyObject validation_data = pyObj.GetItem(1);
                PyObject test_data = pyObj.GetItem(2);

                //PyList training_list = new PyList(training_data);
                //Console.WriteLine(training_list.Length());

                //PyObject pyType = pyObj.GetPythonType();
                //Console.WriteLine(pyType);

                //PyObject pyType2 = training_data.GetPythonType();
                //Console.WriteLine(pyType2);

                //System.Collections.Generic.IEnumerable<string> names = pyType2.GetDynamicMemberNames();
                //PyList l = pyType2.Dir();

                //PyObject name = pyType2.GetAttr(@"__name__");
                //Console.WriteLine(name);
                //string nameStr = (string)name.AsManagedObject(typeof(string));

                //string pyObjTypeNameStr = (string)pyObj.GetAttr(@"__class__").GetAttr(@"__name__").AsManagedObject(typeof(string));
                //Console.Write(pyObjTypeNameStr);

                List<float> values = new List<float>();
                foreach (PyObject tup in test_data)
                {
                    PyObject pixels = tup.GetItem(0);
                    foreach (PyObject pixel in pixels)
                    {
                        float value = (float)pixel.AsManagedObject(typeof(float));
                        values.Add(value);
                    }
                }
            }
        }

        #endregion
    }
}
