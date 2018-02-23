using System;

using Public.Common.Lib;


namespace Eshunna.Lib
{
    [Serializable]
    public struct Location2D : IEquatable<Location2D>
    {
        #region Static

        public static bool operator==(Location2D lhs, Location2D rhs)
        {
            bool output = lhs.Equals(rhs);
            return output;
        }

        public static bool operator !=(Location2D lhs, Location2D rhs)
        {
            bool output = !lhs.Equals(rhs);
            return output;
        }

        #endregion


        public double X { get; }
        public double Y { get; }


        public Location2D(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public bool Equals(Location2D other)
        {
            bool output =
                this.X == other.X &&
                this.Y == other.Y;
            return output;
        }

        public override bool Equals(object obj)
        {
            bool output = false;
            if (obj is Location2D objAsLocation2D)
            {
                output = this.Equals(objAsLocation2D);
            }
            return output;
        }

        public override int GetHashCode()
        {
            int output = HashHelper.GetHashCode(this.X, this.Y);
            return output;
        }

        public override string ToString()
        {
            string output = $@"X: {this.X.ToString()}, Y: {this.Y.ToString()}";
            return output;
        }
    }
}
