using System;
using System.Collections.Generic;

using Public.Common.Lib;

using Eshunna.Lib.Logging;


namespace Eshunna.Lib.Comparers
{
    public class Array2DEqualityComparer<T> : IEqualityComparer<T[,]>
    {
        public IEqualityComparer<T> ValueComparer { get; }
        public ILog Log { get; }


        public Array2DEqualityComparer(IEqualityComparer<T> valueComparer, ILog log)
        {
            this.ValueComparer = valueComparer;
            this.Log = log;
        }

        public bool Equals(T[,] x, T[,] y)
        {
            bool output = true;

            int rowsX = x.GetLength(0);
            int colsX = x.GetLength(1);
            int rowsY = y.GetLength(0);
            int colsY = y.GetLength(1);

            bool sizeEquals = (rowsX == rowsY) && (colsX == colsY);
            if(!sizeEquals)
            {
                output = false;

                string message = $@"Size mismatch: x: ({rowsX.ToString()}, {colsX.ToString()}), y: ({rowsY.ToString()}, {colsY.ToString()})";
                this.Log.WriteLine(message);
            }
            else
            {
                for (int iRow = 0; iRow < rowsX; iRow++)
                {
                    for (int iCol = 0; iCol < colsX; iCol++)
                    {
                        T valueX = x[iRow, iCol];
                        T valueY = y[iRow, iCol];

                        bool valuesEqual = this.ValueComparer.Equals(valueX, valueY);
                        if(!valuesEqual)
                        {
                            output = false;

                            string message = $@"Value mismatch: location: ({iRow.ToString()}, {iCol.ToString()}), x: {valueX.ToString()}, y: {y.ToString()}";
                            this.Log.WriteLine(message);
                        }
                    }
                }
            }

            return output;
        }

        public int GetHashCode(T[,] obj)
        {
            int rows = obj.GetLength(0);
            int cols = obj.GetLength(0);

            T firstValue = rows > 0 && cols > 0 ? obj[0, 0] : default(T);

            int output = HashHelper.GetHashCode(rows, cols, firstValue);
            return output;
        }
    }
}
