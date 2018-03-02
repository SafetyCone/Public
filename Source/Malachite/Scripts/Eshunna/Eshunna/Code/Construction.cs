using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

using Public.Common.Lib.Extensions;
using Public.Common.Lib.IO;
using Public.Common.Lib.IO.Serialization;
using Public.Common.Lib.Visuals;
using Public.Common.Lib.Visuals.MetadataExtractor;
using Public.Common.Lib.Visuals.MSWindows;

using Eshunna.Lib;
using Eshunna.Lib.Logging;
using Eshunna.Lib.Match;
using Eshunna.Lib.NVM;
using Eshunna.Lib.Patches;
using Eshunna.Lib.PLY;
using Eshunna.Lib.SIFT;
using Eshunna.Lib.Verification;
using EshunnaProperties = Eshunna.Properties;


namespace Eshunna
{
    public static class Construction
    {
        public static void SubMain()
        {
            //Construction.Scratch();
            //Construction.RoundTripNvmFileToNetBinary();
            //Construction.RoundTripNViewMatch();
            //Construction.RoundTripNvmFile();
            //Construction.DeserializePatchCollection();
            //Construction.RoundTripPatchFile();
            //Construction.DeserializePlyTextFile();
            //Construction.SerializePlyTextFile();
            //Construction.RoundTripPlyFile();
            //Construction.DeserializePlyBinaryFile();
            //Construction.SerializePlyBinaryFile();
            //Construction.RoundTripPlyFileBinary();
            //Construction.DeserializeSiftFileBinary();
            //Construction.SerializeSiftFileBinary();
            //Construction.RoundTripSiftFileBinary();
            //Construction.DeserializeMatchFile();
            //Construction.SerializeMatchFile();
            //Construction.RoundTripMatchFile();
            //Construction.ConvertNvmToPly();
            //Construction.FindAllDensePlyPointsInPatchFile();
            //Construction.MarkPointInImages();
            Construction.MarkTriangleInImages();
        }

