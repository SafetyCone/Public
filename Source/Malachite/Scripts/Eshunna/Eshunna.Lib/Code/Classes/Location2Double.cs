using System;
using System.Collections.Generic;

using Public.Common.Lib;


namespace Eshunna.Lib
{
    [Serializable]
    public struct Location2Double : IEquatable<Location2Double>
    {
        #region Static

        public static bool operator==(Location2Double lhs, Location2Double rhs)
        {
            bool output = lhs.Equals(rhs);
            return output;
        }

        public static bool operator !=(Location2Double lhs, Location2Double rhs)
        {
            bool output = !lhs.Equals(rhs);
            return output;
        }

        #endregion


        public double X { get; }
        public double Y { get; }


        public Location2Double(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public bool Equals(Location2Double other)
        {
            bool output =
                this.X == other.X &&
                this.Y == other.Y;
            return output;
        }

        public override bool Equals(object obj)
        {
            bool output = false;
            if (obj is Location2Double objAsLocation2Double)
            {
                output = this.Equals(objAsLocation2Double);
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
            string output = $@"X: {this.X.ToString()}, Y: {this.Y.ToString()} (double)";
            return output;
        }
    }


    public static class Location2DoubleExtensions
    {
        public static Location2Integer Round(this Location2Double location)
        {
            var output = location.ToLocation2Integer();
            return output;
        }

        public static List<Location2Integer> Round(this IEnumerable<Location2Double> locations)
        {
            var output = new List<Location2Integer>();
            foreach (var location in locations)
            {
                var roundedLocation = location.ToLocation2Integer();
                output.Add(roundedLocation);
            }
            return output;
        }

        public static Location2Integer ToLocation2Integer(this Location2Double location)
        {
            int x = Convert.ToInt32(Math.Round(location.X));
            int y = Convert.ToInt32(Math.Round(location.Y));

            var output = new Location2Integer(x, y);
            return output;
        }
    }
}
