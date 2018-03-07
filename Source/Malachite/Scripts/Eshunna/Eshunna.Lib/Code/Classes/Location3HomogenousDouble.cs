using System;

using Public.Common.Lib;


namespace Eshunna.Lib
{
    [Serializable]
    public struct Location3HomogenousDouble : IEquatable<Location3HomogenousDouble>
    {
        #region Static

        public static bool operator ==(Location3HomogenousDouble lhs, Location3HomogenousDouble rhs)
        {
            bool output = lhs.Equals(rhs);
            return output;
        }

        public static bool operator !=(Location3HomogenousDouble lhs, Location3HomogenousDouble rhs)
        {
            bool output = !lhs.Equals(rhs);
            return output;
        }

        #endregion


        public double X { get; }
        public double Y { get; }
        public double Z { get; }
        public double H { get; }


        public Location3HomogenousDouble(double x, double y, double z, double h)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.H = h;
        }

        public bool Equals(Location3HomogenousDouble other)
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
            if (obj is Location3HomogenousDouble objAsLocation3HomogenousDouble)
            {
                output = this.Equals(objAsLocation3HomogenousDouble);
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
            string output = $@"X: {this.X.ToString()}, Y: {this.Y.ToString()}, Z: {this.Z.ToString()}, H: {this.H.ToString()} (double)";
            return output;
        }
    }


    public static class Location3DHomogenousDoubleExtensions
    {
        public static Location3Double ToLocation3Double(this Location3HomogenousDouble value)
        {
            var output = new Location3Double(value.X, value.Y, value.Z);
            return output;
        }

        public static Vector3Double ToVector3Double(this Location3HomogenousDouble value)
        {
            var output = new Vector3Double(value.X, value.Y, value.Z);
            return output;
        }

        public static Vector4Double ToVector4Double(this Location3HomogenousDouble value)
        {
            var output = new Vector4Double(value.X, value.H, value.Z, value.H);
            return output;
        }
    }
}
