using System;

using Public.Common.Lib;


namespace Eshunna.Lib
{
    [Serializable]
    public struct QuaternionFloat : IEquatable<QuaternionFloat>
    {
        #region Static

        public static bool operator ==(QuaternionFloat lhs, QuaternionFloat rhs)
        {
            bool output = lhs.Equals(rhs);
            return output;
        }

        public static bool operator !=(QuaternionFloat lhs, QuaternionFloat rhs)
        {
            bool output = !lhs.Equals(rhs);
            return output;
        }

        /// <summary>
        /// Implements n = q * r.
        /// </summary>
        public static QuaternionFloat operator *(QuaternionFloat q, QuaternionFloat r)
        {
            float w = r.W * q.W - r.X * q.X - r.Y * q.Y - r.Z * q.Z;
            float x = r.W * q.X + r.X * q.W - r.Y * q.Z + r.Z * q.Y;
            float y = r.W * q.Y + r.X * q.Z + r.Y * q.W - r.Z * q.X;
            float z = r.W * q.Z - r.X * q.Y + r.Y * q.X + r.Z * q.W;

            var output = new QuaternionFloat(w, x, y, z);
            return output;
        }

        /// <summary>
        /// Assumes a unit quaternion.
        public static MatrixFloat GetRotationMatrix(QuaternionFloat q)
        {
            float[] values = new float[]
            {
                1 - 2 * (q.Y * q.Y + q.Z * q.Z), 2 * (q.X * q.Y - q.Z * q.W), 2 * (q.X * q.Z + q.Y * q.W),
                2 * (q.X * q.Y + q.Z * q.W), 1 - 2 * (q.X * q.X + q.Z * q.Z), 2 * (q.Y * q.Z - q.X * q.W),
                2 * (q.X * q.Z - q.Y * q.W), 2 * (q.Y * q.Z + q.X * q.W), 1 - 2 * (q.X * q.X + q.Y * q.Y),
            };

            MatrixFloat output = new MatrixFloat(3, 3, values);
            return output;
        }

        #endregion


        public float W { get; }
        public float X { get; }
        public float Y { get; }
        public float Z { get; }


        public QuaternionFloat(float w, float x, float y, float z)
        {
            this.W = w;
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public bool Equals(QuaternionFloat other)
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
            if (obj is QuaternionFloat objAsQuaternion)
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
