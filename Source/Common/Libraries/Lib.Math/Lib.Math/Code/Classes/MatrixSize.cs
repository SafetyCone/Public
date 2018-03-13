

namespace Public.Common.Lib.Math
{
    public struct MatrixSize
    {
        public int RowCount { get; }
        public int ColumnCount { get; }


        public MatrixSize(int rowCount, int columnCount)
        {
            this.RowCount = rowCount;
            this.ColumnCount = columnCount;
        }

        public MatrixSize(int[] dimensionSizes)
        {
            this.RowCount = dimensionSizes[0];
            this.ColumnCount = dimensionSizes[1];
        }

        public int[] ToArray()
        {
            int[] output = new int[] { this.RowCount, this.ColumnCount };
            return output;
        }
    }
}
