using System;

using Public.Common.Lib;


namespace Eshunna.Lib
{
    public struct RectangleInteger : IEquatable<RectangleInteger>
    {
        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }


        public RectangleInteger(int x, int y, int width, int height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public bool Equals(RectangleInteger other)
        {
            bool output =
                this.X == other.X &&
                this.Y == other.Y &&
                this.Width == other.Width &&
                this.Height == other.Height;
            return output;
        }

        public override bool Equals(object obj)
        {
            bool output = false;
            if (obj is RectangleInteger objAsRectangleInteger)
            {
                output = this.Equals(objAsRectangleInteger);
            }
            return output;
        }

        public override int GetHashCode()
        {
            int output = HashHelper.GetHashCode(this.X, this.Y, this.Width, this.Height);
            return output;
        }

        public override string ToString()
        {
            string output = $@"X: {this.X.ToString()}, Y: {this.Y.ToString()}, Width: {this.Width.ToString()}, Height: {this.Height.ToString()} (int)";
            return output;
        }
    }
}
