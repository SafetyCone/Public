using System;

using Public.Common.Lib;


namespace Eshunna.Lib
{
    [Serializable]
    public struct QuaternionDouble : IEquatable<QuaternionDouble>
    {
        #region Static

        public static bool operator == (QuaternionDouble lhs, QuaternionDouble rhs)
        {
            bool output = lhs.Equals(rhs);
            return output;
        }

        public static bool operator != (QuaternionDouble lhs, QuaternionDouble rhs)
        {
            bool output = !lhs.Equals(rhs);
            return output;
        }

        /// <summary>
        /// Implements n = q * r.
        /// </summary>
        public static QuaternionDouble operator *(QuaternionDouble q, QuaternionDouble r)
        {
            double w = r.W * q.W - r.X * q.X - r.Y * q.Y - r.Z * q.Z;
            double x = r.W * q.X + r.X * q.W - r.Y * q.Z + r.Z * q.Y;
            double y = r.W * q.Y + r.X * q.Z + r.Y * q.W - r.Z * q.X;
            double z = r.W * q.Z - r.X * q.Y + r.Y * q.X + r.Z * q.W;

            var output = new QuaternionDouble(w, x, y, z);
            return output;
        }

        /// <summary>
        /// Assumes a unit quaternion.
        public static MatrixDouble GetRotationMatrix(QuaternionDouble q)
        {
            double[] values = new double[]
            {
                1 - 2 * (q.Y * q.Y + q.Z * q.Z), 2 * (q.X * q.Y - q.Z * q.W), 2 * (q.X * q.Z + q.Y * q.W),
                2 * (q.X * q.Y + q.Z * q.W), 1 - 2 * (q.X * q.X + q.Z * q.Z), 2 * (q.Y * q.Z - q.X * q.W),
                2 * (q.X * q.Z - q.Y * q.W), 2 * (q.Y * q.Z + q.X * q.W), 1 - 2 * (q.X * q.X + q.Y * q.Y),
            };

            MatrixDouble output = new MatrixDouble(3, 3, values);
            return output;
        }

        public static QuaternionDouble GetQuaternion(MatrixDouble rotationMatrix)
        {
            // https://en.wikipedia.org/wiki/Rotation_matrix#Quaternion

            // Construct a 4 x 4 symmetric matrix, and find the eigenvector with the largest eigenvalue. Measure eigenvalue difference from 1 (which would be a pure rotation).

            throw new NotImplementedException();
        }
        
        #endregion


        public double W { get; }
        public double X { get; }
        public double Y { get; }
        public double Z { get; }


        public QuaternionDouble(double w, double x, double y, double z)
        {
            this.W = w;
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public QuaternionDouble(double[] values)
        {
            this.W = values[0];
            this.X = values[1];
            this.Y = values[2];
            this.Z = values[3];
        }

        public bool Equals(QuaternionDouble other)
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
            if (obj is QuaternionDouble objAsQuaternion)
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

        public double[] ToArray()
        {
            var output = new double[] {this.W, this.X, this.Y, this.Z };
            return output;
        }

        public double L2Norm()
        {
            double output = VectorDouble.L2Norm(this.ToArray());
            return output;
        }

        public QuaternionDouble L2Normalize()
        {
            var values = this.ToArray();
            var normalizedValues = VectorDouble.L2Normalize(values);
            var output = new QuaternionDouble(normalizedValues);
            return output;
        }
    }
}
