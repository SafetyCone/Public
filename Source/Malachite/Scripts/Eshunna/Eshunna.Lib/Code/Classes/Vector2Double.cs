using System;

using Public.Common.Lib;


namespace Eshunna.Lib
{
    [Serializable]
    public struct Vector2Double : IEquatable<Vector2Double>
    {
        #region Static

        public static bool operator ==(Vector2Double lhs, Vector2Double rhs)
        {
            bool output = lhs.Equals(rhs);
            return output;
        }

        public static bool operator !=(Vector2Double lhs, Vector2Double rhs)
        {
            bool output = !lhs.Equals(rhs);
            return output;
        }

        public static Vector2Double operator +(Vector2Double lhs, Vector2Double rhs)
        {
            var output = new Vector2Double(lhs.X + rhs.X, lhs.Y + rhs.Y);
            return output;
        }

        public static Vector2Double operator -(Vector2Double lhs, Vector2Double rhs)
        {
            var output = new Vector2Double(lhs.X - rhs.X, lhs.Y - rhs.Y);
            return output;
        }

        #endregion


        public double X { get; }
        public double Y { get; }


        public Vector2Double(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public Vector2Double(double[] values)
        {
            // The choice was made to not validate values array length.
            // Generally the array will be the correct length, in which case the cost of checking the array was wasted.
            // If the array is too short, a System.IndexOutOfRange exception will be raised making the caller aware of the problem.
            // If the array is too long, well, too bad!

            this.X = values[0];
            this.Y = values[1];
        }

        public bool Equals(Vector2Double other)
        {
            bool output =
                this.X == other.X &&
                this.Y == other.Y;
            return output;
        }

        public override bool Equals(object obj)
        {
            bool output = false;
            if (obj is Vector2Double objAsVecctor2Double)
            {
                output = this.Equals(objAsVecctor2Double);
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

        public double[] ToArray()
        {
            var output = new double[] { this.X, this.Y };
            return output;
        }

        public double L2Norm()
        {
            double output = VectorDouble.L2Norm(this.ToArray());
            return output;
        }

        public Vector2Double L2Normalize()
        {
            var values = this.ToArray();
            var normalizedValues = VectorDouble.L2Normalize(values);
            var output = new Vector2Double(normalizedValues);
            return output;
        }
    }
}
