using System;


namespace PythonFromNet
{
    public class Vector
    {
        #region Static

        private static Vector Iterate(Vector lhs, Vector rhs, Func<double, double, double> operation)
        {
            if (lhs.Count != rhs.Count)
            {
                string message = String.Format(@"Element number mismatch. Left-hand side: {0}, right-hand side: {1}.", lhs.Count, rhs.Count);
                throw new Exception(message);
            }

            double[] outputValues = new double[lhs.Count];
            for (int iValue = 0; iValue < lhs.Count; iValue++)
            {
                outputValues[iValue] = operation(lhs.Values[iValue], rhs.Values[iValue]);
            }

            Vector output = new Vector(outputValues);
            return output;
        }

        public static Vector operator +(Vector lhs, Vector rhs)
        {
            Vector output = Vector.Iterate(lhs, rhs, (x, y) => x + y);
            return output;
        }

        public static Vector operator -(Vector lhs, Vector rhs)
        {
            Vector output = Vector.Iterate(lhs, rhs, (x, y) => x - y);
            return output;
        }

        /// <summary>
        /// This is the dot product. Vector multiplication into a matrix is handled by the matrix class.
        /// </summary>
        public static Vector operator *(Vector lhs, Vector rhs)
        {
            Vector output = Vector.Iterate(lhs, rhs, (x, y) => x * y);
            return output;
        }

        public static Vector operator *(Vector lhs, double rhs)
        {
            Vector constant = new Vector(lhs.Count, rhs);

            Vector output = lhs * constant;
            return output;
        }

        #endregion


        public double[] Values { get; set; }
        public int Count
        {
            get
            {
                int output = this.Values.Length;
                return output;
            }
        }


        public Vector()
        {
        }

        public Vector(double[] values)
        {
            this.Values = values;
        }

        public Vector(int count)
        {
            this.Values = new double[count];
        }

        public Vector(int count, double defaultValue)
            : this(count)
        {
            for (int iElement = 0; iElement < this.Count; iElement++)
            {
                this.Values[iElement] = defaultValue;
            }
        }
    }
}
