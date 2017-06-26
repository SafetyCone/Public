using System;
using System.Linq;


namespace PythonFromNet.Lib
{
    [Serializable]
    public class Matrix
    {
        #region Static

        public static void IterateSetValue(Matrix m, Func<double> operation)
        {
            int numRows = m.Rows;
            int numCols = m.Columns;
            for (int iRow = 0; iRow < numRows; iRow++)
            {
                for (int iCol = 0; iCol < numCols; iCol++)
                {
                    m.Values[iRow, iCol] = operation();
                }
            }
        }

        public static void ZeroOut(Matrix m)
        {
            Matrix.IterateSetValue(m, () => 0);
        }

        public static void IterateInPlace(Matrix input, Matrix output, Func<double, double> operation)
        {
            int numRows = input.Rows;
            int numCols = input.Columns;
            for (int iRow = 0; iRow < numRows; iRow++)
            {
                for (int iCol = 0; iCol < numCols; iCol++)
                {
                    double inputValue = input.Values[iRow, iCol];

                    double outputValue = operation(inputValue);
                    output.Values[iRow, iCol] = outputValue;
                }
            }
        }

        public static void Copy(Matrix input, Matrix output)
        {
            Matrix.IterateInPlace(input, output, (x) => x);
        }

        public static void MultiplyByConstantInPlace(Matrix input, Matrix output, double multiplier)
        {
            Matrix.IterateInPlace(input, output, (x) => x * multiplier);
        }

        public static void IterateInPlace(Matrix lhs, Matrix rhs, Matrix output, Func<double, double, double> operation)
        {
            int numRows = lhs.Rows;
            int numCols = lhs.Columns;
            for (int iRow = 0; iRow < numRows; iRow++)
            {
                for (int iCol = 0; iCol < numCols; iCol++)
                {
                    double lhsInputValue = lhs.Values[iRow, iCol];
                    double rhsInputValue = rhs.Values[iRow, iCol];

                    double outputValue = operation(lhsInputValue, rhsInputValue);
                    output.Values[iRow, iCol] = outputValue;
                }
            }
        }

        public static void AddInPlace(Matrix lhs, Matrix rhs, Matrix output)
        {
            Matrix.IterateInPlace(lhs, rhs, output, (x, y) => x + y);
        }

        public static void SubtractInPlace(Matrix lhs, Matrix rhs, Matrix output)
        {
            Matrix.IterateInPlace(lhs, rhs, output, (x, y) => x - y);
        }

        public static void MultiplyInPlace(Matrix lhs, Matrix rhs, Matrix output)
        {
            Matrix.IterateInPlace(lhs, rhs, output, (x, y) => x * y);
        }

        public static void VectorOuterMultiplyInPlace(Vector x, Vector y, Matrix output)
        {
            int xCount = x.Count;
            int yCount = y.Count;

            for (int iXElement = 0; iXElement < xCount; iXElement++)
            {
                for (int iYElement = 0; iYElement < yCount; iYElement++)
                {
                    output.Values[iXElement, iYElement] = x.Values[iXElement] * y.Values[iYElement];
                }
            }
        }

        /// <summary>
        /// Iterates over the elements of a matrix.
        /// </summary>
        /// <param name="outputObjectGenerator">Function determining whether a new matrix object is returned, or whether the LHS matrix is modified and returned.</param>
        private static Matrix Iterate(Matrix lhs, Matrix rhs, Func<double, double, double> operation, Func<Matrix, Matrix> outputObjectGenerator)
        {
            int[] lhsShape = lhs.Shape;
            if (!Enumerable.SequenceEqual(lhsShape, rhs.Shape))
            {
                string message = String.Format(@"Matrix shap mismatch. Left-hand side: {0}x{1}, right-hand side: {2}x{3}.", lhs.Rows, lhs.Columns, rhs.Rows, rhs.Columns);
                throw new ArgumentException(message);
            }

            Matrix output = outputObjectGenerator(lhs);
            for (int iRow = 0; iRow < lhsShape[0]; iRow++)
            {
                for (int iColumn = 0; iColumn < lhsShape[1]; iColumn++)
                {
                    output.Values[iRow, iColumn] = operation(lhs.Values[iRow, iColumn], rhs.Values[iRow, iColumn]);
                }
            }

            return output;
        }

        /// <remarks>
        /// Creates a new matrix.
        /// </remarks>
        private static Matrix Iterate(Matrix lhs, Matrix rhs, Func<double, double, double> operation)
        {
            Matrix output = Matrix.Iterate(lhs, rhs, operation, (x) => new Matrix(x.Shape));
            return output;
        }

        public static Matrix operator +(Matrix lhs, Matrix rhs)
        {
            Matrix output = Matrix.Iterate(lhs, rhs, (x, y) => x + y);
            return output;
        }

