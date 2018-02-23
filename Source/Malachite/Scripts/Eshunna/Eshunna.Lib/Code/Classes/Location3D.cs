﻿using System;

using Public.Common.Lib;


namespace Eshunna.Lib
{
    [Serializable]
    public struct Location3D : IEquatable<Location3D>
    {
        #region Static

        public static bool operator ==(Location3D lhs, Location3D rhs)
        {
            bool output = lhs.Equals(rhs);
            return output;
        }

        public static bool operator !=(Location3D lhs, Location3D rhs)
        {
            bool output = !lhs.Equals(rhs);
            return output;
        }

        #endregion


        public double X { get; }
        public double Y { get; }
        public double Z { get; }


        public Location3D(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public bool Equals(Location3D other)
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
            if (obj is Location3D objAsLocation3D)
            {
                output = this.Equals(objAsLocation3D);
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
            string output = $@"X: {this.X.ToString()}, Y: {this.Y.ToString()}, Z: {this.Z.ToString()}";
            return output;
        }
    }
}
