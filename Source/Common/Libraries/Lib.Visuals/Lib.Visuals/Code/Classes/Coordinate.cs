using System;


namespace Minex.Common.Lib.Visuals
{
    [Serializable]
    public class Coordinate
    {
        public int Row { get; set; }
        public int Column { get; set; }


        public Coordinate() { }

        public Coordinate(int row, int column)
        {
            this.Row = row;
            this.Column = column;
        }
    }
}
