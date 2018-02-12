

namespace Public.Common.Lib.Visuals
{
    public struct ImageSize
    {
        public int Height { get; }
        public int Rows => this.Height;
        public int Y => this.Height;
        public int Width { get; }
        public int Columns => this.Width;
        public int X => this.Width;


        public ImageSize(int height_rows_y, int width_columns_x)
        {
            this.Height = height_rows_y;
            this.Width = width_columns_x;
        }

        public ImageSize(int[] dimensionSizes)
        {
            this.Height = dimensionSizes[0];
            this.Width = dimensionSizes[1];
        }
    }
}
