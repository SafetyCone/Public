using System;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;


using Public.Common.Lib;


namespace Eshunna.Lib
{
    public class MatrixFloat : IEquatable<MatrixFloat>
    {
        #region Static

        public static bool operator ==(MatrixFloat lhs, MatrixFloat rhs)
        {
            bool output;
            if(object.ReferenceEquals(null, lhs))
            {
                output = object.ReferenceEquals(null, rhs);
            }
            else
            {
                output = lhs.Equals(rhs);
            }
            return output;
        }

        public static bool operator !=(MatrixFloat lhs, MatrixFloat rhs)
        {
            bool output = !lhs.Equals(rhs);
            return output;
        }

        /// <summary>
        /// Assumes zero-based row and column.
        /// </summary>
        public static int GetIndex(int row, int column, int columnCount)
        {
            int output = (row) * columnCount + column;
            return output;
        }

        #endregion


        public float[] RowMajorValues { get; set; }
        public int RowCount { get; set; }
        public int ColumnCount { get; set; }
        /// <summary>
        /// Zero-based row and column values.
        /// </summary>
        public float this[int row, int column]
        {
            get
            {
                int index = MatrixFloat.GetIndex(row, column, this.ColumnCount);

                float output = this.RowMajorValues[index];
                return output;
            }
            set
            {
                int index = MatrixFloat.GetIndex(row, column, this.ColumnCount);

                this.RowMajorValues[index] = value;
            }
        }


        public MatrixFloat(int rowCount, int columnCount, float[] rowMajorValues)
        {
            this.RowMajorValues = rowMajorValues;
            this.RowCount = rowCount;
            this.ColumnCount = columnCount;
        }

        public bool Equals(MatrixFloat other)
        {
            if(object.ReferenceEquals(this, other))
            {
                return true;
            }

            if(object.ReferenceEquals(null, other))
            {
                return false;
            }

            if(this.GetType() != other.GetType())
            {
                return false;
            }

            bool output = true;
            if(this.RowCount == other.RowCount && this.ColumnCount == other.ColumnCount)
            {
                for (int iRow = 0; iRow < this.RowCount; iRow++)
                {
                    for (int iCol = 0; iCol < this.ColumnCount; iCol++)
                    {
                        float thisValue = this[iRow, iCol];
                        float otherValue = other[iRow, iCol];
                        if (Single.IsNaN(thisValue))
                        {
                            if(!Single.IsNaN(otherValue))
                            {
                                output = false;
                                break;
                            }
                        }
                        else
                        {
                            if (thisValue != otherValue)
                            {
                                output = false;
                                break;
                            }
                        }
                    }

                    if(!output)
                    {
                        break;
                    }
                }
            }
            else
            {
                output = false;
            }
            return output;
        }

        public override bool Equals(object obj)
        {
            bool output = false;
            if(obj.GetType() == typeof(MatrixFloat))
            {
                output = this.Equals(obj as MatrixFloat);
            }
            return output;
        }

        public override int GetHashCode()
        {
            float firstValue = this.RowMajorValues.Length > 0 ? this.RowMajorValues[0] : 0;

            int output = HashHelper.GetHashCode(this.RowCount, this.ColumnCount, firstValue);
            return output;
        }
    }


    public static class MatrixFloatExtensions
    {
        public static Matrix<float> ToMathNetMatrix(this MatrixFloat matrix)
        {
            var output = DenseMatrix.Build.DenseOfRowMajor(matrix.RowCount, matrix.ColumnCount, matrix.RowMajorValues);
            return output;
        }
    }
}
