using System;
using System.Collections.Generic;

using Public.Common.Lib.Extensions;

using Eshunna.Lib.Patches;
using Eshunna.Lib.PLY;


namespace Eshunna.Lib
{
    public static class Operations
    {
        public static Location3Double GetFacetCentroid(StructureModel structureModel, int facetIndex)
        {
            var facet = structureModel.Facets[facetIndex];

            var vertex1 = structureModel.Vertices[facet.Vertex1Index];
            var vertex2 = structureModel.Vertices[facet.Vertex2Index];
            var vertex3 = structureModel.Vertices[facet.Vertex3Index];

            double x = (vertex1.X + vertex2.X + vertex3.X) / 3;
            double y = (vertex1.Y + vertex2.Y + vertex3.Y) / 3;
            double z = (vertex1.Z + vertex2.Z + vertex3.Z) / 3;

            var output = new Location3Double(x, y, z);
            return output;
        }

        /// <summary>
        /// Uses the cross-product of the vectors between the facet vertices.
        /// </summary>
        public static Vector3Double GetFacetNormal(StructureModel structureModel, int facetIndex)
        {
            var facet = structureModel.Facets[facetIndex];

            var vertex1 = structureModel.Vertices[facet.Vertex1Index];
            var vertex2 = structureModel.Vertices[facet.Vertex2Index];
            var vertex3 = structureModel.Vertices[facet.Vertex3Index];

            var output = Operations.GetFacetNormal(vertex1, vertex2, vertex3);
            return output;
        }

        /// <summary>
        /// Computes the cross-product: (v3 - v1) x (v2 - v1). This direction results in normals that are well aligned with the average of the vertex patch normals.
        /// </summary> o
        public static Vector3Double GetFacetNormal(Location3Double vertex1, Location3Double vertex2, Location3Double vertex3)
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
        public static Vector3Double GetFacetNormal(StructureModel structureModel, int facetIndex, PatchFile patchFile)
        {
            var facet = structureModel.Facets[facetIndex];

            var normalVertex1 = patchFile.Patches[facet.Vertex1Index].Normal.L2Normalize();
            var normalVertex2 = patchFile.Patches[facet.Vertex2Index].Normal.L2Normalize();
            var normalVertex3 = patchFile.Patches[facet.Vertex3Index].Normal.L2Normalize();

            double x = (normalVertex1.X + normalVertex2.X + normalVertex3.X) / 3;
            double y = (normalVertex1.Y + normalVertex2.Y + normalVertex3.Y) / 3;
            double z = (normalVertex1.Z + normalVertex2.Z + normalVertex3.Z) / 3;

            var average = new Vector3Double(x, y, z);
            var output = average.L2Normalize();
            return output;
        }

        public static int[] GetImageIndicesForFacet(StructureModel structureModel, int facetIndex, PatchFile patchFile)
        {
            var facet = structureModel.Facets[facetIndex];

            Patch vertex1Patch = patchFile.Patches[facet.Vertex1Index];
            Patch vertex2Patch = patchFile.Patches[facet.Vertex2Index];
            Patch vertex3Patch = patchFile.Patches[facet.Vertex3Index];

            HashSet<int> imageIndices = new HashSet<int>();
            vertex1Patch.ImageIndicesWithGoodAgreement.ForEach((x) => imageIndices.Add(x));
            vertex2Patch.ImageIndicesWithGoodAgreement.ForEach((x) => imageIndices.Add(x));
            vertex3Patch.ImageIndicesWithGoodAgreement.ForEach((x) => imageIndices.Add(x));

            List<int> indicesInOrder = new List<int>(imageIndices);
            indicesInOrder.Sort();

            var output = indicesInOrder.ToArray();
            return output;
        }

        public static Vector3Double GetPatchNormal(PatchFile patchFile, int patchIndex)
        {
            var patch = patchFile.Patches[patchIndex];

            var output = Operations.GetPatchNormal(patch);
            return output;
        }

        public static Vector3Double GetPatchNormal(Patch patch)
        {
            var output = patch.Location.ToVector3Double();
            return output;
        }

        public static StructureModel GetStructureModel(PlyFile plyFile)
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
    }
}
