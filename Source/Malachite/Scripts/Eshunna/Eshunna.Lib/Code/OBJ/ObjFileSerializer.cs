using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace Eshunna.Lib.OBJ
{
    public class ObjFileSerializer
    {
        #region Static

        public static void Serialize(string filePath, string materialFileRelativePath, IEnumerable<Vector3Double> vertexLocations, IEnumerable<Location2Double> vertexTextureUVs, IEnumerable<ObjFacet> facets)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine($@"mtllib {materialFileRelativePath}");

                // Vertices.
                foreach (var vertexLocation in vertexLocations)
                {
                    string line = $@"v {vertexLocation.X.ToString()} {vertexLocation.Y.ToString()} {vertexLocation.Z.ToString()}";
                    writer.WriteLine(line);
                }

                // Vertex texture coordinates.
                foreach (var vertexTextureUV in vertexTextureUVs)
                {
                    string line = $@"vt {vertexTextureUV.X.ToString()} {vertexTextureUV.Y.ToString()}";
                    writer.WriteLine(line);
                }

                // Faces.
                StringBuilder builder = new StringBuilder();
                foreach (var facet in facets)
                {
                    builder.Clear();
                    builder.Append(@"f");

                    var facetVertices = new ObjFacetVertex[] { facet.Vertex1, facet.Vertex2, facet.Vertex3 };
                    foreach (var facetVertex in facetVertices)
                    {
                        string vertexIndexStr = ObjFacetVertex.NotSpecifiedValue == facetVertex.VertexIndex ? String.Empty : facetVertex.VertexIndex.ToString();
                        string normalIndexStr = ObjFacetVertex.NotSpecifiedValue == facetVertex.NormalIndex ? String.Empty : facetVertex.NormalIndex.ToString();
                        string textureIndexStr = ObjFacetVertex.NotSpecifiedValue == facetVertex.TextureIndex ? String.Empty : facetVertex.TextureIndex.ToString();

                        string appendix = $@" {vertexIndexStr}/{textureIndexStr}/{normalIndexStr}";
                        builder.Append(appendix);
                    }

                    string line = builder.ToString();
                    writer.WriteLine(line);
                }
            }
        }

        #endregion
    }
}