        private static void MarkTriangleInImages()
        {
            var properties = Program.GetProjectProperties();
            string nvmFilePath = properties[EshunnaProperties.ExampleNvmFilePathPropertyName];
            string plyFilePath = properties[EshunnaProperties.ExamplePlyBinaryFilePathPropertyName]; // The vertices and faces meshed, dense PLY file.
            string patchFilePath = properties[EshunnaProperties.ExamplePatchFilePathPropertyName];
            string imagesDirectoryPath = properties[EshunnaProperties.ImagesDirectoryPathPropertyName];

            var nvmFile = NvmV3Serializer.Deserialize(nvmFilePath);
            var plyFile = PlyV1BinarySerializer.Deserialize(plyFilePath);
            var patchFile = PatchCollectionV1Serializer.Deserialize(patchFilePath);

            // Get the face of interest.
            int faceOfInterestIndex = 7945;
            int[] vertexIndices = Construction.GetFaceVertexIndices(plyFile, faceOfInterestIndex);
            int nVertices = vertexIndices.Length; // Should be 3.

            // Get the vertices of the face.
            Location3DHomogenousFloat[] vertices = new Location3DHomogenousFloat[nVertices];
            for (int iVertexIndex = 0; iVertexIndex < nVertices; iVertexIndex++)
            {
                int vertexIndex = vertexIndices[iVertexIndex];

                Location3DHomogenousFloat vertex = Construction.GetPlyFileVertex(plyFile, vertexIndex);
                vertices[iVertexIndex] = vertex;
            }

            // Get the patch for each vertex.
            Patch[] patches = new Patch[nVertices];
            for (int iVertexIndex = 0; iVertexIndex < nVertices; iVertexIndex++)
            {
                var point3d = vertices[iVertexIndex];
                Patch patch = patchFile.Patches.Where((x) => Convert.ToSingle(x.Location.X) == point3d.X && Convert.ToSingle(x.Location.Y) == point3d.Y && Convert.ToSingle(x.Location.Z) == point3d.Z).FirstOrDefault();
                patches[iVertexIndex] = patch;
            }

            // Determine all images that contain at least one of the points.
            HashSet<int> allImageIndices = new HashSet<int>();
            for (int iVertexIndex = 0; iVertexIndex < nVertices; iVertexIndex++)
            {
                Patch patch = patches[iVertexIndex];
                patch.ImageIndicesWithGoodAgreement.ForEach((x) => allImageIndices.Add(x));
            }

            int[] imageIndices = allImageIndices.ToArray();
            int nImageIndices = imageIndices.Length;

            // Get the camera information for each image.
            string[] imageFilePaths = new string[nImageIndices];
            double[] focalLengths = new double[nImageIndices];
            MatrixDouble[] rotations = new MatrixDouble[nImageIndices];
            Location3DDouble[] translations = new Location3DDouble[nImageIndices];
            for (int iImageIndex = 0; iImageIndex < nImageIndices; iImageIndex++)
            {
                int imageIndex = imageIndices[iImageIndex];

                Camera camera = nvmFile.Cameras[imageIndex];

                string fileName = camera.FileName;
                string filePath = Path.Combine(imagesDirectoryPath, fileName);
                imageFilePaths[iImageIndex] = filePath;

                MatrixDouble rotation = QuaternionDouble.GetRotationMatrix(camera.Rotation);
                rotations[iImageIndex] = rotation;

                Location3DDouble translation = Construction.GetTranslation(rotation, camera.Location);
                translations[iImageIndex] = translation;

                focalLengths[iImageIndex] = camera.FocalLength;
            }

            // Create camera matrices.
            Matrix<double>[] cameraMatrices = new Matrix<double>[nImageIndices];
            for (int iImage = 0; iImage < nImageIndices; iImage++)
            {
                var K = DenseMatrix.Build.DenseIdentity(3, 3);
                K[0, 0] = focalLengths[iImage];
                K[1, 1] = focalLengths[iImage];

                var R = DenseMatrix.Build.DenseOfRowMajor(3, 3, rotations[iImage].RowMajorValues);
                var T = DenseVector.Build.DenseOfArray(new double[] { translations[iImage].X, translations[iImage].Y, translations[iImage].Z });
                var P = DenseMatrix.Build.DenseOfColumns(new Vector<double>[] { R.Column(0), R.Column(1), R.Column(2), T });

                var cameraMatrix = K * P;
                cameraMatrices[iImage] = cameraMatrix;
            }

            // Build the array of homogenous 3D point vectors.
            Vector<double>[] vertexHomogenousVectors = new Vector<double>[nVertices];
            for (int iVertex = 0; iVertex < nVertices; iVertex++)
            {
                Location3DHomogenousFloat vertex = vertices[iVertex];
                Vector<double> vertexHomogenous = DenseVector.Build.DenseOfArray(new double[] { vertex.X, vertex.Y, vertex.Z, vertex.H });
                vertexHomogenousVectors[iVertex] = vertexHomogenous;
            }

            Matrix<double> verticesHomogenous = DenseMatrix.Build.DenseOfColumns(vertexHomogenousVectors);

            // Determine image locations for each image and vertex.
            Location2D[][] imageLocations = new Location2D[nImageIndices][];
            for (int iImage = 0; iImage < nImageIndices; iImage++)
            {
                Matrix<double> cameraMatrix = cameraMatrices[iImage];

                var unNormalizedHomogenous2D = cameraMatrix * verticesHomogenous;

                var xVector = unNormalizedHomogenous2D.Row(0) / unNormalizedHomogenous2D.Row(2);
                var yVector = unNormalizedHomogenous2D.Row(1) / unNormalizedHomogenous2D.Row(2);

                Location2D[] locations = new Location2D[nVertices];
                imageLocations[iImage] = locations;
                for (int iVertex = 0; iVertex < nVertices; iVertex++)
                {
                    var location2D = new Location2D(xVector[iVertex], yVector[iVertex]);
                    locations[iVertex] = location2D;
                }
            }

            // Get image sizes.
            Tuple<int, int>[] imageCenters = new Tuple<int, int>[nImageIndices];
            for (int iImage = 0; iImage < nImageIndices; iImage++)
            {
                string filePath = imageFilePaths[iImage];

                var imageSize = ExternalFormatImageSizeProvider.GetSizeS(filePath);

                int imageCenterX = imageSize.X / 2;
                int imageCenterY = imageSize.Y / 2;

                imageCenters[iImage] = Tuple.Create(imageCenterX, imageCenterY);
            }

            // Determine pixel locations for each vertex in each image.
            Tuple<int, int>[][] imagePixelLocations = new Tuple<int, int>[nImageIndices][];
            for (int iImage = 0; iImage < nImageIndices; iImage++)
            {
                Tuple<int, int> imageCenter = imageCenters[iImage];

                Location2D[] locations = imageLocations[iImage];

                Tuple<int, int>[] pixelLocations = new Tuple<int, int>[nVertices];
                imagePixelLocations[iImage] = pixelLocations;
                for (int iVertex = 0; iVertex < nVertices; iVertex++)
                {
                    var location = locations[iVertex];
                    int pixelLocationX = Convert.ToInt32(Math.Round(location.X)) + imageCenter.Item1;
                    int pixelLocationY = Convert.ToInt32(Math.Round(location.Y)) + imageCenter.Item2;
                    var pixelLocation = Tuple.Create(pixelLocationX, pixelLocationY);
                    pixelLocations[iVertex] = pixelLocation;
                }
            }

            // Mark each image.
            for (int iImage = 0; iImage < nImageIndices; iImage++)
            {
                string imageFilePath = imageFilePaths[iImage];
                string outputFilePath = Path.Combine(@"C:\temp", Path.GetFileName(imageFilePath));

                var pixelLocations = imagePixelLocations[iImage];

                ImageMarker.MarkImage(imageFilePath, outputFilePath, pixelLocations);
            }
        }

