using System;

using Public.Common.Lib;


namespace Eshunna.Lib
{
    [Serializable]
    public struct Location2Integer : IEquatable<Location2Integer>
    {
        #region Static

        public static bool operator ==(Location2Integer lhs, Location2Integer rhs)
        {
            bool output = lhs.Equals(rhs);
            return output;
        }

        public static bool operator !=(Location2Integer lhs, Location2Integer rhs)
        {
            bool output = !lhs.Equals(rhs);
            return output;
        }

        #endregion


        public int X { get; }
        public int Y { get; }


        public Location2Integer(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public bool Equals(Location2Integer other)
        {
            bool output =
                this.X == other.X &&
                this.Y == other.Y;
            return output;
        }

        public override bool Equals(object obj)
        {
            bool output = false;
            if (obj is Location2Integer objAsLocation2Integer)
            {
                output = this.Equals(objAsLocation2Integer);
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
            string output = $@"X: {this.X.ToString()}, Y: {this.Y.ToString()} (int)";
            return output;
        }
    }
}
