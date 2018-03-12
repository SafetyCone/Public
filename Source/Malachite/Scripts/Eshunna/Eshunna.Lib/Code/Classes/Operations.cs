using System;
using System.Collections.Generic;
using System.Drawing;
using SysImageFormat = System.Drawing.Imaging.ImageFormat;
using System.IO;
using System.Linq;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

using Public.Common.Lib.Extensions;

using Eshunna.Lib.NVM;
using Eshunna.Lib.Patches;
using Eshunna.Lib.PLY;


namespace Eshunna.Lib
{
    public static class Operations
    {
        public static Matrix<double> CameraMatrix(NViewMatch nvm, int imageIndex)
        {
            var camera = nvm.Cameras[imageIndex];

            var output = Operations.CameraMatrix(camera);
            return output;
        }

        public static Matrix<double> CameraMatrix(Camera camera)
        {
            double focalLength = camera.FocalLength;
            MatrixDouble rotation = QuaternionDouble.GetRotationMatrix(camera.Rotation);
            Location3Double translation = Operations.TranslationGet(rotation, camera.Location);

            var K = DenseMatrix.Build.DenseIdentity(3, 3);
            K[0, 0] = focalLength;
            K[1, 1] = focalLength;

            var R = DenseMatrix.Build.DenseOfRowMajor(3, 3, rotation.RowMajorValues);
            var T = DenseVector.Build.DenseOfArray(new double[] { translation.X, translation.Y, translation.Z });
            var P = DenseMatrix.Build.DenseOfColumns(new Vector<double>[] { R.Column(0), R.Column(1), R.Column(2), T });

            var cameraMatrix = K * P;
            return cameraMatrix;
        }

        public static Vector3Double CameraLocation(NViewMatch nvm, int cameraIndex)
        {
            var output = nvm.Cameras[cameraIndex].Location.ToVector3Double();
            return output;
        }

        public static void DrawPoints(IEnumerable<Vector2Double> points, string filePath)
        {
            Operations.DrawPoints(points, filePath, SysImageFormat.Jpeg); // Dummy format.
        }

        public static void DrawPoints(IEnumerable<Vector2Double> points, string filePath, SysImageFormat imageFormat, bool useDefault = true)
        {
            var boundingBox = points.GetBoundingBox();
            var boundingBoxInt = boundingBox.ToBoundingBoxInteger();
            var rectangle = boundingBoxInt.RectangleXWidthGet();
            var bitmap = new Bitmap(rectangle.Width, rectangle.Height);
            foreach (var point in points)
            {
                bitmap.SetPixel(Convert.ToInt32(Math.Round(point.X)) - boundingBoxInt.XMin, Convert.ToInt32(Math.Round(point.Y)) - boundingBoxInt.YMin, System.Drawing.Color.Green);
            }

            if (useDefault)
            {
                bitmap.Save(filePath);
            }
            else
            {
                bitmap.Save(filePath, imageFormat);
            }
        }

        public static List<Vector3Double> FacetVertices(StructureModel structureModel, int facetIndex)
        {
            var facet = structureModel.Facets[facetIndex];
            var output = Operations.FacetVertices(structureModel, facet);
            return output;
        }

        public static List<Vector3Double> FacetVertices(StructureModel structureModel, Facet facet)
        {
            var output = new List<Vector3Double>
            {
                structureModel.Vertices[facet.Vertex1Index].ToVector3Double(),
                structureModel.Vertices[facet.Vertex2Index].ToVector3Double(),
                structureModel.Vertices[facet.Vertex3Index].ToVector3Double(),
            };
            return output;
        }

        public static Location3Double FacetCentroid(StructureModel structureModel, int facetIndex)
        {
            var facet = structureModel.Facets[facetIndex];

            var output = Operations.FacetCentroid(structureModel, facet);
            return output;
        }