        public static Matrix operator -(Matrix lhs, Matrix rhs)
        {
            Matrix output = Matrix.Iterate(lhs, rhs, (x, y) => x - y);
            return output;
        }

        public static Matrix operator *(Matrix lhs, double rhs)
        {
            Matrix constant = new Matrix(lhs.Shape, rhs);

            Matrix output = Matrix.Iterate(lhs, constant, (x, y) => x * y);
            return output;
        }

        /// <summary>
        /// Multiplying a 10x1 vector by a 1x30 vector results in a 10x30 matrix. This method produces that result.
        /// </summary>
        public static Matrix VectorOuterMultiply(Vector x, Vector y)
        {
            int xCount = x.Count;
            int yCount = y.Count;

            Matrix output = new Matrix(xCount, yCount);
            for (int iXElement = 0; iXElement < xCount; iXElement++)
            {
                for (int iYElement = 0; iYElement < yCount; iYElement++)
                {
                    output.Values[iXElement, iYElement] = x.Values[iXElement] * y.Values[iYElement];
                }
            }

            return output;
        }

        public static Matrix GetRandomMatrix(int rows, int columns, Random random)
        {
            double[,] values = new double[rows, columns];
            for (int iRow = 0; iRow < rows; iRow++)
            {
                for (int iCol = 0; iCol < columns; iCol++)
                {
                    values[iRow, iCol] = (float)random.NextGaussian();
                }
            }

            Matrix output = new Matrix(values);
            return output;
        }

        public static Matrix GetRandomMatrix(int rows, int columns)
        {
            Matrix output = Matrix.GetRandomMatrix(rows, columns, Utilities.SingletonRandom);
            return output;
        }

        #endregion


        //public double[,] Values { get; protected set; }
        public double[,] Values;
        public int Rows { get; protected set; }
        public int Columns { get; protected set; }
        /// <summary>
        /// A two element array of sizes.
        /// </summary>
        public int[] Shape { get; protected set; }


        public Matrix(double[,] values)
        {
            this.Values = values;

            this.SetMetaData();
        }

        private void SetMetaData()
        {
            this.Rows = this.Values.GetLength(0);
            this.Columns = this.Values.GetLength(1);
            this.Shape = new int[]
            {
                this.Rows,
                this.Columns
            };
        }

        public Matrix(int rows, int columns)
        {
            this.Values = new double[rows, columns];

            this.SetMetaData();
        }

        public Matrix(int[] shape)
            : this(shape[0], shape[1])
        {
        }

        public Matrix(int[] shape, double defaultValue)
            : this(shape)
        {
            for (int iRow = 0; iRow < this.Rows; iRow++)
            {
                for (int iCol = 0; iCol < this.Columns; iCol++)
                {
                    this.Values[iRow, iCol] = defaultValue;
                }
            }
        }

        /// <summary>
        /// A dot-product with the input vector on the right, i.e. M dot V.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public Vector DotProductFromRight(Vector vector)
        {
            if(this.Columns != vector.Count)
            {
                string message = String.Format(@"Dimension mismatch. Matrix is RxC {0)x{1}, vector is {2}x1.", this.Rows, this.Columns, vector.Count);
                throw new ArgumentException(message);
            }

            Vector output = new Vector(this.Rows);

            this.DotProductFromRightInPlace(vector, output);

            return output;
        }

        public void DotProductFromRightInPlace(Vector rightVector, Vector output)
        {
            for (int iRow = 0; iRow < this.Rows; iRow++)
            {
                double outputValue = 0;
                for (int iCol = 0; iCol < this.Columns; iCol++)
                {
                    double curValue = this.Values[iRow, iCol] * rightVector.Values[iCol];
                    outputValue += curValue;
                }

                output.Values[iRow] = outputValue;
            }
        }

        public void DotProductFromLeftInPlace(Vector leftVector, Vector output)
        {
            for (int iCol = 0; iCol < this.Columns; iCol++)
            {
                double outputValue = 0;
                for (int iRow = 0; iRow < this.Rows; iRow++)
                {
                    double curValue = this.Values[iRow, iCol] * leftVector.Values[iRow];
                    outputValue += curValue;
                }

                output.Values[iCol] = outputValue;
            }
        }

        public Matrix Transpose()
        {
            int[] thisShape = this.Shape;

            int[] transposeShape = new int[2]
            {
                thisShape[1],
                thisShape[0],
            };

            Matrix output = new Matrix(transposeShape);
            for (int iRow = 0; iRow < this.Rows; iRow++)
            {
                for (int iColumn = 0; iColumn < this.Columns; iColumn++)
                {
                    output.Values[iColumn, iRow] = this.Values[iRow, iColumn];
                }
            }

            return output;
        }
    }
}
