using System;

using Public.Common.Lib.Math.Extensions;


namespace Public.Common.Lib.Math
{
    [Serializable]
    public class Vector
    {
        #region Static

        public static void IterateSetValue(Vector v, Func<double> operation)
        {
            int numElements = v.Count;
            for (int iElement = 0; iElement < numElements; iElement++)
            {
                v.Values[iElement] = operation();
            }
        }

        public static void ZeroOut(Vector v)
        {
            Vector.IterateSetValue(v, () => 0);
        }

        public static void IterateInPlace(Vector input, Vector output, Func<double, double> operation)
        {
            int numElements = input.Count;
            for (int iElement = 0; iElement < numElements; iElement++)
            {
                double inputValue = input.Values[iElement];

                double outputValue = operation(inputValue);
                output.Values[iElement] = outputValue;
            }
        }

        public static void Copy(Vector input, Vector output)
        {
            Vector.IterateInPlace(input, output, (x) => x);
        }

        public static void MultiplyByConstantInPlace(Vector input, Vector output, double multiplier)
        {
            Vector.IterateInPlace(input, output, (x) => x * multiplier);
        }

        /// <remarks>
        /// Input vector count match not performed.
        /// </remarks>
        public static void IterateInPlace(Vector lhs, Vector rhs, Vector output, Func<double, double, double> operation)
        {
            int numElements = lhs.Count;
            for (int iElement = 0; iElement < numElements; iElement++)
            {
                double lhsInputValue = lhs.Values[iElement];
                double rhsInputValue = rhs.Values[iElement];

                double outputValue = operation(lhsInputValue, rhsInputValue);
                output.Values[iElement] = outputValue;
            }
        }

        public static void AddInPlace(Vector lhs, Vector rhs, Vector output)
        {
            Vector.IterateInPlace(lhs, rhs, output, (x, y) => x + y);
        }

        public static void SubtractInPlace(Vector lhs, Vector rhs, Vector output)
        {
            Vector.IterateInPlace(lhs, rhs, output, (x, y) => x - y);
        }

        public static void MultiplyInPlace(Vector lhs, Vector rhs, Vector output)
        {
            Vector.IterateInPlace(lhs, rhs, output, (x, y) => x * y);
        }

        public static Vector Iterate(Vector lhs, Vector rhs, Func<double, double, double> operation)
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

        public static Vector GetRandomVector(int count, Random random)
        {
            double[] values = new double[count];
            for (int iIndex = 0; iIndex < count; iIndex++)
            {
                values[iIndex] = random.NextGaussian();
            }

            Vector output = new Vector(values);
            return output;
        }

        public static Vector GetRandomVector(int count)
        {
            Vector output = Vector.GetRandomVector(count, Utilities.SingletonRandom);
            return output;
        }

        #endregion


        public readonly double[] Values;
        public int Count { get; protected set; }
        public double this[int index]
        {
            get
            {
                double output = this.Values[index];
                return output;
            }
            set
            {
                this.Values[index] = value;
            }
        }


        public Vector(double[] values)
        {
            this.Values = values;

            this.SetMetaData();
        }

        private void SetMetaData()
        {
            this.Count = this.Values.Length;
        }

        public Vector(int count)
        {
            this.Values = new double[count];

            this.SetMetaData();
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
