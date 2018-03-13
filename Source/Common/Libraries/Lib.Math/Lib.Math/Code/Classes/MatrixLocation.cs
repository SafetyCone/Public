

namespace Public.Common.Lib.Math
{
    public struct MatrixLocation
    {
        public int Row { get; }
        public int Column { get; }


        public MatrixLocation(int row, int column)
        {
            this.Row = row;
            this.Column = column;
        }
    }
}
