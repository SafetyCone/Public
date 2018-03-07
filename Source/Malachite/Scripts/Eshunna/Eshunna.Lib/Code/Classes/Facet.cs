using System;

using Public.Common.Lib;


namespace Eshunna.Lib
{
    public struct Facet : IEquatable<Facet>
    {
        public const int NumberOfVertices = 3;


        #region Static

        public static bool operator ==(Facet lhs, Facet rhs)
        {
            bool output = lhs.Equals(rhs);
            return output;
        }

        public static bool operator !=(Facet lhs, Facet rhs)
        {
            bool output = !lhs.Equals(rhs);
            return output;
        }

        #endregion


        public int Vertex1Index { get; }
        public int Vertex2Index { get; }
        public int Vertex3Index { get; }


        public Facet(int vertex1Index, int vertex2Index, int vertex3Index)
        {
            this.Vertex1Index = vertex1Index;
            this.Vertex2Index = vertex2Index;
            this.Vertex3Index = vertex3Index;
        }

        public bool Equals(Facet other)
        {
            bool output =
                this.Vertex1Index == other.Vertex1Index &&
                this.Vertex2Index == other.Vertex2Index &&
                this.Vertex3Index == other.Vertex3Index;
            return output;
        }

        public override bool Equals(object obj)
        {
            bool output = false;
            if (obj is Facet objAsFacet)
            {
                output = this.Equals(objAsFacet);
            }
            return output;
        }

        public override int GetHashCode()
        {
            int output = HashHelper.GetHashCode(this.Vertex1Index, this.Vertex2Index, this.Vertex3Index);
            return output;
        }

        public override string ToString()
        {
            string output = $@"V1: {this.Vertex1Index.ToString()}, V2: {this.Vertex2Index.ToString()}, V3: {this.Vertex3Index.ToString()}";
            return output;
        }
    }
}
