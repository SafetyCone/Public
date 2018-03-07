using System;

using Public.Common.Lib;


namespace Eshunna.Lib
{
    [Serializable]
    public struct Location3Double : IEquatable<Location3Double>
    {
        #region Static

        public static bool operator ==(Location3Double lhs, Location3Double rhs)
        {
            bool output = lhs.Equals(rhs);
            return output;
        }

        public static bool operator !=(Location3Double lhs, Location3Double rhs)
        {
            bool output = !lhs.Equals(rhs);
            return output;
        }

        #endregion


        public double X { get; }
        public double Y { get; }
        public double Z { get; }


        public Location3Double(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public bool Equals(Location3Double other)
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
            if (obj is Location3Double objAsLocation3Double)
            {
                output = this.Equals(objAsLocation3Double);
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
            string output = $@"X: {this.X.ToString()}, Y: {this.Y.ToString()}, Z: {this.Z.ToString()} (double)";
            return output;
        }
    }


    public static class Location3DoubleExtensions
    {
        public static Vector3Double ToVector3Double(this Location3Double value)
        {
            var output = new Vector3Double(value.X, value.Y, value.Z);
            return output;
        }
    }
}
