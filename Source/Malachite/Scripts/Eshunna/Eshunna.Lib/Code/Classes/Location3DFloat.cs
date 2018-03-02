using System;

using Public.Common.Lib;


namespace Eshunna.Lib
{
    [Serializable]
    public struct Location3DFloat : IEquatable<Location3DFloat>
    {
        #region Static

        public static bool operator ==(Location3DFloat lhs, Location3DFloat rhs)
        {
            bool output = lhs.Equals(rhs);
            return output;
        }

        public static bool operator !=(Location3DFloat lhs, Location3DFloat rhs)
        {
            bool output = !lhs.Equals(rhs);
            return output;
        }

        #endregion


        public float X { get; }
        public float Y { get; }
        public float Z { get; }


        public Location3DFloat(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public bool Equals(Location3DFloat other)
        {
            bool output =
                this.X == other.X &&
                this.Y == other.Y &&
                this.Z == other.Z;
            return output;
        }

        public override bool Equals(object obj)
        {
            bool output = false;
            if (obj is Location3DFloat objAsLocation3DFloat)
            {
                output = this.Equals(objAsLocation3DFloat);
            }
            return output;
        }

        public override int GetHashCode()
        {
            int output = HashHelper.GetHashCode(this.X, this.Y, this.Z);
            return output;
        }

        public override string ToString()
        {
            string output = $@"X: {this.X.ToString()}, Y: {this.Y.ToString()}, Z: {this.Z.ToString()}";
            return output;
        }
    }
}
