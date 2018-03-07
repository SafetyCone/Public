using System;

using Public.Common.Lib;


namespace Eshunna.Lib
{
    [Serializable]
    public struct Location2Float : IEquatable<Location2Float>
    {
        #region Static

        public static bool operator ==(Location2Float lhs, Location2Float rhs)
        {
            bool output = lhs.Equals(rhs);
            return output;
        }

        public static bool operator !=(Location2Float lhs, Location2Float rhs)
        {
            bool output = !lhs.Equals(rhs);
            return output;
        }

        #endregion


        public float X { get; }
        public float Y { get; }


        public Location2Float(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public bool Equals(Location2Float other)
        {
            bool output =
                this.X == other.X &&
                this.Y == other.Y;
            return output;
        }

        public override bool Equals(object obj)
        {
            bool output = false;
            if (obj is Location2Float objAsLocation2Float)
            {
                output = this.Equals(objAsLocation2Float);
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
            string output = $@"X: {this.X.ToString()}, Y: {this.Y.ToString()} (float)";
            return output;
        }
    }
}