        private static int[] GetFaceVertexIndices(PlyFile plyFile, int index)
        {
            string faceElementName = PlyFile.faceElementName;
            string xPropertyName = @"vertex_indices";

            var faceVertexValues = (int[][])plyFile.Values[faceElementName][xPropertyName];

            int[] vertexIndices = faceVertexValues[index];
            return vertexIndices;
        }

        private static void MarkPointInImages()
        {
            var properties = Program.GetProjectProperties();
            string nvmFilePath = properties[EshunnaProperties.ExampleNvmFilePathPropertyName];
            string plyFilePath = properties[EshunnaProperties.ExamplePlyTextFilePathPropertyName]; // The vertices only dense PLY file.
            string patchFilePath = properties[EshunnaProperties.ExamplePatchFilePathPropertyName];
            string imagesDirectoryPath = properties[EshunnaProperties.ImagesDirectoryPathPropertyName];

            var nvmFile = NvmV3Serializer.Deserialize(nvmFilePath);
            var plyFile = PlyV1TextSerializer.Deserialize(plyFilePath);
            var patchFile = PatchCollectionV1Serializer.Deserialize(patchFilePath);

            int vertexIndexOfInterest = 1850;
            Location3DHomogenousFloat point3d = Construction.GetPlyFileVertex(plyFile, vertexIndexOfInterest);

            Patch patch = patchFile.Patches.Where((x) => Convert.ToSingle(x.Location.X) == point3d.X && Convert.ToSingle(x.Location.Y) == point3d.Y && Convert.ToSingle(x.Location.Z) == point3d.Z).FirstOrDefault();

            int[] imageIndices = patch.ImageIndicesWithGoodAgreement;
            int nImageIndices = imageIndices.Length;
            string[] imageFilePaths = new string[nImageIndices];
            double[] focalLengths = new double[nImageIndices];
            MatrixDouble[] rotations = new MatrixDouble[nImageIndices];
            Location3DDouble[] translations = new Location3DDouble[nImageIndices];
            for (int iImageIndex = 0; iImageIndex < nImageIndices; iImageIndex++)
            {
                int imageIndex = imageIndices[iImageIndex];

                Camera camera = nvmFile.Cameras[imageIndex];
                
                string fileName = camera.FileName;
                string filePath = Path.Combine(imagesDirectoryPath, fileName);
                imageFilePaths[iImageIndex] = filePath;

                MatrixDouble rotation = QuaternionDouble.GetRotationMatrix(camera.Rotation);
                rotations[iImageIndex] = rotation;

                Location3DDouble translation = Construction.GetTranslation(rotation, camera.Location);
                translations[iImageIndex] = translation;

                focalLengths[iImageIndex] = camera.FocalLength;
            }

            // Create camera matrices.
            Matrix<double>[] cameraMatrices = new Matrix<double>[nImageIndices];
            for (int iImage = 0; iImage < nImageIndices; iImage++)
            {
                var K = DenseMatrix.Build.DenseIdentity(3, 3);
                K[0, 0] = focalLengths[iImage];
                K[1, 1] = focalLengths[iImage];

                var R = DenseMatrix.Build.DenseOfRowMajor(3, 3, rotations[iImage].RowMajorValues);
                var T = DenseVector.Build.DenseOfArray(new double[] { translations[iImage].X, translations[iImage].Y, translations[iImage].Z });
                var P = DenseMatrix.Build.DenseOfColumns(new Vector<double>[] { R.Column(0), R.Column(1), R.Column(2), T });

                var cameraMatrix = K * P;
                cameraMatrices[iImage] = cameraMatrix;
            }

            var homogenousPoint = DenseVector.Build.DenseOfArray(new double[] { point3d.X, point3d.Y, point3d.Z, point3d.H });

            Location2D[] imageLocations = new Location2D[nImageIndices];
            for (int iImage = 0; iImage < nImageIndices; iImage++)
            {
                var cameraMatrix = cameraMatrices[iImage];

                var nonNormalizedHomogenous2D = cameraMatrix * homogenousPoint;

                double x = nonNormalizedHomogenous2D[0] / nonNormalizedHomogenous2D[2];
                double y = nonNormalizedHomogenous2D[1] / nonNormalizedHomogenous2D[2];

                Location2D imageLocation = new Location2D(x, y);
                imageLocations[iImage] = imageLocation;
            }

            // Determine the pixel location for each image.
            Tuple<int, int>[] pixelLocations = new Tuple<int, int>[nImageIndices];
            for (int iImage = 0; iImage < nImageIndices; iImage++)
            {
                var imageSize = ExternalFormatImageSizeProvider.GetSizeS(imageFilePaths[iImage]);

                int imageCenterX = imageSize.X / 2;
                int imageCenterY = imageSize.Y / 2;

                int pixelLocationX = Convert.ToInt32(Math.Round(imageLocations[iImage].X)) + imageCenterX;
                int pixelLocationY = Convert.ToInt32(Math.Round(imageLocations[iImage].Y)) + imageCenterY;

                pixelLocations[iImage] = Tuple.Create(pixelLocationX, pixelLocationY);
            }

            // Mark each image.
            for (int iImage = 0; iImage < nImageIndices; iImage++)
            {
                string imageFilePath = imageFilePaths[iImage];
                string outputFilePath = Path.Combine(@"C:\temp", Path.GetFileName(imageFilePath));

                var pixel = pixelLocations[iImage];

                ImageMarker.MarkImage(imageFilePath, outputFilePath, pixel.Item1, pixel.Item2);
            }
        }

