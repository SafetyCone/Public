using System;

using Public.Common.Lib;


namespace Eshunna.Lib
{
    public struct RectangleDouble : IEquatable<RectangleDouble>
    {
        public double X { get; }
        public double Y { get; }
        public double Width { get; }
        public double Height { get; }


        public RectangleDouble(double x, double y, double width, double height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public bool Equals(RectangleDouble other)
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
            if (obj is RectangleDouble objAsRectangleInteger)
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
            string output = $@"X: {this.X.ToString()}, Y: {this.Y.ToString()}, Width: {this.Width.ToString()}, Height: {this.Height.ToString()} (double)";
            return output;
        }
    }
}
