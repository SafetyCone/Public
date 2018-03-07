using System;

using Public.Common.Lib;


namespace Eshunna.Lib
{
    [Serializable]
    public struct Vector3Float : IEquatable<Vector3Float>
    {
        public const int NumberOfDimensions = 3;


        #region Static

        public static bool operator ==(Vector3Float lhs, Vector3Float rhs)
        {
            bool output = lhs.Equals(rhs);
            return output;
        }

        public static bool operator !=(Vector3Float lhs, Vector3Float rhs)
        {
            bool output = !lhs.Equals(rhs);
            return output;
        }

        public static Vector3Float operator +(Vector3Float lhs, Vector3Float rhs)
        {
            var output = new Vector3Float(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);
            return output;
        }

        public static Vector3Float operator -(Vector3Float lhs, Vector3Float rhs)
        {
            var output = new Vector3Float(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);
            return output;
        }

        public static Vector3Float CrossProduct(Vector3Float u, Vector3Float v)
        {
            float x = u.Y * v.Z - u.Z * v.Y;
            float y = u.Z * v.X - u.X * v.Z;
            float z = u.X * v.Y - u.Y * v.X;
            var output = new Vector3Float(x, y, z);
            return output;
        }

        #endregion


        public float X { get; }
        public float Y { get; }
        public float Z { get; }


        public Vector3Float(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vector3Float(float[] values)
        {
            // The choice was made to not validate values array length.
            // Generally the array will be the correct length, in which case the cost of checking the array was wasted.
            // If the array is too short, a System.IndexOutOfRange exception will be raised making the caller aware of the problem.
            // If the array is too long, well, too bad!

            this.X = values[0];
            this.Y = values[1];
            this.Z = values[2];
        }

        public bool Equals(Vector3Float other)
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
            if (obj is Vector3Float objAsVector3Float)
            {
                output = this.Equals(objAsVector3Float);
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
            string output = $@"X: {this.X.ToString()}, Y: {this.Y.ToString()}, Z: {this.Z.ToString()} (float)";
            return output;
        }

        public float[] ToArray()
        {
            var output = new float[] { this.X, this.Y, this.Z };
            return output;
        }

        public float L2Norm()
        {
            float output = VectorFloat.L2Norm(this.ToArray());
            return output;
        }

        public Vector3Float L2Normalize()
        {
            var values = this.ToArray();
            var normalizedValues = VectorFloat.L2Normalize(values);
            var output = new Vector3Float(normalizedValues);
            return output;
        }
    }
}
