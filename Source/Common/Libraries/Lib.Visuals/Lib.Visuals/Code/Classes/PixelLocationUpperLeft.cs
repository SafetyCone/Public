using System;


namespace Public.Common.Lib.Visuals
{
    /// <summary>
    /// The location of a pixel within an image relative to the origin in the upper left of the image.
    /// </summary>
    /// <remarks>
    /// * All X and Y image conventions <see cref="PixelLocation"/>.
    /// * By convention, the origin is at the upper-left of the image.
    /// </remarks>
    /// <cref></cref>
    public struct PixelLocationUpperLeft : IEquatable<PixelLocationUpperLeft>
    {
        #region Static

        public static bool operator ==(PixelLocationUpperLeft lhs, PixelLocationUpperLeft rhs)
        {
            return lhs.Equals(rhs);
        }

        // If op_Equals is present, op_NotEquals must also be present, and vice-versa.
        public static bool operator !=(PixelLocationUpperLeft lhs, PixelLocationUpperLeft rhs)
        {
            return !(lhs.Equals(rhs));
        }

        #endregion


        public int X { get; }
        public int Column => this.X;
        public int Y { get; }
        public int Row => this.Y;


        public PixelLocationUpperLeft(int x_column, int y_row)
        {
            this.X = x_column;
            this.Y = y_row;
        }

        public bool Equals(PixelLocationUpperLeft other)
        {
            bool output =
                this.X == other.X &&
                this.Y == other.Y;
            return output;
        }

        public override bool Equals(object obj)
        {
            bool output = false;
            if (obj is PixelLocationUpperLeft objAsPixelLocation)
            {
                output = this.Equals(objAsPixelLocation);
            }
            return output;
        }

        public override int GetHashCode()
        {
            return HashHelper.GetHashCode(this.X, this.Y);
        }

        public override string ToString()
        {
            string output = $@"X: {this.X.ToString()}, Y: {this.Y.ToString()} (int)";
            return output;
        }
    }
}
