using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using SysImageFormat = System.Drawing.Imaging.ImageFormat;
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
using Eshunna.Lib.OBJ;
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
            //Construction.DetermineMostFrontalImage();
            //Construction.DetermineAllPatchImageIndices();
            //Construction.CreateStructureModel();
            //Construction.CompareVectorNormals();
            //Construction.TestPointInTriangleInteger();
            //Construction.TestPointInTriangleDouble();
            //Construction.CutFacetFromAllImages();
            //Construction.RotateFacetFullyTowardsCamera();
            //Construction.CompareFacetNormals();
            //Construction.CreateSingleTriangleObjFile();
            Construction.CreateObjFile();
        }

        private static void CreateObjFileWithDifferentImages()
        {
            // Load data files.
            var properties = Program.GetProjectProperties();
            //string imagesDirectoryPath = properties[EshunnaProperties.ImagesDirectoryPathPropertyName];
            //string imagesOriginalHdDirectoryPath = properties[EshunnaProperties.ImagesOriginalHdDirectoryPathPropertyName];
            string imagesDirectoryPath = properties[EshunnaProperties.ImagesOriginalHdDirectoryPathPropertyName];
            string nvmFilePath = properties[EshunnaProperties.ExampleNvmFilePathPropertyName];
            string patchFilePath = properties[EshunnaProperties.ExamplePatchFilePathPropertyName];
            string meshedPlyFilePath = properties[EshunnaProperties.ExampleMeshPoissonSimplePlyFilePathPropertyName];

            var nvm = NvmV3Serializer.Deserialize(nvmFilePath);
            var patchFile = PatchFileV1Serializer.Deserialize(patchFilePath);
            var plyFile = PlyV1TextSerializer.Deserialize(meshedPlyFilePath);

            // Create the structure model data structure.
            var structureModel = Operations.StructureModelBuild(plyFile);

            // Create OBJ required file paths.
            string outputDirectoryPath = @"C:\temp";
            string objFileNameWithoutExtension = @"mesh_hd";
            string objFileName = $@"{objFileNameWithoutExtension}.obj";
            string objFilePath = Path.Combine(outputDirectoryPath, objFileName);
            string materialFileName = $@"{objFileNameWithoutExtension}.mtl";
            string materialFilePath = Path.Combine(outputDirectoryPath, materialFileName);
            string materialFileRelativePathFromObjFile = materialFileName;
            string textureFileName = $@"{objFileNameWithoutExtension}.bmp"; // No spaces in relative path from OBJ file to 
            string textureFilePath = Path.Combine(outputDirectoryPath, textureFileName);
            string textureFileRelativePathFromMaterialFile = textureFileName;

            // Create the vertices in the same order as given by the structure model.
            int nVertices = structureModel.Vertices.Count;
            List<Vector3Double> vertices = new List<Vector3Double>(nVertices);
            foreach (var vertex in structureModel.Vertices)
            {
                var vertexVector = vertex.ToVector3Double();
                vertices.Add(vertexVector);
            }

            // Create the facets, using the vertex indices in the same order as given by the structure model.
            // The vertex texture UV vector indices will be created in the same order as the facets given by the structure model.
            int nFacets = structureModel.Facets.Count;
            List<ObjFacet> facets = new List<ObjFacet>(nFacets);
            int uvCoordinateVectorIndex = 1; // OBJ file starts at 1.
            foreach (var facet in structureModel.Facets)
            {
                ObjFacetVertex v1 = new ObjFacetVertex(facet.Vertex1Index + 1, ObjFacetVertex.NotSpecifiedValue, uvCoordinateVectorIndex++); // OBJ file starts at 1.
                ObjFacetVertex v2 = new ObjFacetVertex(facet.Vertex2Index + 1, ObjFacetVertex.NotSpecifiedValue, uvCoordinateVectorIndex++); // OBJ file starts at 1.
                ObjFacetVertex v3 = new ObjFacetVertex(facet.Vertex3Index + 1, ObjFacetVertex.NotSpecifiedValue, uvCoordinateVectorIndex++); // OBJ file starts at 1.

                ObjFacet objFacet = new ObjFacet(v1, v2, v3);
                facets.Add(objFacet);
            }
        }

        private static void CreateObjFile()
        {
            // Load data files.
            var properties = Program.GetProjectProperties();
            string sfmImagesDirectoryPath = properties[EshunnaProperties.ImagesDirectoryPathPropertyName];
            string wedgeImagesDirectoryPath = properties[EshunnaProperties.ImagesDirectoryPathPropertyName];
            //string wedgeImagesDirectoryPath = properties[EshunnaProperties.ImagesOriginalHdDirectoryPathPropertyName];
            string nvmFilePath = properties[EshunnaProperties.ExampleNvmFilePathPropertyName];
            string patchFilePath = properties[EshunnaProperties.ExamplePatchFilePathPropertyName];
            string meshedPlyFilePath = properties[EshunnaProperties.ExampleMeshPoissonSimplePlyFilePathPropertyName];

            var nvm = NvmV3Serializer.Deserialize(nvmFilePath);
            var patchFile = PatchFileV1Serializer.Deserialize(patchFilePath);
            var plyFile = PlyV1TextSerializer.Deserialize(meshedPlyFilePath);

            // Create the structure model data structure.
            var structureModel = Operations.StructureModelBuild(plyFile);

            // Create OBJ required file paths.
            string outputDirectoryPath = @"C:\temp";
            string objFileNameWithoutExtension = @"mesh_simplified_poisson_regular";
            string objFileName = $@"{objFileNameWithoutExtension}.obj";
            string objFilePath = Path.Combine(outputDirectoryPath, objFileName);
            string materialFileName = $@"{objFileNameWithoutExtension}.mtl";
            string materialFilePath = Path.Combine(outputDirectoryPath, materialFileName);
            string materialFileRelativePathFromObjFile = materialFileName;
            string textureFileName = $@"{objFileNameWithoutExtension}.bmp"; // No spaces in relative path from OBJ file to 
            string textureFilePath = Path.Combine(outputDirectoryPath, textureFileName);
            string textureFileRelativePathFromMaterialFile = textureFileName;

            // Create the vertices in the same order as given by the structure model.
            int nVertices = structureModel.Vertices.Count;
            List<Vector3Double> vertices = new List<Vector3Double>(nVertices);
            foreach (var vertex in structureModel.Vertices)
            {
                var vertexVector = vertex.ToVector3Double();
                vertices.Add(vertexVector);
            }

            // Create the facets, using the vertex indices in the same order as given by the structure model.
            // The vertex texture UV vector indices will be created in the same order as the facets given by the structure model.
            int nFacets = structureModel.Facets.Count;
            List<ObjFacet> facets = new List<ObjFacet>(nFacets);
            int uvCoordinateVectorIndex = 1; // OBJ file starts at 1.
            foreach (var facet in structureModel.Facets)
            {
                ObjFacetVertex v1 = new ObjFacetVertex(facet.Vertex1Index + 1, ObjFacetVertex.NotSpecifiedValue, uvCoordinateVectorIndex++); // OBJ file starts at 1.
                ObjFacetVertex v2 = new ObjFacetVertex(facet.Vertex2Index + 1, ObjFacetVertex.NotSpecifiedValue, uvCoordinateVectorIndex++); // OBJ file starts at 1.
                ObjFacetVertex v3 = new ObjFacetVertex(facet.Vertex3Index + 1, ObjFacetVertex.NotSpecifiedValue, uvCoordinateVectorIndex++); // OBJ file starts at 1.

                ObjFacet objFacet = new ObjFacet(v1, v2, v3);
                facets.Add(objFacet);
            }

            // Create the texture image.
            // Setup, create camera matrices for all cameras, get image sizes, load images.
            int nCameras = nvm.Cameras.Length;
            List<Matrix<double>> cameraMatrices = new List<Matrix<double>>(nCameras);
            foreach (var camera in nvm.Cameras)
            {
                var cameraMatrix = Operations.CameraMatrix(camera);
                cameraMatrices.Add(cameraMatrix);
            }

            // Setup, find patches closest to each vertex, with patches being repeated but for each vertex index in order, the closest patch is identified.
            List<Patch> patchesClosestToVertex = Operations.PatchesClosest(patchFile, vertices);

            // Setup, get image info for both SFM and wedge images.
            var imageInfoSets = Operations.ImageInfoSets(sfmImagesDirectoryPath, wedgeImagesDirectoryPath, nvm.Cameras);

            // Create a mini-image for each facet.
            var miniImagesAndVertexPixels = Operations.MiniImagePerFacet(nvm.Cameras, structureModel, patchesClosestToVertex, imageInfoSets, cameraMatrices);
            List<Bitmap> miniImages = miniImagesAndVertexPixels.Item1;
            List<List<Location2Integer>> miniImageVertexPixels = miniImagesAndVertexPixels.Item2;

            // Pack the facet mini-images into a single texture image.
            Console.WriteLine($@"Packing {miniImages.Count.ToString()} mini-images...");
            var compositeImageAndTranslatedVertexPixels = ImagePacker.PackImages(miniImages, miniImageVertexPixels);
            Console.WriteLine(@"Done packing mini-images.");
            var textureImage = compositeImageAndTranslatedVertexPixels.Item1;
            int textureImageWidth = textureImage.Width;
            int textureImageHeight = textureImage.Height;
            var translatedVertexPixels = compositeImageAndTranslatedVertexPixels.Item2;

            // Get the UV coordinates.
            var vertexTextureUVs = new List<Location2Double>();
            foreach (var translatedMiniImageVertexPixels in translatedVertexPixels)
            {
                var uvMappedPixels = Operations.UVMapPixels(translatedMiniImageVertexPixels, textureImageWidth, textureImageHeight);
                vertexTextureUVs.AddRange(uvMappedPixels);
            }

            // Serialize all files.
            textureImage.Save(textureFilePath, SysImageFormat.Bmp);

            MaterialFile materialFile = new MaterialFile(textureFileRelativePathFromMaterialFile);
            MaterialFileSerializer.Serialize(materialFilePath, materialFile);

            ObjFileSerializer.Serialize(objFilePath, materialFileRelativePathFromObjFile, vertices, vertexTextureUVs, facets);
        }

        private static void CreateSingleTriangleObjFile()
        {
            var properties = Program.GetProjectProperties();
            string imagesDirectoryPath = properties[EshunnaProperties.ImagesDirectoryPathPropertyName];
            string nvmFilePath = properties[EshunnaProperties.ExampleNvmFilePathPropertyName];
            string meshedPlyFilePath = properties[EshunnaProperties.ExampleMeshPoissonSimplePlyFilePathPropertyName];

            var nvm = NvmV3Serializer.Deserialize(nvmFilePath);
            var plyFile = PlyV1TextSerializer.Deserialize(meshedPlyFilePath);

            var structureModel = Operations.StructureModelBuild(plyFile);

            int facetIndex = 2473;
            int imageIndex = 16;

            // Get the vertices of the face.
            var vertices = Operations.FacetVertices(structureModel, facetIndex);

            // Build the array of homogenous 3D point vectors.
            var verticesHomogenous = Operations.HomogenousVectorArrayCreate(vertices);

            // Get the camera matrix for the image.
            var cameraMatrix = Operations.CameraMatrix(nvm, imageIndex);

            // Get the centered-origin 2D image locations of the vertices.
            var vertexImageLocationsCenteredHomogenous = Operations.ProductHomogenousNormalizeColumnVectors(cameraMatrix, verticesHomogenous);
            var vertexImageLocationsCentered = Operations.Location2Double(vertexImageLocationsCenteredHomogenous);

            // Get the upper-left origin 2D image locations of the vertices.
            string imageFilePath = Operations.ImageFilePathGet(imagesDirectoryPath, nvm, imageIndex);
            var imageSize = ExternalFormatImageSizeProvider.GetSizeS(imageFilePath);
            var vertexImageLocationsUpperLeft = Operations.OriginTranslateCenteredToUpperLeft(vertexImageLocationsCentered, imageSize.Width, imageSize.Height);
            var vertexImagePixelLocations = vertexImageLocationsUpperLeft.Round();

            // Cut out the facet from the image and place in a mini-image.
            // Get the mini-image, upper-left locations of the vertices.
            // Create the mini-image.
            var miniImageBoundingBox = vertexImagePixelLocations.BoundingBoxGet();
            var miniImageRectangle = miniImageBoundingBox.RectangleXWidthGet();
            var miniImage = new Bitmap(miniImageRectangle.Width, miniImageRectangle.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            var vertexMiniImagePixelLocations = Operations.OriginTranslate(vertexImagePixelLocations, miniImageBoundingBox.XMin, miniImageBoundingBox.YMin);

            // Color the mini-image.
            var image = new Bitmap(imageFilePath);

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

            string textureFilePath = @"C:\temp\single_triangle.bmp"; // No spaces.
            string objFilePath = @"C:\temp\single_triangle.obj"; // No spaces.
            string materialFilePath = @"C:\temp\single_triangle.mtl"; // No spaces.

            // Save the mini-image.
            miniImage.Save(textureFilePath, SysImageFormat.Bmp);

            // Convert the mini-image pixel locations to UV-image coordinates.
            var uvMappedVertexPixels = Operations.UVMapPixels(vertexMiniImagePixelLocations, miniImageRectangle.Width, miniImageRectangle.Height);

            // Create the OBJ data structure.
            List<Vector3Double> vertexLocations = vertices;
            List<Location2Double> vertexTextureUVs = uvMappedVertexPixels;

            ObjFacetVertex v1 = new ObjFacetVertex(1, ObjFacetVertex.NotSpecifiedValue, 1); // Counting starts at 1.
            ObjFacetVertex v2 = new ObjFacetVertex(2, ObjFacetVertex.NotSpecifiedValue, 2);
            ObjFacetVertex v3 = new ObjFacetVertex(3, ObjFacetVertex.NotSpecifiedValue, 3);

            ObjFacet facet = new ObjFacet(v1, v2, v3);

            ObjFileSerializer.Serialize(objFilePath, Path.GetFileName(materialFilePath), vertexLocations, vertexTextureUVs, new ObjFacet[] { facet });

            MaterialFile materialFile = new MaterialFile(Path.GetFileName(textureFilePath));
            MaterialFileSerializer.Serialize(materialFilePath, materialFile);

            // Write the OBJ file.
            // Write the vertex coordinates, vertex texture coordinates, and faces.
            // Write the MTL file.
            // Write the texture file (as bitmap if possible).
        }

        private static void CompareFacetNormals()
        {
            var properties = Program.GetProjectProperties();

            string nvmFilePath = properties[EshunnaProperties.ExampleNvmFilePathPropertyName];
            string patchFilePath = properties[EshunnaProperties.ExamplePatchFilePathPropertyName];
            string meshedPlyFilePath = properties[EshunnaProperties.ExamplePlyBinaryFilePathPropertyName];

            var nvm = NvmV3Serializer.Deserialize(nvmFilePath);
            var patchFile = PatchFileV1Serializer.Deserialize(patchFilePath);
            var plyFile = PlyV1BinarySerializer.Deserialize(meshedPlyFilePath);

            var structureModel = Operations.StructureModelBuild(plyFile);

            int facetIndex = 1399;
            var crossProductNormal = Operations.FacetNormal(structureModel, facetIndex);
            var patchAverageNormal = Operations.FacetNormal(structureModel, facetIndex, patchFile);
            var centroid = Operations.FacetCentroidVector(structureModel, facetIndex);

            int cameraIndex = 4;
            var centroidToCameraUnit = Operations.PointToCameraVectorUnit(nvm, cameraIndex, centroid);

            var dotCrossProductToPatchAverage = crossProductNormal.Dot(patchAverageNormal);
            var dotCrossProductToCamera = crossProductNormal.Dot(centroidToCameraUnit);
            var dotPatchAverageToCamera = patchAverageNormal.Dot(centroidToCameraUnit);
        }

        private static void RotateFacetFullyTowardsCamera()
        {
            var properties = Program.GetProjectProperties();
            string imagesDirectoryPath = properties[EshunnaProperties.ImagesDirectoryPathPropertyName];
            string nvmFilePath = properties[EshunnaProperties.ExampleNvmFilePathPropertyName];
            string patchFilePath = properties[EshunnaProperties.ExamplePatchFilePathPropertyName];
            string meshedPlyFilePath = properties[EshunnaProperties.ExamplePlyBinaryFilePathPropertyName];

            var nvm = NvmV3Serializer.Deserialize(nvmFilePath);
            var patchFile = PatchFileV1Serializer.Deserialize(patchFilePath);
            var plyFile = PlyV1BinarySerializer.Deserialize(meshedPlyFilePath);

            var structureModel = Operations.StructureModelBuild(plyFile);

            // Get the 3D vertex locations of a facet.
            int facetIndex = 1399;
            var facet = structureModel.Facets[facetIndex];

            var v1 = structureModel.Vertices[facet.Vertex1Index].ToVector3Double();
            var v2 = structureModel.Vertices[facet.Vertex2Index].ToVector3Double();
            var v3 = structureModel.Vertices[facet.Vertex3Index].ToVector3Double();
            var vertices = new Vector3Double[] { v1, v2, v3 };

            // Determine the facet centroid.
            var facetCentroid = Operations.FacetCentroid(structureModel, facetIndex).ToVector3Double();

            // Determine the normal vector.
            //var facetNormal = Operations.FacetNormal(structureModel, facetIndex); // Cross-product normal.
            var facetNormal = Operations.FacetNormal(structureModel, facetIndex, patchFile); // Patch-average normal.

            // Determine the vector from facet centroid to camera.
            int imageIndex = 4;
            var cameraLocation = nvm.Cameras[imageIndex].Location.ToVector3Double();
            var facetToCamera = cameraLocation - facetCentroid;
            var facetToCameraUnit = facetToCamera.L2Normalize();

            // Get the cross-product vector and dot-product value of the facet normal and the facet to camera unit vectors.
            var crossProduct = facetToCameraUnit.Cross(facetNormal);
            var crossProductUnit = crossProduct.L2Normalize();
            var dotProduct = facetToCameraUnit.Dot(facetNormal);

            // Create the rotation matrix.
            var rotationAngle = Math.Acos(dotProduct);
            var rotationQuaternion = new QuaternionDouble(rotationAngle, crossProductUnit.X, crossProductUnit.Y, crossProductUnit.Z);
            var rotationQuaternionUnit = rotationQuaternion.L2Normalize();
            var rotationMatrix = QuaternionDouble.GetRotationMatrix(rotationQuaternionUnit).ToMathNetMatrix();

            // Compute the facet vertex locations relative to the facet centroid.
            var verticesCentroid = vertices - facetCentroid;

            var verticesCentroidMat = Operations.InhomogenousVectorArrayCreate(verticesCentroid);

            // Rotate the facet.
            var rotatedVerticesCentroidHomogenous = rotationMatrix * verticesCentroidMat;

            var centroidRotated = Operations.Vector3ArrayCreate(rotatedVerticesCentroidHomogenous);

            var rotated = centroidRotated + facetCentroid;

            // Now project both the facet and the rotated facet onto the camera.
            var cameraMatrix = Operations.CameraMatrix(nvm, imageIndex);

            var verticesHomogenous = Operations.HomogenousVectorArrayCreate(v1, v2, v3);
            var projectedVerticesHomogenous = Operations.ProductHomogenousNormalizeColumnVectors(cameraMatrix, verticesHomogenous);

            var verticesRotatedHomogenous = Operations.HomogenousVectorArrayCreate(rotated);
            var projectedVerticesRotatedHomogenous = Operations.ProductHomogenousNormalizeColumnVectors(cameraMatrix, verticesRotatedHomogenous);

            var projectedVertices = Operations.Vector2ArrayCreate(projectedVerticesHomogenous);

            var projectedVerticesRotated = Operations.Vector2ArrayCreate(projectedVerticesRotatedHomogenous);

            Operations.DrawPoints(projectedVertices, @"C:\temp\projected vertices.bmp", SysImageFormat.Bmp, false);

            //Operations.DrawPoints(projectedVerticesRotated, @"C:\temp\projected vertices rotated-cross product.bmp", SysImageFormat.Bmp, false);
            Operations.DrawPoints(projectedVerticesRotated, @"C:\temp\projected vertices rotated-patch average.bmp", SysImageFormat.Bmp, false);
        }

        private static void CutFacetFromAllImages()
        {
            var properties = Program.GetProjectProperties();
            string imagesDirectoryPath = properties[EshunnaProperties.ImagesDirectoryPathPropertyName];
            string nvmFilePath = properties[EshunnaProperties.ExampleNvmFilePathPropertyName];
            string patchFilePath = properties[EshunnaProperties.ExamplePatchFilePathPropertyName];
            string meshedPlyFilePath = properties[EshunnaProperties.ExamplePlyBinaryFilePathPropertyName];

            var nvm = NvmV3Serializer.Deserialize(nvmFilePath);
            var patchFile = PatchFileV1Serializer.Deserialize(patchFilePath);
            var plyFile = PlyV1BinarySerializer.Deserialize(meshedPlyFilePath);

            var structureModel = Operations.StructureModelBuild(plyFile);

            int facetIndex = 1399;
            var facet = structureModel.Facets[facetIndex];

            var v1 = structureModel.Vertices[facet.Vertex1Index].ToVector3Double();
            var v2 = structureModel.Vertices[facet.Vertex2Index].ToVector3Double();
            var v3 = structureModel.Vertices[facet.Vertex3Index].ToVector3Double();

            var homogenousPointLocations = Operations.HomogenousVectorArrayCreate(v1, v2, v3);

            // To pick the facet triangle out of each image and place them on a single slide image:
            // For each image.
            //  Load the image.
            //  Create a mini-image that is the size of the facet bounding box in the image.
            //  For each pixel location within the facet triangle:
            //      Get the pixel color from the main image at that location.
            //      Put the pixel color into the mini-image at the location, minus the facet image bounding box XMin and YMin.

            int[] imageIndicesForFacet = Operations.ImageIndicesForFacet(structureModel, facetIndex, patchFile);
            int nImages = imageIndicesForFacet.Length;
            var boundingBoxes = new List<BoundingBoxInteger>(nImages);
            var miniImages = new List<Bitmap>(nImages);
            for (int iImage = 0; iImage < nImages; iImage++)
            {
                int imageIndex = imageIndicesForFacet[iImage];

                var cameraMatrix = Operations.CameraMatrix(nvm, imageIndex);

                var imageVertexLocationsCenterOrigin = Operations.ImageLocationsGet(cameraMatrix, homogenousPointLocations);

                string imageFileName = nvm.Cameras[imageIndex].FileName;
                string imageFilePath = Operations.ImageFilePathGet(imagesDirectoryPath, nvm, imageIndex);
                var image = new Bitmap(imageFilePath);

                var imageVertexLocationsUpperLeftOrigin = Operations.OriginTranslateCenteredToUpperLeft(imageVertexLocationsCenterOrigin, image.Width, image.Height);

                var imageVertexLocations = imageVertexLocationsUpperLeftOrigin.Round();

                var v1Image = imageVertexLocations[0];
                var v2Image = imageVertexLocations[1];
                var v3Image = imageVertexLocations[2];

                var imagePixelLocations = Geometry.ListTriangleLocations(v1Image, v2Image, v3Image);

                var boundingBox = imagePixelLocations.BoundingBoxGet();
                boundingBoxes.Add(boundingBox);

                var miniImageRectangle = boundingBox.RectangleXWidthGet();
                var miniImage = new Bitmap(miniImageRectangle.Width, miniImageRectangle.Height);
                miniImages.Add(miniImage);

                foreach (var imagePixelLocation in imagePixelLocations)
                {
                    var color = image.GetPixel(imagePixelLocation.X, imagePixelLocation.Y);

                    int miniImageX = imagePixelLocation.X - boundingBox.XMin;
                    int miniImageY = imagePixelLocation.Y - boundingBox.YMin;
                    miniImage.SetPixel(miniImageX, miniImageY, color);
                }

                string fileNameNoExtension = Path.GetFileNameWithoutExtension(imageFileName);
                string miniImageFilePath = $@"C:\temp\mini image {iImage.ToString()} {fileNameNoExtension}.bmp";
                miniImage.Save(miniImageFilePath, System.Drawing.Imaging.ImageFormat.Bmp);
            }

            var outputRectangle = Operations.RectangleHorizontal(boundingBoxes);
            var slide = new Bitmap(outputRectangle.Width + nImages + 1, outputRectangle.Height + 6); // Allow for a single line of vertical space between specimens.
            using (var gfx = Graphics.FromImage(slide))
            {
                int leftPixelStart = 1;
                for (int iImage = 0; iImage < nImages; iImage++)
                {
                    var miniImage = miniImages[iImage];
                    gfx.DrawImage(miniImage, leftPixelStart, 3);

                    var boundingBox = boundingBoxes[iImage];
                    var miniImageRectangle = boundingBox.RectangleXWidthGet();

                    leftPixelStart += miniImageRectangle.Width + 1;
                }
            }

            string slideImageFilePath = @"C:\temp\slide.bmp";
            slide.Save(slideImageFilePath, System.Drawing.Imaging.ImageFormat.Bmp);
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
                    bool isPointInTriangle = Geometry.IsPointInTriangle(new Location2Double(iRow, iCol), v1, v2, v3);
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
                    bool isPointInTriangle = Geometry.IsPointInTriangle(new Location2Integer(iRow, iCol), v1, v2, v3);
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

        private static void CompareVectorNormals()
        {
            var properties = Program.GetProjectProperties();

            string plyFilePath = properties[EshunnaProperties.ExamplePlyBinaryFilePathPropertyName]; // The vertices and faces meshed, dense PLY file.
            string patchFilePath = properties[EshunnaProperties.ExamplePatchFilePathPropertyName];

            var plyFile = PlyV1BinarySerializer.Deserialize(plyFilePath);
            var patchFile = PatchFileV1Serializer.Deserialize(patchFilePath);

            var structureModel = Operations.StructureModelBuild(plyFile);

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
                Vector3Double vertexCrossProductNormal = Operations.FacetNormal(structureModel, iFacet);
                vertexCrossProductNormals[iFacet] = vertexCrossProductNormal;

                Vector3Double vertexPatchNormalsAverageNormal = Operations.FacetNormal(structureModel, iFacet, patchFile);
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

            var structureModel = Operations.StructureModelBuild(plyFile);
        }

        private static void DetermineAllPatchImageIndices()
        {
            var properties = Program.GetProjectProperties();
            string patchFilePath = properties[EshunnaProperties.ExamplePatchFilePathPropertyName];

            var patchFile = PatchFileV1Serializer.Deserialize(patchFilePath);

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
            var patchFile = PatchFileV1Serializer.Deserialize(patchFilePath);
            var plyFile = PlyV1BinarySerializer.Deserialize(meshedPlyFilePath);

            var structureModel = Operations.StructureModelBuild(plyFile);

            int facetIndex = 1399;
            int[] imageIndicesForFacet = Operations.ImageIndicesForFacet(structureModel, facetIndex, patchFile);

            var facetCentroidLocation = Operations.FacetCentroid(structureModel, facetIndex).ToVector3Double();

            var facetNormal = Operations.FacetNormal(structureModel, facetIndex);

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
            //string plyFilePath = properties[EshunnaProperties.ExamplePlyBinaryFilePathPropertyName]; // The vertices and faces meshed, dense PLY file.
            string plyFilePath = properties[EshunnaProperties.ExampleMeshPoissonSimplePlyFilePathPropertyName];
            string patchFilePath = properties[EshunnaProperties.ExamplePatchFilePathPropertyName];
            string imagesDirectoryPath = properties[EshunnaProperties.ImagesDirectoryPathPropertyName];

            var nvm = NvmV3Serializer.Deserialize(nvmFilePath);
            //var plyFile = PlyV1BinarySerializer.Deserialize(plyFilePath);
            var plyFile = PlyV1TextSerializer.Deserialize(plyFilePath);
            var patchFile = PatchFileV1Serializer.Deserialize(patchFilePath);

            var structureModel = Operations.StructureModelBuild(plyFile);

            // Get the face of interest.
            int facetIndex = 2473; // 1399; // 7945;
            var facet = structureModel.Facets[facetIndex];

            // Get the vertices of the face.
            var vertices = Operations.FacetVertices(structureModel, facetIndex);

            // Build the array of homogenous 3D point vectors.
            var verticesHomogenous = Operations.HomogenousVectorArrayCreate(vertices);

            // Get the patch for each vertex.
            var patches = Operations.PatchesClosest(patchFile, vertices);

            // Determine all images that contain at least one of the points.
            var imageIndices = Operations.ImageIndicesWithGoodAgreement(patches);
            foreach (var imageIndex in imageIndices)
            {
                var cameraMatrix = Operations.CameraMatrix(nvm, imageIndex);
                var imageFilePath = Operations.ImageFilePathGet(imagesDirectoryPath, nvm, imageIndex);

                // Compute the image points relative to an origin at the image center.
                var imagePointsCenteredHomogenous = Operations.ProductHomogenousNormalizeColumnVectors(cameraMatrix, verticesHomogenous);
                var imagePointsCentered = Operations.Location2Double(imagePointsCenteredHomogenous);

                // Shift the image points to be relative to an origin at the upper-left of the image.
                var imageSize = ExternalFormatImageSizeProvider.GetSizeS(imageFilePath);
                var imagePointsUpperLeft = Operations.OriginTranslateCenteredToUpperLeft(imagePointsCentered, imageSize.Width, imageSize.Height);

                // Get pixel points.
                var imagePixels = imagePointsUpperLeft.Round();

                // Make the image.
                string outputFilePath = Path.Combine(@"C:\temp", Path.GetFileName(imageFilePath));

                var tuples = Operations.Tuple2Integer(imagePixels);
                ImageMarker.MarkImage(imageFilePath, outputFilePath, tuples, System.Drawing.Imaging.ImageFormat.Bmp, false);
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
            var patchFile = PatchFileV1Serializer.Deserialize(patchFilePath);

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

                Location3Double translation = Operations.TranslationGet(rotation, camera.Location);
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
            var patchFile = PatchFileV1Serializer.Deserialize(patchFilePath);

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

            var serializer = new PatchFileV1Serializer();

            var log = new StringListLog();
            var patchCollectionComparer = new PatchFileEqualityComparer(log);

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

            PatchFile patchCollection = PatchFileV1Serializer.Deserialize(exampleFilePath);
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

            //double[] arr = new double[0];
            //double value = arr[1]; // System.IndexOutOfRangeException

            int a;
            a = 1;
            int b = ++a;
            a = 1;
            int c = a++;
        }
    }
}
