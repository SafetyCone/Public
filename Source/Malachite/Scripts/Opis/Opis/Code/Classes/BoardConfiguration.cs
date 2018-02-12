using Public.Common.Lib;


namespace Opis
{
    public struct BoardConfiguration
    {
        public string WorldUnits { get; }
        public MatrixSize BoardSize { get; }
        public double SquareSize { get; }


        public BoardConfiguration(string worldUnits, MatrixSize boardSize, double squareSize)
        {
            this.WorldUnits = worldUnits;
            this.BoardSize = boardSize;
            this.SquareSize = squareSize;
        }

        public BoardConfiguration(string worldUnits, int boardRows, int boardColumns, double squareSize)
            : this(worldUnits, new MatrixSize(boardRows, boardColumns), squareSize)
        {
        }
    }
}
