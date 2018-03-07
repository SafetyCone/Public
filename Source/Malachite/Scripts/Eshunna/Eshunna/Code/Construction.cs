using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

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
        private const int CubeEdgeLengthPx = 120;
        private const string CubeMaterialFileName = @"temp.mtl";
        private const int NFaces = 12;
        private const int TextureWidth = Construction.NFaces * Construction.CubeEdgeLengthPx;
        private const int TextureHeight = Construction.CubeEdgeLengthPx;


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
            //Construction.MarkTriangleInImages();
            //Construction.CreateCubeObjFile();
            //Construction.CreateCubeTexture();
            //Construction.CreateCubeMaterialFile();
            Construction.DetermineMostFrontalImage();
            //Construction.DetermineAllPatchImageIndices();
            //Construction.CreateStructureModel();
            //Construction.CompareVectorNormals();
            //Construction.TestPointInTriangleInteger();
            //Construction.TestPointInTriangleDouble();
        }

        private static void TestPointInTriangleDouble()
        {
            int nRows = 10;
            int nCols = 10;
            Bitmap b = new Bitmap(nRows, nCols, PixelFormat.Format24bppRgb);

            var v1 = new Location2Double(3, 3);
            var v2 = new Location2Double(5, 8);
            var v3 = new Location2Double(9, 5);

            for (int iRow = 0; iRow < nRows; iRow++)
            {
                for (int iCol = 0; iCol < nCols; iCol++)
                {
                    bool isPointInTriangle = Construction.IsPointInTriangle(new Location2Double(iRow, iCol), v1, v2, v3);
                    if (isPointInTriangle)
                    {
                        b.SetPixel(iRow, iCol, System.Drawing.Color.Black);
                    }
                    else
                    {
                        b.SetPixel(iRow, iCol, System.Drawing.Color.White);
                    }
                }
            }

            string outputFileName = @"C:\temp\triangle test.bmp";
            b.Save(outputFileName, System.Drawing.Imaging.ImageFormat.Bmp);
        }

        private static bool IsPointInTriangle(Location2Double point, Location2Double v1, Location2Double v2, Location2Double v3, double tolerance = 0)
        {
            bool b1 = Construction.Sign(point, v1, v2) < tolerance;
            bool b2 = Construction.Sign(point, v2, v3) < tolerance;
            bool b3 = Construction.Sign(point, v3, v1) < tolerance;

            bool output = ((b1 == b2) && (b2 == b3));
            return output;
        }

        private static double Sign(Location2Double point, Location2Double u, Location2Double v)
        {
            double output = (point.X - v.X) * (u.Y - v.Y) - (point.Y - v.Y) * (u.X - v.X);
            return output;
        }

        private static void TestPointInTriangleInteger()
        {
            int nRows = 10;
            int nCols = 10;
            Bitmap b = new Bitmap(nRows, nCols, PixelFormat.Format24bppRgb);

            var v1 = new Location2Integer(3, 3);
            var v2 = new Location2Integer(5, 8);
            var v3 = new Location2Integer(9, 5);

            for (int iRow = 0; iRow < nRows; iRow++)
            {
                for (int iCol = 0; iCol < nCols; iCol++)
                {
                    bool isPointInTriangle = Construction.IsPointInTriangle(new Location2Integer(iRow, iCol), v1, v2, v3, 1);
                    if(isPointInTriangle)
                    {
                        b.SetPixel(iRow, iCol, System.Drawing.Color.Black);
                    }
                    else
                    {
                        b.SetPixel(iRow, iCol, System.Drawing.Color.White);
                    }
                }
            }

            string outputFileName = @"C:\temp\triangle test.bmp";
            b.Save(outputFileName, System.Drawing.Imaging.ImageFormat.Bmp);
        }

        private static bool IsPointInTriangle(Location2Integer point, Location2Integer v1, Location2Integer v2, Location2Integer v3, int tolerance)
        {
            bool b1 = Construction.Sign(point, v1, v2) < tolerance;
            bool b2 = Construction.Sign(point, v2, v3) < tolerance;
            bool b3 = Construction.Sign(point, v3, v1) < tolerance;

            bool output = ((b1 == b2) && (b2 == b3));
            return output;
        }

        private static int Sign(Location2Integer point, Location2Integer u, Location2Integer v)
        {
            int output = (point.X - v.X) * (u.Y - v.Y) - (point.Y - v.Y) * (u.X - v.X);
            return output;
        }

        private static void CompareVectorNormals()
        {
            var properties = Program.GetProjectProperties();

            string plyFilePath = properties[EshunnaProperties.ExamplePlyBinaryFilePathPropertyName]; // The vertices and faces meshed, dense PLY file.
            string patchFilePath = properties[EshunnaProperties.ExamplePatchFilePathPropertyName];

            var plyFile = PlyV1BinarySerializer.Deserialize(plyFilePath);
            var patchFile = PatchCollectionV1Serializer.Deserialize(patchFilePath);

            var structureModel = Operations.GetStructureModel(plyFile);

            int nFacets = structureModel.Facets.Count;
            Vector3Double[] vertexCrossProductNormals = new Vector3Double[nFacets];
            Vector3Double[] vertexPatchNormalsAverageNormals = new Vector3Double[nFacets];
            Vector3Double[] diffs = new Vector3Double[nFacets];
            double[] diffL2Norms = new double[nFacets];
            Vector3Double[] negDiffs = new Vector3Double[nFacets];
            double[] negDiffL2Norms = new double[nFacets];
            double[] dotProducts = new double[nFacets];
            double[] negDotProducts = new double[nFacets];
            for (int iFacet = 0; iFacet < nFacets; iFacet++)
            {
                Vector3Double vertexCrossProductNormal = Operations.GetFacetNormal(structureModel, iFacet);
                vertexCrossProductNormals[iFacet] = vertexCrossProductNormal;

                Vector3Double vertexPatchNormalsAverageNormal = Operations.GetFacetNormal(structureModel, iFacet, patchFile);
                vertexPatchNormalsAverageNormals[iFacet] = vertexPatchNormalsAverageNormal;

                Vector3Double diff = vertexCrossProductNormal - vertexPatchNormalsAverageNormal;
                diffs[iFacet] = diff;

                double diffL2Norm = diff.L2Norm();
                diffL2Norms[iFacet] = diffL2Norm;

                Vector3Double negVertexCrossProductNormal = -vertexCrossProductNormal;

                Vector3Double negDiff = negVertexCrossProductNormal - vertexPatchNormalsAverageNormal;
                negDiffs[iFacet] = negDiff;

                double negDiffL2Norm = negDiff.L2Norm();
                negDiffL2Norms[iFacet] = negDiffL2Norm;

                double dotProduct = vertexCrossProductNormal.Dot(vertexPatchNormalsAverageNormal);
                dotProducts[iFacet] = dotProduct;

                double negDotProduct = negVertexCrossProductNormal.Dot(vertexPatchNormalsAverageNormal);
                negDotProducts[iFacet] = negDotProduct;
            }

            CsvFile.WriteAllValues(@"C:\temp\normal diffs.csv", diffL2Norms);
            CsvFile.WriteAllValues(@"C:\temp\neg normal diffs.csv", negDiffL2Norms);
            CsvFile.WriteAllValues(@"C:\temp\dot products.csv", dotProducts);
            CsvFile.WriteAllValues(@"C:\temp\neg dot products.csv", negDotProducts);
        }

        private static void CreateStructureModel()
        {
            var properties = Program.GetProjectProperties();
            string meshedPlyFilePath = properties[EshunnaProperties.ExamplePlyBinaryFilePathPropertyName];

            var plyFile = PlyV1BinarySerializer.Deserialize(meshedPlyFilePath);

            var structureModel = Operations.GetStructureModel(plyFile);
        }

        private static void DetermineAllPatchImageIndices()
        {
            var properties = Program.GetProjectProperties();
            string patchFilePath = properties[EshunnaProperties.ExamplePatchFilePathPropertyName];

            var patchFile = PatchCollectionV1Serializer.Deserialize(patchFilePath);

            HashSet<int> allImageIndices = new HashSet<int>();
            foreach (var patch in patchFile.Patches)
            {
                patch.ImageIndicesWithGoodAgreement.ForEach((x) => allImageIndices.Add(x));
                patch.ImageIndicesWithSomeAgreement.ForEach((x) => allImageIndices.Add(x));
            }

            List<int> allImageIndicesInOrder = new List<int>(allImageIndices);
            allImageIndicesInOrder.Sort();
        }

        private static void DetermineMostFrontalImage()
        {
            var properties = Program.GetProjectProperties();
            string nvmFilePath = properties[EshunnaProperties.ExampleNvmFilePathPropertyName];
            string patchFilePath = properties[EshunnaProperties.ExamplePatchFilePathPropertyName];
            string meshedPlyFilePath = properties[EshunnaProperties.ExamplePlyBinaryFilePathPropertyName];

            var nvmFile = NvmV3Serializer.Deserialize(nvmFilePath);
            var patchFile = PatchCollectionV1Serializer.Deserialize(patchFilePath);
            var plyFile = PlyV1BinarySerializer.Deserialize(meshedPlyFilePath);

            var structureModel = Operations.GetStructureModel(plyFile);

            int facetIndex = 1399;
            int[] imageIndicesForFacet = Operations.GetImageIndicesForFacet(structureModel, facetIndex, patchFile);

            var facetCentroidLocation = Operations.GetFacetCentroid(structureModel, facetIndex).ToVector3Double();

            var facetNormal = Operations.GetFacetNormal(structureModel, facetIndex);

            int nImages = imageIndicesForFacet.Length;
            double[] dotProducts = new double[nImages];
            for (int iImage = 0; iImage < nImages; iImage++)
            {
                int imageIndex = imageIndicesForFacet[iImage];
                var camera = nvmFile.Cameras[imageIndex];
                var cameraLocation = camera.Location.ToVector3Double();

                var centroidToCamera = cameraLocation - facetCentroidLocation;
                var centroidToCamerDirection = centroidToCamera.L2Normalize();

                double facetNormalAndCentroidToCameraDotProduct = facetNormal.Dot(centroidToCamerDirection);
                dotProducts[iImage] = facetNormalAndCentroidToCameraDotProduct;
                //double rotationAngle = Math.Acos(facetNormalAndCentroidToCameraDotProduct); // Relative to the cross product u x v, the angle theta is always what counterclockwise from u to v. So, u would need to rotate by theta to reach v.

                //var facetNormalAndCentroidToCameraCrossProduct = facetNormal.Cross(centroidToCamerDirection);

                // Rotation matrix for angle theta about the cross product axis.
            }

            double max = Double.NegativeInfinity;
            int indexOfMax = -1;
            for (int iImage = 0; iImage < nImages; iImage++)
            {
                double value = dotProducts[iImage];
                if(value > max)
                {
                    max = value;
                    indexOfMax = iImage;
                }
            }

            //Patch patch = patchFile.Patches[0];
            //int[] imageIndicesWithGoodAlignment = patch.ImageIndicesWithGoodAgreement;
            //var patchLocation = patch.Location.ToLocation3Double();
            //var patchNormal = patch.Normal.ToLocation3Double();
            //Vector<double> patchNormalV = DenseVector.Build.DenseOfArray(new double[] { patchNormal.X, patchNormal.Y, patchNormal.Z });

            //int cameraIndex = imageIndicesWithGoodAlignment[0];
            //Camera camera = nvmFile.Cameras[cameraIndex];
            //var cameraLocation = camera.Location;

            //var patchToCamera = cameraLocation - patchLocation;
            //Vector<double> patchToCameraV = DenseVector.Build.DenseOfArray(new double[] { patchToCamera.X, patchToCamera.Y, patchToCamera.Z });
            //Vector<double> normalizedPatchToCamera = patchToCameraV.Normalize(2);

            //var diffPatchToCameraWithNormal = patchNormalV - normalizedPatchToCamera;
        }

        private static void CreateCubeMaterialFile()
        {
            string filePath = $@"C:\temp\{Construction.CubeMaterialFileName}";

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine(@"newmtl material0");
                writer.WriteLine(@"Ka 1.000000 1.000000 1.000000");
                writer.WriteLine(@"Kd 1.000000 1.000000 1.000000");
                writer.WriteLine(@"Ks 0.000000 0.000000 0.000000");
                writer.WriteLine(@"Tr 1.000000");
                writer.WriteLine(@"illum 1");
                writer.WriteLine(@"Ns 0.000000");
                writer.WriteLine(@"map_Kd texture.bmp");
            }
        }

        private static void CreateCubeTexture()
        {
            int nFaces = 12;
            Bitmap texture = new Bitmap(Construction.TextureWidth, Construction.TextureHeight, PixelFormat.Format24bppRgb);
            using (Graphics gfx = Graphics.FromImage(texture))
            using (SolidBrush brush = new SolidBrush(System.Drawing.Color.Red))
            {
                gfx.FillRectangle(brush, 0, 0, Construction.TextureWidth, Construction.TextureHeight);
            }

            using (Graphics gfx = Graphics.FromImage(texture))
            {
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gfx.PixelOffsetMode = PixelOffsetMode.HighQuality;
                for (int iFace = 0; iFace < nFaces; iFace++)
                {
                    //int leftLimitPx = iFace * Construction.CubeEdgeLengthPx;
                    int leftLimitPx = iFace * Construction.CubeEdgeLengthPx + Construction.CubeEdgeLengthPx / 2;
                    int upperLimitPx = 0;
                    //int upperLimitPx = Construction.CubeEdgeLengthPx / 2;
                    int width = Construction.CubeEdgeLengthPx / 2;
                    int height = Construction.CubeEdgeLengthPx / 2;

                    gfx.DrawString((iFace + 1).ToString(), new Font(FontFamily.GenericMonospace, 20), Brushes.Black, new RectangleF(leftLimitPx, upperLimitPx, width, height));
                    gfx.DrawString((iFace + 1).ToString(), new Font(FontFamily.GenericMonospace, 20), Brushes.Black, new RectangleF(leftLimitPx, upperLimitPx + 25, width, height));
                    //gfx.DrawString(iFace.ToString(), new Font(FontFamily.GenericMonospace, 20), Brushes.Black, new RectangleF(leftLimitPx, upperLimitPx + 10, width, height));
                }
            }

            string outputTextureFilePath = @"C:\temp\texture.bmp";
            texture.Save(outputTextureFilePath, System.Drawing.Imaging.ImageFormat.Bmp);
        }

        private static void CreateCubeObjFile()
        {
            int[,] vertices = new int[,]
            {
                {0, 0, 0},
                {1, 0, 0},
                {0, 1, 0},
                {0, 0, 1},
                {1, 0, 1},
                {0, 1, 1},
                {1, 1, 0},
                {1, 1, 1},
            };
            int nVertices = vertices.GetLength(0);
            int nCoordinates = vertices.GetLength(1);

            float[,] vertexNormals = new float[nVertices, nCoordinates];
            for (int iVertex = 0; iVertex < nVertices; iVertex++)
            {
                for (int iCoordinate = 0; iCoordinate < nCoordinates; iCoordinate++)
                {
                    int vertexValue = vertices[iVertex, iCoordinate];
                    float normalValue = Convert.ToSingle(vertexValue) - 0.5f; // For cube centered at (0.5, 0.5, 0.5);
                    //float normalValue = 0.5f - Convert.ToSingle(vertexValue); // For cube centered at (0.5, 0.5, 0.5);
                    vertexNormals[iVertex, iCoordinate] = normalValue;
                }
            }

            int[,] faces = new int[,]
            {
                { 2, 1, 7 },
                { 3, 7, 1 },
                { 5, 2, 8 },
                { 7, 8, 2 },
                { 7, 3, 8 },
                { 6, 8, 3 },
                { 1, 4, 3 },
                { 6, 3, 4 },
                { 5, 4, 2 },
                { 1, 2, 4 },
                { 8, 6, 5 },
                { 4, 5, 6 },
            };
            int nFaces = faces.GetLength(0);
            int nFaceVertices = faces.GetLength(1);

            Tuple<float, float>[,] textureCoordinates = new Tuple<float, float>[nFaces, nFaceVertices];
            for (int iFace = 0; iFace < nFaces; iFace++)
            {
                // x is width, then y is height. Origin
                //Tuple<float, float> upperLeftInTexture = Tuple.Create(iFace * Convert.ToSingle(Construction.CubeEdgeLengthPx) / Construction.TextureWidth, 1f);
                ////Tuple<float, float> upperRightInTexture = Tuple.Create((iFace + 1) * Convert.ToSingle(Construction.CubeEdgeLengthPx - 1) / Construction.TextureWidth);
                //Tuple<float, float> lowerRightInTexture = Tuple.Create((iFace + 1) * Convert.ToSingle(Construction.CubeEdgeLengthPx - 1) / Construction.TextureWidth, 0f);
                //Tuple<float, float> lowerLeftInTexture = Tuple.Create(Convert.ToSingle(Construction.CubeEdgeLengthPx - 1) / Construction.TextureHeight, 0f);

                //textureCoordinates[iFace, 0] = upperLeftInTexture;
                ////textureCoordinates[iFace, 1] = upperRightInTexture;
                //textureCoordinates[iFace, 1] = lowerRightInTexture;
                //textureCoordinates[iFace, 2] = lowerLeftInTexture;

                //// Texture image origin at lower-left!
                //var upperLeft = Tuple.Create(iFace * Convert.ToSingle(Construction.CubeEdgeLengthPx) / Construction.TextureWidth, 1f);
                //var upperRight = Tuple.Create((iFace * Convert.ToSingle(Construction.CubeEdgeLengthPx) + Construction.CubeEdgeLengthPx / 2) / Construction.TextureWidth, 1f);
                //var lowerLeft = Tuple.Create(upperLeft.Item1, 0f);
                //textureCoordinates[iFace, 0] = upperLeft;
                //textureCoordinates[iFace, 1] = upperRight;
                //textureCoordinates[iFace, 2] = lowerLeft;

                var upperRight = Tuple.Create((iFace + 1) * Convert.ToSingle(Construction.CubeEdgeLengthPx) / Construction.TextureWidth, 1f);
                var upperLeft = Tuple.Create(iFace * Convert.ToSingle(Construction.CubeEdgeLengthPx) / Construction.TextureWidth, 1f);
                var lowerRight = Tuple.Create(upperRight.Item1, 0f);

                textureCoordinates[iFace, 0] = upperRight;
                textureCoordinates[iFace, 1] = upperLeft;
                textureCoordinates[iFace, 2] = lowerRight;
            }

            string outputObjFilePath = @"C:\temp\temp.obj";
            using (StreamWriter writer = new StreamWriter(outputObjFilePath))
            {
                writer.WriteLine($@"mtllib {Construction.CubeMaterialFileName}");

                StringBuilder builder = new StringBuilder();

                // Vertices.
                for (int iVertex = 0; iVertex < nVertices; iVertex++)
                {
                    builder.Clear();
                    builder.Append(@"v");

                    for (int iCoordinate = 0; iCoordinate < nCoordinates; iCoordinate++)
                    {
                        string appendix = $@" {Convert.ToSingle(vertices[iVertex, iCoordinate]).ToString()}"; // Include prefix space.
                        builder.Append(appendix);
                    }

                    string line = builder.ToString();
                    writer.WriteLine(line);
                }

                // Vertex normals.
                for (int iVertex = 0; iVertex < nVertices; iVertex++)
                {
                    builder.Clear();
                    builder.Append(@"vn");

                    for (int iCoordinate = 0; iCoordinate < nCoordinates; iCoordinate++)
                    {
                        string appendix = $@" {vertexNormals[iVertex, iCoordinate].ToString()}"; // Include prefix space.
                        builder.Append(appendix);
                    }

                    string line = builder.ToString();
                    writer.WriteLine(line);
                }

                // Vertex texture coordinates.
                for (int iFace = 0; iFace < nFaces; iFace++)
                {
                    for (int iFaceVertex = 0; iFaceVertex < nFaceVertices; iFaceVertex++)
                    {
                        Tuple<float, float> textureCoordinate = textureCoordinates[iFace, iFaceVertex];

                        string line = $@"vt {textureCoordinate.Item1.ToString()} {textureCoordinate.Item2.ToString()}"; // U is horizontal, V is vertical.
                        writer.WriteLine(line);
                    }
                }

                // Faces.
                for (int iFace = 0; iFace < nFaces; iFace++)
                {
                    builder.Clear();
                    builder.Append(@"f");

                    for (int iVertex = 0; iVertex < nFaceVertices; iVertex++)
                    {
                        int vertexIndex = faces[iFace, iVertex];
                        int textureCoordinateIndex = iFace * nFaceVertices + iVertex + 1; // Texture coordinates are listed in the same face-major order. +1 since numbering starts at 1.
                        int vertexNormalIndex = vertexIndex; // Normals were written in the same order as the vertices.

                        string v = vertexIndex.ToString();
                        string vt = textureCoordinateIndex.ToString();
                        string vn = vertexNormalIndex.ToString();

                        string appendix = $@" {v}/{vt}/{vn}"; // Include prefix space.
                        builder.Append(appendix);
                    }

                    string line = builder.ToString();
                    writer.WriteLine(line);
                }
            }
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
            Location3HomogenousFloat[] vertices = new Location3HomogenousFloat[nVertices];
            for (int iVertexIndex = 0; iVertexIndex < nVertices; iVertexIndex++)
            {
                int vertexIndex = vertexIndices[iVertexIndex];

                Location3HomogenousFloat vertex = Construction.GetPlyFileVertex(plyFile, vertexIndex);
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
            Location3Double[] translations = new Location3Double[nImageIndices];
            for (int iImageIndex = 0; iImageIndex < nImageIndices; iImageIndex++)
            {
                int imageIndex = imageIndices[iImageIndex];

                Camera camera = nvmFile.Cameras[imageIndex];

                string fileName = camera.FileName;
                string filePath = Path.Combine(imagesDirectoryPath, fileName);
                imageFilePaths[iImageIndex] = filePath;

                MatrixDouble rotation = QuaternionDouble.GetRotationMatrix(camera.Rotation);
                rotations[iImageIndex] = rotation;

                Location3Double translation = Construction.GetTranslation(rotation, camera.Location);
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
                Location3HomogenousFloat vertex = vertices[iVertex];
                Vector<double> vertexHomogenous = DenseVector.Build.DenseOfArray(new double[] { vertex.X, vertex.Y, vertex.Z, vertex.H });
                vertexHomogenousVectors[iVertex] = vertexHomogenous;
            }

            Matrix<double> verticesHomogenous = DenseMatrix.Build.DenseOfColumns(vertexHomogenousVectors);

            // Determine image locations for each image and vertex.
            Location2Double[][] imageLocations = new Location2Double[nImageIndices][];
            for (int iImage = 0; iImage < nImageIndices; iImage++)
            {
                Matrix<double> cameraMatrix = cameraMatrices[iImage];

                var unNormalizedHomogenous2D = cameraMatrix * verticesHomogenous;

                var xVector = unNormalizedHomogenous2D.Row(0) / unNormalizedHomogenous2D.Row(2);
                var yVector = unNormalizedHomogenous2D.Row(1) / unNormalizedHomogenous2D.Row(2);

                Location2Double[] locations = new Location2Double[nVertices];
                imageLocations[iImage] = locations;
                for (int iVertex = 0; iVertex < nVertices; iVertex++)
                {
                    var location2D = new Location2Double(xVector[iVertex], yVector[iVertex]);
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

                Location2Double[] locations = imageLocations[iImage];

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
            Location3HomogenousFloat point3d = Construction.GetPlyFileVertex(plyFile, vertexIndexOfInterest);

            Patch patch = patchFile.Patches.Where((x) => Convert.ToSingle(x.Location.X) == point3d.X && Convert.ToSingle(x.Location.Y) == point3d.Y && Convert.ToSingle(x.Location.Z) == point3d.Z).FirstOrDefault();

            int[] imageIndices = patch.ImageIndicesWithGoodAgreement;
            int nImageIndices = imageIndices.Length;
            string[] imageFilePaths = new string[nImageIndices];
            double[] focalLengths = new double[nImageIndices];
            MatrixDouble[] rotations = new MatrixDouble[nImageIndices];
            Location3Double[] translations = new Location3Double[nImageIndices];
            for (int iImageIndex = 0; iImageIndex < nImageIndices; iImageIndex++)
            {
                int imageIndex = imageIndices[iImageIndex]; // Assumes patch file image indices start at zero.

                Camera camera = nvmFile.Cameras[imageIndex];

                string fileName = camera.FileName;
                string filePath = Path.Combine(imagesDirectoryPath, fileName);
                imageFilePaths[iImageIndex] = filePath;

                MatrixDouble rotation = QuaternionDouble.GetRotationMatrix(camera.Rotation);
                rotations[iImageIndex] = rotation;

                Location3Double translation = Construction.GetTranslation(rotation, camera.Location);
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

            Location2Double[] imageLocations = new Location2Double[nImageIndices];
            for (int iImage = 0; iImage < nImageIndices; iImage++)
            {
                var cameraMatrix = cameraMatrices[iImage];

                var nonNormalizedHomogenous2D = cameraMatrix * homogenousPoint;

                double x = nonNormalizedHomogenous2D[0] / nonNormalizedHomogenous2D[2];
                double y = nonNormalizedHomogenous2D[1] / nonNormalizedHomogenous2D[2];

                Location2Double imageLocation = new Location2Double(x, y);
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

        private static Location3Double GetTranslation(MatrixDouble rotation, Location3Double cameraLocation)
        {
            double[] values = rotation.RowMajorValues;

            double x = -(values[0] * cameraLocation.X + values[1] * cameraLocation.Y + values[2] * cameraLocation.Z);
            double y = -(values[3] * cameraLocation.X + values[4] * cameraLocation.Y + values[5] * cameraLocation.Z);
            double z = -(values[6] * cameraLocation.X + values[7] * cameraLocation.Y + values[8] * cameraLocation.Z);

            Location3Double output = new Location3Double(x, y, z);
            return output;
        }

        private static Location3HomogenousFloat GetPlyFileVertex(PlyFile plyFile, int index)
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

            Location3HomogenousFloat output = new Location3HomogenousFloat(xValue, yValue, zValue, 1);
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

            PatchFile patchCollection = PatchCollectionV1Serializer.Deserialize(exampleFilePath);
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
            if (!textFilesEqual)
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

            double[] arr = new double[0];
            double value = arr[1]; // System.IndexOutOfRangeException
        }
    }
}