        public static Location3Double FacetCentroid(StructureModel structureModel, Facet facet)
        {
            var vertex1 = structureModel.Vertices[facet.Vertex1Index];
            var vertex2 = structureModel.Vertices[facet.Vertex2Index];
            var vertex3 = structureModel.Vertices[facet.Vertex3Index];

            var output = Operations.FacetCentroid(vertex1, vertex2, vertex3);
            return output;
        }

        public static Location3Double FacetCentroid(Location3Double vertex1, Location3Double vertex2, Location3Double vertex3)
        {
            double x = (vertex1.X + vertex2.X + vertex3.X) / 3;
            double y = (vertex1.Y + vertex2.Y + vertex3.Y) / 3;
            double z = (vertex1.Z + vertex2.Z + vertex3.Z) / 3;

            var output = new Location3Double(x, y, z);
            return output;
        }

        public static Vector3Double FacetCentroidVector(StructureModel structureModel, int facetIndex)
        {
            var centroidLocation = Operations.FacetCentroid(structureModel, facetIndex);
            var output = centroidLocation.ToVector3Double();
            return output;
        }

        public static Vector3Double FacetCentroidVector(StructureModel structureModel, Facet facet)
        {
            var centroidLocation = Operations.FacetCentroid(structureModel, facet);
            var output = centroidLocation.ToVector3Double();
            return output;
        }

        /// <summary>
        /// Uses the cross-product of the vectors between the facet vertices.
        /// </summary>
        public static Vector3Double FacetNormal(StructureModel structureModel, int facetIndex)
        {
            var facet = structureModel.Facets[facetIndex];

            var vertex1 = structureModel.Vertices[facet.Vertex1Index];
            var vertex2 = structureModel.Vertices[facet.Vertex2Index];
            var vertex3 = structureModel.Vertices[facet.Vertex3Index];

            var output = Operations.FacetNormal(vertex1, vertex2, vertex3);
            return output;
        }

        /// <summary>
        /// Computes the cross-product: (v3 - v1) x (v2 - v1). This direction results in normals that are well aligned with the average of the vertex patch normals.
        /// </summary> o
        public static Vector3Double FacetNormal(Location3Double vertex1, Location3Double vertex2, Location3Double vertex3)
        {
            var vextex1Vector3Double = vertex1.ToVector3Double();
            var v1ToV2 = vertex2.ToVector3Double() - vextex1Vector3Double;
            var v1ToV3 = vertex3.ToVector3Double() - vextex1Vector3Double;

            var crossProduct = Vector3Double.CrossProduct(v1ToV3, v1ToV2);
            var output = crossProduct.L2Normalize();
            return output;
        }

        /// <summary>
        /// Uses the average of the normals of the three facet vertex patches.
        /// </summary>
        public static Vector3Double FacetNormal(StructureModel structureModel, int facetIndex, PatchFile patchFile)
        {
            var facet = structureModel.Facets[facetIndex];

            var p1 = patchFile.Patches[facet.Vertex1Index];
            var p2 = patchFile.Patches[facet.Vertex2Index];
            var p3 = patchFile.Patches[facet.Vertex3Index];

            var output = Operations.FacetNormal(p1, p2, p3);
            return output;
        }

        public static Vector3Double FacetNormal(Patch p1, Patch p2, Patch p3)
        {
            // Make sure the normals are actually normalized.
            var normalV1 = p1.Normal.L2Normalize();
            var normalV2 = p2.Normal.L2Normalize();
            var normalV3 = p3.Normal.L2Normalize();

            double x = (normalV1.X + normalV2.X + normalV3.X) / 3;
            double y = (normalV1.Y + normalV2.Y + normalV3.Y) / 3;
            double z = (normalV1.Z + normalV2.Z + normalV3.Z) / 3;

            var average = new Vector3Double(x, y, z);
            var output = average.L2Normalize(); // Normalize the normal.
            return output;
        }