        private static Location3DDouble GetTranslation(MatrixDouble rotation, Location3DDouble cameraLocation)
        {
            double[] values = rotation.RowMajorValues;

            double x = -(values[0] * cameraLocation.X + values[1] * cameraLocation.Y + values[2] * cameraLocation.Z);
            double y = -(values[3] * cameraLocation.X + values[4] * cameraLocation.Y + values[5] * cameraLocation.Z);
            double z = -(values[6] * cameraLocation.X + values[7] * cameraLocation.Y + values[8] * cameraLocation.Z);

            Location3DDouble output = new Location3DDouble(x, y, z);
            return output;
        }

        private static Location3DHomogenousFloat GetPlyFileVertex(PlyFile plyFile, int index)
        {
            string vertexElementName = PlyFile.vertexElementName;
            string xPropertyName = @"x";
            string yPropertyName = @"y";
            string zPropertyName = @"z";

            var xValues = (float[])plyFile.Values[vertexElementName][xPropertyName];
            var yValues = (float[])plyFile.Values[vertexElementName][yPropertyName];
            var zValues = (float[])plyFile.Values[vertexElementName][zPropertyName];

            float xValue = xValues[index];
            float yValue = yValues[index];
            float zValue = zValues[index];

            Location3DHomogenousFloat output = new Location3DHomogenousFloat(xValue, yValue, zValue, 1);
            return output;
        }

