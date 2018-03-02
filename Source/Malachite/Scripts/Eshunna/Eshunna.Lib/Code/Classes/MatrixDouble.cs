using System;

using Public.Common.Lib;


namespace Eshunna.Lib
{
    public class MatrixDouble : IEquatable<MatrixDouble>
    {
        #region Static

        public static bool operator ==(MatrixDouble lhs, MatrixDouble rhs)
        {
            bool output;
            if (object.ReferenceEquals(null, lhs))
            {
                output = object.ReferenceEquals(null, rhs);
            }
            else
            {
                output = lhs.Equals(rhs);
            }
            return output;
        }

        public static bool operator !=(MatrixDouble lhs, MatrixDouble rhs)
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


        public double[] RowMajorValues { get; set; }
        public int RowCount { get; set; }
        public int ColumnCount { get; set; }
        /// <summary>
        /// Zero-based row and column values.
        /// </summary>
        public double this[int row, int column]
        {
            get
            {
                int index = MatrixDouble.GetIndex(row, column, this.ColumnCount);

                double output = this.RowMajorValues[index];
                return output;
            }
            set
            {
                int index = MatrixDouble.GetIndex(row, column, this.ColumnCount);

                this.RowMajorValues[index] = value;
            }
        }


        public MatrixDouble(int rowCount, int columnCount, double[] rowMajorValues)
        {
            this.RowMajorValues = rowMajorValues;
            this.RowCount = rowCount;
            this.ColumnCount = columnCount;
        }

        public bool Equals(MatrixDouble other)
        {
            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (object.ReferenceEquals(null, other))
            {
                return false;
            }

            if (this.GetType() != other.GetType())
            {
                return false;
            }

            bool output = true;
            if (this.RowCount == other.RowCount && this.ColumnCount == other.ColumnCount)
            {
                for (int iRow = 0; iRow < this.RowCount; iRow++)
                {
                    for (int iCol = 0; iCol < this.ColumnCount; iCol++)
                    {
                        double thisValue = this[iRow, iCol];
                        double otherValue = other[iRow, iCol];
                        if (Double.IsNaN(thisValue))
                        {
                            if (!Double.IsNaN(otherValue))
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

                    if (!output)
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
            if (obj.GetType() == typeof(MatrixDouble))
            {
                output = this.Equals(obj as MatrixDouble);
            }
            return output;
        }

        public override int GetHashCode()
        {
            double firstValue = this.RowMajorValues.Length > 0 ? this.RowMajorValues[0] : 0;

            int output = HashHelper.GetHashCode(this.RowCount, this.ColumnCount, firstValue);
            return output;
        }
    }
}