        public static string ImageFilePathGet(string imageDirectorypath, NViewMatch nvm, int imageIndex)
        {
            string fileName = nvm.Cameras[imageIndex].FileName;
            string filePath = Path.Combine(imageDirectorypath, fileName);
            return filePath;
        }

        public static Matrix<double> HomogenousToInhomogenousColumnVectors(Matrix<double> homogenous)
        {
            var output = DenseMatrix.Build.DenseOfMatrix(homogenous);

            int lastRowIndex = homogenous.RowCount - 1;
            output.RemoveRow(lastRowIndex);
            return output;
        }

        public static Matrix<double> HomogenousVectorArrayCreate(IList<Vector3Double> vectors)
        {
            int nVectors = vectors.Count;
            var output = DenseMatrix.Build.Dense(4, nVectors);
            for (int iVector = 0; iVector < nVectors; iVector++)
            {
                var vector = vectors[iVector];
                output[0, iVector] = vector.X;
                output[1, iVector] = vector.Y;
                output[2, iVector] = vector.Z;
                output[3, iVector] = 1;
            }
            return output;
        }

        public static Matrix<double> HomogenousVectorArrayCreate(params Vector3Double[] vectors)
        {
            var output = Operations.HomogenousVectorArrayCreate(vectors as IList<Vector3Double>);
            return output;
        }

        public static int[] ImageIndicesForFacet(StructureModel structureModel, int facetIndex, PatchFile patchFile)
        {
            var vertices = Operations.FacetVertices(structureModel, facetIndex);

            var patches = Operations.PatchesClosest(patchFile, vertices);

            var output = Operations.ImageIndicesWithGoodAgreement(patches);
            return output;
        }

        public static int[] ImageIndicesWithGoodAgreement(IEnumerable<Patch> patches)
        {
            HashSet<int> imageIndices = new HashSet<int>();
            foreach (var patch in patches)
            {
                patch.ImageIndicesWithGoodAgreement.ForEach((x) => imageIndices.Add(x));
            }

            List<int> indicesInOrder = new List<int>(imageIndices);
            indicesInOrder.Sort();

            var output = indicesInOrder.ToArray();
            return output;
        }

        public static List<Location2Double> ImageLocationsGet(Matrix<double> cameraMatrix, Matrix<double> homogenousPointLocations)
        {
            var unNormalizedHomogenous2D = cameraMatrix * homogenousPointLocations;

            var xVector = unNormalizedHomogenous2D.Row(0) / unNormalizedHomogenous2D.Row(2);
            var yVector = unNormalizedHomogenous2D.Row(1) / unNormalizedHomogenous2D.Row(2);

            int nVertices = unNormalizedHomogenous2D.ColumnCount;
            var output = new List<Location2Double>(nVertices);
            for (int iVertex = 0; iVertex < nVertices; iVertex++)
            {
                var location2D = new Location2Double(xVector[iVertex], yVector[iVertex]);
                output.Add(location2D);
            }
            return output;
        }

        public static Matrix<double> InhomogenousVectorArrayCreate(IList<Vector3Double> vectors)
        {
            int nVectors = vectors.Count;
            var output = DenseMatrix.Build.Dense(3, nVectors);
            for (int iVector = 0; iVector < nVectors; iVector++)
            {
                var vector = vectors[iVector];
                output[0, iVector] = vector.X;
                output[1, iVector] = vector.Y;
                output[2, iVector] = vector.Z;
            }
            return output;
        }

        public static Matrix<double> InhomogenousVectorArrayCreate(params Vector3Double[] vectors)
        {
            var output = Operations.InhomogenousVectorArrayCreate(vectors as IList<Vector3Double>);
            return output;
        }

        public static List<Location2Double> Location2Double(Matrix<double> columnVectors)
        {
            int nLocations = columnVectors.ColumnCount;
            var locations = new List<Location2Double>(nLocations);
            for (int iLocation = 0; iLocation < nLocations; iLocation++)
            {
                var location = new Location2Double(columnVectors[0, iLocation], columnVectors[1, iLocation]);
                locations.Add(location);
            }
            return locations;
        }