        private static void FindAllDensePlyPointsInPatchFile()
        {
            var properties = Program.GetProjectProperties();
            string plyFilePath = properties[EshunnaProperties.ExamplePlyTextFilePathPropertyName];
            string patchFilePath = properties[EshunnaProperties.ExamplePatchFilePathPropertyName];

            var plyFile = PlyV1TextSerializer.Deserialize(plyFilePath);
            var patchFile = PatchCollectionV1Serializer.Deserialize(patchFilePath);

            string vertexElementName = PlyFile.vertexElementName;
            string xPropertyName = @"x";
            string yPropertyName = @"y";
            string zPropertyName = @"z";

            int nValues = plyFile.Header.Elements.Where((x) => x.Name == vertexElementName).First().Count;
            var xValues = (float[])plyFile.Values[vertexElementName][xPropertyName];
            var yValues = (float[])plyFile.Values[vertexElementName][yPropertyName];
            var zValues = (float[])plyFile.Values[vertexElementName][zPropertyName];

            for (int iValue = 0; iValue < nValues; iValue++)
            {
                var xValue = xValues[iValue];
                var yValue = yValues[iValue];
                var zValue = zValues[iValue];

                var patch = patchFile.Patches[iValue];
                var patchLocation = patch.Location;

                var patchX = Convert.ToSingle(patchLocation.X);
                var patchY = Convert.ToSingle(patchLocation.Y);
                var patchZ = Convert.ToSingle(patchLocation.Z);

                if (xValue != patchX || yValue != patchY || zValue != patchZ)
                {
                    throw new Exception();
                }
            }
        }

        private static void ConvertNvmToPly()
        {
            var properties = Program.GetProjectProperties();
            string inputNvmFilePath = properties[EshunnaProperties.ExampleNvmFilePathPropertyName];
            string outputPlyFilePath = @"C:\temp\Model-Sparse.ply";


            NViewMatch nvm = NvmV3Serializer.Deserialize(inputNvmFilePath);

            // Get data from the NVM.
            int nFeatures = nvm.FeaturePoints.Length;

            // Build the PLY file.
            string vertexElementName = PlyFile.vertexElementName;

            var header = PlyFileHeader.GetDefault();
            var vertexDescriptor = new PlyElementDescriptor
            {
                Count = nFeatures,
                Name = vertexElementName
            };
            header.Elements.Add(vertexDescriptor);

            string xPropertyName = @"x";
            string yPropertyName = @"y";
            string zPropertyName = @"z";
            string redPropertyName = @"diffuse_red";
            string greenPropertyName = @"diffuse_green";
            string bluePropertyName = @"diffuse_blue";
            vertexDescriptor.PropertyDescriptors.AddRange(new PlyPropertyDescriptor[]
            {
                new PlyPropertyDescriptor(xPropertyName, PlyDataType.Float),
                new PlyPropertyDescriptor(yPropertyName, PlyDataType.Float),
                new PlyPropertyDescriptor(zPropertyName, PlyDataType.Float),
                new PlyPropertyDescriptor(redPropertyName, PlyDataType.CharacterUnsigned),
                new PlyPropertyDescriptor(greenPropertyName, PlyDataType.CharacterUnsigned),
                new PlyPropertyDescriptor(bluePropertyName, PlyDataType.CharacterUnsigned),
            });

            var plyFile = new PlyFile(header);

            var xValues = new float[nFeatures];
            var yValues = new float[nFeatures];
            var zValues = new float[nFeatures];
            var redValues = new byte[nFeatures];
            var greenValues = new byte[nFeatures];
            var blueValues = new byte[nFeatures];
            var vertexValues = new Dictionary<string, object>
            {
                { xPropertyName, xValues },
                { yPropertyName, yValues },
                { zPropertyName, zValues },
                { redPropertyName, redValues },
                { greenPropertyName, greenValues },
                { bluePropertyName, blueValues }
            };
            plyFile.Values.Add(vertexElementName, vertexValues);

            for (int iFeature = 0; iFeature < nFeatures; iFeature++)
            {
                var feature = nvm.FeaturePoints[iFeature];
                xValues[iFeature] = Convert.ToSingle(feature.Location.X);
                yValues[iFeature] = Convert.ToSingle(feature.Location.Y);
                zValues[iFeature] = Convert.ToSingle(feature.Location.Z);
                redValues[iFeature] = feature.Color.Red;
                greenValues[iFeature] = feature.Color.Green;
                blueValues[iFeature] = feature.Color.Blue;
            }

            PlyV1TextSerializer.Serialize(outputPlyFilePath, plyFile);
        }

