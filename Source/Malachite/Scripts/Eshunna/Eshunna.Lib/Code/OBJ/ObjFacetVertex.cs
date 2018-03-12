using System;

using Public.Common.Lib;


namespace Eshunna.Lib.OBJ
{
    [Serializable]
    public struct ObjFacetVertex : IEquatable<ObjFacetVertex>
    {
        public const int NotSpecifiedValue = Int32.MinValue;


        #region Static

        public static bool operator ==(ObjFacetVertex lhs, ObjFacetVertex rhs)
        {
            bool output = lhs.Equals(rhs);
            return output;
        }

        public static bool operator !=(ObjFacetVertex lhs, ObjFacetVertex rhs)
        {
            bool output = !lhs.Equals(rhs);
            return output;
        }

        #endregion


        public int VertexIndex { get; }
        public int NormalIndex { get; }
        public int TextureIndex { get; }


        public ObjFacetVertex(int vertexIndex, int normalIndex, int textureIndex)
        {
            this.VertexIndex = vertexIndex;
            this.NormalIndex = normalIndex;
            this.TextureIndex = textureIndex;
        }

        public bool Equals(ObjFacetVertex other)
        {
            bool output =
                this.VertexIndex == other.VertexIndex &&
                this.NormalIndex == other.NormalIndex &&
                this.TextureIndex == other.TextureIndex;
            return output;
        }

        public override bool Equals(object obj)
        {
            bool output = false;
            if (obj is ObjFacetVertex objAsObjFacetVertex)
            {
                output = this.Equals(objAsObjFacetVertex);
            }
            return output;
        }

        public override int GetHashCode()
        {
            int output = HashHelper.GetHashCode(this.VertexIndex, this.NormalIndex, this.TextureIndex);
            return output;
        }

        public override string ToString()
        {
            string output = $@"Vertex: {this.VertexIndex.ToString()}, Normal: {this.NormalIndex.ToString()}, Texture: {this.TextureIndex.ToString()} (int)";
            return output;
        }
    }
}
