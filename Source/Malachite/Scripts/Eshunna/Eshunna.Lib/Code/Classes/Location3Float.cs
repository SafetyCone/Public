using System;

using Public.Common.Lib;


namespace Eshunna.Lib
{
    [Serializable]
    public struct Location3Float : IEquatable<Location3Float>
    {
        #region Static

        public static bool operator ==(Location3Float lhs, Location3Float rhs)
        {
            bool output = lhs.Equals(rhs);
            return output;
        }

        public static bool operator !=(Location3Float lhs, Location3Float rhs)
        {
            bool output = !lhs.Equals(rhs);
            return output;
        }

        #endregion


        public float X { get; }
        public float Y { get; }
        public float Z { get; }


        public Location3Float(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public bool Equals(Location3Float other)
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
            if (obj is Location3Float objAsLocation3Float)
            {
                output = this.Equals(objAsLocation3Float);
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
            string output = $@"X: {this.X.ToString()}, Y: {this.Y.ToString()}, Z: {this.Z.ToString()} (float)";
            return output;
        }
    }


    public static class Location3FloatExtensions
    {
        public static Vector3Float ToVector3Float(this Location3Float value)
        {
            var output = new Vector3Float(value.X, value.Y, value.Z);
            return output;
        }
    }
}
