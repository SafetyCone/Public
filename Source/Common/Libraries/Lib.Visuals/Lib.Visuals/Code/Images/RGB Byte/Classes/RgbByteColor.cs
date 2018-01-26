using System;


namespace Public.Common.Lib.Visuals
{
    /// <summary>
    /// Represents an RGB color.
    /// 
    /// As a properly implemented structure, this is immutable and implements IEquatable.
    /// </summary>
    public struct RgbByteColor : IEquatable<RgbByteColor>
    {
        public const byte MaxChannelValue = Byte.MaxValue;
        public const byte MinChannelValue = Byte.MinValue;


        #region Static

        public static RgbByteColor Black => new RgbByteColor(RgbByteColor.MinChannelValue, RgbByteColor.MinChannelValue, RgbByteColor.MinChannelValue);
        public static RgbByteColor White => new RgbByteColor(RgbByteColor.MaxChannelValue, RgbByteColor.MaxChannelValue, RgbByteColor.MaxChannelValue);
        public static RgbByteColor Red => new RgbByteColor(RgbByteColor.MaxChannelValue, RgbByteColor.MinChannelValue, RgbByteColor.MinChannelValue);
        public static RgbByteColor Green => new RgbByteColor(RgbByteColor.MinChannelValue, RgbByteColor.MaxChannelValue, RgbByteColor.MinChannelValue);
        public static RgbByteColor Blue => new RgbByteColor(RgbByteColor.MinChannelValue, RgbByteColor.MinChannelValue, RgbByteColor.MaxChannelValue);


        public static bool operator ==(RgbByteColor lhs, RgbByteColor rhs)
        {
            return lhs.Equals(rhs);
        }

        // If op_Equals is present, op_NotEquals must also be present, and vice-versa.
        public static bool operator !=(RgbByteColor lhs, RgbByteColor rhs)
        {
            return !(lhs.Equals(rhs));
        }

        #endregion


        public byte R { get; }
        public byte G { get; }
        public byte B { get; }


        public RgbByteColor(byte red, byte green, byte blue)
        {
            this.R = red;
            this.G = green;
            this.B = blue;
        }

        public RgbByteColor(int red, int green, int blue)
        {
            this.R = Convert.ToByte(red);
            this.G = Convert.ToByte(green);
            this.B = Convert.ToByte(blue);
        }

        public bool Equals(RgbByteColor other)
        {
            bool output =
                this.R == other.R &&
                this.G == other.G &&
                this.B == other.B;
            return output;
        }

        public override bool Equals(object obj)
        {
            // Note that because structs are implicitly sealed (no descendent classes) instead of using 'as' we can use 'is'.
            bool output = false;
            if (obj is RgbByteColor)
            {
                output = this.Equals((RgbByteColor)obj);
            }
            return output;
        }

        public override int GetHashCode()
        {
            return HashHelper.GetHashCode(this.R, this.G, this.B);
        }
    }
}
