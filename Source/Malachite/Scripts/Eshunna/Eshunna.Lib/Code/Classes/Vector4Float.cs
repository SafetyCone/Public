using System;

using Public.Common.Lib;


namespace Eshunna.Lib
{
    [Serializable]
    public struct Vector4Float : IEquatable<Vector4Float>
    {
        #region Static

        public static bool operator ==(Vector4Float lhs, Vector4Float rhs)
        {
            bool output = lhs.Equals(rhs);
            return output;
        }

        public static bool operator !=(Vector4Float lhs, Vector4Float rhs)
        {
            bool output = !lhs.Equals(rhs);
            return output;
        }

        public static Vector4Float operator +(Vector4Float lhs, Vector4Float rhs)
        {
            var output = new Vector4Float(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z, lhs.W + rhs.W);
            return output;
        }

        public static Vector4Float operator -(Vector4Float lhs, Vector4Float rhs)
        {
            var output = new Vector4Float(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z, lhs.W - rhs.W);
            return output;
        }

        #endregion


        public float X { get; }
        public float Y { get; }
        public float Z { get; }
        public float W { get; }


        public Vector4Float(float x, float y, float z, float w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public Vector4Float(float[] values)
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

        public bool Equals(Vector4Float other)
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
            if (obj is Vector4Float objAsVector4Float)
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
            string output = $@"X: {this.X.ToString()}, Y: {this.Y.ToString()}, Z: {this.Z.ToString()}, W: {this.W.ToString()} (float)";
            return output;
        }

        public float[] ToArray()
        {
            var output = new float[] { this.X, this.Y, this.Z, this.W };
            return output;
        }

        public float L2Norm()
        {
            float output = VectorFloat.L2Norm(this.ToArray());
            return output;
        }

        public Vector4Float L2Normalize()
        {
            var values = this.ToArray();
            var normalizedValues = VectorFloat.L2Normalize(values);
            var output = new Vector4Float(normalizedValues);
            return output;
        }
    }
}
