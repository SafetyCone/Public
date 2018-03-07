using System;

using Public.Common.Lib;


namespace Eshunna.Lib
{
    [Serializable]
    public struct Location3HomogenousFloat : IEquatable<Location3HomogenousFloat>
    {
        #region Static

        public static bool operator ==(Location3HomogenousFloat lhs, Location3HomogenousFloat rhs)
        {
            bool output = lhs.Equals(rhs);
            return output;
        }

        public static bool operator !=(Location3HomogenousFloat lhs, Location3HomogenousFloat rhs)
        {
            bool output = !lhs.Equals(rhs);
            return output;
        }

        #endregion


        public float X { get; }
        public float Y { get; }
        public float Z { get; }
        public float H { get; }


        public Location3HomogenousFloat(float x, float y, float z, float h)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.H = h;
        }

        public bool Equals(Location3HomogenousFloat other)
        {
            bool output =
                this.X == other.X &&
                this.Y == other.Y &&
                this.Z == other.Z &&
                this.H == other.H;
            return output;
        }

        public override bool Equals(object obj)
        {
            bool output = false;
            if (obj is Location3HomogenousFloat objAsLocation3HomogenousFloat)
            {
                output = this.Equals(objAsLocation3HomogenousFloat);
            }
            return output;
        }

        public override int GetHashCode()
        {
            int output = HashHelper.GetHashCode(this.X, this.Y, this.Z); // Ignore H.
            return output;
        }

        public override string ToString()
        {
            string output = $@"X: {this.X.ToString()}, Y: {this.Y.ToString()}, Z: {this.Z.ToString()}, H: {this.H.ToString()} (float)";
            return output;
        }
    }
}
