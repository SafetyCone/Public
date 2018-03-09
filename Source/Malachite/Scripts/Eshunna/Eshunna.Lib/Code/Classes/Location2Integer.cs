using System;
using System.Collections.Generic;

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

    public static class Location2IntegerExtensions
    {
        public static BoundingBoxInteger GetBoundingBox(this IEnumerable<Location2Integer> locations)
        {
            int xMin = Int32.MaxValue;
            int xMax = Int32.MinValue;
            int yMin = Int32.MaxValue;
            int yMax = Int32.MinValue;

            foreach (var location in locations)
            {
                if (location.X < xMin)
                {
                    xMin = location.X;
                }

                if (location.X > xMax)
                {
                    xMax = location.X;
                }

                if (location.Y < yMin)
                {
                    yMin = location.Y;
                }

                if (location.Y > yMax)
                {
                    yMax = location.Y;
                }
            }

            var output = new BoundingBoxInteger(xMin, xMax, yMin, yMax);
            return output;
        }
    }
}