        public static Tuple<Bitmap, List<Location2Integer>> MiniImageOfTriangle(Bitmap image, IList<Location2Integer> vertexImagePixelLocations)
        {
            var miniImageBoundingBox = vertexImagePixelLocations.BoundingBoxGet();
            var miniImageRectangle = miniImageBoundingBox.RectangleXWidthGet();
            var miniImage = new Bitmap(miniImageRectangle.Width, miniImageRectangle.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            var vertexMiniImagePixelLocations = Operations.OriginTranslate(vertexImagePixelLocations, miniImageBoundingBox.XMin, miniImageBoundingBox.YMin);

            // Color the mini-image.
            bool allPixelsInImage = Operations.PixelLocationsInImage(vertexImagePixelLocations, image.Width, image.Height);
            if (allPixelsInImage)
            {
                var pixelsInImage = Geometry.ListTriangleLocations(vertexImagePixelLocations[0], vertexImagePixelLocations[1], vertexImagePixelLocations[2]);
                var pixelsInMiniImage = Geometry.ListTriangleLocations(vertexMiniImagePixelLocations[0], vertexMiniImagePixelLocations[1], vertexMiniImagePixelLocations[2]);
                int nPixels = pixelsInImage.Count;
                for (int iPixel = 0; iPixel < nPixels; iPixel++)
                {
                    var pixelInImage = pixelsInImage[iPixel];
                    var color = image.GetPixel(pixelInImage.X, pixelInImage.Y);
                    var pixelInMiniImage = pixelsInMiniImage[iPixel];
                    miniImage.SetPixel(pixelInMiniImage.X, pixelInMiniImage.Y, color);
                }
            }
            else
            {
                // No nothing.
            }

            var output = Tuple.Create(miniImage, vertexMiniImagePixelLocations);
            return output;
        }

        public static List<Location2Double> OriginTranslateCenteredToUpperLeft(IEnumerable<Location2Double> imagePixelLocations, int imageWidth, int imageHeight)
        {
            int halfWidth = imageWidth / 2;
            int halfHeight = imageHeight / 2;

            var output = new List<Location2Double>();
            foreach (var imagePixelLocation in imagePixelLocations)
            {
                var upperLeftRelativeLocation = new Location2Double(imagePixelLocation.X + halfWidth, imagePixelLocation.Y + halfHeight);
                output.Add(upperLeftRelativeLocation);
            }
            return output;
        }

        public static List<Location2Float> OriginTranslateCenteredToUpperLeft(IEnumerable<Location2Float> imagePixelLocations, int imageWidth, int imageHeight)
        {
            int halfWidth = imageWidth / 2;
            int halfHeight = imageHeight / 2;

            var output = new List<Location2Float>();
            foreach (var imagePixelLocation in imagePixelLocations)
            {
                var upperLeftRelativeLocation = new Location2Float(imagePixelLocation.X + halfWidth, imagePixelLocation.Y + halfHeight);
                output.Add(upperLeftRelativeLocation);
            }
            return output;
        }

        public static List<Location2Integer> OriginTranslateCenteredToUpperLeft(IEnumerable<Location2Integer> imagePixelLocations, int imageWidth, int imageHeight)
        {
            int halfWidth = imageWidth / 2;
            int halfHeight = imageHeight / 2;

            var output = new List<Location2Integer>();
            foreach (var imagePixelLocation in imagePixelLocations)
            {
                var upperLeftRelativeLocation = new Location2Integer(imagePixelLocation.X + halfWidth, imagePixelLocation.Y + halfHeight);
                output.Add(upperLeftRelativeLocation);
            }
            return output;
        }

        public static List<Location2Integer> OriginTranslate(IEnumerable<Location2Integer> locations, int originX, int originY)
        {
            var origin = new Location2Integer(originX, originY);
            var output = Operations.OriginTranslate(locations, origin);
            return output;
        }

        public static List<Location2Integer> OriginTranslate(IEnumerable<Location2Integer> locations, Location2Integer origin)
        {
            var output = new List<Location2Integer>();
            foreach (var location in locations)
            {
                var translatedLocation = location - origin;
                output.Add(translatedLocation);
            }
            return output;
        }

        public static Matrix<double> ProductHomogenousNormalizeColumnVectors(Matrix<double> m1, Matrix<double> m2)
        {
            var output = m1 * m2;

            int homogenousRowIndex = output.RowCount - 1;
            int maxNonHomogenousRow = homogenousRowIndex;
            for (int iRow = 0; iRow < maxNonHomogenousRow; iRow++)
            {
                var row = output.Row(iRow) / output.Row(homogenousRowIndex);
                output.SetRow(iRow, row);
            }

            return output;
        }

        public static List<Patch> PatchesClosest(PatchFile patchFile, IEnumerable<Vector3Double> vertices)
        {
            var output = new List<Patch>();
            foreach (var vertex in vertices)
            {
                var vertexPatch = Operations.PatchClosest(patchFile, vertex);
                output.Add(vertexPatch);
            }
            return output;
        }

        public static Patch PatchClosest(PatchFile patchFile, Vector3Double vertex)
        {
            double minDiff = Double.MaxValue;
            int index = 0;
            int minIndex = -1;
            foreach (var patch in patchFile.Patches)
            {
                double diffX = patch.Location.X - vertex.X;
                double diffY = patch.Location.Y - vertex.Y;
                double diffZ = patch.Location.Z - vertex.Z;

                double diff = diffX * diffX + diffY * diffY + diffZ * diffZ;
                if (diff < minDiff)
                {
                    minDiff = diff;
                    minIndex = index;
                }
                index++;
            }

            var output = patchFile.Patches[minIndex];
            return output;
        }

        public static Vector3Double PatchNormal(PatchFile patchFile, int patchIndex)
        {
            var patch = patchFile.Patches[patchIndex];

            var output = Operations.PatchNormal(patch);
            return output;
        }

        public static Vector3Double PatchNormal(Patch patch)
        {
            var output = patch.Location.ToVector3Double();
            return output;
        }

        public static bool PixelLocationsInImage(IEnumerable<Location2Integer> locations, int imageWidth, int imageHeight)
        {
            bool output = true;
            foreach (var location in locations)
            {
                if(location.X >= imageWidth || location.X < 0 || location.Y >= imageHeight || location.Y < 0)
                {
                    output = false;
                    break;
                }
            }

            return output;
        }

        public static Vector3Double PointToCameraVector(NViewMatch nvm, int cameraIndex, Vector3Double point)
        {
            var cameraLocation = Operations.CameraLocation(nvm, cameraIndex);
            var output = cameraLocation - point;
            return output;
        }

        public static Vector3Double PointToCameraVectorUnit(NViewMatch nvm, int cameraIndex, Vector3Double point)
        {
            var pointToCamera = Operations.PointToCameraVector(nvm, cameraIndex, point);
            var output = pointToCamera.L2Normalize();
            return output;
        }

        /// <summary>
        /// Creates a rectangle capable of containing all provided bounding boxes, arranged horizontally. The output rectangle is assumed be its own objects, thus has X = 0, and Y = 0.
        /// </summary>
        public static RectangleInteger RectangleHorizontal(IEnumerable<BoundingBoxInteger> boundingBoxes)
        {
            int width = 0;
            int maxHeight = Int32.MinValue;
            foreach (var boundingBox in boundingBoxes)
            {
                var rectangle = boundingBox.RectangleXWidthGet();

                width += (rectangle.Width);

                if (rectangle.Height > maxHeight)
                {
                    maxHeight = rectangle.Height;
                }
            }

            var output = new RectangleInteger(0, 0, width, maxHeight);
            return output;
        }

        public static StructureModel StructureModelBuild(PlyFile plyFile)
        {
            Operations.CheckPlyFileForGetStructureModel(plyFile, out float[] xVertexValues, out float[] yVertexValues, out float[] zVertexValues, out int[][] faceVertexValues);

            int nVertices = xVertexValues.Length;
            int nFacets = faceVertexValues.Length;

            var output = new StructureModel(nVertices, nFacets);

            for (int iVertex = 0; iVertex < nVertices; iVertex++)
            {
                double x = Convert.ToDouble(xVertexValues[iVertex]);
                double y = Convert.ToDouble(yVertexValues[iVertex]);
                double z = Convert.ToDouble(zVertexValues[iVertex]);

                var vertex = new Location3Double(x, y, z);
                output.Vertices.Add(vertex);
            }

            for (int iFacet = 0; iFacet < nFacets; iFacet++)
            {
                int[] facetVertexIndices = faceVertexValues[iFacet];

                int vertex1Index = facetVertexIndices[0];
                int vertex2Index = facetVertexIndices[1];
                int vertex3Index = facetVertexIndices[2];

                var facet = new Facet(vertex1Index, vertex2Index, vertex3Index);
                output.Facets.Add(facet);
            }

            return output;
        }

        /// <summary>
        /// Ensure that vertices have x, y, and z location values, and that our facets are triangular.
        /// </summary>
        private static void CheckPlyFileForGetStructureModel(PlyFile plyFile, out float[] xVertexValues, out float[] yVertexValues, out float[] zVertexValues, out int[][] faceVertexIndices)
        {
            // Do we have vertices?
            string vertexElementName = PlyFile.vertexElementName;
            if (!plyFile.Values.ContainsKey(vertexElementName))
            {
                throw new ArgumentException($@"PLY file did not contain vertices. Could not find element '{vertexElementName}'.", nameof(plyFile));
            }
            var vertexValues = plyFile.Values[vertexElementName];

            // Do our vertices have the right properties?
            string xVertexPropertyName = @"x";
            if (!vertexValues.ContainsKey(xVertexPropertyName))
            {
                throw new ArgumentException($@"PLY file vertices missing property: '{xVertexPropertyName}'.", nameof(plyFile));
            }
            xVertexValues = (float[])vertexValues[xVertexPropertyName];

            string yVertexPropertyName = @"y";
            if (!vertexValues.ContainsKey(yVertexPropertyName))
            {
                throw new ArgumentException($@"PLY file vertices missing property: '{yVertexPropertyName}'.", nameof(plyFile));
            }
            yVertexValues = (float[])vertexValues[yVertexPropertyName];

            string zVertexPropertyName = @"z";
            if (!vertexValues.ContainsKey(zVertexPropertyName))
            {
                throw new ArgumentException($@"PLY file vertices missing property: '{zVertexPropertyName}'.", nameof(plyFile));
            }
            zVertexValues = (float[])vertexValues[zVertexPropertyName];

            // Are our vertex properties all of the same length?
            int nVertices = xVertexValues.Length;
            if (nVertices != yVertexValues.Length)
            {
                throw new ArgumentException($@"Vertex property value mismatch: '{xVertexPropertyName}' - {nVertices.ToString()} values, '{yVertexPropertyName}' - {yVertexValues.Length} values.", nameof(plyFile));
            }
            if (nVertices != zVertexValues.Length)
            {
                throw new ArgumentException($@"Vertex property value mismatch: '{xVertexPropertyName}' - {nVertices.ToString()} values, '{zVertexPropertyName}' - {zVertexValues.Length} values.", nameof(plyFile));
            }

            // Do we have faces?
            string faceElementName = PlyFile.faceElementName;
            if (!plyFile.Values.ContainsKey(faceElementName))
            {
                throw new ArgumentException($@"PLY file did not contain faces. Could not find element '{faceElementName}'.", nameof(plyFile));
            }
            var faceValues = plyFile.Values[faceElementName];

            // Do our faces have the right property?
            string vertex_indicesFacePropertyName = Constants.VertexIndicesPropertyName;
            if (!faceValues.ContainsKey(vertex_indicesFacePropertyName))
            {
                throw new ArgumentException($@"PLY file faces missing property: '{vertex_indicesFacePropertyName}'.", nameof(plyFile));
            }
            faceVertexIndices = (int[][])faceValues[vertex_indicesFacePropertyName];

            // Are all of our faces triangular facets?
            int nFaces = faceVertexIndices.Length;
            if (0 == nFaces)
            {
                throw new ArgumentException(@"Zero faces found in PLY file.", nameof(plyFile));
            }
            for (int iFace = 0; iFace < nFaces; iFace++)
            {
                int[] vertexIndices = faceVertexIndices[iFace];
                if (Facet.NumberOfVertices != vertexIndices.Length)
                {
                    throw new ArgumentException($@"Incorrect number of vertices for triangular facet. Expected: {Facet.NumberOfVertices}, found: {vertexIndices.Length}.", nameof(plyFile));
                }
            }
        }

        public static Location3Double TranslationGet(MatrixDouble rotation, Location3Double cameraLocation)
        {
            double[] values = rotation.RowMajorValues;

            double x = -(values[0] * cameraLocation.X + values[1] * cameraLocation.Y + values[2] * cameraLocation.Z);
            double y = -(values[3] * cameraLocation.X + values[4] * cameraLocation.Y + values[5] * cameraLocation.Z);
            double z = -(values[6] * cameraLocation.X + values[7] * cameraLocation.Y + values[8] * cameraLocation.Z);

            Location3Double output = new Location3Double(x, y, z);
            return output;
        }

        public static List<Tuple<int, int>> Tuple2Integer(IEnumerable<Location2Integer> locations)
        {
            var output = new List<Tuple<int, int>>();
            foreach (var location in locations)
            {
                var tuple = Tuple.Create(location.X, location.Y);
                output.Add(tuple);
            }
            return output;
        }

        public static List<Location2Double> UVMapPixels(IEnumerable<Location2Integer> locations, int uvImageWidth, int uvImageHeight)
        {
            double width = Convert.ToDouble(uvImageWidth);
            double height = Convert.ToDouble(uvImageHeight);

            var output = new List<Location2Double>();
            foreach (var location in locations)
            {
                double x = Convert.ToDouble(location.X) / width;
                double y = Convert.ToDouble(location.Y) / height;
                double yInverted = 1 - y; // MeshLab assumes the origin is in the lower-left of the image and Y increases upwards (this is the opposite of the image convention).
                var uvImageLocation = new Location2Double(x, yInverted);
                output.Add(uvImageLocation);
            }
            return output;
        }

        public static List<Vector3Double> Vector3ArrayCreate(Matrix<double> columnVectors)
        {
            int nVectors = columnVectors.ColumnCount;
            var output = new List<Vector3Double>(nVectors);
            for (int iVector = 0; iVector < nVectors; iVector++)
            {
                var vector = new Vector3Double(columnVectors[0, iVector], columnVectors[1, iVector], columnVectors[2, iVector]);
                output.Add(vector);
            }
            return output;
        }

        public static List<Vector2Double> Vector2ArrayCreate(Matrix<double> columnVectors)
        {
            int nVectors = columnVectors.ColumnCount;
            var output = new List<Vector2Double>(nVectors);
            for (int iVector = 0; iVector < nVectors; iVector++)
            {
                var vector = new Vector2Double(columnVectors[0, iVector], columnVectors[1, iVector]);
                output.Add(vector);
            }
            return output;
        }
    }
}
