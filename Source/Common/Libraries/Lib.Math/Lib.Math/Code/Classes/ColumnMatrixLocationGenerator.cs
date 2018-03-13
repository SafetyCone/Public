using System.Collections.Generic;


namespace Public.Common.Lib.Math
{
    public class ColumnMatrixLocationGenerator
    {
        public int Column { get; }


        public ColumnMatrixLocationGenerator(int column)
        {
            this.Column = column;
        }

        public IEnumerable<MatrixLocation> GetLocations(int startRow, int endRow)
        {
            for (int iRow = startRow; iRow < endRow; iRow++)
            {
                yield return new MatrixLocation(iRow, this.Column);
            }
        }

        public IEnumerable<MatrixLocation> GetLocations(int endRow)
        {
            IEnumerable<MatrixLocation> output = this.GetLocations(0, endRow);
            return output;
        }
    }
}
