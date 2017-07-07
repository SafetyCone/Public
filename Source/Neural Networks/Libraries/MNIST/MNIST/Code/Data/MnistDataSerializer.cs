using System;
using System.Collections.Generic;
using System.IO;


namespace Public.NeuralNetworks.MNIST
{
    public class MnistDataSerializer
    {
        public const int ExpectedNumberOfAllTrainingImages = 60000;
        public const int NumberOfTrainingImages = 50000;
        public const int NumberOfValidationImages = 10000;
        public const int ExpectedNumberOfTestingImages = 10000;
        public const double DataMaximumValue = 256;


        #region Static

        public static MnistData DeserializeRaw(
            string trainingImagesDataFilePath, string trainingLabelsDataFilePath,
            string testingImagesDataFilePath, string testingLabelsDataFilePath)
        {
            // Training data.
            Tuple<uint[,,], uint[]> rawTrainingData = MnistDataSerializer.GetRawData(trainingImagesDataFilePath, trainingLabelsDataFilePath);
            List<MnistImage> allTrainingData = MnistDataSerializer.BuildMnistImages(rawTrainingData.Item1, rawTrainingData.Item2);

            // Training data is split into training data and validation data to allow using the "hold-out" method for hyper-parameter exploration.
            List<MnistImage> trainingData = new List<MnistImage>();
            for (int iImage = 0; iImage < MnistDataSerializer.NumberOfTrainingImages; iImage++)
            {
                trainingData.Add(allTrainingData[iImage]);
            }

            List<MnistImage> validationData = new List<MnistImage>();
            for (int iImage = MnistDataSerializer.NumberOfTrainingImages; iImage < MnistDataSerializer.ExpectedNumberOfAllTrainingImages; iImage++)
            {
                validationData.Add(allTrainingData[iImage]);
            }

            // Now the testing data.
            Tuple<uint[,,], uint[]> rawTestingData = MnistDataSerializer.GetRawData(testingImagesDataFilePath, testingLabelsDataFilePath);
            List<MnistImage> testingData = MnistDataSerializer.BuildMnistImages(rawTestingData.Item1, rawTestingData.Item2);

            MnistData output = new MnistData(trainingData, validationData, testingData);
            return output;
        }

        private static List<MnistImage> BuildMnistImages(uint[,,] images, uint[] labels)
        {
            int numImages = images.GetLength(0);
            int numLabels = labels.Length;
            if (numImages != numLabels)
            {
                string message = String.Format(@"Different numbers of images and labels. Images: {0}, labels: {1}", numImages, numLabels);
                throw new Exception(message);
            }

            int numXPixels = images.GetLength(1);
            if (MnistImage.ImageHeightInPixels != numXPixels)
            {
                string message = String.Format(@"Wrong number of image X (height) pixels found. Expected 28, found: {0}", numXPixels);
            }
            int numYPixels = images.GetLength(2);
            if (MnistImage.ImageWidthInPixels != numYPixels)
            {
                string message = String.Format(@"Wrong number of image Y (width) pixels found. Expected 28, found: {0}", numYPixels);
            }

            List<MnistImage> output = new List<MnistImage>();
            for (int iImage = 0; iImage < numImages; iImage++)
            {
                double[] pixelData = new double[784]; // 28 x 28.
                int iCount = 0;
                for (int iXPixel = 0; iXPixel < MnistImage.ImageHeightInPixels; iXPixel++)
                {
                    for (int iYPixel = 0; iYPixel < MnistImage.ImageWidthInPixels; iYPixel++)
                    {
                        double value = images[iImage, iXPixel, iYPixel] / MnistDataSerializer.DataMaximumValue;

                        pixelData[iCount] = value;
                        iCount++;
                    }
                }

                int trueValue = (int)labels[iImage];

                MnistImage imageData = new MnistImage(pixelData, trueValue);
                output.Add(imageData);
            }

            return output;
        }

        private static Tuple<uint[,,], uint[]> GetRawData(string imagesDataFilePath, string labelsDataFilePath)
        {
            uint[,,] images;
            using (FileStream fStream = new FileStream(imagesDataFilePath, FileMode.Open))
            {
                images = MnistDataSerializer.DeserializeMnistImages(fStream);
            }

            uint[] labels;
            using (FileStream fStream = new FileStream(labelsDataFilePath, FileMode.Open))
            {
                labels = MnistDataSerializer.TryDeserializeMnistLabels(fStream);
            }

            return new Tuple<uint[,,], uint[]>(images, labels);
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

        #endregion
    }
}
