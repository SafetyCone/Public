using System;

using Public.Common.Lib;


namespace Eshunna.Lib
{
    [Serializable]
    public struct Vector2Float : IEquatable<Vector2Float>
    {
        #region Static

        public static bool operator ==(Vector2Float lhs, Vector2Float rhs)
        {
            bool output = lhs.Equals(rhs);
            return output;
        }

        public static bool operator !=(Vector2Float lhs, Vector2Float rhs)
        {
            bool output = !lhs.Equals(rhs);
            return output;
        }

        public static Vector2Float operator +(Vector2Float lhs, Vector2Float rhs)
        {
            var output = new Vector2Float(lhs.X + rhs.X, lhs.Y + rhs.Y);
            return output;
        }

        public static Vector2Float operator -(Vector2Float lhs, Vector2Float rhs)
        {
            var output = new Vector2Float(lhs.X - rhs.X, lhs.Y - rhs.Y);
            return output;
        }

        #endregion


        public float X { get; }
        public float Y { get; }


        public Vector2Float(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public Vector2Float(float[] values)
        {
            // The choice was made to not validate values array length.
            // Generally the array will be the correct length, in which case the cost of checking the array was wasted.
            // If the array is too short, a System.IndexOutOfRange exception will be raised making the caller aware of the problem.
            // If the array is too long, well, too bad!

            this.X = values[0];
            this.Y = values[1];
        }

        public bool Equals(Vector2Float other)
        {
            bool output =
                this.X == other.X &&
                this.Y == other.Y;
            return output;
        }

        public override bool Equals(object obj)
        {
            bool output = false;
            if (obj is Vector2Float objAsVecctor2Float)
            {
                output = this.Equals(objAsVecctor2Float);
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

        public float[] ToArray()
        {
            var output = new float[] { this.X, this.Y };
            return output;
        }

        public float L2Norm()
        {
            float output = VectorFloat.L2Norm(this.ToArray());
            return output;
        }

        public Vector2Float L2Normalize()
        {
            var values = this.ToArray();
            var normalizedValues = VectorFloat.L2Normalize(values);
            var output = new Vector2Float(normalizedValues);
            return output;
        }
    }
}
