using System;

using Public.Common.Lib;


namespace Eshunna.Lib
{
    [Serializable]
    public struct Vector4Double : IEquatable<Vector4Double>
    {
        #region Static

        public static bool operator ==(Vector4Double lhs, Vector4Double rhs)
        {
            bool output = lhs.Equals(rhs);
            return output;
        }

        public static bool operator !=(Vector4Double lhs, Vector4Double rhs)
        {
            bool output = !lhs.Equals(rhs);
            return output;
        }

        public static Vector4Double operator +(Vector4Double lhs, Vector4Double rhs)
        {
            var output = new Vector4Double(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z, lhs.W + rhs.W);
            return output;
        }

        public static Vector4Double operator -(Vector4Double lhs, Vector4Double rhs)
        {
            var output = new Vector4Double(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z, lhs.W - rhs.W);
            return output;
        }

        #endregion


        public double X { get; }
        public double Y { get; }
        public double Z { get; }
        public double W { get; }


        public Vector4Double(double x, double y, double z, double w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public Vector4Double(double[] values)
        {
            // The choice was made to not validate values array length.
            // Generally the array will be the correct length, in which case the cost of checking the array was wasted.
            // If the array is too short, a System.IndexOutOfRange exception will be raised making the caller aware of the problem.
            // If the array is too long, well, too bad!

            this.X = values[0];
            this.Y = values[1];
            this.Z = values[2];
            this.W = values[3];
        }

        public bool Equals(Vector4Double other)
        {
            bool output =
                this.X == other.X &&
                this.Y == other.Y &&
                this.Z == other.Z &&
                this.W == other.W;
            return output;
        }

        public override bool Equals(object obj)
        {
            bool output = false;
            if (obj is Vector4Double objAsVector4Float)
            {
                output = this.Equals(objAsVector4Float);
            }
            return output;
        }

        public override int GetHashCode()
        {
            int output = HashHelper.GetHashCode(this.X, this.Y, this.Z, this.W);
            return output;
        }

        public override string ToString()
        {
            string output = $@"X: {this.X.ToString()}, Y: {this.Y.ToString()}, Z: {this.Z.ToString()}, W: {this.W.ToString()} (double)";
            return output;
        }

        public double[] ToArray()
        {
            var output = new double[] { this.X, this.Y, this.Z, this.W };
            return output;
        }

        public double L2Norm()
        {
            double output = VectorDouble.L2Norm(this.ToArray());
            return output;
        }

        public Vector4Double L2Normalize()
        {
            var values = this.ToArray();
            var normalizedValues = VectorDouble.L2Normalize(values);
            var output = new Vector4Double(normalizedValues);
            return output;
        }
    }


    public static class Vector4DoubleExtensions
    {
        public static Location3HomogenousDouble ToLocation3HomogenousDouble(this Vector4Double value)
        {
            var output = new Location3HomogenousDouble(value.X, value.Y, value.Z, value.W);
            return output;
        }
    }
}