        private static void RoundTripMatchFile()
        {
            var properties = Program.GetProjectProperties();

            string exampleFilePath = properties[EshunnaProperties.ExampleMatchFilePathPropertyName];
            string serializationFilePath = @"C:\temp\match.mat";

            var serializer = new MatchFileSerializer();

            var log1 = new StringListLog();
            var matchFileComparer = new MatchFileEqualityComparer(log1);

            bool matchFilesEqual = RoundTripExternalDataStructureVerifier.Verify(serializer, exampleFilePath, serializationFilePath, matchFileComparer);

            string logFilePath = @"C:\temp\match file.txt";
            if (matchFilesEqual)
            {
                File.WriteAllText(logFilePath, @"Match files are equal.");
            }
            else
            {
                File.WriteAllLines(logFilePath, log1.Lines);
            }

            var log2 = new StringListLog();
            //var fileComparer = new BinaryFileComparer(log2);
            var fileComparer = new TextFileComparer(log2);

            bool textFilesEqual = RoundTripExternalFileFormat.Verify(serializer, exampleFilePath, serializationFilePath, fileComparer);
            string textFilePath = @"C:\temp\text file.txt";
            if (textFilesEqual)
            {
                File.WriteAllText(textFilePath, @"Text files are equal.");
            }
            else
            {
                File.WriteAllLines(textFilePath, log2.Lines);
            }
        }

        private static void SerializeMatchFile()
        {
            var properties = Program.GetProjectProperties();

            string exampleFilePath = properties[EshunnaProperties.ExampleMatchFilePathPropertyName];
            string serializaztionFilePath = @"C:\temp\match.mat";

            MatchFile matchFile = MatchFileSerializer.Deserialize(exampleFilePath);

            MatchFileSerializer.Serialize(serializaztionFilePath, matchFile);

            MatchFile matchFile2 = MatchFileSerializer.Deserialize(serializaztionFilePath);

            var log1 = new StringListLog();
            var matchFileComparer = new MatchFileEqualityComparer(log1);

            bool matchFilesEqual = matchFileComparer.Equals(matchFile, matchFile2);

            string logFilePath = @"C:\temp\match file.txt";
            if (matchFilesEqual)
            {
                File.WriteAllText(logFilePath, @"Match files are equal.");
            }
            else
            {
                File.WriteAllLines(logFilePath, log1.Lines);
            }
        }

        private static void DeserializeMatchFile()
        {
            var properties = Program.GetProjectProperties();

            string exampleFilePath = properties[EshunnaProperties.ExampleMatchFilePathPropertyName];

            MatchFile matchFile = MatchFileSerializer.Deserialize(exampleFilePath);
        }

        private static void RoundTripSiftFileBinary()
        {
            var properties = Program.GetProjectProperties();

            string exampleFilePath = properties[EshunnaProperties.ExampleSiftBinaryFilePathPropertyName];
            string serializationFilePath = @"C:\temp\binary.sift";

            var serializer = new SiftBinarySerializer();

            var log = new StringListLog();
            var siftFileComparer = new SiftFileEqualityComparer(log);

            bool siftFilesEqual = RoundTripExternalDataStructureVerifier.Verify(serializer, exampleFilePath, serializationFilePath, siftFileComparer);

            string logFilePath = @"C:\temp\sift binary file.txt";
            if (siftFilesEqual)
            {
                File.WriteAllText(logFilePath, @"Sift binary files are equal.");
            }
            else
            {
                File.WriteAllLines(logFilePath, log.Lines);
            }

            var fileComparer = new TextFileComparer(log);

            bool textFilesEqual = RoundTripExternalFileFormat.Verify(serializer, exampleFilePath, serializationFilePath, fileComparer);
            string textFilePath = @"C:\temp\text file.txt";
            if (textFilesEqual)
            {
                File.WriteAllText(textFilePath, @"Text files are equal.");
            }
            else
            {
                File.WriteAllLines(textFilePath, log.Lines);
            }
        }

        private static void SerializeSiftFileBinary()
        {
            var properties = Program.GetProjectProperties();

            string exampleFilePath = properties[EshunnaProperties.ExampleSiftBinaryFilePathPropertyName];
            string serializationFilePath = @"C:\temp\binary.sift";

            SiftFile siftFile = SiftBinarySerializer.Deserialize(exampleFilePath);

            SiftBinarySerializer.Serialize(serializationFilePath, siftFile);
        }

        private static void DeserializeSiftFileBinary()
        {
            var properties = Program.GetProjectProperties();

            string exampleFilePath = properties[EshunnaProperties.ExampleSiftBinaryFilePathPropertyName];

            SiftFile siftFile = SiftBinarySerializer.Deserialize(exampleFilePath);
        }

