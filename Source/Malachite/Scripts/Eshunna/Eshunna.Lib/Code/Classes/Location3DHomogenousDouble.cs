using System;

using Public.Common.Lib;


namespace Eshunna.Lib
{
    [Serializable]
    public struct Location3DHomogenousDouble : IEquatable<Location3DHomogenousDouble>
    {
        #region Static

        public static bool operator ==(Location3DHomogenousDouble lhs, Location3DHomogenousDouble rhs)
        {
            bool output = lhs.Equals(rhs);
            return output;
        }

        public static bool operator !=(Location3DHomogenousDouble lhs, Location3DHomogenousDouble rhs)
        {
            bool output = !lhs.Equals(rhs);
            return output;
        }

        #endregion


        public double X { get; }
        public double Y { get; }
        public double Z { get; }
        public double H { get; }


        public Location3DHomogenousDouble(double x, double y, double z, double h)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.H = h;
        }

        public bool Equals(Location3DHomogenousDouble other)
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
            if (obj is Location3DHomogenousDouble objAsLocation3DHomogenous)
            {
                output = this.Equals(objAsLocation3DHomogenous);
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
            string output = $@"X: {this.X.ToString()}, Y: {this.Y.ToString()}, Z: {this.Z.ToString()}, H: {this.H.ToString()}";
            return output;
        }
    }
}
