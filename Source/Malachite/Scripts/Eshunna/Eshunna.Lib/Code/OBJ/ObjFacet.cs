using System;

using Public.Common.Lib;


namespace Eshunna.Lib.OBJ
{
    [Serializable]
    public struct ObjFacet : IEquatable<ObjFacet>
    {
        #region Static

        public static bool operator ==(ObjFacet lhs, ObjFacet rhs)
        {
            bool output = lhs.Equals(rhs);
            return output;
        }

        public static bool operator !=(ObjFacet lhs, ObjFacet rhs)
        {
            bool output = !lhs.Equals(rhs);
            return output;
        }

        #endregion


        public ObjFacetVertex Vertex1 { get; }
        public ObjFacetVertex Vertex2 { get; }
        public ObjFacetVertex Vertex3 { get; }


        public ObjFacet(ObjFacetVertex v1, ObjFacetVertex v2, ObjFacetVertex v3)
        {
            this.Vertex1 = v1;
            this.Vertex2 = v2;
            this.Vertex3 = v3;
        }

        public bool Equals(ObjFacet other)
        {
            bool output =
                this.Vertex1 == other.Vertex1 &&
                this.Vertex2 == other.Vertex2 &&
                this.Vertex3 == other.Vertex3;
            return output;
        }

        public override bool Equals(object obj)
        {
            bool output = false;
            if (obj is ObjFacet objAsObjFacet)
            {
                output = this.Equals(objAsObjFacet);
            }
            return output;
        }

        public override int GetHashCode()
        {
            int output = HashHelper.GetHashCode(this.Vertex1, this.Vertex2, this.Vertex3);
            return output;
        }
    }
}