        private static void RoundTripPlyFileBinary()
        {
            var properties = Program.GetProjectProperties();

            string exampleFilePath = properties[EshunnaProperties.ExamplePlyBinaryFilePathPropertyName];
            string serializationFilePath = @"C:\temp\binary.ply";

            var serializer = new PlyV1BinarySerializer();

            var log = new StringListLog();
            var plyFileComparer = new PlyFileEqualityComparer(log);

            bool plyFilesEqual = RoundTripExternalDataStructureVerifier.Verify(serializer, exampleFilePath, serializationFilePath, plyFileComparer);

            string logFilePath = @"C:\temp\ply file.txt";
            if (plyFilesEqual)
            {
                File.WriteAllText(logFilePath, @"PLY files are equal.");
            }
            else
            {
                File.WriteAllLines(logFilePath, log.Lines);
            }

            var fileComparer = new TextFileComparer(log);

            bool textFilesEqual = RoundTripExternalFileFormat.Verify(serializer, exampleFilePath, serializationFilePath, fileComparer);
            string textFilePath = @"C:\temp\text file.txt";
            if (textFilesEqual)
            {
                File.WriteAllText(textFilePath, @"Text files are equal.");
            }
            else
            {
                File.WriteAllLines(textFilePath, log.Lines);
            }
        }

        private static void SerializePlyBinaryFile()
        {
            var properties = Program.GetProjectProperties();

            string exampleFilePath = properties[EshunnaProperties.ExamplePlyBinaryFilePathPropertyName];
            string serializationFilePath = @"C:\temp\binary.ply";

            PlyFile plyFile = PlyV1BinarySerializer.Deserialize(exampleFilePath);

            PlyV1BinarySerializer.Serialize(serializationFilePath, plyFile);

            PlyFile plyFile2 = PlyV1BinarySerializer.Deserialize(serializationFilePath);
        }

        private static void DeserializePlyBinaryFile()
        {
            var properties = Program.GetProjectProperties();

            string exampleFilePath = properties[EshunnaProperties.ExamplePlyBinaryFilePathPropertyName];

            PlyFile plyFile = PlyV1BinarySerializer.Deserialize(exampleFilePath);
        }

        private static void RoundTripPlyFile()
        {
            var properties = Program.GetProjectProperties();

            string exampleFilePath = properties[EshunnaProperties.ExamplePlyTextFilePathPropertyName];
            string serializationFilePath = @"C:\temp\temp.ply";

            var serializer = new PlyV1TextSerializer();

            var log = new StringListLog();
            var plyFileComparer = new PlyFileEqualityComparer(log);

            bool plyFilesEqual = RoundTripExternalDataStructureVerifier.Verify(serializer, exampleFilePath, serializationFilePath, plyFileComparer);

            string logFilePath = @"C:\temp\ply file.txt";
            if (plyFilesEqual)
            {
                File.WriteAllText(logFilePath, @"PLY files are equal.");
            }
            else
            {
                File.WriteAllLines(logFilePath, log.Lines);
            }

            var fileComparer = new TextFileComparer(log);

            bool textFilesEqual = RoundTripExternalFileFormat.Verify(serializer, exampleFilePath, serializationFilePath, fileComparer);
            string textFilePath = @"C:\temp\text file.txt";
            if (textFilesEqual)
            {
                File.WriteAllText(textFilePath, @"Text files are equal.");
            }
            else
            {
                File.WriteAllLines(textFilePath, log.Lines);
            }
        }

        private static void SerializePlyTextFile()
        {
            var properties = Program.GetProjectProperties();

            string exampleFilePath = properties[EshunnaProperties.ExamplePlyTextFilePathPropertyName];
            string serializationFilePath = @"C:\temp\temp.ply";

            PlyFile plyFile = PlyV1TextSerializer.Deserialize(exampleFilePath);

            PlyV1TextSerializer.Serialize(serializationFilePath, plyFile);
        }

        private static void DeserializePlyTextFile()
        {
            var properties = Program.GetProjectProperties();

            string exampleFilePath = properties[EshunnaProperties.ExamplePlyTextFilePathPropertyName];

            PlyFile plyFile = PlyV1TextSerializer.Deserialize(exampleFilePath);
        }

