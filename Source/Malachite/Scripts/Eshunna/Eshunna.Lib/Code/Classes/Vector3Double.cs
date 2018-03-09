using System;
using System.Collections.Generic;

using Public.Common.Lib;


namespace Eshunna.Lib
{
    [Serializable]
    public struct Vector3Double : IEquatable<Vector3Double>
    {
        #region Static

        public static bool operator ==(Vector3Double lhs, Vector3Double rhs)
        {
            bool output = lhs.Equals(rhs);
            return output;
        }

        public static bool operator !=(Vector3Double lhs, Vector3Double rhs)
        {
            bool output = !lhs.Equals(rhs);
            return output;
        }

        public static Vector3Double operator +(Vector3Double lhs, Vector3Double rhs)
        {
            var output = new Vector3Double(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);
            return output;
        }

        public static Vector3Double operator -(Vector3Double lhs, Vector3Double rhs)
        {
            var output = new Vector3Double(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);
            return output;
        }

        public static Vector3Double operator -(Vector3Double vector)
        {
            var output = new Vector3Double(-vector.X, -vector.Y, -vector.Z);
            return output;
        }

        public static List<Vector3Double> operator +(IEnumerable<Vector3Double> lhs, Vector3Double rhs)
        {
            var output = new List<Vector3Double>();
            foreach (var vector in lhs)
            {
                var sum = vector + rhs;
                output.Add(sum);
            }
            return output;
        }

        public static List<Vector3Double> operator -(IEnumerable<Vector3Double> lhs, Vector3Double rhs)
        {
            var output = new List<Vector3Double>();
            foreach (var vector in lhs)
            {
                var sum = vector - rhs;
                output.Add(sum);
            }
            return output;
        }

        public static Vector3Double CrossProduct(Vector3Double u, Vector3Double v)
        {
            double x = u.Y * v.Z - u.Z * v.Y;
            double y = u.Z * v.X - u.X * v.Z;
            double z = u.X * v.Y - u.Y * v.X;
            var output = new Vector3Double(x, y, z);
            return output;
        }

        public static double DotProduct(Vector3Double u, Vector3Double v)
        {
            double output = VectorDouble.DotProduct(u.ToArray(), v.ToArray());
            return output;
        }

        #endregion


        public double X { get; }
        public double Y { get; }
        public double Z { get; }


        public Vector3Double(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vector3Double(double[] values)
        {
            // The choice was made to not validate values array length.
            // Generally the array will be the correct length, in which case the cost of checking the array was wasted.
            // If the array is too short, a System.IndexOutOfRange exception will be raised making the caller aware of the problem.
            // If the array is too long, well, too bad!

            this.X = values[0];
            this.Y = values[1];
            this.Z = values[2];
        }

        public bool Equals(Vector3Double other)
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
            if (obj is Vector3Double objAsVector3Double)
            {
                output = this.Equals(objAsVector3Double);
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
            string output = $@"X: {this.X.ToString()}, Y: {this.Y.ToString()}, Z: {this.Z.ToString()} (double)";
            return output;
        }

        public double[] ToArray()
        {
            var output = new double[] { this.X, this.Y, this.Z };
            return output;
        }

        public double L2Norm()
        {
            double output = VectorDouble.L2Norm(this.ToArray());
            return output;
        }

        public Vector3Double L2Normalize()
        {
            var values = this.ToArray();
            var normalizedValues = VectorDouble.L2Normalize(values);
            var output = new Vector3Double(normalizedValues);
            return output;
        }

        public double Dot(Vector3Double v)
        {
            double output = VectorDouble.DotProduct(this.ToArray(), v.ToArray());
            return output;
        }

        public Vector3Double Cross(Vector3Double v)
        {
            var output = Vector3Double.CrossProduct(this, v);
            return output;
        }
    }
}
