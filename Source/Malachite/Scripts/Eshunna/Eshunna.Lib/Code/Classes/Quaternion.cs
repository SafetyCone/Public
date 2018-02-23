using System;

using Public.Common.Lib;


namespace Eshunna.Lib
{
    [Serializable]
    public struct Quaternion : IEquatable<Quaternion>
    {
        #region Static

        public static bool operator == (Quaternion lhs, Quaternion rhs)
        {
            bool output = lhs.Equals(rhs);
            return output;
        }

        public static bool operator != (Quaternion lhs, Quaternion rhs)
        {
            bool output = !lhs.Equals(rhs);
            return output;
        }
        
        #endregion


        public double W { get; }
        public double X { get; }
        public double Y { get; }
        public double Z { get; }


        public Quaternion(double w, double x, double y, double z)
        {
            this.W = w;
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public bool Equals(Quaternion other)
        {
            bool output =
                this.W == other.W &&
                this.X == other.X &&
                this.Y == other.Y &&
                this.Z == other.Z;
            return output;
        }

        public override bool Equals(object obj)
        {
            bool output = false;
            if (obj is Quaternion objAsQuaternion)
            {
                this.Equals(objAsQuaternion);
            }
            return output;
        }

        public override int GetHashCode()
        {
            int output = HashHelper.GetHashCode(this.W, this.X, this.Y, this.Z);
            return output;
        }

        public override string ToString()
        {
            string output = $@"W: {this.W.ToString()}, X: {this.X.ToString()}, Y: {this.Y.ToString()}, Z: {this.Z.ToString()}";
            return output;
        }
    }
}