        private static void RoundTripPatchFile()
        {
            var properties = Program.GetProjectProperties();

            string exampleFilePath = properties[EshunnaProperties.ExamplePatchFilePathPropertyName];
            string serializationFilePath = @"C:\temp\temp.patch";

            var serializer = new PatchCollectionV1Serializer();

            var log = new StringListLog();
            var patchCollectionComparer = new PatchCollectionEqualityComparer(log);

            bool patchCollectionsEqual = RoundTripExternalDataStructureVerifier.Verify(serializer, exampleFilePath, serializationFilePath, patchCollectionComparer);
            if (!patchCollectionsEqual)
            {
                string logFilePath = @"C:\temp\patch collections.txt";
                File.WriteAllLines(logFilePath, log.Lines);
            }

            var fileComparer = new TextFileComparer(log);

            bool textFilesEqual = RoundTripExternalFileFormat.Verify(serializer, exampleFilePath, serializationFilePath, fileComparer);
            if (!textFilesEqual)
            {
                string logFilePath = @"C:\temp\text file.txt";
                File.WriteAllLines(logFilePath, log.Lines);
            }
        }

        private static void DeserializePatchCollection()
        {
            var properties = Program.GetProjectProperties();

            string exampleFilePath = properties[EshunnaProperties.ExamplePatchFilePathPropertyName];

            PatchCollection patchCollection = PatchCollectionV1Serializer.Deserialize(exampleFilePath);
        }

        private static void RoundTripNvmFile()
        {
            var properties = Program.GetProjectProperties();

            string exampleFilePath = properties[EshunnaProperties.ExampleNvmFilePathPropertyName];
            string serializationFilePath = @"C:\temp\temp.nvm";

            var serializer = new NvmV3Serializer();

            var log = new StringListLog();
            var fileComparer = new TextFileComparer(log);

            bool textFilesEqual = RoundTripExternalFileFormat.Verify(serializer, exampleFilePath, serializationFilePath, fileComparer);
            if(!textFilesEqual)
            {
                string logFilePath = @"C:\temp\log.txt";
                File.WriteAllLines(logFilePath, log.Lines);
            }
        }

        private static void RoundTripNViewMatch()
        {
            var properties = Program.GetProjectProperties();

            string exampleFilePath = properties[EshunnaProperties.ExampleNvmFilePathPropertyName];
            string serializationFilePath = @"C:\temp\temp.nvm";

            var serializer = new NvmV3Serializer();

            var log = new StringListLog();
            var comparer = new NViewMatchEqualityComparer(log);

            bool nViewMatchesEqual = RoundTripExternalDataStructureVerifier.Verify(serializer, exampleFilePath, serializationFilePath, comparer);
            if (!nViewMatchesEqual)
            {
                string logFilePath = @"C:\temp\log.txt";
                File.WriteAllLines(logFilePath, log.Lines);
            }
        }

        private static void RoundTripNvmFileToNetBinary()
        {
            var properties = Program.GetProjectProperties();

            string exampleFilePath = properties[EshunnaProperties.ExampleNvmFilePathPropertyName];

            NViewMatch nViewMatch = NvmV3Serializer.Deserialize(exampleFilePath);

            string tempBinaryFileName = @"C:\temp\Model NVM.dat";
            BinaryFileSerializer.Serialize(tempBinaryFileName, nViewMatch);

            NViewMatch nViewMatch2 = BinaryFileSerializer.Deserialize<NViewMatch>(tempBinaryFileName);


        }

        private static void Scratch()
        {
            //double[] values = { -20.1572380596, -0.0269060720416, 103.726450237, 10.3726450237, 1.03726450237, -0.0700699378582, -0.00945108265552 };
            //string[] strings = { @"-20.1572380596", @"-0.0269060720416", @"103.726450237", @"10.3726450237", @"1.03726450237", @"-0.0700699378582", @"-0.00945108265552" };

            //int nValues = values.Length;
            //for (int iValue = 0; iValue < nValues; iValue++)
            //{
            //    double value = values[iValue];
            //    string @string = strings[iValue];

            //    string valueAs12DigitString = value.Format12SignificantDigits();
            //    if(@string != valueAs12DigitString)
            //    {
            //        throw new Exception();
            //    }
            //}

            //double value = 0.263498472003;
            //double value = -141;
            //double value = -2.77555756156e-017;
            //string valueStr = value.FormatNvm12SignificantDigits();

            //double value = -9.8642e-005;
            //string valueStr = value.FormatPatch6SignificantDigits();

            //int[][] temp = new int[5][];

            //temp[0] = new int[2];
            //temp[1] = new int[3];
            //temp[2] = new int[4];

            //var list = new List<string>(4);
            //list.Add(@""); // list[0] = @""; // Errors.
        }
    }
}
