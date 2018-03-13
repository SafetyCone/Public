using System.Collections.Generic;


namespace Public.Common.Lib.Math
{
    public class RowMatrixLocationGenerator
    {
        public int Row { get; }


        public RowMatrixLocationGenerator(int row)
        {
            this.Row = row;
        }

        public IEnumerable<MatrixLocation> GetLocations(int startColumn, int endColumn)
        {
            for (int iColumn = startColumn; iColumn < endColumn; iColumn++)
            {
                yield return new MatrixLocation(this.Row, iColumn);
            }
        }

        public IEnumerable<MatrixLocation> GetLocations(int endColumn)
        {
            IEnumerable<MatrixLocation> output = this.GetLocations(0, endColumn);
            return output;
        }
    }
}
