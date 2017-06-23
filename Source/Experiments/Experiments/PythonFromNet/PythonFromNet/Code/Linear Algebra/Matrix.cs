using System;
using System.Linq;


namespace PythonFromNet
{
    public class Matrix
    {
        #region Static

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

        #endregion


        public double[,] Values { get; set; }
        public int Rows
        {
            get
            {
                int output = this.Values.GetLength(0);
                return output;
            }
        }
        public int Columns
        {
            get
            {
                int output = this.Values.GetLength(1);
                return output;
            }
        }
        /// <summary>
        /// A two element array of sizes.
        /// </summary>
        public int[] Shape
        {
            get
            {
                int[] output = new int[]
                {
                    this.Rows,
                    this.Columns,
                };

                return output;
            }
        }


        public Matrix()
        {
        }

        public Matrix(double[,] values)
        {
            this.Values = values;
        }

        public Matrix(int rows, int columns)
        {
            this.Values = new double[rows, columns];
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

            double[] outputValues = new double[this.Rows];
            for (int iRow = 0; iRow < this.Rows; iRow++)
            {
                double outputValue = 0;
                for (int iCol = 0; iCol < this.Columns; iCol++)
                {
                    double curValue = this.Values[iRow, iCol] * vector.Values[iCol];
                    outputValue += curValue;
                }

                outputValues[iRow] = outputValue;
            }

            Vector output = new Vector(outputValues);
            return output;
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
