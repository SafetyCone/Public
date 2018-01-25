using System;


namespace Public.Common.Lib.Visuals
{
    public struct PixelLocation : IEquatable<PixelLocation>
    {
        #region Static

        public static bool operator ==(PixelLocation lhs, PixelLocation rhs)
        {
            return lhs.Equals(rhs);
        }

        // If op_Equals is present, op_NotEquals must also be present, and vice-versa.
        public static bool operator !=(PixelLocation lhs, PixelLocation rhs)
        {
            return !(lhs.Equals(rhs));
        }

        #endregion


        public int X => this.Column;
        public int Column { get; private set; }
        public int Y => this.Row;
        public int Row { get; private set; }


        public PixelLocation(int row_y, int column_x)
        {
            this.Row = row_y;
            this.Column = column_x;
        }

        public bool Equals(PixelLocation other)
        {
            bool output =
                this.Row == other.Row &&
                this.Column == other.Column;
            return output;
        }

        public override bool Equals(object obj)
        {
            // Note that because structs are implicitly sealed (no descendent classes) instead of using 'as' we can use 'is'.
            bool output = false;
            if (obj is PixelLocation)
            {
                output = this.Equals((PixelLocation)obj);
            }
            return output;
        }

        public override int GetHashCode()
        {
            return HashHelper.GetHashCode(this.Row, this.Column);
        }
    }
}
